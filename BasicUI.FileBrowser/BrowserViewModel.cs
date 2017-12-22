using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BasicUI.FileBrowser
{
    public enum Sorting
    {
        Default,
        Name,
        Size,
        Modified,
        Type
    }

    public class BrowserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _selectedPath = "";
        public string SelectedPath
        {
            get => _selectedPath;
            set
            {
                _selectedPath = value;
                OnPropertyChanged(nameof(SelectedPath));
            }
        }

        private bool _openFilePromptShown = false;
        public bool OpenFilePromptShown
        {
            get => _openFilePromptShown;
            set
            {
                _openFilePromptShown = value;
                OnPropertyChanged(nameof(OpenFilePromptShown));
            }
        }

        private string _currentDirectory = "";
        public string CurrentDirectory
        {
            get => _currentDirectory;
            set
            {
                if (value != _currentDirectory)
                {
                    RefreshFiles(value);
                    _currentDirectory = value;

                    OnPropertyChanged(nameof(Files));
                }
            }
        }

        private Sorting _sorting = Sorting.Default;
        public Sorting Sorting
        {
            get => _sorting;
            set
            {
                _sorting = value;
                RefreshFiles();

                OnPropertyChanged(nameof(Files));
                OnPropertyChanged(nameof(Sorting));
            }
        }

        private bool _animateLabels = true;
        public bool AnimateLabels
        {
            get => _animateLabels;
            set
            {
                _animateLabels = value;
                OnPropertyChanged(nameof(AnimateLabels));
            }
        }

        private List<string> _files = new List<string>();
        public IEnumerable<string> Files => _files.AsReadOnly();

        public void RefreshFiles(string path = null)
        {
            if (path == null) { path = CurrentDirectory; }

            _files.Clear();

            IEnumerable<string> sortedFiles = new List<string>(Directory.GetFiles(path));

            if (Sorting != Sorting.Default)
            {
                switch (Sorting)
                {
                    case Sorting.Name:
                        sortedFiles = sortedFiles.OrderBy(d => Path.GetFileName(d));
                        break;
                    case Sorting.Size:
                        sortedFiles = sortedFiles.OrderBy(d => new FileInfo(d).Length);
                        break;
                    case Sorting.Modified:
                        sortedFiles = sortedFiles.OrderBy(d => new FileInfo(d).LastWriteTime);
                        break;
                    case Sorting.Type:
                        sortedFiles = sortedFiles.OrderBy(d => Path.GetExtension(d));
                        break;
                }
            }

            _files.AddRange(Directory.GetDirectories(path));
            _files.AddRange(sortedFiles);

            OnPropertyChanged(nameof(Files));
        }

        public void NavigateToSelected()
        {
            CurrentDirectory = SelectedPath;
        }

        //TODO: Make this crossplat
        public void OpenSelected()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = SelectedPath
            });
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        internal void NavigateUpDirectory()
        {
            var up = Directory.GetParent(CurrentDirectory)?.FullName;

            if (up != null)
            {
                CurrentDirectory = up;
                OnPropertyChanged(nameof(CurrentDirectory));
            }
        }
    }
}
