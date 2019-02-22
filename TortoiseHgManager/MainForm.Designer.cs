namespace TortoiseHgManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node2");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btStart = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.diagnosticsTextBox1 = new CodeArtEng.Diagnostics.Controls.DiagnosticsTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbCounter = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.lbErrorCounts = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btCmdLineBuilder = new System.Windows.Forms.Button();
            this.cbThreads = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbFunctions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.threadWorker = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstRepos = new RikTheVeggie.TriStateTreeView();
            this.popUpMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openWithTortoiseHgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btSelectNone = new System.Windows.Forms.Button();
            this.btSelectAll = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.repoProperties = new System.Windows.Forms.PropertyGrid();
            this.cmdLineExecutor = new System.Windows.Forms.Timer(this.components);
            this.chkSilentMode = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.popUpMenuStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btStart
            // 
            this.btStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btStart.Location = new System.Drawing.Point(1021, 5);
            this.btStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(61, 32);
            this.btStart.TabIndex = 4;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStop
            // 
            this.btStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btStop.Location = new System.Drawing.Point(1084, 5);
            this.btStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(61, 32);
            this.btStop.TabIndex = 4;
            this.btStop.Text = "Stop";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // diagnosticsTextBox1
            // 
            this.diagnosticsTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosticsTextBox1.FlushEnabled = false;
            this.diagnosticsTextBox1.ListenerEnabled = false;
            this.diagnosticsTextBox1.Location = new System.Drawing.Point(7, 6);
            this.diagnosticsTextBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.diagnosticsTextBox1.Multiline = true;
            this.diagnosticsTextBox1.Name = "diagnosticsTextBox1";
            this.diagnosticsTextBox1.OutputFile = null;
            this.diagnosticsTextBox1.OutputFileBackup = null;
            this.diagnosticsTextBox1.ReadOnly = true;
            this.diagnosticsTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.diagnosticsTextBox1.Size = new System.Drawing.Size(807, 473);
            this.diagnosticsTextBox1.TabIndex = 0;
            this.diagnosticsTextBox1.WordWrap = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbCounter,
            this.progressBar1,
            this.lbErrorCounts});
            this.statusStrip1.Location = new System.Drawing.Point(4, 567);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1149, 26);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbCounter
            // 
            this.lbCounter.AutoSize = false;
            this.lbCounter.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lbCounter.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            this.lbCounter.Name = "lbCounter";
            this.lbCounter.Size = new System.Drawing.Size(80, 21);
            this.lbCounter.Text = "xxx / xxx";
            // 
            // progressBar1
            // 
            this.progressBar1.AutoSize = false;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(240, 20);
            // 
            // lbErrorCounts
            // 
            this.lbErrorCounts.AutoSize = false;
            this.lbErrorCounts.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.lbErrorCounts.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            this.lbErrorCounts.ForeColor = System.Drawing.Color.Red;
            this.lbErrorCounts.Name = "lbErrorCounts";
            this.lbErrorCounts.Size = new System.Drawing.Size(80, 21);
            this.lbErrorCounts.Text = "xxx Errors";
            this.lbErrorCounts.Click += new System.EventHandler(this.lbErrorCounts_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkSilentMode);
            this.panel1.Controls.Add(this.btCmdLineBuilder);
            this.panel1.Controls.Add(this.cbThreads);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbFunctions);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btStart);
            this.panel1.Controls.Add(this.btStop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1149, 41);
            this.panel1.TabIndex = 9;
            // 
            // btCmdLineBuilder
            // 
            this.btCmdLineBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCmdLineBuilder.Location = new System.Drawing.Point(813, 5);
            this.btCmdLineBuilder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btCmdLineBuilder.Name = "btCmdLineBuilder";
            this.btCmdLineBuilder.Size = new System.Drawing.Size(179, 32);
            this.btCmdLineBuilder.TabIndex = 12;
            this.btCmdLineBuilder.Text = "Command Line Builder";
            this.btCmdLineBuilder.UseVisualStyleBackColor = true;
            this.btCmdLineBuilder.Click += new System.EventHandler(this.btCmdLineBuilder_Click);
            // 
            // cbThreads
            // 
            this.cbThreads.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbThreads.FormattingEnabled = true;
            this.cbThreads.Location = new System.Drawing.Point(489, 9);
            this.cbThreads.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbThreads.Name = "cbThreads";
            this.cbThreads.Size = new System.Drawing.Size(76, 24);
            this.cbThreads.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Functions";
            // 
            // cbFunctions
            // 
            this.cbFunctions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFunctions.FormattingEnabled = true;
            this.cbFunctions.Location = new System.Drawing.Point(84, 9);
            this.cbFunctions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbFunctions.Name = "cbFunctions";
            this.cbFunctions.Size = new System.Drawing.Size(315, 24);
            this.cbFunctions.TabIndex = 9;
            this.cbFunctions.SelectedIndexChanged += new System.EventHandler(this.cbFunctions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(420, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Threads";
            // 
            // threadWorker
            // 
            this.threadWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.threadWorker_DoWork);
            this.threadWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.threadWorker_RunWorkerCompleted);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 45);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstRepos);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Size = new System.Drawing.Size(1149, 522);
            this.splitContainer1.SplitterDistance = 307;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 11;
            // 
            // lstRepos
            // 
            this.lstRepos.ContextMenuStrip = this.popUpMenuStrip;
            this.lstRepos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRepos.HideSelection = false;
            this.lstRepos.ImageIndex = 0;
            this.lstRepos.ImageList = this.imageList;
            this.lstRepos.Location = new System.Drawing.Point(4, 70);
            this.lstRepos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstRepos.Name = "lstRepos";
            treeNode5.Name = "Node3";
            treeNode5.StateImageIndex = 0;
            treeNode5.Text = "Node3";
            treeNode6.Name = "Node0";
            treeNode6.StateImageIndex = 0;
            treeNode6.Text = "Node0";
            treeNode7.Name = "Node1";
            treeNode7.StateImageIndex = 0;
            treeNode7.Text = "Node1";
            treeNode8.Name = "Node2";
            treeNode8.StateImageIndex = 0;
            treeNode8.Text = "Node2";
            this.lstRepos.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8});
            this.lstRepos.SelectedImageIndex = 0;
            this.lstRepos.Size = new System.Drawing.Size(299, 448);
            this.lstRepos.TabIndex = 1;
            this.lstRepos.TriStateStyleProperty = RikTheVeggie.TriStateTreeView.TriStateStyles.Installer;
            this.lstRepos.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.lstRepos_AfterCheck);
            this.lstRepos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.lstRepos_AfterSelect);
            this.lstRepos.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.lstRepos_NodeMouseClick);
            // 
            // popUpMenuStrip
            // 
            this.popUpMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.popUpMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWithTortoiseHgToolStripMenuItem});
            this.popUpMenuStrip.Name = "popUpMenuStrip";
            this.popUpMenuStrip.Size = new System.Drawing.Size(224, 28);
            // 
            // openWithTortoiseHgToolStripMenuItem
            // 
            this.openWithTortoiseHgToolStripMenuItem.Name = "openWithTortoiseHgToolStripMenuItem";
            this.openWithTortoiseHgToolStripMenuItem.Size = new System.Drawing.Size(223, 24);
            this.openWithTortoiseHgToolStripMenuItem.Text = "Open with TortoiseHg";
            this.openWithTortoiseHgToolStripMenuItem.Click += new System.EventHandler(this.openWithTortoiseHgToolStripMenuItem_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "GreyDot.png");
            this.imageList.Images.SetKeyName(1, "LightGreenDot.png");
            this.imageList.Images.SetKeyName(2, "GreenDot.png");
            this.imageList.Images.SetKeyName(3, "YellowDot.png");
            this.imageList.Images.SetKeyName(4, "OrangeDot.png");
            this.imageList.Images.SetKeyName(5, "RedDot.png");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btSelectNone);
            this.panel2.Controls.Add(this.btSelectAll);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(4, 29);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(299, 41);
            this.panel2.TabIndex = 2;
            // 
            // btSelectNone
            // 
            this.btSelectNone.Location = new System.Drawing.Point(115, 5);
            this.btSelectNone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btSelectNone.Name = "btSelectNone";
            this.btSelectNone.Size = new System.Drawing.Size(100, 28);
            this.btSelectNone.TabIndex = 1;
            this.btSelectNone.Text = "Select None";
            this.btSelectNone.UseVisualStyleBackColor = true;
            this.btSelectNone.Click += new System.EventHandler(this.btSelectNone_Click);
            // 
            // btSelectAll
            // 
            this.btSelectAll.Location = new System.Drawing.Point(7, 5);
            this.btSelectAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btSelectAll.Name = "btSelectAll";
            this.btSelectAll.Size = new System.Drawing.Size(100, 28);
            this.btSelectAll.TabIndex = 0;
            this.btSelectAll.Text = "Select All";
            this.btSelectAll.UseVisualStyleBackColor = true;
            this.btSelectAll.Click += new System.EventHandler(this.btSelectAll_Click);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label4.Size = new System.Drawing.Size(299, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Repositories";
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 4);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(829, 514);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.diagnosticsTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tabPage1.Size = new System.Drawing.Size(821, 485);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Logger";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.repoProperties);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(820, 485);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Properties";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // repoProperties
            // 
            this.repoProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.repoProperties.LineColor = System.Drawing.SystemColors.ControlDark;
            this.repoProperties.Location = new System.Drawing.Point(4, 4);
            this.repoProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.repoProperties.Name = "repoProperties";
            this.repoProperties.Size = new System.Drawing.Size(812, 477);
            this.repoProperties.TabIndex = 0;
            // 
            // cmdLineExecutor
            // 
            this.cmdLineExecutor.Interval = 10;
            this.cmdLineExecutor.Tick += new System.EventHandler(this.cmdLineExecutor_Tick);
            // 
            // chkSilentMode
            // 
            this.chkSilentMode.AutoSize = true;
            this.chkSilentMode.Location = new System.Drawing.Point(576, 11);
            this.chkSilentMode.Name = "chkSilentMode";
            this.chkSilentMode.Size = new System.Drawing.Size(104, 21);
            this.chkSilentMode.TabIndex = 13;
            this.chkSilentMode.Text = "Silent Mode";
            this.chkSilentMode.UseVisualStyleBackColor = true;
            this.chkSilentMode.CheckedChanged += new System.EventHandler(this.chkSilentMode_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 597);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Text = "TortoiseHg Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.popUpMenuStrip.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbCounter;
        private System.Windows.Forms.ToolStripProgressBar progressBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripStatusLabel lbErrorCounts;
        private System.ComponentModel.BackgroundWorker threadWorker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbThreads;
        private System.Windows.Forms.ComboBox cbFunctions;
        private System.Windows.Forms.Label label2;
        private CodeArtEng.Diagnostics.Controls.DiagnosticsTextBox diagnosticsTextBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private RikTheVeggie.TriStateTreeView lstRepos;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.PropertyGrid repoProperties;
        private System.Windows.Forms.ContextMenuStrip popUpMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openWithTortoiseHgToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btSelectAll;
        private System.Windows.Forms.Button btSelectNone;
        private System.Windows.Forms.Timer cmdLineExecutor;
        private System.Windows.Forms.Button btCmdLineBuilder;
        private System.Windows.Forms.CheckBox chkSilentMode;
    }
}

