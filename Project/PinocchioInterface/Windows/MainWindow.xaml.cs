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
using PinocchioInterface.Classes;
using System.Runtime.InteropServices;

namespace PinocchioInterface.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainWindowViewModel)DataContext;
        }
        

        private void tbModelPath_KeyUp(object sender, KeyEventArgs e)
        {
            //If pressed key is enter, there are no errors in file path and the model is not in the list, add model
            if (e.Key == Key.Enter && !Validation.GetHasError(sender as DependencyObject) && !_viewModel.IsAlreadyInList())
                _viewModel.AddModel();
        }
        
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var riggingModel = button.DataContext;
            _viewModel.RemoveModel((RiggingModel)riggingModel);
        }
        
        
    }
}
