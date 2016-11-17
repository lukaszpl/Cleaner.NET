using Cleaner.NET.Properties;
using Cleaner.NET.ViewModel;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Cleaner.NET
{
    public class SettingsLoader
    {

        public static void LoadSettings(MainWindowViewModel vm)
        {          
            string path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            if (File.Exists(path))
            {
                XmlReader xmlReader = new XmlReader();
                xmlReader.LoadFile(path);

                //only select combobox item, set language in App.xaml.cs
                vm.SelectedLangItem = Convert.ToInt16(xmlReader.GetSettingByName("SelectedLang"));
                //
                foreach (object item in vm.ListOfWindowsElements)
                {
                    CheckBox a = item as CheckBox;
                    if (a != null)
                    {
                        a.IsChecked = Convert.ToBoolean(xmlReader.GetSettingByName(a.Name));
                    }
                }
                //for plugins
                foreach (object item in vm.ListOfOtherElemets)
                {
                    CheckBox a = item as CheckBox;
                    if (a != null)
                    {
                        if(!(bool)a.IsChecked)
                            a.IsChecked = Convert.ToBoolean(xmlReader.GetSettingByName(a.Name));
                    }
                }               
            }
        }
        public static void SaveSettings(MainWindowViewModel vm)
        {
            //add lang setting
            System.Configuration.SettingsProperty langproperty = new System.Configuration.SettingsProperty("SelectedLang");
            langproperty.DefaultValue = true;
            langproperty.IsReadOnly = false;
            langproperty.PropertyType = typeof(int);
            langproperty.Provider = AppSettings.Default.Providers["LocalFileSettingsProvider"];
            langproperty.Attributes.Add(typeof(System.Configuration.UserScopedSettingAttribute), new System.Configuration.UserScopedSettingAttribute());
            AppSettings.Default.Properties.Add(langproperty);
            AppSettings.Default["SelectedLang"] = vm.SelectedLangItem;
            
            //////
            foreach (object item in vm.ListOfWindowsElements)
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
            //for plugins
            foreach (object item in vm.ListOfOtherElemets)
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
            AppSettings.Default.Save();
            AppSettings.Default.Properties.Clear();
        }

    }
}
