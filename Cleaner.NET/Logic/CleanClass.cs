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
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace Cleaner.NET
{
    public class CleanClass
    {
        CultureInfo cultureInfo;
        public CleanClass(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
        }
        private void SetLang()
        {
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
        private static double DeleteFiles(string[] PathToFiles, bool DoDelete)
        {
            float Size = 0;
            foreach (string path in PathToFiles)
            {
                try
                {
                    FileInfo info = new FileInfo(path);
                    Size += info.Length;
                    try
                    {
                        if (DoDelete)
                            File.Delete(path);
                    }
                    catch { Size -= info.Length; }
                }
                catch { }
            }
            return Math.Round(Size / (1024 * 1024), 2);
        }
        public string CleanTempFiles(bool DoClean)
        {
            SetLang();
            double Size = 0;
            string[] FilesPath = Directory.GetFiles(Path.GetTempPath(), "*", SearchOption.AllDirectories);
            Size += DeleteFiles(FilesPath, DoClean);

            DirectoryInfo di = new DirectoryInfo(Path.GetTempPath());
            if (DoClean)
            {
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch { }
                }
            }
            return  Languages.Lang.Temp_CheckBox + ": " + Size + " MB" + "\n\n";
        }
        public string CleanWinErrors(bool DoClean)
        {
            SetLang();
            double Size = 0;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Windows\\WER\\ReportArchive";
            if (Directory.Exists(path))
            {
                string[] FilesPath = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                Size += DeleteFiles(FilesPath, DoClean);
                if (DoClean)
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch { }
                    }
                }
            }
            return Languages.Lang.WinErrors_CheckBox + ": " + Size + " MB" + "\n\n";
        }
        public string CleanFontCache(bool DoClean)
        {
            SetLang();
            float Size = 0;
            string path = SystemInformation.GetSystemDirectory() + "\\FNTCACHE.DAT";
            string[] array = { path };
            DeleteFiles(array, DoClean);

            return Languages.Lang.TempFont_CheckBox + ": " + Size + " MB" + "\n\n";
        }
        #region DNS
        [DllImport("dnsapi.dll", EntryPoint = "DnsFlushResolverCache")]
        private static extern uint DnsFlushResolverCache();
        public string CleanDNSCache(bool DoClean)
        {
            SetLang();
            uint result = 0;
            if (DoClean)
                result = DnsFlushResolverCache();
            if ((DoClean) && (result == 1))
                return Languages.Lang.FlushDns_OK + "\n\n";
            if ((DoClean) && (result != 1))
               return Languages.Lang.FlushDns_Error + "\n\n";
            if (!DoClean)
               return Languages.Lang.FlushDns + "\n\n";
            return null;
        }
        #endregion
        public string CleanClipboard(bool DoClean)
        {
            SetLang();
            if (DoClean)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { Clipboard.SetData(DataFormats.Text, (Object)" "); })); //set in clipboard " "
            }
            return Languages.Lang.ClipboardNotClean + "\n\n";
        }
        #region RecycleBin
        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000001,
            SHERB_NOSOUND = 0x00000004
        }
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        extern static uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);
        public string CleanTrash(bool DoClean)
        {
            SetLang();
            if (DoClean)
            {
                try
                {
                    uint result = SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOCONFIRMATION | RecycleFlags.SHERB_NOSOUND);
                    return Languages.Lang.TrashClean + "\n\n";
                }
                catch { return null; }
            }
            else
                return Languages.Lang.TrashNoClean + "\n\n";
        }
        #endregion
        public string CleanRecDoc(bool DoClean)
        {
            SetLang();
            double Size = 0;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Recent";
            if (Directory.Exists(path))
            {
                string[] FilesPath = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                Size += DeleteFiles(FilesPath, DoClean);

                if (DoClean)
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch { }
                    }
                }
            }
            return Languages.Lang.Recentdoc_CheckBox + ": " + Size + " MB" + "\n\n";
        }
        public string CleanThumbCache(bool DoClean)
        {
            SetLang();
            double Size = 0;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Windows\\Explorer\\";
            if (Directory.Exists(path))
            {
                string[] FilesPath = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                Size += DeleteFiles(FilesPath, DoClean);

                if (DoClean)
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch { }
                    }
                }
            }
            return Languages.Lang.ThumbnailsCache_CheckBox + ": " + Size + " MB" + "\n\n";      
        }
    }
}
