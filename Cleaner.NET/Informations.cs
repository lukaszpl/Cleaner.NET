using System;

namespace Cleaner.NET
{
    public class Informations
    {
        public static string ProgramName
        {
            get
            {
                string bit = null;
                if (Environment.Is64BitProcess)
                    bit = " 64 bit";
                else
                    bit = " 32 bit";
                return "Cleaner .NET v" + Version + bit;
            }
        }
        public static string TextBuild
        {
            get { return "Build " + Build._Build; }
        }
        public static string Version
        {
            get { return "1.0"; }
        }
        public static string Username
        {
            get { return  Languages.Lang.User + ": " + SystemInformation.GetUsername(); }
        }
        public static string SystemName
        {
            get { return SystemInformation.GetWindowsVersion() + "build: " + SystemInformation.GetWindowsCompilation(); }
        }
        public static string LicenseAuthor
        {
            get { return Languages.Lang.License + ": LGPL\n" + Languages.Lang.Author + ": Łukasz Soroczyński\n"; }
        }
    }
}
