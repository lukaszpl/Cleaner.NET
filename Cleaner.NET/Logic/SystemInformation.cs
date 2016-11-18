/* This file is part of Cleaner .NET

    Cleaner .NET is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 3 of the License.

    Cleaner .NET is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA */
using System;
using System.Management;

namespace Cleaner.NET
{
    class SystemInformation
    {
        public static string GetWindowsVersion()
        {
            string bit = null;
            /*32 or 64bit system*/
            if (Environment.Is64BitOperatingSystem == true)
            {
                bit = " 64-bit ";
            }
            else
            {
                bit = " 32-bit ";
            }
            try
            {
                string system = null;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                   system = queryObj["Caption"].ToString() + bit;
                }
                return system;              
            }
            catch (Exception)
            {
                return "Unknown error";
            }
        }
        public static string GetUsername()
        {
            return Environment.UserName;
        }
        public static string GetWindowsCompilation()
        {
            try
            {
                string version = null;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    version = queryObj["Version"].ToString();
                }
                return version;
            }
            catch (Exception)
            {
                return "Unknown error";
            }
        }
        public static string GetSystemDirectory()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["SystemDirectory"].ToString();
                }
            }
            catch (Exception)
            {
                return "Unknown error";
            }
            return null;
        }
    }
}
