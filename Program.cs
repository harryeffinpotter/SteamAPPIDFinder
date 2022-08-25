using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamAppIdIdentifier
{
    static class Program
    {
        public static string[] args2;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {

            args2 = args;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SteamAppId());
        }
    }
}
