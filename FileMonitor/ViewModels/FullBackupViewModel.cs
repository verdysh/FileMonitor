using Services.Dto;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace FileMonitor.ViewModels
{
    public class FullBackupViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FullBackupDto> _backupPaths;
        private bool _backupSelected;
        public event PropertyChangedEventHandler? PropertyChanged;

        public FullBackupViewModel(ObservableCollection<FullBackupDto> collection)
        {
            _backupPaths = collection;
            _backupSelected = IsAnyBackupSelected();

            BackupPaths.CollectionChanged += BackupPaths_CollectionChanged;
        }

        private void BackupPaths_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SetBackupWidth();
        }

        public void SetBackupWidth()
        {
            int maxWidth = 1;
            foreach (FullBackupDto backup in _backupPaths)
                maxWidth = Math.Max(maxWidth, backup.Path.Length);

            BackupWidth = maxWidth;
        }

        public ObservableCollection<FullBackupDto> BackupPaths
        {
            get { return _backupPaths; }
            set
            {
                _backupPaths = value;
            }
        }

        public bool BackupSelected
        {
            get { return _backupSelected; }
            set 
            {
                _backupSelected = value;
                OnPropertyChanged(nameof(BackupSelected));
            }
        }

        private int _backupWidth;
        public int BackupWidth
        {
            set
            {
                _backupWidth = value * 7;
                OnPropertyChanged(nameof(BackupWidth));
            }
            get => _backupWidth;
        }

        public void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsAnyBackupSelected()
        {
            foreach (FullBackupDto dto in  _backupPaths)
            {
                if (dto.IsSelected == true) return true;
            }
            return false;
        }
    }
}
