namespace RelayMonitoring
{
    partial class RelayMonitoring
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
            this.ledStatus = new Bulb.LedBulb();
            this.lblSignalName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ledStatus
            // 
            this.ledStatus.Location = new System.Drawing.Point(33, 10);
            this.ledStatus.Name = "ledStatus";
            this.ledStatus.On = true;
            this.ledStatus.Size = new System.Drawing.Size(80, 80);
            this.ledStatus.TabIndex = 1;
            this.ledStatus.Text = "ledBulb1";
            // 
            // lblSignalName
            // 
            this.lblSignalName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignalName.Location = new System.Drawing.Point(176, 35);
            this.lblSignalName.Name = "lblSignalName";
            this.lblSignalName.Size = new System.Drawing.Size(453, 30);
            this.lblSignalName.TabIndex = 3;
            this.lblSignalName.Text = "label1";
            this.lblSignalName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RelayMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSignalName);
            this.Controls.Add(this.ledStatus);
            this.Name = "RelayMonitoring";
            this.Size = new System.Drawing.Size(700, 100);
            this.ResumeLayout(false);

        }

        #endregion

        private Bulb.LedBulb ledStatus;
        private System.Windows.Forms.Label lblSignalName;
    }
}
