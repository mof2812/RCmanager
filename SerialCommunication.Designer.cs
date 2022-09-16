namespace SerialCommunication
{
    partial class SerialCommunication
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
            this.ComPort = new System.IO.Ports.SerialPort(this.components);
            this.TaskTimer = new System.Windows.Forms.Timer(this.components);
            // 
            // TaskTimer
            // 
            this.TaskTimer.Interval = 10;
            this.TaskTimer.Tick += new System.EventHandler(this.TimerTick);

        }

        #endregion

        private System.IO.Ports.SerialPort ComPort;
        private System.Windows.Forms.Timer TaskTimer;
    }
}
