using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriggerSettings;

namespace TriggerSettingsDlg

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
    public partial class TriggerSettingsDlg : Form
    {
        TriggerSettings.PARAMS_T[] TriggerSettings;
        public TriggerSettingsDlg()
        {
            InitializeComponent();

            Init();
        }
        #region Methods
        TriggerSettings.TriggerSettings ChannelToTriggerSettings(int TriggerChannel)
        {
            TriggerSettings.TriggerSettings Settings = new TriggerSettings.TriggerSettings();

            switch(TriggerChannel)
            {
                case 0:
                    Settings = Trigger1Settings;
                    break;

                case 1:
                    Settings = Trigger2Settings;
                    break;

                case 2:
                    Settings = Trigger3Settings;
                    break;

                default:
                    Settings = Trigger4Settings;
                    break;
            }
            return Settings;
        }
        private void Init()
        {
            Init_TriggerSettings();
        }
        private void Init_TriggerSettings()
        {
            TriggerSettings = new TriggerSettings.PARAMS_T[RCmanager.Constants.IRQ_IOS];

            for (int TriggerChannel = 0; TriggerChannel < RCmanager.Constants.IRQ_IOS; TriggerChannel++)
            {
                Init_TriggerSettings(TriggerChannel);
            }
        }
        private TriggerSettings.TriggerSettings TriggerChannelToTriggerSettings(byte TriggerChannel)
        {
            TriggerSettings.TriggerSettings Control = new TriggerSettings.TriggerSettings();

            switch(TriggerChannel)
            {
                case 0:
                    Control = Trigger1Settings;
                    break;

                case 1:
                    Control = Trigger3Settings;
                    break;

                case 2:
                    Control = Trigger4Settings;
                    break;

                default:
                    Control = Trigger2Settings;
                    break;
            }
            return Control;
        }
        private void Init_TriggerSettings(int TriggerChannel)
        {
            TriggerSettings.TriggerSettings triggerSettings = new TriggerSettings.TriggerSettings();

            triggerSettings = ChannelToTriggerSettings(TriggerChannel);

            triggerSettings.TriggerName = $"Trigger {TriggerChannel + 1}";
        }
        public RETURN_T TriggerParameter(byte Channel, ref TriggerSettings.PARAMS_T TriggerParams)
        {
            RETURN_T Return;

            Return = RETURN_T.OKAY;

            if (Channel < RCmanager.Constants.CHANNELS)
            {
                TriggerParams = ChannelToTriggerSettings(Channel).Parameter;
            }
            else
            {
                Return = RETURN_T.INVALIDE_CHANNEL;
            }

            return Return;
        }
        public TriggerSettings.PARAMS_T[] Settings
        {
            get
            {
                UpdateData(true);

                return TriggerSettings;
            }
            set
            {
                TriggerSettings = value;

                UpdateData(false);
            }
        }
        private void UpdateData(bool get)
        {
            TriggerSettings.TriggerSettings Control = new TriggerSettings.TriggerSettings();

            for (byte TriggerChannel = 0; TriggerChannel < RCmanager.Constants.IRQ_IOS; TriggerChannel++)
            {
                Control = ChannelToTriggerSettings(TriggerChannel);

                if (get)
                {
                    TriggerSettings[TriggerChannel].Enabled = Control.TriggerEnable;
                    TriggerSettings[TriggerChannel].TriggerMode = Control.TriggerMode;
                    TriggerSettings[TriggerChannel].TriggerLevel = Control.TriggerLevel;
                }
                else
                {
                    Control.TriggerEnable = TriggerSettings[TriggerChannel].Enabled;
                    Control.TriggerMode = TriggerSettings[TriggerChannel].TriggerMode;
                    Control.TriggerLevel = TriggerSettings[TriggerChannel].TriggerLevel;
                }
            }
        }
        #endregion
        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void RelayTriggerSettingsDlg_Load(object sender, EventArgs e)
        {
            
        }
    }
}
