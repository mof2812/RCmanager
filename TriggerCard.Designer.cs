namespace TriggerCard
{
    partial class TriggerCard
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
            this.trigger_4 = new Trigger.Trigger();
            this.trigger_3 = new Trigger.Trigger();
            this.trigger_2 = new Trigger.Trigger();
            this.trigger_1 = new Trigger.Trigger();
            this.SuspendLayout();
            // 
            // trigger_4
            // 
            this.trigger_4.BackColor = System.Drawing.Color.White;
            this.trigger_4.Location = new System.Drawing.Point(1048, 51);
            this.trigger_4.Name = "trigger_4";
            this.trigger_4.NightMode = false;
            this.trigger_4.Size = new System.Drawing.Size(350, 100);
            this.trigger_4.TabIndex = 5;
            this.trigger_4.TriggerName = "Trigger 1";
            this.trigger_4.TriggerStatus = false;
            // 
            // trigger_3
            // 
            this.trigger_3.BackColor = System.Drawing.Color.White;
            this.trigger_3.Location = new System.Drawing.Point(699, 51);
            this.trigger_3.Name = "trigger_3";
            this.trigger_3.NightMode = false;
            this.trigger_3.Size = new System.Drawing.Size(351, 100);
            this.trigger_3.TabIndex = 4;
            this.trigger_3.TriggerName = "Trigger 1";
            this.trigger_3.TriggerStatus = false;
            // 
            // trigger_2
            // 
            this.trigger_2.BackColor = System.Drawing.Color.White;
            this.trigger_2.Location = new System.Drawing.Point(350, 51);
            this.trigger_2.Name = "trigger_2";
            this.trigger_2.NightMode = false;
            this.trigger_2.Size = new System.Drawing.Size(350, 100);
            this.trigger_2.TabIndex = 1;
            this.trigger_2.TriggerName = "Trigger 1";
            this.trigger_2.TriggerStatus = false;
            // 
            // trigger_1
            // 
            this.trigger_1.BackColor = System.Drawing.Color.White;
            this.trigger_1.Location = new System.Drawing.Point(0, 51);
            this.trigger_1.Name = "trigger_1";
            this.trigger_1.NightMode = false;
            this.trigger_1.Size = new System.Drawing.Size(351, 100);
            this.trigger_1.TabIndex = 0;
            this.trigger_1.TriggerName = "Trigger 1";
            this.trigger_1.TriggerStatus = false;
            // 
            // TriggerCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.trigger_4);
            this.Controls.Add(this.trigger_3);
            this.Controls.Add(this.trigger_2);
            this.Controls.Add(this.trigger_1);
            this.Name = "TriggerCard";
            this.Size = new System.Drawing.Size(1398, 200);
            this.ResumeLayout(false);

        }

        #endregion

        private Trigger.Trigger trigger_1;
        private Trigger.Trigger trigger_2;
        private Trigger.Trigger trigger_4;
        private Trigger.Trigger trigger_3;
    }
}
