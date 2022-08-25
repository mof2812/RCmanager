using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelayCard16
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
        //SET_COUNTER_PS,
        SET_ENABLE,
        SET_ENABLE_PS,
        SET_MODE,
        //SET_MODE_PS,
    }
    #endregion
    public partial class RelayCard16 : UserControl
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
        }
        public struct PARAMS_T
        {
            public STATE_T State;
            public ACTION_T ActionExecuting;
            public byte ChannelExecuting;
            public string[] ParameterExecuting;
            public string ReceivedData;

        }
        public struct COM_BUFFERT_T
        {
            public ACTION_T[] Action;
            public byte[] Channel;
            public string[,] Parameter;
            public int ReadPtr;
            public int WritePtr;
            public int DataInBuffer;
        }
        #endregion
        #region Members
        private PARAMS_T Params;
        private COM_BUFFERT_T ComBuffer;
        private COM_BUFFERT_T ComBufferContinous;
        #endregion
        #region Constructor, Destructor
        #endregion
        #region Methods
        public bool ClearAction(ACTION_T Action, byte Channel = 0)
        {
            bool Error;
            int Items;
            int Index;

            Error = true;
            Items = ComBufferContinous.Action.Length;

            for (Index = 0; Index < Items; Index++)
            {
                if ((ComBufferContinous.Action[Index] == Action) && (ComBufferContinous.Channel[Index] == Channel))
                {
                    ComBufferContinous.Action[Index] = ComBufferContinous.Action[ComBufferContinous.DataInBuffer - 1];
                    ComBufferContinous.Action[ComBufferContinous.DataInBuffer - 1] = ACTION_T.NONE;
                    ComBufferContinous.Channel[Index] = ComBufferContinous.Channel[ComBufferContinous.DataInBuffer - 1];
                    ComBufferContinous.Channel[ComBufferContinous.DataInBuffer - 1] = 0xFF;
                    for (int Index2 = 0; Index2 < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index2++)
                    {
                        ComBufferContinous.Parameter[ComBufferContinous.DataInBuffer - 1, Index] = "";
                    }
                    ComBufferContinous.DataInBuffer--;
                    ComBufferContinous.WritePtr = ComBufferContinous.DataInBuffer;
                    if (ComBufferContinous.ReadPtr > ComBufferContinous.DataInBuffer)
                    {
                        ComBufferContinous.ReadPtr = Index;
                    }
                    Error = false;
                    break;
                }
            }
            if (!Error)
            {

            }

            return Error;
        }
        private bool CommunicationTask()
        {
            bool NewData;
            bool StayInLoop;
            int Channel;
            RelayControl.RelayControl RC = new RelayControl.RelayControl();
            RelayControl.STATE_T ReturnState;

            Channel = 0;
            NewData = false;

            if (CommunicationOn == true)
            {
                Channel = 0;
                while (Channel < 16)
                {
                    RC = GetRelayControl(Channel);

                    ReturnState = RC.CommunicationTask();

                    if ((ReturnState == RelayControl.STATE_T.OKAY) || (ReturnState == RelayControl.STATE_T.ERROR) || (ReturnState == RelayControl.STATE_T.NO_DATA))
                    {
                        Channel++;
                    }
                }
            }
//            return NewData;
            NewData = false;

            if (CommunicationOn == false)
            {
                Params.State = STATE_T.INIT;
            }
            else
            {

                do
                {
                    StayInLoop = false;

                    switch (Params.State)
                    {
                        case STATE_T.INIT:
                            Params.State = STATE_T.IDLE;
                            StayInLoop = true;
                            break;

                        case STATE_T.IDLE:
                            if (ComBuffer.DataInBuffer > 0)
                            {
                                if (GetAction(ref Params.ActionExecuting, ref Params.ChannelExecuting, ref Params.ParameterExecuting, false) == false)
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
                                if (GetAction(ref Params.ActionExecuting, ref Params.ChannelExecuting, ref Params.ParameterExecuting, true) == false)
                                {
                                    Params.State = STATE_T.SEND;
                                }
                                else
                                {
                                    Params.State = STATE_T.ERROR;
                                }
                                StayInLoop = true;
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
                            Params.State = CommunicationTask_Evaluate(Params) ? STATE_T.ERROR : STATE_T.OKAY;
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

            return NewData;
        }
        private bool CommunicationTask_Evaluate(PARAMS_T Params)
        {
            bool Error;
            RelayControl.RelayControl RC;

            Error = false;
            RC = GetRelayControl(Params.ChannelExecuting);

            switch (Params.ActionExecuting)
            {
                //case ACTION_T.GET_CONFIG:
                //    CommunicationTask_EvaluateConfiguration(Params, RC);
                //    RC.DrawGraph();
                //    RC.XRange = 5000;
                //    break;

                //case ACTION_T.GET_COUNTER:
                ////case ACTION_T.GET_COUNTER_PS:
                //    RC.Params.ImpulseCounter = Convert.ToInt32(Params.ReceivedData);
                //    break;

                //case ACTION_T.GET_MODE:
                //    RC.Params.Mode = (RelayControl.MODE_T)Convert.ToInt32(Params.ReceivedData);
                //    break;

                case ACTION_T.GET_VERSION:
                    RCData.HW_Version = Params.ReceivedData;
                    break;

                //case ACTION_T.SET_COUNTER:
                ////case ACTION_T.SET_COUNTER_PS:
                //case ACTION_T.SET_MODE:
                ////case ACTION_T.SET_MODE_PS:
                    break;

                default:
                    Error = true;
                    break;
            }

            return Error;
        }
/*        private bool CommunicationTask_EvaluateConfiguration(PARAMS_T Params, RelayControl.RelayControl RC)
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
                        RC.Params.Enabled = Params.ReceivedData.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 1:
                        RC.Params.Mode = (RelayControl.MODE_T)Convert.ToInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 2:
                        RC.Params.TimeOn_ms = Convert.ToUInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 3:
                        RC.Params.TimeOff_ms = Convert.ToUInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 4:
                        RC.Params.DelayTime_ms = Convert.ToUInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 5:
                        RC.Params.ImpulseCounter = Convert.ToInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 6:
                        RC.Params.ImpulseCounterActual = Convert.ToInt32(Params.ReceivedData.Substring(0, Pos));
                        break;

                    case 7:
                        RC.Params.AsynchronousMode = Params.ReceivedData.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 8:
                        RC.Params.ImmediateMode = Params.ReceivedData.Substring(0, Pos) == "0" ? false : true;
                        break;

                    case 9:
                        RC.Params.InvertedMode = Params.ReceivedData == "0" ? false : true;
                        break;

                    default:
                        Error = true;
                        break;
                }

                Params.ReceivedData = Params.ReceivedData.Remove(0, Pos + 1);
            }

            return Error;
        }
*/
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
            RelayControl.RelayControl RC;
            string Frame;
            byte Channel;

            Error = false;
            Channel = Data.ChannelExecuting;
            RC = GetRelayControl(Channel);
            Channel = (byte)(Channel % 8);
            Frame = "";

            switch (Data.ActionExecuting)
            {
                case ACTION_T.GET_CONFIG:
                    Frame = Channel < 8 ? $"GSWCONFIG {Channel}" : $"GPSPCONFIG {Channel%8}";
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
                        RC.Params.ImpulseCounter = Convert.ToInt32(Data.ParameterExecuting[0]);
                    }
#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    //Frame = $"SSWC {Channel} {RC.Params.ImpulseCounter}";
                    Frame = Channel < 8 ? $"SSWC {Channel} {RC.Params.ImpulseCounter}" : $"SPSPC {Channel % 8} {RC.Params.ImpulseCounter}";
#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;

                //case ACTION_T.SET_COUNTER_PS:
                //    Frame = $"SPSPC {Channel} {RC.Params.ImpulseCounter}";
                //    break;

                case ACTION_T.SET_MODE:
                    if (Data.ParameterExecuting[0].Length > 0)
                    {
                        RC.Params.Mode = RC.StringToMode(Data.ParameterExecuting[0]);
                    }
#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = $"SSWM {Channel} {RC.Params.Mode.ToString()}";
                    Frame = Channel < 8 ? $"SSWM {Channel} {RC.Params.Mode}" : $"SPSPM {Channel % 8} {RC.Params.Mode}";
#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;

                //case ACTION_T.SET_MODE_PS:
                //    if (Data.ParameterExecuting[0].Length > 0)
                //    {
                //        RC.Params.Mode = RC.StringToMode(Data.ParameterExecuting[0]);
                //    }
                //    Frame = $"SPSPM {Channel} {RC.Params.Mode.ToString()}";
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
        private bool GetAction(ref ACTION_T Action, ref byte Channel, ref string[] Parameter, bool ContinousMode)
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
                    Channel = ComBufferContinous.Channel[ComBufferContinous.ReadPtr];
                    for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index++)
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
                    Channel = ComBuffer.Channel[ComBuffer.ReadPtr];
                    ComBuffer.Channel[ComBuffer.ReadPtr] = 0xFF;
                    for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index++)
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
                    Channel = 0xFF;
                    for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index++)
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
                    Params.ChannelExecuting = ComBufferContinous.Channel[ComBufferContinous.ReadPtr];
                    for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index++)
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
                    Params.ChannelExecuting = ComBuffer.Channel[ComBuffer.ReadPtr];
                    ComBuffer.Channel[ComBuffer.ReadPtr] = 0xFF;
                    for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index++)
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
                    Params.ChannelExecuting = 0xFF;
                    for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index++)
                    {
                        Params.ParameterExecuting[Index] = "";
                    }
                    Error = true;
                }
            }

            return Error;
        }
        private void Init_Communication()
        {
            byte Channel;

            Channel = 255;

            Params.State = STATE_T.INIT;
            Params.ActionExecuting = ACTION_T.NONE;
            Params.ChannelExecuting = 0xFF;
            Params.ParameterExecuting = new string[RelayControl.Constants.ACTIONLIST_PARAMETERS];
            for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index++)
            {
                Params.ParameterExecuting[Index] = "";
            }

            ComBuffer.ReadPtr = ComBuffer.WritePtr = ComBuffer.DataInBuffer = 0;
            ComBuffer.Action = new ACTION_T[RelayControl.Constants.ACTIONLIST_SIZE];
            ComBuffer.Channel = new byte[RelayControl.Constants.ACTIONLIST_SIZE];
            ComBuffer.Parameter = new string[RelayControl.Constants.ACTIONLIST_SIZE, RelayControl.Constants.ACTIONLIST_PARAMETERS];
            for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_SIZE; Index++)
            {
                ComBuffer.Action[Index] = ACTION_T.NONE;
                ComBuffer.Channel[Index] = 0;
                for (int SubIndex = 0; SubIndex < RelayControl.Constants.ACTIONLIST_PARAMETERS; SubIndex++)
                {
                    ComBuffer.Parameter[Index, SubIndex] = "";
                }
            }

            ComBufferContinous.ReadPtr = ComBufferContinous.WritePtr = ComBufferContinous.DataInBuffer = 0;
            ComBufferContinous.Action = new ACTION_T[RelayControl.Constants.ACTIONLIST_SIZE];
            ComBufferContinous.Channel = new byte[RelayControl.Constants.ACTIONLIST_SIZE];
            ComBufferContinous.Parameter = new string[RelayControl.Constants.ACTIONLIST_SIZE, RelayControl.Constants.ACTIONLIST_PARAMETERS];
            for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_SIZE; Index++)
            {
                ComBufferContinous.Action[Index] = ACTION_T.NONE;
                ComBufferContinous.Channel[Index] = 0;
                for (int SubIndex = 0; SubIndex < RelayControl.Constants.ACTIONLIST_PARAMETERS; SubIndex++)
                {
                    ComBufferContinous.Parameter[Index, SubIndex] = "";
                }
            }

            /* For testing only */
            bool Continous = false;
            ACTION_T Action = ACTION_T.NONE;
            Channel = 255;
            //SetAction(ACTION_T.GET_CONFIG, 0, Continous);
            //SetAction(ACTION_T.GET_CONFIG, 1, Continous);
            SetAction(ACTION_T.GET_VERSION, Continous);
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

            if (Continous)
            {
                GetAction(ref Params, Continous);
                GetAction(ref Params, Continous);
                GetAction(ref Params, Continous);
                GetAction(ref Params, Continous);
            }
            //ClearAction(ACTION_T.GET_COUNTER, 2);
            //ClearAction(ACTION_T.GET_MODE_PS, 3);
            //SetAction(ACTION_T.GET_MODE, 10, Continous);
            //SetAction(ACTION_T.GET_MODE, 11, Continous);

            if (Continous)
            {
                GetAction(ref Params, Continous);
                GetAction(ref Params, Continous);
                GetAction(ref Params, Continous);
                GetAction(ref Params, Continous);
            }
            /* End - For testing only */

            CommunicationTimer.Enabled = true;

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
                    for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index++)
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
                    for (int Index = 0; Index < RelayControl.Constants.ACTIONLIST_PARAMETERS; Index++)
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
        public bool SetAction(ACTION_T Action, byte Channel, bool ContinousMode)
        {
            bool Error;

            Error = false;

            if ((Channel / 8) == 0)
            {
                rcCard1.GetRelayControl(Channel % 8).SetAction((RelayControl.ACTION_T)Action, ContinousMode);
            }
            else
            {
                rcCard1.GetRelayControl(Channel % 8).SetAction((RelayControl.ACTION_T)Action, ContinousMode);
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
        public bool SetAction<T,U,V,W>(ACTION_T Action, byte Channel, T Param_1, U Param_2, V Param_3, W Param_4, bool ContinousMode)
        {
            bool Error;

            Error = false;

            if ((Channel / 8) == 0)
            {
                rcCard1.GetRelayControl(Channel % 8).SetAction((RelayControl.ACTION_T)Action, Param_1, Param_2, Param_3, Param_4, ContinousMode);
            }
            else
            {
                rcCard1.GetRelayControl(Channel % 8).SetAction((RelayControl.ACTION_T)Action, Param_1, Param_2, Param_3, Param_4, ContinousMode);
            }
            return Error;
        }

        #endregion
    }
}
