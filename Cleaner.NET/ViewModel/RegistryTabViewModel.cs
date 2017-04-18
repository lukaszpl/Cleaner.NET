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
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
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
        private bool _MissingFilesIsChecked = true;
        private bool _MissingMUIIsChecked = true;
        private bool _InvalidFileExtensionsIsChecked = true;
        private bool _ReferencesToTheInstallerIsChecked = true;

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
        public bool MissingFilesIsChecked
        {
            get { return _MissingFilesIsChecked; }
            set { Set(() => MissingFilesIsChecked, ref _MissingFilesIsChecked, value); }
        }
        public bool MissingMUIIsChecked
        {
            get { return _MissingMUIIsChecked; }
            set { Set(() => MissingMUIIsChecked, ref _MissingMUIIsChecked, value); }
        }
        public bool InvalidFileExtensionsIsChecked
        {
            get { return _InvalidFileExtensionsIsChecked; }
            set { Set(() => InvalidFileExtensionsIsChecked, ref _InvalidFileExtensionsIsChecked, value); }
        }
        public bool ReferencesToTheInstallerIsChecked
        {
            get { return _ReferencesToTheInstallerIsChecked; }
            set { Set(() => ReferencesToTheInstallerIsChecked, ref _ReferencesToTheInstallerIsChecked, value); }
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

            string toExecuteAsAdmin = null;
            string toExecuteAsUser = null;

            foreach(RegistryListItem item in ListOfRegKeys)
            {
                if (item.IsChecked)
                {
                    string value = item.Value;
                    if(value != null)
                        //if end of ValueName is "\" char
                        if (value.LastIndexOf(@"\") == value.Length - 1)
                            value += @"\";
                    //
                    if (item.DeleteFullKey)
                    {
                        if (item.ReqAdminToModify)
                            toExecuteAsAdmin += " -DeleteKey \"" + item.Key + "\"";
                        else
                            toExecuteAsUser += " -DeleteKey \"" + item.Key + "\"";
                    }
                    else
                    {
                        if(item.ReqAdminToModify)
                            toExecuteAsAdmin += " -DeleteValue \"" + item.Key + "\" \"" + value + "\"";
                        else
                            toExecuteAsUser += " -DeleteValue \"" + item.Key + "\" \"" + value + "\"";
                    }
                }
            }

            if (toExecuteAsUser != null)
            {
                bool result = await StartRegCleaner(false, toExecuteAsUser);
                if (result)
                {
                    for (int i = ListOfRegKeys.Count; i > 0; i--)
                    {
                        if ((ListOfRegKeys[i - 1].IsChecked) && (!ListOfRegKeys[i - 1].ReqAdminToModify))
                            ListOfRegKeys.Remove(ListOfRegKeys[i - 1]);
                    }
                }
            }

            if (toExecuteAsAdmin != null)
            {
                bool result = await StartRegCleaner(true, toExecuteAsAdmin);
                if (result)
                {
                    for (int i = ListOfRegKeys.Count; i > 0; i--)
                    {
                        if ((ListOfRegKeys[i - 1].IsChecked) && (ListOfRegKeys [i - 1].ReqAdminToModify))
                            ListOfRegKeys.Remove(ListOfRegKeys[i - 1]);
                    }
                }
            }

            mainWindowViewModel.TabControlIsEnabled = true;
            ProgressBarIsIndeterminate = false;
        }
        private async Task<bool> StartRegCleaner(bool AsAdmin, string toExecute)
        {
            Process regCleaner = new Process();
            if (AsAdmin)
                regCleaner.StartInfo.Verb = "runas";
            regCleaner.StartInfo.FileName = "RegCleaner.exe";
            regCleaner.StartInfo.Arguments = toExecute;
            regCleaner.StartInfo.CreateNoWindow = true;
            if (File.Exists(regCleaner.StartInfo.FileName)){
                try {
                    regCleaner.Start();
                    await Task.Run(() => regCleaner.WaitForExit());
                    if (regCleaner.ExitCode == 1)
                        MessageBox.Show(Languages.Lang.regCleanerWarning, Languages.Lang.MsgWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
                    else if (regCleaner.ExitCode == 0)
                        return true;
                    return false;
                }
                catch (Win32Exception)
                {
                    // if canceled by user
                    MessageBox.Show(Languages.Lang.OperateCanceledByUser, Languages.Lang.MsgError, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), Languages.Lang.MsgError, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }else
                MessageBox.Show(Languages.Lang.FileNotExist + regCleaner.StartInfo.FileName, Languages.Lang.MsgError, MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
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
                    ListOfRegKeys.Add(new RegistryListItem(true, true, true, item.Key, item.Value, item.ValueData));
            }
            if (MissingDLLIsChecked)
            {
                RegistryItem[] regItems = await Task.Run(() => WindowsRegistryCleaner.HKEY_LOCAL_MACHINE_SOFTWARE_Microsoft_Windows_CurrentVersion_SharedDLLs());
                foreach (RegistryItem item in regItems)
                    ListOfRegKeys.Add(new RegistryListItem(true, false, true, item.Key, item.Value, item.ValueData));
            }
            if(MissingFilesIsChecked)
            {
                RegistryItem[] regItems = await Task.Run(() => WindowsRegistryCleaner.HKEY_CURRENT_USER_SOFTWARE_Microsoft_Windows_NT_CurrentVersion_AppCompatFlags_Compatibility_Assistant());
                foreach (RegistryItem item in regItems)
                    ListOfRegKeys.Add(new RegistryListItem(false, false, true, item.Key, item.Value, item.ValueData));
            }
            if (MissingMUIIsChecked)
            {
                RegistryItem[] regItems = await Task.Run(() => WindowsRegistryCleaner.HKEY_CURRENT_USER_SOFTWARE_Classes_Local_Settings_Software_Microsoft_Windows_Shell_MuiCache());
                foreach (RegistryItem item in regItems)
                    ListOfRegKeys.Add(new RegistryListItem(false, false, true, item.Key, item.Value, item.ValueData));
            }
            if(InvalidFileExtensionsIsChecked)
            {
                RegistryItem[] regItems = await Task.Run(() => WindowsRegistryCleaner.HKEY_CURRENT_USER_SOFTWARE_Microsoft_Windows_CurrentVersion_Explorer_FileExts());
                foreach (RegistryItem item in regItems)
                    ListOfRegKeys.Add(new RegistryListItem(false, true, true, item.Key, item.Value, item.ValueData));
            }
            if (ReferencesToTheInstallerIsChecked)
            {
                RegistryItem[] regItems = await Task.Run(() => WindowsRegistryCleaner.HKEY_LOCAL_MACHINE_SOFTWARE_Microsoft_Windows_CurrentVersion_Installer_Folders());
                foreach (RegistryItem item in regItems)
                    ListOfRegKeys.Add(new RegistryListItem(true, false, true, item.Key, item.Value, item.ValueData));
            }
            mainWindowViewModel.TabControlIsEnabled = true;
            ProgressBarIsIndeterminate = false;
        }
    }

    public class RegistryListItem
    {
        /* delete key or only value in key */
        public bool DeleteFullKey { get; set; }
        /*  */
        public bool ReqAdminToModify { get; set; }
        public bool IsChecked { get; set; }
        public string Key { get; set; }
        public string Value {get; set; }
        public string ValueData { get; set; }

        public RegistryListItem(bool ReqAdminToModify, bool DeleteFullKey, bool IsChecked, string Key, string Value, string ValueData)
        {
            this.ReqAdminToModify = ReqAdminToModify;
            this.DeleteFullKey= DeleteFullKey;
            this.IsChecked = IsChecked;
            this.Key = Key;
            this.Value = Value;
            this.ValueData = ValueData;
        }
    }
}
