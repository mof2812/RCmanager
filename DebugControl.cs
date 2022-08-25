using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RCmanager
{
    public partial class DebugControl : UserControl
    {
        #region Definitions
        public enum DEBUGCONTROL_DATADESCRIPTION_T
        {
            RECEIVE = 0,
            SEND,
            INFO,
            WARNING,
            ERROR
        }
        public struct COLORS_T
        {
            public Color InfoBG;
            public Color InfoFG;
            public Color WarningBG;
            public Color WarningFG;
            public Color ErrorBG;
            public Color ErrorFG;
            public Color SendBG;
            public Color SendFG;
            public Color ReceiveBG;
            public Color ReceiveFG;
        }
        public struct PARAMS_T
        {
            public int ListBoxWidth;
        }
        #endregion
        #region Members
        private FileStream DebugStream;
        private StreamWriter DebugStreamWriter;
        private FileStream DebugStream_Filtered;
        private StreamWriter DebugStreamWriter_Filtered;
        private COLORS_T Colors;
        private PARAMS_T Params;
        private bool InitDone = false;
        private bool AddNext = false;
        private DateTime StartTime = DateTime.Now;
        private bool DebugOn;
        private bool RecOn = false;
        #endregion

        public DebugControl()
        {
            InitializeComponent();

            Init();
        }
        public void Add(String Text)
        {
            Add(Text, DEBUGCONTROL_DATADESCRIPTION_T.INFO);
        }
        public void Add(String Text, DEBUGCONTROL_DATADESCRIPTION_T Description)
        {
            TimeSpan TimeStamp;
            TimeStamp = DateTime.Now - StartTime;
            bool StringFound = Text.Contains(tbDebugFilter.Text);

            if (((DebugOn == true) && (StringFound == true)) || (AddNext == true))
            {
                if (StringFound == true)
                {
                    if (cbAddNext.Checked == true)
                    {
                        AddNext = true;
                    }
                }
                else
                {
                    AddNext = false;
                }

                ListViewItem LVItem = new ListViewItem($"{TimeStamp.Hours}{":"}{TimeStamp.Minutes}{":"}{TimeStamp.Seconds}{"."}{TimeStamp.Milliseconds}");
                if (RecOn == true)
                {
                    ulong Timestamp = (ulong)TimeStamp.Milliseconds + 1000 * (ulong)TimeStamp.Seconds + 60000 * (ulong)TimeStamp.Minutes + 3600000 * (ulong)TimeStamp.Hours;

                    DebugStreamWriter.Write($"{TimeStamp.Hours}{":"}{TimeStamp.Minutes}{":"}{TimeStamp.Seconds}{"."}{TimeStamp.Milliseconds};");
                    DebugStreamWriter.Write(Timestamp.ToString());
                    DebugStreamWriter.Write(";");

                    DebugStreamWriter_Filtered.Write($"{TimeStamp.Hours}{":"}{TimeStamp.Minutes}{":"}{TimeStamp.Seconds}{"."}{TimeStamp.Milliseconds};");
                    DebugStreamWriter_Filtered.Write(Timestamp.ToString());
                    DebugStreamWriter_Filtered.Write(";");
                }

                switch (Description)
                {
                    case DEBUGCONTROL_DATADESCRIPTION_T.RECEIVE:
                        LVItem.SubItems.Add(" <<< ");
                        LVItem.BackColor = Colors.ReceiveBG;
                        LVItem.ForeColor = Colors.ReceiveFG;
                        if (RecOn == true)
                        {
                            DebugStreamWriter.Write("receive;");
                            DebugStreamWriter_Filtered.Write("receive;");
                        }
                        break;

                    case DEBUGCONTROL_DATADESCRIPTION_T.SEND:
                        LVItem.SubItems.Add(" >>> ");
                        LVItem.BackColor = Colors.SendBG;
                        LVItem.ForeColor = Colors.SendFG;
                        if (RecOn == true)
                        {
                            DebugStreamWriter.Write("send;");
                            DebugStreamWriter_Filtered.Write("send;");
                        }
                        break;

                    case DEBUGCONTROL_DATADESCRIPTION_T.ERROR:
                        LVItem.SubItems.Add("");
                        LVItem.BackColor = Colors.ErrorBG;
                        LVItem.ForeColor = Colors.ErrorFG;
                        if (RecOn == true)
                        {
                            DebugStreamWriter.Write("Error;");
                            DebugStreamWriter_Filtered.Write("Error;");
                        }
                        break;

                    case DEBUGCONTROL_DATADESCRIPTION_T.WARNING:
                        LVItem.SubItems.Add("");
                        LVItem.BackColor = Colors.WarningBG;
                        LVItem.ForeColor = Colors.WarningFG;
                        if (RecOn == true)
                        {
                            DebugStreamWriter.Write("Warning;");
                            DebugStreamWriter_Filtered.Write("Warning;");
                        }
                        break;

                    case DEBUGCONTROL_DATADESCRIPTION_T.INFO:
                        LVItem.SubItems.Add("");
                        LVItem.BackColor = Colors.InfoBG;
                        LVItem.ForeColor = Colors.InfoFG;
                        if (RecOn == true)
                        {
                            DebugStreamWriter.Write("Info;");
                            DebugStreamWriter_Filtered.Write("Info;");
                        }
                        break;

                    default:
                        break;
                }
                LVItem.SubItems.Add(Text);
                lvDebug.Items.Add(LVItem);
                lvDebug.Items[lvDebug.Items.Count - 1].EnsureVisible();

                if (RecOn == true)
                {
                    if (Description == DEBUGCONTROL_DATADESCRIPTION_T.RECEIVE)
                    {
                        Text = Text.Replace(".", ",");
                    }
                    DebugStreamWriter.Write(Text);
                    DebugStreamWriter.WriteLine();

                    DebugStreamWriter_Filtered.Write(Text);
                    DebugStreamWriter_Filtered.WriteLine();
                }
            }
            else if (DebugOn == true)
            {
                if (RecOn == true)
                {
                    ulong Timestamp = (ulong)TimeStamp.Milliseconds + 1000 * (ulong)TimeStamp.Seconds + 60000 * (ulong)TimeStamp.Minutes + 3600000 * (ulong)TimeStamp.Hours;

                    DebugStreamWriter.Write($"{TimeStamp.Hours}{":"}{TimeStamp.Minutes}{":"}{TimeStamp.Seconds}{"."}{TimeStamp.Milliseconds};");
                    DebugStreamWriter.Write(Timestamp.ToString());
                    DebugStreamWriter.Write(";");

                    switch (Description)
                    {
                        case DEBUGCONTROL_DATADESCRIPTION_T.RECEIVE:
                            DebugStreamWriter.Write("receive;");
                            break;

                        case DEBUGCONTROL_DATADESCRIPTION_T.SEND:
                            DebugStreamWriter.Write("send;");
                            break;

                        case DEBUGCONTROL_DATADESCRIPTION_T.ERROR:
                            DebugStreamWriter.Write("Error;");
                            break;

                        case DEBUGCONTROL_DATADESCRIPTION_T.WARNING:
                            DebugStreamWriter.Write("Warning;");
                            break;

                        case DEBUGCONTROL_DATADESCRIPTION_T.INFO:
                            DebugStreamWriter.Write("Info;");
                            break;

                        default:
                            break;
                    }
                    if (Description == DEBUGCONTROL_DATADESCRIPTION_T.RECEIVE)
                    {
                        Text = Text.Replace(".", ",");
                    }
                    DebugStreamWriter.Write(Text);
                    DebugStreamWriter.WriteLine();
                }
            }
        }
        public void Add(String Text, String Text2)
        {
            Text = Text + " , " + Text2;
            Add(Text, DEBUGCONTROL_DATADESCRIPTION_T.INFO);
        }
        public void AddBreak()
        {
            TimeSpan TimeStamp;
            TimeStamp = DateTime.Now - StartTime;

            ListViewItem LVItem = new ListViewItem($"{TimeStamp.Hours}{":"}{TimeStamp.Minutes}{":"}{TimeStamp.Seconds}{"."}{TimeStamp.Milliseconds}");
            LVItem.SubItems.Add("");
            LVItem.BackColor = Color.Red;
            LVItem.SubItems.Add("---Pause---");
            lvDebug.Items.Add(LVItem);
            lvDebug.Items[lvDebug.Items.Count - 1].EnsureVisible();
        }
        public void AddReceiveFrame(String Text)
        {
            if (Convert.ToChar(Text.Substring(0, 1)) == 0x02)
            {
                Text = Text.Remove(0, 1);
            }
            if (Convert.ToChar(Text.Substring(Text.Length - 1, 1)) == 0x03)
            {
                Text = Text.Remove(Text.Length - 1, 1);
            }

            if (Properties.DebugCtrl.Default.DebugAddFrameDelimiter == true)
            {
                Text = $"<STX>{Text}<ETX>";
            }

            Add(Text, DEBUGCONTROL_DATADESCRIPTION_T.RECEIVE);
        }
        public void AddSendFrame(String Text)
        {
            if (Properties.DebugCtrl.Default.DebugAddFrameDelimiter == true)
            {
                Text = $"<STX>{Text}<ETX>";
            }

            Add(Text, DEBUGCONTROL_DATADESCRIPTION_T.SEND);
        }
        public void Debug_AddSendFrame(String Text, bool Success)
        {
            if (Success == true)
            {
                if (Properties.DebugCtrl.Default.DebugAddFrameDelimiter == true)
                {
                    Text = $"<STX>{Text}<ETX>";
                }

                Add(Text, DEBUGCONTROL_DATADESCRIPTION_T.SEND);
            }
            else
            {
                if (Properties.DebugCtrl.Default.DebugAddFrameDelimiter == true)
                {
                    Text = $"<STX>{Text}<ETX> - Error";
                }

                Add(Text, DEBUGCONTROL_DATADESCRIPTION_T.ERROR);
            }

        }
        private void CloseStreams()
        {
            if (DebugStream != null)
            {
                DebugStream.Close();
            }
            if (DebugStream_Filtered != null)
            {
                DebugStream_Filtered.Close();
            }
        }
        private void ColorsToDefault()
        {
            Init_Colors(true);
        }
        public void EnableControls(bool Enable)
        {
            lvDebug.Visible = Enable;
            lblDebugFilter.Visible = Enable;
            tbDebugFilter.Visible = Enable;
            cbAddNext.Visible = Enable;
            cbRecDebug.Visible = Enable;
            cbDebugOnOff.Visible = Enable;
            btnClearDebug.Visible = Enable;
            cbAddSTX_ETX.Visible = Enable;
            grpDebugging.Visible = Enable;
        }
        public Color ErrorBackGround
        {
            get
            {
                return Colors.ErrorBG;
            }
            set
            {
                Colors.ErrorBG = value;
                Properties.DebugCtrl.Default.ColorErrorBG = Colors.ErrorBG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        public Color ErrorForeGround
        {
            get
            {
                return Colors.ErrorFG;
            }
            set
            {
                Colors.ErrorFG = value;
                Properties.DebugCtrl.Default.ColorErrorFG = Colors.ErrorFG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        public Color InfoBackGround
        {
            get
            {
                return Colors.InfoBG;
            }
            set
            {
                Colors.InfoBG = value;
                Properties.DebugCtrl.Default.ColorInfoBG = Colors.InfoBG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        public Color InfoForeGround
        {
            get
            {
                return Colors.InfoFG;
            }
            set
            {
                Colors.InfoFG = value;
                Properties.DebugCtrl.Default.ColorInfoFG = Colors.InfoFG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        private void Init()
        {
            if (Properties.DebugCtrl.Default.InitCode != 0x55AA55AA)
            {
                Properties.DebugCtrl.Default.InitCode = 0x55AA55AA;
                SetDefault();
            }
            else
            {
                Init_Colors(false);
                Init_Listbox();
            }


            lvDebug.Items.Clear();
            lvDebug.Columns.Clear();
            lvDebug.Columns.Add("Zeit", 80);
            lvDebug.Columns.Add("Dir.");
            lvDebug.Columns.Add("Text", 770);

            tbDebugFilter.Text = Properties.DebugCtrl.Default.DebugFilter;

            DebugOn = Properties.DebugCtrl.Default.DebugOn;

            SetChecked(ref cbAddNext, Properties.DebugCtrl.Default.DebugFilterAddNext, "+ nächstes Frame", "- nächstes Frame");
            SetChecked(ref cbAddSTX_ETX, Properties.DebugCtrl.Default.DebugAddFrameDelimiter, "+ <STX><ETX>", "-- <STX><ETX>");
            SetChecked(ref cbDebugOnOff, Properties.DebugCtrl.Default.DebugOn, "Debug ein", "Debug aus");

            cbRecDebug.Checked = false;
            SetChecked(ref cbRecDebug, cbRecDebug.Checked, "Aufnahme ein", "Aufnahme aus");

            InitDone = true;
        }
        private void Init_Colors(bool SetDefault)
        {
            if (SetDefault)
            {
                Colors.InfoBG = Properties.DebugCtrl.Default.ColorInfoBG = Color.Blue;
                Colors.InfoFG = Properties.DebugCtrl.Default.ColorInfoFG = Color.White;

                Colors.WarningBG = Properties.DebugCtrl.Default.ColorWarningBG = Color.FromArgb(192, 192, 255);
                Colors.WarningFG = Properties.DebugCtrl.Default.ColorWarningFG = Color.White;

                Colors.ErrorBG = Properties.DebugCtrl.Default.ColorErrorBG = Color.Red;
                Colors.ErrorFG = Properties.DebugCtrl.Default.ColorErrorFG = Color.White;

                Colors.SendBG = Properties.DebugCtrl.Default.ColorSendBG = Color.FromArgb(255, 216, 0);
                Colors.SendFG = Properties.DebugCtrl.Default.ColorSendFG = Color.Black;

                Colors.ReceiveBG = Properties.DebugCtrl.Default.ColorReceiveBG = Color.FromArgb(255, 106, 0);
                Colors.ReceiveFG = Properties.DebugCtrl.Default.ColorReceiveFG = Color.Black;
            }
            else
            {
                Colors.InfoBG = Properties.DebugCtrl.Default.ColorInfoBG;
                Colors.InfoFG = Properties.DebugCtrl.Default.ColorInfoFG;

                Colors.WarningBG = Properties.DebugCtrl.Default.ColorWarningBG;
                Colors.WarningFG = Properties.DebugCtrl.Default.ColorWarningFG;

                Colors.ErrorBG = Properties.DebugCtrl.Default.ColorErrorBG;
                Colors.ErrorFG = Properties.DebugCtrl.Default.ColorErrorFG;

                Colors.SendBG = Properties.DebugCtrl.Default.ColorSendBG;
                Colors.SendFG = Properties.DebugCtrl.Default.ColorSendFG;

                Colors.ReceiveBG = Properties.DebugCtrl.Default.ColorReceiveBG;
                Colors.ReceiveFG = Properties.DebugCtrl.Default.ColorReceiveFG;
            }
        }
        private void Init_Listbox()
        {
            lvDebug.Size = new Size(Params.ListBoxWidth = Properties.DebugCtrl.Default.ListBoxWidth, lvDebug.Size.Height); 
        }
        public int ListBoxSize
        {
            get
            {
                return Params.ListBoxWidth;
            }
            set
            {
                Properties.DebugCtrl.Default.ListBoxWidth = Params.ListBoxWidth = value;
                Properties.DebugCtrl.Default.Save();


                lvDebug.Size = new Size(Properties.DebugCtrl.Default.ListBoxWidth, lvDebug.Height);

                grpDebugging.Location = new Point(lvDebug.Location.X + lvDebug.Size.Width + 10, grpDebugging.Location.Y);
            }
        }
        public bool OpenStream(bool ReOpen)
        {
            bool Error;
            FileMode Mode;
            string ProjectName;
            int Pos;

            DateTime dt = new DateTime();

            Error = false;
            ProjectName = "";

            if (RecOn == true)
            {
                if (ReOpen == true)
                {
                    Mode = FileMode.Append;
                }
                else
                {
                    Mode = FileMode.Create;
                }

                if (Properties.DebugCtrl.Default.AddTimeStamp == true)
                {
                    dt = DateTime.Now;
                    ProjectName += dt.ToString("yyyyMMdd_hhmm") + " - ";
                }
                if (Properties.DebugCtrl.Default.AddProjectsName == true)
                {
                    Pos = Properties.DebugCtrl.Default.ProjectFile.LastIndexOf(".");
                    ProjectName += Properties.DebugCtrl.Default.ProjectFile.Substring(0, Pos - 1) + " - ";
                }
                if (Properties.DebugCtrl.Default.Application.Length > 0)
                {
                    ProjectName += Properties.DebugCtrl.Default.Application + " - ";
                }

                try
                {
                    DebugStream = new FileStream("..\\..\\Debug\\" + ProjectName + "Debug.csv", Mode);
                    DebugStreamWriter = new StreamWriter(DebugStream);
                    DebugStream_Filtered = new FileStream("..\\..\\Debug\\" + ProjectName + "Debug_Filtered.csv", Mode);
                    DebugStreamWriter_Filtered = new StreamWriter(DebugStream_Filtered);
                    DebugStreamWriter.Write("Zeitstempel;Millisekunden;Spezifizierer;Bemerkung\r\n");
                    DebugStreamWriter_Filtered.Write("Zeitstempel;Millisekunden;Spezifizierer;Bemerkung\r\n");
                }
                catch
                {
                    MessageBox.Show("Mitschnitt nicht möglich - Bitte ein Verzeichnis 'Debug' im Arbeitsverzeichnis anlegen", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Error = true;
                }
            }
            else
            {
                Error = true;
            }
            return (Error);
        }
        public Color ReceiveBackGround
        {
            get
            {
                return Colors.ReceiveBG;
            }
            set
            {
                Colors.ReceiveBG = value;
                Properties.DebugCtrl.Default.ColorReceiveBG = Colors.ReceiveBG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        public Color ReceiveForeGround
        {
            get
            {
                return Colors.ReceiveFG;
            }
            set
            {
                Colors.ReceiveFG = value;
                Properties.DebugCtrl.Default.ColorReceiveFG = Colors.ReceiveFG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        public Color SendBackGround
        {
            get
            {
                return Colors.SendBG;
            }
            set
            {
                Colors.SendBG = value;
                Properties.DebugCtrl.Default.ColorSendBG = Colors.SendBG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        public Color SendForeGround
        {
            get
            {
                return Colors.SendFG;
            }
            set
            {
                Colors.SendFG = value;
                Properties.DebugCtrl.Default.ColorSendFG = Colors.SendFG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        public string SetApplication
        {
            get
            {
                return Properties.DebugCtrl.Default.Application;
            }
            set
            {
                Properties.DebugCtrl.Default.Application = value;
                Properties.DebugCtrl.Default.Save();
            }
        }
        private void SetChecked(ref CheckBox CheckBoxRef, bool Check, String TextChecked, String TextUnchecked)
        {
            CheckBoxRef.Checked = Check;

            if (Check == true)
            {
                CheckBoxRef.Text = TextChecked;
                CheckBoxRef.BackColor = Color.Green;
            }
            else
            {
                CheckBoxRef.Text = TextUnchecked;
                CheckBoxRef.BackColor = Color.Red;
            }
        }
        public void SetDefault()
        {
            ColorsToDefault();

            if (Properties.DebugCtrl.Default.ListBoxWidthDefault == 0)
            {
                Properties.DebugCtrl.Default.ListBoxWidthDefault = Properties.DebugCtrl.Default.ListBoxWidth = Params.ListBoxWidth = lvDebug.Size.Width;
            }
            else
            {
                Params.ListBoxWidth = Properties.DebugCtrl.Default.ListBoxWidth = Properties.DebugCtrl.Default.ListBoxWidthDefault;
                
            }
            Properties.DebugCtrl.Default.Save();

            lvDebug.Size = new Size(Params.ListBoxWidth, lvDebug.Size.Height);
        }
        public Color WarningBackGround
        {
            get
            {
                return Colors.WarningBG;
            }
            set
            {
                Colors.WarningBG = value;
                Properties.DebugCtrl.Default.ColorWarningBG = Colors.WarningBG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        public Color WarningForeGround
        {
            get
            {
                return Colors.WarningFG;
            }
            set
            {
                Colors.WarningFG = value;
                Properties.DebugCtrl.Default.ColorWarningFG = Colors.WarningFG;
                Properties.DebugCtrl.Default.Save();
            }
        }
        #region Events
        private void cbDebugOnOff_CheckedChanged(object sender, EventArgs e)
        {
            if (InitDone == true)
            {
                if (Properties.DebugCtrl.Default.DebugOn != cbDebugOnOff.Checked)
                {
                    Properties.DebugCtrl.Default.DebugOn = cbDebugOnOff.Checked;
                    Properties.DebugCtrl.Default.Save();
                }
                DebugOn = cbDebugOnOff.Checked;
                SetChecked(ref cbDebugOnOff, Properties.DebugCtrl.Default.DebugOn, "Debug ein", "Debug aus");
            }
        }
        private void tbDebugFilter_TextChanged(object sender, EventArgs e)
        {
            if (Properties.DebugCtrl.Default.DebugFilter != tbDebugFilter.Text)
            {
                Properties.DebugCtrl.Default.DebugFilter = tbDebugFilter.Text;
                Properties.DebugCtrl.Default.Save();

//              lvDebug.Clear();
            }
        }
        private void cbAddSTX_ETX_CheckedChanged(object sender, EventArgs e)
        {
            if (Properties.DebugCtrl.Default.DebugAddFrameDelimiter != cbAddSTX_ETX.Checked)
            {
                Properties.DebugCtrl.Default.DebugAddFrameDelimiter = cbAddSTX_ETX.Checked;
                Properties.DebugCtrl.Default.Save();
            }
            SetChecked(ref cbAddSTX_ETX, Properties.DebugCtrl.Default.DebugAddFrameDelimiter, "+ <STX><ETX>", "-- <STX><ETX>");
        }
        private void cbAddNext_CheckedChanged(object sender, EventArgs e)
        {
            if (Properties.DebugCtrl.Default.DebugFilterAddNext != cbAddNext.Checked)
            {
                Properties.DebugCtrl.Default.DebugFilterAddNext = cbAddNext.Checked;
                Properties.DebugCtrl.Default.Save();
            }
            SetChecked(ref cbAddNext, Properties.DebugCtrl.Default.DebugFilterAddNext, "+ nächstes Frame", "- nächstes Frame");
        }
        private void cbRecDebug_CheckedChanged(object sender, EventArgs e)
        {
            if (InitDone == true)
            {
                if (cbRecDebug.Checked == true)
                {
                    try
                    {
                        DebugStream = new FileStream("..\\..\\Debug\\Debug.csv", FileMode.Create);
                        DebugStreamWriter = new StreamWriter(DebugStream);
                        DebugStream_Filtered = new FileStream("..\\..\\Debug\\Debug_Filtered.csv", FileMode.Create);
                        DebugStreamWriter_Filtered = new StreamWriter(DebugStream_Filtered);
                        DebugStreamWriter.Write("Zeitstempel;Millisekunden;Spezifizierer;Bemerkung\r\n");
                        DebugStreamWriter_Filtered.Write("Zeitstempel;Millisekunden;Spezifizierer;Bemerkung\r\n");
                    }
                    catch
                    {
                        MessageBox.Show("Fehler beim Öffnen der Debugdatei. Bitte überprüfen sie das Vorhandenseins des 'Debug' Verzeichnisses", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    CloseStreams();
                }
                SetChecked(ref cbRecDebug, cbRecDebug.Checked, "Aufnahme ein", "Aufnahme aus");
                RecOn = cbRecDebug.Checked;
            }
        }
        private void btnClearDebug_Click(object sender, EventArgs e)
        {
            lvDebug.Items.Clear();
        }
        #endregion
    }
}
