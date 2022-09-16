using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace RelayCardMonitoring
{ 
    public struct PARAM_T
    {
        public bool NightMode;
        public SerialPort ComPort;
        public byte CardIndex;
    }
    public partial class RelayCardMonitoring : UserControl
    {
        private PARAM_T Param;

        public RelayCardMonitoring()
        {
            InitializeComponent();

            Init();
        }
        private void Init()
        {
        }
        public void Monitoring(UInt32 RelayStates)
        {
            for (byte Index = 0; Index < 16; Index++)
            {
                GetRelayMonitoring(Index).RelayStatus = Convert.ToBoolean(RelayStates & 0x00000001);
                RelayStates >>= 1;
            }
            RelayStates >>= 8;
            for (byte Index = 0; Index < 8; Index++)
            {
                GetTriggerMonitoring(Index).TriggerStatus = Convert.ToBoolean(RelayStates & 0x00000001);
                RelayStates >>= 1;
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
                this.BackColor = value ? Color.White : Color.Black;
                for (byte Channel = 0; Channel < 16; Channel++)
                {
                    GetRelayMonitoring(Channel).NightMode = value;
                }

                for (byte Channel = 0; Channel < 8; Channel++)
                {
                    GetTriggerMonitoring(Channel).NightMode = value;
                }
            }
        }
        private RelayMonitoring.RelayMonitoring GetRelayMonitoring(byte Channel)
        {
            RelayMonitoring.RelayMonitoring rm;

            rm = null;

            if (Channel / 8 == 0)
            {
                switch(Channel % 8)
                {
                    case 0: 
                        rm = rmM1_CH1;
                        break;
                    case 1:
                        rm = rmM1_CH2;
                        break;
                    case 2:
                        rm = rmM1_CH3;
                        break;
                    case 3:
                        rm = rmM1_CH4;
                        break;
                    case 4:
                        rm = rmM1_CH5;
                        break;
                    case 5:
                        rm = rmM1_CH6;
                        break;
                    case 6:
                        rm = rmM1_CH7;
                        break;
                    case 7:
                        rm = rmM1_CH8;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (Channel % 8)
                {
                    case 0:
                        rm = rmM2_CH1;
                        break;
                    case 1:
                        rm = rmM2_CH2;
                        break;
                    case 2:
                        rm = rmM2_CH3;
                        break;
                    case 3:
                        rm = rmM2_CH4;
                        break;
                    case 4:
                        rm = rmM2_CH5;
                        break;
                    case 5:
                        rm = rmM2_CH6;
                        break;
                    case 6:
                        rm = rmM2_CH7;
                        break;
                    case 7:
                        rm = rmM2_CH8;
                        break;
                    default:
                        break;
                }
            }

            return rm;
        }
        private TriggerMonitoring.TriggerMonitoring GetTriggerMonitoring(byte Channel)
        {
            TriggerMonitoring.TriggerMonitoring tm;

            tm = null;

            if (Channel < 8)
            {
                switch (Channel)
                {
                    case 0:
                        tm = tm_CH1;
                        break;
                    case 1:
                        tm = tm_CH2;
                        break;
                    case 2:
                        tm = tm_CH3;
                        break;
                    case 3:
                        tm = tm_CH4;
                        break;
                    case 4:
                        tm = tm_CH5;
                        break;
                    case 5:
                        tm = tm_CH6;
                        break;
                    case 6:
                        tm = tm_CH7;
                        break;
                    case 7:
                        tm = tm_CH8;
                        break;
                    default:
                        break;
                }
            }
            return tm;
        }
        public void SetSignalName(byte Channel, string SignalName)
        {
            GetRelayMonitoring(Channel).SignalName = SignalName;
        }
        public void SetTriggerName(byte Channel, string TriggerName)
        {
            GetTriggerMonitoring(Channel).TriggerName = TriggerName;
        }
    }
}
