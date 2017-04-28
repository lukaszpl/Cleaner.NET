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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using PluginTemplate;
using NLog;

namespace Cleaner.NET
{
    public class PluginLoader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Assembly DLL;
        private int CleanerNetVersion = 1;
        IPlugin plugin;
        public bool IsWorking;

        public string PluginName;
        
        public PluginLoader(string path)
        {
            try
            {
                DLL = Assembly.LoadFile(path);
                foreach (Type type in DLL.GetExportedTypes())
                {
                    if(type.GetInterface(typeof(IPlugin).Name)!=null)
                        plugin = (IPlugin)Activator.CreateInstance(type);
                }
                PluginName = plugin.PluginName;
                if (CleanerNetVersion != plugin.CleanerNetVersion)
                {
                    logger.Warn("Plugin " + PluginName + " is outdate! ");
                    MessageBoxResult result =  MessageBox.Show(Languages.Lang.OutOfDatePlugin + "\n" + path, Languages.Lang.MsgWarning, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                        IsWorking = true;
                    else
                        IsWorking = false;
                }
                else
                    IsWorking = true;
            }
            catch(Exception e)
            {
                MessageBox.Show(Languages.Lang.BadPlugin + "\n" + path, Languages.Lang.MsgError, MessageBoxButton.OK, MessageBoxImage.Error);
                IsWorking = false;
                logger.Error("Plugin loading error: " + e.Message);
            }
        }

        public List<object> GetGUIElements()
        {
            return plugin.GUIElements();
        }

        public string CallPluginMethod(CheckBox checkBox, bool DoClean, Dispatcher dispatcher, CultureInfo cultureInfo)
        {
            return plugin.PluginMethod(checkBox, DoClean, dispatcher, cultureInfo);
        }

        /* static */
        public static string[] GetPathToPlugins()
        {
            List<string> listOfPaths = new List<string>();
            string PluginPath = System.IO.Directory.GetCurrentDirectory() + "\\plugins";
            if (Directory.Exists(PluginPath)){
                foreach (string pathtofile in Directory.GetFiles(PluginPath, "*.dll"))
                    listOfPaths.Add(pathtofile);
            }
            return listOfPaths.ToArray();
        }
    }
}
