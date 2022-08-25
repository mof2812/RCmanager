using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelayCard16
{
    public struct PARAM_T
    {
        public bool NightMode;
    }
    public struct RELAYCARD_DATA_T
    {
        public string HW_Version;
    }
    public partial class RelayCard16 : UserControl
    {
        private PARAM_T Param;
        private RELAYCARD_DATA_T RCData;

        public RelayCard16()
        {
            InitializeComponent();

            Init();
        }
        public bool AddChannel(int Channel, string DataName, Color LineColor)
        {
            bool Error;
            RelayControl.RelayControl RC;

            Error = false;

            if (Channel < 16)
            {
                RC = GetRelayControl(Channel);

                RC.AddChannel(DataName, LineColor);
            }
            else
            {
                Error = true;
            }

            return Error;
        }

        public string BtnTextCard_1
        {
            get
            {
                return rbPage1.Text;
            }
            set
            {
                rbPage1.Text = value;
                rbPage2.Location = new System.Drawing.Point(rbPage1.Location.X + rbPage1.Width + 3, rbPage2.Location.Y);
            }
        }
        public string BtnTextCard_2
        {
            get
            {
                return rbPage2.Text;
            }
            set
            {
                rbPage2.Text = value;
                rbPage2.Location = new System.Drawing.Point(rbPage1.Location.X + rbPage1.Width + 3, rbPage2.Location.Y);
            }
        }
        public void Close()
        {
            SerialPort.Close();
        }
        private RelayControl.RelayControl GetRelayControl(int Channel)
        {
            RelayControl.RelayControl rc = new RelayControl.RelayControl();

            switch (Channel / 8)
            {
                case 0:
                    rc = rcCard1.GetRelayControl(Channel % 8);
                    break;

                case 1:
                    rc = rcCard2.GetRelayControl(Channel % 8);
                    break;

                default:
                    rc = null;
                    break;
            }
            return rc;
        }
        private void Init()
        {
            rbPage1.Checked = true;

            Param.NightMode = false;

            NightMode = true;

            Init_Serial();

            Init_Communication();
        }
        public void Init(int Channel, string DataName, RelayControl.MODE_T Mode, UInt32 DelayTime_ms, UInt32 TimeOn_ms, UInt32 TimeOff_ms, bool Inverted, Color LineColor)
        {
            if (DataName.Length == 0)
            {
                GetRelayControl(Channel).Init($"Kanal {Channel + 1}", Mode, DelayTime_ms, TimeOn_ms, TimeOff_ms, Inverted, LineColor);
            }
            else
            {
                GetRelayControl(Channel).Init(DataName, Mode, DelayTime_ms, TimeOn_ms, TimeOff_ms, Inverted, LineColor);
            }
        }
        public bool NightMode
        {
            get
            {
                return Param.NightMode;
            }
            set
            {
                rcCard1.NightMode = value;
                rcCard2.NightMode = value;
            }
        }
        private void rbPage1_CheckedChanged(object sender, EventArgs e)
        {
            rcCard1.Visible = rbPage1.Checked;
            rcCard2.Visible = !rbPage1.Checked;

            rbPage1.ForeColor = rbPage1.Checked? Color.Black : Color.Gray;
            rbPage2.ForeColor = rbPage1.Checked == false ? Color.Black : Color.Gray;
        }
        private void CommunicationTimer_Tick(object sender, EventArgs e)
        {
            CommunicationTask();
        }

        private void RelayCard16_Leave(object sender, EventArgs e)
        {

        }
    }
}

