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
using Microsoft.Win32;

namespace RegCleaner
{
    class Program
    {
        private static int Main(string[] args)
        {
            int result = 0;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-DeleteKey")
                    if (!DeleteKey(args[i + 1]))
                        result = 1;
                if (args[i] == "-DeleteValue")
                    if ((!DeleteValue(args[i + 1], args[i + 2])))
                        result = 1;
            }
            return result;
        }
        private static bool DeleteKey(string key)
        {
            try
            {
                string _key = key.Substring((key.IndexOf(@"\") + 1));
                string _keyWithOutSubKey = _key.Remove(_key.LastIndexOf(@"\"));
                string _subKeyName = _key.Substring(_key.LastIndexOf(@"\") + 1);
                RegistryKey rkey = GetMasterKeyByName(key).OpenSubKey(_keyWithOutSubKey, true);
                rkey.DeleteSubKeyTree(_subKeyName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool DeleteValue(string key, string value)
        {
            
            try
            {
                RegistryKey rkey = GetMasterKeyByName(key).OpenSubKey(key.Substring((key.IndexOf(@"\") + 1)), true);
                rkey.DeleteValue(value);
                return true;
            }
            catch
            {
                return false;
            }
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
}
