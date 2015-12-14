using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Nakladna.CommonData;

namespace Nakladna.Core
{
    public class AutoDBUpdater
    {
        public Action<string> FileChanged;
        public Action<string> FileRenamed;

        public GoodType GoodType;
        public string filePath;
        public string producer;

        FileSystemWatcher watcher;

        public AutoDBUpdater(GoodType goodType, string filePath, string producer)
        {
            this.GoodType = goodType;
            this.filePath = filePath;
            this.producer = producer;
        }

        public Tuple<bool, byte[]> ChechFileModified(string filePath, byte[] hash)
        {
            var oldHash = hash;

            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                hash = md5.ComputeHash(stream);
                return new Tuple<bool,byte[]>(!oldHash.SequenceEqual(hash), hash);
            }
        }

        public void StartWatchingFile(bool recreateWatcher = false)
        {
            if (recreateWatcher || watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= file_Changed;
                watcher.Renamed -= file_Renamed;
                watcher = null;
            }

            if (watcher == null)
                watcher = new FileSystemWatcher(filePath);

            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            watcher.Changed += file_Changed;
            watcher.Renamed += file_Renamed;

            watcher.EnableRaisingEvents = true;
        }

        private void file_Renamed(object sender, RenamedEventArgs e)
        {
            if (FileRenamed != null)
                FileRenamed(e.FullPath);
        }

        private void file_Changed(object sender, FileSystemEventArgs e)
        {
            if (FileChanged != null)
                FileChanged(e.FullPath);
        }
    }
}
