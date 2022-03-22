using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMMS
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {

        }

        private void timerSplashScreen_Tick(object sender, EventArgs e)
        {
            panel3.Width += 3;

            if(panel3.Width >= 778)
            {
                timerSplashScreen.Stop();
                LoginForm lf = new LoginForm();
                lf.Show();
                this.Hide();
            }
        }
    }
}
