using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;

namespace Cleaner.NET
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (Framework.FrameworkVersion.Get45DotNetVersion())
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
                MainWindow mw = new MainWindow();
                mw.Show();
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
    }
}
