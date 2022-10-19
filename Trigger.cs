using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trigger
{
    public struct PARAMS_T
    {
        public byte Channel;
        public bool NightMode;
        public Color ChartLColor;
        public string TriggerLabel;
        public float AnalogInputValue;
        public float TriggerLevel;
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
    public partial class Trigger : UserControl
    {
#region Members
        protected PARAMS_T Params;
#endregion /* Members */
        public Trigger()
        {
            InitializeComponent();

            Init();
        }
        #region Methods
        public float AnalogInputVoltage
        {
            get
            {
                return Params.AnalogInputValue;
            }
            set
            {
                Params.AnalogInputValue = value;

                lblInputVoltage.Visible = lblTriggerLevel.Visible = value > -5.00f;

                if (value > -5.00f)
                {
                    //lblInputVoltage.Text = $"{value}V";
                    lblInputVoltage.Text = string.Format("{0:0.00}V", value);

                    if (value > Params.TriggerLevel + 0.01f)
                    {
                        ledTrigger.On = true;
                    }
                    else if (value < Params.TriggerLevel - 0.01f)
                    {
                        ledTrigger.On = false;
                    }
                    lblTriggerLevel.Visible = true;
                }
            }
        }
        public PARAMS_T GetParams()
        {
            return Params;
        }
        private void Init()
        {
            Init_Led();
        }
        public RETURN_T Init(byte Channel, bool NightMode)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            Params.Channel = (byte)(Channel + 1);   // Channel starts by 1
            Params.NightMode = NightMode;

            Init_Settings();
            Init_Led();

            Params.TriggerLabel = MySetup.settings.TriggerLabel;

            return Return;
        }
        private void Init_Led()
        {
            ledTrigger.On = false;
        }
        public bool NightMode
        {
            get
            {
                return Params.NightMode;
            }
            set
            {
                this.BackColor = value ? Color.Black : Color.White;
                lblTriggerName.ForeColor = value ? Color.Yellow : Color.Black;
                lblTriggerLevel.ForeColor = value ? Color.Yellow : Color.Black;
                lblInputVoltage.ForeColor = Color.LightBlue;
            }
        }
        private RETURN_T Init_Settings()
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (MySetup.Init($"{Constants.DATAPATH}Trigger_{Params.Channel}.dat"))  /* If file does not exist... */
            {
                // Set default values
                SetDefaults();
            }
            return Return;
        }
        public void SetDefaults()
        {
            MySetup.settings.TriggerLabel = $"Trigger_{Params.Channel}";

            MySetup.Save();
        }
        public RETURN_T SetParams(PARAMS_T Params)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            this.Params = Params;

            return Return;
        }
        public bool TriggerStatus
        {
            get
            {
                return ledTrigger.On;
            }
            set 
            {
                if (Params.AnalogInputValue <= -5.0f)
                {
                    ledTrigger.On = value;
                }
            }
        }
        public float TriggerLevel
        {
            get
            {
                return Params.TriggerLevel;
            }
            set
            {
                Params.TriggerLevel = value;

                lblTriggerLevel.Text = string.Format("{0:0.00}V", value);
            }
        }
        public string TriggerName
        {
            get
            {
                return lblTriggerName.Text;
            }
            set
            {
                lblTriggerName.Text = value;
            }
        }
#endregion /* Methods */
    }
    static class Constants
    {
        public const uint INITCODE = 0x55AA55AA;
        public const string DATAPATH = "..\\..\\Data\\";
    }
}
