using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace DelSeewoEasiRecorder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        protected override void SetVisibleCore(bool value)
        {
            // 记录启动日期
            string filePath = "logs.txt";
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (File.Exists(filePath))
            {
                File.AppendAllText(filePath, "\n" + currentDateTime);
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(currentDateTime);
                }
            }

            //在星期一的时候设置删除目录并执行
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                string filename = @"D:\EasiRecorder";
                try
                {
                    Directory.Delete(filename, true);
                    File.AppendAllText(filePath, $"\n{DateTime.Now:yyyy-MM-dd HH:mm:ss} 删除目录 {filename} 成功");
                }
                catch (Exception ex)
                {
                    File.AppendAllText(filePath, $"\n{DateTime.Now:yyyy-MM-dd HH:mm:ss} 删除目录 {filename} 失败，错误信息：{ex.Message}");
                }
                Directory.CreateDirectory(filename);
            }
            else
            {
                string filename = @"D:\EasiRecorder";
                File.AppendAllText(filePath, $"\n{DateTime.Now:yyyy-MM-dd HH:mm:ss} 不是星期一，不执行删除目录 {filename} 操作");
            }

            //设置程序开机自启动项
            string strName = AppDomain.CurrentDomain.BaseDirectory + "DelSeewoEasiRecorder.exe";
            if (!System.IO.File.Exists(strName))
                return;
            string strnewName = strName.Substring(strName.LastIndexOf("\\") + 1);
            RegistryKey registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (registry == null)
                registry = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            registry.SetValue(strnewName, strName);

            // 关闭窗口
            base.Close();
        }

    }
}
