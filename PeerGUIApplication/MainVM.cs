using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using P2pModels.managers;
using P2pModels.model;
using PeerGUIApplication.Annotations;

namespace PeerGUIApplication
{
    public class MainVM : INotifyPropertyChanged
    {
        private PeerClient pclient = new PeerClient();

        public MainVM()
        {
            _fileEPs = new ObservableCollection<FileEndPoint>();
            _searchCommand = new RelayCommand(Search);
            _downloadCommand = new RelayCommand(Download);
            _enableDownload = false;
        }

        private ObservableCollection<FileEndPoint> _fileEPs;
        private FileEndPoint _selectedEndPoint;
        private RelayCommand _searchCommand;
        private RelayCommand _downloadCommand;
        private String _filename;
        private bool _enableDownload;
        private String _message;

        public RelayCommand SearchCommand => _searchCommand;

        public RelayCommand DownloadCommand => _downloadCommand;

        public ObservableCollection<FileEndPoint> FileEPs => _fileEPs;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public bool EnableDownload
        {
            get => _enableDownload;
            set
            {
                _enableDownload = value;
                OnPropertyChanged();
            }
        }

        public string Filename
        {
            get => _filename;
            set
            {
                _filename = value;
                Message = "";
                OnPropertyChanged();
            }
        }



        public FileEndPoint SelectedEndPoint
        {
            get { return _selectedEndPoint; }
            set
            {
                _selectedEndPoint = value;
                if (value != null)
                {
                    EnableDownload = true;
                }
                OnPropertyChanged();
            }
        }




        public void Search()
        {
            List<FileEndPoint> endPoints = pclient.Search(_filename);

            _fileEPs.Clear();
            foreach (FileEndPoint endPoint in endPoints)
            {
                _fileEPs.Add(endPoint);
            }
        }

        public void Download()
        {
            try
            {
                pclient.Download(_filename, _selectedEndPoint, _filename);
                Message = "Er downloaded";
            }
            catch (Exception ex)
            {
                Message = "Ikke downloaded - net problemer";
            }
            EnableDownload = false;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
