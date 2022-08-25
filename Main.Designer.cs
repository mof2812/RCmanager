namespace RCmanager
{
    partial class MainForm
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.menu = new System.Windows.Forms.MenuStrip();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingsSerialPort = new System.Windows.Forms.ToolStripMenuItem();
            this.RelayCard = new RelayCard16.RelayCard16();
            this.ledBulb1 = new Bulb.LedBulb();
            this.rcCH1 = new RelayControl.RelayControl();
            this.dbgDebugging = new RCmanager.DebugControl();
            this.rcCH0 = new RelayControl.RelayControl();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSettings});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menu.Size = new System.Drawing.Size(1429, 24);
            this.menu.TabIndex = 5;
            this.menu.Text = "menuStrip1";
            // 
            // menuSettings
            // 
            this.menuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingsSerialPort});
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(90, 22);
            this.menuSettings.Text = "Einstellungen";
            // 
            // menuSettingsSerialPort
            // 
            this.menuSettingsSerialPort.Name = "menuSettingsSerialPort";
            this.menuSettingsSerialPort.Size = new System.Drawing.Size(178, 22);
            this.menuSettingsSerialPort.Text = "Serielle Schnittstelle";
            this.menuSettingsSerialPort.Click += new System.EventHandler(this.menuSettingsSerialPort_Click);
            // 
            // RelayCard
            // 
            this.RelayCard.BackColor = System.Drawing.Color.Black;
            this.RelayCard.BtnTextCard_1 = "Kanal 1 - 8";
            this.RelayCard.BtnTextCard_2 = "Kanal 9 - 16";
            this.RelayCard.Location = new System.Drawing.Point(482, 21);
            this.RelayCard.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.RelayCard.Name = "RelayCard";
            this.RelayCard.NightMode = false;
            this.RelayCard.Size = new System.Drawing.Size(1305, 766);
            this.RelayCard.TabIndex = 6;
            // 
            // ledBulb1
            // 
            this.ledBulb1.Location = new System.Drawing.Point(41, 379);
            this.ledBulb1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ledBulb1.Name = "ledBulb1";
            this.ledBulb1.On = true;
            this.ledBulb1.Size = new System.Drawing.Size(50, 15);
            this.ledBulb1.TabIndex = 4;
            this.ledBulb1.Text = "ledBulb1";
            // 
            // rcCH1
            // 
            this.rcCH1.ChartBackColor = System.Drawing.Color.White;
            this.rcCH1.ChartDataName = null;
            this.rcCH1.ChartLineColor = System.Drawing.Color.Empty;
            this.rcCH1.DelayTime = ((uint)(0u));
            this.rcCH1.Invert = false;
            this.rcCH1.Location = new System.Drawing.Point(19, 192);
            this.rcCH1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.rcCH1.Mode = RelayControl.MODE_T.OFF;
            this.rcCH1.Name = "rcCH1";
            this.rcCH1.NightMode = false;
            this.rcCH1.Size = new System.Drawing.Size(465, 164);
            this.rcCH1.TabIndex = 2;
            this.rcCH1.TimeOff = ((uint)(0u));
            this.rcCH1.TimeOn = ((uint)(0u));
            this.rcCH1.XRange = ((uint)(0u));
            // 
            // dbgDebugging
            // 
            this.dbgDebugging.BackColor = System.Drawing.Color.DimGray;
            this.dbgDebugging.ErrorBackGround = System.Drawing.Color.Red;
            this.dbgDebugging.ErrorForeGround = System.Drawing.Color.White;
            this.dbgDebugging.InfoBackGround = System.Drawing.Color.Blue;
            this.dbgDebugging.InfoForeGround = System.Drawing.Color.White;
            this.dbgDebugging.ListBoxSize = 1404;
            this.dbgDebugging.Location = new System.Drawing.Point(1, 671);
            this.dbgDebugging.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.dbgDebugging.Name = "dbgDebugging";
            this.dbgDebugging.ReceiveBackGround = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.dbgDebugging.ReceiveForeGround = System.Drawing.Color.Black;
            this.dbgDebugging.SendBackGround = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(0)))));
            this.dbgDebugging.SendForeGround = System.Drawing.Color.Black;
            this.dbgDebugging.SetApplication = "";
            this.dbgDebugging.Size = new System.Drawing.Size(1186, 184);
            this.dbgDebugging.TabIndex = 1;
            this.dbgDebugging.WarningBackGround = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dbgDebugging.WarningForeGround = System.Drawing.Color.White;
            // 
            // rcCH0
            // 
            this.rcCH0.ChartBackColor = System.Drawing.Color.White;
            this.rcCH0.ChartDataName = null;
            this.rcCH0.ChartLineColor = System.Drawing.Color.Empty;
            this.rcCH0.DelayTime = ((uint)(0u));
            this.rcCH0.Invert = false;
            this.rcCH0.Location = new System.Drawing.Point(19, 24);
            this.rcCH0.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.rcCH0.Mode = RelayControl.MODE_T.OFF;
            this.rcCH0.Name = "rcCH0";
            this.rcCH0.NightMode = false;
            this.rcCH0.Size = new System.Drawing.Size(465, 164);
            this.rcCH0.TabIndex = 0;
            this.rcCH0.TimeOff = ((uint)(0u));
            this.rcCH0.TimeOn = ((uint)(0u));
            this.rcCH0.XRange = ((uint)(0u));
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1429, 881);
            this.Controls.Add(this.RelayCard);
            this.Controls.Add(this.ledBulb1);
            this.Controls.Add(this.rcCH1);
            this.Controls.Add(this.dbgDebugging);
            this.Controls.Add(this.rcCH0);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "RCmanager - Steuerzentrale für Relaiskarten ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RelayControl.RelayControl rcCH0;
        private DebugControl dbgDebugging;
        private RelayControl.RelayControl rcCH1;
        private Bulb.LedBulb ledBulb1;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuSettingsSerialPort;
        private RelayCard16.RelayCard16 RelayCard;
    }
}

