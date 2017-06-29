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

            try
            {
                if (Updater.Updater.IsUpdateNeeded())
                {
                    Updater.Updater.Update();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Оновлення не вдалось.");
            }

            Application.Run(new MainForm());
        }
    }
}
