using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class FormApp : Form
    {
        public FormApp()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            WSocketClient client = new WSocketClient("wss://");
            client.MessageReceived += (data) => { 
                Console.WriteLine(data); 
            };


            client.Start();

            var input = tichtextsend.Text;

            client.SendMessage(input);
            client.Dispose();
        }

 
    }
}
