using System;
using System.Windows.Forms;

namespace Nakladna
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
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            if (Updater.Updater.IsUpdateNeeded())
            {
                Application.Run(Updater.Updater.UpdateForm());
            }
            else
            {
                Application.Run(new MainForm());
            }
        }
    }
}
