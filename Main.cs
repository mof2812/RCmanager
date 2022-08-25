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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            dbgDebugging.InfoBackGround = Color.Blue;
            dbgDebugging.Add("Test");

//            rcCH0.Init("Kanal 0", RelayControl.MODE_T.TOGGLE, 500, 2000, 1000, false, Color.Red);
                rcCH0.Init("Kanal 0", RelayControl.MODE_T.IMPULSE, 500, 2000, 1000, false, Color.Red);
                rcCH1.Init("Kanal 1", RelayControl.MODE_T.TOGGLE, 500, 2000, 1000, true, Color.Green);
//            rcCH1.InitModeOn("Kanal 1", false);

            rcCH0.XRange = 10000;
            rcCH1.XRange = 10000;

            rcCH1.ChartBackColor = Color.White;
            rcCH1.BackColor = rcCH0.BackColor;
            RelayCard.NightMode = true;

            rcCH1.Mode = RelayControl.MODE_T.ON;
            rcCH1.Invert = false;

            RelayCard.BtnTextCard_1 = "Relaiskarte";
            RelayCard.BtnTextCard_2 = "Netzteil";
            RelayCard.AddChannel(0, "Kanal 1", Color.Yellow);
            RelayCard.AddChannel(1, "Kanal 2", Color.Red);
            RelayCard.AddChannel(5, "Kanal 6", Color.Green);
            RelayCard.AddChannel(10, "Kanal 11", Color.Blue);
            //RelayCard.Init(1, "", RelayControl.MODE_T.IMPULSE, 500, 2000, 1000, false, Color.Red);
            //RelayCard.Init(5, "Kanal 6", RelayControl.MODE_T.TOGGLE, 500, 2000, 1000, true, Color.Green);
            //RelayCard.Init(10, "Kanal 10", RelayControl.MODE_T.TOGGLE, 500, 2000, 1000, true, Color.Blue);

            // Led
            ledBulb1.Size = new Size(50,50);
            ledBulb1.Color = Color.Cyan;
            ledBulb1.On = true;
            ledBulb1.Blink(500);

        }
        public void AddHeaderText(string AddText)
        {
            this.Text = "RCmanager - Steuerzentrale für Relaiskarten - " + AddText;
        }
        public void SetHeaderText(string Text)
        {
            this.Text = Text;
        }

        //private SetupSerial.SERIAL_SETTINGS_T SerialSettings
        //{
        //    get
        //    {
        //        SetupSerial.SERIAL_SETTINGS_T Settings = new SetupSerial.SERIAL_SETTINGS_T();
        //        Settings.SerialPort = Properties.SerialControlSettings.Default.SerialPort;
        //        Settings.Baudrate = Properties.SerialControlSettings.Default.Baudrate;
        //        Settings.DataBits = Properties.SerialControlSettings.Default.DataBits;
        //        Settings.StopBits = Properties.SerialControlSettings.Default.StopBits;
        //        Settings.TimeoutGetData = Properties.SerialControlSettings.Default.TimeoutGetData;
        //        Settings.MaxReplies = Properties.SerialControlSettings.Default.MaxReplies;

        //        return Settings;
        //    }
        //    set
        //    {
        //        Properties.SerialControlSettings.Default.SerialPort = value.SerialPort;
        //        Properties.SerialControlSettings.Default.Baudrate = value.Baudrate;
        //        Properties.SerialControlSettings.Default.DataBits = value.DataBits;
        //        Properties.SerialControlSettings.Default.StopBits = value.StopBits;
        //        Properties.SerialControlSettings.Default.TimeoutGetData = value.TimeoutGetData;
        //        Properties.SerialControlSettings.Default.MaxReplies = value.MaxReplies;

        //        Properties.SerialControlSettings.Default.Save();
        //    }
        //}
        //private SetupSerial.SERIAL_SETTINGS_T SerialSettings_PowerSupply
        //{
        //    get
        //    {
        //        SetupSerial.SERIAL_SETTINGS_T Settings = new SetupSerial.SERIAL_SETTINGS_T();
        //        Settings.SerialPort = Properties.SerialControl_PowerSupply.Default.SerialPort;
        //        Settings.Baudrate = Properties.SerialControl_PowerSupply.Default.Baudrate;
        //        Settings.DataBits = Properties.SerialControl_PowerSupply.Default.DataBits;
        //        Settings.StopBits = Properties.SerialControl_PowerSupply.Default.StopBits;
        //        Settings.TimeoutGetData = Properties.SerialControl_PowerSupply.Default.TimeoutGetData;
        //        Settings.MaxReplies = Properties.SerialControl_PowerSupply.Default.MaxReplies;

        //        return Settings;
        //    }
        //    set
        //    {
        //        Properties.SerialControl_PowerSupply.Default.SerialPort = value.SerialPort;
        //        Properties.SerialControl_PowerSupply.Default.Baudrate = value.Baudrate;
        //        Properties.SerialControl_PowerSupply.Default.DataBits = value.DataBits;
        //        Properties.SerialControl_PowerSupply.Default.StopBits = value.StopBits;
        //        Properties.SerialControl_PowerSupply.Default.TimeoutGetData = value.TimeoutGetData;
        //        Properties.SerialControl_PowerSupply.Default.MaxReplies = value.MaxReplies;

        //        Properties.SerialControl_PowerSupply.Default.Save();
        //    }
        //}

        private void menuSettingsSerialPort_Click(object sender, EventArgs e)
        {
            SetupSerial.SERIAL_SETTINGS_T Settings = new SetupSerial.SERIAL_SETTINGS_T();

//            Settings = SerialSettings;

            if (RelayCard.Setup())
            {
//                SerialSettings = Settings;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            RelayCard.Close();
        }
    }
}
