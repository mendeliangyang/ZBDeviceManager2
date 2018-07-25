using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTDataCentreDaemon
{
    public class RegistryAutoStart
    {
        //private static string name = "DBAnalyzer";
        //private static string path = Application.ExecutablePath;

        /// <summary>  
        /// 注册表括操作将程序添加到启动项  
        /// </summary>  
        public static void SetRegistryKey(string name,string startPath ,bool Started)
        {
            try
            {
                RegistryKey HKCU = Registry.CurrentUser;
                RegistryKey Run = HKCU.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\");
                if (Started == true)
                {
                    try
                    {
                        Run.SetValue(name, startPath);
                        Run.Close();
                        HKCU.Close();

                        NLogHelper.DefalutInfo("注册表修改成功,name:{0},value:{1}",name,startPath);
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.DefalutError("注册表修改失败,name:{0},value:{1},errmsg:{2}", name, startPath ,ex.Message);
                        NLogHelper.ExceptionInfo(ex, "注册表修改失败,name:{0},value:{1},errmsg:{2}", name, startPath,ex.Message);
                    }
                }
                else
                {
                    if (Run.GetValue(name) != null)
                    {
                        Run.DeleteValue(name);
                        Run.Close();
                        HKCU.Close();
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                NLogHelper.ExceptionInfo(ex, "注册表修改失败,name:{0},value:{1},errmsg:{2}", name, startPath, ex.Message);
            }
        }



        /// <summary>  
        /// 获取是否开机启动  
        /// </summary>  
        /// <returns></returns>  
        public static bool IsRegeditExit(string name,string startPath)
        {
            try
            {
                RegistryKey HKCU = Registry.CurrentUser;
                RegistryKey software = HKCU.OpenSubKey("SOFTWARE", true);
                RegistryKey aimdir = software.OpenSubKey(@"Microsoft\Windows\CurrentVersion\Run\", true);
                object runObj = aimdir.GetValue(name);
                if (runObj == null || !startPath.Equals(runObj.ToString()))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                NLogHelper.ExceptionInfo(ex, "获取注册表失败,name:{0},value:{1},errmsg:{2}", name, startPath, ex.Message);
            }
            return false;
        }
    }
}
