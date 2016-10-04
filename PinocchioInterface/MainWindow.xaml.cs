﻿using System;
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
        private MainWindowViewModel _dataContext;
        public MainWindow()
        {

            InitializeComponent();

            _dataContext = (MainWindowViewModel)DataContext;
        }



        private void btnAutorig_Click(object sender, RoutedEventArgs e)
        {
            string pinocchioPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Pinocchio", "DemoUI.exe");

            string motionFolder = System.IO.Path.Combine("Pinocchio", "motion");

            _dataContext.AutoRig(pinocchioPath, motionFolder);
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = CreateFileDialogForModelSelection();

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string selectedPath in openFileDialog.FileNames)
                {
                    _dataContext.AddModel(selectedPath);
                }
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dataContext.RemoveModel((RiggingModel)lbModels.SelectedItem);
        }

        private void tbModelPath_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Validation.GetHasError(sender as DependencyObject) && !_dataContext.AlreadyInList())
                _dataContext.AddModel();
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
    }
}
