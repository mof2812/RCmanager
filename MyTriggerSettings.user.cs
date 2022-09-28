using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyTriggerSettings
{
    [Serializable]
    public struct MY_DATA_T
    {
        public string TriggerLabel { get; set; }     // Trigger name
        public Color ChartBColor { get; set; }      // Chart background color
        public Color ChartLColor { get; set; }      // Signalcolor
        public Color ChartTColor { get; set; }      // Textcolor
        public Color ChartBColor_NM { get; set; }      // Chart background color (night mode)
        public Color ChartLColor_NM { get; set; }      // Signalcolor (night mode)
        public Color ChartTColor_NM { get; set; }      // Textcolor (night mode)
    }
}
