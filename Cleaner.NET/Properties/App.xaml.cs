﻿/* This file is part of Cleaner .NET

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
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.Threading;
using System.Windows;

namespace Cleaner.NET
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.ControlAppDomain)]
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(App_UnhandledException);
            if (Framework.FrameworkVersion.Is45DotNetVersion())
            {
                string path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
                if (File.Exists(path))
                {
                    XmlReader xmlReader = new XmlReader();
                    xmlReader.LoadFile(path);
                    int langid = Convert.ToInt16(xmlReader.GetSettingByName("SelectedLang"));
                    if (langid == 0)
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    if (langid == 1)
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
                }
                new MainWindow().Show();
            }
            else
            {
                if (MessageBox.Show("Nie odnaleziono Microsoft .NET Framework w wersji 4.5 lub nowszej! Aby uruchomić program zainstaluj wymagany składnik! Czy przekierować Cię do strony pobierania?", "Błąd!", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("http://www.microsoft.com/en-us/download/details.aspx?id=30653");
                }
                Environment.Exit(1);
            }
        }

        private static void App_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            string SysInfo = SystemInformation.GetWindowsVersion() + SystemInformation.GetWindowsCompilation() + Environment.NewLine + "Cleaner .NET " + Build._Build + Environment.NewLine;
            StreamWriter sw = File.CreateText(DateTime.Now.ToString("hh-mm-ss_dd-MM-yyyy") + "-ErrorLog.txt");
            sw.Write(SysInfo + e.ToString());
            sw.Close();
            //
            MessageBoxResult msgResult = MessageBox.Show(Languages.Lang.SendReportQuestion, Languages.Lang.MsgError, MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (msgResult == MessageBoxResult.Yes)
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        var data = new NameValueCollection();
                        data["report"] = SysInfo + e.ToString();
                        //working with https only
                        var response = client.UploadValues("https://csharp-dev.pl/CleanerNETErrorReports/SendReport.php?", "POST", data);
                    }
                    MessageBox.Show(Languages.Lang.ReportSended, Languages.Lang.Information, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show(Languages.Lang.ReportNotSended, Languages.Lang.MsgError, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Environment.Exit(1);
        }
    }
}
