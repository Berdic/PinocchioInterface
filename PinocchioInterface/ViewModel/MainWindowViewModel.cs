using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinocchioInterface.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<RiggingModel> _riggingModels = new ObservableCollection<RiggingModel>();
        public ObservableCollection<RiggingModel> RiggingModels
        {
            get
            {
                return _riggingModels;
            }

            set
            {
                _riggingModels = value;
                NotifyPropertyChanged("RiggingModels");
            }
        }

        private string _modelPath = "";

        public string ModelPath
        {
            get { return _modelPath; }
            set
            {
                _modelPath = value;
                NotifyPropertyChanged("ModelPath");
            }
        }


        public void AutoRig(string pinocchioPath, string motionFolder)
        {
            foreach (RiggingModel model in RiggingModels)
            {
                Process.Start(pinocchioPath, model.GetCommandLineArguments(motionFolder));
            }
        }

        public void AddModel(string path = null)
        {
            if (path == null)
                RiggingModels.Add(new RiggingModel(ModelPath));
            else
                RiggingModels.Add(new RiggingModel(path));
        }

        public void RemoveModel(RiggingModel model)
        {
            RiggingModels.Remove(model);
        }

        public bool AlreadyInList()
        {
            return RiggingModels.Any(x => x.Path == ModelPath);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
