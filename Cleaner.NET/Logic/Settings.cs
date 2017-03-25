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
        static XmlReader xmlReader;
        public static void Load(MainWindowViewModel vm)
        {          
            string path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            if (File.Exists(path))
            {
                xmlReader = new XmlReader();
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

        private static void LoadRegistryCheckBoxes(MainWindowViewModel vm)
        {
            vm.registryTabViewModel.MissingDLLIsChecked = Convert.ToBoolean(xmlReader.GetSettingByName("MissingSoftIsChecked"));
            vm.registryTabViewModel.MissingFilesIsChecked = Convert.ToBoolean(xmlReader.GetSettingByName("MissingDLLIsChecked"));
            vm.registryTabViewModel.MissingMUIIsChecked = Convert.ToBoolean(xmlReader.GetSettingByName("MissingFilesIsChecked"));
            vm.registryTabViewModel.MissingSoftIsChecked = Convert.ToBoolean(xmlReader.GetSettingByName("MissingMUIIsChecked"));
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

        private static void SaveRegistryCheckBoxes(MainWindowViewModel vm)
        {
            //add settings
            AppSettings.Default[AddSettingItem("MissingSoftIsChecked", typeof(bool)).Name] = vm.registryTabViewModel.MissingDLLIsChecked;
            AppSettings.Default[AddSettingItem("MissingDLLIsChecked", typeof(bool)).Name] = vm.registryTabViewModel.MissingFilesIsChecked;
            AppSettings.Default[AddSettingItem("MissingFilesIsChecked", typeof(bool)).Name] = vm.registryTabViewModel.MissingMUIIsChecked;
            AppSettings.Default[AddSettingItem("MissingMUIIsChecked", typeof(bool)).Name] = vm.registryTabViewModel.MissingSoftIsChecked;
        }

        private static void SaveCheckBoxes(ObservableCollection<object> obj)
        {
            foreach (object item in obj)
            {
                CheckBox a = item as CheckBox;
                if (a != null)
                {
                    AppSettings.Default[AddSettingItem(a.Name, typeof(bool)).Name] = a.IsChecked;
                }
            }
        }

        private static SettingsProperty AddSettingItem(string Name, Type type)
        {
            System.Configuration.SettingsProperty Property = new System.Configuration.SettingsProperty(Name);
            Property.DefaultValue = true;
            Property.IsReadOnly = false;
            Property.PropertyType = type;
            Property.Provider = AppSettings.Default.Providers["LocalFileSettingsProvider"];
            Property.Attributes.Add(typeof(System.Configuration.UserScopedSettingAttribute), new System.Configuration.UserScopedSettingAttribute());
            AppSettings.Default.Properties.Add(Property);
            return Property;
        }
    }
}
