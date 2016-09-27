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



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void btnAutorig_Click(object sender, RoutedEventArgs e)
        {
            //if (ModelPath != "" && ModelPath != null)
            //{

            //    string pinocchioPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Pinocchio", "DemoUI.exe");
            //    string[] parameters = new string[6];
            //    parameters[0] = "\"" + ModelPath + "\"";
            //    parameters[1] = "-rot 1 0 0 " + XRot;
            //    parameters[2] = "-rot 0 1 0 " + YRot;
            //    parameters[3] = "-rot 0 0 1 " + ZRot;
            //    parameters[4] = "-scale " + ScaleFactor;


            //    string motionFolder = System.IO.Path.Combine("Pinocchio", "motion");

            //    if ((bool)rbJump.IsChecked)
            //        parameters[4] = "-motion " + System.IO.Path.Combine(motionFolder, "jumpAround.txt");
            //    else if ((bool)rbRun.IsChecked)
            //        parameters[4] = "-motion " + System.IO.Path.Combine(motionFolder, "runAround.txt");
            //    else if ((bool)rbWalk.IsChecked)
            //        parameters[4] = "-motion " + System.IO.Path.Combine(motionFolder, "walk.txt");

            //    string wholeString = String.Join(" ", parameters);
            //    Process.Start(pinocchioPath, String.Join(" ", parameters));
            //}
            //else
            //    System.Windows.Forms.MessageBox.Show("Please eneter or choose model to autorig");

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose model for autorigging";
            openFileDialog.Filter = "OBJ files (*.obj)|*.obj";
            openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            if (openFileDialog.ShowDialog() == true)
            {
                ModelPath = openFileDialog.FileName;
                RiggingModels.Add(new RiggingModel(ModelPath));
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RiggingModels.Remove((RiggingModel)lbModels.SelectedItem);
        }

        private void tbModelPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Validation.GetHasError(sender as DependencyObject) && !RiggingModels.Any(x=>x.Path == ModelPath))
                RiggingModels.Add(new RiggingModel(ModelPath));
        }
    }
}
