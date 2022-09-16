using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO.Ports;

namespace SerialCommunication
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
    public partial class SerialCommunication : Component
    {
        public void OpenPort()
        {
            if (ComPort.IsOpen)
            {
                ComPort.Close();
            }
            SetPort();
            try
            {
                ComPort.Open();
            }
            catch(Exception ex)
            {
                SetupSerialDlg.MessageDlg(ex);
            }
        }
        private void SetPort()
        {
            ComPort.PortName = RCmanager.Properties.SerialSettings.Default.SerialPort;
            ComPort.BaudRate = RCmanager.Properties.SerialSettings.Default.Baudrate;
            ComPort.DataBits = RCmanager.Properties.SerialSettings.Default.Databits;
            ComPort.StopBits = RCmanager.Properties.SerialSettings.Default.Stopbits;
        }
        public RETURN_T Write(String Command)
        {
            RETURN_T RetVal;
            SerialPort port = new SerialPort();
            String Frame2Send = "";

            RetVal = RETURN_T.OKAY;

            if (!RCmanager.Properties.SerialSettings.Default.UseDelimiters)
            {
                Frame2Send = Command;
            }
            else
            {
                Frame2Send = (char)RCmanager.Properties.SerialSettings.Default.StartDelimiter + Command + (char)RCmanager.Properties.SerialSettings.Default.EndDelimiter;
            }
            port = ComPort;

            try
            {
                port.Write(Frame2Send);
                port.ReadTimeout = -1;

            }
            catch (Exception ex)
            {
                SetupSerialDlg.MessageDlg(ex);
                RetVal = RETURN_T.SERIAL_PORT_WRITE;
            }

            return (RetVal);
        }
        public RETURN_T WriteChar(char CharToSend, bool InDelimiters)
        {
            RETURN_T RetVal;
            SerialPort port = new SerialPort();
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


            port = ComPort;

            try
            {
                port.Write(Frame2Send);
            }
            catch (Exception ex)
            {
                SetupSerialDlg.MessageDlg(ex);
                RetVal = RETURN_T.SERIAL_PORT_WRITE;
            }

            return (RetVal);
        }
        public RETURN_T WriteMultipleChar(char CharToSend, uint Multiplier, bool InDelimiters, ref string FrameSent)
        {
            RETURN_T RetVal;
            SerialPort port = new SerialPort();
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


            port = ComPort;

            try
            {
                port.Write(Frame2Send);
                if (InDelimiters)
                {
                    FrameSent = Frame2Send.Substring(1, Frame2Send.Length - 2);
                    port.ReadTimeout = -1;
                }
                else
                {
                    FrameSent = Frame2Send;
                    port.ReadTimeout = RCmanager.Properties.SerialSettings.Default.TimeoutGetData_ms;
                }
            }
            catch (Exception ex)
            {
                SetupSerialDlg.MessageDlg(ex);
                RetVal = RETURN_T.SERIAL_PORT_WRITE;
            }

            return (RetVal);
        }
    }
}
