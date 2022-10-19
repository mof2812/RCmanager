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
using System.Runtime.Remoting.Channels;

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
        public void Init_Monitoring(bool NightMode)
        {
            this.NightMode = NightMode;

            TriggerBackground.BackColor = NightMode ? Color.Black : Color.White;
            TriggerBackground.ForeColor = NightMode ? Color.White : Color.Black;
        }
        public bool InitRelayMonitoring(byte Index, Relay.SETTINGS_T Settings)
        {
            bool Error;

            Error = false;

            GetRelayMonitoring(Index).Settings = Settings;
            GetRelayMonitoring(Index).SignalName = Settings.SignalLabel;

            return Error;
        }
        public void Monitoring(float[] AnalogInputVoltage)
        {
            for (byte Index = 0; Index < RCmanager.Constants.IRQ_IOS; Index++)
            {
                GetTriggerMonitoring(Index).AnalogInputVoltage = AnalogInputVoltage[Index];
            }
        }
        public void Monitoring(UInt32 RelayStates)
        {
            for (byte Index = 0; Index < 16; Index++)
            {
                GetRelayMonitoring(Index).RelayStatus = Convert.ToBoolean(RelayStates & 0x00000001);
                RelayStates >>= 1;
            }
            RelayStates >>= 8;
            for (byte Index = 0; Index < RCmanager.Constants.IRQ_IOS; Index++)
            {
                GetTriggerMonitoring(Index).TriggerStatus = Convert.ToBoolean(RelayStates & 0x00000001);
                RelayStates >>= 1;
            }
        }
        public void Monitoring(uint OutputStates, float[] AnalogInputVoltage)
        {
            Monitoring(OutputStates);
            Monitoring(AnalogInputVoltage);
        }
        public bool NightMode
        {
            get
            {
                return Param.NightMode;
            }
            set
            {
                TriggerBackground.BackColor = this.BackColor = !value ? Color.White : Color.Black;
                
                for (byte Channel = 0; Channel < 16; Channel++)
                {
                    GetRelayMonitoring(Channel).NightMode = value;
                }

                for (byte Channel = 0; Channel < RCmanager.Constants.IRQ_IOS; Channel++)
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
        public Trigger.Trigger GetTriggerMonitoring(byte Channel)
        {
            Trigger.Trigger tm;

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
/*
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
*/
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
        public void SetSignalNames(Relay.SETTINGS_T[,] RelaySettings)
        {
            for (byte Module = 0; Module < RCmanager.Constants.MODULES; Module++)
            {
                for (byte Channel = 0; Channel < RCmanager.Constants.CHANNELS; Channel++)
                {
                    GetRelayMonitoring((byte)(Module * RCmanager.Constants.CHANNELS + Channel)).SignalName = RelaySettings[Module, Channel].SignalLabel;
                }
            }
        }
        public void SetTriggerName(byte Channel, string TriggerName)
        {
            GetTriggerMonitoring(Channel).TriggerName = TriggerName;
        }
    }
}
