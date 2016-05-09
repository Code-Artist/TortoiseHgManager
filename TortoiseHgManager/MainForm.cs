//#define SIMULATION

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace TortoiseHgManager
{
    internal enum RepoStatus
    {
        Default,
        Executing,
        Completed,
        ERROR
    }

    public partial class MainForm : Form
    {
#if SIMULATION
        private Random rand;
#endif
        #region [ Private Fields ]

        private bool UserAborted;
        private int ThreadCount;
        private TortoiseHgClient hg = new TortoiseHgClient();

        private delegate void HgFunction();
        private Dictionary<string, HgItem> HgFunctions;
        private class HgItem
        {
            public HgFunction Function { get; set; }
            public bool MultiThreading { get; set; }
            public bool PromptWarning { get; set; } = false;
        }

        #endregion

        public MainForm()
        {
            InitializeComponent();
            diagnosticsTextBox1.Clear();
            lbCounter.Text = string.Empty;
            lbErrorCounts.Text = string.Empty;

            cbThreads.Items.Clear();
            for (int x = 0; x < GetNumberOfCores(); x++)
                cbThreads.Items.Add(x + 1);
            cbThreads.SelectedIndex = 0;

            RegisterFunctions();
            cbFunctions.Items.AddRange(HgFunctions.Keys.ToArray());
            cbFunctions.SelectedIndex = 0;
            LoadRepositories();
#if SIMULATION
            Text += " ~~~ SIMULATION ~~~";
            rand = new Random();
#endif
        }

        #region [ Private Functions ]

        private void LoadRepositories()
        {
            hg.LoadRepositories();
            lstRepos.Nodes.Clear();
            TreeNode rootNode, childNode;
            foreach (TortoiseHgRepository repository in hg.Repositories)
            {
                //Group
                string group = repository.Group;
                if (!lstRepos.Nodes.ContainsKey(group))
                {
                    rootNode = new TreeNode(group) { Name = group };
                    lstRepos.Nodes.Add(rootNode);
                }
                else
                    rootNode = lstRepos.Nodes[group];

                //Child
                childNode = new TreeNode(repository.ShortName) { Name = repository.ShortName };
                childNode.Tag = repository;
                childNode.Checked = repository.Selected;
                rootNode.Nodes.Add(childNode);
            }
            lstRepos.ExpandAll();
        }

        private void lstRepos_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TortoiseHgRepository repo = e.Node.Tag as TortoiseHgRepository;
            if (repo != null) repo.Selected = e.Node.Checked;
        }

        private void lstRepos_AfterSelect(object sender, TreeViewEventArgs e)
        {
            repoProperties.SelectedObject = e.Node.Tag;
        }

        private static Process ExecuteCommand(string command, string repositoryPath, bool waitForExit)
        {
            Process p = new Process();
            p.StartInfo.FileName = "C:\\Program Files\\TortoiseHg\\hg.exe";
            p.StartInfo.Arguments = command;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(repositoryPath);
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            if (waitForExit) p.WaitForExit();
            return p;
        }
        private delegate void UpdateDelegate(TortoiseHgRepository repository, RepoStatus status);
        private void UpdateStatus(TortoiseHgRepository repository, RepoStatus status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateDelegate(UpdateStatus), new object[] { repository, status });
                return;
            }
            SetNodeStatus(repository, status);
            if ((status == RepoStatus.Completed) || (status == RepoStatus.ERROR)) ProgressBarCountUp();
            Application.DoEvents();
        }

        private void ProgressBarCountUp()
        {
            progressBar1.Increment(1);
            lbCounter.Text = progressBar1.Value.ToString() + " / " + progressBar1.Maximum.ToString();
        }

        private void SetNodesImage(TreeNodeCollection nodes, int imageIndex)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count > 0) SetNodesImage(node.Nodes, imageIndex);
                node.ImageIndex = node.SelectedImageIndex = imageIndex;
            }
        }

        private void SetNodeStatus(TortoiseHgRepository repo, RepoStatus status)
        {
            TreeNode node = FindNode(lstRepos.Nodes, repo);
            switch(status)
            {
                case RepoStatus.Default:
                    node.ImageIndex = node.SelectedImageIndex = 0;
                    break;

                case RepoStatus.Executing:
                    node.ImageIndex = node.SelectedImageIndex = 3;
                    break;

                case RepoStatus.Completed:
                    node.ImageIndex = node.SelectedImageIndex = 2;
                    break;

                case RepoStatus.ERROR:
                    node.ImageIndex = node.SelectedImageIndex = 5;
                    break;
            }

            //Update parent image index.
            int imageIndex = 0;
            foreach(TreeNode item in node.Parent.Nodes)
            {
                if (!item.Checked) continue;
                if (item.ImageIndex == 3) { imageIndex = 3; break; }
                if ( item.ImageIndex == 5) { imageIndex = 5; break; }
                if( item.ImageIndex == 2) { imageIndex = 2; } //Continue to check all.
            }
            node.Parent.ImageIndex = node.Parent.SelectedImageIndex = imageIndex;
        }

        private TreeNode FindNode(TreeNodeCollection nodes, TortoiseHgRepository repo)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag == repo) return node;
                if (node.Nodes.Count > 0)
                {
                    TreeNode result = FindNode(node.Nodes, repo);
                    if (result != null) return result;
                }
            }
            return null;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            btStart.Enabled = false;
            SetNodesImage(lstRepos.Nodes, 0); //Reset all nodes status
            hg.ClearRepositoriesError();
            progressBar1.Maximum = hg.Repositories.Count(x => x.Selected == true);
            progressBar1.Value = 0;
            UserAborted = false;
            lbErrorCounts.Text = string.Empty;
            lbCounter.Text = string.Empty;
            ThreadCount = cbThreads.Enabled ? Convert.ToInt32(cbThreads.SelectedItem.ToString()) : 1;

            if (HgFunctions[cbFunctions.SelectedItem.ToString()].PromptWarning)
            {
                if (MessageBox.Show("WARNING: This operation will erase uncommitted changes! Continue?", this.Text,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                { return; }
            }
            UpdateControl(true);

            threadWorker.RunWorkerAsync(cbFunctions.SelectedItem.ToString());
        }
        private void btStop_Click(object sender, EventArgs e)
        {
            UserAborted = true;
            UpdateControl(false);
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            diagnosticsTextBox1.ListenerEnabled = false;
        }
        private static int GetNumberOfCores() { return Environment.ProcessorCount; }
        private void cbFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControl(false);
        }

        private void lbErrorCounts_Click(object sender, EventArgs e)
        {
            if (hg.Repositories.Count(x => x.Error) == 0) return;
            using (FailedRepoDialog dialog = new FailedRepoDialog(hg.Repositories.Where(x => x.Error == true).Select(x => x.ShortName).ToArray()))
            {
                dialog.ShowDialog();
            }
        }
        private void threadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            HgFunctions[e.Argument.ToString()].Function();
        }
        private void threadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (UserAborted)
            {
                progressBar1.Value = 0;
                lbCounter.Text = "Aborted";
            }
            else
            {
                lbCounter.Text = "Completed";
            }

            UpdateControl(false);
        }
        private void UpdateControl(bool started)
        {
            btStart.Enabled = !started;
            btStop.Enabled = started;

            cbFunctions.Enabled = !started;
            cbThreads.Enabled = (!started && HgFunctions[cbFunctions.SelectedItem.ToString()].MultiThreading);
        }

        #endregion

        #region [ TortoiseHg Functions ]

        private void RegisterFunctions()
        {
            HgFunctions = new Dictionary<string, HgItem>();
            HgFunctions.Add("Verify Repositories", new HgItem() { Function = Function_VerifyRepositories, MultiThreading = true });
            HgFunctions.Add("Fix Repositories", new HgItem() { Function = Function_FixRepositories, MultiThreading = false, PromptWarning = true });
            HgFunctions.Add("Update Repositories", new HgItem() { Function = Function_UpdateRepositories, MultiThreading = true, PromptWarning = true });
            HgFunctions.Add("Pull Changes (default)", new HgItem() { Function = Function_PullIncomingChanges, MultiThreading = false });
            HgFunctions.Add("Pull Changes (default) + Update", new HgItem() { Function = Function_PullIncomingChanges_Update, MultiThreading = false, PromptWarning = true });
            HgFunctions.Add("Push Changes (default)", new HgItem() { Function = Function_PushOutgoingChanges, MultiThreading = false });
            HgFunctions.Add("Push Changes (default) + Update", new HgItem() { Function = Function_PushOutgoingChanges_Update, MultiThreading = false, PromptWarning = true });

        }
        private void Function_VerifyRepositories()
        {
            if (ThreadCount == 1)
            {
                foreach (TortoiseHgRepository repository in hg.Repositories.Where(x => x.Selected))
                {
                    if (UserAborted) return;
                    UpdateStatus(repository, RepoStatus.Executing);

                    try
                    {
                        hg.VerifyRepository(repository.Path);
                        UpdateStatus(repository, RepoStatus.Completed);
                    }
                    catch (Exception ex)
                    {
                        repository.Error = true;
                        repository.ErrorString = ex.Message;
                        UpdateStatus(repository, RepoStatus.ERROR);
                    }
                }
            }
            else
            {
                //Multi-threading
                ParallelOptions taskOption = new ParallelOptions();
                taskOption.MaxDegreeOfParallelism = ThreadCount;
                TortoiseHgRepository[] Repositories = hg.Repositories.Where(x => x.Selected).ToArray();

                Parallel.For(0, Repositories.Length, taskOption, index =>
                {
                    TortoiseHgRepository repository = Repositories[index];
                    UpdateStatus(repository, RepoStatus.Executing);
                    TortoiseHgClient hg = new TortoiseHgClient();
                    hg.Name = Path.GetFileName(repository.Path);
                    try
                    {
                        hg.VerifyRepository(repository.Path);
                        UpdateStatus(repository, RepoStatus.Completed);
                    }
                    catch (Exception ex)
                    {
                        repository.Error = true;
                        repository.ErrorString = ex.Message;
                        UpdateStatus(repository, RepoStatus.ERROR);
                    }
                });
            }
        }
        private void Function_UpdateRepositories()
        {
            if (ThreadCount == 1)
            {
                foreach (TortoiseHgRepository repository in hg.Repositories.Where(x => x.Selected))
                {
                    if (UserAborted) return;
                    UpdateStatus(repository, RepoStatus.Executing);

                    try
                    {
                        hg.UpdateRepository(repository.Path);
                        UpdateStatus(repository, RepoStatus.Completed);
                    }
                    catch (Exception ex)
                    {
                        repository.Error = true;
                        repository.ErrorString = ex.Message;
                        UpdateStatus(repository, RepoStatus.ERROR);
                    }
                }
            }
            else
            {
                //Multi-threading
                ParallelOptions taskOption = new ParallelOptions();
                taskOption.MaxDegreeOfParallelism = ThreadCount;
                TortoiseHgRepository[] Repositories = hg.Repositories.Where(x => x.Selected).ToArray();

                Parallel.For(0, Repositories.Length, taskOption, index =>
                {
                    TortoiseHgRepository repository = Repositories[index];
                    UpdateStatus(repository, RepoStatus.Executing);
                    TortoiseHgClient hg = new TortoiseHgClient();
                    hg.Name = Path.GetFileName(repository.Path);
                    try
                    {
                        hg.UpdateRepository(repository.Path);
                        UpdateStatus(repository, RepoStatus.Completed);
                    }
                    catch (Exception ex)
                    {
                        repository.Error = true;
                        repository.ErrorString = ex.Message;
                        UpdateStatus(repository, RepoStatus.ERROR);
                    }
                });
            }
        }
        private void Function_FixRepositories()
        {
            foreach (TortoiseHgRepository repository in hg.Repositories.Where(x => x.Selected))
            {
                if (UserAborted) return;
                UpdateStatus(repository, RepoStatus.Executing);

                try
                {
                    hg.UpdateRepository(repository.Path);
                    UpdateStatus(repository, RepoStatus.Completed);
                }
                catch (Exception)
                {
                    try
                    {
                        hg.FixRepository(repository.Path);
                        UpdateStatus(repository, RepoStatus.Completed);
                    }
                    catch (Exception ex)
                    {
                        repository.Error = true;
                        repository.ErrorString = ex.Message;
                        UpdateStatus(repository, RepoStatus.ERROR);
                    }
                }
            }

        }

        private void Function_PullIncomingChanges_Update()
        {
            PullIncomingChanges(true);
        }
        private void Function_PullIncomingChanges()
        {
            PullIncomingChanges(false);
        }
        private void PullIncomingChanges(bool update)
        { 
            foreach(TortoiseHgRepository repository in hg.Repositories.Where(x => x.Selected))
            {
                if (UserAborted) return;
                UpdateStatus(repository, RepoStatus.Executing);

                try
                {
                    hg.PullIncomingChanges(repository.Path);
                    if (update) hg.UpdateRepository(repository.Path);
                    UpdateStatus(repository, RepoStatus.Completed);
                }
                catch (Exception ex)
                {
                    repository.Error = true;
                    repository.ErrorString = ex.Message;
                    UpdateStatus(repository, RepoStatus.ERROR);
                }
            }
        }

        private void Function_PushOutgoingChanges_Update()
        {
            PushOutgoingChanges(true);
        }
        private void Function_PushOutgoingChanges()
        {
            PushOutgoingChanges(false);
        }
        private void PushOutgoingChanges(bool update)
        { 
            foreach (TortoiseHgRepository repository in hg.Repositories.Where(x => x.Selected))
            {
                if (UserAborted) return;
                UpdateStatus(repository, RepoStatus.Executing);

                try
                {
                    hg.PushOutgoingChanges(repository.Path);
                    if (update) hg.UpdateRepository(repository.Path);
                    UpdateStatus(repository, RepoStatus.Completed);
                }
                catch (Exception ex)
                {
                    repository.Error = true;
                    repository.ErrorString = ex.Message;
                    UpdateStatus(repository, RepoStatus.ERROR);
                }
            }
        }

        #endregion

    }
}
