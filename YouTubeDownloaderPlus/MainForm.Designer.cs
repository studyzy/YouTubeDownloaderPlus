namespace YouTubeDownloaderPlus
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
            this.btnDownload = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cmbQuality = new System.Windows.Forms.ComboBox();
            this.cmbConvertionOptions = new System.Windows.Forms.ComboBox();
            this.txbSaveFolder = new System.Windows.Forms.TextBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnDownloadSubtitle = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.cbxWithSubtitle = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(528, 111);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 50);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "Download Video";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 13);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(486, 191);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // cmbQuality
            // 
            this.cmbQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQuality.FormattingEnabled = true;
            this.cmbQuality.Items.AddRange(new object[] {
            "Stardard Quality",
            "Hight Quality",
            "High Definition (720p)",
            "Full High Definition (1080p)"});
            this.cmbQuality.Location = new System.Drawing.Point(528, 24);
            this.cmbQuality.Name = "cmbQuality";
            this.cmbQuality.Size = new System.Drawing.Size(181, 21);
            this.cmbQuality.TabIndex = 2;
            // 
            // cmbConvertionOptions
            // 
            this.cmbConvertionOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConvertionOptions.FormattingEnabled = true;
            this.cmbConvertionOptions.Location = new System.Drawing.Point(528, 66);
            this.cmbConvertionOptions.Name = "cmbConvertionOptions";
            this.cmbConvertionOptions.Size = new System.Drawing.Size(181, 21);
            this.cmbConvertionOptions.TabIndex = 3;
            // 
            // txbSaveFolder
            // 
            this.txbSaveFolder.Location = new System.Drawing.Point(107, 221);
            this.txbSaveFolder.Name = "txbSaveFolder";
            this.txbSaveFolder.Size = new System.Drawing.Size(391, 20);
            this.txbSaveFolder.TabIndex = 4;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectFolder.Location = new System.Drawing.Point(528, 219);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(45, 25);
            this.btnSelectFolder.TabIndex = 5;
            this.btnSelectFolder.Text = "...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "保存到文件夹：";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(697, 380);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "下载进度";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMinSize = new System.Drawing.Size(690, 331);
            this.panel1.AutoSize = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(691, 361);
            this.panel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 665);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(721, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(184, 17);
            this.toolStripStatusLabel1.Text = "欢迎使用深蓝Youtube批量下载器";
            // 
            // btnDownloadSubtitle
            // 
            this.btnDownloadSubtitle.Location = new System.Drawing.Point(577, 181);
            this.btnDownloadSubtitle.Name = "btnDownloadSubtitle";
            this.btnDownloadSubtitle.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadSubtitle.TabIndex = 13;
            this.btnDownloadSubtitle.Text = "Subtitle";
            this.btnDownloadSubtitle.UseVisualStyleBackColor = true;
            this.btnDownloadSubtitle.Click += new System.EventHandler(this.btnDownloadSubtitle_Click);
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(592, 218);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(99, 26);
            this.btnRename.TabIndex = 14;
            this.btnRename.Text = "RenameSubtitle";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbxWithSubtitle
            // 
            this.cbxWithSubtitle.AutoSize = true;
            this.cbxWithSubtitle.Checked = true;
            this.cbxWithSubtitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxWithSubtitle.Location = new System.Drawing.Point(623, 129);
            this.cbxWithSubtitle.Name = "cbxWithSubtitle";
            this.cbxWithSubtitle.Size = new System.Drawing.Size(83, 17);
            this.cbxWithSubtitle.TabIndex = 15;
            this.cbxWithSubtitle.Text = "WithSubtitle";
            this.cbxWithSubtitle.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 687);
            this.Controls.Add(this.cbxWithSubtitle);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnDownloadSubtitle);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.txbSaveFolder);
            this.Controls.Add(this.cmbConvertionOptions);
            this.Controls.Add(this.cmbQuality);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnDownload);
            this.Name = "MainForm";
            this.Text = "深蓝Youtube批量下载器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox cmbQuality;
        private System.Windows.Forms.ComboBox cmbConvertionOptions;
        private System.Windows.Forms.TextBox txbSaveFolder;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDownloadSubtitle;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.CheckBox cbxWithSubtitle;
      
    }
}