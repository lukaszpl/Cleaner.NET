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
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace Cleaner.NET.ViewModel
{
    public class SettingsTabViewModel : ViewModelBase
    {
        private MainWindowViewModel mainWindowViewModel;
        #region private fields/GUI
        private int _SelectedLangItem;
        private ObservableCollection<string> _ListOfLang = new ObservableCollection<string>();
        #endregion
        #region public fields/GUI
        public ObservableCollection<string> ListOfLang
        {
            get { return _ListOfLang; }
            set { Set(() => ListOfLang, ref _ListOfLang, value); }
        }
        public int SelectedLangItem
        {
            get { return _SelectedLangItem; }
            set { Set(() => SelectedLangItem, ref _SelectedLangItem, value); }
        }
        #endregion
        #region commands
        public ICommand SelectionChangedLangCommand { get; set; }
        #endregion

        public SettingsTabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            SelectionChangedLangCommand = new RelayCommand(ChooseLang);
            SetCurrentLanguage();
        }

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
            mainWindowViewModel.CloseAction();
            mw.Show();
        }
        #endregion 
    }
}
