namespace YouTubeDownloaderExt
{
    partial class MainFormTest
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.cmbConvertionOptions = new System.Windows.Forms.ComboBox();
            this.cmbQuality = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(453, 196);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(539, 56);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(62, 40);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // cmbConvertionOptions
            // 
            this.cmbConvertionOptions.FormattingEnabled = true;
            this.cmbConvertionOptions.Location = new System.Drawing.Point(262, 236);
            this.cmbConvertionOptions.Name = "cmbConvertionOptions";
            this.cmbConvertionOptions.Size = new System.Drawing.Size(121, 20);
            this.cmbConvertionOptions.TabIndex = 2;
            // 
            // cmbQuality
            // 
            this.cmbQuality.FormattingEnabled = true;
            this.cmbQuality.Location = new System.Drawing.Point(393, 289);
            this.cmbQuality.Name = "cmbQuality";
            this.cmbQuality.Size = new System.Drawing.Size(121, 20);
            this.cmbQuality.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 519);
            this.Controls.Add(this.cmbQuality);
            this.Controls.Add(this.cmbConvertionOptions);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ComboBox cmbConvertionOptions;
        private System.Windows.Forms.ComboBox cmbQuality;
    }
}

