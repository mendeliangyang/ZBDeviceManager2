using System;
using System.Windows;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using Application = System.Windows.Application;
using StartupEventArgs = System.Windows.StartupEventArgs;

namespace IOTDataCentreDaemon
{
    public static class StartUp
    {
        [STAThread]
        public static void Main(string[] args)
        {
            SingleInstanceManager manager = new SingleInstanceManager();
            manager.Run(args);
        }
    }

    public class SingleInstanceManager : WindowsFormsApplicationBase
    {
        private App app;

        public SingleInstanceManager()
        {
            this.IsSingleInstance = true;
        }

        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
        {
            // First time app is launched
            app = new App();
            app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            // Subsequent launches
            base.OnStartupNextInstance(eventArgs);
            app.Activate();
        }
    }

    /// <summary>
    ///   Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string Param = "";

        /// <summary>
        /// 图盘图标
        /// </summary>
        private static NotifyIcon _trayIcon;

        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var main = new MainWindow();
            RemoveTrayIcon();
            AddTrayIcon();

            MainWindow = main;
            MainWindow.Hide();
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            RemoveTrayIcon();
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        public void Activate()
        {
            // Reactivate application's main window
            MainWindow.Show();
            MainWindow.Activate();
        }

        /// <summary>
        /// 显示托盘图标
        /// </summary>
        private void AddTrayIcon()
        {
            if (_trayIcon != null)
            {
                return;
            }
            _trayIcon = new NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath),
                Text = "中邦IOT守护程序服务"
            };
            _trayIcon.Visible = true;
            _trayIcon.BalloonTipText = "中邦IOT守护程序运行中... ...";
            _trayIcon.ShowBalloonTip(2000);

            ContextMenu menu = new ContextMenu();

            //“关闭”菜单项
            MenuItem closeItem = new MenuItem();
            closeItem.Text = "关闭服务";
            closeItem.Click += new EventHandler(delegate
            {
                if (System.Windows.MessageBox.Show("您确定要停止中邦IOT守护程序服务并退出吗?",
                    "中邦IOT守护程序",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    this.Shutdown();
                }
            });

            //“显示窗口”菜单项
            MenuItem settingItem = new MenuItem();
            settingItem.Text = "查看IOT守护程序配置";
            settingItem.Click += new EventHandler(delegate { this.Activate(); });

            menu.MenuItems.Add(settingItem);
            menu.MenuItems.Add(closeItem);

            //设置NotifyIcon的右键弹出菜单
            _trayIcon.ContextMenu = menu;

            //设置双击显示事件
            _trayIcon.MouseDoubleClick += new MouseEventHandler((o, e) => {
                if (e.Button == MouseButtons.Left) this.Activate();
            });
        }

        /// <summary>
        /// 清除托盘图标
        /// </summary>
        private void RemoveTrayIcon()
        {
            if (_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
                _trayIcon = null;
            }
        }
    }
}
