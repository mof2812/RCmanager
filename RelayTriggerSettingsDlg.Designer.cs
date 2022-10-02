namespace RelayTriggerSettingsDlg
{
    partial class RelayTriggerSettingsDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.Trigger1Settings = new TriggerSettings.TriggerSettings();
            this.Trigger3Settings = new TriggerSettings.TriggerSettings();
            this.Trigger4Settings = new TriggerSettings.TriggerSettings();
            this.Trigger2Settings = new TriggerSettings.TriggerSettings();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(970, 481);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(157, 62);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(1134, 481);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(157, 62);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // Trigger1Settings
            // 
            this.Trigger1Settings.Location = new System.Drawing.Point(13, 13);
            this.Trigger1Settings.Name = "Trigger1Settings";
            this.Trigger1Settings.ReTrigger = false;
            this.Trigger1Settings.Size = new System.Drawing.Size(641, 234);
            this.Trigger1Settings.TabIndex = 2;
            this.Trigger1Settings.TriggerEnable = false;
            this.Trigger1Settings.TriggerLevel = 0F;
            this.Trigger1Settings.TriggerName = "Trigger #";
            // 
            // Trigger3Settings
            // 
            this.Trigger3Settings.Location = new System.Drawing.Point(13, 241);
            this.Trigger3Settings.Name = "Trigger3Settings";
            this.Trigger3Settings.ReTrigger = false;
            this.Trigger3Settings.Size = new System.Drawing.Size(641, 234);
            this.Trigger3Settings.TabIndex = 3;
            this.Trigger3Settings.TriggerEnable = false;
            this.Trigger3Settings.TriggerLevel = 0F;
            this.Trigger3Settings.TriggerName = "Trigger #";
            // 
            // Trigger4Settings
            // 
            this.Trigger4Settings.Location = new System.Drawing.Point(657, 241);
            this.Trigger4Settings.Name = "Trigger4Settings";
            this.Trigger4Settings.ReTrigger = false;
            this.Trigger4Settings.Size = new System.Drawing.Size(641, 234);
            this.Trigger4Settings.TabIndex = 5;
            this.Trigger4Settings.TriggerEnable = false;
            this.Trigger4Settings.TriggerLevel = 0F;
            this.Trigger4Settings.TriggerName = "Trigger #";
            // 
            // Trigger2Settings
            // 
            this.Trigger2Settings.Location = new System.Drawing.Point(657, 13);
            this.Trigger2Settings.Name = "Trigger2Settings";
            this.Trigger2Settings.ReTrigger = false;
            this.Trigger2Settings.Size = new System.Drawing.Size(641, 234);
            this.Trigger2Settings.TabIndex = 4;
            this.Trigger2Settings.TriggerEnable = false;
            this.Trigger2Settings.TriggerLevel = 0F;
            this.Trigger2Settings.TriggerName = "Trigger #";
            // 
            // RelayTriggerSettingsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1308, 551);
            this.Controls.Add(this.Trigger4Settings);
            this.Controls.Add(this.Trigger2Settings);
            this.Controls.Add(this.Trigger3Settings);
            this.Controls.Add(this.Trigger1Settings);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.Name = "RelayTriggerSettingsDlg";
            this.Text = "Trigger-Einstellungen";
            this.Load += new System.EventHandler(this.RelayTriggerSettingsDlg_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOkay;
        private TriggerSettings.TriggerSettings Trigger1Settings;
        private TriggerSettings.TriggerSettings Trigger3Settings;
        private TriggerSettings.TriggerSettings Trigger4Settings;
        private TriggerSettings.TriggerSettings Trigger2Settings;
    }
}