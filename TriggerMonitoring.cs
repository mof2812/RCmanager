using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriggerMonitoring
{
    public struct PARAM_T
    {
        public bool NightMode;
    }
    public partial class TriggerMonitoring : UserControl
    {
        protected PARAM_T Param;

        public TriggerMonitoring()
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
            ledTrigger.On = false;
        }
        public bool NightMode
        {
            get
            {
                return Param.NightMode;
            }
            set
            {
                this.BackColor = value ? Color.Black : Color.White;
                lblTriggerName.ForeColor = value ? Color.Yellow : Color.Black;
            }
        }
        public bool TriggerStatus
        {
            get
            {
                return ledTrigger.On;
            }
            set 
            { 
                ledTrigger.On = value; 
            }
        }
        public string TriggerName
        {
            get
            {
                return lblTriggerName.Text;
            }
            set
            {
                lblTriggerName.Text = value;
            }
        }
    }
}
