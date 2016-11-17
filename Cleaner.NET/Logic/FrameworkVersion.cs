using Microsoft.Win32;
using System;

namespace Framework
{
    class FrameworkVersion
    {
        public static bool Get45DotNetVersion()
        {
            using (RegistryKey DNVKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                int releaseKey = Convert.ToInt32(DNVKey.GetValue("Release"));
                // "4.5 or later";
                if ((releaseKey >= 378389))
                    return true;
                return false;
            }
        }
    }
}
