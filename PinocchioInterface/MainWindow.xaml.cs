using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using PinocchioInterface.ViewModel;
using MahApps.Metro.Controls;


namespace PinocchioInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            RiggingModels = new ObservableCollection<RiggingModel>();
        }

        public ObservableCollection<RiggingModel> RiggingModels { get; set; }

        private void btnAutorig_Click(object sender, RoutedEventArgs e)
        {
            AutoRig();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
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

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RemoveModel((RiggingModel)lbModels.SelectedItem);
        }

        private void tbModelPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Validation.GetHasError(sender as DependencyObject) && !IsAlreadyInList())
                AddModel();
        }

        private OpenFileDialog CreateFileDialogForModelSelection()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Choose model for autorigging";
            openFileDialog.Filter = "OBJ files (*.obj)|*.obj|PLY files (*.ply)|*.ply|OFF files (*.off)|*.off|GTS files (*.gts)|*.gts|STL files (*.stl)|*.stl";
            openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            openFileDialog.Multiselect = true;

            return openFileDialog;
        }

        #region Functions

        /// <summary>
        /// Starts autorigging of models in the list
        /// </summary>
        /// <param name="pinocchioPath">Path to the Pinocchio DemoUI.exe</param>
        /// <param name="motionFolder">Path to the folder which contains motions</param>
        public void AutoRig()
        {
            string pinocchioPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Pinocchio", "DemoUI.exe");

            string motionFolder = System.IO.Path.Combine("Pinocchio", "motion");

            foreach (RiggingModel model in RiggingModels)
            {
                Process.Start(pinocchioPath, model.GetCommandLineArguments(motionFolder));
            }
        }

        /// <summary>
        /// Adds new Rigging Model to the list.
        /// </summary>
        /// <param name="path">Path to the mesh model</param>
        public void AddModel(string path = null)
        {
            if (path == null)
                RiggingModels.Add(new RiggingModel(tbModelPath.Text));
            else
                RiggingModels.Add(new RiggingModel(path));
        }

        /// <summary>
        /// Removes specified Rigging Model object from the list
        /// </summary>
        /// <param name="model">Specific model</param>
        public void RemoveModel(RiggingModel model)
        {
            RiggingModels.Remove(model);
        }

        /// <summary>
        /// Checks if model on specified path is in the rigging list
        /// </summary>
        /// <param name="path">Path of the mesh model</param>
        /// <returns>True if it is in the rigging list, false if it is not in the rigging list</returns>
        public bool IsAlreadyInList(string path = null)
        {
            if (path == null)
                return RiggingModels.Any(x => x.Path == tbModelPath.Text);
            else
                return RiggingModels.Any(x => x.Path == path);
        }
        #endregion
    }
}
