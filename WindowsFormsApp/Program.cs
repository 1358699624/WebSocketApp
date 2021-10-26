using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
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
            Application.Run(new FormApp());
        }*/

        static void Main(string[] args)
        {
            WSocketClient client = new WSocketClient("ws://localhost:1234");
            client.MessageReceived += (data) => { Console.WriteLine("a=bcdefg"); } ;


            client.Start();

            Console.WriteLine("输入“c”，退出");
            var input = "";
            do
            {
                input = Console.ReadLine();
                //客户端发送消息到服务端
                client.SendMessage(input);
            } while (input != "c");
            client.Dispose();
            Console.ReadLine();
        }

    }
}
