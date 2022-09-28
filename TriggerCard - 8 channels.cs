using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriggerCard
{
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
    public partial class TriggerCard : UserControl
    {
        public TriggerCard()
        {
            InitializeComponent();
        }
        #region Methods
        private Trigger.Trigger GetTrigger(byte Channel)
        {
            Trigger.Trigger trigger = new Trigger.Trigger();

            switch (Channel)
            {
                case 0:
                    trigger = trigger_1;
                    break;

                case 1:
                    trigger = trigger_2;
                    break;

                case 2:
                    trigger = trigger_3;
                    break;

                case 3:
                    trigger = trigger_4;
                    break;

                case 4:
                    trigger = trigger_5;
                    break;

                case 5:
                    trigger = trigger_6;
                    break;

                case 6:
                    trigger = trigger_7;
                    break;

                case 7:
                    trigger = trigger_8;
                    break;

                default:
                    trigger = null;
                    break;
            }

            return trigger;
        }
        public RETURN_T GetTriggerParameter(byte Channel, ref Trigger.PARAMS_T TriggerParams)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (Channel < RCmanager.Constants.IRQ_IOS)
            {
                TriggerParams = GetTrigger(Channel).GetParams();
            }
            else
            {
                Return = RETURN_T.INVALIDE_CHANNEL;
            }

            return Return;
        }
        public RETURN_T Init(bool NightMode)
        {
            RETURN_T Return;

            Return = Init_Triggers(NightMode);

            return Return;
        }
        private RETURN_T Init_Triggers(bool NightMode)
        {
            RETURN_T Return;
            byte Channel;

            Return = RETURN_T.INITIALIZE_ERROR;
            Channel = 0;

            if ((RETURN_T)trigger_1.Init(Channel++, NightMode) == RETURN_T.OKAY)
            {
                if ((RETURN_T)trigger_2.Init(Channel++, NightMode) == RETURN_T.OKAY)
                {
                    if ((RETURN_T)trigger_3.Init(Channel++, NightMode) == RETURN_T.OKAY)
                    {
                        if ((RETURN_T)trigger_4.Init(Channel++, NightMode) == RETURN_T.OKAY)
                        {
                            if ((RETURN_T)trigger_5.Init(Channel++, NightMode) == RETURN_T.OKAY)
                            {
                                if ((RETURN_T)trigger_6.Init(Channel++, NightMode) == RETURN_T.OKAY)
                                {
                                    if ((RETURN_T)trigger_7.Init(Channel++, NightMode) == RETURN_T.OKAY)
                                    {
                                        if ((RETURN_T)trigger_8.Init(Channel++, NightMode) == RETURN_T.OKAY)
                                        {
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
#endregion
    }
}
