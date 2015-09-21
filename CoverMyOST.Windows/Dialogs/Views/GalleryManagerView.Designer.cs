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
            this.onlineGroupBox = new System.Windows.Forms.GroupBox();
            this.localGroupBox = new System.Windows.Forms.GroupBox();
            this.okButton = new System.Windows.Forms.Button();
            this.onlineGridView = new System.Windows.Forms.DataGridView();
            this.localList = new System.Windows.Forms.ListBox();
            this.removeButton = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.folderButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.pathLabel = new System.Windows.Forms.Label();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EnabledColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CacheColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ClearCacheColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.onlineGroupBox.SuspendLayout();
            this.localGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.onlineGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // onlineGroupBox
            // 
            this.onlineGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.onlineGroupBox.Controls.Add(this.onlineGridView);
            this.onlineGroupBox.Location = new System.Drawing.Point(9, 9);
            this.onlineGroupBox.Name = "onlineGroupBox";
            this.onlineGroupBox.Padding = new System.Windows.Forms.Padding(7);
            this.onlineGroupBox.Size = new System.Drawing.Size(426, 444);
            this.onlineGroupBox.TabIndex = 0;
            this.onlineGroupBox.TabStop = false;
            this.onlineGroupBox.Text = "Online galleries";
            // 
            // localGroupBox
            // 
            this.localGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localGroupBox.Controls.Add(this.pathLabel);
            this.localGroupBox.Controls.Add(this.addButton);
            this.localGroupBox.Controls.Add(this.folderButton);
            this.localGroupBox.Controls.Add(this.pathTextBox);
            this.localGroupBox.Controls.Add(this.removeButton);
            this.localGroupBox.Controls.Add(this.localList);
            this.localGroupBox.Location = new System.Drawing.Point(441, 8);
            this.localGroupBox.Name = "localGroupBox";
            this.localGroupBox.Size = new System.Drawing.Size(235, 399);
            this.localGroupBox.TabIndex = 1;
            this.localGroupBox.TabStop = false;
            this.localGroupBox.Text = "Local galleries";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(441, 413);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(235, 40);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
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
            this.NameColumn,
            this.EnabledColumn,
            this.CacheColumn,
            this.ClearCacheColumn});
            this.onlineGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.onlineGridView.Location = new System.Drawing.Point(7, 20);
            this.onlineGridView.MultiSelect = false;
            this.onlineGridView.Name = "onlineGridView";
            this.onlineGridView.RowHeadersVisible = false;
            this.onlineGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.onlineGridView.Size = new System.Drawing.Size(412, 417);
            this.onlineGridView.TabIndex = 0;
            // 
            // localList
            // 
            this.localList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localList.FormattingEnabled = true;
            this.localList.Location = new System.Drawing.Point(6, 74);
            this.localList.Name = "localList";
            this.localList.Size = new System.Drawing.Size(223, 316);
            this.localList.TabIndex = 0;
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(6, 44);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(108, 23);
            this.removeButton.TabIndex = 1;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(47, 18);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(143, 20);
            this.pathTextBox.TabIndex = 2;
            // 
            // folderButton
            // 
            this.folderButton.Location = new System.Drawing.Point(196, 16);
            this.folderButton.Name = "folderButton";
            this.folderButton.Size = new System.Drawing.Size(33, 23);
            this.folderButton.TabIndex = 3;
            this.folderButton.Text = "...";
            this.folderButton.UseVisualStyleBackColor = true;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(120, 44);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(109, 23);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(6, 21);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(35, 13);
            this.pathLabel.TabIndex = 5;
            this.pathLabel.Text = "Path :";
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // EnabledColumn
            // 
            this.EnabledColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.EnabledColumn.HeaderText = "Enabled";
            this.EnabledColumn.Name = "EnabledColumn";
            this.EnabledColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.EnabledColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.EnabledColumn.Width = 71;
            // 
            // CacheColumn
            // 
            this.CacheColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CacheColumn.HeaderText = "Use cache";
            this.CacheColumn.Name = "CacheColumn";
            this.CacheColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CacheColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CacheColumn.Width = 84;
            // 
            // ClearCacheColumn
            // 
            this.ClearCacheColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ClearCacheColumn.HeaderText = "Clear cache";
            this.ClearCacheColumn.Name = "ClearCacheColumn";
            this.ClearCacheColumn.Width = 70;
            // 
            // GalleryManagerView
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.localGroupBox);
            this.Controls.Add(this.onlineGroupBox);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "GalleryManagerView";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gallery Manager";
            this.onlineGroupBox.ResumeLayout(false);
            this.localGroupBox.ResumeLayout(false);
            this.localGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.onlineGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox onlineGroupBox;
        private System.Windows.Forms.DataGridView onlineGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EnabledColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CacheColumn;
        private System.Windows.Forms.DataGridViewButtonColumn ClearCacheColumn;
        private System.Windows.Forms.GroupBox localGroupBox;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button folderButton;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.ListBox localList;
        private System.Windows.Forms.Button okButton;
    }
}