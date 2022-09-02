using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamAppIdIdentifier
{
   internal static class Program
    {
        public static Mutex mutex;
        public static SteamAppId form;
        public static string[] args2;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            bool mutexCreated = false;
            mutex = new System.Threading.Mutex(false, "APPID.exe", out mutexCreated);

            if (!mutexCreated)
            {
                MessageBox.Show(new Form { TopMost = true }, "Steam APPID finder is already running!!", "Already Runing!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mutex.Close();
                Application.Exit();
                return;
            }
            try
            {
                // Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                form = new SteamAppId();
                args2 = args;
                Application.Run(form);
                Application.Exit();
            }
            catch { }
        }
    }
}
