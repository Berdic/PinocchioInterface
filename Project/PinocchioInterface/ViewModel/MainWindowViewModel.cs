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

        #region Properties
        private ObservableCollection<RiggingModel> _riggingModels = new ObservableCollection<RiggingModel>();
        /// <summary>
        /// Mesh models which will be rigged and they are showed in list box.
        /// </summary>
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
        /// <summary>
        /// Text inside of the textbox for potential path.
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

        private RiggingModel _selectedRiggingModel;
        /// <summary>
        /// Currently selected item in list of mesh models.
        /// </summary>
        public RiggingModel SelectedRiggingModel
        {
            get { return _selectedRiggingModel; }
            set
            {
                _selectedRiggingModel = value;
                NotifyPropertyChanged("SelectedRiggingModel");
            }
        }


        #endregion

        #region Functions
        /// <summary>
        /// Starts autorigging of all models.
        /// </summary>
        public void AutoRig()
        {
            string pinocchioPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Pinocchio", "DemoUI.exe");
            string motionFolder = System.IO.Path.Combine("Pinocchio", "motion");

            foreach (RiggingModel model in RiggingModels)
                Process.Start(pinocchioPath, model.GetCommandLineArguments(motionFolder));
        }

        /// <summary>
        /// Add new mesh model for rigging
        /// </summary>
        /// <param name="path">Path to the model, if it is not set, it will add from ModelPath</param>
        public void AddModel(string path = null)
        {
            if (path == null)
                RiggingModels.Add(new RiggingModel(ModelPath));
            else
                RiggingModels.Add(new RiggingModel(path));
        }

        /// <summary>
        /// Removes specified model from list of models for rigging
        /// </summary>
        /// <param name="model"></param>
        public void RemoveModel(RiggingModel model)
        {
            RiggingModels.Remove(model);
        }

        /// <summary>
        /// Checks if model is already inside of the list
        /// </summary>
        /// <param name="path">If not specified it checks if model from ModelPath is already in the list, else it cheks if the model from the specified path is already in the list</param>
        /// <returns>If mesh model on specified path or ModelPath is inside of the list</returns>
        public bool IsAlreadyInList(string path = null)
        {
            if (path == null)
                return RiggingModels.Any(x => x.Path == ModelPath);
            else
                return RiggingModels.Any(x => x.Path == path);
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
