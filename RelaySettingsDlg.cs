using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Relay
{
    public enum WHICH_PARAMETER_T
    {
        LABEL,
        ASYNCHRONOUS_MODE,
        DELAY_TIME_MS,
        ENABLE,
        ENABLE_ALL,
        ENABLE_CARD,
        IMMEDIATE_MODE,
        INVERTING_MODE,
        MODE,
        TIME_OFF_MS,
        TIME_ON_MS,
        IMPULSE_COUNTER,
        TRIGGER,
        TRIGGER_CONFIG,
    }
    public struct RELAY_SETTINGS_T
    {
        UInt32 Test;
    }
    public partial class RelaySettingsDlg : Form
    { 
        public event EventHandler<SetParameterEventArgs> SetParameter;

        string[] ListBoxModeItems = new string[] { "Aus", "Ein", "Toggle", "Impuls" };
        string RefText = "";
        PARAMS_T Params;
        public RelaySettingsDlg()
        {
            InitializeComponent();

            Init_ListBox();

            //Init_RadioButtons();
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

            Update_ListBox();
        }
        private void Init_RadioButtons()
        {
            rbImmediateOn.Checked = Params.ImmediateMode;
            rbImmediateOff.Checked = !rbImmediateOn.Checked;
              
            rbAsynchron.Checked = Params.AsynchronousMode;
            rbSynchron.Checked = !rbAsynchron.Checked;
        }
        private void Update_ListBox()
        {
            if (Params.TriggerChannel < cmbTriggerChannel.Items.Count - 1)
            {
                cmbTriggerChannel.SelectedIndex = Params.TriggerChannel;
            }
            //else if (Params.TriggerChannel == 255)
            //{
            //    cmbTriggerChannel.SelectedIndex = cmbTriggerChannel.Items.Count - 1;
            //}
            else
            {
                cmbTriggerChannel.SelectedIndex = cmbTriggerChannel.Items.Count - 1;
            }
        }
        private void SetData()
        {
            txtSignalName.Text = Params.SignalLabel;

            txtDelay_ms.Text = Params.DelayTime_ms.ToString();
            txtOn_ms.Text = Params.TimeOn_ms.ToString();
            txtOff_ms.Text = Params.TimeOff_ms.ToString();
            txtCounter.Text = Params.ImpulseCounter.ToString();

            ledInvertingMode.On = Params.InvertedMode;
            ledEnable.On = Params.Enabled;

            ledTriggerEnable.On = Params.Triggering;
            cmbTriggerChannel.Enabled = lblTriggerChannel.Enabled = Params.Triggering;

            cbMode.SelectedItem = (int)Params.Mode < ListBoxModeItems.Length ? ListBoxModeItems[(int)Params.Mode] : ListBoxModeItems[0];

            Update_ListBox();

            Init_RadioButtons();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        private void ledEnable_Click(object sender, EventArgs e)
        {
            SetParameterEventArgs Args = new SetParameterEventArgs();

            ledEnable.On = !ledEnable.On;
            Params.Enabled = ledEnable.On;

            Args.Parameter = WHICH_PARAMETER_T.ENABLE;
            Args.Params = Params;
            OnSetParmeter(Args);
        }
        private void lblEnable_Click(object sender, EventArgs e)
        {
            SetParameterEventArgs Args = new SetParameterEventArgs();

            ledEnable.On = !ledEnable.On;
            Params.Enabled = ledEnable.On;

            Args.Parameter = WHICH_PARAMETER_T.ENABLE;
            Args.Params = Params;
            OnSetParmeter(Args);
        }
        private void ledInvertingMode_Click(object sender, EventArgs e)
        {
            SetParameterEventArgs Args = new SetParameterEventArgs();

            ledInvertingMode.On = !ledInvertingMode.On;
            Params.InvertedMode = ledInvertingMode.On;

            Args.Parameter = WHICH_PARAMETER_T.INVERTING_MODE;
            Args.Params = Params;
            OnSetParmeter(Args);
        }

        private void lblInvertingMode_Click(object sender, EventArgs e)
        {
            SetParameterEventArgs Args = new SetParameterEventArgs();

            ledInvertingMode.On = !ledInvertingMode.On;
            Params.InvertedMode = ledInvertingMode.On;

            Args.Parameter = WHICH_PARAMETER_T.INVERTING_MODE;
            Args.Params = Params;
            OnSetParmeter(Args);
        }
        protected virtual void OnSetParmeter(SetParameterEventArgs e)
        {
            EventHandler<SetParameterEventArgs> handler = SetParameter;
            handler?.Invoke(this, e);
        }

        private void ledTriggerEnable_Click(object sender, EventArgs e)
        {
            SetParameterEventArgs Args = new SetParameterEventArgs();

            ledTriggerEnable.On = !ledTriggerEnable.On;
            cmbTriggerChannel.Enabled = lblTriggerChannel.Enabled = ledTriggerEnable.On;

            Params.TriggerChannel = Convert.ToUInt16(cmbTriggerChannel.SelectedIndex);
            Params.Triggering = ledTriggerEnable.On;

            Args.Parameter = WHICH_PARAMETER_T.TRIGGER;
            Args.Params = Params;
            OnSetParmeter(Args);

        }

        private void lblTriggerEnable_Click(object sender, EventArgs e)
        {
            SetParameterEventArgs Args = new SetParameterEventArgs();

            ledTriggerEnable.On = !ledTriggerEnable.On;
            Params.Triggering = ledTriggerEnable.On;

            Args.Parameter = WHICH_PARAMETER_T.TRIGGER;
            Args.Params = Params;
            OnSetParmeter(Args);
        }

        private void txtSignalName_TextChanged(object sender, EventArgs e)
        {
            Params.SignalLabel = txtSignalName.Text;
        }

        private void rbImmediateOn_CheckedChanged(object sender, EventArgs e)
        {
            Params.ImmediateMode = rbImmediateOn.Checked;
        }

        private void rbImmediateOff_CheckedChanged(object sender, EventArgs e)
        {
            SetParameterEventArgs Args = new SetParameterEventArgs();

            Params.ImmediateMode = rbImmediateOn.Checked;

            Args.Parameter = WHICH_PARAMETER_T.IMMEDIATE_MODE;
            Args.Params = Params;
            OnSetParmeter(Args);
        }

        private void rbSynchron_CheckedChanged(object sender, EventArgs e)
        {
            Params.AsynchronousMode = rbAsynchron.Checked;
        }

        private void rbAsynchron_CheckedChanged(object sender, EventArgs e)
        {
            SetParameterEventArgs Args = new SetParameterEventArgs();

            Params.AsynchronousMode = rbAsynchron.Checked;

            Args.Parameter = WHICH_PARAMETER_T.ASYNCHRONOUS_MODE;
            Args.Params = Params;
            OnSetParmeter(Args);
        }
        #region txtDelay_ms_XXX
        private void txtDelay_ms_Enter(object sender, EventArgs e)
        {
            RefText = txtDelay_ms.Text;
        }
        private void txtDelay_ms_Leave(object sender, EventArgs e)
        {
            if (RefText != txtDelay_ms.Text)
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.DelayTime_ms = Convert.ToUInt32(txtDelay_ms.Text);

                Args.Parameter = WHICH_PARAMETER_T.DELAY_TIME_MS;
                Args.Params = Params;
                OnSetParmeter(Args);
            }
        }
        private void txtDelay_ms_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Enter)) && (RefText != txtDelay_ms.Text))
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.DelayTime_ms = Convert.ToUInt32(txtDelay_ms.Text);

                Args.Parameter = WHICH_PARAMETER_T.DELAY_TIME_MS;
                Args.Params = Params;
                OnSetParmeter(Args);

                RefText = txtDelay_ms.Text;
            }
        }
#endregion
        #region txtOn_ms_XXX
        private void txtOn_ms_Enter(object sender, EventArgs e)
        {
            RefText = txtOn_ms.Text;
        }
        private void txtOn_ms_Leave(object sender, EventArgs e)
        {
            if (RefText != txtOn_ms.Text)
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.TimeOn_ms = Convert.ToUInt32(txtOn_ms.Text);

                Args.Parameter = WHICH_PARAMETER_T.TIME_ON_MS;
                Args.Params = Params;
                OnSetParmeter(Args);
            }

        }
        private void txtOn_ms_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Enter)) && (RefText != txtOn_ms.Text))
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.TimeOn_ms = Convert.ToUInt32(txtOn_ms.Text);

                Args.Parameter = WHICH_PARAMETER_T.TIME_ON_MS;
                Args.Params = Params;
                OnSetParmeter(Args);

                RefText = txtOn_ms.Text;
            }
        }
        #endregion
        #region txtOff_ms_XXX
        private void txtOff_ms_Enter(object sender, EventArgs e)
        {
            RefText = txtOff_ms.Text;
        }
        private void txtOff_ms_Leave(object sender, EventArgs e)
        {
            if (RefText != txtOff_ms.Text)
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.TimeOff_ms = Convert.ToUInt32(txtOff_ms.Text);

                Args.Parameter = WHICH_PARAMETER_T.TIME_OFF_MS;
                Args.Params = Params;
                OnSetParmeter(Args);
            }

        }
        private void txtOff_ms_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Enter)) && (RefText != txtOff_ms.Text))
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.TimeOff_ms = Convert.ToUInt32(txtOff_ms.Text);

                Args.Parameter = WHICH_PARAMETER_T.TIME_OFF_MS;
                Args.Params = Params;
                OnSetParmeter(Args);

                RefText = txtOff_ms.Text;
            }
        }
        #endregion
        #region cbMode_XXX
        private void cbMode_Enter(object sender, EventArgs e)
        {
            RefText = cbMode.Text;
        }
        private void cbMode_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RefText != cbMode.Text)
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.Mode = (MODE_T)cbMode.SelectedIndex;

                Args.Parameter = WHICH_PARAMETER_T.MODE;
                Args.Mode = (MODE_T)cbMode.SelectedIndex;
                Args.Params = Params;
                OnSetParmeter(Args);

                RefText = cbMode.Text;
            }
        }


        #endregion
        #region txtCounter_XXX
        private void txtCounter_Enter(object sender, EventArgs e)
        {
            RefText = txtCounter.Text;
        }
        private void txtCounter_Leave(object sender, EventArgs e)
        {
            if (RefText != txtCounter.Text)
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.ImpulseCounter = Convert.ToInt32(txtCounter.Text);

                Args.Parameter = WHICH_PARAMETER_T.IMPULSE_COUNTER;
                Args.Params = Params;
                OnSetParmeter(Args);
            }

        }
        private void txtCounter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Enter)) && (RefText != txtCounter.Text))
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.ImpulseCounter = Convert.ToInt32(txtCounter.Text);

                Args.Parameter = WHICH_PARAMETER_T.IMPULSE_COUNTER;
                Args.Params = Params;
                OnSetParmeter(Args);

                RefText = txtCounter.Text;
            }
        }
        #endregion

        private void cmbTriggerChannel_Enter(object sender, EventArgs e)
        {
            RefText = cmbTriggerChannel.Text;
        }

        private void cmbTriggerChannel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == Convert.ToChar(Keys.Enter)) && (RefText != cmbTriggerChannel.Text) && (cmbTriggerChannel.Text.Length > 0))
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                if (cmbTriggerChannel.SelectedIndex >= cmbTriggerChannel.Items.Count - 1)
                {
                    Params.TriggerChannel = 255;
                }
                else
                {
                    Params.TriggerChannel = Convert.ToUInt16(cmbTriggerChannel.SelectedIndex);
                }


                Args.Parameter = WHICH_PARAMETER_T.TRIGGER;
                Args.Params = Params;
                OnSetParmeter(Args);

                RefText = txtDelay_ms.Text;
            }
        }

        private void cmbTriggerChannel_Leave(object sender, EventArgs e)
        {
            if ((RefText != cmbTriggerChannel.Text) && (cmbTriggerChannel.Text.Length > 0))
            {
                SetParameterEventArgs Args = new SetParameterEventArgs();

                Params.TriggerChannel = Convert.ToUInt16(cmbTriggerChannel.SelectedIndex);
                Params.Triggering = ledTriggerEnable.On;

                Args.Parameter = WHICH_PARAMETER_T.TRIGGER;
                Args.Params = Params;
                OnSetParmeter(Args);

                RefText = txtDelay_ms.Text;
            }
        }
    }
    public class SetParameterEventArgs : EventArgs
    {
        public WHICH_PARAMETER_T Parameter { get; set; }
        public MODE_T Mode { get; set; }
        public PARAMS_T Params { get; set; }
    }
}
