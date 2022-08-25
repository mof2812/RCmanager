using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows.Forms;

namespace SerialControl
{
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
    public partial class SerialControl : Component
    {
        public bool CommunicationOn = false;
        private SetupSerial.SetupSerial SetupDlg;
        public SerialControl()
        {
            InitializeComponent();

            Serial_Init();
        }

        public SerialControl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            Serial_Init();
            
        }
        public bool OpenSetup()
        {
            bool Error;

            Error = false;

            SetupDlg.ShowDialog();

            return Error;
        }
        private RETURN_T Serial_ClosePort()
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.OKAY;

            try
            {
                SerialPort.Close();
                CommunicationOn = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                RetVal = RETURN_T.SERIAL_PORT_CLOSE;
            }
            return (RetVal);
        }
        private RETURN_T Serial_Init()
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.OKAY;
            SetupDlg = new SetupSerial.SetupSerial(SerialPort);

            //SerialPort.Close();

            SerialPort.PortName = SerialControlSettings.Default.Port;
            SerialPort.ReadTimeout = (int)SerialControlSettings.Default.TimeoutGetData + 50;
            SerialPort.BaudRate = SerialControlSettings.Default.Baudrate;
            SerialPort.DataBits = SerialControlSettings.Default.DataBits;
            SerialPort.StopBits = SerialControlSettings.Default.StopBits;

            if (SerialPort.PortName.Length > 3)
            {
                try
                {
                    if (!SerialPort.IsOpen)
                    {
                        SerialPort.Open();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    RetVal = RETURN_T.SERIAL_PORT_UNDEF;
                }
            }


            return (RetVal);
        }
        private RETURN_T Serial_OpenPort()
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.OKAY;

            if (SerialPort.IsOpen == false)
            {
                try
                {
                    if (!SerialPort.IsOpen)
                    {
                        SerialPort.Open();
                    }
                    CommunicationOn = SerialPort.IsOpen;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    CommunicationOn = false;
                    RetVal = RETURN_T.SERIAL_PORT_OPEN;
                }
            }
            return (RetVal);
        }
        private RETURN_T Serial_SetBaudrate(int Baudrate)
        {
            RETURN_T RetVal;
            bool IsOpen;

            RetVal = RETURN_T.OKAY;

            if ((Baudrate >= 300) && (Baudrate <= 115200))
            {
                if (RetVal == RETURN_T.OKAY)
                {
                    IsOpen = SerialPort.IsOpen;

                    if (IsOpen == true)
                    {
                        RetVal = Serial_ClosePort();
                    }

                    if (RetVal == RETURN_T.OKAY)
                    {
                        SerialPort.BaudRate = Baudrate;

                        if (Baudrate != SerialControlSettings.Default.Baudrate)
                        {
                            SerialControlSettings.Default.Baudrate = Baudrate;
                            SerialControlSettings.Default.Save();
                        }

                        if (IsOpen == true)
                        {
                            RetVal = Serial_OpenPort();
                        }
                    }
                }
            }
            return (RetVal);
        }
        private RETURN_T Serial_SetDatabits(byte Databits)
        {
            RETURN_T RetVal;
            bool IsOpen;

            RetVal = RETURN_T.OKAY;

            if ((Databits >= 5) && (Databits <= 8))
            {
                if (RetVal == RETURN_T.OKAY)
                {
                    IsOpen = SerialPort.IsOpen;

                    if (IsOpen == true)
                    {
                        RetVal = Serial_ClosePort();
                    }

                    if (RetVal == RETURN_T.OKAY)
                    {
                        SerialPort.DataBits = Databits;

                        if (Databits != SerialControlSettings.Default.DataBits)
                        {
                            SerialControlSettings.Default.DataBits = Databits;
                            SerialControlSettings.Default.Save();
                        }

                        if (IsOpen == true)
                        {
                            RetVal = Serial_OpenPort();
                        }
                    }
                }
            }
            return (RetVal);
        }
        private RETURN_T Serial_SetPort(String Port)
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.OKAY;

            if (Port.Length < 4)
            {
                RetVal = RETURN_T.SERIAL_PORT_UNDEF;
            }
            else
            {
                if (RetVal == RETURN_T.OKAY)
                {
                    if (SerialPort.IsOpen == true)
                    {
                        RetVal = Serial_ClosePort();
                    }

                    if (RetVal == RETURN_T.OKAY)
                    {
                        if (Port != SerialControlSettings.Default.Port)
                        {
                            SerialControlSettings.Default.Port = Port;
                            SerialControlSettings.Default.Save();
                        }

                        if (SerialPort.IsOpen == false)
                        {
                            RetVal = Serial_OpenPort();
                        }
                    }
                }

            }
            return (RetVal);
        }
        private RETURN_T Serial_SetStopbits(StopBits Stopbits)
        {
            RETURN_T RetVal;
            bool IsOpen;

            RetVal = RETURN_T.OKAY;

            if (RetVal == RETURN_T.OKAY)
            {
                IsOpen = SerialPort.IsOpen;

                if (IsOpen == true)
                {
                    RetVal = Serial_ClosePort();
                }

                if (RetVal == RETURN_T.OKAY)
                {
                    SerialPort.StopBits = Stopbits;

                    if (Stopbits != SerialControlSettings.Default.StopBits)
                    {
                        SerialControlSettings.Default.StopBits = Stopbits;
                        SerialControlSettings.Default.Save();
                    }

                    if (IsOpen == true)
                    {
                        RetVal = Serial_OpenPort();
                    }
                }
            }
            return (RetVal);
        }
        private void Serial_Setup()
        {
            SetupSerial.SetupSerial SetupLoadcellDlg = new SetupSerial.SetupSerial(SerialPort);
            DialogResult Result;

            Result = SetupLoadcellDlg.ShowDialog();

            switch (Result)
            {
                case DialogResult.OK:
                    break;

                case DialogResult.Cancel:
                    SerialControlSettings.Default.Reload();
                    break;

                default:
                    break;
            }
        }
        private RETURN_T Serial_Write(String Command)
        {
            RETURN_T RetVal;
            String Frame2Send = "";

            RetVal = RETURN_T.OKAY;
            Frame2Send = (char)0x02 + Command + (char)0x03;

            try
            {
                SerialPort.Write(Frame2Send);
                SerialPort.ReadTimeout = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                RetVal = RETURN_T.SERIAL_PORT_WRITE;
            }

            //            Debug_AddSendFrame(Command, RetVal == RETURN_T.OKAY);

            return (RetVal);
        }
        private RETURN_T Serial_WriteChar(char CharToSend, bool InDelimiters)
        {
            RETURN_T RetVal;
            String Frame2Send = "";

            RetVal = RETURN_T.OKAY;

            if (InDelimiters)
            {
                Frame2Send = Convert.ToString((char)0x02 + (char)CharToSend + (char)0x03);
            }
            else
            {
                Frame2Send = Convert.ToString(CharToSend);
            }

            try
            {
                SerialPort.Write(Frame2Send);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                RetVal = RETURN_T.SERIAL_PORT_WRITE;
            }

            //            Debug_AddSendFrame(Convert.ToString(CharToSend), RetVal == RETURN_T.OKAY);

            return (RetVal);
        }
        private RETURN_T Serial_WriteMultipleChar(char CharToSend, uint Multiplier, bool InDelimiters, ref string FrameSent)
        {
            RETURN_T RetVal;
            String Frame2Send = "";

            RetVal = RETURN_T.OKAY;
            FrameSent = "";

            if (InDelimiters)
            {
                Frame2Send = Convert.ToString((char)0x02);
            }
            while (Multiplier-- > 0)
            {
                Frame2Send += Convert.ToString(CharToSend);
            }
            if (InDelimiters)
            {
                Frame2Send += Convert.ToString((char)0x03);
            }

            try
            {
                SerialPort.Write(Frame2Send);
                if (InDelimiters)
                {
                    FrameSent = Frame2Send.Substring(1, Frame2Send.Length - 2);
                    SerialPort.ReadTimeout = -1;
                }
                else
                {
                    FrameSent = Frame2Send;
                    //port.ReadTimeout = (int)Properties.Settings.Default.CharToSendMultiplier * 4;
                    SerialPort.ReadTimeout = (int)SerialControlSettings.Default.TimeoutGetData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                RetVal = RETURN_T.SERIAL_PORT_WRITE;
            }

            //            Debug_AddSendFrame(FrameSent, RetVal == RETURN_T.OKAY);

            return (RetVal);
        }
    }
}
