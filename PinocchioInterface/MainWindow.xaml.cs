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

namespace PinocchioInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {

            InitializeComponent();
            DataContext = this;
        }


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





        private void btnAutorig_Click(object sender, RoutedEventArgs e)
        {
            string pinocchioPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Pinocchio", "DemoUI.exe");

            string motionFolder = System.IO.Path.Combine("Pinocchio", "motion");

            foreach (RiggingModel model in RiggingModels)
            {
                Process.Start(pinocchioPath, model.GetCommandLineArguments(motionFolder));
            }

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose model for autorigging";
            openFileDialog.Filter = "OBJ files (*.obj)|*.obj | PLY files (*.ply)|*.ply | OFF files (*.off)|*.off | GTS files (*.gts)|*.gts | STL files (*.stl)|*.stl";
            openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string selectedPath in openFileDialog.FileNames)
                {
                    RiggingModels.Add(new RiggingModel(selectedPath));
                }

            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RiggingModels.Remove((RiggingModel)lbModels.SelectedItem);
        }

        private void tbModelPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Validation.GetHasError(sender as DependencyObject) && !RiggingModels.Any(x => x.Path == ModelPath))
                RiggingModels.Add(new RiggingModel(ModelPath));
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
