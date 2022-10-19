namespace Trigger
{
    partial class Trigger
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
            this.components = new System.ComponentModel.Container();
            this.lblTriggerName = new System.Windows.Forms.Label();
            this.lblInputVoltage = new System.Windows.Forms.Label();
            this.ledTrigger = new Bulb.LedBulb();
            this.MySetup = new MyTriggerSettings.MyTriggerSettings(this.components);
            this.lblTriggerLevel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTriggerName
            // 
            this.lblTriggerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTriggerName.Location = new System.Drawing.Point(176, 37);
            this.lblTriggerName.Name = "lblTriggerName";
            this.lblTriggerName.Size = new System.Drawing.Size(161, 30);
            this.lblTriggerName.TabIndex = 3;
            this.lblTriggerName.Text = "Trigger 1";
            this.lblTriggerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInputVoltage
            // 
            this.lblInputVoltage.AutoSize = true;
            this.lblInputVoltage.Location = new System.Drawing.Point(98, 80);
            this.lblInputVoltage.Name = "lblInputVoltage";
            this.lblInputVoltage.Size = new System.Drawing.Size(51, 20);
            this.lblInputVoltage.TabIndex = 4;
            this.lblInputVoltage.Text = "label1";
            // 
            // ledTrigger
            // 
            this.ledTrigger.Color = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(202)))), ((int)(((byte)(255)))));
            this.ledTrigger.Location = new System.Drawing.Point(33, 10);
            this.ledTrigger.Name = "ledTrigger";
            this.ledTrigger.On = true;
            this.ledTrigger.Size = new System.Drawing.Size(80, 80);
            this.ledTrigger.TabIndex = 1;
            this.ledTrigger.Text = "ledBulb1";
            // 
            // MySetup
            // 
            this.MySetup.DefaultDataFileName = null;
            // 
            // lblTriggerLevel
            // 
            this.lblTriggerLevel.AutoSize = true;
            this.lblTriggerLevel.Location = new System.Drawing.Point(98, -1);
            this.lblTriggerLevel.Name = "lblTriggerLevel";
            this.lblTriggerLevel.Size = new System.Drawing.Size(51, 20);
            this.lblTriggerLevel.TabIndex = 5;
            this.lblTriggerLevel.Text = "label1";
            // 
            // Trigger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTriggerLevel);
            this.Controls.Add(this.lblInputVoltage);
            this.Controls.Add(this.lblTriggerName);
            this.Controls.Add(this.ledTrigger);
            this.Name = "Trigger";
            this.Size = new System.Drawing.Size(351, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bulb.LedBulb ledTrigger;
        private System.Windows.Forms.Label lblTriggerName;
        private MyTriggerSettings.MyTriggerSettings MySetup;
        private System.Windows.Forms.Label lblInputVoltage;
        private System.Windows.Forms.Label lblTriggerLevel;
    }
}
