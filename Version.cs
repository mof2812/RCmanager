using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Version
{
    public partial class Version : Form
    {
        private string SWVersion = "V2.000 - 03.10.2022";
        public string SW_Version
        {
            get { return SWVersion; }
        }

        public Version()
        {
            InitializeComponent();

            lblVersion.Text = SWVersion;
        }
    }
}
