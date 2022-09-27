using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SerialCommunication
{
    #region Structs and Enums
    public enum RECEIVE_WAIT_FOR_T
    {
        NONE = 0,
        OK,
        DATA,
        ERROR,
    }
    #endregion
    public partial class SerialCommunication : Component
    {
        #region Structs and Enums
        public enum STATE_T
        {
            INIT = 0,
            IDLE,
            SEND,
            RECEIVE,
            EVALUATE,
            ERROR,
            OKAY,
            NEW_DATA,
            NO_DATA,    /* for return only */
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
            NO_RESPONSE,
        }
        public struct COM_BUFFER_T
        {
            public ACTION_T[] Action;
            public string[,] Parameter;
            public int ReadPtr;
            public int WritePtr;
            public int DataInBuffer;
            public int[] MinInterval_ms;
            public long[] TargetTime;
        }
        public struct PARAMS_T
        {
            public STATE_T State;
            public ACTION_T ActionExecuting;
            public string[] ParameterExecuting;
            public string ReceivedData;
            public bool SmallVersion;
            public bool VersionSet;     // Small version <-> Std version
            public bool UseDelimitersPrepared;
            public bool UseDelimiters;
        }
        public struct RECEIVE_DATA_T
        {
            public byte[] ReceiveBuffer;
            public string ReceiveData;
            public int ReceivePtr;
            public RECEIVE_RETURN_T Callback;
            public int ExpectedReceiveBytes;
            public byte RepliesLeft;
        }
        #endregion
        #region Members
        public event EventHandler<FrameReceivedEventArgs> FrameReceived;
        private SetupSerialDlg SetupSerialDlg = new SetupSerialDlg();
        private PARAMS_T Params;
        private RECEIVE_DATA_T ReceiveData;
        private COM_BUFFER_T ComBuffer;
        private COM_BUFFER_T ComBufferContinous;
        private Stopwatch Timeout = new Stopwatch();
        private Stopwatch CommunicationTime = new Stopwatch();
        #endregion
        #region Properties
        [
        Category("Apperance"),
        Description("Use small version"),
        DefaultValue(false)
        ]
        public bool SmallVersion
        {
            get
            {
                return Params.SmallVersion;
            }
            set
            {
                Params.SmallVersion = value;

                if (Params.UseDelimitersPrepared)
                {
                    if (Params.SmallVersion)
                    {
                        RCmanager.Properties.SerialSettings.Default.UseDelimiters = Params.UseDelimiters;
                        RCmanager.Properties.SerialSettings.Default.Save();
                        Params.UseDelimitersPrepared = false;
                    }
                }
            }
        }
        [
        Category("Bitmap"),
        Description("Bitmap shown in the setup dialog"),
        ]
        public string Bitmap
        {
            get
            {
                return SetupSerialDlg.Bitmap;
            }
            set
            {
                SetupSerialDlg.Bitmap = value;
            }
        }
        [
        Category("Communication"),
        Description("Baudrate by default"),
        ]
        public int DefaultBaudrate
        {
            get
            {
                return SetupSerialDlg.DefaultBaudrate;
            }
            set
            {
                SetupSerialDlg.DefaultBaudrate = value;

                Init();
            }
        }
        [
        Category("Communication"),
        Description("Databits by default"),
        //DefaultValue(8)
        ]
        public byte DefaultDatabits
        {
            get
            {
                return SetupSerialDlg.DefaultDatabits;
            }
            set
            {
                SetupSerialDlg.DefaultDatabits = value;

                Init();
            }
        }
        [
        Category("Communication"),
        Description("Communication port by default"),
        //DefaultValue("COM1")
        ]
        public string DefaultSerialPort
        {
            get
            {
                return SetupSerialDlg.DefaultSerialPort;
            }
            set
            {
                SetupSerialDlg.DefaultSerialPort = value;

                Init();
            }
        }
        [
        Category("Communication"),
        Description("Databits by default"),
        //DefaultValue("One")
        ]
        public StopBits DefaultStopbits
        {
            get
            {
                return SetupSerialDlg.DefaultStopbits;
            }
            set
            {
                SetupSerialDlg.DefaultStopbits = value;

                Init();
            }
        }
        [
        Category("Communication"),
        Description("Use delimiters (used on small version)"),
        DefaultValue(false)
        ]
        public bool UseDelimiters
        {
            get
            {
                return RCmanager.Properties.SerialSettings.Default.UseDelimiters;
            }
            set
            {
                if (Params.VersionSet)
                {
                    RCmanager.Properties.SerialSettings.Default.UseDelimiters = value;
                    RCmanager.Properties.SerialSettings.Default.Save();
                    Params.UseDelimitersPrepared = false;
                }
                else
                {
                    Params.UseDelimitersPrepared = true;
                    Params.UseDelimiters = value;
                }
            }
        }

        #endregion
        public bool ClearAction<S, T, U, V, W>(ACTION_T Action, S Param_1, T Param_2, U Param_3, V Param_4, W Param_5)
        {
            bool Error;
            int Index;
            int Index_2; 
            int Index_WR;
            int ClearIndex;

            Error = false;
            Index = 0;
            Index_2 = 0;
            Index_WR = 0;
            ClearIndex = 0;

            if (ComBufferContinous.DataInBuffer > 0)
            {
                for (Index = 0; Index < ComBufferContinous.DataInBuffer; Index++)
                {
                    if (Action == ComBufferContinous.Action[Index])
                    {
                        if (ComBufferContinous.Parameter[Index, 0] == Param_1.ToString())
                        {
                            if (ComBufferContinous.Parameter[Index, 1] == Param_2.ToString())
                            {
                                if (ComBufferContinous.Parameter[Index, 2] == Param_3.ToString())
                                {
                                    if (ComBufferContinous.Parameter[Index, 3] == Param_4.ToString())
                                    {
                                        if (ComBufferContinous.Parameter[Index, 4] == Param_5.ToString())
                                        {
                                            // Entry found - clear it
                                            // Do reorganisation

                                            Index_WR = 0;

                                            for (Index_2 = 0; Index_2 <= ComBufferContinous.WritePtr; Index_2++)
                                            {
                                                if (Index_2 != Index)
                                                {
                                                    ComBufferContinous.Action[Index_WR] = ComBufferContinous.Action[Index_2];
                                                    ComBufferContinous.Parameter[Index_WR, 0] = ComBufferContinous.Parameter[Index_2, 0];
                                                    ComBufferContinous.Parameter[Index_WR, 1] = ComBufferContinous.Parameter[Index_2, 1];
                                                    ComBufferContinous.Parameter[Index_WR, 2] = ComBufferContinous.Parameter[Index_2, 2];
                                                    ComBufferContinous.Parameter[Index_WR, 3] = ComBufferContinous.Parameter[Index_2, 3];

                                                    Index_WR++;
                                                }
                                            }
                                            // Clear last entry
                                            ComBufferContinous.Action[Index_WR] = ACTION_T.NONE;
                                            for (byte ParameterIndex = 0; ParameterIndex < Constants.ACTIONLIST_PARAMETERS; ParameterIndex++)
                                            {
                                                ComBufferContinous.Parameter[Index_WR, ParameterIndex] = "";
                                            }

                                            if (Index < ComBufferContinous.ReadPtr)
                                            {
                                                ComBufferContinous.ReadPtr--;
                                            }
                                            ComBufferContinous.WritePtr--;
                                            ComBufferContinous.DataInBuffer--;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return Error;
        }
        public STATE_T CommunicationTask()
        {
            STATE_T RetVal;
            bool NewData;
            bool StayInLoop;

            NewData = false;

            do
            {
                StayInLoop = false;
                RetVal = Params.State;

                switch (Params.State)
                {
                    case STATE_T.INIT:
                        Params.State = STATE_T.IDLE;
                        StayInLoop = true;
                        ReceiveData.RepliesLeft = 5;
                        break;

                    case STATE_T.IDLE:
                        if (ComBuffer.DataInBuffer > 0)
                        {
                            if (GetAction(ref Params.ActionExecuting, ref Params.ParameterExecuting, false) == false)
                            {
                                Params.State = STATE_T.SEND;
                            }
                            else
                            {
                                Params.State = STATE_T.ERROR;
                            }
                            StayInLoop = true;
                        }
                        else if (ComBufferContinous.DataInBuffer > 0)
                        {
                            if (GetAction(ref Params.ActionExecuting, ref Params.ParameterExecuting, true) == false)
                            {
                                Params.State = STATE_T.SEND;
                            }
                            else
                            {
                                Params.State = STATE_T.ERROR;
                            }
                            StayInLoop = true;
                        }
                        else
                        {
                            RetVal = STATE_T.NO_DATA;
                        }
                        break;

                    case STATE_T.SEND:
                        if (CommunicationTask_Send(Params))
                        {
                            // Error
                            if (ReceiveData.RepliesLeft-- > 0)
                            {
                                Params.State = STATE_T.SEND;
                            }
                            else
                            {
                                Params.State = STATE_T.ERROR;
                            }
                        }
                        else
                        {
                            Params.State = STATE_T.RECEIVE;
                        }

                        if (Params.State == STATE_T.RECEIVE)
                        {
                            CommunicationTask_ReceiveInit();
                        }
                        break;

                    case STATE_T.RECEIVE:
                        RECEIVE_RETURN_T Callback;

                        Callback = CommunicationTask_Receive(Params);

                        if (Callback == RECEIVE_RETURN_T.OKAY)
                        {
                            Params.State = STATE_T.EVALUATE;
                        }
                        else if (Callback != RECEIVE_RETURN_T.BUSY)
                        {
                            if (ReceiveData.RepliesLeft-- > 0)
                            {
                                Params.State = STATE_T.SEND;
                            }
                            else
                            {
                                Params.State = STATE_T.ERROR;
                            }
                        }
                        break;

                    case STATE_T.EVALUATE:
                        if (CommunicationTask_Evaluate(ref Params))
                        {
                            // Error
                            if (ReceiveData.RepliesLeft-- > 0)
                            {
                                Params.State = STATE_T.SEND;
                            }
                            else
                            {
                                Params.State = STATE_T.ERROR;
                            }
                        }
                        else
                        {
                            Params.State = STATE_T.OKAY;
                        }
                        break;

                    case STATE_T.ERROR:
                        Params.State = STATE_T.INIT;
                        break;

                    case STATE_T.OKAY:
                        NewData = true;
                        Params.State = STATE_T.IDLE;

                        ReceiveData.RepliesLeft = 5;
                        break;

                    case STATE_T.NEW_DATA:
                        Params.State = STATE_T.IDLE;
                        break;

                    default:
                        Params.State = STATE_T.INIT;
                        break;
                }

            } while (StayInLoop == true);

            return RetVal;
        }
        private void CommunicationTask_ClearReceiveBuffer()
        {
            if (ComPort.IsOpen)
            {
                ComPort.DiscardInBuffer();
            }
        }
        private bool CommunicationTask_Evaluate(ref PARAMS_T Params)
        {
            FrameReceivedEventArgs Args = new FrameReceivedEventArgs();

            bool Error;

            Error = false;
            Args.Parameter = Params;

            OnFrameReceived(Args);

            return Error;    
        }
        private RECEIVE_RETURN_T CommunicationTask_Receive(PARAMS_T Params)
        {
            RECEIVE_RETURN_T Callback;
            RECEIVE_WAIT_FOR_T WaitFor;

            Callback = RECEIVE_RETURN_T.ERROR;

            WaitFor = Receive(Params.ActionExecuting);
            
            switch (WaitFor)
            {
                case RECEIVE_WAIT_FOR_T.NONE:
                    Callback = RECEIVE_RETURN_T.OKAY;
                    break;

                case RECEIVE_WAIT_FOR_T.DATA:
                    Callback = CommunicationTask_WaitForData();
                    break;

                case RECEIVE_WAIT_FOR_T.OK:
                    Callback = CommunicationTask_WaitForOkay();
                    break;

                default:
                    break;
            }

            return Callback;
        }
        private RECEIVE_RETURN_T CommunicationTask_ReceiveFrame(ref string ReceivedData)
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
                if (ComPort.IsOpen == true)
                {
                    try
                    {
                        if (Timeout.ElapsedMilliseconds < RCmanager.Properties.SerialSettings.Default.TimeoutGetData_ms)//Timeout.ElapsedMilliseconds < 500L)
                        {
                            try
                            {
                                BytesToRead = ComPort.BytesToRead;

                                try
                                {
                                    if (BytesToRead > 0)
                                    {
                                        BytesRead = ComPort.Read(Data, 0, BytesToRead);
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
                                    if (ReceiveData.ReceivePtr == -1)
                                    {
                                        if (Data[0] == (char)RCmanager.Properties.SerialSettings.Default.StartDelimiter)
                                        {
                                            ReceiveData.ReceiveBuffer[0] = 0;
                                            ReceiveData.ReceivePtr = 0;
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
                                        while ((Data[i] != (char)RCmanager.Properties.SerialSettings.Default.EndDelimiter) && (i < BytesRead))
                                        {
                                            if (Data[i] != (char)RCmanager.Properties.SerialSettings.Default.StartDelimiter)
                                            {
                                                ReceiveData.ReceiveBuffer[ReceiveData.ReceivePtr++] = Data[i];
                                            }
                                            i++;
                                        }

                                        if (Data[i] == (char)RCmanager.Properties.SerialSettings.Default.EndDelimiter)
                                        {
                                            ReceiveData.ReceiveBuffer[ReceiveData.ReceivePtr] = 0;

                                            ComPort.DiscardInBuffer();

                                            ReceivedData = System.Text.Encoding.UTF8.GetString(ReceiveData.ReceiveBuffer, 0, ReceiveData.ReceivePtr);

                                            if ((ReceiveData.ReceiveBuffer[0] == 'O') && (ReceiveData.ReceiveBuffer[1] == 'K'))
                                            {
                                                RetVal = RECEIVE_RETURN_T.OK_FRAME;
                                            }
                                            else if ((ReceiveData.ReceiveBuffer[0] == 'E') && (ReceiveData.ReceiveBuffer[1] >= '0') && (ReceiveData.ReceiveBuffer[1] <= '9') && (ReceiveData.ReceiveBuffer[2] >= '0') && (ReceiveData.ReceiveBuffer[2] <= '9') && (ReceiveData.ReceiveBuffer[3] >= '0') && (ReceiveData.ReceiveBuffer[3] <= '9'))
                                            {
                                                RetVal = RECEIVE_RETURN_T.ERROR_FRAME;
                                            }
                                            else
                                            {
                                                RetVal = RECEIVE_RETURN_T.DATA_FRAME;
                                            }

                                            ReceiveData.ReceivePtr = -1;
                                            ReceiveData.ReceiveBuffer[0] = 0;
                                        }
                                        Copy = false;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                ComPort.DiscardInBuffer();
                            }
                        }
                        else
                        {
                            long TimeoutGetData = 500L;
                            RetVal = RECEIVE_RETURN_T.TIMEOUT;
                            ComPort.DiscardInBuffer();
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
        private RECEIVE_RETURN_T CommunicationTask_ReceiveRawFrame(ref string ReceivedData) // Receive frame without delimiters
        {
            RECEIVE_RETURN_T RetVal;
            int BytesRead;
            int i;
            bool Copy;
            byte[] Data = new byte[64];

            RetVal = RECEIVE_RETURN_T.BUSY;
            Copy = false;
            BytesRead = 0;

            try
            {
                if (ComPort.IsOpen == true)
                {
                    try
                    {
                        while (((BytesRead = ComPort.BytesToRead) > 0) || ((Timeout.ElapsedMilliseconds < RCmanager.Properties.SerialSettings.Default.TimeoutGetData_ms) && (ReceiveData.ReceivePtr < 0))) // as long as there are bytes received or not data received untilnow but the timeout has not expired 
                        {
                            if (BytesRead > 0)//Timeout.ElapsedMilliseconds < RCmanager.Properties.SerialSettings.Default.TimeoutGetData_ms)//Timeout.ElapsedMilliseconds < 500L)
                            {
                                try
                                {
                                    try
                                    {
                                        BytesRead = ComPort.Read(Data, 0, BytesRead);
                                    }
                                    catch (Exception ex)
                                    {
                                        BytesRead = 0;
                                        MessageBox.Show(ex.Message);
                                    }
                                    for (i = 0; i < BytesRead; i++)
                                    {
                                        ReceiveData.ReceivePtr++;
                                        ReceiveData.ReceiveBuffer[ReceiveData.ReceivePtr] = Data[i];
                                    }
                                    ReceiveData.ReceiveBuffer[ReceiveData.ReceivePtr + 1] = 0;
                                    ComPort.DiscardInBuffer();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                    ComPort.DiscardInBuffer();
                                }
                            }
                            else
                            {
                                ReceiveData.ReceivePtr = -1;
                                ReceiveData.ReceiveBuffer[0] = 0;
                                ComPort.DiscardInBuffer();

                                RetVal = RECEIVE_RETURN_T.TIMEOUT;
                            }
                        }
                        if (RetVal == RECEIVE_RETURN_T.BUSY)
                        {
                            if (ReceiveData.ReceiveBuffer[0] == 0)
                            {
                                RetVal = RECEIVE_RETURN_T.NO_RESPONSE;
                            }
                            else
                            { 
                                ReceivedData = System.Text.Encoding.UTF8.GetString(ReceiveData.ReceiveBuffer, 0, ReceiveData.ReceivePtr + 1);

                                if ((ReceiveData.ReceiveBuffer[0] == 'O') && (ReceiveData.ReceiveBuffer[1] == 'K'))
                                {
                                    RetVal = RECEIVE_RETURN_T.OK_FRAME;
                                }
                                else if ((ReceiveData.ReceiveBuffer[0] == 'E') && (ReceiveData.ReceiveBuffer[1] >= '0') && (ReceiveData.ReceiveBuffer[1] <= '9') && (ReceiveData.ReceiveBuffer[2] >= '0') && (ReceiveData.ReceiveBuffer[2] <= '9') && (ReceiveData.ReceiveBuffer[3] >= '0') && (ReceiveData.ReceiveBuffer[3] <= '9'))
                                {
                                    RetVal = RECEIVE_RETURN_T.ERROR_FRAME;
                                }
                                else
                                {
                                    RetVal = RECEIVE_RETURN_T.DATA_FRAME;
                                }
                            }
                            ReceiveData.ReceivePtr = -1;
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
        private void CommunicationTask_ReceiveInit()
        {
            ReceiveData.ReceiveBuffer[0] = 0;
            ReceiveData.ReceivePtr = -1;

            Timeout.Restart();
        }
        private bool CommunicationTask_Send(PARAMS_T Params)
        {
            bool Error;
            string Frame;

            Error = false;
            Frame = Send(Params);

            if (ComPort.IsOpen)
            {
                if (!Error)
                {
                    // Clear input buffer
                    CommunicationTask_ClearReceiveBuffer();

                    Write(Frame);
                }
            }
            else
            {
                Error = true;
            }

            return Error;
        }
        private RECEIVE_RETURN_T CommunicationTask_WaitForData()
        {
            RECEIVE_RETURN_T RetVal;
            RECEIVE_RETURN_T Callback;

            RetVal = RECEIVE_RETURN_T.ERROR;

            Callback = RCmanager.Properties.SerialSettings.Default.UseDelimiters ? CommunicationTask_ReceiveFrame(ref Params.ReceivedData) : CommunicationTask_ReceiveRawFrame(ref Params.ReceivedData);

            switch (Callback)
            {
                case RECEIVE_RETURN_T.BUSY:
                    RetVal = RECEIVE_RETURN_T.BUSY;
                    break;

                case RECEIVE_RETURN_T.DATA_FRAME:
                    RetVal = RECEIVE_RETURN_T.OKAY;
                    break;

                default:
                    break;
            }

            return RetVal;
        }
        private RECEIVE_RETURN_T CommunicationTask_WaitForOkay()
        {
            RECEIVE_RETURN_T RetVal;
            RECEIVE_RETURN_T Callback;

            RetVal = RECEIVE_RETURN_T.ERROR;

            Callback = RCmanager.Properties.SerialSettings.Default.UseDelimiters ? CommunicationTask_ReceiveFrame(ref Params.ReceivedData) : CommunicationTask_ReceiveRawFrame(ref Params.ReceivedData);

            switch (Callback)
            {
                case RECEIVE_RETURN_T.BUSY:
                    RetVal = RECEIVE_RETURN_T.BUSY;
                    break;

                case RECEIVE_RETURN_T.OK_FRAME:
                    RetVal = RECEIVE_RETURN_T.OKAY;
                    break;

                default:
                    break;
            }

            return RetVal;
        }
        private bool GetAction(ref ACTION_T Action, ref string[] Parameter, bool ContinousMode)
        {
            bool Error;
            int Items;
            int Counter;

            Error = false;
            Counter = 0;

            if (ContinousMode)
            {
                Items = ComBufferContinous.Action.Length;
                if (ComBufferContinous.DataInBuffer > 0)
                {
                    Counter = 0;
                    Error = true;

                    while (Counter++ < ComBufferContinous.DataInBuffer)
                    {
                        if (ComBufferContinous.TargetTime[ComBufferContinous.ReadPtr] <= CommunicationTime.ElapsedMilliseconds)
                        {
                            Error = false;
                            Action = ComBufferContinous.Action[ComBufferContinous.ReadPtr];
                            for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
                            {
                                Parameter[Index] = ComBufferContinous.Parameter[ComBufferContinous.ReadPtr, Index];
                            }

                            ComBufferContinous.TargetTime[ComBufferContinous.ReadPtr] = CommunicationTime.ElapsedMilliseconds + ComBufferContinous.MinInterval_ms[ComBufferContinous.ReadPtr];

                            break;
                        }

                        if (ComBufferContinous.ReadPtr < (ComBufferContinous.DataInBuffer - 1))
                        {
                            ComBufferContinous.ReadPtr++;
                        }
                        else
                        {
                            ComBufferContinous.ReadPtr = 0;
                        }
                    }
                }
                else
                {
                    Error = true;
                }
            }
            else
            {
                Items = ComBuffer.Action.Length;
                if (ComBuffer.DataInBuffer > 0)
                {
                    Action = ComBuffer.Action[ComBuffer.ReadPtr];
                    ComBuffer.Action[ComBuffer.ReadPtr] = ACTION_T.NONE;
                    for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
                    {
                        Parameter[Index] = ComBuffer.Parameter[ComBuffer.ReadPtr, Index];
                        ComBuffer.Parameter[ComBuffer.ReadPtr, Index] = "";
                    }

                    if (ComBuffer.ReadPtr < (Items - 1))
                    {
                        ComBuffer.ReadPtr++;
                    }
                    else
                    {
                        ComBuffer.ReadPtr = 0;
                    }
                    ComBuffer.DataInBuffer--;
                }
                else
                {
                    Action = ACTION_T.NONE;
                    for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
                    {
                        Parameter[Index] = "";
                    }
                    Error = true;
                }
            }

            return Error;
        }
        private void Init()
        {
            Params.VersionSet = false;
            Params.UseDelimitersPrepared = false;

            if (SetupSerialDlg.SetupDone)
            {
                if (RCmanager.Properties.SerialSettings.Default.InitCode != Constants.INITCODE)
                {
                    RCmanager.Properties.SerialSettings.Default.InitCode = Constants.INITCODE;
                    RCmanager.Properties.SerialSettings.Default.Save();
                }
                // Setup Timer
                TaskTimer.Interval = RCmanager.Properties.SerialSettings.Default.TaskTiming_ms;
                TaskTimer.Enabled = true;

                // Open serial port
                OpenPort();

                // Init communication stack
                Init_Communication();

                // Start timer
                CommunicationTime.Start();
            }
        }
        public void Init_Communication()
        {
            Params.State = STATE_T.INIT;
            Params.ActionExecuting = ACTION_T.NONE;
            Params.ParameterExecuting = new string[Constants.ACTIONLIST_PARAMETERS];

            for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
            {
                Params.ParameterExecuting[Index] = "";
            }

            ReceiveData.ReceiveBuffer = new byte[64];
            ReceiveData.ReceiveData = "";
            ReceiveData.ReceivePtr = -1;
            ReceiveData.ExpectedReceiveBytes = 0;
            ReceiveData.Callback = RECEIVE_RETURN_T.BUSY;

            ComBuffer.ReadPtr = ComBuffer.WritePtr = ComBuffer.DataInBuffer = 0;
            ComBuffer.Action = new ACTION_T[Constants.ACTIONLIST_SIZE];
            ComBuffer.Parameter = new string[Constants.ACTIONLIST_SIZE, Constants.ACTIONLIST_PARAMETERS];
            ComBufferContinous.MinInterval_ms = new int[Constants.ACTIONLIST_SIZE];
            ComBufferContinous.TargetTime = new long[Constants.ACTIONLIST_SIZE];

            for (int Index = 0; Index < Constants.ACTIONLIST_SIZE; Index++)
            {
                ComBuffer.Action[Index] = ACTION_T.NONE;
                ComBufferContinous.MinInterval_ms[Index] = 0;
                ComBufferContinous.TargetTime[Index] = 0L;

                for (int SubIndex = 0; SubIndex < Constants.ACTIONLIST_PARAMETERS; SubIndex++)
                {
                    ComBuffer.Parameter[Index, SubIndex] = "";
                }
            }


            ComBufferContinous.ReadPtr = ComBufferContinous.WritePtr = ComBufferContinous.DataInBuffer = 0;
            ComBufferContinous.Action = new ACTION_T[Constants.ACTIONLIST_SIZE];
            ComBufferContinous.Parameter = new string[Constants.ACTIONLIST_SIZE, Constants.ACTIONLIST_PARAMETERS];
            for (int Index = 0; Index < Constants.ACTIONLIST_SIZE; Index++)
            {
                ComBufferContinous.Action[Index] = ACTION_T.NONE;
                for (int SubIndex = 0; SubIndex < Constants.ACTIONLIST_PARAMETERS; SubIndex++)
                {
                    ComBufferContinous.Parameter[Index, SubIndex] = "";
                }
            }
        }
        protected virtual void OnFrameReceived(FrameReceivedEventArgs e)
        {
            EventHandler<FrameReceivedEventArgs> handler = FrameReceived;
            handler?.Invoke(this, e);
        }
        public DialogResult Setup()
        {
            DialogResult Result;

            Result = DialogResult.OK;

            SetupSerialDlg.SmallVersion = Params.SmallVersion;

            Result = SetupSerialDlg.ShowDialog();

            switch (Result)
            {
                case DialogResult.OK:
                    OpenPort();
                    break;

                case DialogResult.Cancel:
                    break;

                 default:
                    break;
            }
            return Result;
        }
        public SerialCommunication(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public bool SetAction(ACTION_T Action, bool ContinousMode)
        {
            bool Error;
            int Items;

            Error = false;

            if (ContinousMode)
            {
                Items = ComBufferContinous.Action.Length;
                if (ComBufferContinous.DataInBuffer < Items)
                {
                    ComBufferContinous.Action[ComBufferContinous.WritePtr] = Action;
                    for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
                    {
                        ComBufferContinous.Parameter[ComBufferContinous.WritePtr, Index] = "";
                    }

                    ComBufferContinous.WritePtr++;
                    ComBufferContinous.DataInBuffer++;
                }
                else
                {
                    Error = true;
                }
            }
            else
            {
                Items = ComBuffer.Action.Length;
                if (ComBuffer.DataInBuffer < Items)
                {
                    ComBuffer.Action[ComBuffer.WritePtr] = Action;
                    for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
                    {
                        ComBuffer.Parameter[ComBuffer.WritePtr, Index] = "";
                    }

                    if (ComBuffer.WritePtr < (Items - 1))
                    {
                        ComBuffer.WritePtr++;
                    }
                    else
                    {
                        ComBuffer.WritePtr = 0;
                    }

                    ComBuffer.DataInBuffer++;
                    if (ComBuffer.DataInBuffer > Items)
                    {
                        ComBuffer.DataInBuffer = Items;
                        ComBuffer.ReadPtr = ComBuffer.WritePtr;
                    }
                }
                else
                {
                    Error = true;
                }
            }

            if (((ComBufferContinous.DataInBuffer + ComBuffer.DataInBuffer) > 0) && (TaskTimer.Enabled == false))
            {
                TaskTimer.Enabled = true;
            }
            if (((ComBufferContinous.DataInBuffer + ComBuffer.DataInBuffer) == 0) && (TaskTimer.Enabled == true))
            {
                TaskTimer.Enabled = false;
            }

            return Error;
        }
        public bool SetAction<S, T, U, V, W>(ACTION_T Action, S Param_1, T Param_2, U Param_3, V Param_4, W Param_5, bool ContinousMode)
        {
            // Overrun is exclusivly for single mode implemented
            bool Error;
            int Items;

            Error = false;

            if (ContinousMode)
            {
                Items = ComBufferContinous.Action.Length;
                if (ComBufferContinous.DataInBuffer < Items)
                {
                    ComBufferContinous.Action[ComBufferContinous.WritePtr] = Action;
                    ComBufferContinous.Parameter[ComBufferContinous.WritePtr, 0] = Param_1.ToString();
                    ComBufferContinous.Parameter[ComBufferContinous.WritePtr, 1] = Param_2.ToString();
                    ComBufferContinous.Parameter[ComBufferContinous.WritePtr, 2] = Param_3.ToString();
                    ComBufferContinous.Parameter[ComBufferContinous.WritePtr, 3] = Param_4.ToString();

                    ComBufferContinous.WritePtr++;
                    ComBufferContinous.DataInBuffer++;
                }
                else
                {
                    Error = true;
                }
            }
            else
            {
                Items = ComBuffer.Action.Length;
                if (ComBuffer.DataInBuffer < Items)
                {
                    ComBuffer.Action[ComBuffer.WritePtr] = Action;
                    ComBuffer.Parameter[ComBuffer.WritePtr, 0] = Param_1.ToString();
                    ComBuffer.Parameter[ComBuffer.WritePtr, 1] = Param_2.ToString();
                    ComBuffer.Parameter[ComBuffer.WritePtr, 2] = Param_3.ToString();
                    ComBuffer.Parameter[ComBuffer.WritePtr, 3] = Param_4.ToString();
                    ComBuffer.Parameter[ComBuffer.WritePtr, 4] = Param_5.ToString();

                    if (ComBuffer.WritePtr < (Items - 1))
                    {
                        ComBuffer.WritePtr++;
                    }
                    else
                    {
                        ComBuffer.WritePtr = 0;
                    }

                    ComBuffer.DataInBuffer++;
                    if (ComBuffer.DataInBuffer > Items)
                    {
                        ComBuffer.DataInBuffer = Items;
                        ComBuffer.ReadPtr = ComBuffer.WritePtr;
                    }
                }
                else
                {
                    Error = true;
                }
            }
            if (((ComBufferContinous.DataInBuffer + ComBuffer.DataInBuffer) > 0) && (TaskTimer.Enabled == false))
            {
                TaskTimer.Enabled = true;
            }
            if (((ComBufferContinous.DataInBuffer + ComBuffer.DataInBuffer) == 0) && (TaskTimer.Enabled == true))
            {
                TaskTimer.Enabled = false;
            }

            return Error;
        }
        public bool SetAction<T, U, V, W>(ACTION_T Action, T Param_1, U Param_2, V Param_3, W Param_4, bool ContinousMode, int MinInterval_ms)
        {
            bool Error;
            int Items;

            Error = false;

            if (ContinousMode)
            {
                Items = ComBufferContinous.Action.Length;
                if (ComBufferContinous.DataInBuffer < Items)
                {
                    ComBufferContinous.Action[ComBufferContinous.WritePtr] = Action;
                    ComBufferContinous.Parameter[ComBufferContinous.WritePtr, 0] = Param_1.ToString();
                    ComBufferContinous.Parameter[ComBufferContinous.WritePtr, 1] = Param_2.ToString();
                    ComBufferContinous.Parameter[ComBufferContinous.WritePtr, 2] = Param_3.ToString();
                    ComBufferContinous.Parameter[ComBufferContinous.WritePtr, 3] = Param_4.ToString();
                    ComBufferContinous.MinInterval_ms[ComBufferContinous.WritePtr] = MinInterval_ms;

                    ComBufferContinous.WritePtr++;
                    ComBufferContinous.DataInBuffer++;
                }
                else
                {
                    Error = true;
                }
            }
            else
            {
                Items = ComBuffer.Action.Length;
                if (ComBuffer.DataInBuffer < Items)
                {
                    ComBuffer.Action[ComBuffer.WritePtr] = Action;
                    ComBuffer.Parameter[ComBuffer.WritePtr, 0] = Param_1.ToString();
                    ComBuffer.Parameter[ComBuffer.WritePtr, 1] = Param_2.ToString();
                    ComBuffer.Parameter[ComBuffer.WritePtr, 2] = Param_3.ToString();
                    ComBuffer.Parameter[ComBuffer.WritePtr, 3] = Param_4.ToString();

                    if (ComBuffer.WritePtr < (Items - 1))
                    {
                        ComBuffer.WritePtr++;
                    }
                    else
                    {
                        ComBuffer.WritePtr = 0;
                    }

                    ComBuffer.DataInBuffer++;
                    if (ComBuffer.DataInBuffer > Items)
                    {
                        ComBuffer.DataInBuffer = Items;
                        ComBuffer.ReadPtr = ComBuffer.WritePtr;
                    }
                }
                else
                {
                    Error = true;
                }
            }
            if (((ComBufferContinous.DataInBuffer + ComBuffer.DataInBuffer) > 0) && (TaskTimer.Enabled == false))
            {
                TaskTimer.Enabled = true;
            }
            if (((ComBufferContinous.DataInBuffer + ComBuffer.DataInBuffer) == 0) && (TaskTimer.Enabled == true))
            {
                TaskTimer.Enabled = false;
            }

            return Error;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            CommunicationTask();
        }
    }
    public class FrameReceivedEventArgs : EventArgs
    {
        public SerialCommunication.PARAMS_T Parameter { get; set; }
        //public string ReceivedFrame { get; set; }
    }
    static class Constants
    {
        public const uint INITCODE =                0x55AA55A0;
        public const byte ACTIONLIST_PARAMETERS =   5;
        public const int ACTIONLIST_SIZE =          20;
    }
}
