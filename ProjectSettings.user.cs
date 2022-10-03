using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ProjectSettings
{
    [Serializable]
    public struct PROJECT_SETTINGS_T
    {
        public Relay.PARAMS_T[] RelayParams { get; set; }     // Relay parameters
        public TriggerSettings.PARAMS_T[] TriggerSettings { get; set; }     // Trigger settings
    }
    partial class ProjectSettings
    {
        public void SetDefault()
        {
            settings.RelayParams = new Relay.PARAMS_T[RCmanager.Constants.MODULES * RCmanager.Constants.CHANNELS];
            settings.TriggerSettings = new TriggerSettings.PARAMS_T[RCmanager.Constants.IRQ_IOS];
        }
        public void SetData(Relay.PARAMS_T[] RelayParams, TriggerSettings.PARAMS_T[] TriggerSettings)
        {
            settings.RelayParams = RelayParams;
            settings.TriggerSettings = TriggerSettings;
        }
    }
}
