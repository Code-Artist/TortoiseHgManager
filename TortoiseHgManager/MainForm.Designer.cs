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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node2");
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btStart = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.diagnosticsTextBox1 = new CodeArtEng.Diagnostics.Controls.DiagnosticsTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbCounter = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.lbErrorCounts = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbThreads = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbFunctions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.threadWorker = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.repoProperties = new System.Windows.Forms.PropertyGrid();
            this.popUpMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openWithTortoiseHgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btSelectAll = new System.Windows.Forms.Button();
            this.btSelectNone = new System.Windows.Forms.Button();
            this.lstRepos = new RikTheVeggie.TriStateTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.popUpMenuStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btStart
            // 
            this.btStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btStart.Location = new System.Drawing.Point(766, 4);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(46, 26);
            this.btStart.TabIndex = 4;
            this.btStart.Text = "Start";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStop
            // 
            this.btStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btStop.Location = new System.Drawing.Point(813, 4);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(46, 26);
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
            this.diagnosticsTextBox1.Location = new System.Drawing.Point(5, 5);
            this.diagnosticsTextBox1.Multiline = true;
            this.diagnosticsTextBox1.Name = "diagnosticsTextBox1";
            this.diagnosticsTextBox1.ReadOnly = true;
            this.diagnosticsTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.diagnosticsTextBox1.Size = new System.Drawing.Size(603, 382);
            this.diagnosticsTextBox1.TabIndex = 0;
            this.diagnosticsTextBox1.WordWrap = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbCounter,
            this.progressBar1,
            this.lbErrorCounts});
            this.statusStrip1.Location = new System.Drawing.Point(3, 460);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(862, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbCounter
            // 
            this.lbCounter.AutoSize = false;
            this.lbCounter.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lbCounter.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            this.lbCounter.Name = "lbCounter";
            this.lbCounter.Size = new System.Drawing.Size(80, 17);
            this.lbCounter.Text = "xxx / xxx";
            // 
            // progressBar1
            // 
            this.progressBar1.AutoSize = false;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(180, 16);
            // 
            // lbErrorCounts
            // 
            this.lbErrorCounts.AutoSize = false;
            this.lbErrorCounts.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.lbErrorCounts.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            this.lbErrorCounts.ForeColor = System.Drawing.Color.Red;
            this.lbErrorCounts.Name = "lbErrorCounts";
            this.lbErrorCounts.Size = new System.Drawing.Size(80, 17);
            this.lbErrorCounts.Text = "xxx Errors";
            this.lbErrorCounts.Click += new System.EventHandler(this.lbErrorCounts_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbThreads);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbFunctions);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btStart);
            this.panel1.Controls.Add(this.btStop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(862, 33);
            this.panel1.TabIndex = 9;
            // 
            // cbThreads
            // 
            this.cbThreads.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbThreads.FormattingEnabled = true;
            this.cbThreads.Location = new System.Drawing.Point(367, 7);
            this.cbThreads.Name = "cbThreads";
            this.cbThreads.Size = new System.Drawing.Size(58, 21);
            this.cbThreads.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Functions";
            // 
            // cbFunctions
            // 
            this.cbFunctions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFunctions.FormattingEnabled = true;
            this.cbFunctions.Location = new System.Drawing.Point(63, 7);
            this.cbFunctions.Name = "cbFunctions";
            this.cbFunctions.Size = new System.Drawing.Size(237, 21);
            this.cbFunctions.TabIndex = 9;
            this.cbFunctions.SelectedIndexChanged += new System.EventHandler(this.cbFunctions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(315, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
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
            this.splitContainer1.Location = new System.Drawing.Point(3, 36);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstRepos);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(862, 424);
            this.splitContainer1.SplitterDistance = 231;
            this.splitContainer1.TabIndex = 11;
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
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.label4.Size = new System.Drawing.Size(225, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Repositories";
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(621, 418);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.diagnosticsTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage1.Size = new System.Drawing.Size(613, 392);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Logger";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.repoProperties);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(613, 392);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Properties";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // repoProperties
            // 
            this.repoProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.repoProperties.Location = new System.Drawing.Point(3, 3);
            this.repoProperties.Name = "repoProperties";
            this.repoProperties.Size = new System.Drawing.Size(607, 386);
            this.repoProperties.TabIndex = 0;
            // 
            // popUpMenuStrip
            // 
            this.popUpMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWithTortoiseHgToolStripMenuItem});
            this.popUpMenuStrip.Name = "popUpMenuStrip";
            this.popUpMenuStrip.Size = new System.Drawing.Size(191, 26);
            // 
            // openWithTortoiseHgToolStripMenuItem
            // 
            this.openWithTortoiseHgToolStripMenuItem.Name = "openWithTortoiseHgToolStripMenuItem";
            this.openWithTortoiseHgToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.openWithTortoiseHgToolStripMenuItem.Text = "Open with TortoiseHg";
            this.openWithTortoiseHgToolStripMenuItem.Click += new System.EventHandler(this.openWithTortoiseHgToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btSelectNone);
            this.panel2.Controls.Add(this.btSelectAll);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 23);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(225, 33);
            this.panel2.TabIndex = 2;
            // 
            // btSelectAll
            // 
            this.btSelectAll.Location = new System.Drawing.Point(5, 4);
            this.btSelectAll.Name = "btSelectAll";
            this.btSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btSelectAll.TabIndex = 0;
            this.btSelectAll.Text = "Select All";
            this.btSelectAll.UseVisualStyleBackColor = true;
            this.btSelectAll.Click += new System.EventHandler(this.btSelectAll_Click);
            // 
            // btSelectNone
            // 
            this.btSelectNone.Location = new System.Drawing.Point(86, 4);
            this.btSelectNone.Name = "btSelectNone";
            this.btSelectNone.Size = new System.Drawing.Size(75, 23);
            this.btSelectNone.TabIndex = 1;
            this.btSelectNone.Text = "Select None";
            this.btSelectNone.UseVisualStyleBackColor = true;
            this.btSelectNone.Click += new System.EventHandler(this.btSelectNone_Click);
            // 
            // lstRepos
            // 
            this.lstRepos.ContextMenuStrip = this.popUpMenuStrip;
            this.lstRepos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRepos.HideSelection = false;
            this.lstRepos.ImageIndex = 0;
            this.lstRepos.ImageList = this.imageList;
            this.lstRepos.Location = new System.Drawing.Point(3, 56);
            this.lstRepos.Name = "lstRepos";
            treeNode1.Name = "Node3";
            treeNode1.StateImageIndex = 0;
            treeNode1.Text = "Node3";
            treeNode2.Name = "Node0";
            treeNode2.StateImageIndex = 0;
            treeNode2.Text = "Node0";
            treeNode3.Name = "Node1";
            treeNode3.StateImageIndex = 0;
            treeNode3.Text = "Node1";
            treeNode4.Name = "Node2";
            treeNode4.StateImageIndex = 0;
            treeNode4.Text = "Node2";
            this.lstRepos.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4});
            this.lstRepos.SelectedImageIndex = 0;
            this.lstRepos.Size = new System.Drawing.Size(225, 365);
            this.lstRepos.TabIndex = 1;
            this.lstRepos.TriStateStyleProperty = RikTheVeggie.TriStateTreeView.TriStateStyles.Installer;
            this.lstRepos.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.lstRepos_AfterCheck);
            this.lstRepos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.lstRepos_AfterSelect);
            this.lstRepos.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.lstRepos_NodeMouseClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 485);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "TortoiseHg Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.popUpMenuStrip.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
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
    }
}

