﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cleaner.NET.ViewModel
{
    public class RegistryTabViewModel : ViewModelBase
    {
        private MainWindowViewModel mainWindowViewModel;

        public ICommand AnalyzeCommand { get; set; }
        public ICommand CleanCommand { get; set; }

        private ObservableCollection<RegistryListItem> _ListOfRegKeys = new ObservableCollection<RegistryListItem>();
        private bool _ProgressBarIsIndeterminate;
        private bool _MissingSoftIsChecked = true;
        private bool _MissingDLLIsChecked = true;
        private bool _MissingFiles = true;
        private bool _MissingMUI = true;

        public ObservableCollection<RegistryListItem> ListOfRegKeys
        {
            get { return _ListOfRegKeys; }
            set { Set(() => ListOfRegKeys, ref _ListOfRegKeys, value); }
        }
        public bool ProgressBarIsIndeterminate
        {
            get { return _ProgressBarIsIndeterminate; }
            set { Set(() => ProgressBarIsIndeterminate, ref _ProgressBarIsIndeterminate, value); }
        }
        public bool MissingSoftIsChecked
        {
            get { return _MissingSoftIsChecked; }
            set { Set(() => MissingSoftIsChecked, ref _MissingSoftIsChecked, value); }
        }
        public bool MissingDLLIsChecked
        {
            get { return _MissingDLLIsChecked; }
            set { Set(() => MissingDLLIsChecked, ref _MissingDLLIsChecked, value); }
        }
        public bool MissingFiles
        {
            get { return _MissingFiles; }
            set { Set(() => MissingFiles, ref _MissingFiles, value); }
        }
        public bool MissingMUI
        {
            get { return _MissingMUI; }
            set { Set(() => MissingMUI, ref _MissingMUI, value); }
        }
        public RegistryTabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            AnalyzeCommand = new RelayCommand(AnalyzeMethod);
            CleanCommand = new RelayCommand(CleanMethod);
        }

        private async void CleanMethod()
        {
            mainWindowViewModel.TabControlIsEnabled = false;
            ProgressBarIsIndeterminate = true;
            
            foreach(RegistryListItem item in ListOfRegKeys)
            {
                if (item.IsChecked)
                {
                    if (item.DeleteKeyWithValue)
                        await Task.Run(() => WindowsRegistryCleaner.DeleteKey(item.Key));
                    else
                        await Task.Run(() => WindowsRegistryCleaner.DeleteValue(item.Key, item.Value));  
                }
            }
            for (int i = ListOfRegKeys.Count; i > 0; i--)
            {
                if(ListOfRegKeys[i - 1].IsChecked)
                    ListOfRegKeys.Remove(ListOfRegKeys[i - 1]);
            }

            mainWindowViewModel.TabControlIsEnabled = true;
            ProgressBarIsIndeterminate = false;
        }

        private async void AnalyzeMethod()
        {          
            mainWindowViewModel.TabControlIsEnabled = false;
            ProgressBarIsIndeterminate = true;
            ListOfRegKeys.Clear();
            if (MissingSoftIsChecked)
            {        
                RegistryItem[] regItems = await Task.Run(() => WindowsRegistryCleaner.HKEY_LOCAL_MACHINE_Software_Microsoft_Windows_CurrentVersion_Uninstall());
                foreach (RegistryItem item in regItems)
                    ListOfRegKeys.Add(new RegistryListItem(true, true, item.Key, item.Value, item.ValueData));
            }
            if (MissingDLLIsChecked)
            {
                RegistryItem[] regItems = await Task.Run(() => WindowsRegistryCleaner.HKEY_LOCAL_MACHINE_SOFTWARE_Microsoft_Windows_CurrentVersion_SharedDLLs());
                foreach (RegistryItem item in regItems)
                    ListOfRegKeys.Add(new RegistryListItem(false, true, item.Key, item.Value, item.ValueData));
            }
            if(MissingFiles)
            {
                RegistryItem[] regItems = await Task.Run(() => WindowsRegistryCleaner.HKEY_CURRENT_USER_SOFTWARE_Microsoft_Windows_NT_CurrentVersion_AppCompatFlags_Compatibility_Assistant());
                foreach (RegistryItem item in regItems)
                    ListOfRegKeys.Add(new RegistryListItem(false, true, item.Key, item.Value, item.ValueData));
            }
            mainWindowViewModel.TabControlIsEnabled = true;
            ProgressBarIsIndeterminate = false;
        }
    }

    public class RegistryListItem
    {
        /* delete key or only value in key */
        public bool DeleteKeyWithValue { get; set; }
        /*  */
        public bool IsChecked { get; set; }
        public string Key { get; set; }
        public string Value {get; set; }
        public string ValueData { get; set; }

        public RegistryListItem(bool DeleteKeyWithValue, bool IsChecked, string Key, string Value, string ValueData)
        {
            this.DeleteKeyWithValue = DeleteKeyWithValue;
            this.IsChecked = IsChecked;
            this.Key = Key;
            this.Value = Value;
            this.ValueData = ValueData;
        }
    }
}
