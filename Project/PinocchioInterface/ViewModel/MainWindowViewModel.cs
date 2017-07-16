using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using PinocchioInterface.Classes;
using PinocchioInterface.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace PinocchioInterface.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //holds value of previous phase in rigging process
        private int _previousePhase;

        //cancels autorigging process
        private CancellationTokenSource _cts;

        #region Commands

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if(_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(p =>
                    {
                        RiggingProgressStatus = "Canceling process. Closing process safely...";
                        IsCancelEnabled = false;
                        CancelProcess();
                        if (_cts != null)
                            _cts.Cancel();
                    });
                }

                return _cancelCommand;
            }
        }


        private ICommand _autoRigCommand;

        public ICommand AutoRigCommand
        {
            get
            {
                if(_autoRigCommand == null)
                {
                    _autoRigCommand = new RelayCommand(p => 
                    {
                        AutoRiggingProcess();
                    });
                }

                return _autoRigCommand;
            }
        }

        private ICommand _browseCommand;

        public ICommand BrowseCommand
        {
            get
            {
                if(_browseCommand == null)
                {
                    _browseCommand = new RelayCommand(p => 
                    {
                        Browse();
                    });
                }

                return _browseCommand;
            }
        }

        #endregion

        #region Properties

        private bool _showProgressBar;

        public bool ShowProgressBar
        {
            get { return _showProgressBar; }
            set { _showProgressBar = value; NotifyPropertyChanged("ShowProgressBar"); }
        }


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


        private string _riggingProgressStatus;

        public string RiggingProgressStatus
        {
            get { return _riggingProgressStatus; }
            set { _riggingProgressStatus = value; NotifyPropertyChanged("RiggingProgressStatus"); }
        }

        private double _riggingProgressValue;

        public double RiggingProgressValue
        {
            get { return _riggingProgressValue; }
            set { _riggingProgressValue = value; NotifyPropertyChanged("RiggingProgressValue"); }
        }

        private bool _isCancelEnabled;

        public bool IsCancelEnabled
        {
            get { return _isCancelEnabled; }
            set { _isCancelEnabled = value; NotifyPropertyChanged("IsCancelEnabled"); }
        }


        #endregion

        #region Functions

        /// <summary>
        /// Visualizes rigged joints.
        /// </summary>
        private void VisualizeJoints()
        {
            //double heightX = Math.Abs(SelectedRiggingModel.Joints.Max(x => x.X) - SelectedRiggingModel.Joints.Min(x => x.X));
            //double heightY = Math.Abs(SelectedRiggingModel.Joints.Max(x => x.Y) - SelectedRiggingModel.Joints.Min(x => x.Y));
            //double heightZ = Math.Abs(SelectedRiggingModel.Joints.Max(x => x.Z) - SelectedRiggingModel.Joints.Min(x => x.Z));


            //SelectedRiggingModel.Joints.ForEach(dot =>
            //{
            //    Point3D point = new Point3D(
            //        dot.X - (SelectedRiggingModel.Joints.Min(x => x.X) + heightX/2), 
            //        dot.Y - (SelectedRiggingModel.Joints.Min(x => x.Y) + heightY / 2), 
            //        dot.Z - (SelectedRiggingModel.Joints.Min(x => x.Z) + heightZ / 2));

            //    SelectedRiggingModel.VisualJoints.Add(point);
            //});

            SelectedRiggingModel.Joints.ForEach(dot =>
            {
                Point3D point = new Point3D(
                    dot.X,
                    dot.Y,
                    dot.Z);

                SelectedRiggingModel.VisualJoints.Add(point);
            });



        }

        /// <summary>
        /// Browse for new models 
        /// </summary>
        private void Browse()
        {
            OpenFileDialog openFileDialog = CreateFileDialogForModelSelection();

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string selectedPath in openFileDialog.FileNames)
                {
                    if (!IsAlreadyInList(selectedPath))
                        AddModel(selectedPath);
                }
            }
        }

        /// <summary>
        /// Start whole auto rigging process
        /// </summary>
        public async void AutoRiggingProcess()
        {
            IsCancelEnabled = true;

            //restart progress status and value
            RestartProgress();

            //show progress bar
            ShowProgressBar = true;

            //start autorigging process
            await AutoRig();

            //check for generated joints, if any, show them
            if (SelectedRiggingModel.Joints.Count > 0)
                VisualizeJoints();

            //hid progress bar
            ShowProgressBar = false;
        }
        
        /// <summary>
        /// Starts autorigging of all models.
        /// </summary>
        public async Task AutoRig()
        {
            Task task = Task.Run(() => BackgroundAutoRigging(), _cts.Token);
            
            await Task.Factory.StartNew(() =>
            {
                while (!task.IsCompleted && !task.IsCanceled)
                {
                    Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        UpdateProgress(GetProgressUpdate());
                        Thread.Sleep(100);
                    });
                }
            });
            
        }

        private void RestartProgress()
        {
            //Reset process in Pinocchio library
            ResetProcess();
            //setting status
            RiggingProgressStatus = "Configuration...";
            //setting progress bar to 0
            RiggingProgressValue = 0;
            //reinitializing cancellation token
            _cts = new CancellationTokenSource();
        }
        
        /// <summary>
        /// Updates status of progress depending on phase
        /// </summary>
        /// <param name="phase">Phase returned from C++</param>
        private void UpdateProgress(int phase)
        {
            //if previous phase is the same as this one, don't update progress
            if (_previousePhase == phase)
                return;

            switch(phase)
            {
                case 1:
                    OvertimeProgressUpdate(8);
                    RiggingProgressStatus = "Configuration...";
                    break;
                case 2:
                    OvertimeProgressUpdate(6);
                    RiggingProgressStatus = "Mesh preparing...";
                    break;
                case 3:
                    OvertimeProgressUpdate(4);
                    RiggingProgressStatus = "Construction of distance field...";
                    break;
                case 4:
                    OvertimeProgressUpdate(70);
                    RiggingProgressStatus = "Discretization...";
                    break;
                case 5:
                    OvertimeProgressUpdate(60);
                    RiggingProgressStatus = "Sphere packing phase...";
                    break;
                case 6:
                    OvertimeProgressUpdate(4);
                    RiggingProgressStatus = "Connecting samples...";
                    break;
                case 7:
                    OvertimeProgressUpdate(10);
                    RiggingProgressStatus = "Computing possibilities...";
                    break;
                case 8:
                    OvertimeProgressUpdate(4);
                    RiggingProgressStatus = "Discrete embedding...";
                    break;
                case 9:
                    OvertimeProgressUpdate(10);
                    RiggingProgressStatus = "Path splitting...";
                    break;
                case 10:
                    OvertimeProgressUpdate(10);
                    RiggingProgressStatus = "Medial surface...";
                    break;
                case 11:
                    OvertimeProgressUpdate(4);
                    RiggingProgressStatus = "Refine embedding...";
                    break;
                case 12:
                    OvertimeProgressUpdate(10);
                    RiggingProgressStatus = "Finishing...";
                    break;
                case -1:
                    RiggingProgressStatus = "Error. " + RiggingProgressStatus;
                    break;
                case -2:
                    //fully exited
                    ShowProgressBar = false;
                    break;
            }


            _previousePhase = phase;
        }

        private void OvertimeProgressUpdate(int value)
        {
            for(int i = 1; i <= value; i++)
            {
                RiggingProgressValue += 1;
                Thread.Sleep(10);
            }
        }

        private async Task BackgroundAutoRigging()
        {
            byte[] bytes = Encoding.ASCII.GetBytes(SelectedRiggingModel.Path);
            
            unsafe
            {
                fixed (byte* p = bytes)
                {
                    //converting bytes to signed bytes
                    sbyte* sp = (sbyte*)p;

                    //reading generated array of x,y,z values
                    double** items;
                    int itemsCount;
                    using (GenerateItemsWrapper(out items, out itemsCount, sp))
                    {
                        if (_cts.IsCancellationRequested)
                            return;

                        for (int i = 0; i < itemsCount; i++)
                        {
                            SelectedRiggingModel.Joints.Add(new Joint
                            {
                                X = items[i][0],
                                Y = items[i][1],
                                Z = items[i][2]
                            });

                        }
                    }
                }
            }

            await Task.Delay(10);
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

        /// <summary>
        /// Creates desired FileDialog for browsing models.
        /// </summary>
        /// <returns>OpenFileDialog</returns>
        private OpenFileDialog CreateFileDialogForModelSelection()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Choose model for autorigging";
            openFileDialog.Filter = "OBJ files (*.obj)|*.obj|PLY files (*.ply)|*.ply|OFF files (*.off)|*.off|GTS files (*.gts)|*.gts|STL files (*.stl)|*.stl";
            openFileDialog.InitialDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Pinocchio");
            openFileDialog.Multiselect = true;

            return openFileDialog;
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

        
        #region C++ Wrapper


        [DllImport("Pinocchio.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        static unsafe extern bool GenerateItems(out ItemsSafeHandle itemsHandle, out double** items, out int itemCount, sbyte* file);

        [DllImport("Pinocchio.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        static unsafe extern bool ReleaseItems(IntPtr itemsHandle);

        [DllImport("Pinocchio.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        static unsafe extern int GetProgressUpdate();

        [DllImport("Pinocchio.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        static unsafe extern void CancelProcess();

        [DllImport("Pinocchio.dll", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        static unsafe extern void ResetProcess();

        static unsafe ItemsSafeHandle GenerateItemsWrapper(out double** items, out int itemsCount, sbyte* file)
        {
            ItemsSafeHandle itemsHandle;
            if (!GenerateItems(out itemsHandle, out items, out itemsCount, file))
            {
                throw new InvalidOperationException();
            }
            return itemsHandle;
        }


        class ItemsSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public ItemsSafeHandle(): base(true)
            {
            }

            protected override bool ReleaseHandle()
            {
                return ReleaseItems(handle);
            }
        }

        
        #endregion
    }
}
