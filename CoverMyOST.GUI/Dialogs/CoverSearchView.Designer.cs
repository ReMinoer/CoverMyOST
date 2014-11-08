namespace CoverMyOST.GUI.Dialogs
{
    partial class CoverSearchView
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
            this.listView = new System.Windows.Forms.ListView();
            this.imageColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.coverPreview = new System.Windows.Forms.PictureBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.nextButton = new System.Windows.Forms.Button();
            this.fileLabel = new System.Windows.Forms.Label();
            this.albumLabel = new System.Windows.Forms.Label();
            this.countLabel = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.searchProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.coverPreview)).BeginInit();
            this.groupBox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.imageColumn});
            this.listView.Location = new System.Drawing.Point(12, 12);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(157, 309);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            // 
            // imageColumn
            // 
            this.imageColumn.Text = "Image";
            this.imageColumn.Width = -1;
            // 
            // coverPreview
            // 
            this.coverPreview.BackColor = System.Drawing.Color.White;
            this.coverPreview.Location = new System.Drawing.Point(6, 19);
            this.coverPreview.Name = "coverPreview";
            this.coverPreview.Size = new System.Drawing.Size(200, 200);
            this.coverPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.coverPreview.TabIndex = 1;
            this.coverPreview.TabStop = false;
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this.coverPreview);
            this.groupBox.Location = new System.Drawing.Point(175, 47);
            this.groupBox.Name = "groupBox";
            this.groupBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox.Size = new System.Drawing.Size(212, 227);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Preview";
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Location = new System.Drawing.Point(246, 280);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(141, 41);
            this.nextButton.TabIndex = 3;
            this.nextButton.Text = "Next file";
            this.nextButton.UseVisualStyleBackColor = true;
            // 
            // fileLabel
            // 
            this.fileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fileLabel.AutoSize = true;
            this.fileLabel.Location = new System.Drawing.Point(175, 9);
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new System.Drawing.Size(32, 13);
            this.fileLabel.TabIndex = 4;
            this.fileLabel.Text = "File : ";
            // 
            // albumLabel
            // 
            this.albumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.albumLabel.AutoSize = true;
            this.albumLabel.Location = new System.Drawing.Point(178, 31);
            this.albumLabel.Name = "albumLabel";
            this.albumLabel.Size = new System.Drawing.Size(45, 13);
            this.albumLabel.TabIndex = 5;
            this.albumLabel.Text = "Album : ";
            // 
            // countLabel
            // 
            this.countLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(178, 294);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(48, 13);
            this.countLabel.TabIndex = 6;
            this.countLabel.Text = "100/100";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchProgressBar,
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 335);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(399, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // searchProgressBar
            // 
            this.searchProgressBar.Name = "searchProgressBar";
            this.searchProgressBar.Size = new System.Drawing.Size(100, 16);
            this.searchProgressBar.Step = 1;
            this.searchProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.searchProgressBar.Value = 20;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = false;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(251, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Search...";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CoverSearchView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 357);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.albumLabel);
            this.Controls.Add(this.fileLabel);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.listView);
            this.MinimumSize = new System.Drawing.Size(415, 365);
            this.Name = "CoverSearchView";
            this.Text = "Select a cover";
            ((System.ComponentModel.ISupportInitialize)(this.coverPreview)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.PictureBox coverPreview;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Label fileLabel;
        private System.Windows.Forms.Label albumLabel;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.ColumnHeader imageColumn;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar searchProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;

    }
}