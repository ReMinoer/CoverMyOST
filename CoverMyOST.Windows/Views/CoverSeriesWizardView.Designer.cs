namespace CoverMyOST.Windows.Views
{
    partial class CoverSeriesWizardView
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
            this._listView = new System.Windows.Forms.ListView();
            this.imageColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._coverPreview = new System.Windows.Forms.PictureBox();
            this._applyButton = new System.Windows.Forms.Button();
            this._countLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._searchProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.albumLabel = new System.Windows.Forms.Label();
            this._fileTextBox = new System.Windows.Forms.TextBox();
            this._albumTextBox = new System.Windows.Forms.TextBox();
            this._playButton = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this._coverNameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._coverPreview)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _listView
            // 
            this._listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.imageColumn});
            this._listView.FullRowSelect = true;
            this._listView.GridLines = true;
            this._listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._listView.HideSelection = false;
            this._listView.LabelWrap = false;
            this._listView.Location = new System.Drawing.Point(12, 12);
            this._listView.MultiSelect = false;
            this._listView.Name = "_listView";
            this._listView.Size = new System.Drawing.Size(299, 366);
            this._listView.TabIndex = 0;
            this._listView.UseCompatibleStateImageBehavior = false;
            // 
            // imageColumn
            // 
            this.imageColumn.Text = "Image";
            this.imageColumn.Width = 25;
            // 
            // _coverPreview
            // 
            this._coverPreview.BackColor = System.Drawing.Color.White;
            this._coverPreview.Location = new System.Drawing.Point(6, 19);
            this._coverPreview.Name = "_coverPreview";
            this._coverPreview.Size = new System.Drawing.Size(200, 200);
            this._coverPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._coverPreview.TabIndex = 1;
            this._coverPreview.TabStop = false;
            // 
            // _applyButton
            // 
            this._applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._applyButton.Location = new System.Drawing.Point(388, 337);
            this._applyButton.Name = "_applyButton";
            this._applyButton.Size = new System.Drawing.Size(141, 41);
            this._applyButton.TabIndex = 3;
            this._applyButton.Text = "Apply";
            this._applyButton.UseVisualStyleBackColor = true;
            // 
            // _countLabel
            // 
            this._countLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._countLabel.AutoSize = true;
            this._countLabel.Location = new System.Drawing.Point(317, 15);
            this._countLabel.Name = "_countLabel";
            this._countLabel.Size = new System.Drawing.Size(48, 13);
            this._countLabel.TabIndex = 6;
            this._countLabel.Text = "100/100";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._searchProgressBar,
            this._statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 387);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(538, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // _searchProgressBar
            // 
            this._searchProgressBar.Name = "_searchProgressBar";
            this._searchProgressBar.Size = new System.Drawing.Size(100, 16);
            this._searchProgressBar.Step = 1;
            this._searchProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._searchProgressBar.Value = 20;
            // 
            // _statusLabel
            // 
            this._statusLabel.AutoSize = false;
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(421, 17);
            this._statusLabel.Spring = true;
            this._statusLabel.Text = "Search...";
            this._statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // albumLabel
            // 
            this.albumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.albumLabel.AutoSize = true;
            this.albumLabel.Location = new System.Drawing.Point(317, 41);
            this.albumLabel.Name = "albumLabel";
            this.albumLabel.Size = new System.Drawing.Size(45, 13);
            this.albumLabel.TabIndex = 5;
            this.albumLabel.Text = "Album : ";
            // 
            // _fileTextBox
            // 
            this._fileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._fileTextBox.Location = new System.Drawing.Point(371, 12);
            this._fileTextBox.Name = "_fileTextBox";
            this._fileTextBox.ReadOnly = true;
            this._fileTextBox.Size = new System.Drawing.Size(158, 20);
            this._fileTextBox.TabIndex = 8;
            // 
            // _albumTextBox
            // 
            this._albumTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._albumTextBox.Location = new System.Drawing.Point(371, 38);
            this._albumTextBox.Name = "_albumTextBox";
            this._albumTextBox.Size = new System.Drawing.Size(158, 20);
            this._albumTextBox.TabIndex = 9;
            // 
            // _playButton
            // 
            this._playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._playButton.Location = new System.Drawing.Point(317, 337);
            this._playButton.Name = "_playButton";
            this._playButton.Size = new System.Drawing.Size(65, 41);
            this._playButton.TabIndex = 10;
            this._playButton.Text = "Play";
            this._playButton.UseVisualStyleBackColor = true;
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this._coverPreview);
            this.groupBox.Location = new System.Drawing.Point(317, 103);
            this.groupBox.Name = "groupBox";
            this.groupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox.Size = new System.Drawing.Size(212, 228);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Preview";
            // 
            // _coverNameLabel
            // 
            this._coverNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._coverNameLabel.AutoEllipsis = true;
            this._coverNameLabel.AutoSize = true;
            this._coverNameLabel.Location = new System.Drawing.Point(320, 74);
            this._coverNameLabel.MaximumSize = new System.Drawing.Size(210, 15);
            this._coverNameLabel.Name = "_coverNameLabel";
            this._coverNameLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._coverNameLabel.Size = new System.Drawing.Size(64, 13);
            this._coverNameLabel.TabIndex = 11;
            this._coverNameLabel.Text = "Cover name";
            // 
            // CoverSeriesWizardView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(538, 409);
            this.Controls.Add(this._coverNameLabel);
            this.Controls.Add(this._playButton);
            this.Controls.Add(this._albumTextBox);
            this.Controls.Add(this._fileTextBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._countLabel);
            this.Controls.Add(this.albumLabel);
            this.Controls.Add(this._applyButton);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this._listView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(554, 447);
            this.Name = "CoverSeriesWizardView";
            this.ShowInTaskbar = false;
            this.Text = "Select a cover";
            ((System.ComponentModel.ISupportInitialize)(this._coverPreview)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView _listView;
        private System.Windows.Forms.PictureBox _coverPreview;
        private System.Windows.Forms.Button _applyButton;
        private System.Windows.Forms.Label _countLabel;
        private System.Windows.Forms.ColumnHeader imageColumn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar _searchProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel _statusLabel;
        private System.Windows.Forms.Label albumLabel;
        private System.Windows.Forms.TextBox _fileTextBox;
        private System.Windows.Forms.TextBox _albumTextBox;
        private System.Windows.Forms.Button _playButton;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label _coverNameLabel;

    }
}