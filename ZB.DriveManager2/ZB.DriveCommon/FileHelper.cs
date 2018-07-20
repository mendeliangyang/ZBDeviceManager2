using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.DriveCommon
{
    public class FileHelper
    {
        /// <summary>
        /// 读取指定文件文件内容
        /// </summary>
        /// <param name="filePath">文本文件路径</param>
        /// <param name="strFileContent">读取的文件内容</param>
        /// <returns>0:成功  1:文件不存在  -1:未知异常错误</returns>
        public static int ReadAllText(string filePath,out string strFileContent)
        {
            strFileContent = null;
            try
            {
                if (!File.Exists(filePath))
                {
                    return 1;
                }
                strFileContent = File.ReadAllText(filePath, Encoding.UTF8);
                return 0;
            }
            catch (Exception ex)
            {
                NlogHelper.NLogHelper.ExceptionInfo(ex, "ReadAllText  param:'{1}', excepiton:{0}",ex.Message,filePath);
                return -1;
            }
        }

        /// <summary>
        /// 写入文本内容到指定文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="strFileContent">文本文件内容</param>
        /// <returns>0:成功  -1未知异常错误</returns>
        public static int WriteStringToFile(string filePath,string strFileContent)
        {
            //string jsonVideoPoistionStr = JsonConvert.SerializeObject(VideoPanelSettingJsonMdoel);
            StreamWriter sw = null;
            try
            {
                if (!File.Exists(filePath))
                {
                    sw = File.CreateText(filePath);
                }
                sw = new StreamWriter(filePath, false);
                sw.Write(strFileContent);
                sw.Flush();
                sw.Close();
                return 0;
            }
            catch (Exception ex)
            {
                NlogHelper.NLogHelper.ExceptionInfo(ex, "WriteStringToFile  param:'{1}', excepiton:{0}", ex.Message, filePath);
                return -1;
            }
            
        }
    }
}
