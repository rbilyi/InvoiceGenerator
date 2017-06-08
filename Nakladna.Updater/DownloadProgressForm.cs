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
    }
}
