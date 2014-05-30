using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TortoiseHgManager
{
    public partial class FailedRepoDialog : Form
    {
        public FailedRepoDialog(string [] failedRepositories)
        {
            InitializeComponent();
            lstRepos.Items.Clear();
            if(failedRepositories != null)
                lstRepos.Items.AddRange(failedRepositories);
            try
            {
                System.IO.File.WriteAllLines("FailedRepositories.txt", failedRepositories);
            }
            catch { }
        }
    }
}
