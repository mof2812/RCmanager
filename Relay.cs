using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Relay
{
    [Serializable]
    public struct SETTINGS_T
    {
        public string SignalLabel;
        public UInt32 DelayTime_ms;
        public UInt32 TimeOn_ms;
        public UInt32 TimeOff_ms;
        public Int32 ImpulseCounter;
        public Int32 ImpulseCounterActual;
        public MODE_T Mode;
        public bool Enabled;
        public bool InvertedMode;
        public bool AsynchronousMode;
        public bool ImmediateMode;
        public TRIGGERMODE_T TriggerMode;
        public bool Triggering;
        public UInt16 TriggerChannel;
        public bool SignalNameEditable;
        /* Add data */
        public Color ChartLColor;
        //public Color ChartBColor;
        /* End - Add data */
    }
    public struct PARAMS_T
    {
        public byte Channel;
        public double MinChartXRange_ms;
        public double MaxChartXRange_ms;
        public double MinChartYRange_ms;
        public double MaxChartYRange_ms;
        public bool NightMode;
    }
        public enum AXIS_SELECT_T
    {
        X = 0,
        Y = 1,
    }
    public enum MODE_T
    {
        // Wichtig! Reihenfolge nicht ändern, da über den Index die Texte der Combobox ausgewählt werden. Neue Einträge hinten anstellen und Combobox anpassen
        OFF = 0,
        ON,
        TOGGLE,
        IMPULSE,
    }
    public enum TRIGGERMODE_T
    {
        LOW_LEVEL,
        HIGH_LEVEL,
        FALLING_EDGE,
        RISING_EDGE,
        LEVEL_DOWN,
        LEVEL_UP,
        IRQ_DOWN,
        IRQ_UP,
    }
    public enum RETURN_T
    {
        OKAY = 0,
        ERROR,
        INITIALIZE_ERROR,
        PARAM_ERROR,
        PARAM_MISSING,
        PARAM_OUT_OF_RANGE,
        INVALIDE_CHANNEL,
        INVALIDE_MODULE,
    }
    public partial class Relay : UserControl
    {
        public event EventHandler<SetParameterRelayEventArgs> SetParameterRelay;
        public event EventHandler<OpenRelaySettingsDlgEventArgs> OpenRelaySettingsDlg;
        public event EventHandler<OpenTriggerSettingsDlgEventArgs> OpenTriggerSettingsDlg;

        public bool SignalNameEditable
        {
            get
            { 
                return Settings.SignalNameEditable; 
            }
            set 
            {
                Settings.SignalNameEditable = value; 
            }

        }

        #region Members
        private PARAMS_T Params;
        private SETTINGS_T Settings;
        private Color[] LineColorDefault = new Color[] { Color.Red, Color.Yellow, Color.Blue, Color.Green, Color.Cyan, Color.Magenta, Color.Gray, Color.White, Color.Red, Color.Yellow, Color.Blue, Color.Green, Color.Cyan, Color.Magenta, Color.Gray, Color.White, Color.Red, Color.Yellow, Color.Blue, Color.Green, Color.Cyan, Color.Magenta, Color.Gray, Color.White };
        #endregion
        #region Properties
        //[
        //Category("Colors"),
        //Description("Background color of the chart area"),
        //DefaultValue("Color.White")
        //]
        public Color ChartBackColor
        {
            get
            {
                return MySetup.settings.ChartBColor;
            }
            set
            {
                if (MySetup.settings.ChartBColor != value)
                {
                    MySetup.settings.ChartBColor = value;
                    MySetup.Save();
                }
                chartRelay.ChartAreas[0].BackColor = value;
                this.BackColor = value;
            }
        }
        //[
        //Category("Texts"),
        //Description("Name of the dataset"),
        //DefaultValue("Test")
        //]
        public string ChartDataName
        {
            get
            {
                return MySetup.settings.SignalLabel;
            }
            set
            {
                if (MySetup.settings.SignalLabel != value)
                {
                    MySetup.settings.SignalLabel = Settings.SignalLabel = value;
                    chartRelay.Series[0].Label = MySetup.settings.SignalLabel;

                    MySetup.Save();
                }
            }
        }
        //[
        //Category("Colors"),
        //Description("Line color of the chart"),
        //DefaultValue("Color.Red")
        //]
        public Color ChartLineColor
        {
            get
            {
                return !Params.NightMode ? MySetup.settings.ChartLColor : MySetup.settings.ChartLColor_NM;
            }
            set
            {
                if (!Params.NightMode)
                {
                    if (MySetup.settings.ChartLColor != value)
                    {
                        MySetup.settings.ChartLColor = value;
                        MySetup.Save();
                        Init_Colors();
                    }
                }
                else
                {
                    if (MySetup.settings.ChartLColor_NM != value)
                    {
                        MySetup.settings.ChartLColor_NM = value;
                        MySetup.Save();
                        Init_Colors();
                    }
                }
                Settings.ChartLColor = chartRelay.Series[0].Color = value;
            }
        }
        public Color GetChartLineColor()
        {
            return !Params.NightMode ? MySetup.settings.ChartLColor : MySetup.settings.ChartLColor_NM;
        }
        public void SetChartLineColor(Color value)
        {
            if (!Params.NightMode)
            {
                if (MySetup.settings.ChartLColor != value)
                {
                    MySetup.settings.ChartLColor = value;
                    MySetup.Save();
                    Init_Colors();
                }
            }
            else
            {
                if (MySetup.settings.ChartLColor_NM != value)
                {
                    MySetup.settings.ChartLColor_NM = value;
                    MySetup.Save();
                    Init_Colors();
                }
            }
            Settings.ChartLColor = chartRelay.Series[0].Color = value;
        }
        public double MaxChartXRange_ms
        {
            get
            {
                return Params.MaxChartXRange_ms;
            }
            set
            {
                Params.MaxChartXRange_ms = value;
            }
        }

        public double MinChartXRange_ms
        {
            get
            {
                return Params.MinChartXRange_ms;
            }
            set
            {
                Params.MinChartXRange_ms = value;
            }
        }
        public bool NightMode
        {
            get
            {
                return Params.NightMode;
            }
            set
            {
                Params.NightMode = value;

                Init_Colors();
            }
        }
        #endregion
        #region Constructors, destructors
        public Relay()
        {
            InitializeComponent();

            SignalNameEditable = true;
        }
        #endregion
        #region Methods
        public void AddData(int Series, double X, double Y)
        {
            if (chartRelay.Series.Count > Series)
            {
                chartRelay.Series[Series].Points.AddXY(X, Y);
            }
        }
        private RETURN_T ChannelToModule(byte ChannelToEvaluate, ref SetParameterRelayEventArgs Args)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            Args.Module = (byte)((ChannelToEvaluate - 1) / RCmanager.Constants.CHANNELS);

            if (Args.Module < RCmanager.Constants.MODULES)
            {
                Args.Channel = (byte)((ChannelToEvaluate - 1) % RCmanager.Constants.CHANNELS);

                if (Args.Channel >= RCmanager.Constants.CHANNELS)
                {
                    Return = RETURN_T.INVALIDE_CHANNEL;
                }
            }
            else
            {
                Return = RETURN_T.INVALIDE_MODULE;
            }

            return Return;
        }
        private RETURN_T ChannelToModule(byte ChannelToEvaluate, ref byte Module, ref byte Channel)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            Module = (byte)((ChannelToEvaluate - 1) / RCmanager.Constants.CHANNELS);

            if (Module < RCmanager.Constants.MODULES)
            {
                Channel = (byte)((ChannelToEvaluate - 1) % RCmanager.Constants.CHANNELS);

                if (Channel >= RCmanager.Constants.CHANNELS)
                {
                    Return = RETURN_T.INVALIDE_CHANNEL;
                }
            }
            else
            {
                Return = RETURN_T.INVALIDE_MODULE;
            }

            return Return;
        }
        public void Draw()
        {
            int Temp;
            uint Time;

            // Set line style
            if (Settings.Enabled)
            {
                chartRelay.Series[0].BorderDashStyle = ChartDashStyle.Solid;
            }
            else
            {
                chartRelay.Series[0].BorderDashStyle = ChartDashStyle.Dash;
            }

            chartRelay.Series[0].Points.Clear();

            chartRelay.ChartAreas[0].AxisX.Maximum = Params.MaxChartXRange_ms * 1.05;

            Temp = (int)(Math.Log10(Params.MaxChartXRange_ms));

            chartRelay.ChartAreas[0].AxisX.Interval = Math.Pow(10, Temp) / 2;

            while ((Params.MaxChartXRange_ms / chartRelay.ChartAreas[0].AxisX.Interval) < 5)
            {
                chartRelay.ChartAreas[0].AxisX.Interval /= 2;
            }
            while ((Params.MaxChartXRange_ms / chartRelay.ChartAreas[0].AxisX.Interval) > 10)
            {
                chartRelay.ChartAreas[0].AxisX.Interval *= 2;
            }

            switch (Settings.Mode)
            {
                case MODE_T.ON:
                    #region MODE_T.ON
                    AddData(0, 0f, Settings.InvertedMode == false ? 1f : 0f);
                    AddData(0, Params.MaxChartXRange_ms, Settings.InvertedMode == false ? 1f : 0f);
                    break;
                #endregion
                case MODE_T.OFF:
                    #region MODE_T.OFF
                    AddData(0, 0f, Settings.InvertedMode == false ? 0f : 1f);
                    AddData(0, Params.MaxChartXRange_ms, Settings.InvertedMode == false ? 0f : 1f);
                    break;
                #endregion
                case MODE_T.TOGGLE:
                    #region MODE_T.TOGGLE
                    AddData(0, 0f, Settings.InvertedMode == false ? 0f : 1f);
                    Time = Settings.DelayTime_ms;
                    if (Time > Params.MaxChartXRange_ms)
                    {
                        Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                    }
                    AddData(0, Time, Settings.InvertedMode == false ? 0f : 1f);

                    if ((Settings.TimeOn_ms + Settings.TimeOff_ms) > 0)
                    {
                        while (Time < Params.MaxChartXRange_ms)
                        {
                            AddData(0, Time, Settings.InvertedMode == false ? 1f : 0f);

                            Time += Settings.TimeOn_ms;
                            if (Time >= Params.MaxChartXRange_ms)
                            {
                                if (Time == Params.MaxChartXRange_ms)
                                {
                                    AddData(0, Time, Settings.InvertedMode == false ? 1f : 0f);
                                    AddData(0, Time, Settings.InvertedMode == false ? 0f : 1f);
                                }
                                else
                                {
                                    Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                                    AddData(0, Time, Settings.InvertedMode == false ? 1f : 0f);
                                }
                            }
                            else
                            {
                                AddData(0, Time, Settings.InvertedMode == false ? 1f : 0f);
                                AddData(0, Time, Settings.InvertedMode == false ? 0f : 1f);

                                Time += Settings.TimeOff_ms;
                                if (Time >= Params.MaxChartXRange_ms)
                                {
                                    if (Time == Params.MaxChartXRange_ms)
                                    {
                                        AddData(0, Time, Settings.InvertedMode == false ? 0f : 1f);
                                    }
                                    Time = Convert.ToUInt32(Params.MaxChartXRange_ms);

                                }
                                else
                                {
                                    AddData(0, Time, Settings.InvertedMode == false ? 0f : 1f);
                                    //AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                }
                            }
                        }
                    }
                    break;
                #endregion

                case MODE_T.IMPULSE:
                    #region MODE_T.IMPULSE
                    AddData(0, 0f, Settings.InvertedMode == false ? 0f : 1f);
                    Time = Settings.DelayTime_ms;
                    if (Time > Convert.ToUInt32(Params.MaxChartXRange_ms))
                    {
                        Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                    }
                    AddData(0, Time, Settings.InvertedMode == false ? 0f : 1f);

                    if ((Settings.TimeOn_ms + Settings.TimeOff_ms) > 0)
                    {
                        if (Time < Convert.ToUInt32(Params.MaxChartXRange_ms))
                        {
                            AddData(0, Time, Settings.InvertedMode == false ? 1f : 0f);

                            Time += Settings.TimeOn_ms;
                            if (Time >= Convert.ToUInt32(Params.MaxChartXRange_ms))
                            {
                                if (Time == Convert.ToUInt32(Params.MaxChartXRange_ms))
                                {
                                    AddData(0, Time, Settings.InvertedMode == false ? 1f : 0f);
                                    AddData(0, Time, Settings.InvertedMode == false ? 0f : 1f);
                                }
                                else
                                {
                                    Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                                    AddData(0, Time, Settings.InvertedMode == false ? 1f : 0f);
                                    //AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                }
                            }
                            else
                            {
                                AddData(0, Time, Settings.InvertedMode == false ? 1f : 0f);
                                AddData(0, Time, Settings.InvertedMode == false ? 0f : 1f);

                                Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                                AddData(0, Time, Settings.InvertedMode == false ? 0f : 1f);
                            }
                        }
                    }
                    break;
                #endregion

                default:
                    break;
            }
        }
        public PARAMS_T GetParams()
        {
            return Params;
        }
        public string GetSignalName()
        {
            //return Settings.SignalLabel = chartRelay.Series[0].Name;
            return chartRelay.Series[0].Name = Settings.SignalLabel;
        }
        public RETURN_T Init(byte Channel, bool NightMode)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            Params.Channel = (byte)(Channel + 1);   // Channel starts by 1
            Params.NightMode = NightMode;

            Init_Settings();

            Settings.SignalLabel = MySetup.settings.SignalLabel;
            Settings.ChartLColor = !Params.NightMode ? MySetup.settings.ChartLColor : MySetup.settings.ChartLColor_NM;

            Return = Init_Chart();

            Enable_Controls();

            return Return;
        }
        private RETURN_T Init_Chart()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            chartRelay.Series.Clear();
            chartRelay.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series(MySetup.settings.SignalLabel));

            chartRelay.Series[0].BorderWidth = 1;
            chartRelay.Series[0].ChartType = SeriesChartType.Line;

            chartRelay.ChartAreas[0].AxisX.Minimum = 0;
            chartRelay.ChartAreas[0].AxisY.Minimum = 0;
            chartRelay.ChartAreas[0].AxisY.Maximum = 1.1;
            chartRelay.ChartAreas[0].AxisY.Interval = 1;
            chartRelay.ChartAreas[0].AxisX.Title = "Zeit";
            chartRelay.ChartAreas[0].AxisY.Title = "Status";

            chartRelay.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartRelay.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
            chartRelay.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartRelay.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartRelay.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
            chartRelay.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            Init_Colors();

            return Return;
        }
        private void Init_Colors()
        {
            chartRelay.BackColor = Params.NightMode ? MySetup.settings.ChartBColor_NM : MySetup.settings.ChartBColor;
            chartRelay.ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].BackColor = Params.NightMode ? MySetup.settings.ChartBColor_NM : MySetup.settings.ChartBColor;
            chartRelay.Series[0].Color = Settings.ChartLColor = Params.NightMode ? MySetup.settings.ChartLColor_NM : MySetup.settings.ChartLColor;
            chartRelay.Series[0].LabelForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.Legends[0].BackColor = Params.NightMode ? MySetup.settings.ChartBColor_NM : MySetup.settings.ChartBColor;
            chartRelay.Legends[0].BorderColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.Legends[0].ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;

            lblEnable.BackColor = ledEnable.BackColor = Params.NightMode ? MySetup.settings.ChartBColor_NM : MySetup.settings.ChartBColor;
            lblEnable.ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            lblInvertingMode.BackColor = ledInvertingMode.BackColor = Params.NightMode ? MySetup.settings.ChartBColor_NM : MySetup.settings.ChartBColor;
            lblInvertingMode.ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            lblTriggerMode.BackColor = ledTriggerMode.BackColor = Params.NightMode ? MySetup.settings.ChartBColor_NM : MySetup.settings.ChartBColor;
            lblTriggerMode.ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;

            chartRelay.ChartAreas[0].AxisX.LineColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisY.LineColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisY2.LineColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisX.TitleForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisX2.TitleForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisY.TitleForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisY2.TitleForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisX.LabelStyle.ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisY.LabelStyle.ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisY2.LabelStyle.ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
        }
        private RETURN_T Init_Settings()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (MySetup.Init($"{Constants.DATAPATH}Relay_{Params.Channel}.dat"))
            {
                // Set default values
                SetDefaults();
            }
/* For testing only
            MySetup.settings.ChartLColor = Color.Green;
            MySetup.settings.ChartLColor_NM = Color.Blue;
            MySetup.Save();
*/
            return Return;
        }
        public string ModeToString(MODE_T Mode)
        {
            string ModeAsString;

            ModeAsString = "OFF";

            switch (Mode)
            {
                case MODE_T.ON:
                    ModeAsString = "ON";
                    break;

                case MODE_T.TOGGLE:
                    ModeAsString = "TOGGLE";
                    break;

                case MODE_T.IMPULSE:
                    ModeAsString = "IMPULSE";
                    break;

                case MODE_T.OFF:
                default:
                    ModeAsString = "OFF";
                    break;
            }

            return ModeAsString;
        }
        protected virtual void OnSetParmeterRelay(SetParameterRelayEventArgs e)
        {
            EventHandler<SetParameterRelayEventArgs> handler = SetParameterRelay;
            handler?.Invoke(this, e);
        }
        protected virtual void OnOpenRelaySettingsDlg(OpenRelaySettingsDlgEventArgs e)
        {
            EventHandler<OpenRelaySettingsDlgEventArgs> handler = OpenRelaySettingsDlg;
            handler?.Invoke(this, e);
        }
        protected virtual void OnOpenTriggerSettingsDlg(OpenTriggerSettingsDlgEventArgs e)
        {
            EventHandler<OpenTriggerSettingsDlgEventArgs> handler = OpenTriggerSettingsDlg;
            handler?.Invoke(this, e);
        }
        public PARAMS_T parameter
        {
            get
            {
                return Params;
            }
            set
            {
                Params = value;

                Draw();
            }
        }
        public RETURN_T SetAxisLimit(AXIS_SELECT_T Axis, double Limit, bool Minimum)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (Minimum)
            {
                if (Axis == AXIS_SELECT_T.X)
                {
                    Params.MinChartXRange_ms = Limit;
//                    chartRelay.ChartAreas[0].AxisX.Minimum = Limit;
                }
                else
                {
                    Params.MinChartYRange_ms = Limit;
//                    chartRelay.ChartAreas[0].AxisY.Minimum = Limit;
                }
            }
            else
            {
                if (Axis == AXIS_SELECT_T.X)
                {
                    Params.MaxChartXRange_ms = Limit;
//                    chartRelay.ChartAreas[0].AxisX.Maximum = Limit;
                }
                else
                {
                    Params.MaxChartYRange_ms = Limit;
//                  chartRelay.ChartAreas[0].AxisY.Maximum = Limit;
                }
            }

            Draw();

            return Return;
        }
        public void SetDefaults()
        {
            MySetup.settings.SignalLabel = $"Signal_{Params.Channel}";
            MySetup.settings.ChartBColor = Color.White;
            MySetup.settings.ChartLColor = LineColorDefault[Params.Channel - 1];
            MySetup.settings.ChartTColor = Color.Black;
            MySetup.settings.ChartBColor_NM = Color.Black;
            MySetup.settings.ChartLColor_NM = LineColorDefault[Params.Channel - 1];
            MySetup.settings.ChartTColor_NM = Color.White;

            MySetup.Save();
        }
        private void SetSettings()
        {
            chartRelay.Series[0].Name = Settings.SignalLabel = MySetup.settings.SignalLabel;
            ledEnable.On = Settings.Enabled;
            ledInvertingMode.On = Settings.InvertedMode;
            ledTriggerMode.On = Settings.Triggering;
            Enable_Controls();

            if (ledTriggerMode.On)
            {
                lblTriggerMode.Text = $"Triggerung({Settings.TriggerChannel + 1})";
            }
            else
            {
                lblTriggerMode.Text = $"Triggerung";
            }
        }
        public void SetSignalName(string SignalName)
        {
            MySetup.settings.SignalLabel = Settings.SignalLabel = SignalName;
            MySetup.Save();
        }
        public SETTINGS_T settings
        {
            get
            {
                return Settings;
            }
            set
            {
                Settings = value;

                SetSettings();
                Draw();
            }
        }
        public MODE_T StringToMode(string ModeAsString)
        {
            MODE_T Mode;

            Mode = MODE_T.OFF;

            switch (ModeAsString)
            {
                case "ON":
                    Mode = MODE_T.ON;
                    break;

                case "TOGGLE":
                    Mode = MODE_T.TOGGLE;
                    break;

                case "IMPULSE":
                    Mode = MODE_T.IMPULSE;
                    break;

                case "OFF":
                default:
                    Mode = MODE_T.OFF;
                    break;
            }

            return Mode;
        }
        private void Enable_Controls()
        {
            switch (Settings.Mode)
            {
                case MODE_T.TOGGLE:
                    ledTriggerMode.Visible = lblTriggerMode.Visible = true;
                    break;

                case MODE_T.OFF:
                case MODE_T.ON:
                    ledTriggerMode.Visible = lblTriggerMode.Visible = true;
                    break;

                default: /* MODE_T.IMPULSE */
                    ledTriggerMode.Enabled = lblTriggerMode.Visible = true;
                    break;
            }
        }
        private RETURN_T FctTemplate()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            return Return;
        }
        #endregion

        #region Events
        private void AddSettingsMenuItem_Click(object sender, EventArgs e)
        {
            OpenRelaySettingsDlgEventArgs Args = new OpenRelaySettingsDlgEventArgs();

            Args.Module = (byte)((Params.Channel - 1) / RCmanager.Constants.CHANNELS);
            Args.Channel = (byte)((Params.Channel - 1) % RCmanager.Constants.CHANNELS);

            OnOpenRelaySettingsDlg(Args);
            //DialogResult Result;

            //RelaySettingsDlg AddSettingsDlg = new RelaySettingsDlg();

            //AddSettingsDlg.SetParameter += SetParameter;

            //Settings.SignalLabel = MySetup.settings.SignalLabel;

            //AddSettingsDlg.Parameter = Params;

            //Result = AddSettingsDlg.ShowDialog();

            //if (Result == DialogResult.OK)
            //{
            //    Params = AddSettingsDlg.Parameter;

            //    if (MySetup.settings.SignalLabel != Settings.SignalLabel)
            //    {
            //        MySetup.settings.SignalLabel = Settings.SignalLabel;
            //        MySetup.Save();
            //    }

            //    SetSettings();
            //    Draw();
            //}
        }
        private void SetParameter(object sender, SetParameterRelayEventArgs e)
        {
            SetParameterRelayEventArgs Args = new SetParameterRelayEventArgs();

            Args.Channel = (byte)(e.Params.Channel - 1);
            Args.Parameter = e.Parameter;
            Args.Mode = e.Mode;
            Args.Params = e.Params;

            OnSetParmeterRelay(Args);

            Params = e.Params;
            Draw();
        }
        #endregion

        private void ledEnable_Click(object sender, EventArgs e)
        {
            SetParameterRelayEventArgs Args = new SetParameterRelayEventArgs();

            ledEnable.On = Settings.Enabled = !Settings.Enabled;

            /* Send Message to parent */
            ChannelToModule(Params.Channel, ref Args);
            Args.Parameter = RCmanager.WHICH_PARAMETER_T.ENABLE;
            Args.Mode = ledEnable.On ? MODE_T.ON : MODE_T.OFF;

            OnSetParmeterRelay(Args);

            SetSettings();
            Draw();
        }

        private void ledInvertingMode_Click(object sender, EventArgs e)
        {
            SetParameterRelayEventArgs Args = new SetParameterRelayEventArgs();

            ledInvertingMode.On = Settings.InvertedMode = !Settings.InvertedMode;

            /* Send Message to parent */
            ChannelToModule(Params.Channel, ref Args);
            Args.Parameter = RCmanager.WHICH_PARAMETER_T.INVERTING_MODE;
            Args.Mode = ledInvertingMode.On ? MODE_T.ON : MODE_T.OFF;

            OnSetParmeterRelay(Args);

            SetSettings();
            Draw();
        }
        private void ledTriggerMode_MouseClick(object sender, MouseEventArgs e)
        {
            SetParameterRelayEventArgs Args = new SetParameterRelayEventArgs();

            ledTriggerMode.On = Settings.Triggering = !Settings.Triggering;

            /* Send Message to parent */
            ChannelToModule(Params.Channel, ref Args);
            Args.Parameter = RCmanager.WHICH_PARAMETER_T.TRIGGER;
            Args.Mode = ledTriggerMode.On ? MODE_T.ON : MODE_T.OFF;
            Args.Settings = Settings;

            OnSetParmeterRelay(Args);

            SetSettings();
            Draw();

        }

        private void chartRelay_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenRelaySettingsDlgEventArgs Args = new OpenRelaySettingsDlgEventArgs();

            Args.Module = (byte)((Params.Channel - 1) / RCmanager.Constants.CHANNELS);
            Args.Channel = (byte)((Params.Channel - 1) % RCmanager.Constants.CHANNELS);

            OnOpenRelaySettingsDlg(Args);
        }

        private void ledTriggerMode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //DialogResult Result;
            OpenTriggerSettingsDlgEventArgs Args = new OpenTriggerSettingsDlgEventArgs();

            OnOpenTriggerSettingsDlg(Args);

            TriggerSettingsDlg.TriggerSettingsDlg AddTriggerSettingsDlg = new TriggerSettingsDlg.TriggerSettingsDlg();

            ledTriggerMode_MouseClick(sender, e);
        }

        private void TriggerSettingsMenuItem_Click(object sender, EventArgs e)
        {
            OpenTriggerSettingsDlgEventArgs Args = new OpenTriggerSettingsDlgEventArgs();

            OnOpenTriggerSettingsDlg(Args);
        }

    }
    public class SetParameterRelayEventArgs : EventArgs
    {
        public byte Module { get; set; }
        public byte Channel { get; set; }
        public RCmanager.WHICH_PARAMETER_T Parameter { get; set; }
        public MODE_T Mode { get; set; }
        public PARAMS_T Params { get; set; }
        public SETTINGS_T Settings { get; set; }
    }
    public class OpenRelaySettingsDlgEventArgs : EventArgs
    {
        public byte Channel { get; set; }
        public byte Module { get; set; }
    }
    public class OpenTriggerSettingsDlgEventArgs : EventArgs
    { 
    }
    static class Constants
    {
        public const uint INITCODE = 0x55AA55AA;
        public const string DATAPATH = "..\\..\\Data\\";
    }

}
