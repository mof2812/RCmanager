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

namespace SerialCommunication
{
    public enum CHECKSUM_TYPE_T
    {
        NONE = 0,
        CRC8,
        CRC16,
        CRC32,
        CRC64,
        XOR,
    }
    public enum SETUP_FLAG_T
    {
        SERIAL_PORT = 0x00000001,
        BAUDRATE = 0x00000002,
        DATABITS = 0x00000004,
        STOPBITS = 0x00000008,
    }
    public struct PARAMS_T
    {
        public string Bitmap;
        public uint SetupFlags;
        public bool SmallVersion;
    }
    public struct SERIAL_PARAMS_T
    {
        public string SerialPort;
        public int Baudrate;
        public byte Databits;
        public StopBits Stopbits;
    }
    public struct SETTINGS_T
    {
        public SERIAL_PARAMS_T SerialDefaults;
    }
    public partial class SetupSerialDlg : Form
    {
        private PARAMS_T Params;
        private const uint SetupReadyFlags = (uint)(SETUP_FLAG_T.SERIAL_PORT | SETUP_FLAG_T.BAUDRATE | SETUP_FLAG_T.DATABITS | SETUP_FLAG_T.STOPBITS);
        public SetupSerialDlg()
        {
            InitializeComponent();
        }
        public string Bitmap
        {
            get
            {
                return Params.Bitmap;
            }
            set
            {
                Params.Bitmap = value;
                try
                {
                    pbBitmap.Image = Image.FromFile($"..\\..\\res\\{value}");
                }
                catch (Exception ex)
                {

                }
            }
        }
        private int ChecksumTypeToInt(CHECKSUM_TYPE_T Type)
        {
            return (int)Type;
        }
        private CHECKSUM_TYPE_T ChecksumTypeFromInt(int ChecksumTypeAsInt)
        {
            return (CHECKSUM_TYPE_T)ChecksumTypeAsInt;
        }
        private string ChecksumTypeToText(CHECKSUM_TYPE_T Type)
        {
            string ChecksumTypeAsText;

            ChecksumTypeAsText = "NONE";

            switch(Type)
            {
                case CHECKSUM_TYPE_T.CRC8:
                    ChecksumTypeAsText = "CRC8";
                    break;

                case CHECKSUM_TYPE_T.CRC16:
                    ChecksumTypeAsText = "CRC16";
                    break;

                case CHECKSUM_TYPE_T.CRC32:
                    ChecksumTypeAsText = "CRC32";
                    break;

                case CHECKSUM_TYPE_T.CRC64:
                    ChecksumTypeAsText = "CRC64";
                    break;

                case CHECKSUM_TYPE_T.XOR:
                    ChecksumTypeAsText = "XOR";
                    break;

                default:
                    break;
            }

            return ChecksumTypeAsText;
        }
        private CHECKSUM_TYPE_T ChecksumTypeFromText(string ChecksumTypeAsText)
        {
            CHECKSUM_TYPE_T ChecksumType;

            ChecksumType = CHECKSUM_TYPE_T.NONE;

            switch (ChecksumTypeAsText)
            {
                case "CRC8":
                    ChecksumType = CHECKSUM_TYPE_T.CRC8;
                    break;

                case "CRC16":
                    ChecksumType = CHECKSUM_TYPE_T.CRC16;
                    break;

                case "CRC32":
                    ChecksumType = CHECKSUM_TYPE_T.CRC32;
                    break;

                case "CRC64":
                    ChecksumType = CHECKSUM_TYPE_T.CRC64;
                    break;

                case "XOR":
                    ChecksumType = CHECKSUM_TYPE_T.XOR;
                    break;

                default:
                    break;
            }

            return ChecksumType;
        }
        public int DefaultBaudrate
        {
            get
            {
                return RCmanager.Properties.SerialSettings.Default.Baudrate;
            }
            set
            {
                if (RCmanager.Properties.SerialSettings.Default.InitCode != Constants.INITCODE)
                {
                    if (value != 0)
                    {
                        RCmanager.Properties.SerialSettings.Default.Baudrate = value;
                        RCmanager.Properties.SerialSettings.Default.Save();
                    }
                }
                Params.SetupFlags |= (uint)SETUP_FLAG_T.BAUDRATE;
            }
        }
        public byte DefaultDatabits
        {
            get
            {
                return RCmanager.Properties.SerialSettings.Default.Databits;
            }
            set
            {
                if (RCmanager.Properties.SerialSettings.Default.InitCode != Constants.INITCODE)
                {
                    if (value != 0)
                    {
                        RCmanager.Properties.SerialSettings.Default.Databits = value;
                        RCmanager.Properties.SerialSettings.Default.Save();
                    }
                }
                Params.SetupFlags |= (uint)SETUP_FLAG_T.DATABITS;
            }
        }
        public string DefaultSerialPort
        {
            get
            {
                return RCmanager.Properties.SerialSettings.Default.SerialPort;
            }
            set
            {
                if (RCmanager.Properties.SerialSettings.Default.InitCode != Constants.INITCODE)
                {
                    if (value.Length != 0)
                    {
                        RCmanager.Properties.SerialSettings.Default.SerialPort = value;
                        RCmanager.Properties.SerialSettings.Default.Save();
                    }
                }
                Params.SetupFlags |= (uint)SETUP_FLAG_T.SERIAL_PORT;
            }
        }
        public StopBits DefaultStopbits
        {
            get
            {
                return RCmanager.Properties.SerialSettings.Default.Stopbits;
            }
            set
            {
                if (RCmanager.Properties.SerialSettings.Default.InitCode != Constants.INITCODE)
                {
                    if ((value >= StopBits.None) && (value <= StopBits.OnePointFive))
                    {
                        RCmanager.Properties.SerialSettings.Default.Stopbits = value;
                        RCmanager.Properties.SerialSettings.Default.Save();
                    }
                }
                Params.SetupFlags |= (uint)SETUP_FLAG_T.STOPBITS;
            }
        }
        public void Init()
        {
            Params.SetupFlags = 0;

            //if (byDefault)
            //{
            //    Init_Defaults();
            //}
            Init_Ports();
            Init_Baudrate();
            Init_Databits();
            Init_Stopbits();
            Init_Timeout();
            Init_Replies();
            Init_FrameDelimiter();
            Init_Checksum();
            Init_TaskTiming();
            Init_Apperance();
        }
        //private void Init_Defaults()
        //{
        //    RCmanager.Properties.SerialSettings.Default.TimeoutGetData_ms = 500;
        //    RCmanager.Properties.SerialSettings.Default.MaxReplies = 3;
        //}
        private bool Init_Apperance()
        {
            bool Error;
            int Width;
            double Ratio;

            Error = true;
            Width = this.Width;

            Ratio = Convert.ToDouble(Width) / 1139.0;

            grpProtocol.Visible = !Params.SmallVersion;
            grpMisc.Visible = !Params.SmallVersion;

            if (Params.SmallVersion)
            {
                pbBitmap.Left = grpProtocol.Left;
                btnCancel.Left = grpProtocol.Left;
                btnOK.Left = btnCancel.Left + btnCancel.Width + 6;
                this.Width = btnOK.Right + 30;
            }
            return Error;
        }
        private bool Init_Baudrate()
        {
            bool Error;

            Error = true;

            cmbBaudrate.Items.Clear();

            cmbBaudrate.Items.Add("300");
            cmbBaudrate.Items.Add("600");
            cmbBaudrate.Items.Add("1200");
            cmbBaudrate.Items.Add("2400");
            cmbBaudrate.Items.Add("4800");
            cmbBaudrate.Items.Add("9600");
            cmbBaudrate.Items.Add("19200");
            cmbBaudrate.Items.Add("38400");
            cmbBaudrate.Items.Add("56000");
            cmbBaudrate.Items.Add("57600");
            cmbBaudrate.Items.Add("115200");

            for (int Index = 0; Index < cmbBaudrate.Items.Count; Index++)
            {
                if (Convert.ToInt32(cmbBaudrate.Items[Index]) == RCmanager.Properties.SerialSettings.Default.Baudrate)
                {
                    cmbBaudrate.SelectedIndex = Index;
                    Error = false;
                    break;
                }
            }

            if (Error == true)
            {
                // Set default value
                cmbBaudrate.SelectedIndex = 5;
                RCmanager.Properties.SerialSettings.Default.Baudrate = Convert.ToInt32(cmbBaudrate.SelectedItem);
            }

            return Error;
        }
        private bool Init_Checksum()
        {
            bool Error;
            CHECKSUM_TYPE_T Test;

            Error = true;

            cbUseChecksum.Checked = RCmanager.Properties.SerialSettings.Default.UseChecksum;
            lblChecksumType.Enabled = cbUseChecksum.Checked;
            cmbChecksumType.Enabled = cbUseChecksum.Checked;

            cmbChecksumType.Items.Clear();

            cmbChecksumType.Items.Add("CRC8");
            cmbChecksumType.Items.Add("CRC16");
            cmbChecksumType.Items.Add("CRC32");
            cmbChecksumType.Items.Add("XOR");

            cmbChecksumType.Text = ChecksumTypeToText((CHECKSUM_TYPE_T)RCmanager.Properties.SerialSettings.Default.ChecksumType);

            return Error;
        }
        private bool Init_Databits()
        {
            bool Error;

            Error = true;

            cmbDatabits.Items.Clear();

            cmbDatabits.Items.Add("5");
            cmbDatabits.Items.Add("6");
            cmbDatabits.Items.Add("7");
            cmbDatabits.Items.Add("8");

            for (int Index = 0; Index < cmbDatabits.Items.Count; Index++)
            {
                if (Convert.ToInt32(cmbDatabits.Items[Index]) == RCmanager.Properties.SerialSettings.Default.Databits)
                {
                    cmbDatabits.SelectedIndex = Index;
                    Error = false;
                    break;
                }
            }

            return Error;
        }
        private bool Init_FrameDelimiter()
        {
            bool Error;

            Error = true;

            cbUseDelimiters.Checked = RCmanager.Properties.SerialSettings.Default.UseDelimiters;
            lblStartDelimiter.Enabled = cbUseDelimiters.Checked;
            txtStartDelimiter.Enabled = cbUseDelimiters.Checked;
            txtStartDelimiter.Text = $"{RCmanager.Properties.SerialSettings.Default.StartDelimiter}";
            lblEndDelimiter.Enabled = cbUseDelimiters.Checked;
            txtEndDelimiter.Enabled = cbUseDelimiters.Checked;
            txtEndDelimiter.Text = $"{RCmanager.Properties.SerialSettings.Default.EndDelimiter}";

            return Error;
        }
        private bool Init_Ports()
        {
            bool Error;

            Error = false;

            cmbPort.Items.Clear();

            string[] Ports = SerialPort.GetPortNames();

            foreach (string Port in Ports)
            {
                cmbPort.Items.Add(Port);

                if (Port == RCmanager.Properties.SerialSettings.Default.SerialPort)
                {
                    cmbPort.Text = Port;
                }
            }

            return Error;
        }
        private bool Init_Replies()
        {
            bool Error;

            Error = false;

            txtReplies.Text = RCmanager.Properties.SerialSettings.Default.MaxReplies.ToString();

            return Error;
        }
        private bool Init_Stopbits()
        {
            bool Error;

            Error = false;

            cmbStopbits.Items.Clear();

            cmbStopbits.Items.Add("Keins");
            cmbStopbits.Items.Add("1");
            cmbStopbits.Items.Add("1.5");
            cmbStopbits.Items.Add("2");

            cmbStopbits.Text = StopbitsToText(RCmanager.Properties.SerialSettings.Default.Stopbits);

            return Error;
        }
        private bool Init_TaskTiming()
        {
            bool Error;

            Error = false;

            txtTiming.Text = RCmanager.Properties.SerialSettings.Default.TaskTiming_ms.ToString();

            return Error;
        }
        private bool Init_Timeout()
        {
            bool Error;

            Error = false;

            txtReceiveTimeout.Text = RCmanager.Properties.SerialSettings.Default.TimeoutGetData_ms.ToString();

            return Error;
        }
        public DialogResult MessageDlg(Exception ex)
        {
            DialogResult Result;

            Result = DialogResult.OK;

            MessageBox.Show(ex.Message);

            return Result;
        }
        public bool SetupDone
        {
            get
            {
                return Params.SetupFlags == SetupReadyFlags;
            }
        }
        public bool SmallVersion
        {
            get 
            { 
                return Params.SmallVersion; 
            }
            set 
            { 
                Params.SmallVersion = value; 
            }
        }
        private string StopbitsToText(StopBits stopBits)
        {
            string RetVal;

            RetVal = "";

            switch (stopBits)
            {
                case StopBits.One:
                    RetVal = "1";
                    break;

                case StopBits.OnePointFive:
                    RetVal = "1.5";
                    break;

                case StopBits.Two:
                    RetVal = "2";
                    break;

                default:
                    RetVal = "Keins";
                    break;
            }
            return RetVal;
        }
        private StopBits TextToStopbits(string Text)
        {
            StopBits RetVal;

            RetVal = StopBits.None;

            switch (Text)
            {
                case "1":
                    RetVal = StopBits.One;
                    break;

                case "1.5":
                    RetVal = StopBits.OnePointFive;
                    break;

                case "2":
                    RetVal = StopBits.Two;
                    break;

                default:
                    RetVal = StopBits.None;
                    break;
            }
            return RetVal;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            RCmanager.Properties.SerialSettings.Default.SerialPort = cmbPort.Text;
            RCmanager.Properties.SerialSettings.Default.Baudrate = Convert.ToInt32(cmbBaudrate.Text);
            RCmanager.Properties.SerialSettings.Default.Databits = Convert.ToByte(cmbDatabits.Text);
            RCmanager.Properties.SerialSettings.Default.Stopbits = TextToStopbits(cmbStopbits.Text);
            RCmanager.Properties.SerialSettings.Default.TimeoutGetData_ms = Convert.ToInt32(txtReceiveTimeout.Text);
            RCmanager.Properties.SerialSettings.Default.MaxReplies = Convert.ToByte(txtReplies.Text);
            RCmanager.Properties.SerialSettings.Default.UseDelimiters = cbUseDelimiters.Checked;
            RCmanager.Properties.SerialSettings.Default.StartDelimiter = Convert.ToByte(txtStartDelimiter.Text);
            RCmanager.Properties.SerialSettings.Default.EndDelimiter = Convert.ToByte(txtEndDelimiter.Text);
            RCmanager.Properties.SerialSettings.Default.UseChecksum = cbUseChecksum.Checked;
            RCmanager.Properties.SerialSettings.Default.ChecksumType = (int)ChecksumTypeFromText(cmbChecksumType.Text);
            RCmanager.Properties.SerialSettings.Default.TaskTiming_ms = Convert.ToInt32(txtTiming.Text);

            RCmanager.Properties.SerialSettings.Default.Save();

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult=DialogResult.Cancel;
        }

        private void cbUseDelimiters_CheckedChanged(object sender, EventArgs e)
        {
            lblStartDelimiter.Enabled = cbUseDelimiters.Checked;
            txtStartDelimiter.Enabled = cbUseDelimiters.Checked;
            lblEndDelimiter.Enabled = cbUseDelimiters.Checked;
            txtEndDelimiter.Enabled = cbUseDelimiters.Checked;
        }

        private void cbUseChecksum_CheckedChanged(object sender, EventArgs e)
        {
            lblChecksumType.Enabled = cbUseChecksum.Checked;
            cmbChecksumType.Enabled = cbUseChecksum.Checked;
        }

        private void SetupSerialDlg_Load(object sender, EventArgs e)
        {
            Init();
        }
    }
}
