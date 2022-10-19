using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunication
{
    public enum ACTION_T
    {
        NONE = 0,
        ENABLE_ALL,
        GET_CONFIG,
        GET_COUNTER,
        GET_ENABLE,
        GET_INVERTINGMODE,
        GET_MODE,
        GET_OUTPUT_STATES,
        GET_TRIGGER_CONFIG,
        GET_VERSION,
        SET_ASYNCHRONOUS_MODE,
        SET_CONFIG,
        SET_COUNTER,
        SET_DELAY_TIME_MS,
        SET_ENABLE,
        SET_ENABLE_CARD,
        SET_IMMEDIATE_MODE,
        SET_INVERTING_MODE,
        SET_MODE,
        SET_TIME_OFF_MS,
        SET_TIME_ON_MS,
        SET_TRIGGER_CONFIG,
        SET_TRIGGER,
    }
    public partial class SerialCommunication : Component
    {
        public event EventHandler<SetConfigEventArgs> SetConfig;
        
        Relay.SETTINGS_T RelaySettings = new Relay.SETTINGS_T();

        protected virtual void OnSetConfig(SetConfigEventArgs e)
        {
            EventHandler<SetConfigEventArgs> handler = SetConfig;
            handler?.Invoke(this, e);
        }
        public RECEIVE_WAIT_FOR_T Receive(ACTION_T Action)
        {
            RECEIVE_WAIT_FOR_T WaitFor;

            WaitFor = RECEIVE_WAIT_FOR_T.NONE;

            switch (Action)
            {
                #region USER_PART
                case ACTION_T.ENABLE_ALL:
                case ACTION_T.SET_ENABLE:
                case ACTION_T.SET_ENABLE_CARD:
                case ACTION_T.SET_CONFIG:
                case ACTION_T.SET_TRIGGER:
                case ACTION_T.SET_TRIGGER_CONFIG:
                    WaitFor = RECEIVE_WAIT_FOR_T.OK;
                    break;
                case ACTION_T.GET_CONFIG:
                case ACTION_T.GET_TRIGGER_CONFIG:
                    WaitFor = RECEIVE_WAIT_FOR_T.DATA;
                    break;
                case ACTION_T.GET_OUTPUT_STATES:
                    WaitFor = RECEIVE_WAIT_FOR_T.DATA;
                    break;
                #endregion
                default:
                    break;
            }
            return WaitFor;
        }
        public string Send(PARAMS_T Params)
        {
            string Frame;
            byte Channel;

            Frame = "";
            if (Params.ParameterExecuting[0] != "")
            {
                Channel = Convert.ToByte(Params.ParameterExecuting[0]);
            }
            else
            {
                Channel = 0;
            }

            switch (Params.ActionExecuting)
            {
                #region USER_PART
                #region ACTION_T.ENABLE_ALL
                case ACTION_T.ENABLE_ALL:
                    if (Params.ParameterExecuting[1].Length > 0)
                    {
                        if (Params.ParameterExecuting[1] != "0")
                        {
                            Params.ParameterExecuting[1] = "1";
                        }
                    }
                    else
                    {
                        Params.ParameterExecuting[1] = "0";
                    }
                    //#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = $"EA {Params.ParameterExecuting[1]} ";
                    //#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;
                #endregion
                #region ACTION_T.GET_CONFIG
                case ACTION_T.GET_CONFIG:
                    Frame = Channel < 8 ? $"GSWCONFIG {Channel}" : $"GPSPCONFIG {Channel % 8}";
                    break;
                #endregion
                #region ACTION_T.GET_TRIGGER_CONFIG
                case ACTION_T.GET_TRIGGER_CONFIG:
                    Frame = $"GTRGCONFIG {Channel % 8}";
                    break;
                #endregion
                #region GET_OUTPUT_STATES
                case ACTION_T.GET_OUTPUT_STATES:
                    Frame = Channel == 0 ? $"GSTATES" : "";
                    break;
                #endregion /* GET_OUTPUT_STATES */
                #region ACTION_T.SET_ASYNCHRONOUS_MODE
                case ACTION_T.SET_ASYNCHRONOUS_MODE:
                    if (Params.ParameterExecuting[1].Length > 0)
                    {
                        if (Params.ParameterExecuting[1] != "0")
                        {
                            Params.ParameterExecuting[1] = "1";
                        }
                    }
                    else
                    {
                        Params.ParameterExecuting[1] = "0";
                    }
#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = Channel < 8 ? $"SSWAF {Channel} " : $"SPSPAF {Channel % 8} ";
                    Frame += Params.ParameterExecuting[1];
#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;
                #endregion
                #region ACTION_T.SET_CONFIG
                case ACTION_T.SET_CONFIG:
                    bool Enable;
                    Relay.SETTINGS_T RelaySettings = new Relay.SETTINGS_T();
                    SetConfigEventArgs Args = new SetConfigEventArgs();

                    Args.Module = (byte)(Channel / RCmanager.Constants.CHANNELS);
                    Args.Channel = (byte)(Channel % RCmanager.Constants.CHANNELS);
                    Args.Action = Params.ActionExecuting;

                    OnSetConfig(Args);

                    RelaySettings = Get_Settings();

                    //#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = Channel < RCmanager.Constants.CHANNELS ? $"SSWCONFIG {Channel % RCmanager.Constants.CHANNELS}" : $"SPSPCONFIG {Channel % RCmanager.Constants.CHANNELS}";
                    Frame += RelaySettings.Enabled == false ? " 0" : " 1";
                    Frame += $" {RelaySettings.Mode.ToString()} {RelaySettings.TimeOn_ms} {RelaySettings.TimeOff_ms} {RelaySettings.DelayTime_ms}";
                    Frame += $" {RelaySettings.ImpulseCounter}";
                    Frame += RelaySettings.AsynchronousMode == false ? " 0" : " 1";
                    Frame += RelaySettings.ImmediateMode == false ? " 0" : " 1";
                    Frame += RelaySettings.InvertedMode == false ? " 0" : " 1";
                    Frame += $" {RelaySettings.TriggerChannel}";
                    Frame += RelaySettings.Triggering == false ? " 0" : " 1";
                    //#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;
                #endregion
                #region ACTION_T.SET_COUNTER
                case ACTION_T.SET_COUNTER:
                    int ImpulseCounter = 0;

                    if (Params.ParameterExecuting[1].Length > 0)
                    {
                        ImpulseCounter = Convert.ToInt32(Params.ParameterExecuting[1]);
                    }
//#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = Channel < 8 ? $"SSWC {Channel} {ImpulseCounter}" : $"SPSPC {Channel % 8} {ImpulseCounter}";
//#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;
                #endregion
                #region ACTION_T.SET_ENABLE
                case ACTION_T.SET_ENABLE:
                    if (Params.ParameterExecuting[1].Length > 0)
                    {
                        if (Params.ParameterExecuting[1] != "0")
                        {
                            Params.ParameterExecuting[1] = "1";
                        }
                    }
                    else
                    {
                        Params.ParameterExecuting[1] = "0";
                    }
//#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = Channel < 8 ? $"SSWE {Channel} " : $"SPSPE {Channel % 8} ";
                    Frame += Params.ParameterExecuting[1];
//#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;
                #endregion
                #region ACTION_T.SET_ENABLE_CARD
                case ACTION_T.SET_ENABLE_CARD:
                    if (Params.ParameterExecuting[1].Length > 0)
                    {
                        if (Params.ParameterExecuting[1] != "0")
                        {
                            Params.ParameterExecuting[1] = "1";
                        }
                    }
                    else
                    {
                        Params.ParameterExecuting[1] = "0";
                    }
                    //#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = Params.ParameterExecuting[2] == "0" ? $"SSWEA {Params.ParameterExecuting[1]} " : $"SPSPEA {Params.ParameterExecuting[1]} ";
                    //#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;
                #endregion
                #region ACTION_T.SET_IMMEDIATE_MODE
                case ACTION_T.SET_IMMEDIATE_MODE:
                    if (Params.ParameterExecuting[1].Length > 0)
                    {
                        if (Params.ParameterExecuting[1] != "0")
                        {
                            Params.ParameterExecuting[1] = "1";
                        }
                    }
                    else
                    {
                        Params.ParameterExecuting[1] = "0";
                    }
//#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = Channel < 8 ? $"SSWIF {Channel} " : $"SPSPIF {Channel % 8} ";
                    Frame += Params.ParameterExecuting[1];
//#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;
                #endregion
                #region ACTION_T.SET_INVERTING_MODE
                case ACTION_T.SET_INVERTING_MODE:
                    if (Params.ParameterExecuting[1].Length > 0)
                    {
                        if (Params.ParameterExecuting[1] != "0")
                        {
                            Params.ParameterExecuting[1] = "1";
                        }
                    }
                    else
                    {
                        Params.ParameterExecuting[1] = "0";
                    }
//#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    Frame = Channel < 8 ? $"SSWNF {Channel} " : $"SPSPNF {Channel % 8} ";
                    Frame += Params.ParameterExecuting[1];
//#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;
                #endregion
                #region ACTION_T.SET_MODE
                case ACTION_T.SET_MODE:
                    Relay.Relay relay = new Relay.Relay();
                    Relay.MODE_T Mode;
                    if (Params.ParameterExecuting[1].Length > 0)
                    {
                        Mode = relay.StringToMode(Params.ParameterExecuting[1]);
                    }
#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    //Frame = $"SSWM {Channel} {Mode.ToString()}";
                    Frame = Channel < 8 ? $"SSWM {Channel} {Params.ParameterExecuting[1]}" : $"SPSPM {Channel % 8} {Params.ParameterExecuting[1]}";
#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    break;
                    #endregion
                #region ACTION_T.SET_TIMING
                case ACTION_T.SET_DELAY_TIME_MS:
                case ACTION_T.SET_TIME_OFF_MS:
                case ACTION_T.SET_TIME_ON_MS:
                    if ((Params.ParameterExecuting[1].Length > 0) && (Params.ParameterExecuting[2].Length > 0) && (Params.ParameterExecuting[3].Length > 0))
                    {
//#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                        Frame = Channel < 8 ? $"SSWT {Channel} " : $"SPSPT {Channel % 8} ";
                        Frame += Params.ParameterExecuting[1] + " " + Params.ParameterExecuting[2] + " " + Params.ParameterExecuting[3];
//#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                    }
                    break;
                #endregion
                #region ACTION_T.SET_TRIGGER
                case ACTION_T.SET_TRIGGER:
                    if (Params.ParameterExecuting[1].Length > 0)
                    {
                        if (Convert.ToByte(Params.ParameterExecuting[1]) < RCmanager.Constants.IRQ_IOS)
                        {
                            if (Params.ParameterExecuting[2].Length > 0)
                            {
                                if (Params.ParameterExecuting[2] != "0")
                                {
                                    Params.ParameterExecuting[2] = "1";
                                }
                            }
                            else
                            {
                                Params.ParameterExecuting[2] = "0";
                            }
                            //#pragma warning disable CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                            Frame = Channel < 8 ? $"SSWTF {Channel} {Params.ParameterExecuting[1]} {Params.ParameterExecuting[2]}" : $"SPSPTF {Channel} {Params.ParameterExecuting[1]}  {Params.ParameterExecuting[2]}";
                            //#pragma warning restore CS1690 // Beim Zugriff auf ein Element zu einem Feld einer "Marshal by Reference"-Klasse kann eine Laufzeitausnahme ausgelöst werden
                        }
                    }
                    break;
                #endregion
                #region ACTION_T.SET_TRIGGER_CONFIG
                case ACTION_T.SET_TRIGGER_CONFIG:
                    Params.ParameterExecuting[2] = TriggerModeToString((TriggerSettings.TRIGGER_MODE_T)Convert.ToInt32(Params.ParameterExecuting[2]));
                    Params.ParameterExecuting[3] = Params.ParameterExecuting[3].Insert(Params.ParameterExecuting[3].Length - 3, ".");
                    Frame = $"STRGCONFIG {Channel} {Params.ParameterExecuting[1]} {Params.ParameterExecuting[2]} {Params.ParameterExecuting[3]}";
                    break;
                #endregion
                #endregion
                default:
                    break;
            }

            return Frame;
        }
        public Relay.SETTINGS_T Get_Settings()
        {
            return RelaySettings;
        }
        public void Set_Settings(Relay.SETTINGS_T value)
        {
            RelaySettings = value;
        }
        private string TriggerModeToString(TriggerSettings.TRIGGER_MODE_T TriggerMode)
        {
            string Mode;

            Mode = "";

            switch (TriggerMode)
            {
                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_LOW:
                    Mode = "LOW";
                    break;

                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_HIGH:
                    Mode = "HIGH";
                    break;

                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_FALLING:
                    Mode = "FALLING";
                    break;

                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_RISING:
                    Mode = "RISING";
                    break;

                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_LEVEL_DOWN:
                    Mode = "LEVEL_DOWN";
                    break;

                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_LEVEL_UP:
                    Mode = "LEVEL_UP";
                    break;

                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_LEVEL_FALLING:
                    Mode = "LEVEL_FALLING";
                    break;

                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_LEVEL_RISING:
                    Mode = "LEVEL_RISING";
                    break;

                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_IRQ_DOWN:
                    Mode = "IRQ_DOWN";
                    break;

                case TriggerSettings.TRIGGER_MODE_T.TRIGGER_MODE_IRQ_UP:
                    Mode = "IRQ_UP";
                    break;

                default:
                    Mode = "LOW";
                    break;
            }
            return Mode;
        }
    }
    public class SetConfigEventArgs : EventArgs
    {
        public byte Module { get; set; }
        public byte Channel { get; set; }
        public ACTION_T Action { get; set; }
    }
}
