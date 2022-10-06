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
        public Relay.SETTINGS_T[,] RelaySettings { get; set; }     // Relay parameters
        public TriggerSettings.PARAMS_T[] TriggerSettings { get; set; }     // Trigger settings
        public string Info { get; set; }     // Project info 
    }
    partial class ProjectSettings
    {
        public void Init_Data()
        {
            settings.RelaySettings = new Relay.SETTINGS_T[RCmanager.Constants.MODULES, RCmanager.Constants.CHANNELS];
            settings.TriggerSettings = new TriggerSettings.PARAMS_T[RCmanager.Constants.IRQ_IOS];
            settings.Info = "";
        }
        //public void SetData(Relay.SETTINGS_T[,] RelaySettings, TriggerSettings.PARAMS_T[] TriggerSettings)
        //{
        //    settings.RelaySettings = RelaySettings;
        //    settings.TriggerSettings = TriggerSettings;
        //}
    }
}
