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

namespace RelayControl8
{
    public struct PARAM_T
    {
        public bool NightMode;
        public SerialPort ComPort;
        public byte CardIndex;
    }
    public partial class RelayControl8 : UserControl
    {
        private PARAM_T Param;
        public RelayControl8()
        {
            InitializeComponent();
        }
        public void AddSerialPort(byte CardIndex, ref SerialPort Port)
        {
            Param.ComPort = Port;
            Param.CardIndex = CardIndex;

            for (int Channel = 0; Channel < 8; Channel++)
            {
                GetRelayControl(Channel).AddSerialPort((byte)(CardIndex * 8 + Channel), ref Port);
            }
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
                for (byte Channel = 0; Channel < 8; Channel++)
                {
                    GetRelayControl(Channel).NightMode = value;
                    this.BackColor = value? Color.White : Color.Black;
                }
            }
        }
        public RelayControl.RelayControl GetRelayControl(int Channel)
        {
            RelayControl.RelayControl rc = new RelayControl.RelayControl();

            switch (Channel)
            {
                case 0:
                    rc = rcCH1;
                    break;

                case 1:
                    rc = rcCH2;
                    break;

                case 2:
                    rc = rcCH3;
                    break;

                case 3:
                    rc = rcCH4;
                    break;

                case 4:
                    rc = rcCH5;
                    break;

                case 5:
                    rc = rcCH6;
                    break;

                case 6:
                    rc = rcCH7;
                    break;

                case 7:
                    rc = rcCH8;
                    break;
/*
                case 8:
                    rc = rcCH9;
                    break;

                case 9:
                    rc = rcCH10;
                    break;

                case 10:
                    rc = rcCH11;
                    break;

                case 11:
                    rc = rcCH12;
                    break;

                case 12:
                    rc = rcCH13;
                    break;

                case 13:
                    rc = rcCH14;
                    break;

                case 14:
                    rc = rcCH15;
                    break;

                case 15:
                    rc = rcCH16;
                    break;
*/
                default:
                    rc = null;
                    break;
            }

            return rc;
        }
    }
}
