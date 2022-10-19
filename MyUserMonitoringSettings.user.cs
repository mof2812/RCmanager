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
        public bool[] RectangleMode { get; set; }   // Monitor signals in rectangle mode
        public Color ChartBColor { get; set; }      // Chart background color
        public Color ChartLColor { get; set; }      // Signalcolor
        public Color ChartTColor { get; set; }      // Textcolor
        public Color ChartBColor_NM { get; set; }      // Chart background color (night mode)
        public Color ChartLColor_NM { get; set; }      // Signalcolor (night mode)
        public Color ChartTColor_NM { get; set; }      // Textcolor (night mode)
        public bool[] Y_Grid { get; set; }              // Use grid on y axes
        public Color[] ChartGridColor { get; set; }      // Grid color (day mode)
        public Color[] ChartGridColor_NM { get; set; }      // Grid color (night mode)
    }
    partial class MyUserMonitoringSettings
    {
        public void SetDefault()
        {
            settings.UserMonitoringTimeBase_ms = new int[2];
            settings.UsedInChart = new bool[2, RCmanager.Constants.MODULES * RCmanager.Constants.CHANNELS + RCmanager.Constants.IRQ_IOS];
            settings.SeriesInChart = new int[2, RCmanager.Constants.MODULES * RCmanager.Constants.CHANNELS + RCmanager.Constants.IRQ_IOS];
            settings.RectangleMode = new bool[2];
            settings.Y_Grid = new bool[2];
            settings.ChartGridColor = new Color[] {Color.DarkGray, Color.DarkGray};
            settings.ChartGridColor_NM = new Color[] { Color.DarkGray, Color.DarkGray};
        }
    }
}
