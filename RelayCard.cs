using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelayCard
{
    public struct PARAMS_T
    {
        public byte Module;
        public double MaxChartXRange_ms;
        public double MinChartXRange_ms;
    }
    public enum RETURN_T
    {
        OKAY = 0,
        ERROR,
        INITIALIZE_ERROR,
        PARAM_ERROR,
        PARAM_MISSING,
        PARAM_OUT_OF_RANGE,
        INVALIDE_CHANNEL,
    }
    public partial class RelayCard : UserControl
    {
        public event EventHandler<SetParameterRelayCardEventArgs> SetParameterRelayCard;

        #region Members
        PARAMS_T Params;
        #endregion
        #region Properties
        public double MaxChartXRange_ms
        {
            get
            {
                Params.MaxChartXRange_ms = -1e30;
                double Limit;

                for (byte Channel = 0; Channel < RCmanager.Constants.CHANNELS; Channel++)
                {
                    Limit = GetRelay(Channel).MaxChartXRange_ms;
                    Params.MaxChartXRange_ms = Limit > Params.MaxChartXRange_ms ? Limit : Params.MaxChartXRange_ms;
                }
                return (Params.MaxChartXRange_ms);
            }
            //set
            //{

            //}
        }
        public double MinChartXRange_ms
        {
            get
            {
                Params.MinChartXRange_ms = 1e30;
                double Limit;

                for (byte Channel = 0; Channel < RCmanager.Constants.CHANNELS; Channel++)
                {
                    Limit = GetRelay(Channel).MinChartXRange_ms;
                    Params.MinChartXRange_ms = Limit < Params.MinChartXRange_ms ? Limit : Params.MinChartXRange_ms;
                }
                return (Params.MinChartXRange_ms);
            }
            //set
            //{

            //}
        }

        #endregion
        #region Constructors, destructors
        public RelayCard()
        {
            InitializeComponent();
        }
        #endregion
        #region Methods
        public RETURN_T EvaluateConfigurationString(byte Channel, string ConfigurationString)
        {
            RETURN_T Return;
            Relay.Relay relay = new Relay.Relay();

            Return = RETURN_T.OKAY;

            relay = GetRelay(Channel);

            relay.EvaluateConfigurationString(ConfigurationString);

            return Return;
        }
        public Relay.Relay GetRelay(byte Channel)
        {
            Relay.Relay relay = new Relay.Relay();

            switch (Channel)
            {
                case 0:
                    relay = relayCH1;
                    break;

                case 1:
                    relay = relayCH2;
                    break;

                case 2:
                    relay = relayCH3;
                    break;

                case 3:
                    relay = relayCH4;
                    break;

                case 4:
                    relay = relayCH5;
                    break;

                case 5:
                    relay = relayCH6;
                    break;

                case 6:
                    relay = relayCH7;
                    break;

                case 7:
                    relay = relayCH8;
                    break;

                default:
                    relay = null;
                    break;
            }

            return relay;
        }
        public RETURN_T GetRelayParameter(byte Channel, ref Relay.PARAMS_T RelayParams)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (Channel < RCmanager.Constants.CHANNELS)
            {
                RelayParams = GetRelay(Channel).GetParams();
            }
            else
            {
                Return = RETURN_T.INVALIDE_CHANNEL;
            }

            return Return;
        }
        public RETURN_T GetSignalName(byte Channel, ref string SignalName)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;
            SignalName = "";
            
            if (Channel < RCmanager.Constants.CHANNELS)
            {
                SignalName = GetRelay(Channel).GetSignalName();
            }
            else
            {
                Return = RETURN_T.INVALIDE_CHANNEL;
            }

            return Return;
        }
        public RETURN_T Init(byte Module, bool NightMode)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            Params.Module = Module;

            Return = Init_Relays(NightMode);

            return Return;
        }
        private RETURN_T Init_Relays(bool NightMode)
        {
            RETURN_T Return;
            byte Channel;

            Return = RETURN_T.INITIALIZE_ERROR;
            Channel = (byte)(Params.Module * 8);

            if ((RETURN_T)relayCH1.Init(Channel++, NightMode) == RETURN_T.OKAY)
            {
                relayCH1.SetParameterRelay += SetParameter;
                if ((RETURN_T)relayCH2.Init(Channel++, NightMode) == RETURN_T.OKAY)
                {
                    relayCH2.SetParameterRelay += SetParameter;

                    if ((RETURN_T)relayCH3.Init(Channel++, NightMode) == RETURN_T.OKAY)
                    {
                        relayCH3.SetParameterRelay += SetParameter;

                        if ((RETURN_T)relayCH4.Init(Channel++, NightMode) == RETURN_T.OKAY)
                        {
                            relayCH4.SetParameterRelay += SetParameter;

                            if ((RETURN_T)relayCH5.Init(Channel++, NightMode) == RETURN_T.OKAY)
                            {
                                relayCH5.SetParameterRelay += SetParameter;

                                if ((RETURN_T)relayCH6.Init(Channel++, NightMode) == RETURN_T.OKAY)
                                {
                                    relayCH6.SetParameterRelay += SetParameter;

                                    if ((RETURN_T)relayCH7.Init(Channel++, NightMode) == RETURN_T.OKAY)
                                    {
                                        relayCH7.SetParameterRelay += SetParameter;

                                        if ((RETURN_T)relayCH8.Init(Channel++, NightMode) == RETURN_T.OKAY)
                                        {
                                            relayCH8.SetParameterRelay += SetParameter;

                                            Return = RETURN_T.OKAY;
                                        }

                                    }

                                }

                            }

                        }

                    }

                }

            }
            return Return;
        }
        protected virtual void OnSetParmeterRelayCard(SetParameterRelayCardEventArgs e)
        {
            EventHandler<SetParameterRelayCardEventArgs> handler = SetParameterRelayCard;
            handler?.Invoke(this, e);
        }
        public void NightMode(bool Set)
        {
            Relay.Relay relay = new Relay.Relay();

            for (byte Channel = 0; Channel < 8; Channel++)
            {
                relay = GetRelay(Channel);

                if (relay != null)
                {
                    relay.NightMode = Set;
                }
            }
        }
        public RETURN_T SetAxisLimit(byte Channel, Relay.AXIS_SELECT_T Axis, double Limit, bool Minimum)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (Channel < RCmanager.Constants.CHANNELS)
            {
                Return = (RETURN_T)GetRelay(Channel).SetAxisLimit(Axis, Limit, Minimum);
            }
            else
            {
                Return = RETURN_T.INVALIDE_CHANNEL;
            }

            return Return;
        }

        private RETURN_T FctTemplate()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            return Return;
        }
        #endregion
        #region Events
        private void SetParameter(object sender, Relay.SetParameterRelayEventArgs e)
        {
            SetParameterRelayCardEventArgs Args = new SetParameterRelayCardEventArgs();

            Args.Module = (byte)(e.Channel / 8);
            Args.Channel = e.Channel;
            Args.Parameter = e.Parameter;
            Args.Mode = e.Mode;
            Args.Params = e.Params;

            OnSetParmeterRelayCard(Args);
        }

        #endregion
    }
    public class SetParameterRelayCardEventArgs : EventArgs
    {
        public byte Channel { get; set; }
        public byte Module { get; set; }
        public Relay.WHICH_PARAMETER_T Parameter { get; set; }
        public Relay.MODE_T Mode { get; set; }
        public Relay.PARAMS_T Params { get; set; }
    }
}
