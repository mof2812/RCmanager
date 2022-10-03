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
            public Relay.PARAMS_T[] RelayParams;
            public Trigger.PARAMS_T[] TriggerParams;
            public TriggerSettings.PARAMS_T[] TriggerSettings;
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
        public RETURN_T EvaluateTriggerConfigurationString(byte TriggerChannel, string ConfigurationString)
        {
            RETURN_T Return;
            TriggerSettings.PARAMS_T TriggerParams;
            TriggerSettings.TriggerSettings triggerSettings = new TriggerSettings.TriggerSettings();
            int Pos;

            Return = RETURN_T.OKAY;
            Pos = 0;

            for (int Index = 0; Index < 4; Index++)
            {
                Pos = ConfigurationString.IndexOf(" ");
                switch (Index)
                {
                    case 0:
                        Params.TriggerSettings[TriggerChannel].Enabled = ConfigurationString.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 1:
                        Params.TriggerSettings[TriggerChannel].TriggerMode = (TriggerSettings.TRIGGER_MODE_T)Convert.ToInt32(ConfigurationString.Substring(0, Pos));
                        break;

                    case 2:
                        ConfigurationString = ConfigurationString.Replace('.', ',');
                        Params.TriggerSettings[TriggerChannel].TriggerLevel = (float)Convert.ToDouble(ConfigurationString.Substring(0, Pos));
                        break;

                    case 3:
                        Params.TriggerSettings[TriggerChannel].Retrigger = ConfigurationString == "0" ? false : true;
                        break;

                    default:
                        Return = RETURN_T.ERROR;
                        break;
                }

                ConfigurationString = ConfigurationString.Remove(0, Pos + 1);
            }



            return Return;
        }
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
        private RETURN_T GetTriggerConfig(byte Channel)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (Channel == 255)
            {
                for (Channel = 0; Channel < Constants.IRQ_IOS; Channel++)
                {
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.GET_TRIGGER_CONFIG, Channel, 0, 0, 0, 0, false);
                }
            }
            else if (Channel < Constants.IRQ_IOS)
            {
                serialCommunication.SetAction(SerialCommunication.ACTION_T.GET_TRIGGER_CONFIG, Channel, 0, 0, 0, 0, false);
            }
            else
            {
                Return = RETURN_T.INVALIDE_CHANNEL;
            }

            return Return;
        }
        //private RETURN_T GetTriggerName(byte Channel, ref string TriggerName)
        //{
        //    RETURN_T Return;

        //    Return = RETURN_T.OKAY;

        //    if (Channel < Constants.IRQ_IOS)
        //    {
        //        TriggerName = TriggerChannel[Channel].TriggerLabel;
        //    }
        //    else
        //    {
        //        Return = RETURN_T.INVALIDE_CHANNEL;
        //    }

        //    return Return;
        //}
        private RETURN_T GetTriggerParameter(byte Channel, ref Trigger.PARAMS_T TriggerParams)
        {
            RETURN_T Return;
            byte Module;
            byte RelayChannel;
            Trigger.Trigger tm;

            Return = RETURN_T.OKAY;
            Module = (byte)(Channel / Constants.CHANNELS);
            RelayChannel = (byte)(Channel % Constants.CHANNELS);

            if (Channel < Constants.IRQ_IOS)
            {
                TriggerParams = relayCardMonitoring.GetTriggerMonitoring(Channel).GetParams();
            }
            else
            {
                Return = RETURN_T.INVALIDE_CHANNEL;
            }

            return Return;
        }
        private RETURN_T Init()
        {
            RETURN_T Return;
            Version.Version Version = new Version.Version();

            Return = RETURN_T.INITIALIZE_ERROR;

            this.Text = $"{this.Text} - {Version.SW_Version}";

            Params.RelayParams = new Relay.PARAMS_T[Constants.MODULES * Constants.CHANNELS];
            Params.TriggerParams = new Trigger.PARAMS_T[Constants.IRQ_IOS];
            Params.TriggerSettings = new TriggerSettings.PARAMS_T[Constants.IRQ_IOS];

            Return = Init_Settings();

            if (Return == RETURN_T.OKAY)
            {
                Return = Init_RelayCards();
            }
            if (Return == RETURN_T.OKAY)
            {
                Return = Init_Triggers(Properties.Settings.Default.NightMode);
            }
            if (Return == RETURN_T.OKAY)
            {
                Return = Init_Menu();
            }
            if (Return == RETURN_T.OKAY)
            {
                Return = Init_Communication();
            }
            if (Return == RETURN_T.OKAY)
            {
                // Init the monitoring routines
                Init_Monitoring();
                // Get the configuration of all relays
                GetConfig(255);

                // Get the configuration of all triggers
                GetTriggerConfig(255);
            }

            if (Return != RETURN_T.OKAY)
            {
                Return = RETURN_T.INITIALIZE_ERROR;
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

            relayCardMonitoring.Init_Monitoring(menuMainSettingsViewNightMode.Checked);
            relayCardUserMonitoring.Init_Monitoring(menuMainSettingsViewNightMode.Checked);

            for (Index = 0; (Index < Constants.MODULES * Constants.CHANNELS) && (Return == RETURN_T.OKAY); Index++)
            {
                if (Return == RETURN_T.OKAY)
                {
                    Return = GetRelayParameter(Index, ref Params.RelayParams[Index]);

                    if (Return == RETURN_T.OKAY)
                    {
                        relayCardMonitoring.InitRelayMonitoring(Index, Params.RelayParams[Index]);
                    }
                }
            }

            for (Index = 0; (Index < Constants.IRQ_IOS) && (Return == RETURN_T.OKAY); Index++)
            {
                if (Return == RETURN_T.OKAY)
                {
                    Return = GetTriggerParameter(Index, ref Params.TriggerParams[Index]);

                    if (Return == RETURN_T.OKAY)
                    {
                        //relayCardUserMonitoring.Init_TriggerMonitoring(Index, Params.TriggerParams[Index]);
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
                Properties.Settings.Default.TriggerLabel = new string[8];
                SetDefault();

                Properties.Settings.Default.InitCode = Constants.INITCODE;
                Properties.Settings.Default.Save();
            }

            return Return;
        }
        private RETURN_T Init_Triggers(bool NightMode)
        {
            RETURN_T Return;
            byte Channel;
            RelayCard.RelayCard Card = new RelayCard.RelayCard();

            Return = RETURN_T.INITIALIZE_ERROR;
            Channel = 0;

            if ((RETURN_T)relayCardMonitoring.GetTriggerMonitoring(Channel).Init(Channel, NightMode) == RETURN_T.OKAY)
            {
                Channel++;
                if ((RETURN_T)relayCardMonitoring.GetTriggerMonitoring(Channel).Init(Channel, NightMode) == RETURN_T.OKAY)
                {
                    Channel++;
                    if ((RETURN_T)relayCardMonitoring.GetTriggerMonitoring(Channel).Init(Channel, NightMode) == RETURN_T.OKAY)
                    {
                        Channel++;
                        if ((RETURN_T)relayCardMonitoring.GetTriggerMonitoring(Channel).Init(Channel, NightMode) == RETURN_T.OKAY)
                        {
/*
                            Channel++;
                            if ((RETURN_T)relayCardMonitoring.GetTriggerMonitoring(Channel).Init(Channel, NightMode) == RETURN_T.OKAY)
                            {
                                Channel++;
                                if ((RETURN_T)relayCardMonitoring.GetTriggerMonitoring(Channel).Init(Channel, NightMode) == RETURN_T.OKAY)
                                {
                                    Channel++;
                                    if ((RETURN_T)relayCardMonitoring.GetTriggerMonitoring(Channel).Init(Channel, NightMode) == RETURN_T.OKAY)
                                    {
                                        Channel++;
                                        if ((RETURN_T)relayCardMonitoring.GetTriggerMonitoring(Channel).Init(Channel, NightMode) == RETURN_T.OKAY)
                                        {
*/
                                            Return = RETURN_T.OKAY;
/*
                                        }

                                    }

                                }

                            }
*/
                        }

                    }

                }

            }


            for (byte Module = 0; Module < Constants.MODULES; Module++)
            {
                Card = Module == 0 ? relayCard : relayCardPowerSupply;

                for (Channel = 0; Channel < Constants.CHANNELS; Channel++)
                {
                    Card.GetRelay(Channel).OpenTriggerSettingsDlg += OpenTriggerSettingsDlg;
                }

            }

            return Return;
        }
        private RETURN_T Init_TriggerCard()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (triggerCard.Init(Properties.Settings.Default.NightMode) != TriggerCard.RETURN_T.OKAY)
            {
                Return = RETURN_T.INITIALIZE_ERROR;
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
            for (int Index = 0; Index < 8; Index++)
            {
                Properties.Settings.Default.TriggerLabel[Index] = $"Trigger {Index + 1}";
            }
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
                            break;
                        default:
                            break;
                    }
                    break;
                    case SerialCommunication.ACTION_T.GET_OUTPUT_STATES:
                        relayCardMonitoring.Monitoring(Convert.ToUInt32(e.Parameter.ReceivedData));
                        relayCardUserMonitoring.Monitoring(Convert.ToUInt32(e.Parameter.ReceivedData));
                    break;

                    case SerialCommunication.ACTION_T.GET_TRIGGER_CONFIG:
                    Channel = (byte)(Convert.ToInt32(e.Parameter.ParameterExecuting[0]));
                    EvaluateTriggerConfigurationString(Channel, e.Parameter.ReceivedData);
                    if (Channel == 3)
                    {
                        Initialized = true;

                        Init_Part_2();
                    }
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
        private void OpenTriggerSettingsDlg(object sender, Relay.OpenTriggerSettingsDlgEventArgs e)
        {
            DialogResult Result;
            TriggerSettingsDlg.TriggerSettingsDlg AddTriggerSettingsDlg = new TriggerSettingsDlg.TriggerSettingsDlg();

            AddTriggerSettingsDlg.Settings = Params.TriggerSettings;

            Result = AddTriggerSettingsDlg.ShowDialog();

            if (Result == DialogResult.OK)
            {
                Params.TriggerSettings = AddTriggerSettingsDlg.Settings;

                for (byte TriggerChannel = 0; TriggerChannel < Constants.IRQ_IOS; TriggerChannel++)
                {
                    serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_TRIGGER_CONFIG, TriggerChannel, Params.TriggerSettings[TriggerChannel].Enabled ? 1 : 0, (int)Params.TriggerSettings[TriggerChannel].TriggerMode, (int)(Params.TriggerSettings[TriggerChannel].TriggerLevel * 1000), Params.TriggerSettings[TriggerChannel].Retrigger ? 1 : 0, false);
                }
            }
        }
        private void RCmanager_Load(object sender, EventArgs e)
        {
            //Init();
        }
        private void ledAllOff_Click(object sender, EventArgs e)
        {
            serialCommunication.SetAction(SerialCommunication.ACTION_T.ENABLE_ALL, 0, 0, 0, 0, 0, false);

            GetConfig(255);
        }
        private void ledAllCardChannelsOff_Click(object sender, EventArgs e)
        {
            if (lblAllCardChannelsOff.Text == "Alle Schaltkanäle abschalten")
            {
                serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_ENABLE_CARD, 0, 0, 0, 0, 0, false);
            }
            else
            {
                serialCommunication.SetAction(SerialCommunication.ACTION_T.SET_ENABLE_CARD, 0, 0, 1, 0, 0, false);
            }

            GetConfig(255);
        }
        private void tabSheetSwitchingCard_Enter(object sender, EventArgs e)
        {
            ledAllCardChannelsOff.Visible = lblAllCardChannelsOff.Visible = true;
            lblAllCardChannelsOff.Text = "Alle Schaltkanäle abschalten";
        }
        private void tabSheetPowerSupply_Enter(object sender, EventArgs e)
        {
            ledAllCardChannelsOff.Visible = lblAllCardChannelsOff.Visible = true;
            lblAllCardChannelsOff.Text = "Alle Netzteil-Schaltkanäle abschalten";
        }
        private void tabMonitoring1_Enter(object sender, EventArgs e)
        {
            ledAllCardChannelsOff.Visible = lblAllCardChannelsOff.Visible = false;
        }
        private void tabMonitoring2_Enter(object sender, EventArgs e)
        {
            ledAllCardChannelsOff.Visible = lblAllCardChannelsOff.Visible = false;

            for (byte Index = 0; Index < Constants.MODULES * Constants.CHANNELS; Index++)
            {
                Relay.PARAMS_T RelayParams = new Relay.PARAMS_T();
                GetRelayParameter(Index, ref RelayParams);
                relayCardUserMonitoring.SetRelayParams(Index, RelayParams);
            }
            
            for (byte Index = 0; Index < Constants.MODULES * Constants.CHANNELS; Index++)
            {
                Trigger.PARAMS_T TriggerParams = new Trigger.PARAMS_T();
                GetTriggerParameter(Index, ref TriggerParams);
                relayCardUserMonitoring.SetTriggerParams(Index, TriggerParams);
            }
        }
        #endregion

        private void MenuFileOpenProject(object sender, EventArgs e)
        {
            DialogResult Result;
            string Path;

            Path = Properties.Settings.Default.ProjectPath;

            Result = projectSettings.Open(ref Path);

            if (Result == DialogResult.OK)
            {
                Properties.Settings.Default.ProjectPath = Path;

                Properties.Settings.Default.Save();
            }

            projektSichernToolStripMenuItem.Enabled = (Result == DialogResult.OK);
        }

        private void MenuFileSaveProject(object sender, EventArgs e)
        {
            DialogResult Result;
            string Path;

            projectSettings.SetData(Params.RelayParams, Params.TriggerSettings);

            Path = Properties.Settings.Default.ProjectPath;

            Result = projectSettings.Save();
        }

        private void MenuFileSaveAsProject(object sender, EventArgs e)
        {
            DialogResult Result;
            string Path;

            projectSettings.SetData(Params.RelayParams, Params.TriggerSettings);

            Path = Properties.Settings.Default.ProjectPath;

            Result = projectSettings.SaveAs(ref Path);

            if (Result == DialogResult.OK)
            {
                Properties.Settings.Default.ProjectPath = Path;

                Properties.Settings.Default.Save();
            }
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            Version.Version Dlg = new Version.Version();

            Dlg.ShowDialog();
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
