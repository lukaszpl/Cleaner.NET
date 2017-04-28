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
using NLog;
using System;

namespace Framework
{
    class FrameworkVersion
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static bool Is45DotNetVersion()
        {
            using (RegistryKey DNVKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                int releaseKey = Convert.ToInt32(DNVKey.GetValue("Release"));
                logger.Info("Installed .NET version: " + releaseKey);
                // "4.5 or later";
                if ((releaseKey >= 378389))
                    return true;
                return false;
            }
        }
    }
}
