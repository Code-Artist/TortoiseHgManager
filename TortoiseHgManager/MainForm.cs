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
using CodeArtEng.Controls;

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
            public string CommandLineSwitch { get; set; }
        }

        private CommandLineHelper CommandLine;
        private bool CommandLineValid = false;
        private List<string> CommandLineQueue = null;
        private bool CloseAfterComplete = false;
        private int CommandLineStatus = 0;

        #endregion

        public MainForm()
        {
            InitializeComponent();
            Text += " (V" + Application.ProductVersion + ")";
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

            CommandLine = new CommandLineHelper(Text);
            CommandLine.AddSwitch("/t", "Number of thread used for repositories operation [1 to 4]. Default = 1", "thread");

            CommandLine.AddSwitch("/a", "Select all repositories.");
            CommandLine.AddSwitch("/exclude-repo", "Exclude defined repositories and childs, separated by ';'. Can use with /a.", "repositories");
            CommandLine.AddSwitch("/include-repo", "Include defined repositories and childs, separated by ';'.", "repositories");

            CommandLine.AddSwitch("/v", "Verify Repositories");
            CommandLine.AddSwitch("/f", "Fix Repositories.");
            CommandLine.AddSwitch("/u", "Update Repositories");
            CommandLine.AddSwitch("/pull", "Pull incoming changes from default path");
            CommandLine.AddSwitch("/pull&update", "Pull incoming changes from default path, update local changes to tip revision");
            CommandLine.AddSwitch("/push", "Push outgoing changes from default path.");
            CommandLine.AddSwitch("/scan", "Check repositories for changes.");

            CommandLine.AddSwitch("/min", "Start application in minimized mode.");

            CommandLine.AddSwitch("/x", "Close application end of execution.");

            CommandLineValid = CommandLine.ParseCommandLine();
#if SIMULATION
            Text += " ~~~ SIMULATION ~~~";
            rand = new Random();
#endif
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (CommandLineValid)
            {
                CommandLineQueue = new List<string>();

                if (CommandLine.IsSwitchSet("/min")) this.WindowState = FormWindowState.Minimized;
                if (CommandLine.IsSwitchSet("/x")) CloseAfterComplete = true;

                //Thread
                int threadCount = 1;
                if (CommandLine.IsSwitchSet("/t"))
                {
                    if (Int32.TryParse(CommandLine.GetSwitchValue("/t", "1"), out threadCount))
                    {
                        if ((threadCount < 1) && (threadCount > 4))
                        {
                            Console.WriteLine("WARNING: Option /t value out of range --> [" + threadCount + "]. Default to 1!");
                            threadCount = 1;
                        }
                    }
                    else
                    {
                        Console.WriteLine("WARNING: Invalid input value for /t: default to 1!");
                        threadCount = 1;
                    }
                    cbThreads.SelectedIndex = cbThreads.Items.IndexOf(threadCount);
                }

                //Repositories
                if (CommandLine.IsSwitchSet("/a"))
                {
                    SelectAllNodes();
                }
                if (CommandLine.IsSwitchSet("/include-repo"))
                {
                    string[] repos = CommandLine.GetSwitchValue("/include-repo").Split(';');
                    foreach (string repo in repos) SelectNode(true, repo.Trim());
                }
                if (CommandLine.IsSwitchSet("/exclude-repo"))
                {
                    string[] repos = CommandLine.GetSwitchValue("/exclude-repo").Split(';');
                    foreach (string repo in repos) SelectNode(false, repo.Trim());
                }

                //Functions
                //ToDo: Create Queue, Generic Queue?
                if (CommandLine.IsSwitchSet("/v")) CommandLineQueue.Add("/v");
                if (CommandLine.IsSwitchSet("/f")) CommandLineQueue.Add("/f");
                if (CommandLine.IsSwitchSet("/u")) CommandLineQueue.Add("/u");
                if (CommandLine.IsSwitchSet("/pull")) CommandLineQueue.Add("/pull");
                if (CommandLine.IsSwitchSet("/pull&update")) CommandLineQueue.Add("/pull&update");
                if (CommandLine.IsSwitchSet("/push")) CommandLineQueue.Add("/push");
                if (CommandLine.IsSwitchSet("/scan")) CommandLineQueue.Add("/scan");

                if (CommandLineQueue.Count > 0) cmdLineExecutor.Enabled = true;
            }//Command Line
        }

        private void cmdLineExecutor_Tick(object sender, EventArgs e)
        {
            cmdLineExecutor.Enabled = false;
            if (CommandLineQueue.Count == 0)
            {
                CommandLineValid = false;

                //Terminate application with exit code.
                if (CloseAfterComplete)
                {
                    Trace.WriteLine("Application terminated with code " + CommandLineStatus);
                    Environment.Exit(CommandLineStatus);
                }
                return;
            }

            string sw = CommandLineQueue.First();
            CommandLineQueue.RemoveAt(0);
            string hgFuncKey = HgFunctions.FirstOrDefault(x => x.Value.CommandLineSwitch == sw).Key;

            cbFunctions.SelectedIndex = cbFunctions.Items.IndexOf(hgFuncKey);
            btStart.PerformClick();
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
            if (status == RepoStatus.ERROR) repository.Error = true;
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
            switch (status)
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
            foreach (TreeNode item in node.Parent.Nodes)
            {
                if (!item.Checked) continue;
                if (item.ImageIndex == 3) { imageIndex = 3; break; }
                if (item.ImageIndex == 5) { imageIndex = 5; break; }
                if (item.ImageIndex == 2) { imageIndex = 2; } //Continue to check all.
            }
            node.Parent.ImageIndex = node.Parent.SelectedImageIndex = imageIndex;
        }

        private TreeNode FindNodeByName(TreeNodeCollection nodes, string nodeName)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name == nodeName) return node;
                if (node.Nodes.Count > 0)
                {
                    TreeNode result = FindNodeByName(node.Nodes, nodeName);
                    if (result != null) return result;
                }
            }
            return null;
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
            CommandLineStatus = -9;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.ExitCode = CommandLineStatus;
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
            if (CommandLineValid && !UserAborted)
            {
                foreach (TortoiseHgRepository repo in hg.Repositories)
                {
                    if (repo.Error) CommandLineStatus = -1;
                }
                cmdLineExecutor.Enabled = true;
            }
        }

        private void UpdateControl(bool started)
        {
            btStart.Enabled = !started;
            btCmdLineBuilder.Enabled = !started;
            btStop.Enabled = started;

            cbFunctions.Enabled = !started;
            cbThreads.Enabled = (!started && HgFunctions[cbFunctions.SelectedItem.ToString()].MultiThreading);
        }

        private void SelectAllNodes()
        {
            foreach (TreeNode node in lstRepos.Nodes)
            {
                SelectNode(true, node);
            }
        }
        private void SelectNode(bool check, string nodeName)
        {
            TreeNode ptrNode = FindNodeByName(lstRepos.Nodes, nodeName);
            if (ptrNode == null) return;
            SelectNode(check, ptrNode);
        }

        private void btSelectAll_Click(object sender, EventArgs e)
        {
            SelectAllNodes();
        }
        private void btSelectNone_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in lstRepos.Nodes)
            {
                SelectNode(false, node);
            }
        }
        private void SelectNode(bool check, TreeNode parentNode)
        {
            parentNode.Checked = check;
            foreach (TreeNode node in parentNode.Nodes)
            {
                SelectNode(check, node);
            }
        }

        private void openWithTortoiseHgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TortoiseHgRepository repo = lstRepos.SelectedNode?.Tag as TortoiseHgRepository ?? null;
            if (repo == null) return;
            string repositoryPath = Path.GetFullPath(repo.Path);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "thg.exe";
            info.WorkingDirectory = repositoryPath;
            Process.Start(info);
        }

        private void lstRepos_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            lstRepos.SelectedNode = e.Node;
        }

        #endregion

        #region [ TortoiseHg Functions ]

        private void RegisterFunctions()
        {
            HgFunctions = new Dictionary<string, HgItem>();
            HgFunctions.Add("Verify Repositories", new HgItem() { Function = Function_VerifyRepositories, MultiThreading = true, CommandLineSwitch = "/v" });
            HgFunctions.Add("Fix Repositories", new HgItem() { Function = Function_FixRepositories, MultiThreading = false, PromptWarning = true, CommandLineSwitch = "/f" });
            HgFunctions.Add("Update Repositories", new HgItem() { Function = Function_UpdateRepositories, MultiThreading = true, PromptWarning = true, CommandLineSwitch = "/u" });
            HgFunctions.Add("Pull Changes (default)", new HgItem() { Function = Function_PullIncomingChanges, MultiThreading = false, CommandLineSwitch = "/pull" });
            HgFunctions.Add("Pull Changes (default) + Update", new HgItem() { Function = Function_PullIncomingChanges_Update, MultiThreading = false, PromptWarning = true, CommandLineSwitch = "/pull&update" });
            HgFunctions.Add("Push Changes (default)", new HgItem() { Function = Function_PushOutgoingChanges, MultiThreading = false, CommandLineSwitch = "/push" });
            //HgFunctions.Add("Push Changes (default) + Update", new HgItem() { Function = Function_PushOutgoingChanges_Update, MultiThreading = false, PromptWarning = true });
            HgFunctions.Add("Check Repositories for Changes", new HgItem() { Function = Function_CheckRepositoriesForChanges, MultiThreading = true, PromptWarning = false, CommandLineSwitch = "/scan" });

        }
        private void Function_VerifyRepositories()
        {
            if (ThreadCount == 1)
            {
                foreach (TortoiseHgRepository repository in hg.Repositories.Where(x => x.Selected))
                {
                    if (UserAborted)
                    {
                        repository.Error = true;
                        repository.ErrorString = "User Aborted!";
                        UpdateStatus(repository, RepoStatus.ERROR);
                        continue;
                    }
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
                    if (UserAborted)
                    {
                        repository.Error = true;
                        repository.ErrorString = "User Aborted!";
                        UpdateStatus(repository, RepoStatus.ERROR);
                        return;
                    }
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
                    if (UserAborted)
                    {
                        repository.Error = true;
                        repository.ErrorString = "User Aborted!";
                        UpdateStatus(repository, RepoStatus.ERROR);
                        continue;
                    }
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
                    if (UserAborted)
                    {
                        repository.Error = true;
                        repository.ErrorString = "User Aborted!";
                        UpdateStatus(repository, RepoStatus.ERROR);
                        return;
                    }
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
                if (UserAborted)
                {
                    repository.Error = true;
                    repository.ErrorString = "User Aborted!";
                    UpdateStatus(repository, RepoStatus.ERROR);
                    continue;
                }
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
            foreach (TortoiseHgRepository repository in hg.Repositories.Where(x => x.Selected))
            {
                if (UserAborted)
                {
                    repository.Error = true;
                    repository.ErrorString = "User Aborted!";
                    UpdateStatus(repository, RepoStatus.ERROR);
                    continue;
                }
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
                if (UserAborted)
                {
                    repository.Error = true;
                    repository.ErrorString = "User Aborted!";
                    UpdateStatus(repository, RepoStatus.ERROR);
                    continue;
                }
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

        private void Function_CheckRepositoriesForChanges()
        {
            if (ThreadCount == 1)
            {
                foreach (TortoiseHgRepository repository in hg.Repositories.Where(x => x.Selected))
                {
                    if (UserAborted) return;
                    UpdateStatus(repository, RepoStatus.Executing);

                    try
                    {
                        int status = hg.CheckModifiedRepository(repository.Path);
                        UpdateStatus(repository, status == 0 ? RepoStatus.Completed : RepoStatus.ERROR);
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
                    if (UserAborted)
                    {
                        repository.Error = true;
                        repository.ErrorString = "User Aborted!";
                        UpdateStatus(repository, RepoStatus.ERROR);
                        return;
                    }
                    UpdateStatus(repository, RepoStatus.Executing);
                    TortoiseHgClient hg = new TortoiseHgClient();
                    hg.Name = Path.GetFileName(repository.Path);
                    try
                    {
                        int status = hg.CheckModifiedRepository(repository.Path);
                        UpdateStatus(repository, status == 0 ? RepoStatus.Completed : RepoStatus.ERROR);
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

        #endregion

        private void btCmdLineBuilder_Click(object sender, EventArgs e)
        {
            CommandLine.Reset();
            TortoiseHgRepository[] selectedRepos = GetSelectedRepos();
            TortoiseHgRepository[] notSelectedRepos = GetNotSelectedRepos();

            string[] selectedGroup = selectedRepos.Select(x => x.Group).Distinct().ToArray();
            string[] notSelectedGroup = notSelectedRepos.Select(x => x.Group).Distinct().ToArray();

            if (notSelectedGroup.Length == 0) CommandLine.SetSwitch("/A");
            else
            {
                string[] mixStateGroup = selectedGroup.Intersect(notSelectedGroup).ToArray();
                string[] exclusiveSelectedGroup = selectedGroup.Except(mixStateGroup).ToArray();
                string[] exclusiveNotSelectedGroup = notSelectedGroup.Except(mixStateGroup).ToArray();

                if(exclusiveSelectedGroup.Length > exclusiveNotSelectedGroup.Length)
                {
                    CommandLine.SetSwitch("/A");
                    CommandLine.SetSwitch("/EXCLUDE-REPO", string.Join("; ", exclusiveNotSelectedGroup));
                }
                else
                {
                    CommandLine.SetSwitch("/INCLUDE-REPO", string.Join("; ", exclusiveSelectedGroup));
                }

                if (mixStateGroup.Length != 0)
                {
                    //Process mixStateGroup;
                    List<TortoiseHgRepository> r = new List<TortoiseHgRepository>();
                    foreach(string g in mixStateGroup)
                    {
                        r.AddRange(selectedRepos.Where(x => x.Group == g).ToArray());
                    }
                    string value = CommandLine.GetSwitchValue("/INCLUDE-REPO");
                    if (!string.IsNullOrEmpty(value)) value += "; ";
                    value += string.Join("; ", r.Select(x => x.ShortName));
                    CommandLine.SetSwitch("/INCLUDE-REPO", value);
                }

            }
            CommandLine.SetSwitch("/T", cbThreads.Text);
            CommandLine.SetSwitch(HgFunctions[cbFunctions.Text].CommandLineSwitch.ToUpper());

            CommandLine.ShowDialog();
        }

        private TortoiseHgRepository[] GetSelectedRepos()
        {
            List<TortoiseHgRepository> repos = new List<TortoiseHgRepository>();
            foreach (TortoiseHgRepository repo in hg.Repositories)
            {
                if (repo.Selected) repos.Add(repo);
            }
            return repos.ToArray();
        }

        private TortoiseHgRepository[] GetNotSelectedRepos()
        {
            List<TortoiseHgRepository> repos = new List<TortoiseHgRepository>();
            foreach (TortoiseHgRepository repo in hg.Repositories)
            {
                if (!repo.Selected) repos.Add(repo);
            }
            return repos.ToArray();
        }
    }
}
