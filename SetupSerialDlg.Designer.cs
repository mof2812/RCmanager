namespace SerialCommunication
{
    partial class SetupSerialDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.grpCOMSetup = new System.Windows.Forms.GroupBox();
            this.txtReplies = new System.Windows.Forms.TextBox();
            this.lblReplies = new System.Windows.Forms.Label();
            this.txtReceiveTimeout = new System.Windows.Forms.TextBox();
            this.lblReceiveTimeout = new System.Windows.Forms.Label();
            this.lblStoppbits = new System.Windows.Forms.Label();
            this.cmbStopbits = new System.Windows.Forms.ComboBox();
            this.lblDatabits = new System.Windows.Forms.Label();
            this.cmbDatabits = new System.Windows.Forms.ComboBox();
            this.lblCOMBaudrate = new System.Windows.Forms.Label();
            this.cmbBaudrate = new System.Windows.Forms.ComboBox();
            this.lblCOMPort = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbBitmap = new System.Windows.Forms.PictureBox();
            this.grpProtocol = new System.Windows.Forms.GroupBox();
            this.lblChecksumType = new System.Windows.Forms.Label();
            this.cmbChecksumType = new System.Windows.Forms.ComboBox();
            this.txtEndDelimiter = new System.Windows.Forms.TextBox();
            this.txtStartDelimiter = new System.Windows.Forms.TextBox();
            this.cbUseChecksum = new System.Windows.Forms.CheckBox();
            this.lblEndDelimiter = new System.Windows.Forms.Label();
            this.lblStartDelimiter = new System.Windows.Forms.Label();
            this.cbUseDelimiters = new System.Windows.Forms.CheckBox();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.lblTiming = new System.Windows.Forms.Label();
            this.txtTiming = new System.Windows.Forms.TextBox();
            this.grpCOMSetup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBitmap)).BeginInit();
            this.grpProtocol.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(942, 388);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(158, 50);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Okay";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpCOMSetup
            // 
            this.grpCOMSetup.Controls.Add(this.txtReplies);
            this.grpCOMSetup.Controls.Add(this.lblReplies);
            this.grpCOMSetup.Controls.Add(this.txtReceiveTimeout);
            this.grpCOMSetup.Controls.Add(this.lblReceiveTimeout);
            this.grpCOMSetup.Controls.Add(this.lblStoppbits);
            this.grpCOMSetup.Controls.Add(this.cmbStopbits);
            this.grpCOMSetup.Controls.Add(this.lblDatabits);
            this.grpCOMSetup.Controls.Add(this.cmbDatabits);
            this.grpCOMSetup.Controls.Add(this.lblCOMBaudrate);
            this.grpCOMSetup.Controls.Add(this.cmbBaudrate);
            this.grpCOMSetup.Controls.Add(this.lblCOMPort);
            this.grpCOMSetup.Controls.Add(this.cmbPort);
            this.grpCOMSetup.Location = new System.Drawing.Point(16, 31);
            this.grpCOMSetup.Name = "grpCOMSetup";
            this.grpCOMSetup.Size = new System.Drawing.Size(205, 407);
            this.grpCOMSetup.TabIndex = 2;
            this.grpCOMSetup.TabStop = false;
            this.grpCOMSetup.Text = "Serielle Schnittstelle";
            // 
            // txtReplies
            // 
            this.txtReplies.Location = new System.Drawing.Point(17, 365);
            this.txtReplies.Name = "txtReplies";
            this.txtReplies.Size = new System.Drawing.Size(170, 26);
            this.txtReplies.TabIndex = 12;
            this.txtReplies.Text = "Wiederholungen";
            // 
            // lblReplies
            // 
            this.lblReplies.AutoSize = true;
            this.lblReplies.Location = new System.Drawing.Point(16, 341);
            this.lblReplies.Name = "lblReplies";
            this.lblReplies.Size = new System.Drawing.Size(125, 20);
            this.lblReplies.TabIndex = 11;
            this.lblReplies.Text = "Wiederholungen";
            // 
            // txtReceiveTimeout
            // 
            this.txtReceiveTimeout.Location = new System.Drawing.Point(17, 305);
            this.txtReceiveTimeout.Name = "txtReceiveTimeout";
            this.txtReceiveTimeout.Size = new System.Drawing.Size(170, 26);
            this.txtReceiveTimeout.TabIndex = 10;
            this.txtReceiveTimeout.Text = "Timeout";
            // 
            // lblReceiveTimeout
            // 
            this.lblReceiveTimeout.AutoSize = true;
            this.lblReceiveTimeout.Location = new System.Drawing.Point(16, 281);
            this.lblReceiveTimeout.Name = "lblReceiveTimeout";
            this.lblReceiveTimeout.Size = new System.Drawing.Size(173, 20);
            this.lblReceiveTimeout.TabIndex = 9;
            this.lblReceiveTimeout.Text = "Empfangs-Timeout[ms]";
            // 
            // lblStoppbits
            // 
            this.lblStoppbits.AutoSize = true;
            this.lblStoppbits.Location = new System.Drawing.Point(16, 221);
            this.lblStoppbits.Name = "lblStoppbits";
            this.lblStoppbits.Size = new System.Drawing.Size(77, 20);
            this.lblStoppbits.TabIndex = 7;
            this.lblStoppbits.Text = "Stoppbits";
            // 
            // cmbStopbits
            // 
            this.cmbStopbits.FormattingEnabled = true;
            this.cmbStopbits.Location = new System.Drawing.Point(17, 245);
            this.cmbStopbits.Name = "cmbStopbits";
            this.cmbStopbits.Size = new System.Drawing.Size(170, 28);
            this.cmbStopbits.TabIndex = 6;
            this.cmbStopbits.Text = "Stoppbits";
            // 
            // lblDatabits
            // 
            this.lblDatabits.AutoSize = true;
            this.lblDatabits.Location = new System.Drawing.Point(16, 161);
            this.lblDatabits.Name = "lblDatabits";
            this.lblDatabits.Size = new System.Drawing.Size(78, 20);
            this.lblDatabits.TabIndex = 5;
            this.lblDatabits.Text = "Datenbits";
            // 
            // cmbDatabits
            // 
            this.cmbDatabits.FormattingEnabled = true;
            this.cmbDatabits.Location = new System.Drawing.Point(17, 185);
            this.cmbDatabits.Name = "cmbDatabits";
            this.cmbDatabits.Size = new System.Drawing.Size(170, 28);
            this.cmbDatabits.TabIndex = 4;
            this.cmbDatabits.Text = "Datenbits";
            // 
            // lblCOMBaudrate
            // 
            this.lblCOMBaudrate.AutoSize = true;
            this.lblCOMBaudrate.Location = new System.Drawing.Point(16, 101);
            this.lblCOMBaudrate.Name = "lblCOMBaudrate";
            this.lblCOMBaudrate.Size = new System.Drawing.Size(75, 20);
            this.lblCOMBaudrate.TabIndex = 3;
            this.lblCOMBaudrate.Text = "Baudrate";
            // 
            // cmbBaudrate
            // 
            this.cmbBaudrate.FormattingEnabled = true;
            this.cmbBaudrate.Location = new System.Drawing.Point(17, 125);
            this.cmbBaudrate.Name = "cmbBaudrate";
            this.cmbBaudrate.Size = new System.Drawing.Size(170, 28);
            this.cmbBaudrate.TabIndex = 2;
            this.cmbBaudrate.Text = "Baudrate";
            // 
            // lblCOMPort
            // 
            this.lblCOMPort.AutoSize = true;
            this.lblCOMPort.Location = new System.Drawing.Point(16, 41);
            this.lblCOMPort.Name = "lblCOMPort";
            this.lblCOMPort.Size = new System.Drawing.Size(96, 20);
            this.lblCOMPort.TabIndex = 1;
            this.lblCOMPort.Text = "Schnittstelle";
            // 
            // cmbPort
            // 
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(17, 65);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(170, 28);
            this.cmbPort.Sorted = true;
            this.cmbPort.TabIndex = 0;
            this.cmbPort.Text = "COM-Port";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(778, 388);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(158, 50);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pbBitmap
            // 
            this.pbBitmap.Location = new System.Drawing.Point(737, 31);
            this.pbBitmap.Name = "pbBitmap";
            this.pbBitmap.Size = new System.Drawing.Size(364, 317);
            this.pbBitmap.TabIndex = 4;
            this.pbBitmap.TabStop = false;
            // 
            // grpProtocol
            // 
            this.grpProtocol.Controls.Add(this.lblChecksumType);
            this.grpProtocol.Controls.Add(this.cmbChecksumType);
            this.grpProtocol.Controls.Add(this.txtEndDelimiter);
            this.grpProtocol.Controls.Add(this.txtStartDelimiter);
            this.grpProtocol.Controls.Add(this.cbUseChecksum);
            this.grpProtocol.Controls.Add(this.lblEndDelimiter);
            this.grpProtocol.Controls.Add(this.lblStartDelimiter);
            this.grpProtocol.Controls.Add(this.cbUseDelimiters);
            this.grpProtocol.Location = new System.Drawing.Point(256, 29);
            this.grpProtocol.Name = "grpProtocol";
            this.grpProtocol.Size = new System.Drawing.Size(217, 407);
            this.grpProtocol.TabIndex = 5;
            this.grpProtocol.TabStop = false;
            this.grpProtocol.Text = "Protokoll";
            // 
            // lblChecksumType
            // 
            this.lblChecksumType.AutoSize = true;
            this.lblChecksumType.Location = new System.Drawing.Point(53, 281);
            this.lblChecksumType.Name = "lblChecksumType";
            this.lblChecksumType.Size = new System.Drawing.Size(120, 20);
            this.lblChecksumType.TabIndex = 14;
            this.lblChecksumType.Text = "Prüfsummentyp";
            // 
            // cmbChecksumType
            // 
            this.cmbChecksumType.FormattingEnabled = true;
            this.cmbChecksumType.Location = new System.Drawing.Point(54, 305);
            this.cmbChecksumType.Name = "cmbChecksumType";
            this.cmbChecksumType.Size = new System.Drawing.Size(145, 28);
            this.cmbChecksumType.TabIndex = 13;
            this.cmbChecksumType.Text = "Prüfsummentyp";
            // 
            // txtEndDelimiter
            // 
            this.txtEndDelimiter.Location = new System.Drawing.Point(54, 187);
            this.txtEndDelimiter.Name = "txtEndDelimiter";
            this.txtEndDelimiter.Size = new System.Drawing.Size(89, 26);
            this.txtEndDelimiter.TabIndex = 19;
            // 
            // txtStartDelimiter
            // 
            this.txtStartDelimiter.Location = new System.Drawing.Point(54, 127);
            this.txtStartDelimiter.Name = "txtStartDelimiter";
            this.txtStartDelimiter.Size = new System.Drawing.Size(89, 26);
            this.txtStartDelimiter.TabIndex = 18;
            // 
            // cbUseChecksum
            // 
            this.cbUseChecksum.AutoSize = true;
            this.cbUseChecksum.Location = new System.Drawing.Point(17, 246);
            this.cbUseChecksum.Name = "cbUseChecksum";
            this.cbUseChecksum.Size = new System.Drawing.Size(197, 24);
            this.cbUseChecksum.TabIndex = 17;
            this.cbUseChecksum.Text = "Prüfsumme verwenden";
            this.cbUseChecksum.UseVisualStyleBackColor = true;
            this.cbUseChecksum.CheckedChanged += new System.EventHandler(this.cbUseChecksum_CheckedChanged);
            // 
            // lblEndDelimiter
            // 
            this.lblEndDelimiter.AutoSize = true;
            this.lblEndDelimiter.Location = new System.Drawing.Point(53, 163);
            this.lblEndDelimiter.Name = "lblEndDelimiter";
            this.lblEndDelimiter.Size = new System.Drawing.Size(93, 20);
            this.lblEndDelimiter.TabIndex = 16;
            this.lblEndDelimiter.Text = "Endzeichen";
            // 
            // lblStartDelimiter
            // 
            this.lblStartDelimiter.AutoSize = true;
            this.lblStartDelimiter.Location = new System.Drawing.Point(53, 103);
            this.lblStartDelimiter.Name = "lblStartDelimiter";
            this.lblStartDelimiter.Size = new System.Drawing.Size(99, 20);
            this.lblStartDelimiter.TabIndex = 14;
            this.lblStartDelimiter.Text = "Startzeichen";
            // 
            // cbUseDelimiters
            // 
            this.cbUseDelimiters.AutoSize = true;
            this.cbUseDelimiters.Location = new System.Drawing.Point(17, 67);
            this.cbUseDelimiters.Name = "cbUseDelimiters";
            this.cbUseDelimiters.Size = new System.Drawing.Size(153, 24);
            this.cbUseDelimiters.TabIndex = 0;
            this.cbUseDelimiters.Text = "Framebegrenzer";
            this.cbUseDelimiters.UseVisualStyleBackColor = true;
            this.cbUseDelimiters.CheckedChanged += new System.EventHandler(this.cbUseDelimiters_CheckedChanged);
            // 
            // grpMisc
            // 
            this.grpMisc.Controls.Add(this.txtTiming);
            this.grpMisc.Controls.Add(this.lblTiming);
            this.grpMisc.Location = new System.Drawing.Point(496, 29);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(217, 407);
            this.grpMisc.TabIndex = 6;
            this.grpMisc.TabStop = false;
            this.grpMisc.Text = "Verschiedenes";
            // 
            // lblTiming
            // 
            this.lblTiming.AutoSize = true;
            this.lblTiming.Location = new System.Drawing.Point(23, 41);
            this.lblTiming.Name = "lblTiming";
            this.lblTiming.Size = new System.Drawing.Size(84, 20);
            this.lblTiming.TabIndex = 2;
            this.lblTiming.Text = "Timing[ms]";
            // 
            // txtTiming
            // 
            this.txtTiming.Location = new System.Drawing.Point(23, 65);
            this.txtTiming.Name = "txtTiming";
            this.txtTiming.Size = new System.Drawing.Size(170, 26);
            this.txtTiming.TabIndex = 3;
            this.txtTiming.Text = "Timing_ms";
            // 
            // SetupSerialDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 450);
            this.Controls.Add(this.grpMisc);
            this.Controls.Add(this.grpProtocol);
            this.Controls.Add(this.pbBitmap);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpCOMSetup);
            this.Controls.Add(this.btnOK);
            this.Name = "SetupSerialDlg";
            this.Text = "SetupSerialDlg";
            this.Load += new System.EventHandler(this.SetupSerialDlg_Load);
            this.grpCOMSetup.ResumeLayout(false);
            this.grpCOMSetup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBitmap)).EndInit();
            this.grpProtocol.ResumeLayout(false);
            this.grpProtocol.PerformLayout();
            this.grpMisc.ResumeLayout(false);
            this.grpMisc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox grpCOMSetup;
        private System.Windows.Forms.Label lblStoppbits;
        private System.Windows.Forms.ComboBox cmbStopbits;
        private System.Windows.Forms.Label lblDatabits;
        private System.Windows.Forms.ComboBox cmbDatabits;
        private System.Windows.Forms.Label lblCOMBaudrate;
        private System.Windows.Forms.ComboBox cmbBaudrate;
        private System.Windows.Forms.Label lblCOMPort;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pbBitmap;
        private System.Windows.Forms.Label lblReceiveTimeout;
        private System.Windows.Forms.TextBox txtReplies;
        private System.Windows.Forms.Label lblReplies;
        private System.Windows.Forms.TextBox txtReceiveTimeout;
        private System.Windows.Forms.GroupBox grpProtocol;
        private System.Windows.Forms.Label lblChecksumType;
        private System.Windows.Forms.ComboBox cmbChecksumType;
        private System.Windows.Forms.TextBox txtEndDelimiter;
        private System.Windows.Forms.TextBox txtStartDelimiter;
        private System.Windows.Forms.CheckBox cbUseChecksum;
        private System.Windows.Forms.Label lblEndDelimiter;
        private System.Windows.Forms.Label lblStartDelimiter;
        private System.Windows.Forms.CheckBox cbUseDelimiters;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.TextBox txtTiming;
        private System.Windows.Forms.Label lblTiming;
    }
}