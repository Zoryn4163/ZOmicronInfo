using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZOmicronInfo
{
    public partial class FormMain : Form
    {
        public bool ShowingConsole { get; set; }

        public void EnableFromConsole()
        {
            Program.FreeConsole();
            Program.UsingGui = true;
            Opacity = 1;
            ShowInTaskbar = true;
            Show();
        }

        public FormMain()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
        }

        private void btnConsole_Click(object sender, EventArgs e)
        {
            if (ShowingConsole)
            {
                Program.ConsoleThread.Abort();
                Program.FreeConsole();
                Log.StopWriting();
            }
            else
            {
                Program.ConsoleThread = new Thread(Program.UseConsole);
                Program.ConsoleThread.Start();
                this.Hide();
            }
            ShowingConsole = !ShowingConsole;
        }
    }
}
