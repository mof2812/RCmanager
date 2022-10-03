using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriggerSettings
{
    public enum TRIGGER_MODE_T
    {
        TRIGGER_MODE_LOW,
        TRIGGER_MODE_HIGH,
        TRIGGER_MODE_FALLING,
        TRIGGER_MODE_RISING,
        TRIGGER_MODE_LEVEL_DOWN,
        TRIGGER_MODE_LEVEL_UP,
        TRIGGER_MODE_IRQ_DOWN,
        TRIGGER_MODE_IRQ_UP
    }
    [Serializable]
    public struct PARAMS_T
    {
        public bool Enabled;
        public bool Retrigger;
        public TRIGGER_MODE_T TriggerMode;
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
    public partial class TriggerSettings : UserControl
    {
        #region Members
        PARAMS_T Params;
        bool Initialized;
        bool DoNotUpdateText;
        #endregion // Members
        public TriggerSettings()
        {
            InitializeComponent();

            Init();
        }
        #region Methods
        private void Init()
        {
            Initialized = false;
            DoNotUpdateText = false;

            Init_TrackBar();

            Initialized = true;
        }

        private void Init_TrackBar()
        {
            txtTriggerLevel.Text = Convert.ToString((float)tbTriggerLevel.Value / 1000);
        }
        public PARAMS_T Parameter
        {
            get
            {
                return Params;
            }
            set
            {
                Params = value;
            }
        }
        public bool ReTrigger
        {
            get
            {
                return this.Params.Retrigger;
            }
            set
            {
                this.Params.Retrigger = value;
                ledRetrigger.On = value;
            }
        }
        public bool TriggerEnable
        {
            get
            {
                return this.Params.Enabled;
            }
            set
            {
                this.Params.Enabled = value;
                ledEnable.On = value;
            }
        }
        public TRIGGER_MODE_T TriggerMode
        {
            get
            {
                return this.Params.TriggerMode;
            }
            set
            {
                this.Params.TriggerMode = value;
                cbTriggerMode.SelectedIndex = (int)value;
            }
        }
        public string TriggerName
        {
            get
            {
                return this.grpTriggerSettings.Text;
            }
            set
            {
                this.grpTriggerSettings.Text = value;
            }
        }
        public float TriggerLevel
        {
            get
            {
                return (float)this.tbTriggerLevel.Value / 1000;
            }
            set
            {
                this.tbTriggerLevel.Value = (int)(value * 1000);
            }
        }

        #endregion

        private void tbTriggerLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!DoNotUpdateText)
            {
                txtTriggerLevel.Text = Convert.ToString((float)(tbTriggerLevel.Value / 50) / 20.0f);
            }
            else
            {
                DoNotUpdateText = false;
            }
        }
        private void ledEnable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Params.Enabled = !Params.Enabled;
            ledEnable.On = Params.Enabled;
        }

        private void ledRetrigger_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Params.Retrigger = !Params.Retrigger;
            ledRetrigger.On = Params.Retrigger;
        }

        private void cbTriggerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Params.TriggerMode = (TRIGGER_MODE_T)cbTriggerMode.SelectedIndex;
        }

        private void txtTriggerLevel_TextChanged(object sender, EventArgs e)
        {
            float TriggerLevel;

            TriggerLevel = 0.0f;

            if (Initialized)
            {
                try
                {
                    txtTriggerLevel.Text.Replace(".", ",");
                    TriggerLevel = (float)Convert.ToDouble(txtTriggerLevel.Text);
                    Params.TriggerLevel = TriggerLevel;
                    tbTriggerLevel.Value = (int)(TriggerLevel * 1000f);
                    DoNotUpdateText = true;
                }
                catch
                {

                }
            }
        }
    }
}
