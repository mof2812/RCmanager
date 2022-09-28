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
        public bool TriggerStatus
        {
            get
            {
                return ledTrigger.On;
            }
            set 
            { 
                ledTrigger.On = value; 
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
