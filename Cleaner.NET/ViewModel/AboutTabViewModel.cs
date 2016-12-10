using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Cleaner.NET.ViewModel
{
    public class AboutTabViewModel : ViewModelBase
    {
        private MainWindowViewModel mainWindowViewModel;
        #region private fields/GUI
        private ObservableCollection<object> _ListOfPlugins = new ObservableCollection<object>();
        #endregion
        #region public fields/GUI
        public ObservableCollection<object> ListOfPlugins
        {
            get { return _ListOfPlugins; }
            set { Set(() => ListOfPlugins, ref _ListOfPlugins, value); }
        }
        #endregion

        public AboutTabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            GetPluginsElements();
        }

        private void GetPluginsElements()
        {
            List<PluginLoader> plugins = mainWindowViewModel.plugins;
            foreach (PluginLoader pl in plugins)
            {
                if (pl.IsWorking)
                {
                    TextBlock tb = new TextBlock();
                    tb.Text = pl.PluginName;
                    ListOfPlugins.Add(tb);
                }
            }
        }
    }
}
