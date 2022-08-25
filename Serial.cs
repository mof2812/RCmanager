using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;

namespace RelayCard16
{
    public enum RETURN_T : uint
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
    public struct SERIAL_PARAMS_T
    {
        public byte[] ReceiveBuffer;
        public string ReceiveData;
        public int ReceivePtr;
        public RECEIVE_RETURN_T Callback;
        public int ExpectedReceiveBytes;
    }
    public partial class RelayCard16 : UserControl
    {
        public bool CommunicationOn = false;
        private SetupSerial.SetupSerial SetupDlg;
        private SERIAL_PARAMS_T SParams;
        private Stopwatch Timeout = new Stopwatch();

        private RETURN_T ClosePort()
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
        private RETURN_T Init_Serial()
        {
            RETURN_T RetVal;

            RetVal = RETURN_T.OKAY;
            SetupDlg = new SetupSerial.SetupSerial(SerialPort);

            // Add serial port to rcCard1 and rcCard2
            rcCard1.AddSerialPort(0, ref SerialPort);
            rcCard2.AddSerialPort(1, ref SerialPort);


            SParams.ReceiveBuffer = new byte[64];
            SParams.ReceiveData = "";
            SParams.ReceivePtr = -1;
            SParams.ExpectedReceiveBytes = 0;
            SParams.Callback = RECEIVE_RETURN_T.BUSY;


            SerialPort.Close();

            SerialPort.PortName = SerialPortSettings.Default.Port;
            SerialPort.ReadTimeout = (int)SerialPortSettings.Default.TimeoutGetData + 50;
            SerialPort.BaudRate = SerialPortSettings.Default.Baudrate;
            SerialPort.DataBits = SerialPortSettings.Default.DataBits;
            SerialPort.StopBits = SerialPortSettings.Default.StopBits;

            if (SerialPort.PortName.Length > 3)
            {
                RetVal = OpenPort();
            }

            return (RetVal);
        }
        private RETURN_T OpenPort()
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
        private RECEIVE_RETURN_T ReceiveFrame(ref string ReceiveData)
        {
            RECEIVE_RETURN_T RetVal;
            int BytesRead;
            int BytesToRead;
            int i;
            bool Copy;
            byte[] Data = new byte[64];

            RetVal = RECEIVE_RETURN_T.BUSY;
            Copy = false;
            BytesRead = 0;

            try
            {
                if (SerialPort.IsOpen == true)
                {
                    try
                    {
                        if (Timeout.ElapsedMilliseconds < (long)SerialPortSettings.Default.TimeoutGetData)
                        {
                            try
                            {
                                BytesToRead = SerialPort.BytesToRead;

                                try
                                {
                                    if (BytesToRead > 0)
                                    {
                                        BytesRead = SerialPort.Read(Data, 0, BytesToRead);
                                    }
                                    else
                                    {
                                        BytesRead = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    BytesRead = 0;
                                    MessageBox.Show(ex.Message);
                                }
                                if (BytesRead > 0)
                                {
                                    if (SParams.ReceivePtr == -1)
                                    {
                                        if (Data[0] == (char)0x02)
                                        {
                                            SParams.ReceiveBuffer[0] = 0;
                                            SParams.ReceivePtr = 0;
                                            Copy = true;
                                        }
                                    }
                                    else
                                    {
                                        Copy = true;
                                    }

                                    if (Copy == true)
                                    {
                                        i = 0;
                                        while ((Data[i] != (char)0x03) && (i < BytesRead))
                                        {
                                            if (Data[i] != (char)0x02)
                                            {
                                                SParams.ReceiveBuffer[SParams.ReceivePtr++] = Data[i];
                                            }
                                            i++;
                                        }

                                        if (Data[i] == (char)0x03)
                                        {
                                            SParams.ReceiveBuffer[SParams.ReceivePtr] = 0;

                                            SerialPort.DiscardInBuffer();

                                            ReceiveData = System.Text.Encoding.UTF8.GetString(SParams.ReceiveBuffer, 0, SParams.ReceivePtr);

                                            if ((SParams.ReceiveBuffer[0] == 'O') && (SParams.ReceiveBuffer[1] == 'K'))
                                            {
                                                RetVal = RECEIVE_RETURN_T.OK_FRAME;
                                            }
                                            else if ((SParams.ReceiveBuffer[0] == 'E') && (SParams.ReceiveBuffer[1] >= '0') && (SParams.ReceiveBuffer[1] <= '9') && (SParams.ReceiveBuffer[2] >= '0') && (SParams.ReceiveBuffer[2] <= '9') && (SParams.ReceiveBuffer[3] >= '0') && (SParams.ReceiveBuffer[3] <= '9'))
                                            {
                                                RetVal = RECEIVE_RETURN_T.ERROR_FRAME;
                                            }
                                            else
                                            {
                                                RetVal = RECEIVE_RETURN_T.DATA_FRAME;
                                            }

                                            SParams.ReceivePtr = -1;
                                            SParams.ReceiveBuffer[0] = 0;

                                            //Debug_AddReceiveFrame(ReceiveData);
                                        }
                                        Copy = false;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                SerialPort.DiscardInBuffer();
                            }
                        }
                        else
                        {
                            long TimeoutGetData = (long)SerialPortSettings.Default.TimeoutGetData;
                            RetVal = RECEIVE_RETURN_T.TIMEOUT;
                            SerialPort.DiscardInBuffer();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult != -2146233083)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        else
                        {
                            RetVal = RECEIVE_RETURN_T.TIMEOUT;
                        }
                    }
                }
                else
                {
                    RetVal = RECEIVE_RETURN_T.PORT_CLOSED;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return (RetVal);
        }
        private RECEIVE_RETURN_T ReceiveRawData(ref string ReceiveData)
        {
            RECEIVE_RETURN_T RetVal;
            int BytesRead;
            int i;
            byte[] Data = new byte[64];

            RetVal = RECEIVE_RETURN_T.BUSY;
            BytesRead = 0;

            try
            {
                if (SerialPort.IsOpen == true)
                {
                    try
                    {
                        if (Timeout.ElapsedMilliseconds < (long)SerialPortSettings.Default.TimeoutGetData)
                        {
                            try
                            {
                                BytesRead = SerialPort.BytesToRead;
//                                txtDebug2.Text = BytesRead.ToString();
                                if (BytesRead >= SParams.ExpectedReceiveBytes)
                                {
                                    BytesRead = SerialPort.Read(Data, 0, SParams.ExpectedReceiveBytes);
                                    Array.Copy(Data, SParams.ReceiveBuffer, BytesRead);
                                    SParams.ReceivePtr = BytesRead;
                                    SerialPort.DiscardInBuffer();


                                    ReceiveData = System.Text.Encoding.UTF8.GetString(SParams.ReceiveBuffer, 0, BytesRead);

                                    RetVal = RECEIVE_RETURN_T.DATA_FRAME;

//                                    Debug_AddReceiveFrame(ReceiveData);

                                    Reset_Reception();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            RetVal = RECEIVE_RETURN_T.TIMEOUT;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult != -2146233083)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        else
                        {
                            RetVal = RECEIVE_RETURN_T.TIMEOUT;
                        }
                    }
                }
                else
                {
                    RetVal = RECEIVE_RETURN_T.PORT_CLOSED;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return (RetVal);
        }
        private void Reset_Reception()
        {
            SParams.ReceiveBuffer[0] = 0;
            SParams.ReceivePtr = -1;

            Timeout.Restart();
            SerialPort.ReadTimeout = SerialPortSettings.Default.ReadTimeout;
        }
        private RETURN_T SetBaudrate(int Baudrate)
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
                        RetVal = ClosePort();
                    }

                    if (RetVal == RETURN_T.OKAY)
                    {
                        SerialPort.BaudRate = Baudrate;

                        if (Baudrate != SerialPortSettings.Default.Baudrate)
                        {
                            SerialPortSettings.Default.Baudrate = Baudrate;
                            SerialPortSettings.Default.Save();
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
        private RETURN_T SetDatabits(byte Databits)
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
                        RetVal = ClosePort();
                    }

                    if (RetVal == RETURN_T.OKAY)
                    {
                        SerialPort.DataBits = Databits;

                        if (Databits != SerialPortSettings.Default.DataBits)
                        {
                            SerialPortSettings.Default.DataBits = Databits;
                            SerialPortSettings.Default.Save();
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
        private RETURN_T SetPort(String Port)
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
                        RetVal = ClosePort();
                    }

                    if (RetVal == RETURN_T.OKAY)
                    {
                        if (Port != SerialPortSettings.Default.Port)
                        {
                            SerialPortSettings.Default.Port = Port;
                            SerialPortSettings.Default.Save();
                        }

                        if (SerialPort.IsOpen == false)
                        {
                            RetVal = OpenPort();
                        }
                    }
                }

            }
            return (RetVal);
        }
        private RETURN_T SetStopbits(StopBits Stopbits)
        {
            RETURN_T RetVal;
            bool IsOpen;

            RetVal = RETURN_T.OKAY;

            if (RetVal == RETURN_T.OKAY)
            {
                IsOpen = SerialPort.IsOpen;

                if (IsOpen == true)
                {
                    RetVal = ClosePort();
                }

                if (RetVal == RETURN_T.OKAY)
                {
                    SerialPort.StopBits = Stopbits;

                    if (Stopbits != SerialPortSettings.Default.StopBits)
                    {
                        SerialPortSettings.Default.StopBits = Stopbits;
                        SerialPortSettings.Default.Save();
                    }

                    if (IsOpen == true)
                    {
                        RetVal = OpenPort();
                    }
                }
            }
            return (RetVal);
        }
        public bool Setup()
        {
            bool Error;

            Error = false;

            SetupDlg.ShowDialog();

            return Error;
        }
        private RETURN_T Write(String Text)
        {
            RETURN_T RetVal;
            String Frame2Send = "";

            RetVal = RETURN_T.OKAY;
            Frame2Send = (char)0x02 + Text + (char)0x03;

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
        private RETURN_T WriteChar(char CharToSend, bool InDelimiters)
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
        private RETURN_T WriteMultipleChar(char CharToSend, uint Multiplier, bool InDelimiters, ref string FrameSent)
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
                    SerialPort.ReadTimeout = (int)SerialPortSettings.Default.TimeoutGetData;
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
