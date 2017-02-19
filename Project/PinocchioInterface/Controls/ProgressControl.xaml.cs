using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PinocchioInterface.Controls
{
    /// <summary>
    /// Interaction logic for ProgressControl.xaml
    /// </summary>
    public partial class ProgressControl : UserControl
    {
        public ProgressControl()
        {
            InitializeComponent();
        }


        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(ProgressControl));


        public bool IsCancelEnabled
        {
            get { return (bool)GetValue(IsCancelEnabledProperty); }
            set { SetValue(IsCancelEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCancelEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCancelEnabledProperty =
            DependencyProperty.Register("IsCancelEnabled", typeof(bool), typeof(UserControl));



        public string ProgressStatus
        {
            get { return (string)GetValue(ProgressStatusProperty); }
            set { SetValue(ProgressStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressStatusProperty =
            DependencyProperty.Register("ProgressStatus", typeof(string), typeof(ProgressControl));
        

        public double ProgressValue
        {
            get { return (double)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.Register("ProgressValue", typeof(double), typeof(ProgressControl));

    }
}
