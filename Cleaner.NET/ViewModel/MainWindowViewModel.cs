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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Cleaner.NET.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public CleanerTabViewModel cleanerTabViewModel;
        public RegistryTabViewModel registryTabViewModel;
        public SettingsTabViewModel settingsTabViewModel;
        public AboutTabViewModel aboutTabViewModel;
        #region private fields/GUI
        private bool _TabControlIsEnabled;
        private ObservableCollection<object> _children;
        #endregion

        #region Actions
        public Action CloseAction { get; internal set; }
        internal void OnClosing(object sender, CancelEventArgs e)
        {
            Settings.Save(this);
        }
        #endregion

        public MainWindowViewModel()
        {
            TabControlIsEnabled = true;
            LoadPlugins();
            _children = new ObservableCollection<object>();
            _children.Add(cleanerTabViewModel =  new CleanerTabViewModel(this));
            _children.Add(registryTabViewModel = new RegistryTabViewModel(this));
            _children.Add(settingsTabViewModel = new SettingsTabViewModel(this));
            _children.Add(aboutTabViewModel = new AboutTabViewModel(this));
            Settings.Load(this);
        }

        public ObservableCollection<object> Children
        {
            get { return _children; }
        }

        #region public fields/GUI
        public bool TabControlIsEnabled
        {
            get { return _TabControlIsEnabled; }
            set { Set(() => TabControlIsEnabled, ref _TabControlIsEnabled, value); }
        }
        #endregion

        #region for plugins
        public List<PluginLoader> plugins = new List<PluginLoader>();
        private void LoadPlugins()
        {
            foreach (string path in PluginLoader.GetPathToPlugins())
            {
                PluginLoader pl = new PluginLoader(path);
                if (pl.IsWorking)
                    plugins.Add(pl);
            }
        }
        #endregion
    }
}