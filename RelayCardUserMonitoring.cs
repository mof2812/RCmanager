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
using System.Diagnostics;

namespace RelayCardUserMonitoring
{
    public struct TIMEBASE_T
    {
        public int TimeBase_ms;
        public TIMEBASE_UNIT_T TimeBase;
        public string TimeBaseAsString;
    }
    public enum TIMEBASE_UNIT_T
    {
        MILLISECONDS = 1,
        SECONDS = 1000,
        MINUTES = 60000,
        HOURS = 3600000,
    }
    public enum MONITORING_STATE_T
    {
        STOPPED = 0,
        PAUSED,
        RUNNING
    }
    public struct PARAM_T
    {
        public bool NightMode;
        public string[] SignalName;
        public Color[] SignalChartLColor;
        public string[] TriggerName;
        public Color[] TriggerChartLColor;
        //public SerialPort ComPort;
        //public byte CardIndex;
        public bool AddButton;
        //public int[,] LastValue;
        public MONITORING_STATE_T MonitoringState;
        public TIMEBASE_UNIT_T[] TimebaseUnit;
        public string[] TimeUnit;
        public int RelayChannels;
        public int TriggerChannels;
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
    public partial class RelayCardUserMonitoring : UserControl
    {
        #region Members
        private PARAM_T Params;
        private Stopwatch MonitoringTime = new Stopwatch();
        #endregion
        public RelayCardUserMonitoring()
        {
            InitializeComponent();

            Init();
        }
        #region Methods
        private RETURN_T Init()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            Params.RelayChannels = RCmanager.Constants.MODULES * RCmanager.Constants.CHANNELS;
            Params.TriggerChannels = RCmanager.Constants.IRQ_IOS;

            Params.SignalName = new string[Params.RelayChannels];
            Params.SignalChartLColor = new Color[Params.RelayChannels];

            Params.TriggerName = new string[Params.TriggerChannels];
            Params.TriggerChartLColor = new Color[Params.TriggerChannels];

            Params.TimebaseUnit = new TIMEBASE_UNIT_T[2];
            Params.TimeUnit = new string[2];

            Params.MonitoringState = MONITORING_STATE_T.STOPPED;

            Init_Settings();

            Init_ListBoxes();
            Init_ComboBoxes();

            Init_Graphs();

            return Return;
        }
        private bool Init_ComboBoxes()
        {
            return (Init_ComboBox(0) | Init_ComboBox(1));
        }
        private bool Init_ComboBox(byte ChartID)
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

                    if (TimeBase_ms == (int)MyUserMonitoringSettings.settings.UserMonitoringTimeBase_ms[ChartID])
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
            else if (ChartID == 1)
            {
                chart = UserChart_2;
            }
            else
            {
                Error = true;
            }

            if (!Error)
            {
                double Maximum = (double)MyUserMonitoringSettings.settings.UserMonitoringTimeBase_ms[ChartID] / (double)Params.TimebaseUnit[ChartID];

                chart.ChartAreas[0].AxisX.Minimum = 0;
                chart.ChartAreas[0].AxisX.Maximum = Maximum;
                //chart.ChartAreas[0].AxisX.Maximum = RelayCard16.RelaySettings.Default.UserMonitoringTimeBase_ms[ChartID];
                chart.ChartAreas[0].AxisY.Minimum = 0;
                chart.ChartAreas[0].AxisY.Maximum = 1.1;
                chart.ChartAreas[0].AxisY.Interval = 1;
                chart.ChartAreas[0].AxisX.Title = $"Zeit [{Params.TimeUnit[ChartID]}]";
                chart.ChartAreas[0].AxisY.Title = "Status";

                chart.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
                chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                chart.Series.Clear();

                for (int Index = 0; Index < Params.RelayChannels + Params.TriggerChannels; Index++)
                {
                    if (((ChartID == 0) && (MyUserMonitoringSettings.settings.UsedInChart[0, Index])) || ((ChartID == 1) && (MyUserMonitoringSettings.settings.UsedInChart[1, Index])))
                    {
                        chart.Series.Add(new Series(Index < Params.RelayChannels ? Params.SignalName[Index] : Params.TriggerName[Index - Params.RelayChannels]));

                        SeriesID = chart.Series.Count() - 1;
                        chart.Series[SeriesID].Color = Index < Params.RelayChannels ? Params.SignalChartLColor[Index] : Params.TriggerChartLColor[Index - Params.RelayChannels];
                        chart.Series[SeriesID].BorderWidth = 1;
                        chart.Series[SeriesID].ChartType = SeriesChartType.Line;

                        MyUserMonitoringSettings.settings.SeriesInChart[ChartID, Index] = SeriesID;
                        chart.Series[SeriesID].Points.AddXY(0, 0);
                    }
                    else
                    {
                        MyUserMonitoringSettings.settings.SeriesInChart[ChartID, Index] = -1;
                    }

                }

                //chart.Series.Add(new Series("Trennlinie"));
                //SeriesID = chart.Series.Count() - 1;
                //chart.Series[SeriesID].Color = Color.Gray;
                //chart.Series[SeriesID].LabelForeColor = Color.Gray;
                //chart.Series[SeriesID].ChartType = SeriesChartType.Line;
                //chart.Series[SeriesID].IsValueShownAsLabel = false;
                chart.ChartAreas[0].CursorX.LineColor = Color.Green;
                chart.ChartAreas[0].CursorX.LineWidth = 1;
                chart.ChartAreas[0].CursorX.LineDashStyle = ChartDashStyle.Dot;
                chart.ChartAreas[0].CursorX.Interval = 0;
                chart.ChartAreas[0].CursorX.SetCursorPosition((chart.ChartAreas[0].AxisX.Maximum + chart.ChartAreas[0].AxisX.Minimum) / 2);
                chart.ChartAreas[0].CursorX.SetCursorPosition(6);

                MyUserMonitoringSettings.Save();


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
            bool Error;

            Error = false;

            if ((Params.SignalName[0] == null) || (Params.TriggerName[0] == null))
            {
                Error = true;
            }
            else
            {
                Error = Init_Graph(0) | Init_Graph(1);
            }
            return Error;
        }
        private void Init_ListBoxes()
        {
            lbSignalSelect_Chart_1.Items.Clear();
            lbSignalSelect_Chart_2.Items.Clear();
        }
        public void Init_Monitoring(bool NightMode)
        {
            this.NightMode = NightMode;

            UserChart_1.Series.Clear();
            UserChart_2.Series.Clear();
        }
        public bool Init_RelayMonitoring(byte Index, Relay.PARAMS_T Params)
        {
            bool Error;

            Error = false;

            if (Index < (byte)this.Params.RelayChannels)
            {
                this.Params.SignalName[Index] = Params.SignalLabel;
            }

            return Error;
        }
        private RETURN_T Init_Settings()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (MyUserMonitoringSettings.Init($"{Constants.DATAPATH}UserMonitoring.dat"))
            {
                // Set default values
                SetDefaults();
            }

            //if (RCmanager.Properties.MonitoringSettings.Default.InitCode != Constants.INITCODE)  /* Initcode not found -> set default values */
            //{
            //    RCmanager.Properties.UserMonitoringSettings.Default.UserMonitoringTimeBase_ms = new UInt64[2];
            //    RCmanager.Properties.UserMonitoringSettings.Default.UsedInChart = new bool[2, Params.RelayChannels + Params.TriggerChannels];
            //    RCmanager.Properties.MonitoringSettings.Default.UserMonitoring_Chart_1 = new bool[Params.RelayChannels + Params.TriggerChannels];

            //    RCmanager.Properties.MonitoringSettings.Default.InitCode = Constants.INITCODE;

            //    // Set default values
            //    SetDefaults();

            //    RCmanager.Properties.MonitoringSettings.Default.Save();
            //}
            return Return;
        }
        public bool Init_TriggerMonitoring(byte Index, Trigger.PARAMS_T Params)
        {
            bool Error;

            Error = false;

            if (Index < (byte)this.Params.RelayChannels)
            {
                Params.TriggerLabel = Params.TriggerLabel;
            }

            return Error;
        }
        public void Monitoring(UInt32 RelayStates)
        {
            int SeriesID;
            int BitState;
            double Time_ms;
            Chart chart = new Chart();

            txtStates.Text = RelayStates.ToString();
            

            if (Params.MonitoringState == MONITORING_STATE_T.RUNNING)
            {
                for (byte ChartID = 0; ChartID < 2; ChartID++)
                {
                    chart = ChartID == 0 ? UserChart_1 : UserChart_2;

                    Time_ms = (double)MonitoringTime.ElapsedMilliseconds / (double)Params.TimebaseUnit[ChartID];

                    txtElapsed.Text = Convert.ToString(Time_ms);

                    for (int BitPos = 0; BitPos < Params.RelayChannels + Params.TriggerChannels; BitPos++)
                    {
                        SeriesID = MyUserMonitoringSettings.settings.SeriesInChart[ChartID, BitPos];

                        if (SeriesID >= 0)
                        {
                            BitState = (RelayStates & (0x00000001 << BitPos)) == 0 ? 0 : 1;

                            if (SeriesID == 0)
                            {
                                ledSeries0.On = (BitState == 1);
                            }
                            if (chart.ChartAreas[0].AxisX.Maximum < Time_ms)
                            {
                                chart.ChartAreas[0].AxisX.Maximum = Time_ms;
                                chart.ChartAreas[0].AxisX.Minimum = UserChart_1.ChartAreas[0].AxisX.Maximum - (double)MyUserMonitoringSettings.settings.UserMonitoringTimeBase_ms[ChartID] / (double)Params.TimebaseUnit[ChartID];
                            }
                            chart.Series[SeriesID].Points.AddXY(Time_ms, BitState);
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
                Chart chart = new Chart();

                this.BackColor = value ? Color.Black : Color.White;
                this.ForeColor = value ? Color.White : Color.Black;
                grpMonitor_1.ForeColor = grpMonitor_2.ForeColor = this.ForeColor;

                for (byte ChartID = 0; ChartID < 2; ChartID++)
                {
                    btnAddSignal_Chart_1.BackColor = btnAddSignal_Chart_2.BackColor = value ? Color.DarkGray : Color.White;
                    btnDelSignal_Chart_1.BackColor = btnDelSignal_Chart_2.BackColor = value ? Color.DarkGray : Color.White;
                    btnStartStopMonitoring.BackColor = btnPauseMonitoring.BackColor = value ? Color.DarkGray : Color.White;

                    if (ChartID == 0)
                    {
                        chart = UserChart_1;
                    }
                    else
                    {
                        chart = UserChart_2;
                    }
                    chart.BackColor = value ? Color.Black : Color.White;
                    chart.ForeColor = value ? Color.White : Color.Black;
                    chart.ChartAreas[0].BackColor = value ? Color.Black : Color.White;
                    chart.ChartAreas[0].AxisX.LineColor = value ? Color.White : Color.Black;
                    chart.ChartAreas[0].AxisY.LineColor = value ? Color.White : Color.Black;
                    chart.ChartAreas[0].AxisX.TitleForeColor = value ? Color.White : Color.Black;
                    chart.ChartAreas[0].AxisX2.TitleForeColor = value ? Color.White : Color.Black;
                    chart.ChartAreas[0].AxisY.TitleForeColor = value ? Color.White : Color.Black;
                    chart.ChartAreas[0].AxisY2.TitleForeColor = value ? Color.White : Color.Black;
                    chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = value ? Color.White : Color.Black;
                    chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = value ? Color.White : Color.Black;
                    chart.Legends[0].ForeColor = value ? Color.White : Color.Black;
                    chart.Legends[0].BackColor = value ? Color.Black : Color.White;
                }
                lblTimebase_1.ForeColor = lblTimebase_2.ForeColor = value ? Color.White : Color.Black;
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

        private int SelectedToIndex(string Name)
        {
            int RetVal;
            int Index;

            RetVal = -1;

            for (Index = 0; Index < Params.RelayChannels + Params.TriggerChannels; Index++)
            {
                if (Index < 16)
                {
                    if (Name == Params.SignalName[Index])
                    {
                        RetVal = Index;
                        break;
                    }
                }
                else
                {
                    if (Name == Params.TriggerName[Index - Params.RelayChannels])
                    {
                        RetVal = Index;
                        break;
                    }
                }
            }

            return RetVal;
        }
        public void SetDefaults()
        {
            for (int Chart = 0; Chart < 2; Chart++)
            {
                MyUserMonitoringSettings.settings.UserMonitoringTimeBase_ms[Chart] = 10000;
//                RCmanager.Properties.UserMonitoringSettings.Default.UserMonitoringTimeBase_ms[Chart] = 10000;
                for (int Index = 0; Index < Params.RelayChannels + Params.TriggerChannels; Index++)
                {
                    MyUserMonitoringSettings.settings.UsedInChart[Chart, Index] = false;
                    MyUserMonitoringSettings.settings.SeriesInChart[Chart, Index] = -1;
                    //RCmanager.Properties.UserMonitoringSettings.Default.UsedInChart[Chart, Index] = false;
                }
            }

            MyUserMonitoringSettings.Save();
            //RCmanager.Properties.UserMonitoringSettings.Default.Save();
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

            for (Index = 0; Index < Params.RelayChannels + Params.TriggerChannels; Index++)
            {
                if (Chart1)
                {
                    if (MyUserMonitoringSettings.settings.UsedInChart[0,Index] != Add)
                    {
                        if (Index < Params.RelayChannels)
                        {
                            lb.Items.Add(Params.SignalName[Index]);
                        }
                        else
                        {
                            lb.Items.Add(Params.TriggerName[Index - Params.RelayChannels]);
                        }
                        Count++;
                    }
                }
                else
                {
                    if (MyUserMonitoringSettings.settings.UsedInChart[1, Index] != Add)
                    {
                        if (Index < Params.RelayChannels)
                        {
                            lb.Items.Add(Params.SignalName[Index]);
                        }
                        else
                        {
                            lb.Items.Add(Params.TriggerName[Index - Params.RelayChannels]);
                        }
                        Count++;
                    }
                }
            }

            btn.Visible = Count > 0;
        }
        public bool SetRelayParams(byte Index, Relay.PARAMS_T RelayParams)
        {
            bool Error;

            Error = false;

            if (Index < Params.RelayChannels)
            {
                Params.SignalName[Index] = RelayParams.SignalLabel;
                Params.SignalChartLColor[Index] = RelayParams.ChartLColor;

                ////if (RCmanager.Properties.UserMonitoringSettings.Default.UsedInChart[0, Index] == false)
                //if (MyUserMonitoringSettings.settings.UsedInChart[0, Index] == false)
                //{
                //    lbSignalSelect_Chart_1.Items.Add($"({Index}) {Params.SignalName[Index]}");
                //}

                ////if (RCmanager.Properties.UserMonitoringSettings.Default.UsedInChart[1, Index] == false)
                //if (MyUserMonitoringSettings.settings.UsedInChart[0, Index] == false)
                //{
                //    lbSignalSelect_Chart_2.Items.Add($"({Index}) {Params.SignalName[Index]}");
                //}
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

                if (Params.MonitoringState == MONITORING_STATE_T.RUNNING)
                {
                    double Maximum = (double)MyUserMonitoringSettings.settings.UserMonitoringTimeBase_ms[ChartID] / (double)Params.TimebaseUnit[ChartID];

                    if (Maximum < chart.ChartAreas[0].AxisX.Maximum)
                    {
                        //Maximum = chart.ChartAreas[0].AxisX.Maximum;
                        chart.ChartAreas[0].AxisX.Maximum = Maximum;
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

                    double Maximum = (double)MyUserMonitoringSettings.settings.UserMonitoringTimeBase_ms[ChartID] / (double)Params.TimebaseUnit[ChartID];
                    chart.ChartAreas[0].AxisX.Minimum = 0;
                    chart.ChartAreas[0].AxisX.Maximum = Maximum;


                }
                chart.ChartAreas[0].AxisX.Title = $"Zeit [{Params.TimeUnit[ChartID]}]";
            }
        }
        public bool SetTriggerParams(byte Index, Trigger.PARAMS_T TriggerParams)
        {
            bool Error;
            byte Index2;

            Error = false;
            Index2 = Convert.ToByte(Index + Params.RelayChannels);

            if (Index < Params.TriggerChannels)
            {
                Params.TriggerName[Index] = TriggerParams.TriggerLabel;
                Params.TriggerChartLColor[Index] = TriggerParams.ChartLColor;

                ////if (RCmanager.Properties.UserMonitoringSettings.Default.UsedInChart[0, Index2] == false)
                //if (MyUserMonitoringSettings.settings.UsedInChart[0, Index2] == false)
                //{
                //    lbSignalSelect_Chart_1.Items.Add($"(T{Index}) {Params.TriggerName[Index]}");
                //}

                ////if (RCmanager.Properties.UserMonitoringSettings.Default.UsedInChart[1, Index2] == false)
                //if (MyUserMonitoringSettings.settings.UsedInChart[0, Index2] == false)
                //{
                //    lbSignalSelect_Chart_2.Items.Add($"(T{Index}) {Params.TriggerName[Index]}");
                //}
            }
            else
            {
                Error = true;
            }

            return Error;
        }
        #endregion /* Methods */
        #region Events
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
        #endregion /* Events */

        private void lbSignalSelect_Chart_1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int Index;

            Index = SelectedToIndex(lbSignalSelect_Chart_1.SelectedItem.ToString());

            if (Index >= 0)
            {
                MyUserMonitoringSettings.settings.UsedInChart[0, Index] = !MyUserMonitoringSettings.settings.UsedInChart[0, Index];
                MyUserMonitoringSettings.Save();

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
                    MyUserMonitoringSettings.settings.UsedInChart[0, Index] = !MyUserMonitoringSettings.settings.UsedInChart[0, Index];
                    MyUserMonitoringSettings.Save();

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
                MyUserMonitoringSettings.settings.UsedInChart[1, Index] = !MyUserMonitoringSettings.settings.UsedInChart[1, Index];
                MyUserMonitoringSettings.Save();

                SetListBox(true, !Params.AddButton);

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
                    MyUserMonitoringSettings.settings.UsedInChart[1, Index] = !MyUserMonitoringSettings.settings.UsedInChart[1, Index];
                    MyUserMonitoringSettings.Save();

                    SetListBox(true, !Params.AddButton);

                    lbSignalSelect_Chart_2.Visible = false;

                    Init_Graphs();
                }
            }
            else if (e.KeyChar == 0x1B)
            {
                lbSignalSelect_Chart_2.Visible = false;
            }
        }

        private void btnStartStopMonitoring_Click(object sender, EventArgs e)
        {
            switch (Params.MonitoringState)
            {
                case MONITORING_STATE_T.STOPPED:
                    Params.MonitoringState = MONITORING_STATE_T.RUNNING;
                    btnStartStopMonitoring.Text = "Stop";
                    btnPauseMonitoring.Visible = true;
                    MonitoringTime.Restart();

                    Init_Graphs();
                    break;

                case MONITORING_STATE_T.RUNNING:
                case MONITORING_STATE_T.PAUSED:
                    Params.MonitoringState = MONITORING_STATE_T.STOPPED;
                    btnStartStopMonitoring.Text = "Start";
                    btnPauseMonitoring.Text = "Pause";
                    btnPauseMonitoring.Visible = false;
                    MonitoringTime.Stop();
                    break;

                default:
                    break;
            }
        }
        private void btnPauseMonitoring_Click(object sender, EventArgs e)
        {
            if (Params.MonitoringState == MONITORING_STATE_T.RUNNING)
            {
                btnPauseMonitoring.Text = "Fortsetzen";
                Params.MonitoringState = MONITORING_STATE_T.PAUSED;
            }
            else if (Params.MonitoringState == MONITORING_STATE_T.PAUSED)
            {
                btnPauseMonitoring.Text = "Pause";
                //btnPauseMonitoring.Visible = false;
                Params.MonitoringState = MONITORING_STATE_T.RUNNING;
            }
        }
        private void RelayCardUserMonitoring_Load(object sender, EventArgs e)
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

                MyUserMonitoringSettings.settings.UserMonitoringTimeBase_ms[Index] = Timebase.TimeBase_ms;

                SetTimebase(Index, Timebase);

                MyUserMonitoringSettings.Save();
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

                MyUserMonitoringSettings.settings.UserMonitoringTimeBase_ms[Index] = Timebase.TimeBase_ms;

                SetTimebase(Index, Timebase);

                MyUserMonitoringSettings.Save();
            }
        }
   }
    static class Constants
    {
        public const uint INITCODE = 0x55AA55AB;
        public const string DATAPATH = "..\\..\\Data\\";
    }
}
