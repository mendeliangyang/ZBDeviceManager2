
using System.IO;

namespace IOTDataCentreDaemon
{
    class LocalConfig
    {
        internal static string AppRegName = "ZB.IOTDataCentreDaemon";
        internal static string AppPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

        internal static string AppWorkDirectory = System.Environment.CurrentDirectory;
        internal static string IOTAppName = "IOTDataBus.exe";
        internal static string IOTProcessName = "IOTDataBus";

        internal static string GetIOTWorkDirectory()
        {
           return Path.Combine(AppWorkDirectory, IOTAppName);
        }
    }
}
