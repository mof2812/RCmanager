namespace SetupSerial
{
    partial class SetupSerial
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
            this.grpCOMSetup = new System.Windows.Forms.GroupBox();
            this.lblStoppbits = new System.Windows.Forms.Label();
            this.cmbStopbits = new System.Windows.Forms.ComboBox();
            this.lblDatabits = new System.Windows.Forms.Label();
            this.cmbDatabits = new System.Windows.Forms.ComboBox();
            this.lblCOMBaudrate = new System.Windows.Forms.Label();
            this.cmbBaudrate = new System.Windows.Forms.ComboBox();
            this.lblCOMPort = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpCOMSetup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCOMSetup
            // 
            this.grpCOMSetup.Controls.Add(this.lblStoppbits);
            this.grpCOMSetup.Controls.Add(this.cmbStopbits);
            this.grpCOMSetup.Controls.Add(this.lblDatabits);
            this.grpCOMSetup.Controls.Add(this.cmbDatabits);
            this.grpCOMSetup.Controls.Add(this.lblCOMBaudrate);
            this.grpCOMSetup.Controls.Add(this.cmbBaudrate);
            this.grpCOMSetup.Controls.Add(this.lblCOMPort);
            this.grpCOMSetup.Controls.Add(this.cmbPort);
            this.grpCOMSetup.Location = new System.Drawing.Point(72, 46);
            this.grpCOMSetup.Name = "grpCOMSetup";
            this.grpCOMSetup.Size = new System.Drawing.Size(205, 293);
            this.grpCOMSetup.TabIndex = 11;
            this.grpCOMSetup.TabStop = false;
            this.grpCOMSetup.Text = "Serielle Schnittstelle";
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
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(569, 379);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(158, 50);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "Okay";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(364, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(364, 317);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(405, 379);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(158, 50);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SetupSerial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grpCOMSetup);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCancel);
            this.Name = "SetupSerial";
            this.Text = "Wägezelleneinstellungen";
            this.Load += new System.EventHandler(this.SetupLoadCell_Load);
            this.grpCOMSetup.ResumeLayout(false);
            this.grpCOMSetup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCOMSetup;
        private System.Windows.Forms.Label lblStoppbits;
        private System.Windows.Forms.ComboBox cmbStopbits;
        private System.Windows.Forms.Label lblDatabits;
        private System.Windows.Forms.ComboBox cmbDatabits;
        private System.Windows.Forms.Label lblCOMBaudrate;
        private System.Windows.Forms.ComboBox cmbBaudrate;
        private System.Windows.Forms.Label lblCOMPort;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnCancel;
    }
}