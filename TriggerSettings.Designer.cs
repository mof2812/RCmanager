namespace TriggerSettings
{
    partial class TriggerSettings
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpTriggerSettings = new System.Windows.Forms.GroupBox();
            this.lblTriggerLevelUnit = new System.Windows.Forms.Label();
            this.txtTriggerLevel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbTriggerMode = new System.Windows.Forms.ComboBox();
            this.lblTriggerLevel = new System.Windows.Forms.Label();
            this.tbTriggerLevel = new System.Windows.Forms.TrackBar();
            this.lblEnable = new System.Windows.Forms.Label();
            this.ledEnable = new Bulb.LedBulb();
            this.grpTriggerSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTriggerLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // grpTriggerSettings
            // 
            this.grpTriggerSettings.Controls.Add(this.lblTriggerLevelUnit);
            this.grpTriggerSettings.Controls.Add(this.txtTriggerLevel);
            this.grpTriggerSettings.Controls.Add(this.label2);
            this.grpTriggerSettings.Controls.Add(this.cbTriggerMode);
            this.grpTriggerSettings.Controls.Add(this.lblTriggerLevel);
            this.grpTriggerSettings.Controls.Add(this.tbTriggerLevel);
            this.grpTriggerSettings.Controls.Add(this.lblEnable);
            this.grpTriggerSettings.Controls.Add(this.ledEnable);
            this.grpTriggerSettings.Location = new System.Drawing.Point(7, 9);
            this.grpTriggerSettings.Name = "grpTriggerSettings";
            this.grpTriggerSettings.Size = new System.Drawing.Size(626, 217);
            this.grpTriggerSettings.TabIndex = 1;
            this.grpTriggerSettings.TabStop = false;
            this.grpTriggerSettings.Text = "Trigger #";
            // 
            // lblTriggerLevelUnit
            // 
            this.lblTriggerLevelUnit.AutoSize = true;
            this.lblTriggerLevelUnit.Location = new System.Drawing.Point(591, 128);
            this.lblTriggerLevelUnit.Name = "lblTriggerLevelUnit";
            this.lblTriggerLevelUnit.Size = new System.Drawing.Size(20, 20);
            this.lblTriggerLevelUnit.TabIndex = 9;
            this.lblTriggerLevelUnit.Text = "V";
            // 
            // txtTriggerLevel
            // 
            this.txtTriggerLevel.Location = new System.Drawing.Point(528, 128);
            this.txtTriggerLevel.Name = "txtTriggerLevel";
            this.txtTriggerLevel.Size = new System.Drawing.Size(56, 26);
            this.txtTriggerLevel.TabIndex = 8;
            this.txtTriggerLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTriggerLevel.TextChanged += new System.EventHandler(this.txtTriggerLevel_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(38, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 26);
            this.label2.TabIndex = 7;
            this.label2.Text = "Trigger-Mode";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbTriggerMode
            // 
            this.cbTriggerMode.FormattingEnabled = true;
            this.cbTriggerMode.Items.AddRange(new object[] {
            "LOW-Pegel",
            "HIGH-Pegel",
            "HIGH -> LOW Pegel",
            "LOW -> HIGH Pegel",
            "Triggerpegel - Unterschreitung ",
            "Triggerpegel - Überschreitung",
            "Triggerpegel - abfallende Flanke",
            "Triggerpegel - ansteigende Flanke",
            "IRQ fallend",
            "IRQ steigend"});
            this.cbTriggerMode.Location = new System.Drawing.Point(38, 128);
            this.cbTriggerMode.Name = "cbTriggerMode";
            this.cbTriggerMode.Size = new System.Drawing.Size(229, 28);
            this.cbTriggerMode.TabIndex = 4;
            this.cbTriggerMode.SelectedIndexChanged += new System.EventHandler(this.cbTriggerMode_SelectedIndexChanged);
            // 
            // lblTriggerLevel
            // 
            this.lblTriggerLevel.Location = new System.Drawing.Point(313, 94);
            this.lblTriggerLevel.Name = "lblTriggerLevel";
            this.lblTriggerLevel.Size = new System.Drawing.Size(208, 26);
            this.lblTriggerLevel.TabIndex = 3;
            this.lblTriggerLevel.Text = "Trigger-Spannung";
            this.lblTriggerLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTriggerLevel
            // 
            this.tbTriggerLevel.LargeChange = 500;
            this.tbTriggerLevel.Location = new System.Drawing.Point(313, 128);
            this.tbTriggerLevel.Maximum = 5000;
            this.tbTriggerLevel.Name = "tbTriggerLevel";
            this.tbTriggerLevel.Size = new System.Drawing.Size(208, 69);
            this.tbTriggerLevel.SmallChange = 50;
            this.tbTriggerLevel.TabIndex = 2;
            this.tbTriggerLevel.Tag = "";
            this.tbTriggerLevel.TickFrequency = 50;
            this.tbTriggerLevel.ValueChanged += new System.EventHandler(this.tbTriggerLevel_ValueChanged);
            // 
            // lblEnable
            // 
            this.lblEnable.AutoSize = true;
            this.lblEnable.Location = new System.Drawing.Point(67, 45);
            this.lblEnable.Name = "lblEnable";
            this.lblEnable.Size = new System.Drawing.Size(72, 20);
            this.lblEnable.TabIndex = 1;
            this.lblEnable.Text = "Freigabe";
            this.lblEnable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblEnable.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ledEnable_MouseClick);
            // 
            // ledEnable
            // 
            this.ledEnable.Location = new System.Drawing.Point(38, 44);
            this.ledEnable.Name = "ledEnable";
            this.ledEnable.On = true;
            this.ledEnable.Size = new System.Drawing.Size(23, 23);
            this.ledEnable.TabIndex = 0;
            this.ledEnable.Tag = "Enable";
            this.ledEnable.Text = "ledBulb1";
            this.ledEnable.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ledEnable_MouseClick);
            // 
            // TriggerSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpTriggerSettings);
            this.Name = "TriggerSettings";
            this.Size = new System.Drawing.Size(641, 234);
            this.grpTriggerSettings.ResumeLayout(false);
            this.grpTriggerSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTriggerLevel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bulb.LedBulb ledEnable;
        private System.Windows.Forms.GroupBox grpTriggerSettings;
        private System.Windows.Forms.Label lblEnable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbTriggerMode;
        private System.Windows.Forms.Label lblTriggerLevel;
        private System.Windows.Forms.TrackBar tbTriggerLevel;
        private System.Windows.Forms.Label lblTriggerLevelUnit;
        private System.Windows.Forms.TextBox txtTriggerLevel;
    }
}
