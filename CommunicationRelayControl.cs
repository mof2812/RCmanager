using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace RelayControl
{
    #region Structs and Enums
    public enum ACTION_T
    {
        NONE = 0,
        GET_CONFIG,
        GET_COUNTER,
        GET_COUNTER_PS,
        GET_ENABLE,
        GET_ENABLE_PS,
        GET_MODE,
        GET_MODE_PS,
        GET_VERSION,
        SET_COUNTER,
        SET_ENABLE,
        SET_ENABLE_PS,
        SET_MODE,
    }
    public enum STATE_T
    {
        INIT = 0,
        IDLE,
        SEND,
        RECEIVE,
        EVALUATE,
        ERROR,
        OKAY,
        NO_DATA,    /* for return only */
    }
    public struct SERIAL_PARAMS_T
    {
        public byte[] ReceiveBuffer;
        public string ReceiveData;
        public int ReceivePtr;
        public RECEIVE_RETURN_T Callback;
        public int ExpectedReceiveBytes;
    }
    #endregion
    public partial class RelayControl : UserControl
    {
        #region Structs and Enums
      //public struct PARAMS_T
        //{
        //    public STATE_T State;
        //    public ACTION_T ActionExecuting;
        //    public byte ChannelExecuting;
        //    public string[] ParameterExecuting;
        //    public string ReceivedData;

        //}
        public struct COM_BUFFERT_T
        {
            public ACTION_T[] Action;
            public string[,] Parameter;
            public int ReadPtr;
            public int WritePtr;
            public int DataInBuffer;
        }
        #endregion
        #region Members
        public bool CommunicationOn = false;
        private SERIAL_PARAMS_T SParams;
        private COM_BUFFERT_T ComBuffer;
        private COM_BUFFERT_T ComBufferContinous;
        private Stopwatch Timeout = new Stopwatch();
        #endregion

        public STATE_T CommunicationTask()
        {
            STATE_T RetVal;
            bool NewData;
            bool StayInLoop;

            NewData = false;

            if (false)
            {
                Params.State = STATE_T.INIT;
            }
            else
            {

                do
                {
                    StayInLoop = false;
                    RetVal = Params.State;

                    switch (Params.State)
                    {
                        case STATE_T.INIT:
                            Params.State = STATE_T.IDLE;
                            StayInLoop = true;
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
                            Params.State = CommunicationTask_Send(Params) == false ? STATE_T.RECEIVE : STATE_T.ERROR;
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
                                Params.State = STATE_T.ERROR;
                            }
                            break;

                        case STATE_T.EVALUATE:
                            Params.State = CommunicationTask_Evaluate(ref Params) ? STATE_T.ERROR : STATE_T.OKAY;
                            break;

                        case STATE_T.ERROR:
                            Params.State = STATE_T.INIT;
                            break;

                        case STATE_T.OKAY:
                            NewData = true;
                            Params.State = STATE_T.IDLE;
                            break;

                        default:
                            Params.State = STATE_T.INIT;
                            break;
                    }

                } while (StayInLoop == true);
            }

            return RetVal;
        }
        private bool CommunicationTask_Evaluate(ref PARAMS_T Params)
        {
            bool Error;

            Error = false;

            switch (Params.ActionExecuting)
            {
                case ACTION_T.GET_CONFIG:
                    CommunicationTask_EvaluateConfiguration(ref Params);
                    DrawGraph();
                    XRange = 5000;
                    break;

                case ACTION_T.GET_COUNTER:
                    //case ACTION_T.GET_COUNTER_PS:
                    Params.ImpulseCounter = Convert.ToInt32(Params.ReceivedData);
                    break;

                case ACTION_T.GET_MODE:
                    Params.Mode = (MODE_T)Convert.ToUInt32(Params.ReceivedData);
                    break;

                case ACTION_T.SET_COUNTER:
                //case ACTION_T.SET_COUNTER_PS:
                case ACTION_T.SET_MODE:
                    //case ACTION_T.SET_MODE_PS:
                    break;

                default:
                    Error = true;
                    break;
            }

            return Error;
        }
        private bool CommunicationTask_EvaluateConfiguration(ref PARAMS_T Params)
        {
            bool Error;
            int Pos;

            Error = false;
            Pos = 0;

            for (int Index = 0; Index < 10; Index++)
            {
                Pos = Params.ReceivedData.IndexOf(" ");
                switch (Index)
                {
                    case 0:
                        Params.Enabled = Params.ReceivedData.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 1:
                        Params.Mode = (MODE_T)Convert.ToInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 2:
                        Params.TimeOn_ms = Convert.ToUInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 3:
                        Params.TimeOff_ms = Convert.ToUInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 4:
                        Params.DelayTime_ms = Convert.ToUInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 5:
                        Params.ImpulseCounter = Convert.ToInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 6:
                        Params.ImpulseCounterActual = Convert.ToInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 7:
                        Params.AsynchronousMode = Params.ReceivedData.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 8:
                        Params.ImmediateMode = Params.ReceivedData.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 9:
                        Params.InvertedMode = Params.ReceivedData == "0" ? false : true;
                        break;

                    case 10:
                        Params.TriggerMode = Params.ReceivedData == "0" ? false : true;
                        break;

                    default:
                        Error = true;
                        break;
                }

                Params.ReceivedData = Params.ReceivedData.Remove(0, Pos + 1);
            }

            return Error;
        }
        private RECEIVE_RETURN_T CommunicationTask_Receive(PARAMS_T Params)
        {
            bool Error;
            string Data;
            RECEIVE_RETURN_T Callback;


            Callback = RECEIVE_RETURN_T.ERROR;
            Error = false;
            Data = "";

            switch (Params.ActionExecuting)
            {
                case ACTION_T.GET_CONFIG:
                case ACTION_T.GET_COUNTER:
                //case ACTION_T.GET_COUNTER_PS:
                case ACTION_T.GET_MODE:
                case ACTION_T.GET_VERSION:
                    Callback = CommunicationTask_WaitForData();
                    if (Callback == RECEIVE_RETURN_T.DATA_FRAME)
                    {
                        Callback = RECEIVE_RETURN_T.OKAY;
                    }
                    break;

                case ACTION_T.SET_COUNTER:
                //case ACTION_T.SET_COUNTER_PS:
                case ACTION_T.SET_MODE:
                    //case ACTION_T.SET_MODE_PS:
                    Callback = CommunicationTask_WaitForData();
                    if (Callback == RECEIVE_RETURN_T.OK_FRAME)
                    {
                        Callback = RECEIVE_RETURN_T.OKAY;
                    }
                    break;
                default:
                    break;
            }

            if ((Callback != RECEIVE_RETURN_T.OKAY) && (Callback != RECEIVE_RETURN_T.BUSY))
            {
                Callback = RECEIVE_RETURN_T.ERROR;
            }
            return Callback;
        }
        private bool CommunicationTask_Send(PARAMS_T Data)
        {
            bool Error;
            string Frame;
            byte Channel;

            Error = false;
            Channel = Params.Channel;
            Frame = "";

            switch (Data.ActionExecuting)
            {
                case ACTION_T.GET_CONFIG:
                    Frame = Channel < 8 ? $"GSWCONFIG {Channel}" : $"GPSPCONFIG {Channel % 8}";
                    break;

                case ACTION_T.GET_COUNTER:
                    //Frame = $"GSWC {Channel}";
                    Frame = Channel < 8 ? $"GSWC {Channel}" : $"GPSPC {Channel % 8}";
                    break;

                //case ACTION_T.GET_COUNTER_PS:
                //    Frame = $"GPSPC {Channel}";
                //    break;

                case ACTION_T.GET_MODE:
                    //Frame = $"GSWM {Channel}";
                    Frame = Channel < 8 ? $"GSWM {Channel}" : $"GPSPM {Channel % 8}";
                    break;

                case ACTION_T.GET_VERSION:
                    Frame = $"VERSION";
                    break;

                case ACTION_T.SET_COUNTER:
                    if (Data.ParameterExecuting[0].Length > 0)
                    {
                        Params.ImpulseCounter = Convert.ToInt32(Data.ParameterExecuting[0]);
                    }
#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    //Frame = $"SSWC {Channel} {Params.ImpulseCounter}";
                    Frame = Channel < 8 ? $"SSWC {Channel} {Params.ImpulseCounter}" : $"SPSPC {Channel % 8} {Params.ImpulseCounter}";
#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;

                //case ACTION_T.SET_COUNTER_PS:
                //    Frame = $"SPSPC {Channel} {Params.ImpulseCounter}";
                //    break;

                case ACTION_T.SET_MODE:
                    if (Data.ParameterExecuting[0].Length > 0)
                    {
                        Params.Mode = StringToMode(Data.ParameterExecuting[0]);
                    }
#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = $"SSWM {Channel} {Params.Mode.ToString()}";
                    Frame = Channel < 8 ? $"SSWM {Channel} {Params.Mode}" : $"SPSPM {Channel % 8} {Params.Mode}";
#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;

                //case ACTION_T.SET_MODE_PS:
                //    if (Data.ParameterExecuting[0].Length > 0)
                //    {
                //        Params.Mode = RC.StringToMode(Data.ParameterExecuting[0]);
                //    }
                //    Frame = $"SPSPM {Channel} {Params.Mode.ToString()}";
                //    break;

                default:
                    Error = true;
                    break;
            }

            if (!Error)
            {
                //Frame = $"{(char)0x02}{Frame}{(char)0x03}";
                Write(Frame);
            }
            return Error;
        }
        private RECEIVE_RETURN_T CommunicationTask_WaitForData()
        {
            RECEIVE_RETURN_T RetVal;

            RetVal = RECEIVE_RETURN_T.ERROR;

            switch (ReceiveFrame(ref Params.ReceivedData))
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
        private bool GetAction(ref ACTION_T Action, ref string[] Parameter, bool ContinousMode)
        {
            bool Error;
            int Items;

            Error = false;

            if (ContinousMode)
            {
                Items = ComBufferContinous.Action.Length;
                if (ComBufferContinous.DataInBuffer > 0)
                {
                    Action = ComBufferContinous.Action[ComBufferContinous.ReadPtr];
                    for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
                    {
                        Parameter[Index] = ComBufferContinous.Parameter[ComBufferContinous.ReadPtr, Index];
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
        private bool GetAction(ref PARAMS_T Params, bool ContinousMode)
        {
            bool Error;
            int Items;

            Error = false;

            if (ContinousMode)
            {
                Items = ComBufferContinous.Action.Length;
                if (ComBufferContinous.DataInBuffer > 0)
                {
                    Params.ActionExecuting = ComBufferContinous.Action[ComBufferContinous.ReadPtr];
                    for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
                    {
                        Params.ParameterExecuting[Index] = ComBufferContinous.Parameter[ComBufferContinous.ReadPtr, Index];
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
                    Params.ActionExecuting = ComBuffer.Action[ComBuffer.ReadPtr];
                    ComBuffer.Action[ComBuffer.ReadPtr] = ACTION_T.NONE;
                    for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
                    {
                        Params.ParameterExecuting[Index] = ComBuffer.Parameter[ComBuffer.ReadPtr, Index];
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
                    Params.ActionExecuting = ACTION_T.NONE;
                    for (int Index = 0; Index < Constants.ACTIONLIST_PARAMETERS; Index++)
                    {
                        Params.ParameterExecuting[Index] = "";
                    }
                    Error = true;
                }
            }

            return Error;
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

            SParams.ReceiveBuffer = new byte[64];
            SParams.ReceiveData = "";
            SParams.ReceivePtr = -1;
            SParams.ExpectedReceiveBytes = 0;
            SParams.Callback = RECEIVE_RETURN_T.BUSY;

            ComBuffer.ReadPtr = ComBuffer.WritePtr = ComBuffer.DataInBuffer = 0;
            ComBuffer.Action = new ACTION_T[Constants.ACTIONLIST_SIZE];
            ComBuffer.Parameter = new string[Constants.ACTIONLIST_SIZE, Constants.ACTIONLIST_PARAMETERS];
            for (int Index = 0; Index < Constants.ACTIONLIST_SIZE; Index++)
            {
                ComBuffer.Action[Index] = ACTION_T.NONE;
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

            /* For testing only */
            bool Continous = false;
            ACTION_T Action = ACTION_T.NONE;
            byte Channel = 255;
            //SetAction(ACTION_T.GET_CONFIG, 0, Continous);
            //SetAction(ACTION_T.GET_CONFIG, 1, Continous);
            //SetAction(ACTION_T.GET_VERSION, 1, Continous);
            //SetAction(ACTION_T.SET_COUNTER, 1, Continous);
            //SetAction(ACTION_T.SET_COUNTER_PS, 10, Continous);
            //SetAction(ACTION_T.GET_COUNTER, 1, Continous);
            //SetAction(ACTION_T.GET_COUNTER_PS, 10, Continous);
            //SetAction(ACTION_T.SET_MODE, 1, "TOGGLE", 0, 0, 0, Continous);
            //SetAction(ACTION_T.SET_MODE_PS, 10, Continous);
            //SetAction(ACTION_T.GET_MODE, 1, Continous);
            //SetAction(ACTION_T.GET_COUNTER, 2, Continous);
            //SetAction(ACTION_T.GET_MODE_PS, 10, Continous);
            //SetAction(ACTION_T.GET_COUNTER_PS, 10, Continous);
            //SetAction(ACTION_T.GET_ENABLE, 5, Continous);
            //SetAction(ACTION_T.GET_ENABLE_PS, 10, Continous);

            //if (Continous)
            //{
            //    GetAction(ref Params, Continous);
            //    GetAction(ref Params, Continous);
            //    GetAction(ref Params, Continous);
            //    GetAction(ref Params, Continous);
            //}
            //ClearAction(ACTION_T.GET_COUNTER, 2);
            //ClearAction(ACTION_T.GET_MODE_PS, 3);
            //SetAction(ACTION_T.GET_MODE, 10, Continous);
            //SetAction(ACTION_T.GET_MODE, 11, Continous);

            //if (Continous)
            //{
            //    GetAction(ref Params, Continous);
            //    GetAction(ref Params, Continous);
            //    GetAction(ref Params, Continous);
            //    GetAction(ref Params, Continous);
            //}
            /* End - For testing only */
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
                if (Params.ComPort.IsOpen == true)
                {
                    try
                    {
                        if (Timeout.ElapsedMilliseconds < 500L)
                        {
                            try
                            {
                                BytesToRead = Params.ComPort.BytesToRead;

                                try
                                {
                                    if (BytesToRead > 0)
                                    {
                                        BytesRead = Params.ComPort.Read(Data, 0, BytesToRead);
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

                                            Params.ComPort.DiscardInBuffer();

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
                                Params.ComPort.DiscardInBuffer();
                            }
                        }
                        else
                        {
                            long TimeoutGetData = 500L;
                            RetVal = RECEIVE_RETURN_T.TIMEOUT;
                            Params.ComPort.DiscardInBuffer();
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
            return Error;
        }
        public bool SetAction<T, U, V, W>(ACTION_T Action, T Param_1, U Param_2, V Param_3, W Param_4, bool ContinousMode)
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
            return Error;
        }
        private bool Write(String Text)
        {
            bool Error;
            String Frame2Send = "";

            Error = false;
            Frame2Send = (char)0x02 + Text + (char)0x03;

            try
            {
                Params.ComPort.Write(Frame2Send);
                Params.ComPort.ReadTimeout = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Error = true;
            }

            //            Debug_AddSendFrame(Command, RetVal == RETURN_T.OKAY);

            return Error;
        }

    }
    static class Constants
    {
        public const int ACTIONLIST_SIZE = 20;
        public const byte ACTIONLIST_PARAMETERS = 4;
    }


}
