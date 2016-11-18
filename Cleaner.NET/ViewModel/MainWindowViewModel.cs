/* This file is part of Cleaner .NET

    Cleaner .NET is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 3 of the License.

    Cleaner .NET is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA */
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;

namespace Cleaner.NET.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region private fields/GUI
        private double _ProgressBarValue;
        private int _SelectedLangItem;
        private string _ClearLogText;
        private bool _TabControlIsEnabled;
        private string _Percent;
        private bool _ProgressBarIsIndeterminate;
        private Visibility _ProgressBarIsIndeterminateVisibility;

        private ObservableCollection<object> _ListOfOtherElemets = new ObservableCollection<object>();
        private ObservableCollection<object> _ListOfWindowsElements = new ObservableCollection<object>();

        private ObservableCollection<object> _ListOfPlugins = new ObservableCollection<object>();
        private ObservableCollection<string> _ListOfLang = new ObservableCollection<string>();
        #endregion

        #region commands
        public ICommand SelectionChangedLangCommand { get; set; }
        public ICommand AnalyzeCommand { get; set; }
        public ICommand CleanCommand { get; set; }
        #endregion

        #region Actions
        public Action CloseAction { get; internal set; }
        internal void OnClosing(object sender, CancelEventArgs e)
        {
            SettingsLoader.SaveSettings(this);
        }
        #endregion

        public MainWindowViewModel()
        {
            SelectionChangedLangCommand = new RelayCommand(ChooseLang);
            AnalyzeCommand = new RelayCommand(AnalyzeMethod);
            CleanCommand = new RelayCommand(CleanMethod);
            TabControlIsEnabled = true;
            Percent = "0%";
            ProgressBarIsIndeterminateVisibility = Visibility.Hidden;
            WindowsItems WinItems = new WindowsItems();
            ListOfWindowsElements = WinItems.GetWindowsElements();
            LoadPlugins();
            SettingsLoader.LoadSettings(this);
            SetCurrentLanguage();
        }

        #region public fields/GUI
        public ObservableCollection<object> ListOfWindowsElements
        {
            get { return _ListOfWindowsElements; }
            set { Set(() => ListOfWindowsElements, ref _ListOfWindowsElements, value); }
        }
        public ObservableCollection<object> ListOfOtherElemets
        {
            get { return _ListOfOtherElemets; }
            set { Set(() => ListOfOtherElemets, ref _ListOfOtherElemets, value); }
        }
        public ObservableCollection<string> ListOfLang
        {
            get { return _ListOfLang; }
            set { Set(() => ListOfLang, ref _ListOfLang, value); }
        }
        public ObservableCollection<object> ListOfPlugins
        {
            get { return _ListOfPlugins; }
            set { Set(() => ListOfPlugins, ref _ListOfPlugins, value); }
        }
        public int SelectedLangItem
        {
            get { return _SelectedLangItem; }
            set { Set(() => SelectedLangItem, ref _SelectedLangItem, value); }
        }
        public string ClearLogText
        {
            get { return _ClearLogText; }
            set { Set(() => ClearLogText, ref _ClearLogText, value); }
        }
        public double ProgressBarValue
        {
            get { return Math.Round(_ProgressBarValue, 0); }
            set { Set(() => ProgressBarValue, ref _ProgressBarValue, value); }
        }
        public bool TabControlIsEnabled
        {
            get { return _TabControlIsEnabled; }
            set { Set(() => TabControlIsEnabled, ref _TabControlIsEnabled, value); }
        }
        public string Percent
        {
            get { return _Percent; }
            set { Set(() => Percent, ref _Percent, value); }
        }
        public bool ProgressBarIsIndeterminate
        {
            get { return _ProgressBarIsIndeterminate; }
            set { Set(() => ProgressBarIsIndeterminate, ref _ProgressBarIsIndeterminate, value); }
        }
        public Visibility ProgressBarIsIndeterminateVisibility
        {
            get { return _ProgressBarIsIndeterminateVisibility; }
            set { Set(() => ProgressBarIsIndeterminateVisibility, ref _ProgressBarIsIndeterminateVisibility, value); }
        }
        #endregion

        #region Methods for commands
        private async void AnalyzeMethod()
        {
            StartAnalyzeOrClean();
            ClearLogText = "------------------------------\n" +
                Informations.ProgramName + " " + Informations.TextBuild +
                "\n------------------------------\n\n" +
                Languages.Lang.AnalyzeLog + "\n\n";
            await Clean(false);
            await PluginEvents(false);
            StopAnalyzeOrClean();
        }
        private async void CleanMethod()
        {
            StartAnalyzeOrClean();
            ClearLogText = "------------------------------\n" +
               Informations.ProgramName + " " + Informations.TextBuild +
               "\n------------------------------\n\n" +
               Languages.Lang.CleanLog + "\n\n";
            await Clean(true);
            await PluginEvents(true);
            StopAnalyzeOrClean();
        }
        #endregion

        #region for languages
        private void SetCurrentLanguage()
        {
            ListOfLang.Add("English (English)");
            ListOfLang.Add("Polski (Polish)");
            if (CultureInfo.CurrentUICulture.IetfLanguageTag == "en-US")
                SelectedLangItem = 0;
            if (CultureInfo.CurrentUICulture.IetfLanguageTag == "pl-PL")
                SelectedLangItem = 1;
        }

        private void ChooseLang()
        {
            if (SelectedLangItem == 0)
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            if (SelectedLangItem == 1)
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            MainWindow mw = new MainWindow();
            mw.Show();
            CloseAction();
        }
        #endregion

        #region for plugins
        List<PluginLoader> plugins = new List<PluginLoader>();
        private void LoadPlugins()
        {
            foreach (string path in PluginLoader.GetPathToPlugins())
            {
                PluginLoader pl = new PluginLoader(path);
                if(pl.IsWorking)
                    plugins.Add(pl);
            }
            foreach (PluginLoader pl in plugins)
            {
                List<object> elements = pl.GetGUIElements();
                if (elements != null)
                {
                    foreach (object obj in elements)
                        ListOfOtherElemets.Add(obj);
                    var textBlock = new TextBlock();
                    textBlock.Text = pl.PluginName;
                    ListOfPlugins.Add(textBlock);
                }
            }
        }

        private async Task PluginEvents(bool DoClean)
        {
            try
            {
                foreach (PluginLoader pl in plugins)
                    foreach (object obj in ListOfOtherElemets)
                    {
                        CheckBox a = obj as CheckBox;
                        if (a != null)
                            if (a.IsChecked == true)
                            {
                                CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture;          
                                ClearLogText += await Task.Run(() => pl.CallPluginMethod(a, DoClean, Application.Current.Dispatcher, cultureInfo));
                                UpdateProgressBar(); //Update progress bar, when method is end                                              
                            }
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show(Languages.Lang.PluginError + "\n\n" + e.ToString(), Languages.Lang.MsgError, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region GUI operations

        int Index = 0;
        private void UpdateProgressBar()
        {           
            int NumberOfElements = 0;
            foreach (object obj in ListOfOtherElemets)
            {
                CheckBox a = obj as CheckBox;
                if (a != null)
                    if (a.IsChecked == true)
                        ++NumberOfElements;
            }
            foreach (object obj in ListOfWindowsElements)
            {
                CheckBox a = obj as CheckBox;
                if (a != null)
                    if (a.IsChecked == true)
                        ++NumberOfElements;
            }
            float percentOfElement = 100 / NumberOfElements;
            ProgressBarValue += percentOfElement;
            ++Index;
            if (Index == NumberOfElements)
            {
                ProgressBarValue = 100;
                Index = 0;
            }
            Percent = ProgressBarValue.ToString() + "%";         
        }

        private void StartAnalyzeOrClean()
        {
            ProgressBarValue = 0;
            TabControlIsEnabled = false;
            ProgressBarIsIndeterminate = true;
            ProgressBarIsIndeterminateVisibility = Visibility.Visible;
        }

        private void StopAnalyzeOrClean()
        {
            TabControlIsEnabled = true;
            ProgressBarIsIndeterminate = false;
            ProgressBarIsIndeterminateVisibility = Visibility.Hidden;
            Percent = ProgressBarValue.ToString() + "%";
        }
        #endregion

        private async Task Clean(bool DoClean)
        {
            CleanClass cleanClass = new CleanClass(Thread.CurrentThread.CurrentUICulture);
            foreach (object obj in ListOfWindowsElements)
            {
                CheckBox a = obj as CheckBox;
                if (a != null)
                    if (a.IsChecked == true)
                    {
                        if (a.Name == "CleanTempFiles")
                            ClearLogText += await Task.Run(() => cleanClass.CleanTempFiles(DoClean));
                        if (a.Name == "CleanWinErrors")
                            ClearLogText += await Task.Run(() => cleanClass.CleanWinErrors(DoClean));
                        if (a.Name == "CleanCacheDNS")
                            ClearLogText += await Task.Run(() => cleanClass.CleanDNSCache(DoClean));
                        if (a.Name == "CleanFontCache")
                            ClearLogText += await Task.Run(() => cleanClass.CleanFontCache(DoClean));
                        if (a.Name == "CleanClipboard")
                            ClearLogText += await Task.Run(() => cleanClass.CleanClipboard(DoClean));
                        if (a.Name == "CleanTrash")
                            ClearLogText += await Task.Run(() => cleanClass.CleanTrash(DoClean));
                        /////
                        if (a.Name == "CleanRecentDocuments")
                            ClearLogText += await Task.Run(() => cleanClass.CleanRecDoc(DoClean));
                        if (a.Name == "CleanThubCache")
                            ClearLogText += await Task.Run(() => cleanClass.CleanThumbCache(DoClean));
                        UpdateProgressBar(); //Update progress bar, when method is end                        
                    }
            }
        }
    }
}