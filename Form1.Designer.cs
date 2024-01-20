
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Copy files to:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(35, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select Destination Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // destinationFolder
            // 
            this.destinationFolder.Location = new System.Drawing.Point(117, 221);
            this.destinationFolder.Multiline = true;
            this.destinationFolder.Name = "destinationFolder";
            this.destinationFolder.Size = new System.Drawing.Size(209, 20);
            this.destinationFolder.TabIndex = 2;
            // 
            // sourceFolder
            // 
            this.sourceFolder.Location = new System.Drawing.Point(117, 99);
            this.sourceFolder.Name = "sourceFolder";
            this.sourceFolder.Size = new System.Drawing.Size(209, 20);
            this.sourceFolder.TabIndex = 5;
            // 
            // selectSource
            // 
            this.selectSource.Location = new System.Drawing.Point(35, 73);
            this.selectSource.Name = "selectSource";
            this.selectSource.Size = new System.Drawing.Size(167, 23);
            this.selectSource.TabIndex = 4;
            this.selectSource.Text = "Select Source Folder";
            this.selectSource.UseVisualStyleBackColor = true;
            this.selectSource.Click += new System.EventHandler(this.selectSource_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Copy files from:";
            // 
            // copyButton
            // 
            this.copyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyButton.Location = new System.Drawing.Point(35, 307);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(98, 51);
            this.copyButton.TabIndex = 6;
            this.copyButton.Text = "Copy!";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // favoritesButton
            // 
            this.favoritesButton.Location = new System.Drawing.Point(35, 24);
            this.favoritesButton.Name = "favoritesButton";
            this.favoritesButton.Size = new System.Drawing.Size(127, 23);
            this.favoritesButton.TabIndex = 7;
            this.favoritesButton.Text = "Select Favorites File";
            this.favoritesButton.UseVisualStyleBackColor = true;
            this.favoritesButton.Click += new System.EventHandler(this.favoritesButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.favoritesButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.sourceFolder);
            this.Controls.Add(this.selectSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.destinationFolder);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Favorites";
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
    }
}

