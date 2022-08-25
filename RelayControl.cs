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
using System.IO.Ports;

namespace RelayControl
{
    public enum MODE_T
    {
        // Wichtig! Reihenfolge nicht ändern, da über den Index die Texte der Combobox ausgewählt werden. Neue Einträge hinten anstellen und Combobox anpassen
        OFF = 0,
        ON,
        TOGGLE,
        IMPULSE,
    }

    public struct PARAMS_T
    {
        public byte Channel;
        public Color ChartBColor;
        public Color ChartLColor;
        public string ChartDName;
        public UInt32 ChartXRange_ms;
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
        public bool TriggerMode;
        public UInt16 TriggerChannel;
        public bool NightMode;
        public bool ChannelInitialized;
/* Communication */
        public STATE_T State;
        public ACTION_T ActionExecuting;
        public string[] ParameterExecuting;
        public string ReceivedData;
        public SerialPort ComPort;
/* End - Communication */
    }
    public enum RECEIVE_RETURN_T
    {
        DATA_FRAME,
        ACK_FRAME,
        OK_FRAME,
        ERROR_FRAME,
        NAK_FRAME,
        CORRUPTED_FRAME,
        PORT_CLOSED,
        TIMEOUT,
        BUSY,
        WRONG_DATA,
        OK_INSTEAD_OF_DATA,
        DATA_INSTEAD_OF_OK,
        TEXT_INSTEAD_OF_NUMBER,
        OKAY,
        ERROR,
    }
    public partial class RelayControl : UserControl
    {
        public PARAMS_T Params;
        public RelayControl()
        {
            InitializeComponent();

            Init_Communication();

            /* Get configuration */
            SetAction(ACTION_T.GET_CONFIG, false);
        }
        public void AddChannel(string DataName, Color LineColor)
        {
            Params.ChartDName = DataName;
            Params.ChartLColor = LineColor;
            Params.ChannelInitialized = true;

            chartRelay.Series.Clear();

            chartRelay.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series(Params.ChartDName));
            chartRelay.Series[0].Color = Params.ChartLColor;
        }
        public void AddData(int Series, double X, double Y)
        {
            if (chartRelay.Series.Count > Series)
            {
                chartRelay.Series[Series].Points.AddXY(X, Y);
            }
        }
        public void AddSerialPort(byte Channel, ref SerialPort Port)
        {
            Params.ComPort = Port;
            Params.Channel = Channel;
        }
        public Color ChartBackColor
        {
            get
            {
                return Params.ChartBColor;
            }
            set
            {
                Params.ChartBColor = value;
                chartRelay.ChartAreas[0].BackColor = value;
            }
        }
        public string ChartDataName
        {
            get
            {
                return Params.ChartDName;
            }
            set
            {
                Params.ChartDName = value;
            }
        }
        public Color ChartLineColor
        {
            get
            {
                return Params.ChartLColor;
            }
            set
            {
                Params.ChartLColor = value;
                chartRelay.Series[0].Color = value;
            }
        }
        public UInt32 DelayTime
        {
            get
            {
                return Params.DelayTime_ms;
            }
            set 
            { 
                Params.DelayTime_ms = value;

                Draw();
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

            chartRelay.ChartAreas[0].AxisX.Maximum = Params.ChartXRange_ms * 1.05;

            Temp = (int)(Math.Log10(Params.ChartXRange_ms));

            chartRelay.ChartAreas[0].AxisX.Interval = Math.Pow(10, Temp) / 2;

            while ((Params.ChartXRange_ms / chartRelay.ChartAreas[0].AxisX.Interval) < 5)
            {
                chartRelay.ChartAreas[0].AxisX.Interval /= 2;
            }
            while ((Params.ChartXRange_ms / chartRelay.ChartAreas[0].AxisX.Interval) > 10)
            {
                chartRelay.ChartAreas[0].AxisX.Interval *= 2;
            }

            switch (Params.Mode)
            {
                case MODE_T.ON:
                    #region MODE_T.ON
                    AddData(0, 0f, Params.InvertedMode == false ? 1f : 0f);
                    AddData(0, Params.ChartXRange_ms, Params.InvertedMode == false ? 1f : 0f);
                    break;
                    #endregion
                case MODE_T.OFF:
                    #region MODE_T.OFF
                    AddData(0, 0f, Params.InvertedMode == false ? 0f : 1f);
                    AddData(0, Params.ChartXRange_ms, Params.InvertedMode == false ? 0f : 1f);
                    break;
                    #endregion
                case MODE_T.TOGGLE:
                    #region MODE_T.TOGGLE
                    AddData(0, 0f, Params.InvertedMode == false ? 0f : 1f);
                    Time = Params.DelayTime_ms;
                    if (Time > Params.ChartXRange_ms)
                    {
                        Time = Params.ChartXRange_ms;
                    }
                    AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);

                    if ((Params.TimeOn_ms + Params.TimeOff_ms) > 0)
                    {
                        while (Time < Params.ChartXRange_ms)
                        {
                            AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);

                            Time += Params.TimeOn_ms;
                            if (Time >= Params.ChartXRange_ms)
                            {
                                if (Time == Params.ChartXRange_ms)
                                {
                                    AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                    AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                }
                                else
                                {
                                    Time = Params.ChartXRange_ms;
                                    AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                }
                            }
                            else
                            {
                                AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);

                                Time += Params.TimeOff_ms;
                                if (Time >= Params.ChartXRange_ms)
                                {
                                    if (Time == Params.ChartXRange_ms)
                                    {
                                        AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                    }
                                    Time = Params.ChartXRange_ms;
                                    
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
                    if (Time > Params.ChartXRange_ms)
                    {
                        Time = Params.ChartXRange_ms;
                    }
                    AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);

                    if ((Params.TimeOn_ms + Params.TimeOff_ms) > 0)
                    {
                        if (Time < Params.ChartXRange_ms)
                        {
                            AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);

                            Time += Params.TimeOn_ms;
                            if (Time >= Params.ChartXRange_ms)
                            {
                                if (Time == Params.ChartXRange_ms)
                                {
                                    AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                    AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                }
                                else
                                {
                                    Time = Params.ChartXRange_ms;
                                    AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                    //AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);
                                }
                            }
                            else
                            {
                                AddData(0, Time, Params.InvertedMode == false ? 1f : 0f);
                                AddData(0, Time, Params.InvertedMode == false ? 0f : 1f);

                                Time = Params.ChartXRange_ms;
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
        public bool DrawGraph()
        {
            bool Error;

            Error = false;

            if (Params.ChannelInitialized)
            {
                Params.ChartXRange_ms = Params.DelayTime_ms + Params.TimeOn_ms + Params.TimeOff_ms;

                chartRelay.Series.Clear();

                chartRelay.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series(Params.ChartDName));
                chartRelay.Series[0].Color = Params.ChartLColor;
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

                SetSettings();

                Draw();
            }
            else
            {
                Error = true;
            }

            return Error;
        }
        private void Init(ref SerialPort Port)
        {
            Params.NightMode = false;
            Params.ChannelInitialized = false;
        }
        public void Init(string DataName, MODE_T Mode, UInt32 DelayTime_ms, UInt32 TimeOn_ms, UInt32 TimeOff_ms, bool Inverted)
        {
            Init(DataName, Mode, DelayTime_ms, TimeOn_ms, TimeOff_ms, Inverted, Params.ChartLColor);
        }
        public void Init(string DataName, MODE_T Mode, UInt32 DelayTime_ms, UInt32 TimeOn_ms, UInt32 TimeOff_ms, bool Inverted, Color LineColor)
        {
            Params.ChartLColor = LineColor;
            Params.ChartDName = DataName;
            Params.Mode = Mode;
            Params.DelayTime_ms = DelayTime_ms;
            Params.TimeOn_ms = TimeOn_ms;
            Params.TimeOff_ms = TimeOff_ms;
            Params.InvertedMode = Inverted;

            Params.ChartXRange_ms = Params.DelayTime_ms + Params.TimeOn_ms + Params.TimeOff_ms;

            chartRelay.Series.Clear();

            chartRelay.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series(Params.ChartDName));
            chartRelay.Series[0].Color = Params.ChartLColor;
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

            SetSettings();

            Draw();
        }
        public void InitModeOff(string DataName, bool Inverted)
        {
            InitModeOff(DataName, Inverted, Params.ChartLColor);
        }
        public void InitModeOff(string DataName, bool Inverted, Color LineColor)
        {
            Init(DataName, MODE_T.OFF, 0, 0, 0, Inverted, LineColor);
        }
        public void InitModeOn(string DataName, bool Inverted)
        {
            InitModeOn(DataName, Inverted, Params.ChartLColor);
        }
        public void InitModeOn(string DataName, bool Inverted, Color LineColor)
        {
            Init(DataName, MODE_T.ON, 0, 0, 0, Inverted, LineColor);
        }
        public bool Invert
        {
            get
            {
                return Params.InvertedMode;
            }
            set
            {
                Params.InvertedMode = value;

                Draw();
            }
        }
        public MODE_T Mode
        {
            get
            {
                return Params.Mode;
            }
            set
            {
                Params.Mode = value;
                Draw();
            }
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
        public bool NightMode
        {
            get 
            { 
                return Params.NightMode; 
            }
            set
            {
                Params.NightMode = value;

                lblEnable.ForeColor = value ? Color.White : Color.Black;
                lblEnable.BackColor = value ? Color.Black : Color.White;
                lblInvertingMode.ForeColor = value ? Color.White : Color.Black;
                lblInvertingMode.BackColor = value ? Color.Black : Color.White;
                lblInvertingMode.ForeColor = value ? Color.White : Color.Black;
                ledInvertingMode.BackColor = value ? Color.Black : Color.White;
                ledEnable.BackColor = value ? Color.Black : Color.White;
                chartRelay.ForeColor = value ? Color.White : Color.Black;
                chartRelay.BackColor = value ? Color.Black : Color.White;
                chartRelay.ChartAreas[0].BorderColor = value ? Color.White : Color.Black;
                chartRelay.ChartAreas[0].BackColor = value ? Color.Black : Color.White;
                chartRelay.ChartAreas[0].AxisX.LineColor = value ? Color.White : Color.Black;
                chartRelay.ChartAreas[0].AxisY.LineColor = value ? Color.White : Color.Black;
                chartRelay.ChartAreas[0].AxisX.TitleForeColor = value ? Color.White : Color.Black;
                chartRelay.ChartAreas[0].AxisX2.TitleForeColor = value ? Color.White : Color.Black;
                chartRelay.ChartAreas[0].AxisY.TitleForeColor = value ? Color.White : Color.Black;
                chartRelay.ChartAreas[0].AxisY2.TitleForeColor = value ? Color.White : Color.Black;
                chartRelay.ChartAreas[0].AxisX.LabelStyle.ForeColor = value ? Color.White : Color.Black;
                chartRelay.ChartAreas[0].AxisY.LabelStyle.ForeColor = value ? Color.White : Color.Black;
            }
        }
        private void SetSettings()
        {
            //lblEnable.Visible = txtDelay_ms.Visible = (Params.Mode == MODE_T.IMPULSE) | (Params.Mode == MODE_T.TOGGLE);
            //txtDelay_ms.Text = Params.DelayTime_ms.ToString();
            //lblOn_ms.Visible = txtOn_ms.Visible = (Params.Mode == MODE_T.IMPULSE) | (Params.Mode == MODE_T.TOGGLE);
            //txtOn_ms.Text = Params.TimeOn_ms.ToString();
            //lblOff_ms.Visible = txtOff_ms.Visible = (Params.Mode == MODE_T.IMPULSE) | (Params.Mode == MODE_T.TOGGLE);
            //txtOff_ms.Text = Params.TimeOff_ms.ToString();
            ledEnable.On = Params.Enabled;
            ledInvertingMode.On = Params.InvertedMode;
        }
        public MODE_T StringToMode(string ModeAsString)
        {
            MODE_T Mode;

            Mode = MODE_T.OFF;

            switch(ModeAsString)
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
        public UInt32 TimeOff
        {
            get
            {
                return Params.TimeOff_ms;
            }
            set
            {
                Params.TimeOff_ms = value;

                Draw();
            }
        }
        public UInt32 TimeOn
        {
            get
            {
                return Params.TimeOn_ms;
            }
            set
            {
                Params.TimeOn_ms = value;

                Draw();
            }
        }
        public UInt32 XRange
        {
            get
            {
                return Params.ChartXRange_ms;
            }
            set
            {
                if (value > 0)
                {
                    Params.ChartXRange_ms = value;
                    Draw();
                }
            }

        }

        private void AddSettingsMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Result;

            RelayControlSettings AddSettingsDlg = new RelayControlSettings();

            AddSettingsDlg.Parameter = Params;
           
            AddSettingsDlg.SetParameter += SetParameter;

 
            Result = AddSettingsDlg.ShowDialog();

            if (Result == DialogResult.OK)
            {
                Params = AddSettingsDlg.Parameter;
                SetSettings();
                Draw();
            }
        }

        private void SetParameter(object sender, SetParameterEventArgs e)
        {
            switch (e.Parameter)
            {
                case WHICH_PARAMETER_T.MODE:
                    //SetAction(ACTION_T.SET_MODE, "TOGGLE", 0, 0, 0, false);
                    SetAction(ACTION_T.SET_MODE, ModeToString(e.Mode), 0, 0, 0, false);
                    break;

                default:
                    break;
            }
//            MessageBox.Show($"ParameterIndex = {e.Parameter}");
            MessageBox.Show($"Event ausgelöst\n\rParameterIndex = {e.Parameter}");
        }
    }
}
