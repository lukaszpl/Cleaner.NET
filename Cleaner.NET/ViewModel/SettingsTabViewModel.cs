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
