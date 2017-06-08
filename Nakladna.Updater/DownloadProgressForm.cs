using System.Windows.Forms;

namespace Nakladna.Updater
{
    public partial class DownloadProgressForm : Form
    {
        public DownloadProgressForm()
        {
            InitializeComponent();
        }

        public ProgressBar ProgressBar
        {
            get
            {
                return progressBar1;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                Close();
            }
            base.Dispose(disposing);

        }
    }
}
