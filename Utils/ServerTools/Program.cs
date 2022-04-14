using System;
using System.Windows.Forms;

namespace ServerTools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool result;
            var mutex = new System.Threading.Mutex(true, "CSGOServerTools", out result);

            if (!result)
            {
                MessageBox.Show("Another instance is already running.");
                return;
            }
            Application.Run(new frmMain());
            GC.KeepAlive(mutex);

        }
    }
}
