using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelayMonitoring
{
    public struct PARAM_T
    {
        public bool NightMode;
    }
    public partial class RelayMonitoring : UserControl
    {
        public Relay.SETTINGS_T Settings;

        public RelayMonitoring()
        {
            InitializeComponent();

            Init();
        }
        private void Init()
        {
            Init_Led();
        }
        private void Init_Led()
        {
            ledStatus.On = false;
        }
        public bool NightMode
        {
            set
            {
                this.BackColor = value ? Color.Black : Color.White;
                lblSignalName.ForeColor = value ? Color.Cyan : Color.DarkGray;
            }
        }
        public bool RelayStatus
        {
            get
            {
                return ledStatus.On;
            }
            set 
            { 
                ledStatus.On = value; 
            }
        }
        public string SignalName
        {
            get
            {
                return lblSignalName.Text;
            }
            set
            {
                lblSignalName.Text = value;
            }
        }
    }
}
