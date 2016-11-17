using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Cleaner.NET
{
    public class PluginLoader
    {
        private Assembly DLL;
        private int CleanerNetVersion = 1;
        dynamic c;
        public bool IsWorking;

        public string PluginName;
        
        public PluginLoader(string path)
        {
            try
            {
                DLL = Assembly.LoadFile(path);
                foreach (Type type in DLL.GetExportedTypes())
                {
                    c = Activator.CreateInstance(type);
                }
                PluginName = c.PluginName;
                if (CleanerNetVersion != c.CleanerNetVersion)
                {
                    MessageBoxResult result =  MessageBox.Show(Languages.Lang.OutOfDatePlugin + "\n" + path, Languages.Lang.MsgWarning, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                        IsWorking = true;
                    else
                        IsWorking = false;
                }
                else
                    IsWorking = true;
            }
            catch
            {
                MessageBox.Show(Languages.Lang.BadPlugin + "\n" + path, Languages.Lang.MsgError, MessageBoxButton.OK, MessageBoxImage.Error);
                IsWorking = false;
            }
        }

        public List<object> GetGUIElements()
        {
            List<object> list = new List<object>();
            try
            {
                list = c.GUIElements();
            }
            catch
            {
                return null;
            }
            return list;
        }

        public string CallPluginMethod(CheckBox checkBox, bool DoClean, Dispatcher dispatcher, CultureInfo cultureInfo)
        {
            string output = null;
            output = c.PluginMethod(checkBox, DoClean, dispatcher, cultureInfo);
            return output;
        }


        /* static */
        public static string[] GetPathToPlugins()
        {
            List<string> listOfPaths = new List<string>();
            string PluginPath = System.IO.Directory.GetCurrentDirectory() + "\\plugins";
            if (Directory.Exists(PluginPath)){
                foreach (string pathtofile in Directory.GetFiles(PluginPath))
                    listOfPaths.Add(pathtofile);
            }
            return listOfPaths.ToArray();
        }
    }
}
