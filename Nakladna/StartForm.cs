using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nakladna.Core;

namespace Nakladna
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        internal void FillSteps(IEnumerable<StartingStep> steps)
        {
            foreach (var step in steps)
            {
                var item = new ListViewItem();
                item.Text = step.Title;
                item.Tag = step;
                listView1.Items.Add(item);
                RunStep(step, item);
            }
        }

        private async void RunStep(StartingStep step, ListViewItem item)
        {
            bool r = await step.RunAsync();
            if (r)
                item.ForeColor = Color.LightGreen;
            else
            {
                item.ForeColor = Color.LightCoral;
                item.ToolTipText = step.Message;
            }

            var steps = listView1.Items.Cast<ListViewItem>().Select(i => i.Tag as StartingStep);
            if (steps.All(i => i.Completed))
            {
                if (steps.All(i => !i.Failed))
                {
                    Close();
                }
                label1.Text = "Щось не так.";
            }
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            var steps = new List<StartingStep>()
            {
                new StartingStep("База даних",
                ()=> {
                    InvoiceCore.Instance.GetGoods(new DbScope()).ToList();
                    return true; }),

                new StartingStep("Шаблон накладної",
                ()=> {
                   using (var stream = File.OpenRead(Path.Combine(Application.StartupPath, Nakladna.Settings.TemplateFileName)))
                    {
                        if (stream.Length > 0)
                            return true;
                    }
                    throw new Exception("Incorrect template data. Stream length = 0.");}),

                new StartingStep("Google account",
                    ()=> { return true; })
            };

            FillSteps(steps);
        }
    }
}
