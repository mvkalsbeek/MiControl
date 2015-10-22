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
        MiController Controller = new MiController("255.255.255.255");
        ScreenColor Ambilight = new ScreenColor();

        public MiForm()
        {
            InitializeComponent();
        }
        
        void MiFormLoad(object sender, EventArgs e)
		{
        	cmbGroup.SelectedIndex = 0;
		}
        
        void ItemIPAddressLeave(object sender, EventArgs e)
		{
        	Controller = new MiController(itemIPAddress.Text);
		}
        
        private void btnOn1_Click(object sender, EventArgs e)
        {
            Controller.RGBSwitchOn(1);
        }
        
        private void btnOff1_Click(object sender, EventArgs e)
        {
        	Controller.RGBSwitchOff(1);
        }
        
        private void trackBrightness1_Scroll(object sender, EventArgs e)
        {
            Controller.RGBSetBrightness(1, trackBrightness1.Value);
        }
        
        private void btnOn2_Click(object sender, EventArgs e)
        {
            Controller.RGBSwitchOn(2);
        }
        
        private void btnOff2_Click(object sender, EventArgs e)
        {
        	Controller.RGBSwitchOff(2);
        }
        
        private void trackBrightness2_Scroll(object sender, EventArgs e)
        {
            Controller.RGBSetBrightness(2, trackBrightness2.Value);
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
        
        private void btnOn4_Click(object sender, EventArgs e)
        {
            Controller.RGBSwitchOn(4);
        }
        
        private void btnOff4_Click(object sender, EventArgs e)
        {
        	Controller.RGBSwitchOff(4);
        }
        
        private void trackBrightness4_Scroll(object sender, EventArgs e)
        {
            Controller.RGBSetBrightness(4, trackBrightness4.Value);
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

            while(!bwAmbi.CancellationPending) {
                color = Ambilight.AverageColor();
                btnAmbi.BackColor = color;
                btnAmbi.Invoke((MethodInvoker)delegate {
                    btnAmbi.Text = string.Format("H:{0} S:{1} L:{2}", 
                        color.GetHue(),
                        color.GetSaturation(),
                        color.GetBrightness());
                });

                Controller.RGBSetColor(cmbGroup.SelectedIndex, color);
                Thread.Sleep(50);
            }

            btnAmbi.BackColor = SystemColors.Control;
            btnAmbi.Text = "Ambi";
        }	
    }
}
