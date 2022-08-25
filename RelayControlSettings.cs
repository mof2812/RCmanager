using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelayControl
{
    public enum WHICH_PARAMETER_T
    {
        MODE,
    }
    public struct RELAY_SETTINGS_T
    {
        UInt32 Test;
    }
    public partial class RelayControlSettings : Form
    {
        public event EventHandler<SetParameterEventArgs> SetParameter;

        string[] ListBoxModeItems = new string[] {"Aus", "Ein", "Toggle", "Impuls"};
        PARAMS_T Params;
        public RelayControlSettings()
        {
            InitializeComponent();

            Init_ListBox();
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

                SetData();
            }

        }

        private void Init_ListBox()
        {
            cbMode.Items.Clear();

            for (int i = 0; i < ListBoxModeItems.Length; i++)
            {
                cbMode.Items.Add(ListBoxModeItems[i]);
            }
        }
        private void SetData()
        {
            txtSignalName.Text = Params.ChartDName;

            txtDelay_ms.Text = Params.DelayTime_ms.ToString();
            txtOn_ms.Text = Params.TimeOn_ms.ToString();
            txtOff_ms.Text = Params.TimeOff_ms.ToString();

            ledInvertingMode.On = Params.InvertedMode;
            ledEnable.On = Params.Enabled;

            ledTriggerEnable.On = Params.TriggerMode;

            cbMode.SelectedItem = (int)Params.Mode < ListBoxModeItems.Length ? ListBoxModeItems[(int)Params.Mode] : ListBoxModeItems[0];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void txtDelay_ms_TextChanged(object sender, EventArgs e)
        {
            Params.DelayTime_ms = Convert.ToUInt32(txtDelay_ms.Text);
        }

        private void txtOn_ms_TextChanged(object sender, EventArgs e)
        {
            Params.TimeOn_ms = Convert.ToUInt32(txtOn_ms.Text);
        }

        private void txtOff_ms_TextChanged(object sender, EventArgs e)
        {
            Params.TimeOff_ms = Convert.ToUInt32(txtOff_ms.Text);
        }

        private void ledInvertingMode_Click(object sender, EventArgs e)
        {
            ledInvertingMode.On = !ledInvertingMode.On;
            Params.InvertedMode = ledInvertingMode.On;
        }

        private void ledEnable_Click(object sender, EventArgs e)
        {
            ledEnable.On = !ledEnable.On;
            Params.Enabled = ledEnable.On;
        }

        private void lblInvertingMode_Click(object sender, EventArgs e)
        {
            ledInvertingMode.On = !ledInvertingMode.On;
            Params.InvertedMode = ledInvertingMode.On;
        }

        private void lblEnable_Click(object sender, EventArgs e)
        {
            ledEnable.On = !ledEnable.On;
            Params.Enabled = ledEnable.On;
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Params.Mode = (MODE_T)cbMode.SelectedIndex;

            SetParameterEventArgs Args = new SetParameterEventArgs();
            Args.Parameter = WHICH_PARAMETER_T.MODE;
            Args.Mode = (MODE_T)cbMode.SelectedIndex;
            OnSetParmeter(Args);

            //RelayCard16.RelayCard16 RCard = new RelayCard16.RelayCard16();
            //RCard.SetAction(RelayCard16.ACTION_T.GET_CONFIG, 0, false);
            
        }

        protected virtual void OnSetParmeter(SetParameterEventArgs e)
        {
            EventHandler<SetParameterEventArgs> handler = SetParameter;
            handler?.Invoke(this, e);
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ledTriggerEnable_Click(object sender, EventArgs e)
        {
            ledTriggerEnable.On = !ledTriggerEnable.On;
            Params.TriggerMode = ledTriggerEnable.On;
        }

        private void lblTriggerEnable_Click(object sender, EventArgs e)
        {
            ledTriggerEnable.On = !ledTriggerEnable.On;
            Params.TriggerMode = ledTriggerEnable.On;
        }

        private void txtSignalName_TextChanged(object sender, EventArgs e)
        {
            Params.ChartDName = txtSignalName.Text;
        }
    }
    public class SetParameterEventArgs : EventArgs
    {
        public WHICH_PARAMETER_T Parameter { get; set; }
        public MODE_T Mode { get; set; }
}

}
