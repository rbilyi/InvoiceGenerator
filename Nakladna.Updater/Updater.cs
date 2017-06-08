using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nakladna.Updater
{
    public class Updater
    {
        private const string AssemblyVersionStart = @"\[assembly\: AssemblyVersion\(""(\d+\.\d+\.\d+\.\d+)""\)\]";

        public static async Task<bool> IsUpdateNeeded()
        {
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var availableVersion = await GetAvailableVersion();

            return currentVersion < availableVersion && MessageBox.Show("Є нова версія. Оновити зараз?", "Є нова версія", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes;
        }
        public static async Task UpdateToNewVersion()
        {
            var filePath = Path.GetTempPath() + Path.GetTempFileName();
            try
            {
                using (WebClient wc = new WebClient())
                using (var progressForm = new DownloadProgressForm())
                {
                    progressForm.Show();
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        progressForm.ProgressBar.Value = e.ProgressPercentage;
                    };

                    await wc.DownloadFileTaskAsync(new Uri(Settings.SetupFile), filePath);
                }

                if (File.Exists(filePath))
                {
                    Process.Start(filePath);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        public static async Task<Version> GetAvailableVersion()
        {
            var filePath = Path.GetTempPath() + Path.GetTempFileName();
            try
            {
                using (WebClient wc = new WebClient())
                using (var progressForm = new DownloadProgressForm())
                {
                    progressForm.Text = "Перевірка оновлень";
                    progressForm.Show();
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        progressForm.ProgressBar.Value = e.ProgressPercentage;
                    };

                    await wc.DownloadFileTaskAsync(new System.Uri(Settings.UpdateVersionFile), filePath);
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
            }
            finally
            {
                File.Delete(filePath);
            }
            return null;
        }
    }
}
