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
