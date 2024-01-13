using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RVP_3_Yurlova
{
    public partial class ProgressBarForm : System.Windows.Forms.Form
    {
        public ProgressBarForm()
        {
            InitializeComponent();
        }
        public string BrandName { get; set; }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            string brandName = BrandName;

            progressBar1.Value = Loader.GetProgress(brandName);

            if (progressBar1.Value >= 100)
            {
                timer1.Stop();
            }
        }

        private void ProgressBarForm_Load(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Blocks;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;

            timer1.Interval = 100;
            timer1.Start();
        }
    }
}
