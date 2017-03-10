using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

namespace Cleaner .NET
{
    public class WindowsRegistryCleaner
    {
        public static RegistryItem[] HKEY_LOCAL_MACHINE_Software_Microsoft_Windows_CurrentVersion_Uninstall()
        {
            //delete all subkey
            List<RegistryItem> result = new List<RegistryItem>();
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
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
                        if (Directory.GetFiles(Path.GetTempPath(), "*", SearchOption.AllDirectories).Length == 0)
                            result.Add(new RegistryItem(rkey.Name + "\\" + name, "InstallLocation", valueData));
                    }
                }
            }
            return result.ToArray();
        }

        public static RegistryItem[] HKEY_LOCAL_MACHINE_SOFTWARE_Microsoft_Windows_CurrentVersion_SharedDLLs()
        {
            //delete only value
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDLLs");
            if(rkey != null)
                return GetRegItemsForMissingFiles(rkey);
            return null;
        }
        public static RegistryItem[] HKEY_CURRENT_USER_SOFTWARE_Microsoft_Windows_NT_CurrentVersion_AppCompatFlags_Compatibility_Assistant()
        {
            //delete only value
            List<RegistryItem> regItems = new List<RegistryItem>();
           
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store");
            if(rkey != null)
            {
                foreach (RegistryItem item in GetRegItemsForMissingFiles(rkey))
                    regItems.Add(item);
            }
            RegistryKey rkey2 = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Persisted");
            if(rkey2 != null)
            {
                foreach (RegistryItem item in GetRegItemsForMissingFiles(rkey2))
                    regItems.Add(item);
            }
            return regItems.ToArray();
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
            string[] values = rKey.GetValueNames();
            foreach (string value in values)
            {
                if ((value != null) && (value != ""))
                {
                    if (!File.Exists(value))
                        result.Add(new RegistryItem(rKey.Name, value, Convert.ToString(Registry.GetValue(rKey.Name, value, null))));
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
