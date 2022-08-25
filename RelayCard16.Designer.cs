namespace RelayCard16
{
    partial class RelayCard16
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
            this.grpCards = new System.Windows.Forms.GroupBox();
            this.rcCard2 = new RelayControl8.RelayControl8();
            this.rcCard1 = new RelayControl8.RelayControl8();
            this.rbPage2 = new System.Windows.Forms.RadioButton();
            this.rbPage1 = new System.Windows.Forms.RadioButton();
            this.CommunicationTimer = new System.Windows.Forms.Timer(this.components);
            this.SerialPort = new System.IO.Ports.SerialPort(this.components);
            this.grpCards.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCards
            // 
            this.grpCards.Controls.Add(this.rcCard2);
            this.grpCards.Controls.Add(this.rcCard1);
            this.grpCards.Controls.Add(this.rbPage2);
            this.grpCards.Controls.Add(this.rbPage1);
            this.grpCards.ForeColor = System.Drawing.SystemColors.Control;
            this.grpCards.Location = new System.Drawing.Point(12, 9);
            this.grpCards.Name = "grpCards";
            this.grpCards.Size = new System.Drawing.Size(1464, 1118);
            this.grpCards.TabIndex = 0;
            this.grpCards.TabStop = false;
            this.grpCards.Text = "                           ";
            // 
            // rcCard2
            // 
            this.rcCard2.BackColor = System.Drawing.Color.Black;
            this.rcCard2.Location = new System.Drawing.Point(6, 47);
            this.rcCard2.Name = "rcCard2";
            this.rcCard2.NightMode = false;
            this.rcCard2.Size = new System.Drawing.Size(1403, 1009);
            this.rcCard2.TabIndex = 3;
            // 
            // rcCard1
            // 
            this.rcCard1.BackColor = System.Drawing.Color.Black;
            this.rcCard1.Location = new System.Drawing.Point(6, 47);
            this.rcCard1.Name = "rcCard1";
            this.rcCard1.NightMode = false;
            this.rcCard1.Size = new System.Drawing.Size(1403, 1009);
            this.rcCard1.TabIndex = 2;
            // 
            // rbPage2
            // 
            this.rbPage2.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbPage2.AutoSize = true;
            this.rbPage2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rbPage2.Location = new System.Drawing.Point(106, 0);
            this.rbPage2.Name = "rbPage2";
            this.rbPage2.Size = new System.Drawing.Size(103, 30);
            this.rbPage2.TabIndex = 1;
            this.rbPage2.TabStop = true;
            this.rbPage2.Text = "Kanal 9 - 16";
            this.rbPage2.UseVisualStyleBackColor = true;
            // 
            // rbPage1
            // 
            this.rbPage1.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbPage1.AutoSize = true;
            this.rbPage1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rbPage1.Location = new System.Drawing.Point(6, 0);
            this.rbPage1.Name = "rbPage1";
            this.rbPage1.Size = new System.Drawing.Size(94, 30);
            this.rbPage1.TabIndex = 0;
            this.rbPage1.TabStop = true;
            this.rbPage1.Text = "Kanal 1 - 8";
            this.rbPage1.UseVisualStyleBackColor = true;
            this.rbPage1.CheckedChanged += new System.EventHandler(this.rbPage1_CheckedChanged);
            // 
            // CommunicationTimer
            // 
            this.CommunicationTimer.Interval = 10;
            this.CommunicationTimer.Tick += new System.EventHandler(this.CommunicationTimer_Tick);
            // 
            // RelayCard16
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.grpCards);
            this.Name = "RelayCard16";
            this.Size = new System.Drawing.Size(1958, 1178);
            this.grpCards.ResumeLayout(false);
            this.grpCards.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCards;
        private System.Windows.Forms.RadioButton rbPage2;
        private System.Windows.Forms.RadioButton rbPage1;
        private RelayControl8.RelayControl8 rcCard2;
        private RelayControl8.RelayControl8 rcCard1;
        private System.Windows.Forms.Timer CommunicationTimer;
        private System.IO.Ports.SerialPort SerialPort;
    }
}
