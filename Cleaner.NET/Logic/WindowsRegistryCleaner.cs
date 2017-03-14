using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Cleaner .NET
{
    public class WindowsRegistryCleaner
    {
        public static RegistryItem[] HKEY_LOCAL_MACHINE_Software_Microsoft_Windows_CurrentVersion_Uninstall()
        {
            List<RegistryItem> result = new List<RegistryItem>();
            if (Environment.Is64BitProcess)
            {
                RegistryKey rkey64 = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                    result.AddRange(HKEY_LOCAL_MACHINE_Software_Microsoft_Windows_CurrentVersion_Uninstall_Method(rkey64));
                RegistryKey rkey32 = Registry.LocalMachine.OpenSubKey(@"Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
                    result.AddRange(HKEY_LOCAL_MACHINE_Software_Microsoft_Windows_CurrentVersion_Uninstall_Method(rkey32));
            }
            else
            {
                RegistryKey rkey32 = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                result.AddRange(HKEY_LOCAL_MACHINE_Software_Microsoft_Windows_CurrentVersion_Uninstall_Method(rkey32));
            }
            return result.ToArray();
        }
        private static RegistryItem[] HKEY_LOCAL_MACHINE_Software_Microsoft_Windows_CurrentVersion_Uninstall_Method(RegistryKey rkey)
        {
            //delete all subkeys
            List<RegistryItem> result = new List<RegistryItem>();
            if (rkey != null)
            {
                string[] names = rkey.GetSubKeyNames();
                foreach (string name in names)
                {
                    string valueData = (string)Registry.GetValue(rkey.Name + "\\" + name, "InstallLocation", null);
                    if ((valueData != null) && (valueData != ""))
                    {
                        if (!Directory.Exists(valueData))
                            result.Add(new RegistryItem(rkey.Name + "\\" + name, "InstallLocation", valueData));
                        else
                        {
                            if (Directory.GetFiles(valueData, "*", SearchOption.AllDirectories).Length == 0)
                                result.Add(new RegistryItem(rkey.Name + "\\" + name, "InstallLocation", valueData));
                        }
                    }
                }
            }
            return result.ToArray();
        }

        public static RegistryItem[] HKEY_LOCAL_MACHINE_SOFTWARE_Microsoft_Windows_CurrentVersion_SharedDLLs()
        {
            //delete only value
            List<RegistryItem> result = new List<RegistryItem>();
            if (Environment.Is64BitProcess)
            {
                RegistryKey rkey64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDLLs");
                result.AddRange(GetRegItemsForMissingFiles(rkey64));
                RegistryKey rkey32 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\SharedDLLs");
                result.AddRange(GetRegItemsForMissingFiles(rkey32));
            }
            else
            {
                RegistryKey rkey32 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDLLs");
                result.AddRange(GetRegItemsForMissingFiles(rkey32));
            }

            return result.ToArray();
        }      
        public static RegistryItem[] HKEY_CURRENT_USER_SOFTWARE_Microsoft_Windows_NT_CurrentVersion_AppCompatFlags_Compatibility_Assistant()
        {
            //delete only value
            List<RegistryItem> result = new List<RegistryItem>();
            if (Environment.Is64BitProcess)
            {
                RegistryKey rkey64 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store");
                result.AddRange(GetRegItemsForMissingFiles(rkey64));
                RegistryKey rkey32 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store");
                result.AddRange(GetRegItemsForMissingFiles(rkey32));
                RegistryKey rkey264 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Persisted");
                result.AddRange(GetRegItemsForMissingFiles(rkey264));
                RegistryKey rkey232 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Persisted");
                result.AddRange(GetRegItemsForMissingFiles(rkey232));
            }else
            {
                RegistryKey rkey32 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store");
                result.AddRange(GetRegItemsForMissingFiles(rkey32));
                RegistryKey rkey232 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Persisted");
                result.AddRange(GetRegItemsForMissingFiles(rkey232));
            }
            return result.ToArray();
        }
        
        public static RegistryItem[] HKEY_CURRENT_USER_SOFTWARE_Classes_Local_Settings_Software_Microsoft_Windows_Shell_MuiCache()
        {
            List<RegistryItem> result = new List<RegistryItem>();
            if (Environment.Is64BitProcess)
            {
                RegistryKey rkey64 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache");
                result.AddRange(HKEY_CURRENT_USER_SOFTWARE_Classes_Local_Settings_Software_Microsoft_Windows_Shell_MuiCache_Method(rkey64));
                RegistryKey rkey32 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\WOW6432Node\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache");
                result.AddRange(HKEY_CURRENT_USER_SOFTWARE_Classes_Local_Settings_Software_Microsoft_Windows_Shell_MuiCache_Method(rkey32));
            }
            else
            {
                RegistryKey rkey32 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache");
                result.AddRange(HKEY_CURRENT_USER_SOFTWARE_Classes_Local_Settings_Software_Microsoft_Windows_Shell_MuiCache_Method(rkey32));
            }                          
            return result.ToArray();
        }
        private static RegistryItem[] HKEY_CURRENT_USER_SOFTWARE_Classes_Local_Settings_Software_Microsoft_Windows_Shell_MuiCache_Method(RegistryKey rkey)
        {
            List<RegistryItem> result = new List<RegistryItem>();
            foreach (RegistryItem item in GetRegistryItems(rkey))
            {
                int index = item.Value.LastIndexOf(".");
                if (index > 0)
                {
                    string path = item.Value.Remove(index);
                    if (!File.Exists(path))
                        result.Add(item);
                }
            }
            return result.ToArray();
        }

        public static void DeleteKey(string key)
        {
            string _key = key.Substring((key.IndexOf(@"\") + 1));
            string _keyWithOutSubKey = _key.Remove(_key.LastIndexOf(@"\"));
            string _subKeyName = _key.Substring(_key.LastIndexOf(@"\") + 1);        
            RegistryKey rkey = GetMasterKeyByName(key).OpenSubKey(_keyWithOutSubKey, true);
            rkey.DeleteSubKey(_subKeyName);
        }

        public static void DeleteValue(string key, string value)
        {
            RegistryKey rkey = GetMasterKeyByName(key).OpenSubKey(key.Substring((key.IndexOf(@"\") + 1)), true);
            rkey.DeleteValue(value);
        }        

        /* Only use with subkeys, what value is path to files */
        private static RegistryItem[] GetRegItemsForMissingFiles(RegistryKey rKey)
        {            
            List<RegistryItem> result = new List<RegistryItem>();
            foreach (RegistryItem item in GetRegistryItems(rKey))
            {
                if (!File.Exists(item.Value))
                    result.Add(new RegistryItem(rKey.Name, item.Value, Convert.ToString(Registry.GetValue(rKey.Name, item.Value, null))));
            }
            return result.ToArray();
        }

        private static RegistryItem[] GetRegistryItems(RegistryKey rKey)
        {
            List<RegistryItem> result = new List<RegistryItem>();
            if (rKey != null)
            {
                string[] values = rKey.GetValueNames();
                foreach (string value in values)
                {
                    if ((value != null) && (value != ""))
                    {
                        if (!File.Exists(value))
                            result.Add(new RegistryItem(rKey.Name, value, Convert.ToString(Registry.GetValue(rKey.Name, value, null))));
                    }
                }
            }
            return result.ToArray();
        }

        private static RegistryKey GetMasterKeyByName(string name)
        {
            string key = name.Remove(name.IndexOf(@"\"));
            if (key == "HKEY_CLASSES_ROOT")
                return Registry.ClassesRoot;
            if (key == "HKEY_CURRENT_USER")
                return Registry.CurrentUser;
            if (key == "HKEY_LOCAL_MACHINE")
                return Registry.LocalMachine;
            if (key == "HKEY_USERS")
                return Registry.Users;
            if (key == "HKEY_CURRENT_CONFIG")
                return Registry.CurrentConfig;
            return null;         
        }
    }

    public class RegistryItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string ValueData { get; set; }
        public RegistryItem(string Key, string Value, string ValueData)
        {
            this.Key = Key;
            this.Value = Value;
            this.ValueData = ValueData;
        }
    }
}
