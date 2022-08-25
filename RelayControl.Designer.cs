namespace RelayControl
{
    partial class RelayControl
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartRelay = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.conMenuRelayControl = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblEnable = new System.Windows.Forms.Label();
            this.ledEnable = new Bulb.LedBulb();
            this.ledInvertingMode = new Bulb.LedBulb();
            this.lblInvertingMode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartRelay)).BeginInit();
            this.conMenuRelayControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartRelay
            // 
            this.chartRelay.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chartRelay.ChartAreas.Add(chartArea1);
            this.chartRelay.ContextMenuStrip = this.conMenuRelayControl;
            legend1.Name = "Legend1";
            this.chartRelay.Legends.Add(legend1);
            this.chartRelay.Location = new System.Drawing.Point(0, 0);
            this.chartRelay.Name = "chartRelay";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartRelay.Series.Add(series1);
            this.chartRelay.Size = new System.Drawing.Size(700, 250);
            this.chartRelay.TabIndex = 2;
            this.chartRelay.Text = "chart1";
            // 
            // conMenuRelayControl
            // 
            this.conMenuRelayControl.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.conMenuRelayControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddSettingsMenuItem});
            this.conMenuRelayControl.Name = "conMenuRelayControl";
            this.conMenuRelayControl.Size = new System.Drawing.Size(244, 36);
            // 
            // AddSettingsMenuItem
            // 
            this.AddSettingsMenuItem.Name = "AddSettingsMenuItem";
            this.AddSettingsMenuItem.Size = new System.Drawing.Size(243, 32);
            this.AddSettingsMenuItem.Text = "Weiter Einstellungen";
            this.AddSettingsMenuItem.Click += new System.EventHandler(this.AddSettingsMenuItem_Click);
            // 
            // lblEnable
            // 
            this.lblEnable.BackColor = System.Drawing.Color.Transparent;
            this.lblEnable.Location = new System.Drawing.Point(584, 136);
            this.lblEnable.Name = "lblEnable";
            this.lblEnable.Size = new System.Drawing.Size(72, 23);
            this.lblEnable.TabIndex = 6;
            this.lblEnable.Text = "Freigabe";
            this.lblEnable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ledEnable
            // 
            this.ledEnable.Location = new System.Drawing.Point(546, 136);
            this.ledEnable.Name = "ledEnable";
            this.ledEnable.On = true;
            this.ledEnable.Size = new System.Drawing.Size(23, 23);
            this.ledEnable.TabIndex = 9;
            this.ledEnable.Text = "ledBulb1";
            // 
            // ledInvertingMode
            // 
            this.ledInvertingMode.Location = new System.Drawing.Point(546, 175);
            this.ledInvertingMode.Name = "ledInvertingMode";
            this.ledInvertingMode.On = true;
            this.ledInvertingMode.Size = new System.Drawing.Size(23, 23);
            this.ledInvertingMode.TabIndex = 11;
            this.ledInvertingMode.Text = "ledBulb1";
            // 
            // lblInvertingMode
            // 
            this.lblInvertingMode.BackColor = System.Drawing.Color.Transparent;
            this.lblInvertingMode.Location = new System.Drawing.Point(584, 175);
            this.lblInvertingMode.Name = "lblInvertingMode";
            this.lblInvertingMode.Size = new System.Drawing.Size(113, 23);
            this.lblInvertingMode.TabIndex = 10;
            this.lblInvertingMode.Text = "Invertierung";
            this.lblInvertingMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RelayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ledInvertingMode);
            this.Controls.Add(this.lblInvertingMode);
            this.Controls.Add(this.ledEnable);
            this.Controls.Add(this.lblEnable);
            this.Controls.Add(this.chartRelay);
            this.Name = "RelayControl";
            this.Size = new System.Drawing.Size(700, 250);
            ((System.ComponentModel.ISupportInitialize)(this.chartRelay)).EndInit();
            this.conMenuRelayControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartRelay;
        private System.Windows.Forms.Label lblEnable;
        private System.Windows.Forms.ContextMenuStrip conMenuRelayControl;
        private System.Windows.Forms.ToolStripMenuItem AddSettingsMenuItem;
        private Bulb.LedBulb ledEnable;
        private Bulb.LedBulb ledInvertingMode;
        private System.Windows.Forms.Label lblInvertingMode;
    }
}
