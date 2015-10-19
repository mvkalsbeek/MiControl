using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiControl;

namespace MiControlGUI
{
    public partial class MiForm : Form
    {
        MiController Controller = new MiController("192.168.178.13");

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
    }
}
