/* This file is part of Cleaner .NET

    Cleaner .NET is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 3 of the License.

    Cleaner .NET is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Cleaner .NET; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA */
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
