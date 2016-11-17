using System;
using System.Windows;

namespace Cleaner.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {         
            InitializeComponent();
            ViewModel.MainWindowViewModel vm = new ViewModel.MainWindowViewModel();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(Close);
            Closing += vm.OnClosing;
        }
    }
}
