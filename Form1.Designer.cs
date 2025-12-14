
namespace MoveSelectedFavorites
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
            this.folderBrowserDestination = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.destinationFolder = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.sourceFolder = new System.Windows.Forms.TextBox();
            this.selectSource = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.copyButton = new System.Windows.Forms.Button();
            this.folderBrowserSource = new System.Windows.Forms.FolderBrowserDialog();
            this.favoritesButton = new System.Windows.Forms.Button();
            this.favoritesLabel = new System.Windows.Forms.Label();
            this.labelCopyCount = new System.Windows.Forms.Label();
            this.suffix = new System.Windows.Forms.TextBox();
            this.checkRenSource = new System.Windows.Forms.CheckBox();
            this.labelLog = new System.Windows.Forms.Label();
            this.log = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 345);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Copy files to:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(52, 300);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(250, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select Destination Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // destinationFolder
            // 
            this.destinationFolder.Location = new System.Drawing.Point(176, 340);
            this.destinationFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.destinationFolder.Multiline = true;
            this.destinationFolder.Name = "destinationFolder";
            this.destinationFolder.Size = new System.Drawing.Size(468, 29);
            this.destinationFolder.TabIndex = 2;
            // 
            // sourceFolder
            // 
            this.sourceFolder.Location = new System.Drawing.Point(176, 152);
            this.sourceFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sourceFolder.Name = "sourceFolder";
            this.sourceFolder.Size = new System.Drawing.Size(468, 26);
            this.sourceFolder.TabIndex = 5;
            // 
            // selectSource
            // 
            this.selectSource.Location = new System.Drawing.Point(52, 112);
            this.selectSource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectSource.Name = "selectSource";
            this.selectSource.Size = new System.Drawing.Size(250, 35);
            this.selectSource.TabIndex = 4;
            this.selectSource.Text = "Select Source Folder";
            this.selectSource.UseVisualStyleBackColor = true;
            this.selectSource.Click += new System.EventHandler(this.selectSource_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 157);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Copy files from:";
            // 
            // copyButton
            // 
            this.copyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyButton.Location = new System.Drawing.Point(52, 472);
            this.copyButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(147, 78);
            this.copyButton.TabIndex = 6;
            this.copyButton.Text = "Copy!";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // favoritesButton
            // 
            this.favoritesButton.Location = new System.Drawing.Point(52, 37);
            this.favoritesButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.favoritesButton.Name = "favoritesButton";
            this.favoritesButton.Size = new System.Drawing.Size(190, 35);
            this.favoritesButton.TabIndex = 7;
            this.favoritesButton.Text = "Select Favorites File";
            this.favoritesButton.UseVisualStyleBackColor = true;
            this.favoritesButton.Click += new System.EventHandler(this.favoritesButton_Click);
            // 
            // favoritesLabel
            // 
            this.favoritesLabel.AutoSize = true;
            this.favoritesLabel.Location = new System.Drawing.Point(272, 45);
            this.favoritesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.favoritesLabel.Name = "favoritesLabel";
            this.favoritesLabel.Size = new System.Drawing.Size(0, 20);
            this.favoritesLabel.TabIndex = 8;
            // 
            // labelCopyCount
            // 
            this.labelCopyCount.AutoSize = true;
            this.labelCopyCount.Location = new System.Drawing.Point(236, 503);
            this.labelCopyCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCopyCount.Name = "labelCopyCount";
            this.labelCopyCount.Size = new System.Drawing.Size(0, 20);
            this.labelCopyCount.TabIndex = 9;
            // 
            // suffix
            // 
            this.suffix.Enabled = false;
            this.suffix.Location = new System.Drawing.Point(352, 406);
            this.suffix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.suffix.Name = "suffix";
            this.suffix.Size = new System.Drawing.Size(148, 26);
            this.suffix.TabIndex = 11;
            this.suffix.Text = "_work";
            // 
            // checkRenSource
            // 
            this.checkRenSource.AutoSize = true;
            this.checkRenSource.Location = new System.Drawing.Point(51, 409);
            this.checkRenSource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkRenSource.Name = "checkRenSource";
            this.checkRenSource.Size = new System.Drawing.Size(290, 24);
            this.checkRenSource.TabIndex = 12;
            this.checkRenSource.Text = "Rename Source Folder using Suffix:";
            this.checkRenSource.UseVisualStyleBackColor = true;
            this.checkRenSource.CheckedChanged += new System.EventHandler(this.checkRenSource_CheckedChanged);
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(650, 103);
            this.labelLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(40, 20);
            this.labelLog.TabIndex = 13;
            this.labelLog.Text = "Log:";
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(700, 98);
            this.log.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log.Size = new System.Drawing.Size(451, 459);
            this.log.TabIndex = 14;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 578);
            this.Controls.Add(this.log);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.checkRenSource);
            this.Controls.Add(this.suffix);
            this.Controls.Add(this.labelCopyCount);
            this.Controls.Add(this.favoritesLabel);
            this.Controls.Add(this.favoritesButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.sourceFolder);
            this.Controls.Add(this.selectSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.destinationFolder);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1144, 608);
            this.Name = "MainForm";
            this.Text = "Favorites";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDestination;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox destinationFolder;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox sourceFolder;
        private System.Windows.Forms.Button selectSource;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserSource;
        private System.Windows.Forms.Button favoritesButton;
        private System.Windows.Forms.Label favoritesLabel;
        private System.Windows.Forms.Label labelCopyCount;
        private System.Windows.Forms.TextBox suffix;
        private System.Windows.Forms.CheckBox checkRenSource;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.TextBox log;
    }
}

