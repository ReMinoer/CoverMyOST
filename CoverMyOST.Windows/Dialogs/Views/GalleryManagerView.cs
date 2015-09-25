using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CoverMyOST.Galleries;
using CoverMyOST.Galleries.Base;
using CoverMyOST.Windows.Sockets;
using FormPlug.WindowsForm;

namespace CoverMyOST.Windows.Dialogs.Views
{
    public partial class GalleryManagerView : Form
    {
        private const string ClearButtonText = "Clear";
        public event EventHandler<AddLocalGalleryRequestEventArgs> TryAddLocalGalleryRequest;
        public event EventHandler<RemoveLocalGalleryRequestEventArgs> RemoveLocalGalleryRequest;
        public event EventHandler<ChangeDescriptionRequestEventArgs> ChangeDescriptionRequest;
        public event EventHandler<EnabledChangedEventArgs> GalleryEnabledChanged;
        public event EventHandler<UseCacheChangedEventArgs> UseCacheChanged;
        public event EventHandler<ClearCacheRequestEventArgs> ClearCacheRequest;

        public GalleryManagerView(IEnumerable<OnlineGallery> onlineGalleries, IEnumerable<LocalGallery> localGalleries)
        {
            InitializeComponent();

            foreach (OnlineGallery onlineGallery in onlineGalleries)
                onlineGridView.Rows.Add(onlineGallery.Name, onlineGallery.Enabled, onlineGallery.UseCache, ClearButtonText);

            foreach (LocalGallery localGallery in localGalleries)
                AddToLocalGridView(localGallery);

            onlineGridView.SelectionChanged += OnlineGridViewOnSelectionChanged;
            onlineGridView.CellContentClick += OnlineGridViewOnCellContentClick;

            localGridView.SelectionChanged += LocalGridViewOnSelectionChanged;
            localGridView.CellContentClick += LocalGridViewOnCellContentClick;

            addButton.Click += AddButtonOnClick;
            removeButton.Click += RemoveButtonOnClick;
        }

        public void CompleteAddLocalGallery(LocalGallery localGallery)
        {
            AddToLocalGridView(localGallery);
        }

        public void FailAddLocalGallery(string name)
        {
            MessageBox.Show(@"A gallery with the same name exists already.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void CompleteChangeDescriptionRequest(string description)
        {
            descriptionTextBox.Text = description;
        }

        private void OnlineGridViewOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if (onlineGridView.SelectedRows.Count == 0)
                return;

            localGridView.ClearSelection();
            DescriptionSelectionChanged((string)onlineGridView.SelectedRows[0].Cells["OnlineName"].Value);
        }

        private void OnlineGridViewOnCellContentClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            onlineGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataGridViewRow row = onlineGridView.Rows[eventArgs.RowIndex];

            switch (onlineGridView.Columns[eventArgs.ColumnIndex].Name)
            {
                case "OnlineEnabled":

                    var enabledArgs = new EnabledChangedEventArgs() {
                        GalleryName = (string)row.Cells["OnlineName"].Value,
                        Enabled = (bool)row.Cells["OnlineEnabled"].Value
                    };

                    if (GalleryEnabledChanged != null)
                        GalleryEnabledChanged.Invoke(this, enabledArgs);

                    break;

                case "UseCache":
                    
                    var useCacheArgs = new UseCacheChangedEventArgs()
                    {
                        GalleryName = (string)row.Cells["OnlineName"].Value,
                        UseCache = (bool)row.Cells["UseCache"].Value
                    };

                    if (UseCacheChanged != null)
                        UseCacheChanged.Invoke(this, useCacheArgs);

                    break;

                case "ClearCache":

                    var claerCacheArgs = new ClearCacheRequestEventArgs {
                        GalleryName = (string)row.Cells["OnlineName"].Value
                    };

                    string message = string.Format(@"Are you sure you want to clear the cache of the {0} ?", claerCacheArgs.GalleryName);
                    DialogResult dialogResult = MessageBox.Show(message, @"Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult != DialogResult.Yes)
                        return;

                    if (ClearCacheRequest != null)
                        ClearCacheRequest.Invoke(this, claerCacheArgs);

                    break;
            }
        }

        private void LocalGridViewOnCellContentClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            localGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DataGridViewRow row = localGridView.Rows[eventArgs.RowIndex];

            switch (localGridView.Columns[eventArgs.ColumnIndex].Name)
            {
                case "LocalEnabled":

                    var enabledArgs = new EnabledChangedEventArgs {
                        GalleryName = (string)row.Cells["LocalName"].Value,
                        Enabled = (bool)row.Cells["LocalEnabled"].Value
                    };

                    if (GalleryEnabledChanged != null)
                        GalleryEnabledChanged.Invoke(this, enabledArgs);

                    break;
            }
        }

        private void LocalGridViewOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            if (localGridView.SelectedRows.Count == 0)
            {
                removeButton.Enabled = false;
                return;
            }

            onlineGridView.ClearSelection();
            DescriptionSelectionChanged((string)localGridView.SelectedRows[0].Cells["LocalName"].Value);
            removeButton.Enabled = true;
        }

        private void DescriptionSelectionChanged(string galleryName)
        {
            var args = new ChangeDescriptionRequestEventArgs {
                GalleryName = galleryName
            };

            descriptionHeaderLabel.Text = galleryName;

            if (ChangeDescriptionRequest != null)
                ChangeDescriptionRequest.Invoke(this, args);
        }

        private void AddButtonOnClick(object sender, EventArgs eventArgs)
        {
            var localGalleryCreator = new LocalGalleryCreator();

            var form = new LocalGalleryCreatorView(localGalleryCreator);
            DialogResult dialogResult = form.ShowDialog();

            if (dialogResult != DialogResult.OK)
                return;

            var args = new AddLocalGalleryRequestEventArgs
            {
                Gallery = localGalleryCreator.Create()
            };

            if (TryAddLocalGalleryRequest != null)
                TryAddLocalGalleryRequest.Invoke(this, args);
        }

        private void RemoveButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (localGridView.SelectedRows.Count == 0)
                throw new InvalidOperationException();

            DataGridViewRow row = localGridView.SelectedRows[0];

            var args = new RemoveLocalGalleryRequestEventArgs {
                GalleryName = (string)row.Cells["LocalName"].Value
            };

            localGridView.Rows.Remove(row);

            if (RemoveLocalGalleryRequest != null)
                RemoveLocalGalleryRequest.Invoke(this, args);
        }

        private void AddToLocalGridView(ICoversGallery localGallery)
        {
            localGridView.Rows.Add(localGallery.Name, localGallery.Enabled);
        }

        public class AddLocalGalleryRequestEventArgs : EventArgs
        {
            public LocalGallery Gallery { get; set; }
        }

        public class RemoveLocalGalleryRequestEventArgs : EventArgs
        {
            public string GalleryName { get; set; }
        }

        public class ChangeDescriptionRequestEventArgs : EventArgs
        {
            public string GalleryName { get; set; }
        }

        public class EnabledChangedEventArgs : EventArgs
        {
            public string GalleryName { get; set; }
            public bool Enabled { get; set; }
        }

        public class UseCacheChangedEventArgs : EventArgs
        {
            public string GalleryName { get; set; }
            public bool UseCache { get; set; }
        }

        public class ClearCacheRequestEventArgs : EventArgs
        {
            public string GalleryName { get; set; }
        }
    }
}