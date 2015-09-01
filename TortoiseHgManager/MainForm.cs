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

namespace TortoiseHgManager
{
    public partial class MainForm : Form
    {
#if SIMULATION
        private Random rand;
#endif
        #region [ Private Fields ]

        private bool UserAborted;
        private int ThreadCount;
        private List<string> ErrorRepos;
        private List<string> FilteredRepo;
        private TortoiseHgClient hg = new TortoiseHgClient();

        private delegate void HgFunction();
        private Dictionary<string, HgItem> HgFunctions;
        private class HgItem
        {
            public HgFunction Function { get; set; }
            public bool MultiThreading { get; set; }
        }

        #endregion

        public MainForm()
        {
            InitializeComponent();
            diagnosticsTextBox1.Clear();
            lbCounter.Text = string.Empty;
            lbErrorCounts.Text = string.Empty;
            ErrorRepos = new List<string>();
            FilteredRepo = new List<string>();

            cbThreads.Items.Clear();
            for (int x = 0; x < GetNumberOfCores(); x++)
                cbThreads.Items.Add(x + 1);
            cbThreads.SelectedIndex = 0;

            RegisterFunctions();
            cbFunctions.Items.AddRange(HgFunctions.Keys.ToArray());
            cbFunctions.SelectedIndex = 0;

            LoadRepositories();
            FilteredRepo.AddRange(hg.Repositories);
#if SIMULATION
            Text += " ~~~ SIMULATION ~~~";
            rand = new Random();
#endif
        }

        #region [ Private Functions ]

        private void LoadRepositories()
        {
            hg.LoadRepositories();
            dgRepo.Rows.Clear();
            txtRepoFilter.Text = string.Empty;

            foreach (string repository in hg.Repositories)
            {
                dgRepo.Rows.Add(repository);
            }
        }
        private void UpdateRepositories()
        {
            string filterSpec = txtRepoFilter.Text.ToLower();
            FilteredRepo.Clear();
            if (string.IsNullOrEmpty(filterSpec))
                FilteredRepo.AddRange(hg.Repositories);
            else
            {
                foreach (string repository in hg.Repositories)
                {
                    if (repository.ToLower().Contains(filterSpec))
                        FilteredRepo.Add(repository);
                }
            }

            dgRepo.Rows.Clear();
            foreach (string repository in FilteredRepo)
                dgRepo.Rows.Add(repository);
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
        private delegate void UpdateDelegate(string repository, string value);
        private void UpdateStatus(string repository, string value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateDelegate(UpdateStatus), new object[] { repository, value });
                return;
            }

            foreach (DataGridViewRow row in dgRepo.Rows)
            {
                if (row.Cells[0].Value.ToString() == repository)
                {
                    row.Cells[1].Value = value;
                    progressBar1.Increment(1);
                    lbCounter.Text = progressBar1.Value.ToString() + " / " + progressBar1.Maximum.ToString();
                    if (value.CompareTo("OK") != 0)
                    {
                        lbErrorCounts.Text = ErrorRepos.Count.ToString() + (ErrorRepos.Count > 1 ? "Errors" : "Error");
                    }
                    return;
                }
            }

        }
        private void dgRepo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgRepo.SelectedRows.Count != 0)
            {
                DataGridViewRow dgRepoSelectedRows = dgRepo.SelectedRows[0];
                if (dgRepoSelectedRows.Tag != null)
                {
                    Trace.WriteLine("");
                    Trace.WriteLine(dgRepoSelectedRows.Cells[1].Value);
                    Trace.WriteLine(dgRepoSelectedRows.Tag.ToString());
                }
            }
        }
        private void btStart_Click(object sender, EventArgs e)
        {
            btStart.Enabled = false;

            progressBar1.Maximum = dgRepo.Rows.Count;
            progressBar1.Value = 0;
            UserAborted = false;
            lbErrorCounts.Text = string.Empty;
            lbCounter.Text = string.Empty;
            ThreadCount = cbThreads.Enabled ? Convert.ToInt32(cbThreads.SelectedItem.ToString()) : 1;
            if(cbFunctions.SelectedItem.ToString() != "Fix Repositories") ErrorRepos.Clear();

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
        private void txtRepoFilter_TextChanged(object sender, EventArgs e)
        {
            UpdateRepositories();
        }
        private static int GetNumberOfCores(){ return Environment.ProcessorCount; }
        private void cbFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControl(false);
        }

        private void lbErrorCounts_Click(object sender, EventArgs e)
        {
            if (ErrorRepos.Count() == 0) return;
            using (FailedRepoDialog dialog = new FailedRepoDialog(ErrorRepos.ToArray()))
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

            btStart.Enabled = true;
            btStop.Enabled = false;
            cbFunctions.Enabled = true;

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
            HgFunctions.Add("Fix Repositories", new HgItem() { Function = Function_FixRepositories, MultiThreading = false });
            HgFunctions.Add("Pull Changes (default)", new HgItem() { Function = Function_PullIncomingChanges, MultiThreading = false });
            HgFunctions.Add("Push Changes (default)", new HgItem() { Function = Function_PushOutgoingChanges, MultiThreading = false });

        }
        private void Function_VerifyRepositories()
        {
            if (ThreadCount == 1)
            {
                foreach (string repository in FilteredRepo)
                {
                    if (UserAborted) return;

                    string result = hg.VerifyRepository(repository);
                    if (result == "ERROR") ErrorRepos.Add(repository);
                    UpdateStatus(repository, result);
                }
            }
            else
            {
                //Multi-threading
                ParallelOptions taskOption = new ParallelOptions();
                taskOption.MaxDegreeOfParallelism = ThreadCount;

                Parallel.For(0, FilteredRepo.Count, taskOption, index =>
                {
                    string item = FilteredRepo[index];
                    TortoiseHgClient hg = new TortoiseHgClient();
                    hg.Name = Path.GetFileName(item);
                    string result = hg.VerifyRepository(item);
                    if (result == "ERROR") ErrorRepos.Add(item);
                    UpdateStatus(item, result);
                });
            }
        }
        private void Function_FixRepositories()
        {
            if(ErrorRepos.Count == 0)
            {
                Trace.WriteLine("No problematic repositories detected. Run 'Verify Repositories' first.");
            }
            Trace.WriteLine("Fixing " + ErrorRepos.Count() + " repositories...");

            List<string> NeedRepair = new List<string>();
            NeedRepair.AddRange(ErrorRepos);
            while (NeedRepair.Count() != 0)
            {
                string target = NeedRepair[0];
                if (UserAborted) return;
                string result = hg.FixRepository(target);
                UpdateStatus(target, result);
                if (result == "OK") ErrorRepos.Remove(target);
                NeedRepair.RemoveAt(0); //Remove repository regardless whether it had been fixed.
            };
        }
        private void Function_PullIncomingChanges()
        {
            foreach (string repository in FilteredRepo)
            {
                if (UserAborted) return;
                string result = hg.PullIncomingChanges(repository);
                UpdateStatus(repository, result);
            }
        }
        private void Function_PushOutgoingChanges()
        {
            foreach (string repository in FilteredRepo)
            {
                if (UserAborted) return;
                string result = hg.PushOutgoingChanges(repository);
                UpdateStatus(repository, result);
            }
        }

        #endregion

    }
}
