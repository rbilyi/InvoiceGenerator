using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Nakladna.Updater
{
    public class Updater
    {
        private const string AssemblyVersionStart = @"\[assembly\: AssemblyVersion\(""(\d+\.\d+\.\d+\.\d+)""\)\]";

        public static bool IsUpdateNeeded()
        {
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var availableVersion = GetAvailableVersion();

            return currentVersion < availableVersion && MessageBox.Show("Є нова версія. Оновити зараз?", "Є нова версія", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes;
        }

        public static void Update()
        {
            var filePath = Path.ChangeExtension(Path.GetTempFileName(), Path.GetExtension(Settings.SetupFile));
            var progressForm = new DownloadProgressForm();
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        progressForm.ProgressBar.Value = e.ProgressPercentage;
                    };

                    wc.DownloadFileCompleted += (s, e) => progressForm.Close();

                    wc.DownloadFileAsync(new Uri(Settings.SetupFile), filePath);
                    progressForm.ShowDialog();
                    progressForm.Close();
                }

                if (File.Exists(filePath))
                {
                    Process.Start("msiexec", " /i " + filePath).WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static Version GetAvailableVersion()
        {
            var filePath = Path.GetTempFileName();
            var progressForm = new DownloadProgressForm();
            try
            {
                progressForm.Text = "Перевірка оновлень";

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        progressForm.ProgressBar.Value = e.ProgressPercentage;
                    };

                    wc.DownloadFileCompleted += (s, e) => progressForm.Close();

                    wc.DownloadFileAsync(new System.Uri(Settings.UpdateVersionFile), filePath);
                    progressForm.ShowDialog();
                }

                if (File.Exists(filePath))
                {
                    string fileText = File.ReadAllText(filePath);
                    var match = Regex.Match(fileText, AssemblyVersionStart);
                    return System.Version.Parse(match.Groups[1].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                File.Delete(filePath);
                progressForm.Close();
            }
            return null;
        }
    }
}
