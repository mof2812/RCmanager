namespace Relay
{
    partial class Relay
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.conMenuRelay = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TriggerSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTriggerMode = new System.Windows.Forms.Label();
            this.lblInvertingMode = new System.Windows.Forms.Label();
            this.lblEnable = new System.Windows.Forms.Label();
            this.chartRelay = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ledTriggerMode = new Bulb.LedBulb();
            this.ledInvertingMode = new Bulb.LedBulb();
            this.ledEnable = new Bulb.LedBulb();
            this.MySetup = new MySettings.MySettings(this.components);
            this.conMenuRelay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRelay)).BeginInit();
            this.SuspendLayout();
            // 
            // conMenuRelay
            // 
            this.conMenuRelay.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.conMenuRelay.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddSettingsMenuItem,
            this.TriggerSettingsMenuItem});
            this.conMenuRelay.Name = "conMenuRelay";
            this.conMenuRelay.Size = new System.Drawing.Size(244, 68);
            // 
            // AddSettingsMenuItem
            // 
            this.AddSettingsMenuItem.Name = "AddSettingsMenuItem";
            this.AddSettingsMenuItem.Size = new System.Drawing.Size(243, 32);
            this.AddSettingsMenuItem.Text = "Weiter Einstellungen";
            this.AddSettingsMenuItem.Click += new System.EventHandler(this.AddSettingsMenuItem_Click);
            // 
            // TriggerSettingsMenuItem
            // 
            this.TriggerSettingsMenuItem.Name = "TriggerSettingsMenuItem";
            this.TriggerSettingsMenuItem.Size = new System.Drawing.Size(243, 32);
            this.TriggerSettingsMenuItem.Text = "Triggereinstellungen";
            // 
            // lblTriggerMode
            // 
            this.lblTriggerMode.BackColor = System.Drawing.Color.Transparent;
            this.lblTriggerMode.Location = new System.Drawing.Point(569, 17);
            this.lblTriggerMode.Name = "lblTriggerMode";
            this.lblTriggerMode.Size = new System.Drawing.Size(128, 23);
            this.lblTriggerMode.TabIndex = 27;
            this.lblTriggerMode.Text = "Triggerung";
            this.lblTriggerMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInvertingMode
            // 
            this.lblInvertingMode.BackColor = System.Drawing.Color.Transparent;
            this.lblInvertingMode.Location = new System.Drawing.Point(449, 17);
            this.lblInvertingMode.Name = "lblInvertingMode";
            this.lblInvertingMode.Size = new System.Drawing.Size(98, 23);
            this.lblInvertingMode.TabIndex = 25;
            this.lblInvertingMode.Text = "Invertierung";
            this.lblInvertingMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInvertingMode.Click += new System.EventHandler(this.ledInvertingMode_Click);
            // 
            // lblEnable
            // 
            this.lblEnable.BackColor = System.Drawing.Color.Transparent;
            this.lblEnable.Location = new System.Drawing.Point(335, 17);
            this.lblEnable.Name = "lblEnable";
            this.lblEnable.Size = new System.Drawing.Size(72, 23);
            this.lblEnable.TabIndex = 23;
            this.lblEnable.Text = "Freigabe";
            this.lblEnable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblEnable.Click += new System.EventHandler(this.ledEnable_Click);
            // 
            // chartRelay
            // 
            this.chartRelay.BackColor = System.Drawing.Color.Black;
            chartArea2.Name = "ChartArea1";
            this.chartRelay.ChartAreas.Add(chartArea2);
            this.chartRelay.ContextMenuStrip = this.conMenuRelay;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend2.Name = "Legend1";
            this.chartRelay.Legends.Add(legend2);
            this.chartRelay.Location = new System.Drawing.Point(0, 0);
            this.chartRelay.Name = "chartRelay";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartRelay.Series.Add(series2);
            this.chartRelay.Size = new System.Drawing.Size(700, 250);
            this.chartRelay.TabIndex = 29;
            this.chartRelay.Text = "chart1";
            // 
            // ledTriggerMode
            // 
            this.ledTriggerMode.Location = new System.Drawing.Point(547, 17);
            this.ledTriggerMode.Name = "ledTriggerMode";
            this.ledTriggerMode.On = true;
            this.ledTriggerMode.Size = new System.Drawing.Size(23, 23);
            this.ledTriggerMode.TabIndex = 28;
            this.ledTriggerMode.Text = "ledBulb1";
            // 
            // ledInvertingMode
            // 
            this.ledInvertingMode.Location = new System.Drawing.Point(427, 17);
            this.ledInvertingMode.Name = "ledInvertingMode";
            this.ledInvertingMode.On = true;
            this.ledInvertingMode.Size = new System.Drawing.Size(23, 23);
            this.ledInvertingMode.TabIndex = 26;
            this.ledInvertingMode.Text = "ledBulb1";
            this.ledInvertingMode.Click += new System.EventHandler(this.ledInvertingMode_Click);
            // 
            // ledEnable
            // 
            this.ledEnable.Location = new System.Drawing.Point(307, 17);
            this.ledEnable.Name = "ledEnable";
            this.ledEnable.On = true;
            this.ledEnable.Size = new System.Drawing.Size(23, 23);
            this.ledEnable.TabIndex = 24;
            this.ledEnable.Text = "ledBulb1";
            this.ledEnable.Click += new System.EventHandler(this.ledEnable_Click);
            // 
            // MySetup
            // 
            this.MySetup.DefaultDataFileName = null;
            // 
            // Relay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ledTriggerMode);
            this.Controls.Add(this.lblTriggerMode);
            this.Controls.Add(this.ledInvertingMode);
            this.Controls.Add(this.lblInvertingMode);
            this.Controls.Add(this.ledEnable);
            this.Controls.Add(this.lblEnable);
            this.Controls.Add(this.chartRelay);
            this.Name = "Relay";
            this.Size = new System.Drawing.Size(700, 250);
            this.conMenuRelay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartRelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip conMenuRelay;
        private System.Windows.Forms.ToolStripMenuItem AddSettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TriggerSettingsMenuItem;
        private Bulb.LedBulb ledTriggerMode;
        private System.Windows.Forms.Label lblTriggerMode;
        private Bulb.LedBulb ledInvertingMode;
        private System.Windows.Forms.Label lblInvertingMode;
        private Bulb.LedBulb ledEnable;
        private System.Windows.Forms.Label lblEnable;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRelay;
        private MySettings.MySettings MySetup;
    }
}
