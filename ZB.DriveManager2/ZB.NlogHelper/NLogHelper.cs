using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.NlogHelper
{
    public class NLogHelper
    {
        internal static readonly NLog.Logger DefalutLogger = NLog.LogManager.GetLogger("DefaultLogger");
        
        internal static readonly NLog.Logger ExceptionLogger = NLog.LogManager.GetLogger("ExceptionLogger");

        

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