using System.Reflection;

namespace Cleaner.NET
{
    public class Build
    {
        public static string _Build
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
    }
}