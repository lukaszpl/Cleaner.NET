using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PluginTemplate
{
    public interface IPlugin
    {
        int CleanerNetVersion { get; }
        string PluginName { get; }
        List<object> GUIElements();
        string PluginMethod(CheckBox obj, bool DoClean, Dispatcher GuiThr, CultureInfo cultureInfo);
    }
}
