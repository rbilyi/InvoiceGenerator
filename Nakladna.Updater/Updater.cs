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
        private const string AssemblyVersionStart = @"\[assembly\: AssemblyVersion\(""(\d+\.\d+(\.\d+)*(\.\d+)*)""\)\]";

        public static bool IsUpdateNeeded()
        {
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var availableVersion = GetAvailableVersion();

            return currentVersion < availableVersion
                && MessageBox.Show("Є нова версія програми (" + availableVersion + "). Оновити зараз?", "Є нова версія", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes;
        }

        public static void Update()
        {
            var setupFileUrl = Configuration.Develop ? Settings.SetupFile_Develop : Settings.SetupFile;
            var tempFilePath = Path.ChangeExtension(Path.GetTempFileName(), Path.GetExtension(Settings.SetupFile));
            var progressForm = new DownloadProgressForm("Скачую нову версію програми...");
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        progressForm.ProgressBar.Value = e.ProgressPercentage;
                    };

                    progressForm.ProgressBar.Style = ProgressBarStyle.Blocks;
                    wc.DownloadFileCompleted += (s, e) => progressForm.Close();

                    wc.DownloadFileAsync(new Uri(setupFileUrl), tempFilePath);
                    progressForm.ShowDialog();
                    progressForm.Close();
                }

                if (File.Exists(tempFilePath))
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = tempFilePath,
                        UseShellExecute = true,
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static Version GetAvailableVersion()
        {
            var remoteVersionFile = Configuration.Develop ? Settings.UpdateVersionFile_Develop : Settings.UpdateVersionFile;
            var filePath = Path.GetTempFileName();
            var progressForm = new DownloadProgressForm("Перевірка оновлень");
            try
            {
                using (WebClient wc = new WebClient())
                {
                    progressForm.ProgressBar.Style = ProgressBarStyle.Marquee;
                    wc.DownloadFileCompleted += (s, e) => progressForm.Close();

                    wc.DownloadFileAsync(new System.Uri(remoteVersionFile), filePath);
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
