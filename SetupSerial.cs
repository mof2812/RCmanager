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

namespace SetupSerial
{
    public struct SERIAL_SETTINGS_T
    {
        public string SerialPort;
        public int Baudrate;
        public byte DataBits;
        public System.IO.Ports.StopBits StopBits;
        public ulong TimeoutGetData;
        public byte MaxReplies;
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
    }
    enum RETURN_T : uint
    {
        OKAY = 0x00000000,
        ERROR,
        #region Devices
        /* Devices (0x8xxx xxxx) */
        #region Serial port
        /* Serial Port (0x8xxx x0xx) */
        SERIAL_PORT_OPEN = 0x80000000,
        SERIAL_PORT_CLOSE,
        SERIAL_PORT_UNDEF,  /* undefined port */
        SERIAL_PORT_UNDEF_DEVICE,
        SERIAL_PORT_WRITE,
        #endregion
        #endregion
        #region Misc
        #endregion
    }
    public partial class SetupSerial : Form
    {
        private SERIAL_SETTINGS_T Settings;
        public struct CELL_PARAMS_T
        {
            public bool CommunicationOn;
        }

        private CELL_PARAMS_T Params;
        SerialPort Port = new SerialPort();

        public SERIAL_SETTINGS_T SerialSettings
        {
            get
            {
                Settings.SerialPort = SetupSerialSettings.Default.Port;
                Settings.Baudrate = SetupSerialSettings.Default.Baudrate;
                Settings.DataBits = SetupSerialSettings.Default.DataBits;
                Settings.StopBits = SetupSerialSettings.Default.StopBits;
                Settings.TimeoutGetData = SetupSerialSettings.Default.TimeoutGetData;
                Settings.MaxReplies = SetupSerialSettings.Default.MaxReplies;

                return Settings;
            }
            set
            {
                SetupSerialSettings.Default.Port = value.SerialPort;
                SetupSerialSettings.Default.Baudrate = value.Baudrate;
                SetupSerialSettings.Default.DataBits = value.DataBits;
                SetupSerialSettings.Default.StopBits = value.StopBits;
                SetupSerialSettings.Default.TimeoutGetData = value.TimeoutGetData;
                SetupSerialSettings.Default.MaxReplies = value.MaxReplies;

                SetupSerialSettings.Default.Save();
            }
        }

        #region Methods
        public SetupSerial(SerialPort serialPort)
        {
            Port = serialPort;
            InitializeComponent();
        }
        private RETURN_T ClosePort()
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.OKAY;

            try
            {
                Port.Close();
                Params.CommunicationOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                RetVal = RETURN_T.SERIAL_PORT_CLOSE;
            }
            return (RetVal);
        }
        private RETURN_T Init()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            InitPorts();
            InitBaudrate();
            InitDatabits();
            InitStopbits();

            return (Return);
        }
        private RETURN_T InitBaudrate()
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.ERROR;

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

            for(int Index = 0; Index < cmbBaudrate.Items.Count; Index++)
            {
                if (Convert.ToInt32(cmbBaudrate.Items[Index]) == Settings.Baudrate)
                {
                    cmbBaudrate.SelectedIndex = Index;
                    RetVal = RETURN_T.OKAY;
                    break;
                }
            }

            if (RetVal == RETURN_T.ERROR)
            {
                // Set default value
                cmbBaudrate.SelectedIndex = 5;
                Settings.Baudrate = Convert.ToInt32(cmbBaudrate.SelectedItem);
            }

            return (RetVal);
        }
        private RETURN_T InitDatabits(bool bUpdateOnly = true)
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.OKAY;

            cmbDatabits.Items.Clear();

            cmbDatabits.Items.Add("5");
            cmbDatabits.Items.Add("6");
            cmbDatabits.Items.Add("7");
            cmbDatabits.Items.Add("8");

            if (bUpdateOnly == false)
            {
                SetDatabits(Settings.DataBits);
            }
            else
            {
                RetVal = RETURN_T.ERROR;

                for (int Index = 0; Index < cmbDatabits.Items.Count; Index++)
                {
                    if (Convert.ToByte(cmbDatabits.Items[Index]) == Settings.DataBits)
                    {
                        cmbDatabits.SelectedIndex = Index;
                        RetVal = RETURN_T.OKAY;
                        break;
                    }
                }

                if (RetVal == RETURN_T.ERROR)
                {
                    // Set default value
                    cmbDatabits.SelectedIndex = 3;
                    Settings.DataBits = Convert.ToByte(cmbDatabits.SelectedItem);
                }
            }

            cmbDatabits.Text = Settings.DataBits.ToString();

            return (RetVal);
        }
        private RETURN_T InitPorts()
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.ERROR;

            cmbPort.Items.Clear();

            string[] Ports = SerialPort.GetPortNames();

            foreach (string Port in Ports)
            {
                cmbPort.Items.Add(Port);
            }

            foreach (string Port in cmbPort.Items)
            {
                if (Port == Settings.SerialPort)
                {
                    cmbPort.Text = Port;
                    RetVal = RETURN_T.OKAY;
                    break;
                }
            }

            if (RetVal == RETURN_T.ERROR)
            {
                cmbPort.SelectedIndex = 0;
                Settings.SerialPort = cmbPort.SelectedItem.ToString();
            }

            return (RetVal);
        }
        private RETURN_T InitStopbits()
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.ERROR;

            cmbStopbits.Items.Clear();

            cmbStopbits.Items.Add("Keins");
            cmbStopbits.Items.Add("1");
            cmbStopbits.Items.Add("1.5");
            cmbStopbits.Items.Add("2");

            for (int Index = 0; Index < cmbStopbits.Items.Count; Index++)
            {
                if (cmbStopbits.Items[Index] == StopBitsToText(Settings.StopBits))
                {
                    cmbStopbits.SelectedIndex = Index;
                    RetVal = RETURN_T.OKAY;
                    break;
                }
            }

            if (RetVal == RETURN_T.ERROR)
            {
                // Set default value
                cmbStopbits.SelectedIndex = 3;
                Settings.StopBits = (StopBits)cmbStopbits.SelectedItem;
            }

            return (RetVal);
        }
        private RETURN_T OpenPort()
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.OKAY;

            if (Port.IsOpen == false)
            {
                try
                {
                    if (!Port.IsOpen)
                    {
                        Port.Open();
                    }
                    Params.CommunicationOn = Port.IsOpen;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    Params.CommunicationOn = false;
                    RetVal = RETURN_T.SERIAL_PORT_OPEN;
                }
            }

            return (RetVal);
        }
        private RETURN_T SetDatabits(byte Databits)
        {
            RETURN_T RetVal;
            bool IsOpen;
            SerialPort port = new SerialPort();

            RetVal = RETURN_T.OKAY;

            if ((Databits >= 5) && (Databits <= 8))
            {
                if (RetVal == RETURN_T.OKAY)
                {
                    IsOpen = port.IsOpen;

                    if (IsOpen == true)
                    {
                        RetVal = ClosePort();
                    }

                    if (RetVal == RETURN_T.OKAY)
                    {
                        port.DataBits = Databits;

                        if (Databits != Settings.DataBits)
                        {
                            Settings.DataBits = Databits;
                        }

                        if (IsOpen == true)
                        {
                            RetVal = OpenPort();
                        }
                    }
                }
            }
            return (RetVal);
        }
        private string StopBitsToText(StopBits stopbits)
        {
            string RetString;

            if (stopbits == StopBits.None)
                RetString = "0";
            if (stopbits == StopBits.OnePointFive)
                RetString = "1.5";
            if (stopbits == StopBits.Two)
                RetString = "2";
            else
                RetString = "1";

            return (RetString);
        }
        #endregion
        #region Events
        private void btnOK_Click(object sender, EventArgs e)
        {
            bool Error;
            string Portname;
            int Baudrate;
            int Databits;
            StopBits Stopbits;
            
            Error = true;
            Portname = cmbPort.Text;
            Baudrate = Int32.Parse(cmbBaudrate.Text);
            Databits = Int32.Parse(cmbDatabits.Text);
            if (cmbStopbits.Text == "1")
                Stopbits = StopBits.One;
            else if (cmbStopbits.Text == "1.5")
                Stopbits = StopBits.OnePointFive;
            else Stopbits = StopBits.Two;

            if (Portname.Length > 3)
            {
                Port.Close();

                Port.PortName = Portname;
                Settings.SerialPort = Port.PortName;

                if ((Baudrate >= 300) && (Baudrate <= 115200))
                {
                    Port.BaudRate = Baudrate;

                    Settings.Baudrate = Port.BaudRate;

                    if ((Databits >= 5) && (Databits <= 8))
                    {
                        Port.DataBits = Databits;

                        Settings.DataBits = (byte)Port.DataBits;

                        if ((Stopbits >= (StopBits)1) && (Stopbits <= (StopBits)2))
                        {
                            Port.StopBits = Stopbits;

                            Settings.StopBits = Port.StopBits;

                            Error = false;
                        }
                    }
                }
            }

            if (Error == false)
            {
                Port.Close();

                if (Port.IsOpen == false)
                {
                    try
                    {
                        if (!Port.IsOpen)
                        {
                            Port.Open();
                        }
                        DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Error = true;
                    }
                }
            }

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void SetupLoadCell_Load(object sender, EventArgs e)
        {
            Init();
        }
        #endregion
    }
}
