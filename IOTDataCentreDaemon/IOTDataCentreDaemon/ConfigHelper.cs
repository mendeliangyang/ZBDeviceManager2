using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IOTDataCentreDaemon
{
    internal static class ConfigHelper
    {
        internal static readonly string DefaultConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        internal static readonly string DefaultDaemonProcessListFileName = Path.Combine(DefaultConfigFilePath, "IOTDataCentreDaemonConfig.json");

        internal static ConfigEntity ReadDaemonConfig(string file)
        {
            if (!File.Exists(file))
                throw new Exception(string.Format("配置文件'{0}',不存在。", file));

            string jsonString = File.ReadAllText(file, Encoding.UTF8);
            if (string.IsNullOrEmpty(jsonString))
                return new ConfigEntity();

            return JsonConvert.DeserializeObject<ConfigEntity>(jsonString);
        }

        
    }
}
