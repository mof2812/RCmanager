using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCmanager
{
    public enum RETURN_T
    {
        OKAY = 0,
        ERROR,
        INITIALIZE_ERROR,
        PARAM_ERROR,
        PARAM_MISSING,
        PARAM_OUT_OF_RANGE,
        INVALIDE_CHANNEL,
    }
    public partial class RCmanager : Form
    {
        public struct PARAMS_T
        {
            public bool Initialized;
            public double MaxChartXRange_ms;
            public double MinChartXRange_ms;
        }

        #region Members
        private bool Initialized = false;
        private PARAMS_T Params;
        #endregion
        #region Properties
        #endregion
        #region Constructors, destructors
        public RCmanager()
        {
            InitializeComponent();

            Init();
        }
        #endregion
        #region Methods
        private RETURN_T GetConfig(byte Channel)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (Channel == 255)
            {
                for (Channel = 0; Channel < (Constants.MODULES * Constants.CHANNELS); Channel++)
                {
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.GET_CONFIG, Channel, 0, 0, 0, 0, false);
                }
            }
            else if (Channel < (Constants.MODULES * Constants.CHANNELS))
            {
                serialCommunication.SetAction(SerialCommunication.ACTION_T.GET_CONFIG, Channel, 0, 0, 0, 0, false);
            }
            else
            {
                Return = RETURN_T.INVALIDE_CHANNEL;
            }

            return Return;
        }
        private RETURN_T GetRelayParameter(byte Channel, ref Relay.PARAMS_T RelayParams)
        {
            RETURN_T Return;
            byte Module;
            byte RelayChannel;

            Return = RETURN_T.OKAY;
            Module = (byte)(Channel / Constants.CHANNELS);
            RelayChannel = (byte)(Channel % Constants.CHANNELS);

            switch (Module)
            {
                case 0:
                    Return = (RETURN_T)relayCard.GetRelayParameter(RelayChannel, ref RelayParams);
                    break;

                case 1:
                    Return = (RETURN_T)relayCardPowerSupply.GetRelayParameter(RelayChannel, ref RelayParams);
                    break;

                default:
                    Return = RETURN_T.INVALIDE_CHANNEL;
                    break;
            }

            return Return;
        }
        private RETURN_T GetSignalName(byte Channel, ref string SignalName)
        {
            RETURN_T Return;
            byte Module;
            byte RelayChannel;

            Return = RETURN_T.OKAY;
            SignalName = "";
            Module = (byte)(Channel / Constants.CHANNELS);
            RelayChannel = (byte)(Channel % Constants.CHANNELS);

            switch (Module)
            {
                case 0:
                    Return = (RETURN_T)relayCard.GetSignalName(RelayChannel, ref SignalName);
                    break;

                case 1:
                    Return = (RETURN_T)relayCardPowerSupply.GetSignalName(RelayChannel, ref SignalName);
                    break;

                default:
                    Return = RETURN_T.INVALIDE_CHANNEL;
                    break;
            }

            return Return;
        }
        private RETURN_T Init()
        {
            RETURN_T Return;

            Return = RETURN_T.INITIALIZE_ERROR;

            this.Text = $"{this.Text} - {SW_Version}";

            if (Init_Settings() == RETURN_T.OKAY)
            {
                if (Init_RelayCards() == RETURN_T.OKAY)
                {
                    if (Init_Menu() == RETURN_T.OKAY)
                    {
                        if (Init_Communication() == RETURN_T.OKAY)
                        {
                            Return = RETURN_T.OKAY;
                        }
                    }
                }
            }

            if (Return == RETURN_T.OKAY)
            {
                // Init the monitoring routines
                Init_Monitoring();
                // Get the configuration of all relays
                GetConfig(255);
            }
            return Return;
        }
        private RETURN_T Init_Communication()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            serialCommunication.Init_Communication();

            serialCommunication.SetAction(SerialCommunication.ACTION_T.GET_OUTPUT_STATES, true);

            // AddEvent
            serialCommunication.FrameReceived += FrameReceived;

            return Return;
        }
        private RETURN_T Init_Limits()
        {
            RETURN_T Return;
            
            Return = RETURN_T.OKAY;

            Params.MaxChartXRange_ms = relayCard.MaxChartXRange_ms;
            Params.MinChartXRange_ms = relayCard.MinChartXRange_ms;

            Params.MaxChartXRange_ms = relayCardPowerSupply.MaxChartXRange_ms > Params.MaxChartXRange_ms ? relayCardPowerSupply.MaxChartXRange_ms : Params.MaxChartXRange_ms;
            Params.MinChartXRange_ms = relayCardPowerSupply.MinChartXRange_ms < Params.MinChartXRange_ms ? relayCardPowerSupply.MinChartXRange_ms : Params.MinChartXRange_ms;

            SetAxisLimit(255, Relay.AXIS_SELECT_T.X, Params.MaxChartXRange_ms, false);
            SetAxisLimit(255, Relay.AXIS_SELECT_T.X, Params.MinChartXRange_ms, true);

            return Return;
        }
        private RETURN_T Init_Menu()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            menuMainSettingsViewNightMode.Checked = Properties.Settings.Default.NightMode;

            return Return;
        }
        private RETURN_T Init_Monitoring()
        {
            RETURN_T Return;
            string SignalName;
            byte Index;
            Relay.PARAMS_T RelayParams = new Relay.PARAMS_T();

            Return = RETURN_T.OKAY;
            SignalName = "";

            relayCardMonitoring1.InitMonitoring(menuMainSettingsViewNightMode.Checked);
//            relayCardMonitoring2.InitMonitoring(menuMainSettingsViewNightMode.Checked);

            for (Index = 0; (Index < Constants.MODULES * Constants.CHANNELS) && (Return == RETURN_T.OKAY); Index++)
            {
                if (Return == RETURN_T.OKAY)
                {
                    Return = GetRelayParameter(Index, ref RelayParams);

                    if (Return == RETURN_T.OKAY)
                    {
                        relayCardMonitoring1.InitRelayMonitoring(Index, RelayParams);
                    }
                }
            }

            return Return;
        }
        private RETURN_T Init_Part_2()
        {
            // Relay parameter read
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            Init_Limits();

            return Return;
        }
        private RETURN_T Init_RelayCards()
        {
            RETURN_T Return;

            Return = RETURN_T.INITIALIZE_ERROR;

            if (relayCard.Init(0, Properties.Settings.Default.NightMode) == RelayCard.RETURN_T.OKAY)
            {
                relayCard.SetParameterRelayCard += SetParameter;

                if (relayCardPowerSupply.Init(1, Properties.Settings.Default.NightMode) == RelayCard.RETURN_T.OKAY)
                {
                    relayCardPowerSupply.SetParameterRelayCard += SetParameter;

                    return RETURN_T.OKAY;
                }
            }
            return Return;
        }
        private RETURN_T Init_Settings()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (Properties.Settings.Default.InitCode != Constants.INITCODE)
            {
                SetDefault();

                Properties.Settings.Default.InitCode = Constants.INITCODE;
                Properties.Settings.Default.Save();
            }
            return Return;
        }
        private void NightMode(bool Set)
        {
            Properties.Settings.Default.NightMode = Set;
            Properties.Settings.Default.Save();

            this.BackColor = !Set ? Properties.Settings.Default.BackColor : Properties.Settings.Default.BackColor_NM;

            relayCard.NightMode(Set);
            relayCardPowerSupply.NightMode(Set);
        }
        public RETURN_T SetAxisLimit(byte Channel, Relay.AXIS_SELECT_T Axis, double Limit, bool Minimum)
        {
            RETURN_T Return;
            byte Module;
            byte RelayChannel;

            Return = RETURN_T.OKAY;

            if (Channel == 255)
            {
                for (Channel = 0; Channel < ((Constants.MODULES * Constants.CHANNELS)) && (Return == RETURN_T.OKAY); Channel++)
                {
                    Module = (byte)(Channel / Constants.CHANNELS);
                    RelayChannel = (byte)(Channel % Constants.CHANNELS);

                    switch (Module)
                    {
                        case 0:
                            Return = (RETURN_T)relayCard.SetAxisLimit(RelayChannel, Axis, Limit, Minimum);
                            break;

                        case 1:
                            Return = (RETURN_T)relayCardPowerSupply.SetAxisLimit(RelayChannel, Axis, Limit, Minimum);
                            break;

                        default:
                            Return = RETURN_T.INVALIDE_CHANNEL;
                            break;
                    }
                }
            }
            else if (Channel < (Constants.MODULES * Constants.CHANNELS))
            {
                Module = (byte)(Channel / Constants.CHANNELS);
                RelayChannel = (byte)(Channel % Constants.CHANNELS);

                switch (Module)
                {
                    case 0:
                        Return = (RETURN_T)relayCard.SetAxisLimit(RelayChannel, Axis, Limit, Minimum);
                        break;

                    case 1:
                        Return = (RETURN_T)relayCardPowerSupply.SetAxisLimit(RelayChannel, Axis, Limit, Minimum);
                        break;

                    default:
                        Return = RETURN_T.INVALIDE_CHANNEL;
                        break;
                }
            }
            else
            {
                Return = RETURN_T.INVALIDE_CHANNEL;
            }

            return Return;
        }
        private void SetDefault()
        {
            Properties.Settings.Default.NightMode = false;
        }
        public void SetupSerial()
        {
            serialCommunication.Setup();
        }
        private RETURN_T FctTemplate()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            return Return;
        }
        #endregion
        #region Events
        private void FrameReceived(object sender, SerialCommunication.FrameReceivedEventArgs e)
        {
            string ReceivedFrame;
            byte Channel;
            byte Module;

            Channel = 255;
            ReceivedFrame = e.Parameter.ReceivedData;

            switch (e.Parameter.ActionExecuting)
            {
                case SerialCommunication.ACTION_T.GET_CONFIG:
                    Module = (byte)(Convert.ToInt32(e.Parameter.ParameterExecuting[0]) / 8);
                    Channel = (byte)(Convert.ToInt32(e.Parameter.ParameterExecuting[0]) % 8);

                    switch (Module)
                    {
                        case 0:
                            relayCard.EvaluateConfigurationString(Channel, e.Parameter.ReceivedData);
                            break;
                        case 1:
                            relayCardPowerSupply.EvaluateConfigurationString(Channel, e.Parameter.ReceivedData);

                            if (Channel == 7)
                            {
                                Initialized = true;

                                Init_Part_2();
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                    case SerialCommunication.ACTION_T.GET_OUTPUT_STATES:
                        relayCardMonitoring1.Monitoring(Convert.ToUInt32(e.Parameter.ReceivedData));
                    break;
                default:
                    break;
            }
        }
        private void menuMainSettingsSerialInterface_Click(object sender, EventArgs e)
        {
            SetupSerial();
        }
        private void menuMainSettingsViewNightMode_Click(object sender, EventArgs e)
        {
            menuMainSettingsViewNightMode.Checked = !menuMainSettingsViewNightMode.Checked;

            NightMode(menuMainSettingsViewNightMode.Checked);

        }
        private void SetParameter(object sender, RelayCard.SetParameterRelayCardEventArgs e)
        {
            Relay.Relay relay = new Relay.Relay();

            switch (e.Parameter)
            {
                case Relay.WHICH_PARAMETER_T.ASYNCHRONOUS_MODE:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_ASYNCHRONOUS_MODE, e.Channel, e.Params.AsynchronousMode ? 1 : 0, 0, 0, 0, false);
                    break;
                case Relay.WHICH_PARAMETER_T.DELAY_TIME_MS:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_DELAY_TIME_MS, e.Channel, e.Params.TimeOn_ms, e.Params.TimeOff_ms, e.Params.DelayTime_ms, 0, false);
                    break;
                case Relay.WHICH_PARAMETER_T.ENABLE:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_ENABLE, e.Channel, e.Params.Enabled ? 1 : 0, 0, 0, 0, false);
                    break;
                case Relay.WHICH_PARAMETER_T.IMMEDIATE_MODE:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_IMMEDIATE_MODE, e.Channel, e.Params.ImmediateMode ? 1 : 0, 0, 0, 0, false);
                    break;
                case Relay.WHICH_PARAMETER_T.IMPULSE_COUNTER:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_COUNTER, e.Channel, e.Params.ImpulseCounter, 0, 0, 0, false);
                    break;
                case Relay.WHICH_PARAMETER_T.INVERTING_MODE:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_INVERTING_MODE, e.Channel, e.Params.InvertedMode ? 1 : 0, 0, 0, 0, false);
                    break;
                case Relay.WHICH_PARAMETER_T.MODE:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_MODE, e.Channel, relay.ModeToString(e.Mode), 0, 0, 0, false);
                    break;
                case Relay.WHICH_PARAMETER_T.TIME_OFF_MS:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_DELAY_TIME_MS, e.Channel, e.Params.TimeOn_ms, e.Params.TimeOff_ms, e.Params.DelayTime_ms, 0, false);
                    break;
                case Relay.WHICH_PARAMETER_T.TIME_ON_MS:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_DELAY_TIME_MS, e.Channel, e.Params.TimeOn_ms, e.Params.TimeOff_ms, e.Params.DelayTime_ms, 0, false);
                    break;
                case Relay.WHICH_PARAMETER_T.TRIGGER:
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_TRIGGER, e.Channel, e.Params.TriggerChannel, e.Params.Triggering ? 1 : 0, 0, 0, false);
                    break;

                default:
                    break;
            }
        }
        #endregion
        private void RCmanager_Load(object sender, EventArgs e)
        {
            //Init();
        }
    }
    static class Constants
    {
        public const uint INITCODE = 0x55AA55AA;
        public const byte MODULES = 2;          // Number of relaycards  
        public const byte CHANNELS = 8;         // Number of relays on one relaycard
        public const byte IRQ_IOS = 4;          // Number of interrupt IOs
    }
}
