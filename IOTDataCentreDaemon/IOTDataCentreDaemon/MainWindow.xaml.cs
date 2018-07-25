﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace IOTDataCentreDaemon
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DaemonHelper daemonHelper = null;
        Thread daemonThread = null;
        System.Timers.Timer timer = null;
        ConfigEntity iotConfig = null;
        bool receiveDataFlag = false;

        public MainWindow()
        {
            InitializeComponent();
            this.StateChanged += new EventHandler(Window_StateChanged);




            daemonHelper = new DaemonHelper();
            iotConfig = ConfigHelper.ReadDaemonConfig(ConfigHelper.DefaultDaemonProcessListFileName);
            daemonHelper.Init(iotConfig);
            daemonHelper.dataReceived += new DataClientDataReceived(client_DataReceived);

            InitWindow();

            timer = new System.Timers.Timer(iotConfig.ReceiveIotTimeOut);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;



            daemonThread = new Thread(DaemonThreadExecute);
            daemonThread.IsBackground = true;
            daemonThread.Start();

#if !DEBUG
            //检测是否自启动
            if (!RegistryAutoStart.IsRegeditExit(LocalConfig.AppRegName,LocalConfig.AppPath))
            {
                RegistryAutoStart.SetRegistryKey(LocalConfig.AppRegName, LocalConfig.AppPath, true);
            }
#endif

        }

        private void InitWindow()
        {
            if (null == iotConfig)
                return;
            this.txb_iotIp.Text = iotConfig.IotIp;
            this.txb_iotPort.Text = iotConfig.IotPort.ToString();
            this.txb_iotAppID.Text = iotConfig.IotId.ToString();
            this.txb_iotGroupID.Text = iotConfig.IotGroupId.ToString();
            this.txb_receiveIotTimeOut.Text = iotConfig.ReceiveIotTimeOut.ToString();
            this.txb_iotRateIotTime.Text = iotConfig.RateIotTime.ToString();

            this.txb_iotWorkPath.Text = LocalConfig.GetIOTWorkDirectory();

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();

            if (receiveDataFlag)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    txb_iotStatus.Text = "IOT正常运行";
                    txb_iotStatus.Foreground = new SolidColorBrush(Colors.Green);
                }));
                return;
            }
            string errMsg = string.Empty;
                //没有接受到数据，重启iot
            bool bStartRet= StartExe(LocalConfig.GetIOTWorkDirectory(), LocalConfig.IOTProcessName,out errMsg);
            
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (bStartRet)
                {
                    txb_iotStatus.Text = "IOT已重新启动";
                    txb_iotStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                }
                else
                {
                    txb_iotStatus.Text = "IOT重新启动失败。"+errMsg;
                    txb_iotStatus.Foreground = new SolidColorBrush(Colors.Red);
                }
                
            }));
            

        }
        internal static bool StartExe(string processFileName, string processName,out string errMsg)
        {
            errMsg = string.Empty;
            FileInfo file = null;
            try
            {
                NLogHelper.DefalutInfo("    StartExe    开始运行外部程序**********");

                file = new FileInfo(processFileName);

                if (!file.Exists)
                {
                    NLogHelper.DefalutError("   StartExe not exists . file:{0}", processFileName);
                    errMsg = "启动程序文件不存在。";
                    return false;
                }

                Process[] arrayProcess = Process.GetProcessesByName(processName);
                foreach (Process p in arrayProcess)
                {
                    p.Kill();
                    //if (!p.Responding)
                    //{
                    //    p.Kill();
                    //}

                }

                //启动程序休眠3秒等待资源回收
                Thread.Sleep(3000);
                
              
            }
            catch (InvalidOperationException)
            {
                //程序已经退出
            }
            catch (Exception ex)
            {
                NLogHelper.DefalutError("   StartExe    程序 '{0}' 异常", processName);
                NLogHelper.ExceptionInfo(ex, "   StartExe    程序 '{0}' 异常{1} ", processName, ex.Message);
                errMsg = "启动程序异常："+ex.Message;
                return false;
            }


            try
            {
                if (file.Exists)
                {

                    NLogHelper.DefalutInfo("    StartExe    启动程序:{0}", processFileName);
                    // WinAPI_Interop.CreateProcess(file.FullName, file.DirectoryName);
                    Process process = new Process();
                    process.StartInfo.FileName = processFileName;
                    //process.StartInfo.UseShellExecute = true;
                    process.StartInfo.CreateNoWindow = false;
                    //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    process.StartInfo.WorkingDirectory = file.DirectoryName;
                    process.Start();
                }

                NLogHelper.DefalutInfo("    StartExe    成功运行外部程序**********");
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.DefalutError("   StartExe    程序 '{0}' 异常", processName);
                NLogHelper.ExceptionInfo(ex, "   StartExe    程序 '{0}' 异常{1} ", processName, ex.Message);
                errMsg = "启动程序异常：" + ex.Message;
                return false;
            }

        }



        void client_DataReceived(string device, int id, object value)
        {
            receiveDataFlag = true;
            daemonHelper.CloseIotConnected();
        }




        void DaemonThreadExecute()
        {
            receiveDataFlag = false;
            daemonHelper.ConnectIot();
            this.Dispatcher.Invoke(new Action(()=> 
            {
                txb_testIotTime.Text = DateTime.Now.ToString("G");
                txb_iotStatus.Text = "正在检测中……";
                txb_iotStatus.Foreground = new SolidColorBrush(Colors.AliceBlue);
            }));
            
            if (!timer.Enabled)
            {
                timer.Start();
            }
        }


        private void Window_Activated(object sender, EventArgs e)
        {
            if (App.Param == "/s")
            {
                Hide();
            }
        }
        /// <summary>
        /// 窗体状态变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            //最小化时隐藏
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }



    }
}
