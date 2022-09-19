namespace TriggerMonitoring
{
    partial class TriggerMonitoring
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
            this.ledTrigger = new Bulb.LedBulb();
            this.lblTriggerName = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            // TriggerMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTriggerName);
            this.Controls.Add(this.ledTrigger);
            this.Name = "TriggerMonitoring";
            this.Size = new System.Drawing.Size(351, 100);
            this.ResumeLayout(false);

        }

        #endregion

        private Bulb.LedBulb ledTrigger;
        private System.Windows.Forms.Label lblTriggerName;
    }
}
