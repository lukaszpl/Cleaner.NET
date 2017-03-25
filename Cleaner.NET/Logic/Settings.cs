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
using Cleaner.NET.Properties;
using Cleaner.NET.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Windows.Controls;

namespace Cleaner.NET
{
    public class Settings
    {
        public static void Load(MainWindowViewModel vm)
        {          
            string path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            if (File.Exists(path))
            {
                XmlReader xmlReader = new XmlReader();
                xmlReader.LoadFile(path);

                //
                foreach (object item in vm.cleanerTabViewModel.ListOfWindowsElements)
                {
                    CheckBox a = item as CheckBox;
                    if (a != null)
                    {
                        a.IsChecked = Convert.ToBoolean(xmlReader.GetSettingByName(a.Name));
                    }
                }
                //for plugins
                foreach (object item in vm.cleanerTabViewModel.ListOfOtherElemets)
                {
                    CheckBox a = item as CheckBox;
                    if (a != null)
                    {
                        if(!(bool)a.IsChecked)
                            a.IsChecked = Convert.ToBoolean(xmlReader.GetSettingByName(a.Name));
                    }
                }
                //for registry tab
                LoadRegistryCheckBoxes(vm);
            }
        }

        public static void Save(MainWindowViewModel vm)
        {
            //add lang setting
            System.Configuration.SettingsProperty langproperty = new System.Configuration.SettingsProperty("SelectedLang");
            langproperty.DefaultValue = true;
            langproperty.IsReadOnly = false;
            langproperty.PropertyType = typeof(int);
            langproperty.Provider = AppSettings.Default.Providers["LocalFileSettingsProvider"];
            langproperty.Attributes.Add(typeof(System.Configuration.UserScopedSettingAttribute), new System.Configuration.UserScopedSettingAttribute());
            AppSettings.Default.Properties.Add(langproperty);
            AppSettings.Default["SelectedLang"] = vm.settingsTabViewModel.SelectedLangItem;

            SaveCheckBoxes(vm.cleanerTabViewModel.ListOfWindowsElements);
            //for plugins
            SaveCheckBoxes(vm.cleanerTabViewModel.ListOfOtherElemets);
            //for registry tab
            SaveRegistryCheckBoxes(vm);
            //
            AppSettings.Default.Save();
            AppSettings.Default.Properties.Clear();
        }
        private static void LoadRegistryCheckBoxes(MainWindowViewModel vm)
        {
            vm.registryTabViewModel.MissingDLLIsChecked = AppSettings.Default.MissingDLLIsChecked;
            vm.registryTabViewModel.MissingFilesIsChecked = AppSettings.Default.MissingFilesIsChecked;
            vm.registryTabViewModel.MissingMUIIsChecked = AppSettings.Default.MissingMUIIsChecked;
            vm.registryTabViewModel.MissingSoftIsChecked = AppSettings.Default.MissingSoftIsChecked;
        }
        private static void SaveRegistryCheckBoxes(MainWindowViewModel vm)
        {
            AppSettings.Default.MissingDLLIsChecked = vm.registryTabViewModel.MissingDLLIsChecked;
            AppSettings.Default.MissingFilesIsChecked = vm.registryTabViewModel.MissingFilesIsChecked;
            AppSettings.Default.MissingMUIIsChecked = vm.registryTabViewModel.MissingMUIIsChecked;
            AppSettings.Default.MissingSoftIsChecked = vm.registryTabViewModel.MissingSoftIsChecked;           
        }
        private static void SaveCheckBoxes(ObservableCollection<object> obj)
        {
            foreach (object item in obj)
            {
                CheckBox a = item as CheckBox;
                if (a != null)
                {
                    System.Configuration.SettingsProperty property = new System.Configuration.SettingsProperty(a.Name);
                    property.DefaultValue = true;
                    property.IsReadOnly = false;
                    property.PropertyType = typeof(bool);
                    property.Provider = AppSettings.Default.Providers["LocalFileSettingsProvider"];
                    property.Attributes.Add(typeof(System.Configuration.UserScopedSettingAttribute), new System.Configuration.UserScopedSettingAttribute());
                    AppSettings.Default.Properties.Add(property);
                    AppSettings.Default[a.Name] = a.IsChecked;
                    property = null;
                }
            }
        }
    }
}
