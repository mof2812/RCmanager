namespace RCmanager
{
    partial class RCmanager
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
            this.components = new System.ComponentModel.Container();
            this.tabRelayCard = new System.Windows.Forms.TabControl();
            this.tabSheetSwitchingCard = new System.Windows.Forms.TabPage();
            this.relayCard = new RelayCard.RelayCard();
            this.tabSheetPowerSupply = new System.Windows.Forms.TabPage();
            this.relayCardPowerSupply = new RelayCard.RelayCard();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainSettingsSerialInterface = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainSettingsView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainSettingsViewNightMode = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.serialCommunication = new SerialCommunication.SerialCommunication(this.components);
            this.tabRelayCard.SuspendLayout();
            this.tabSheetSwitchingCard.SuspendLayout();
            this.tabSheetPowerSupply.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabRelayCard
            // 
            this.tabRelayCard.Controls.Add(this.tabSheetSwitchingCard);
            this.tabRelayCard.Controls.Add(this.tabSheetPowerSupply);
            this.tabRelayCard.Location = new System.Drawing.Point(3, 58);
            this.tabRelayCard.Name = "tabRelayCard";
            this.tabRelayCard.SelectedIndex = 0;
            this.tabRelayCard.Size = new System.Drawing.Size(1433, 1059);
            this.tabRelayCard.TabIndex = 0;
            this.tabRelayCard.Tag = "";
            // 
            // tabSheetSwitchingCard
            // 
            this.tabSheetSwitchingCard.Controls.Add(this.relayCard);
            this.tabSheetSwitchingCard.Location = new System.Drawing.Point(4, 29);
            this.tabSheetSwitchingCard.Name = "tabSheetSwitchingCard";
            this.tabSheetSwitchingCard.Padding = new System.Windows.Forms.Padding(3);
            this.tabSheetSwitchingCard.Size = new System.Drawing.Size(1425, 1026);
            this.tabSheetSwitchingCard.TabIndex = 0;
            this.tabSheetSwitchingCard.Text = "Schaltkanäle";
            this.tabSheetSwitchingCard.UseVisualStyleBackColor = true;
            // 
            // relayCard
            // 
            this.relayCard.BackColor = System.Drawing.Color.Red;
            this.relayCard.Location = new System.Drawing.Point(9, 7);
            this.relayCard.Name = "relayCard";
            this.relayCard.Size = new System.Drawing.Size(1410, 1013);
            this.relayCard.TabIndex = 0;
            // 
            // tabSheetPowerSupply
            // 
            this.tabSheetPowerSupply.Controls.Add(this.relayCardPowerSupply);
            this.tabSheetPowerSupply.Location = new System.Drawing.Point(4, 29);
            this.tabSheetPowerSupply.Name = "tabSheetPowerSupply";
            this.tabSheetPowerSupply.Padding = new System.Windows.Forms.Padding(3);
            this.tabSheetPowerSupply.Size = new System.Drawing.Size(1425, 1026);
            this.tabSheetPowerSupply.TabIndex = 1;
            this.tabSheetPowerSupply.Text = "Netzteil";
            this.tabSheetPowerSupply.UseVisualStyleBackColor = true;
            // 
            // relayCardPowerSupply
            // 
            this.relayCardPowerSupply.BackColor = System.Drawing.Color.Red;
            this.relayCardPowerSupply.Location = new System.Drawing.Point(9, 7);
            this.relayCardPowerSupply.Name = "relayCardPowerSupply";
            this.relayCardPowerSupply.Size = new System.Drawing.Size(1408, 1013);
            this.relayCardPowerSupply.TabIndex = 0;
            // 
            // menuMain
            // 
            this.menuMain.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.menuMainSettings});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1438, 33);
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(69, 29);
            this.dateiToolStripMenuItem.Text = "Datei";
            // 
            // menuMainSettings
            // 
            this.menuMainSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainSettingsSerialInterface,
            this.menuMainSettingsView});
            this.menuMainSettings.Name = "menuMainSettings";
            this.menuMainSettings.Size = new System.Drawing.Size(132, 29);
            this.menuMainSettings.Text = "Einstellungen";
            // 
            // menuMainSettingsSerialInterface
            // 
            this.menuMainSettingsSerialInterface.Name = "menuMainSettingsSerialInterface";
            this.menuMainSettingsSerialInterface.Size = new System.Drawing.Size(268, 34);
            this.menuMainSettingsSerialInterface.Text = "Serielle Schnittstelle";
            this.menuMainSettingsSerialInterface.Click += new System.EventHandler(this.menuMainSettingsSerialInterface_Click);
            // 
            // menuMainSettingsView
            // 
            this.menuMainSettingsView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMainSettingsViewNightMode});
            this.menuMainSettingsView.Name = "menuMainSettingsView";
            this.menuMainSettingsView.Size = new System.Drawing.Size(268, 34);
            this.menuMainSettingsView.Text = "Ansicht";
            // 
            // menuMainSettingsViewNightMode
            // 
            this.menuMainSettingsViewNightMode.Checked = true;
            this.menuMainSettingsViewNightMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuMainSettingsViewNightMode.Name = "menuMainSettingsViewNightMode";
            this.menuMainSettingsViewNightMode.Size = new System.Drawing.Size(216, 34);
            this.menuMainSettingsViewNightMode.Text = "Nachtmodus";
            this.menuMainSettingsViewNightMode.Click += new System.EventHandler(this.menuMainSettingsViewNightMode_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Location = new System.Drawing.Point(0, 1122);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1438, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // serialCommunication
            // 
            this.serialCommunication.Bitmap = "Relay_sm.jpg";
            this.serialCommunication.DefaultBaudrate = 9600;
            this.serialCommunication.DefaultDatabits = ((byte)(8));
            this.serialCommunication.DefaultSerialPort = "COM1";
            this.serialCommunication.DefaultStopbits = System.IO.Ports.StopBits.One;
            this.serialCommunication.UseDelimiters = true;
            this.serialCommunication.SmallVersion = true;
            // 
            // RCmanager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1438, 1144);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabRelayCard);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.Name = "RCmanager";
            this.Text = "Form1";
            this.tabRelayCard.ResumeLayout(false);
            this.tabSheetSwitchingCard.ResumeLayout(false);
            this.tabSheetPowerSupply.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SerialCommunication.SerialCommunication serialCommunication;
        private System.Windows.Forms.TabControl tabRelayCard;
        private System.Windows.Forms.TabPage tabSheetSwitchingCard;
        private RelayCard.RelayCard relayCard;
        private System.Windows.Forms.TabPage tabSheetPowerSupply;
        private RelayCard.RelayCard relayCardPowerSupply;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuMainSettings;
        private System.Windows.Forms.ToolStripMenuItem menuMainSettingsSerialInterface;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuMainSettingsView;
        private System.Windows.Forms.ToolStripMenuItem menuMainSettingsViewNightMode;
    }
}

