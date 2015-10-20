using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiControl;

namespace MiControlGUI
{
    public partial class MiForm : Form
    {
        MiController Controller = new MiController("192.168.178.13");
        ScreenColor Ambilight = new ScreenColor();

        public MiForm()
        {
            InitializeComponent();
        }

        private void btnOn3_Click(object sender, EventArgs e)
        {
            Controller.RGBSwitchOn(3);
        }

        private void btnOff3_Click(object sender, EventArgs e)
        {
            Controller.RGBSwitchOff(3);
        }

        private void trackBrightness3_Scroll(object sender, EventArgs e)
        {
            Controller.RGBSetBrightness(3, trackBrightness3.Value);
        }

        private void btnOn1_Click(object sender, EventArgs e)
        {
            Controller.RGBSwitchOn(1);
        }

        private void btnAmbi_Click(object sender, EventArgs e)
        {
            if(!bwAmbi.IsBusy) {
                bwAmbi.RunWorkerAsync();
            } else {
                bwAmbi.CancelAsync();
            }
        }

        private void bwAmbi_DoWork(object sender, DoWorkEventArgs e)
        {
            Color color;
            int brightness;

            while(!bwAmbi.CancellationPending) {
                color = Ambilight.AverageColor();
                btnAmbi.BackColor = color;
                btnAmbi.Invoke((MethodInvoker)delegate {
                    btnAmbi.Text = string.Format("H:{0} S:{1} L:{2}", 
                        color.GetHue(),
                        color.GetSaturation(),
                        color.GetBrightness());
                });

                brightness = (int)(color.GetBrightness() * 100);

                if (color.GetSaturation() > 0.2 && color.GetBrightness() < 0.85 ) {
                	Controller.RGBSetHue(3, color.GetHue());
                    Thread.Sleep(50);
                } else { 
                    Controller.RGBSwitchWhite(3);
                    if (color.GetSaturation() <= 0.2) {
                        brightness = (int)(color.GetSaturation() * 100) + 10;
                    }
                }

                Thread.Sleep(50);
                Controller.RGBSetBrightness(3, brightness);
            }

            btnAmbi.BackColor = SystemColors.Control;
            btnAmbi.Text = "Ambi";
        }
    }
}
