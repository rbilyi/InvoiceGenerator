using System.Windows.Forms;

namespace Nakladna.Updater
{
    public partial class DownloadProgressForm : Form
    {
        public DownloadProgressForm()
        {
            InitializeComponent();
        }

        public DownloadProgressForm(string message) : this()
        {
            if (DesignMode)
                return;

            if (!string.IsNullOrEmpty(message))
                label1.Text = message;
        }

        public ProgressBar ProgressBar
        {
            get
            {
                return progressBar1;
            }
        }
    }
}
