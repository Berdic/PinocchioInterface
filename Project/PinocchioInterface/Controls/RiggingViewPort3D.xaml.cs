using PinocchioInterface.ViewModel;
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
using System.Windows.Media.Media3D;

namespace PinocchioInterface.Controls
{
    /// <summary>
    /// Interaction logic for RiggingViewPort3D.xaml
    /// </summary>
    public partial class RiggingViewPort3D : UserControl
    {
        
        public RiggingViewPort3D()
        {
            InitializeComponent();
        }
        

        public RiggingModel RiggingModel
        {
            get { return (RiggingModel)GetValue(RiggingModelProperty); }
            set
            {
                SetValue(RiggingModelProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for RiggingModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RiggingModelProperty =
            DependencyProperty.Register("RiggingModel", typeof(RiggingModel), typeof(RiggingViewPort3D), new PropertyMetadata(
            null, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
