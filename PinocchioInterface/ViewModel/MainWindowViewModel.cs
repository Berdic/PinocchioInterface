using PinocchioInterface.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace PinocchioInterface.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties

        private MainWindow _model;

        public MainWindow Model
        {
            get { return (MainWindow)App.Current.MainWindow; }
        }


        //private ObservableCollection<RiggingModel> _riggingModels = new ObservableCollection<RiggingModel>();
        /// <summary>
        /// Rigging models which are shown in the list.
        /// </summary>
        public ObservableCollection<RiggingModel> RiggingModels
        {
            get
            {
                return Model.RiggingModels;
            }

            set
            {
                Model.RiggingModels = value;

                NotifyPropertyChanged("RiggingModels");
            }
        }

        private string _modelPath = "";

        /// <summary>
        /// Manually entered path
        /// </summary>
        public string ModelPath
        {
            get { return _modelPath; }
            set
            {
                _modelPath = value;
                NotifyPropertyChanged("ModelPath");
            }
        }

        #endregion

        #region Commands

        private ICommand _commandAutoRig = null;

        public ICommand CommandAutoRig
        {
            get
            {
                if (_commandAutoRig == null)
                {
                    _commandAutoRig = new RelayCommand(
                        action => Model.RiggingModels.Count > 0,
                        action => Model.AutoRig());
                }
                return _commandAutoRig;
            }
        }


        #endregion



        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

}
