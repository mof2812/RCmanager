namespace RelayCardUserMonitoring
{
    partial class RelayCardUserMonitoring
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.UserChart_1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lbSignalSelect_Chart_1 = new System.Windows.Forms.ListBox();
            this.btnDelSignal_Chart_1 = new System.Windows.Forms.Button();
            this.cmbTimebaseChart_2 = new System.Windows.Forms.ComboBox();
            this.grpMonitor_1 = new System.Windows.Forms.GroupBox();
            this.lblTimebase_1 = new System.Windows.Forms.Label();
            this.cmbTimebaseChart_1 = new System.Windows.Forms.ComboBox();
            this.btnAddSignal_Chart_1 = new System.Windows.Forms.Button();
            this.grpMonitor_2 = new System.Windows.Forms.GroupBox();
            this.lblTimebase_2 = new System.Windows.Forms.Label();
            this.lbSignalSelect_Chart_2 = new System.Windows.Forms.ListBox();
            this.UserChart_2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnDelSignal_Chart_2 = new System.Windows.Forms.Button();
            this.btnAddSignal_Chart_2 = new System.Windows.Forms.Button();
            this.btnStartStopMonitoring = new System.Windows.Forms.Button();
            this.MyUserMonitoringSettings = new MyUserMonitoringSettings.MyUserMonitoringSettings(this.components);
            this.txtStates = new System.Windows.Forms.TextBox();
            this.txtElapsed = new System.Windows.Forms.TextBox();
            this.ledSeries0 = new Bulb.LedBulb();
            ((System.ComponentModel.ISupportInitialize)(this.UserChart_1)).BeginInit();
            this.grpMonitor_1.SuspendLayout();
            this.grpMonitor_2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserChart_2)).BeginInit();
            this.SuspendLayout();
            // 
            // UserChart_1
            // 
            chartArea1.Name = "ChartArea1";
            this.UserChart_1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.UserChart_1.Legends.Add(legend1);
            this.UserChart_1.Location = new System.Drawing.Point(40, 37);
            this.UserChart_1.Name = "UserChart_1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.UserChart_1.Series.Add(series1);
            this.UserChart_1.Size = new System.Drawing.Size(1049, 357);
            this.UserChart_1.TabIndex = 18;
            this.UserChart_1.Text = "chart1";
            // 
            // lbSignalSelect_Chart_1
            // 
            this.lbSignalSelect_Chart_1.FormattingEnabled = true;
            this.lbSignalSelect_Chart_1.ItemHeight = 20;
            this.lbSignalSelect_Chart_1.Location = new System.Drawing.Point(1118, 105);
            this.lbSignalSelect_Chart_1.Name = "lbSignalSelect_Chart_1";
            this.lbSignalSelect_Chart_1.Size = new System.Drawing.Size(239, 204);
            this.lbSignalSelect_Chart_1.TabIndex = 8;
            this.lbSignalSelect_Chart_1.Visible = false;
            this.lbSignalSelect_Chart_1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lbSignalSelect_Chart_1_KeyPress);
            this.lbSignalSelect_Chart_1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbSignalSelect_Chart_1_MouseDoubleClick);
            // 
            // btnDelSignal_Chart_1
            // 
            this.btnDelSignal_Chart_1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnDelSignal_Chart_1.Location = new System.Drawing.Point(1273, 25);
            this.btnDelSignal_Chart_1.Name = "btnDelSignal_Chart_1";
            this.btnDelSignal_Chart_1.Size = new System.Drawing.Size(84, 70);
            this.btnDelSignal_Chart_1.TabIndex = 3;
            this.btnDelSignal_Chart_1.Text = "Signal -";
            this.btnDelSignal_Chart_1.UseVisualStyleBackColor = true;
            this.btnDelSignal_Chart_1.Click += new System.EventHandler(this.btnDelSignal_Chart_1_Click);
            // 
            // cmbTimebaseChart_2
            // 
            this.cmbTimebaseChart_2.FormattingEnabled = true;
            this.cmbTimebaseChart_2.Items.AddRange(new object[] {
            "100ms",
            "200ms",
            "500ms",
            "1s",
            "2s",
            "5s",
            "10s",
            "20s",
            "50s",
            "1m",
            "2m",
            "5m",
            "10m",
            "20m",
            "50m",
            "1h",
            "2h",
            "5h",
            "10h",
            "20h",
            "50h"});
            this.cmbTimebaseChart_2.Location = new System.Drawing.Point(1120, 392);
            this.cmbTimebaseChart_2.Name = "cmbTimebaseChart_2";
            this.cmbTimebaseChart_2.Size = new System.Drawing.Size(239, 28);
            this.cmbTimebaseChart_2.TabIndex = 16;
            // 
            // grpMonitor_1
            // 
            this.grpMonitor_1.Controls.Add(this.lblTimebase_1);
            this.grpMonitor_1.Controls.Add(this.cmbTimebaseChart_1);
            this.grpMonitor_1.Controls.Add(this.btnAddSignal_Chart_1);
            this.grpMonitor_1.Controls.Add(this.UserChart_1);
            this.grpMonitor_1.Controls.Add(this.lbSignalSelect_Chart_1);
            this.grpMonitor_1.Controls.Add(this.btnDelSignal_Chart_1);
            this.grpMonitor_1.Location = new System.Drawing.Point(14, 16);
            this.grpMonitor_1.Name = "grpMonitor_1";
            this.grpMonitor_1.Size = new System.Drawing.Size(1378, 447);
            this.grpMonitor_1.TabIndex = 23;
            this.grpMonitor_1.TabStop = false;
            this.grpMonitor_1.Text = "Monitor 1";
            // 
            // lblTimebase_1
            // 
            this.lblTimebase_1.Location = new System.Drawing.Point(1123, 367);
            this.lblTimebase_1.Name = "lblTimebase_1";
            this.lblTimebase_1.Size = new System.Drawing.Size(234, 22);
            this.lblTimebase_1.TabIndex = 23;
            this.lblTimebase_1.Text = "Zeitbasis";
            this.lblTimebase_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbTimebaseChart_1
            // 
            this.cmbTimebaseChart_1.FormattingEnabled = true;
            this.cmbTimebaseChart_1.Items.AddRange(new object[] {
            "100ms",
            "200ms",
            "500ms",
            "1s",
            "2s",
            "5s",
            "10s",
            "20s",
            "50s",
            "1m",
            "2m",
            "5m",
            "10m",
            "20m",
            "50m",
            "1h",
            "2h",
            "5h",
            "10h",
            "20h",
            "50h"});
            this.cmbTimebaseChart_1.Location = new System.Drawing.Point(1120, 392);
            this.cmbTimebaseChart_1.Name = "cmbTimebaseChart_1";
            this.cmbTimebaseChart_1.Size = new System.Drawing.Size(239, 28);
            this.cmbTimebaseChart_1.TabIndex = 22;
            // 
            // btnAddSignal_Chart_1
            // 
            this.btnAddSignal_Chart_1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnAddSignal_Chart_1.Location = new System.Drawing.Point(1118, 25);
            this.btnAddSignal_Chart_1.Name = "btnAddSignal_Chart_1";
            this.btnAddSignal_Chart_1.Size = new System.Drawing.Size(84, 70);
            this.btnAddSignal_Chart_1.TabIndex = 21;
            this.btnAddSignal_Chart_1.Text = "Signal +";
            this.btnAddSignal_Chart_1.UseVisualStyleBackColor = true;
            this.btnAddSignal_Chart_1.Click += new System.EventHandler(this.btnAddSignal_Chart_1_Click);
            // 
            // grpMonitor_2
            // 
            this.grpMonitor_2.Controls.Add(this.lblTimebase_2);
            this.grpMonitor_2.Controls.Add(this.lbSignalSelect_Chart_2);
            this.grpMonitor_2.Controls.Add(this.cmbTimebaseChart_2);
            this.grpMonitor_2.Controls.Add(this.UserChart_2);
            this.grpMonitor_2.Controls.Add(this.btnDelSignal_Chart_2);
            this.grpMonitor_2.Controls.Add(this.btnAddSignal_Chart_2);
            this.grpMonitor_2.Location = new System.Drawing.Point(14, 549);
            this.grpMonitor_2.Name = "grpMonitor_2";
            this.grpMonitor_2.Size = new System.Drawing.Size(1378, 447);
            this.grpMonitor_2.TabIndex = 24;
            this.grpMonitor_2.TabStop = false;
            this.grpMonitor_2.Text = "Monitor 2";
            // 
            // lblTimebase_2
            // 
            this.lblTimebase_2.Location = new System.Drawing.Point(1123, 367);
            this.lblTimebase_2.Name = "lblTimebase_2";
            this.lblTimebase_2.Size = new System.Drawing.Size(234, 22);
            this.lblTimebase_2.TabIndex = 25;
            this.lblTimebase_2.Text = "Zeitbasis";
            this.lblTimebase_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSignalSelect_Chart_2
            // 
            this.lbSignalSelect_Chart_2.FormattingEnabled = true;
            this.lbSignalSelect_Chart_2.ItemHeight = 20;
            this.lbSignalSelect_Chart_2.Location = new System.Drawing.Point(1118, 105);
            this.lbSignalSelect_Chart_2.Name = "lbSignalSelect_Chart_2";
            this.lbSignalSelect_Chart_2.Size = new System.Drawing.Size(239, 204);
            this.lbSignalSelect_Chart_2.TabIndex = 24;
            this.lbSignalSelect_Chart_2.Visible = false;
            this.lbSignalSelect_Chart_2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lbSignalSelect_Chart_2_KeyPress);
            this.lbSignalSelect_Chart_2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbSignalSelect_Chart_2_MouseDoubleClick);
            // 
            // UserChart_2
            // 
            chartArea2.Name = "ChartArea1";
            this.UserChart_2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.UserChart_2.Legends.Add(legend2);
            this.UserChart_2.Location = new System.Drawing.Point(40, 37);
            this.UserChart_2.Name = "UserChart_2";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.UserChart_2.Series.Add(series2);
            this.UserChart_2.Size = new System.Drawing.Size(1049, 357);
            this.UserChart_2.TabIndex = 21;
            this.UserChart_2.Text = "chart1";
            // 
            // btnDelSignal_Chart_2
            // 
            this.btnDelSignal_Chart_2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnDelSignal_Chart_2.Location = new System.Drawing.Point(1273, 25);
            this.btnDelSignal_Chart_2.Name = "btnDelSignal_Chart_2";
            this.btnDelSignal_Chart_2.Size = new System.Drawing.Size(84, 70);
            this.btnDelSignal_Chart_2.TabIndex = 23;
            this.btnDelSignal_Chart_2.Text = "Signal -";
            this.btnDelSignal_Chart_2.UseVisualStyleBackColor = true;
            this.btnDelSignal_Chart_2.Click += new System.EventHandler(this.btnDelSignal_Chart_2_Click);
            // 
            // btnAddSignal_Chart_2
            // 
            this.btnAddSignal_Chart_2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnAddSignal_Chart_2.Location = new System.Drawing.Point(1118, 25);
            this.btnAddSignal_Chart_2.Name = "btnAddSignal_Chart_2";
            this.btnAddSignal_Chart_2.Size = new System.Drawing.Size(84, 70);
            this.btnAddSignal_Chart_2.TabIndex = 22;
            this.btnAddSignal_Chart_2.Text = "Signal +";
            this.btnAddSignal_Chart_2.UseVisualStyleBackColor = true;
            this.btnAddSignal_Chart_2.Click += new System.EventHandler(this.btnAddSignal_Chart_2_Click);
            // 
            // btnStartStopMonitoring
            // 
            this.btnStartStopMonitoring.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnStartStopMonitoring.Location = new System.Drawing.Point(581, 475);
            this.btnStartStopMonitoring.Name = "btnStartStopMonitoring";
            this.btnStartStopMonitoring.Size = new System.Drawing.Size(201, 70);
            this.btnStartStopMonitoring.TabIndex = 25;
            this.btnStartStopMonitoring.Text = "Start";
            this.btnStartStopMonitoring.UseVisualStyleBackColor = true;
            this.btnStartStopMonitoring.Click += new System.EventHandler(this.btnStartStopMonitoring_Click);
            // 
            // MyUserMonitoringSettings
            // 
            this.MyUserMonitoringSettings.DefaultDataFileName = null;
            // 
            // txtStates
            // 
            this.txtStates.Location = new System.Drawing.Point(25, 476);
            this.txtStates.Name = "txtStates";
            this.txtStates.Size = new System.Drawing.Size(129, 26);
            this.txtStates.TabIndex = 26;
            // 
            // txtElapsed
            // 
            this.txtElapsed.Location = new System.Drawing.Point(25, 508);
            this.txtElapsed.Name = "txtElapsed";
            this.txtElapsed.Size = new System.Drawing.Size(129, 26);
            this.txtElapsed.TabIndex = 27;
            // 
            // ledSeries0
            // 
            this.ledSeries0.Location = new System.Drawing.Point(176, 475);
            this.ledSeries0.Name = "ledSeries0";
            this.ledSeries0.On = true;
            this.ledSeries0.Size = new System.Drawing.Size(75, 23);
            this.ledSeries0.TabIndex = 28;
            this.ledSeries0.Text = "ledBulb1";
            // 
            // RelayCardUserMonitoring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ledSeries0);
            this.Controls.Add(this.txtElapsed);
            this.Controls.Add(this.txtStates);
            this.Controls.Add(this.grpMonitor_1);
            this.Controls.Add(this.grpMonitor_2);
            this.Controls.Add(this.btnStartStopMonitoring);
            this.Name = "RelayCardUserMonitoring";
            this.Size = new System.Drawing.Size(1406, 1013);
            ((System.ComponentModel.ISupportInitialize)(this.UserChart_1)).EndInit();
            this.grpMonitor_1.ResumeLayout(false);
            this.grpMonitor_2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UserChart_2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart UserChart_1;
        private System.Windows.Forms.ListBox lbSignalSelect_Chart_1;
        private System.Windows.Forms.Button btnDelSignal_Chart_1;
        private System.Windows.Forms.ComboBox cmbTimebaseChart_2;
        private System.Windows.Forms.GroupBox grpMonitor_1;
        private System.Windows.Forms.Label lblTimebase_1;
        private System.Windows.Forms.ComboBox cmbTimebaseChart_1;
        private System.Windows.Forms.Button btnAddSignal_Chart_1;
        private System.Windows.Forms.GroupBox grpMonitor_2;
        private System.Windows.Forms.Label lblTimebase_2;
        private System.Windows.Forms.ListBox lbSignalSelect_Chart_2;
        private System.Windows.Forms.DataVisualization.Charting.Chart UserChart_2;
        private System.Windows.Forms.Button btnDelSignal_Chart_2;
        private System.Windows.Forms.Button btnAddSignal_Chart_2;
        private System.Windows.Forms.Button btnStartStopMonitoring;
        private MyUserMonitoringSettings.MyUserMonitoringSettings MyUserMonitoringSettings;
        private System.Windows.Forms.TextBox txtStates;
        private System.Windows.Forms.TextBox txtElapsed;
        private Bulb.LedBulb ledSeries0;
    }
}
