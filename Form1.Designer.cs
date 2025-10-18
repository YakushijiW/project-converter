namespace project_converter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPagePack = new System.Windows.Forms.TabPage();
            this.btnPack = new System.Windows.Forms.Button();
            this.btnClearFiles = new System.Windows.Forms.Button();
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtExcludeDirs = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtIncludeExt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddPreset = new System.Windows.Forms.Button();
            this.btnDeletePreset = new System.Windows.Forms.Button();
            this.comboProfiles = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseProject = new System.Windows.Forms.Button();
            this.txtProjectPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageUnpack = new System.Windows.Forms.TabPage();
            this.btnUnpack = new System.Windows.Forms.Button();
            this.btnBrowseExtract = new System.Windows.Forms.Button();
            this.txtExtractPath = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.txtDefaultOutputName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.lblGuide = new System.Windows.Forms.Label();
            this.linkGithub = new System.Windows.Forms.LinkLabel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControlMain.SuspendLayout();
            this.tabPagePack.SuspendLayout();
            this.tabPageUnpack.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.tabPageAbout.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPagePack);
            this.tabControlMain.Controls.Add(this.tabPageUnpack);
            this.tabControlMain.Controls.Add(this.tabPageSettings);
            this.tabControlMain.Controls.Add(this.tabPageAbout);
            this.tabControlMain.Location = new System.Drawing.Point(12, 12);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(776, 368);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPagePack
            // 
            this.tabPagePack.Controls.Add(this.btnPack);
            this.tabPagePack.Controls.Add(this.btnClearFiles);
            this.tabPagePack.Controls.Add(this.btnAddFiles);
            this.tabPagePack.Controls.Add(this.lstFiles);
            this.tabPagePack.Controls.Add(this.label9);
            this.tabPagePack.Controls.Add(this.txtExcludeDirs);
            this.tabPagePack.Controls.Add(this.label6);
            this.tabPagePack.Controls.Add(this.txtIncludeExt);
            this.tabPagePack.Controls.Add(this.label5);
            this.tabPagePack.Controls.Add(this.btnDeletePreset);
            this.tabPagePack.Controls.Add(this.btnAddPreset);
            this.tabPagePack.Controls.Add(this.comboProfiles);
            this.tabPagePack.Controls.Add(this.label4);
            this.tabPagePack.Controls.Add(this.btnSaveFile);
            this.tabPagePack.Controls.Add(this.txtOutputFile);
            this.tabPagePack.Controls.Add(this.label3);
            this.tabPagePack.Controls.Add(this.btnBrowseProject);
            this.tabPagePack.Controls.Add(this.txtProjectPath);
            this.tabPagePack.Controls.Add(this.label2);
            this.tabPagePack.Location = new System.Drawing.Point(4, 26);
            this.tabPagePack.Name = "tabPagePack";
            this.tabPagePack.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePack.Size = new System.Drawing.Size(768, 338);
            this.tabPagePack.TabIndex = 0;
            this.tabPagePack.Text = "打包 (Project to Text)";
            this.tabPagePack.UseVisualStyleBackColor = true;
            // 
            // btnPack
            // 
            this.btnPack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPack.BackColor = System.Drawing.Color.LightGreen;
            this.btnPack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPack.Location = new System.Drawing.Point(18, 279);
            this.btnPack.Name = "btnPack";
            this.btnPack.Size = new System.Drawing.Size(732, 44);
            this.btnPack.TabIndex = 12;
            this.btnPack.Text = "开始打包";
            this.btnPack.UseVisualStyleBackColor = false;
            this.btnPack.Click += new System.EventHandler(this.btnPack_Click);
            // 
            // btnClearFiles
            // 
            this.btnClearFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearFiles.Location = new System.Drawing.Point(656, 43);
            this.btnClearFiles.Name = "btnClearFiles";
            this.btnClearFiles.Size = new System.Drawing.Size(94, 24);
            this.btnClearFiles.TabIndex = 23;
            this.btnClearFiles.Text = "清空";
            this.btnClearFiles.UseVisualStyleBackColor = true;
            this.btnClearFiles.Click += new System.EventHandler(this.btnClearFiles_Click);
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFiles.Location = new System.Drawing.Point(656, 13);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(94, 24);
            this.btnAddFiles.TabIndex = 22;
            this.btnAddFiles.Text = "添加文件...";
            this.btnAddFiles.UseVisualStyleBackColor = true;
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.AllowDrop = true;
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.ItemHeight = 17;
            this.lstFiles.Location = new System.Drawing.Point(166, 23);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(484, 38);
            this.lstFiles.TabIndex = 21;
            this.lstFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstFiles_DragDrop);
            this.lstFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstFiles_DragEnter);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 34);
            this.label9.TabIndex = 20;
            this.label9.Text = "按文件打包\n(可多选/拖拽导入):";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // txtExcludeDirs
            // 
            this.txtExcludeDirs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExcludeDirs.Location = new System.Drawing.Point(166, 244);
            this.txtExcludeDirs.Name = "txtExcludeDirs";
            this.txtExcludeDirs.Size = new System.Drawing.Size(584, 23);
            this.txtExcludeDirs.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "排除的文件夹 (逗号分隔):";
            // 
            // txtIncludeExt
            // 
            this.txtIncludeExt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIncludeExt.Location = new System.Drawing.Point(166, 204);
            this.txtIncludeExt.Name = "txtIncludeExt";
            this.txtIncludeExt.Size = new System.Drawing.Size(584, 23);
            this.txtIncludeExt.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "包含的文件后缀 (逗号分隔):";
            // 
            // btnAddPreset
            // 
            this.btnAddPreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPreset.Location = new System.Drawing.Point(528, 164);
            this.btnAddPreset.Name = "btnAddPreset";
            this.btnAddPreset.Size = new System.Drawing.Size(94, 25);
            this.btnAddPreset.TabIndex = 13;
            this.btnAddPreset.Text = "添加预设";
            this.btnAddPreset.UseVisualStyleBackColor = true;
            this.btnAddPreset.Visible = false;
            this.btnAddPreset.Click += new System.EventHandler(this.btnAddPreset_Click);
            // 
            // btnDeletePreset
            // 
            this.btnDeletePreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletePreset.Location = new System.Drawing.Point(628, 164);
            this.btnDeletePreset.Name = "btnDeletePreset";
            this.btnDeletePreset.Size = new System.Drawing.Size(94, 25);
            this.btnDeletePreset.TabIndex = 24;
            this.btnDeletePreset.Text = "删除预设";
            this.btnDeletePreset.UseVisualStyleBackColor = true;
            this.btnDeletePreset.Visible = false;
            this.btnDeletePreset.Click += new System.EventHandler(this.btnDeletePreset_Click);
            // 
            // comboProfiles
            // 
            this.comboProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProfiles.FormattingEnabled = true;
            this.comboProfiles.Location = new System.Drawing.Point(166, 164);
            this.comboProfiles.Name = "comboProfiles";
            this.comboProfiles.Size = new System.Drawing.Size(356, 25);
            this.comboProfiles.TabIndex = 7;
            this.comboProfiles.SelectedIndexChanged += new System.EventHandler(this.comboProfiles_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "项目类型预设:";
            // 
            // btnSaveFile
            // 
            this.btnSaveFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveFile.Location = new System.Drawing.Point(656, 124);
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.Size = new System.Drawing.Size(94, 24);
            this.btnSaveFile.TabIndex = 5;
            this.btnSaveFile.Text = "浏览...";
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFile.Location = new System.Drawing.Point(166, 124);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(484, 23);
            this.txtOutputFile.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "输出文件夹:";
            // 
            // btnBrowseProject
            // 
            this.btnBrowseProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseProject.Location = new System.Drawing.Point(656, 84);
            this.btnBrowseProject.Name = "btnBrowseProject";
            this.btnBrowseProject.Size = new System.Drawing.Size(94, 24);
            this.btnBrowseProject.TabIndex = 2;
            this.btnBrowseProject.Text = "浏览...";
            this.btnBrowseProject.UseVisualStyleBackColor = true;
            this.btnBrowseProject.Click += new System.EventHandler(this.btnBrowseProject_Click);
            // 
            // txtProjectPath
            // 
            this.txtProjectPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProjectPath.Location = new System.Drawing.Point(166, 84);
            this.txtProjectPath.Name = "txtProjectPath";
            this.txtProjectPath.Size = new System.Drawing.Size(484, 23);
            this.txtProjectPath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "项目文件夹:";
            // 
            // tabPageUnpack
            // 
            this.tabPageUnpack.Controls.Add(this.btnUnpack);
            this.tabPageUnpack.Controls.Add(this.btnBrowseExtract);
            this.tabPageUnpack.Controls.Add(this.txtExtractPath);
            this.tabPageUnpack.Controls.Add(this.label8);
            this.tabPageUnpack.Controls.Add(this.btnBrowseFile);
            this.tabPageUnpack.Controls.Add(this.txtInputFile);
            this.tabPageUnpack.Controls.Add(this.label7);
            this.tabPageUnpack.Location = new System.Drawing.Point(4, 26);
            this.tabPageUnpack.Name = "tabPageUnpack";
            this.tabPageUnpack.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUnpack.Size = new System.Drawing.Size(768, 338);
            this.tabPageUnpack.TabIndex = 1;
            this.tabPageUnpack.Text = "解包 (Text to Project)";
            this.tabPageUnpack.UseVisualStyleBackColor = true;
            // 
            // btnUnpack
            // 
            this.btnUnpack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnpack.BackColor = System.Drawing.Color.LightBlue;
            this.btnUnpack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnUnpack.Location = new System.Drawing.Point(18, 176);
            this.btnUnpack.Name = "btnUnpack";
            this.btnUnpack.Size = new System.Drawing.Size(732, 44);
            this.btnUnpack.TabIndex = 6;
            this.btnUnpack.Text = "开始解包";
            this.btnUnpack.UseVisualStyleBackColor = false;
            this.btnUnpack.Click += new System.EventHandler(this.btnUnpack_Click);
            // 
            // btnBrowseExtract
            // 
            this.btnBrowseExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseExtract.Location = new System.Drawing.Point(656, 96);
            this.btnBrowseExtract.Name = "btnBrowseExtract";
            this.btnBrowseExtract.Size = new System.Drawing.Size(94, 24);
            this.btnBrowseExtract.TabIndex = 5;
            this.btnBrowseExtract.Text = "浏览...";
            this.btnBrowseExtract.UseVisualStyleBackColor = true;
            this.btnBrowseExtract.Click += new System.EventHandler(this.btnBrowseExtract_Click);
            // 
            // txtExtractPath
            // 
            this.txtExtractPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExtractPath.Location = new System.Drawing.Point(166, 96);
            this.txtExtractPath.Name = "txtExtractPath";
            this.txtExtractPath.Size = new System.Drawing.Size(484, 23);
            this.txtExtractPath.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "解包到文件夹:";
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseFile.Location = new System.Drawing.Point(656, 56);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(94, 24);
            this.btnBrowseFile.TabIndex = 2;
            this.btnBrowseFile.Text = "浏览...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // txtInputFile
            // 
            this.txtInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFile.Location = new System.Drawing.Point(166, 56);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(484, 23);
            this.txtInputFile.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "选择文本文件:";
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.btnSaveSettings);
            this.tabPageSettings.Controls.Add(this.txtDefaultOutputName);
            this.tabPageSettings.Controls.Add(this.label10);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 26);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(768, 338);
            this.tabPageSettings.TabIndex = 2;
            this.tabPageSettings.Text = "设置";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(18, 104);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(120, 30);
            this.btnSaveSettings.TabIndex = 8;
            this.btnSaveSettings.Text = "保存设置";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // txtDefaultOutputName
            // 
            this.txtDefaultOutputName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultOutputName.Location = new System.Drawing.Point(166, 21);
            this.txtDefaultOutputName.Name = "txtDefaultOutputName";
            this.txtDefaultOutputName.Size = new System.Drawing.Size(484, 23);
            this.txtDefaultOutputName.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "默认输出文件名:";
            // 
            // tabPageAbout
            // 
            this.tabPageAbout.Controls.Add(this.lblGuide);
            this.tabPageAbout.Controls.Add(this.linkGithub);
            this.tabPageAbout.Controls.Add(this.lblVersion);
            this.tabPageAbout.Location = new System.Drawing.Point(4, 26);
            this.tabPageAbout.Name = "tabPageAbout";
            this.tabPageAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAbout.Size = new System.Drawing.Size(768, 338);
            this.tabPageAbout.TabIndex = 3;
            this.tabPageAbout.Text = "关于";
            this.tabPageAbout.UseVisualStyleBackColor = true;
            // 
            // lblGuide
            // 
            this.lblGuide.AutoSize = true;
            this.lblGuide.Location = new System.Drawing.Point(21, 100);
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.Size = new System.Drawing.Size(500, 85);
            this.lblGuide.TabIndex = 2;
            this.lblGuide.Text = "使用指南:\r\n\r\n1) 选择项目文件夹，或拖拽多个文件到列表进行打包。\r\n2) 选择预设或点击自定义后'添加预设'保存自定义扩展名和排除目录。\r\n3) 可以删除不需要的自定义预设。\r\n4) 解包页选择文本文件和目标路径开始解包。\r\n5) 设置页可修改默认输出文件名。";
            // 
            // linkGithub
            // 
            this.linkGithub.AutoSize = true;
            this.linkGithub.Location = new System.Drawing.Point(21, 57);
            this.linkGithub.Name = "linkGithub";
            this.linkGithub.Size = new System.Drawing.Size(175, 17);
            this.linkGithub.TabIndex = 1;
            this.linkGithub.TabStop = true;
            this.linkGithub.Text = "GitHub: 点击打开项目主页链接";
            this.linkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGithub_LinkClicked);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(21, 27);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(113, 17);
            this.lblVersion.TabIndex = 0;
            this.lblVersion.Text = "版本：v1.0.0 (示例)";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 386);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(776, 12);
            this.progressBar.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 406);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(92, 17);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "状态：准备就绪";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel1.Text = "就绪";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.tabControlMain);
            this.Name = "Form1";
            this.Text = "项目打包/还原工具";
            this.tabControlMain.ResumeLayout(false);
            this.tabPagePack.ResumeLayout(false);
            this.tabPagePack.PerformLayout();
            this.tabPageUnpack.ResumeLayout(false);
            this.tabPageUnpack.PerformLayout();
            this.tabPageSettings.ResumeLayout(false);
            this.tabPageSettings.PerformLayout();
            this.tabPageAbout.ResumeLayout(false);
            this.tabPageAbout.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private TabControl tabControlMain;
        private TabPage tabPagePack;
        private TabPage tabPageUnpack;
		private Label label2;
		private TextBox txtProjectPath;
		private Button btnBrowseProject;
		private Label label3;
		private TextBox txtOutputFile;
		private Button btnSaveFile;
		private Label label4;
		private ComboBox comboProfiles;
		private Label label5;
		private TextBox txtIncludeExt;
		private Label label6;
		private TextBox txtExcludeDirs;
		private Button btnPack;
		private Label label7;
		private TextBox txtInputFile;
		private Button btnBrowseFile;
		private Label label8;
		private TextBox txtExtractPath;
		private Button btnBrowseExtract;
		private Button btnUnpack;
		private ProgressBar progressBar;
		private Label lblStatus;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button btnAddPreset;
        private Button btnDeletePreset;
        private Label label9;
        private ListBox lstFiles;
        private Button btnAddFiles;
        private Button btnClearFiles;
        private TabPage tabPageSettings;
        private TabPage tabPageAbout;
        private Label label10;
        private TextBox txtDefaultOutputName;
        private Button btnSaveSettings;
        private Label lblVersion;
        private LinkLabel linkGithub;
        private Label lblGuide;
    }
}