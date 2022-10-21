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
        private string application = "RCmanager";
        private string SWVersion = "V2.012 - 21.10.2022";
        private string SWVersion_RelayCard = "";
        public string SW_Version
        {
            get { return SWVersion; }
        }
        public string SW_Version_RelayCard
        {
            get 
            {
                return SWVersion_RelayCard; 
            }
            set
            {
                SWVersion_RelayCard = value;
                lblVersionRelayCard.Text = "Relay card: " + SW_Version_RelayCard;
            }
        }
        public string Application
        {
            get { return application; }
        }
        public Version()
        {
            InitializeComponent();

            lblVersion.Text = SWVersion;
        }
    }
}
