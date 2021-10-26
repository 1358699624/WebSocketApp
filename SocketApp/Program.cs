using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketApp
{
    internal static class Program
    {
        /*
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        */
        static void Main(string[] args)
        {
            ScoketBLL scoketBLL = new ScoketBLL();
            Console.WriteLine(scoketBLL.Start()?"服务端启动成功": "服务端启动失败");
            Console.Read();
        }
    }
}
