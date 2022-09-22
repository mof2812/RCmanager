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
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace RelayUserMonitoring
{
    public enum TIMEBASE_UNIT_T
    {
        MILLISECONDS = 1,
        SECONDS = 1000,
        MINUTES = 60000,
        HOURS = 3600000,
    }
    public struct PARAM_T
    {
        public bool NightMode;
        public SerialPort ComPort;
        public byte CardIndex;
        public bool AddButton;
        public int[,] LastValue;
        public bool MonitoringRunning;
        public TIMEBASE_UNIT_T[] TimebaseUnit;
        public string[] TimeUnit;
    }
    public struct TIMEBASE_T
    {
        public int TimeBase_ms;
        public TIMEBASE_UNIT_T TimeBase;
        public string TimeBaseAsString;
    }
    public partial class RelayUserMonitoring : UserControl
    {
        private const int Channels = 24;//MOF RCmanager.Constants.MODULES * RCmanager.Constants.CHANNELS; // 16
        private const int TriggerChannels = RCmanager.Constants.IRQ_IOS; // 4
        private PARAM_T Params;
        private Relay.PARAMS_T[] RelayParams = new Relay.PARAMS_T[Channels];
        private Trigger.PARAMS_T[] TriggerParams = new Trigger.PARAMS_T[TriggerChannels];
        private Stopwatch MonitoringTime = new Stopwatch();

        public RelayUserMonitoring()
        {
            InitializeComponent();
        }
        //private RelayMonitoring.RelayMonitoring GetRelayMonitoring(byte Channel)
        //{
        //    RelayMonitoring.RelayMonitoring rm;

        //    rm = null;

        //    if (Channel / 8 == 0)
        //    {
        //        switch (Channel % 8)
        //        {
        //            case 0:
        //                rm = rmM1_CH1;
        //                break;
        //            case 1:
        //                rm = rmM1_CH2;
        //                break;
        //            case 2:
        //                rm = rmM1_CH3;
        //                break;
        //            case 3:
        //                rm = rmM1_CH4;
        //                break;
        //            case 4:
        //                rm = rmM1_CH5;
        //                break;
        //            case 5:
        //                rm = rmM1_CH6;
        //                break;
        //            case 6:
        //                rm = rmM1_CH7;
        //                break;
        //            case 7:
        //                rm = rmM1_CH8;
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        switch (Channel % 8)
        //        {
        //            case 0:
        //                rm = rmM2_CH1;
        //                break;
        //            case 1:
        //                rm = rmM2_CH2;
        //                break;
        //            case 2:
        //                rm = rmM2_CH3;
        //                break;
        //            case 3:
        //                rm = rmM2_CH4;
        //                break;
        //            case 4:
        //                rm = rmM2_CH5;
        //                break;
        //            case 5:
        //                rm = rmM2_CH6;
        //                break;
        //            case 6:
        //                rm = rmM2_CH7;
        //                break;
        //            case 7:
        //                rm = rmM2_CH8;
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    return rm;
        //}
        //private TriggerMonitoring.TriggerMonitoring GetTriggerMonitoring(byte Channel)
        //{
        //    TriggerMonitoring.TriggerMonitoring tm;

        //    tm = null;

        //    if (Channel < 8)
        //    {
        //        switch (Channel)
        //        {
        //            case 0:
        //                tm = tm_CH1;
        //                break;
        //            case 1:
        //                tm = tm_CH2;
        //                break;
        //            case 2:
        //                tm = tm_CH3;
        //                break;
        //            case 3:
        //                tm = tm_CH4;
        //                break;
        //            case 4:
        //                tm = tm_CH5;
        //                break;
        //            case 5:
        //                tm = tm_CH6;
        //                break;
        //            case 6:
        //                tm = tm_CH7;
        //                break;
        //            case 7:
        //                tm = tm_CH8;
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return tm;
        //}
        public void Init()
        {

            Params.LastValue = new int[2, Channels];
            Params.TimebaseUnit = new TIMEBASE_UNIT_T[2];
            Params.TimeUnit = new string[2];

            for (int Chart = 0; Chart < 2; Chart++)
            {
                Params.TimebaseUnit[Chart] = TIMEBASE_UNIT_T.MILLISECONDS;
                Params.TimeUnit[Chart] = "ms";

                for (int Index = 0; Index < 24; Index++)
                {
                    Params.LastValue[Chart, Index] = -99999999;
                }
            }
            Params.AddButton = false;
            Params.MonitoringRunning = false;

            SetListBox(true, true);
            SetListBox(true, false);
            SetListBox(false, true);
            SetListBox(false, false);

            Init_ComboBox(0);
            Init_ComboBox(1);

            Init_Graphs();

            btnStartStopMonitoring.Text = "Start";
            btnStartStopMonitoring.BackColor = Color.Green;
        }
        public bool Init_ComboBox(byte ChartID)
        {
            bool Error;
            int TimeBase_ms;
            string Item;
            ComboBox cb = new ComboBox();
    
            Error = false;

            if (ChartID < 2)
            {
                cb = ChartID == 0 ? cmbTimebaseChart_1 : cmbTimebaseChart_2;

                Params.TimeUnit[ChartID] = "";

                for (int Index = 0; Index < cmbTimebaseChart_1.Items.Count; Index++)
                {
                    Item = cb.Items[Index].ToString();

                    if (Item.Substring(Item.Length - 2, 2) == "ms")
                    {
                        Params.TimebaseUnit[ChartID] = TIMEBASE_UNIT_T.MILLISECONDS;
                        Params.TimeUnit[ChartID] = "ms";
                        TimeBase_ms = Convert.ToInt32(Item.Substring(0, Item.Length - 2));
                    }
                    else
                    {
                        TimeBase_ms = Convert.ToInt32(Item.Substring(0, Item.Length - 1));

                        switch (Item.Substring(Item.Length - 1, 1))
                        {
                            case "s":
                                Params.TimebaseUnit[ChartID] = TIMEBASE_UNIT_T.SECONDS;
                                break;

                            case "m":
                                Params.TimebaseUnit[ChartID] = TIMEBASE_UNIT_T.MINUTES;
                                break;

                            case "h":
                                Params.TimebaseUnit[ChartID] = TIMEBASE_UNIT_T.HOURS;
                                break;

                            default:
                                Error = true;
                                break;
                        }
                        if (!Error)
                        {
                            Params.TimeUnit[ChartID] = Item.Substring(Item.Length - 1, 1);
                            TimeBase_ms *= (int)Params.TimebaseUnit[ChartID];
                        }
                    }
                    if (TimeBase_ms == RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms[ChartID])
                    {
                        cb.Text = Item;

                        break;
                    }
                }
            }
            else
            {
                Error = true;
            }


            return Error;
        }
        private bool Init_Graph(byte ChartID)
        {
            bool Error;
            Chart chart = new Chart();
            int SeriesID;

            Error = false;
            SeriesID = -1;

            if (ChartID == 0)
            {
                chart = UserChart_1;
            }
            else if(ChartID == 1)
            {
                chart = UserChart_2;
            }
            else
            {
                Error = true;
            }

            if (!Error)
            {
                double Maximum = 0;
//MOF                Maximum = (double)RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms[ChartID] / (double)Params.TimebaseUnit[ChartID];

                chart.ChartAreas[0].AxisX.Minimum = 0;
                chart.ChartAreas[0].AxisX.Maximum = Maximum;
                //chart.ChartAreas[0].AxisX.Maximum = RelayCard16.RelaySettings.Default.UserMonitoringTimeBase_ms[ChartID];
                chart.ChartAreas[0].AxisY.Minimum = 0;
                chart.ChartAreas[0].AxisY.Maximum = 1.1;
                chart.ChartAreas[0].AxisY.Interval = 1;
//MOF                chart.ChartAreas[0].AxisX.Title = $"Zeit [{Params.TimeUnit[ChartID]}]";
                chart.ChartAreas[0].AxisY.Title = "Status";

                chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
                chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                chart.Series.Clear();

                for (int Index = 0; Index < Channels; Index++)
                {
//MOF                    if (((ChartID == 0) && (RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1[Index])) || ((ChartID == 1) && (RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_2[Index])))
if (true)
                    {
//MOF                        chart.Series.Add(new Series(Index < 16 ? RelayParams[Index].SignalLabel : RelayCard16.RelaySettings.Default.TriggerName[Index-16]));

                        SeriesID = chart.Series.Count() - 1;
                        chart.Series[SeriesID].Color = RelayParams[Index].ChartLColor;
                        chart.Series[SeriesID].BorderWidth = 1;
                        chart.Series[SeriesID].ChartType = SeriesChartType.Line;
                        if (ChartID == 0)
                        {
                            RCmanager.Properties.MonitoringSettings.Default.UserMonitoringSeries_Chart_1[Index] = SeriesID;
                        }
                        else 
                        { 
                            RCmanager.Properties.MonitoringSettings.Default.UserMonitoringSeries_Chart_2[Index] = SeriesID; 
                        }
                        chart.Series[SeriesID].Points.AddXY(0, 0);
                    }
                    else
                    {
                        if (ChartID == 0)
                        {
                            RCmanager.Properties.MonitoringSettings.Default.UserMonitoringSeries_Chart_1[Index] = -1;
                        }
                        else
                        {
                            RCmanager.Properties.MonitoringSettings.Default.UserMonitoringSeries_Chart_2[Index] = -1;
                        }
                    }

                }
                RCmanager.Properties.MonitoringSettings.Default.Save();


//                SetSettings();

//                Draw();
            }
            else
            {
                Error = true;
            }

            return Error;
        }   
        private bool Init_Graphs()
        {
            return Init_Graph(0) | Init_Graph(1);
        }
        private bool Init_Settings()
        {
            bool Error;

            Error = false;

            if (RCmanager.Properties.MonitoringSettings.Default.InitCode != Constants.INITCODE)
            {
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms = new Int32[2];
                RCmanager.Properties.MonitoringSettings.Default.LineColorRGB = new Color[Channels];
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1 = new bool[Channels];
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_2 = new bool[Channels];
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoringSeries_Chart_1 = new int[Channels];
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoringSeries_Chart_2 = new int[Channels];

                SetDefault();

                RCmanager.Properties.MonitoringSettings.Default.InitCode = Constants.INITCODE;

                RCmanager.Properties.MonitoringSettings.Default.Save();
            }
            return Error;
        }
        public void Monitoring(UInt32 RelayStates)
        {
            long ElapsedMilliseconds = MonitoringTime.ElapsedMilliseconds;

            if (Params.MonitoringRunning)
            {
                Monitoring(0, RelayStates, ElapsedMilliseconds);
                Monitoring(1, RelayStates, ElapsedMilliseconds);
            }
        }
        private void Monitoring(byte ChartID, UInt32 RelayStates, long ElapsedMilliseconds)
        {
            bool Error;
            bool Init;
            Chart chart = new Chart();
            int SeriesID;
            double ElapsedTime;
            double Timebase;

            Error = false;
            Init = false;
            SeriesID = -1;

            if (ChartID == 0)
            {
                chart = UserChart_1;
            }
            else if (ChartID == 1)
            {
                chart = UserChart_2;
            }
            else
            {
                Error = true;
            }

            ElapsedTime = (double)ElapsedMilliseconds / (double)Params.TimebaseUnit[ChartID];
            Timebase = (double)RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms[ChartID] / (double)Params.TimebaseUnit[ChartID];
            

            for (int Index = 0; Index < 24; Index++)
            {
                if (((ChartID == 0) && (RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1[Index])) || ((ChartID == 1) && (RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_2[Index])))
                {
                    if (chart.ChartAreas[0].AxisX.Maximum < ElapsedTime)
                    {
                        //chart.ChartAreas[0].AxisX.Maximum += Timebase * 0.1;
                        chart.ChartAreas[0].AxisX.Maximum = ElapsedTime;

                        chart.ChartAreas[0].AxisX.Minimum = chart.ChartAreas[0].AxisX.Maximum - Timebase;
                    }
                    SeriesID++;
                    if (SeriesID <= chart.Series.Count())
                    {
                        if (Index < 16)
                        {
                            Init = (Params.LastValue[ChartID, SeriesID] == -99999999);
                            if (!Init)
                            {
                                chart.Series[SeriesID].Points.AddXY(ElapsedTime, Params.LastValue[ChartID, SeriesID]);
                            }

                            Params.LastValue[ChartID, SeriesID] = (RelayStates & (0x0001 << Index)) == 0 ? 0 : 1;

                            if (Init)
                            {

                                chart.Series[SeriesID].Points.AddXY(0, Params.LastValue[ChartID, SeriesID]);
                            }
                            chart.Series[SeriesID].Points.AddXY(ElapsedTime, Params.LastValue[ChartID, SeriesID]);
                        }
                        else
                        {
                            if (Params.LastValue[ChartID, SeriesID] != -99999999)
                            {
                                chart.Series[SeriesID].Points.AddXY(ElapsedTime, Params.LastValue[ChartID, SeriesID]);
                            }
                            Params.LastValue[ChartID, SeriesID] = (RelayStates & (0x0001 << (Index + 8))) == 0 ? 0 : 1;
                            chart.Series[SeriesID].Points.AddXY(ElapsedTime, Params.LastValue[ChartID, SeriesID]);
                        }
                    }
                }
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
                //value = false;
                Params.NightMode = value;
                this.BackColor = value ? Color.Black : Color.White;

                UserChart_1.BackColor = value ? Color.Black : Color.White;
                UserChart_1.ForeColor = value ? Color.White : Color.Black;
                UserChart_1.ChartAreas[0].BorderColor = value ? Color.White : Color.Black;
                UserChart_1.ChartAreas[0].BackColor = value ? Color.Black : Color.White;
                UserChart_1.ChartAreas[0].AxisX.LineColor = value ? Color.White : Color.Black;
                UserChart_1.ChartAreas[0].AxisY.LineColor = value ? Color.White : Color.Black;
                UserChart_1.ChartAreas[0].AxisX.TitleForeColor = value ? Color.White : Color.Black;
                UserChart_1.ChartAreas[0].AxisX2.TitleForeColor = value ? Color.White : Color.Black;
                UserChart_1.ChartAreas[0].AxisY.TitleForeColor = value ? Color.White : Color.Black;
                UserChart_1.ChartAreas[0].AxisY2.TitleForeColor = value ? Color.White : Color.Black;
                UserChart_1.ChartAreas[0].AxisX.LabelStyle.ForeColor = value ? Color.White : Color.Black;
                UserChart_1.ChartAreas[0].AxisY.LabelStyle.ForeColor = value ? Color.White : Color.Black;
                UserChart_1.Legends[0].ForeColor = value ? Color.White : Color.Black;
                UserChart_1.Legends[0].BackColor = value ? Color.Black : Color.White;

                UserChart_2.BackColor = value ? Color.Black : Color.White;
                UserChart_2.ForeColor = value ? Color.White : Color.Black;
                UserChart_2.ChartAreas[0].BorderColor = value ? Color.White : Color.Black;
                UserChart_2.ChartAreas[0].BackColor = value ? Color.Black : Color.White;
                UserChart_2.ChartAreas[0].AxisX.LineColor = value ? Color.White : Color.Black;
                UserChart_2.ChartAreas[0].AxisY.LineColor = value ? Color.White : Color.Black;
                UserChart_2.ChartAreas[0].AxisX.TitleForeColor = value ? Color.White : Color.Black;
                UserChart_2.ChartAreas[0].AxisX2.TitleForeColor = value ? Color.White : Color.Black;
                UserChart_2.ChartAreas[0].AxisY.TitleForeColor = value ? Color.White : Color.Black;
                UserChart_2.ChartAreas[0].AxisY2.TitleForeColor = value ? Color.White : Color.Black;
                UserChart_2.ChartAreas[0].AxisX.LabelStyle.ForeColor = value ? Color.White : Color.Black;
                UserChart_2.ChartAreas[0].AxisY.LabelStyle.ForeColor = value ? Color.White : Color.Black;
                UserChart_2.Legends[0].ForeColor = value ? Color.White : Color.Black;
                UserChart_2.Legends[0].BackColor = value ? Color.Black : Color.White;

                lblTimebase_1.ForeColor = lblTimebase_2.ForeColor = value ? Color.White : Color.Black;
            }
        }
        private int SelectedToIndex(string Name)
        {
            int RetVal;
            int Index;

            RetVal = -1;

            for(Index = 0; Index < 24; Index++)
            {
                if (Index < 16)
                {
                    if (Name == RelayParams[Index].SignalLabel)
                    {
                        RetVal = Index;
                        break;
                    }
                }
                else
                {
                    if (true)// MOF Name == RelayCard16.RelaySettings.Default.TriggerName[Index-16])
                    {
                        RetVal = Index;
                        break;
                    }
                }
            }

            return RetVal;
        }
        private void SetDefault()
        {
            RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms[0] = RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms[1] = 10000;
            for (int Index = 0; Index < 24; Index++)
            {
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1[Index] = false;
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_2[Index] = false;
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoringSeries_Chart_1[Index] = -1;
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoringSeries_Chart_2[Index] = -1;
            }

        }
        private void SetListBox(bool Chart1, bool Add)
        {
            ListBox lb = new ListBox();
            Button btn = new Button();
            int Index;
            int Count;

            lb = Chart1 ? lbSignalSelect_Chart_1 : lbSignalSelect_Chart_2;
            if (Chart1)
            {
                btn = Add ? btnAddSignal_Chart_1 : btnDelSignal_Chart_1;
            }
            else
            {
                btn = Add ? btnAddSignal_Chart_2 : btnDelSignal_Chart_2;
            }

            Count = 0;

            lb.Items.Clear();

            for (Index = 0; Index < 24; Index++)
            {
                if (Chart1)
                {
                    if (RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1[Index] != Add)
                    {
                        if (Index < 16)
                        {
                            lb.Items.Add(RelayParams[Index].SignalLabel);
                        }
                        else
                        {
// MOF                            lb.Items.Add(RelayCard16.RelaySettings.Default.TriggerName[Index-16]);
                        }
                        Count++;
                    }
                }
                else
                {
                    if (RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_2[Index] != Add)
                    {
                        if (Index < 16)
                        {
                            lb.Items.Add(RelayParams[Index].SignalLabel);
                        }
                        else
                        {
// MOF                            lb.Items.Add(RelayCard16.RelaySettings.Default.TriggerName[Index-16]);
                        }
                        Count++;
                    }
                }
            }

            btn.Visible = Count > 0;
        }
        public bool SetRelayParams(byte Channel, Relay.PARAMS_T Params)
        {
            bool Error;

            Error = false;

            if (Channel < RCmanager.Constants.MODULES * RCmanager.Constants.CHANNELS)
            {
                RelayParams[Channel] = Params;
            }
            else
            {
                Error = true;
            }

            return Error;
        }
        private void SetTimebase(byte ChartID, TIMEBASE_T Timebase)
        {
            Chart chart = new Chart();

            if (ChartID < 2)
            {
                chart = ChartID == 0 ? UserChart_1 : UserChart_2;

                if (Params.MonitoringRunning)
                {
                    double Maximum = (double)RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms[ChartID] / (double)Params.TimebaseUnit[ChartID];

                    if (Maximum < chart.ChartAreas[0].AxisX.Maximum)
                    {
                        Maximum = chart.ChartAreas[0].AxisX.Maximum;
                    }

                    if ((chart.ChartAreas[0].AxisX.Maximum - Maximum) >= 0)
                    {
                        chart.ChartAreas[0].AxisX.Minimum = chart.ChartAreas[0].AxisX.Maximum - Maximum;
                    }
                    else
                    {
                        chart.ChartAreas[0].AxisX.Minimum = 0;
                    }
                    chart.ChartAreas[0].AxisX.Maximum = Maximum;
                }
                else
                {
                    Params.TimebaseUnit[ChartID] = Timebase.TimeBase;
                    Params.TimeUnit[ChartID] = Timebase.TimeBaseAsString;

                    double Maximum = (double)RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms[ChartID] / (double)Params.TimebaseUnit[ChartID];
                    chart.ChartAreas[0].AxisX.Minimum = 0;
                    chart.ChartAreas[0].AxisX.Maximum = Maximum;

                    
                }
                chart.ChartAreas[0].AxisX.Title = $"Zeit [{Params.TimeUnit[ChartID]}]";
            }
        }
        public bool SetTriggerParams(byte Channel, Trigger.PARAMS_T Params)
        {
            bool Error;

            Error = false;

            if (Channel < RCmanager.Constants.IRQ_IOS)
            {
                TriggerParams[Channel] = Params;
            }
            else
            {
                Error = true;
            }

            return Error;
        }
        private void StartStopMonitoring()
        {
            //UserChart_1.Series.Clear();
            //UserChart_2.Series.Clear();

            if (!Params.MonitoringRunning)
            {
                MonitoringTime.Restart();
                Params.MonitoringRunning = true;

                Init_Graphs();
            }
            else
            {
                MonitoringTime.Stop();
                Params.MonitoringRunning = false;
            }
        }
        private TIMEBASE_T TextToMilliseconds(string Text)
        {
            TIMEBASE_T Timebase;
            bool Error;

            Error = false;

            Timebase.TimeBase_ms = 0;
            Timebase.TimeBase = TIMEBASE_UNIT_T.MILLISECONDS;
            Timebase.TimeBaseAsString = "";

            if (Text.Substring(Text.Length - 2, 2) == "ms")
            {
                Timebase.TimeBase = TIMEBASE_UNIT_T.MILLISECONDS;
                Timebase.TimeBaseAsString = "ms";
                Timebase.TimeBase_ms = Convert.ToInt32(Text.Substring(0, Text.Length - 2));
            }
            else
            {
                Timebase.TimeBase_ms = Convert.ToInt32(Text.Substring(0, Text.Length - 1));

                switch (Text.Substring(Text.Length - 1, 1))
                {
                    case "s":
                        Timebase.TimeBase = TIMEBASE_UNIT_T.SECONDS;
                        break;

                    case "m":
                        Timebase.TimeBase = TIMEBASE_UNIT_T.MINUTES;
                        break;

                    case "h":
                        Timebase.TimeBase = TIMEBASE_UNIT_T.HOURS;
                        break;

                    default:
                        Error = true;
                        break;
                }
                if (!Error)
                {
                    Timebase.TimeBaseAsString = Text.Substring(Text.Length - 1, 1);
                    Timebase.TimeBase_ms *= (int)Timebase.TimeBase;
                }
            }
            return Timebase;
        }
        private Color Uint_To_Color(uint RGB)
        {
            Color color = new Color();
            color = Color.FromArgb((byte)(RGB >> 16), (byte)(RGB >> 8), (byte)RGB);

            return color;
        }
        private void btnAddSignal_Chart_1_Click(object sender, EventArgs e)
        {
            lbSignalSelect_Chart_1.Visible = true;
            Params.AddButton = true;

            SetListBox(true, true);
        }

        private void btnDelSignal_Chart_1_Click(object sender, EventArgs e)
        {
            lbSignalSelect_Chart_1.Visible = true;
            Params.AddButton = false;

            SetListBox(true, false);
        }

        private void btnAddSignal_Chart_2_Click(object sender, EventArgs e)
        {
            lbSignalSelect_Chart_2.Visible = true;
            Params.AddButton = true;

            SetListBox(false, true);
        }

        private void btnDelSignal_Chart_2_Click(object sender, EventArgs e)
        {
            lbSignalSelect_Chart_2.Visible = true;
            Params.AddButton = false;

            SetListBox(false, false);
        }

        private void lbSignalSelect_Chart_1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int Index;

            Index = SelectedToIndex(lbSignalSelect_Chart_1.SelectedItem.ToString());

            if (Index >= 0)
            {
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1[Index] = !RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1[Index];
                RCmanager.Properties.MonitoringSettings.Default.Save();

                SetListBox(true, !Params.AddButton);

                lbSignalSelect_Chart_1.Visible = false;

                Init_Graphs();
            }
        }
        private void lbSignalSelect_Chart_1_KeyPress(object sender, KeyPressEventArgs e)
        {
            int Index;

            if (e.KeyChar == 0x0D)
            {
                Index = SelectedToIndex(lbSignalSelect_Chart_1.SelectedItem.ToString());

                if (Index >= 0)
                {
                    RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1[Index] = !RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1[Index];
                    RCmanager.Properties.MonitoringSettings.Default.Save();

                    SetListBox(true, !Params.AddButton);

                    lbSignalSelect_Chart_1.Visible = false;

                    Init_Graphs();
                }
            }
            else if (e.KeyChar == 0x1B)
            {
                lbSignalSelect_Chart_1.Visible = false;
            }
        }

        private void lbSignalSelect_Chart_2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int Index;

            Index = SelectedToIndex(lbSignalSelect_Chart_2.SelectedItem.ToString());

            if (Index >= 0)
            {
                RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_2[Index] = !RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_2[Index];
                RCmanager.Properties.MonitoringSettings.Default.Save();

                SetListBox(false, !Params.AddButton);

                lbSignalSelect_Chart_2.Visible = false;
                Init_Graphs();
            }
        }

        private void lbSignalSelect_Chart_2_KeyPress(object sender, KeyPressEventArgs e)
        {
            int Index;

            if (e.KeyChar == 0x0D)
            {
                Index = SelectedToIndex(lbSignalSelect_Chart_2.SelectedItem.ToString());

                if (Index >= 0)
                {
                    RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_2[Index] = !RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_2[Index];
                    RCmanager.Properties.MonitoringSettings.Default.Save();

                    SetListBox(false, !Params.AddButton);

                    lbSignalSelect_Chart_2.Visible = false;

                    Init_Graphs();
                }
            }
            else if (e.KeyChar == 0x1B)
            {
                lbSignalSelect_Chart_2.Visible = false;
            }
        }

        private void RelayUserMonitoring_Load(object sender, EventArgs e)
        {
            Init_Graphs();
        }

        private void cmbTimebaseChart_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Item;
            byte Index;
            TIMEBASE_T Timebase;

            Item = cmbTimebaseChart_1.Text;
            Index = 0;

            if (Item.Length > 0)
            {
                Timebase = TextToMilliseconds(Item);

                RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms[Index] = Timebase.TimeBase_ms;

                SetTimebase(Index, Timebase);

                RCmanager.Properties.MonitoringSettings.Default.Save();
            }
        }

        private void cmbTimebaseChart_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Item;
            byte Index;
            TIMEBASE_T Timebase;

            Item = cmbTimebaseChart_2.Text;
            Index = 1;
            

            if (Item.Length > 0)
            {
                Timebase = TextToMilliseconds(Item);

                RCmanager.Properties.MonitoringSettings.Default.UserMonitoringTimeBase_ms[Index] = Timebase.TimeBase_ms;

                SetTimebase(Index, Timebase);

                RCmanager.Properties.MonitoringSettings.Default.Save();
            }

        }
        private void btnStartStopMonitoring_Click(object sender, EventArgs e)
        {
            StartStopMonitoring();

            btnStartStopMonitoring.Text = Params.MonitoringRunning ? "Stop" : "Start";
            btnStartStopMonitoring.BackColor = Params.MonitoringRunning ? Color.Red : Color.Green;
        }
    }
    static class Constants
    {
        public const uint INITCODE = 0x55AA55AA;
        //public const byte MODULES = 2;          // Number of relaycards  
        //public const byte CHANNELS = 8;         // Number of relays on one relaycard
        //public const byte IRQ_IOS = 4;          // Number of interrupt IOs
    }
}
