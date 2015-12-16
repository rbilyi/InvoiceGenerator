using System;
using System.IO;
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
            //string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //string path = (System.IO.Path.GetDirectoryName(executable));
            //AppDomain.CurrentDomain.SetData("DataDirectory", path;)
            //SpreadSheet.Go();
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Application.Run(new StartForm());
            Application.Run(new MainForm());
        }
    }
}
