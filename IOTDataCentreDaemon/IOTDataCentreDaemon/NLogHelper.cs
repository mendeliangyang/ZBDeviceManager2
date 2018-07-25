using System;


namespace IOTDataCentreDaemon
{
    public class NLogHelper
    {
        internal static readonly NLog.Logger DefalutLogger = NLog.LogManager.GetLogger("DefaultLogger");
        

        internal static readonly NLog.Logger StartSvcLogger = NLog.LogManager.GetLogger("StartSvcLogger");
        

        internal static readonly NLog.Logger ExceptionLogger = NLog.LogManager.GetLogger("ExceptionLogger");





        public static void StartSvcInfo(string info, params string[] infoParams)
        {
#if DEBUG
            Console.WriteLine(string.Format(info, infoParams));
#endif
            StartSvcLogger.Info(string.Format(info, infoParams));
        }

        public static void StartSvcError(string info, params string[] infoParams)
        {
#if DEBUG
            Console.WriteLine(string.Format(info, infoParams));
#endif
            StartSvcLogger.Error(string.Format(info, infoParams));
        }


        public static void DefalutInfo(string info, params string[] infoParams) 
        {
#if DEBUG
            Console.WriteLine(string.Format(info, infoParams));
#endif
            DefalutLogger.Info(string.Format(info, infoParams));
        }

        public static void DefalutError(string info, params string[] infoParams)
        {
#if DEBUG
            Console.WriteLine(string.Format(info, infoParams));
#endif
            DefalutLogger.Error(string.Format(info, infoParams));
        }


        public static void ExceptionInfo(Exception ex,string infoStr, params string[] infoParams)
        {
#if DEBUG
            Console.WriteLine(string.Format(infoStr,infoParams));
#endif
            
            ExceptionLogger.Error(ex, string.Format(infoStr, infoParams));
        }
        
    }
}