using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyUserMonitoringSettings
{
    [Serializable]
    public struct MY_USER_MONITORING_DATA_T
    {
        public int[] UserMonitoringTimeBase_ms { get; set; }     // Signal name
        public bool[,] UsedInChart { get; set; }     // Flag, that signal/trigger is used in a chart
        public int [,] SeriesInChart { get; set; }     // signals/triggers is used in a chart
        public Color ChartBColor { get; set; }      // Chart background color
        public Color ChartLColor { get; set; }      // Signalcolor
        public Color ChartTColor { get; set; }      // Textcolor
        public Color ChartBColor_NM { get; set; }      // Chart background color (night mode)
        public Color ChartLColor_NM { get; set; }      // Signalcolor (night mode)
        public Color ChartTColor_NM { get; set; }      // Textcolor (night mode)
    }
    partial class MyUserMonitoringSettings
    {
        public void SetDefault()
        {
            settings.UserMonitoringTimeBase_ms = new int[2];
            settings.UsedInChart = new bool[2, RCmanager.Constants.MODULES * RCmanager.Constants.CHANNELS + RCmanager.Constants.IRQ_IOS];
            settings.SeriesInChart = new int[2, RCmanager.Constants.MODULES * RCmanager.Constants.CHANNELS + RCmanager.Constants.IRQ_IOS];
        }
    }
}
