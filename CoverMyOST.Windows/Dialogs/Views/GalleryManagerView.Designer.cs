namespace CoverMyOST.Windows.Dialogs.Views
{
    partial class GalleryManagerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GalleryManagerView));
            this.onlineGroupBox = new System.Windows.Forms.GroupBox();
            this.descriptionTextBox = new System.Windows.Forms.RichTextBox();
            this.descriptionHeaderLabel = new System.Windows.Forms.Label();
            this.onlineGridView = new System.Windows.Forms.DataGridView();
            this.OnlineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OnlineEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UseCache = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ClearCache = new System.Windows.Forms.DataGridViewButtonColumn();
            this.localGroupBox = new System.Windows.Forms.GroupBox();
            this.localGridView = new System.Windows.Forms.DataGridView();
            this.LocalName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.onlineGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.onlineGridView)).BeginInit();
            this.localGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.localGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // onlineGroupBox
            // 
            this.onlineGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.onlineGroupBox.Controls.Add(this.descriptionTextBox);
            this.onlineGroupBox.Controls.Add(this.descriptionHeaderLabel);
            this.onlineGroupBox.Controls.Add(this.onlineGridView);
            this.onlineGroupBox.Location = new System.Drawing.Point(9, 9);
            this.onlineGroupBox.Name = "onlineGroupBox";
            this.onlineGroupBox.Padding = new System.Windows.Forms.Padding(7);
            this.onlineGroupBox.Size = new System.Drawing.Size(398, 444);
            this.onlineGroupBox.TabIndex = 0;
            this.onlineGroupBox.TabStop = false;
            this.onlineGroupBox.Text = "Online galleries";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.descriptionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.descriptionTextBox.Location = new System.Drawing.Point(11, 370);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Size = new System.Drawing.Size(377, 67);
            this.descriptionTextBox.TabIndex = 2;
            this.descriptionTextBox.Text = resources.GetString("descriptionTextBox.Text");
            // 
            // descriptionHeaderLabel
            // 
            this.descriptionHeaderLabel.AutoSize = true;
            this.descriptionHeaderLabel.Location = new System.Drawing.Point(4, 350);
            this.descriptionHeaderLabel.Name = "descriptionHeaderLabel";
            this.descriptionHeaderLabel.Size = new System.Drawing.Size(68, 13);
            this.descriptionHeaderLabel.TabIndex = 1;
            this.descriptionHeaderLabel.Text = "Gallery name";
            // 
            // onlineGridView
            // 
            this.onlineGridView.AllowUserToAddRows = false;
            this.onlineGridView.AllowUserToDeleteRows = false;
            this.onlineGridView.AllowUserToResizeColumns = false;
            this.onlineGridView.AllowUserToResizeRows = false;
            this.onlineGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.onlineGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.onlineGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.onlineGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OnlineName,
            this.OnlineEnabled,
            this.UseCache,
            this.ClearCache});
            this.onlineGridView.Location = new System.Drawing.Point(7, 20);
            this.onlineGridView.MultiSelect = false;
            this.onlineGridView.Name = "onlineGridView";
            this.onlineGridView.RowHeadersVisible = false;
            this.onlineGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.onlineGridView.Size = new System.Drawing.Size(381, 323);
            this.onlineGridView.TabIndex = 0;
            // 
            // OnlineName
            // 
            this.OnlineName.HeaderText = "Name";
            this.OnlineName.Name = "OnlineName";
            this.OnlineName.ReadOnly = true;
            // 
            // OnlineEnabled
            // 
            this.OnlineEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.OnlineEnabled.HeaderText = "Enabled";
            this.OnlineEnabled.Name = "OnlineEnabled";
            this.OnlineEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.OnlineEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.OnlineEnabled.Width = 71;
            // 
            // UseCache
            // 
            this.UseCache.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.UseCache.HeaderText = "Use cache";
            this.UseCache.Name = "UseCache";
            this.UseCache.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.UseCache.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.UseCache.Width = 84;
            // 
            // ClearCache
            // 
            this.ClearCache.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ClearCache.HeaderText = "Clear cache";
            this.ClearCache.Name = "ClearCache";
            this.ClearCache.Width = 70;
            // 
            // localGroupBox
            // 
            this.localGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localGroupBox.Controls.Add(this.localGridView);
            this.localGroupBox.Controls.Add(this.addButton);
            this.localGroupBox.Controls.Add(this.removeButton);
            this.localGroupBox.Location = new System.Drawing.Point(413, 8);
            this.localGroupBox.Name = "localGroupBox";
            this.localGroupBox.Size = new System.Drawing.Size(263, 399);
            this.localGroupBox.TabIndex = 1;
            this.localGroupBox.TabStop = false;
            this.localGroupBox.Text = "Local galleries";
            // 
            // localGridView
            // 
            this.localGridView.AllowUserToAddRows = false;
            this.localGridView.AllowUserToDeleteRows = false;
            this.localGridView.AllowUserToResizeColumns = false;
            this.localGridView.AllowUserToResizeRows = false;
            this.localGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.localGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.localGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.localGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LocalName,
            this.LocalEnabled});
            this.localGridView.Location = new System.Drawing.Point(6, 19);
            this.localGridView.MultiSelect = false;
            this.localGridView.Name = "localGridView";
            this.localGridView.RowHeadersVisible = false;
            this.localGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.localGridView.Size = new System.Drawing.Size(251, 339);
            this.localGridView.TabIndex = 5;
            // 
            // LocalName
            // 
            this.LocalName.HeaderText = "Name";
            this.LocalName.Name = "LocalName";
            this.LocalName.ReadOnly = true;
            // 
            // LocalEnabled
            // 
            this.LocalEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LocalEnabled.HeaderText = "Enabled";
            this.LocalEnabled.Name = "LocalEnabled";
            this.LocalEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LocalEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.LocalEnabled.Width = 71;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(6, 364);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(109, 25);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "Add path...";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeButton.Enabled = false;
            this.removeButton.Location = new System.Drawing.Point(121, 364);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(136, 25);
            this.removeButton.TabIndex = 1;
            this.removeButton.Text = "Remove selection";
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.Location = new System.Drawing.Point(413, 413);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(263, 40);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // GalleryManagerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.localGroupBox);
            this.Controls.Add(this.onlineGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GalleryManagerView";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gallery Manager";
            this.onlineGroupBox.ResumeLayout(false);
            this.onlineGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.onlineGridView)).EndInit();
            this.localGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.localGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox onlineGroupBox;
        private System.Windows.Forms.DataGridView onlineGridView;
        private System.Windows.Forms.GroupBox localGroupBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.DataGridView localGridView;
        private System.Windows.Forms.Label descriptionHeaderLabel;
        private System.Windows.Forms.RichTextBox descriptionTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LocalEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn OnlineName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn OnlineEnabled;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UseCache;
        private System.Windows.Forms.DataGridViewButtonColumn ClearCache;
    }
}