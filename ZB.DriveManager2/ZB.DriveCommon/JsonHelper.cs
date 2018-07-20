using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.DriveCommon
{
    public class JsonHelper
    {
        /// <summary>
        /// 对象转换json字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="strJson">json 字符串</param>
        /// <returns>0:成功  -1:未知异常错误</returns>
        public static int ObjectToStrJson(object obj,out string strJson)
        {
            strJson = null;
            try
            {
                strJson= JsonConvert.SerializeObject(obj);
                return 0;
            }
            catch (Exception ex)
            {
                NlogHelper.NLogHelper.ExceptionInfo(ex, "ObjectToStrJson param :{0},exception:{1}",obj.ToString(),ex.Message);
                return -1;
            }
        }
    }
}
