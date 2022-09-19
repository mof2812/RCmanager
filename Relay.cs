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
    public struct PARAMS_T
    {
        public byte Channel;
        //public Color ChartBColor;
        //public Color ChartLColor;
        public string SignalLabel;
        public double MinChartXRange_ms;
        public double MaxChartXRange_ms;
        public double MinChartYRange_ms;
        public double MaxChartYRange_ms;
        public UInt32 DelayTime_ms;
        public UInt32 TimeOn_ms;
        public UInt32 TimeOff_ms;
        //public UInt32 OutputStates;
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
        //public float TriggerLevel;
        public bool NightMode;
        //public bool ChannelInitialized;
        /* Communication */
        //public STATE_T State;
        //public ACTION_T ActionExecuting;
        //public string[] ParameterExecuting;
        //public string ReceivedData;
        //public SerialPort ComPort;
        /* End - Communication */
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
    }
    public partial class Relay : UserControl
    {
        public event EventHandler<SetParameterRelayEventArgs> SetParameterRelay;

        #region Members
        PARAMS_T Params;
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
                    MySetup.settings.SignalLabel = Params.SignalLabel = value;
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
                return MySetup.settings.ChartLColor;
            }
            set
            {
                if (MySetup.settings.ChartLColor != value)
                {
                    MySetup.settings.ChartLColor = value;
                    MySetup.Save();
                }
                chartRelay.Series[0].Color = value;
            }
        }
        public double MaxChartXRange_ms
        {
            get
            {
                return (Params.MaxChartXRange_ms);
            }
            //set
            //{

            //}
        }
        public double MinChartXRange_ms
        {
            get
            {
                return (Params.MinChartXRange_ms);
            }
            //set
            //{

            //}
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
        private void Draw()
        {
            int Temp;
            uint Time;

            // Set line style
            if (Params.Enabled)
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

            switch (Params.Mode)
            {
                case MODE_T.ON:
                    #region MODE_T.ON
                    AddData(0, 0f, Params.InvertedMode == false ? 1f : 0f);
                    AddData(0, Params.MaxChartXRange_ms, Params.InvertedMode == false ? 1f : 0f);
                    break;
                #endregion
                case MODE_T.OFF:
                    #region MODE_T.OFF
                    AddData(0, 0f, Params.InvertedMode == false ? 0f : 1f);
                    AddData(0, Params.MaxChartXRange_ms, Params.InvertedMode == false ? 0f : 1f);
                    break;
                #endregion
                case MODE_T.TOGGLE:
                    #region MODE_T.TOGGLE
                    AddData(0, 0f, Params.InvertedMode == false ? 0f : 1f);
                    Time = Params.DelayTime_ms;
                    if (Time > Params.MaxChartXRange_ms)
                    {
                        Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                    }
                    AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);

                    if ((Params.TimeOn_ms + Params.TimeOff_ms) > 0)
                    {
                        while (Time < Params.MaxChartXRange_ms)
                        {
                            AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);

                            Time += Params.TimeOn_ms;
                            if (Time >= Params.MaxChartXRange_ms)
                            {
                                if (Time == Params.MaxChartXRange_ms)
                                {
                                    AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                    AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                }
                                else
                                {
                                    Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                                    AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                }
                            }
                            else
                            {
                                AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);

                                Time += Params.TimeOff_ms;
                                if (Time >= Params.MaxChartXRange_ms)
                                {
                                    if (Time == Params.MaxChartXRange_ms)
                                    {
                                        AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                    }
                                    Time = Convert.ToUInt32(Params.MaxChartXRange_ms);

                                }
                                else
                                {
                                    AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                    //AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                }
                            }
                        }
                    }
                    break;
                #endregion

                case MODE_T.IMPULSE:
                    #region MODE_T.IMPULSE
                    AddData(0, 0f, Params.InvertedMode == false ? 0f : 1f);
                    Time = Params.DelayTime_ms;
                    if (Time > Convert.ToUInt32(Params.MaxChartXRange_ms))
                    {
                        Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                    }
                    AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);

                    if ((Params.TimeOn_ms + Params.TimeOff_ms) > 0)
                    {
                        if (Time < Convert.ToUInt32(Params.MaxChartXRange_ms))
                        {
                            AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);

                            Time += Params.TimeOn_ms;
                            if (Time >= Convert.ToUInt32(Params.MaxChartXRange_ms))
                            {
                                if (Time == Convert.ToUInt32(Params.MaxChartXRange_ms))
                                {
                                    AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                    AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                }
                                else
                                {
                                    Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                                    AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                    //AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                }
                            }
                            else
                            {
                                AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);

                                Time = Convert.ToUInt32(Params.MaxChartXRange_ms);
                                AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                            }
                        }
                    }
                    break;
                #endregion

                default:
                    break;
            }
        }
        public RETURN_T EvaluateConfigurationString(string ConfigurationString)
        {
            RETURN_T Return;
            int Pos;

            Return = RETURN_T.OKAY;
            Pos = 0;

            for (int Index = 0; Index < 12; Index++)
            {
                Pos = ConfigurationString.IndexOf(" ");
                switch (Index)
                {
                    case 0:
                        Params.Enabled = ConfigurationString.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 1:
                        Params.Mode = (MODE_T)Convert.ToInt32(ConfigurationString.Substring(0, Pos));
                        break;

                    case 2:
                        Params.TimeOn_ms = Convert.ToUInt32(ConfigurationString.Substring(0, Pos));
                        break;

                    case 3:
                        Params.TimeOff_ms = Convert.ToUInt32(ConfigurationString.Substring(0, Pos));
                        break;

                    case 4:
                        Params.DelayTime_ms = Convert.ToUInt32(ConfigurationString.Substring(0, Pos));
                        break;

                    case 5:
                        Params.ImpulseCounter = Convert.ToInt32(ConfigurationString.Substring(0, Pos));
                        break;

                    case 6:
                        Params.ImpulseCounterActual = Convert.ToInt32(ConfigurationString.Substring(0, Pos));
                        break;

                    case 7:
                        Params.AsynchronousMode = ConfigurationString.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 8:
                        Params.ImmediateMode = ConfigurationString.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 9:
                        Params.InvertedMode = ConfigurationString.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 10:
                        Params.TriggerChannel = Convert.ToUInt16(ConfigurationString.Substring(0, Pos));
                        break;

                    case 11:
                        Params.Triggering = ConfigurationString == "0" ? false : true;
                        break;

                    default:
                        Return = RETURN_T.ERROR;
                        break;
                }

                ConfigurationString = ConfigurationString.Remove(0, Pos + 1);
            }

            if (Return == RETURN_T.OKAY)
            {
                Params.MaxChartXRange_ms = Params.DelayTime_ms + Params.TimeOn_ms + Params.TimeOff_ms;
                Params.MinChartXRange_ms = Params.DelayTime_ms;

                SetSettings();
                Draw();
            }

            return Return;
        }
        public PARAMS_T GetParams()
        {
            return Params;
        }
        public string GetSignalName()
        {
            return Params.SignalLabel = chartRelay.Series[0].Name;
        }
        public RETURN_T Init(byte Channel, bool NightMode)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            Params.Channel = (byte)(Channel + 1);   // Channel starts by 1
            Params.NightMode = NightMode;

            Init_Settings();

            Params.SignalLabel = MySetup.settings.SignalLabel;

            Return = Init_Chart();

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
            chartRelay.Series[0].Color = Params.NightMode ? MySetup.settings.ChartLColor_NM : MySetup.settings.ChartLColor;
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
            chartRelay.ChartAreas[0].AxisX.TitleForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisX2.TitleForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisY.TitleForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisY2.TitleForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisX.LabelStyle.ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
            chartRelay.ChartAreas[0].AxisY.LabelStyle.ForeColor = Params.NightMode ? MySetup.settings.ChartTColor_NM : MySetup.settings.ChartTColor;
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
            chartRelay.Series[0].Name = Params.SignalLabel = MySetup.settings.SignalLabel;
            ledEnable.On = Params.Enabled;
            ledInvertingMode.On = Params.InvertedMode;
            ledTriggerMode.On = Params.Triggering;

            if (ledTriggerMode.On)
            {
                lblTriggerMode.Text = $"Triggerung({Params.TriggerChannel + 1})";
            }
            else
            {
                lblTriggerMode.Text = $"Triggerung";
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
            DialogResult Result;

            RelaySettingsDlg AddSettingsDlg = new RelaySettingsDlg();

            AddSettingsDlg.SetParameter += SetParameter;

            Params.SignalLabel = MySetup.settings.SignalLabel;

            AddSettingsDlg.Parameter = Params;

            Result = AddSettingsDlg.ShowDialog();

            if (Result == DialogResult.OK)
            {
                Params = AddSettingsDlg.Parameter;

                if (MySetup.settings.SignalLabel != Params.SignalLabel)
                {
                    MySetup.settings.SignalLabel = Params.SignalLabel;
                    MySetup.Save();
                }

                SetSettings();
                Draw();
            }
        }
        private void SetParameter(object sender, SetParameterEventArgs e)
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

            ledEnable.On = Params.Enabled = !Params.Enabled;

            SetSettings();
            Draw();

            /* Send Message to parent */
            Args.Channel = (byte)(Params.Channel - 1);
            Args.Parameter = WHICH_PARAMETER_T.ENABLE;
            Args.Mode = ledEnable.On ? MODE_T.ON : MODE_T.OFF;
            Args.Params = Params;

            OnSetParmeterRelay(Args);
        }

        private void ledInvertingMode_Click(object sender, EventArgs e)
        {
            SetParameterRelayEventArgs Args = new SetParameterRelayEventArgs();

            ledInvertingMode.On = Params.InvertedMode = !Params.InvertedMode;

            SetSettings();
            Draw();

            /* Send Message to parent */
            Args.Channel = (byte)(Params.Channel - 1);
            Args.Parameter = WHICH_PARAMETER_T.INVERTING_MODE;
            Args.Mode = ledInvertingMode.On ? MODE_T.ON : MODE_T.OFF;
            Args.Params = Params;

            OnSetParmeterRelay(Args);
        }
    }
    public class SetParameterRelayEventArgs : EventArgs
    {
        public byte Channel { get; set; }
        public WHICH_PARAMETER_T Parameter { get; set; }
        public MODE_T Mode { get; set; }
        public PARAMS_T Params { get; set; }
    }
    static class Constants
    {
        public const uint INITCODE = 0x55AA55AA;
        public const string DATAPATH = "..\\..\\Data\\";
    }

}
