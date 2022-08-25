namespace RCmanager
{
    partial class DebugControl
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
            this.lvDebug = new System.Windows.Forms.ListView();
            this.grpDebugging = new System.Windows.Forms.GroupBox();
            this.cbDebugOnOff = new System.Windows.Forms.CheckBox();
            this.btnClearDebug = new System.Windows.Forms.Button();
            this.cbAddSTX_ETX = new System.Windows.Forms.CheckBox();
            this.cbRecDebug = new System.Windows.Forms.CheckBox();
            this.tbDebugFilter = new System.Windows.Forms.TextBox();
            this.cbAddNext = new System.Windows.Forms.CheckBox();
            this.lblDebugFilter = new System.Windows.Forms.Label();
            this.grpDebugging.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvDebug
            // 
            this.lvDebug.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lvDebug.HideSelection = false;
            this.lvDebug.Location = new System.Drawing.Point(0, 0);
            this.lvDebug.Name = "lvDebug";
            this.lvDebug.Size = new System.Drawing.Size(1404, 282);
            this.lvDebug.TabIndex = 3;
            this.lvDebug.UseCompatibleStateImageBehavior = false;
            this.lvDebug.View = System.Windows.Forms.View.Details;
            // 
            // grpDebugging
            // 
            this.grpDebugging.BackColor = System.Drawing.Color.Transparent;
            this.grpDebugging.Controls.Add(this.cbDebugOnOff);
            this.grpDebugging.Controls.Add(this.btnClearDebug);
            this.grpDebugging.Controls.Add(this.cbAddSTX_ETX);
            this.grpDebugging.Controls.Add(this.cbRecDebug);
            this.grpDebugging.Controls.Add(this.tbDebugFilter);
            this.grpDebugging.Controls.Add(this.cbAddNext);
            this.grpDebugging.Controls.Add(this.lblDebugFilter);
            this.grpDebugging.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDebugging.Location = new System.Drawing.Point(1414, 0);
            this.grpDebugging.Name = "grpDebugging";
            this.grpDebugging.Size = new System.Drawing.Size(354, 272);
            this.grpDebugging.TabIndex = 12;
            this.grpDebugging.TabStop = false;
            this.grpDebugging.Text = "Debugging";
            // 
            // cbDebugOnOff
            // 
            this.cbDebugOnOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbDebugOnOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDebugOnOff.Location = new System.Drawing.Point(22, 44);
            this.cbDebugOnOff.Name = "cbDebugOnOff";
            this.cbDebugOnOff.Size = new System.Drawing.Size(314, 35);
            this.cbDebugOnOff.TabIndex = 3;
            this.cbDebugOnOff.Text = "Debug on";
            this.cbDebugOnOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDebugOnOff.UseVisualStyleBackColor = true;
            this.cbDebugOnOff.CheckedChanged += new System.EventHandler(this.cbDebugOnOff_CheckedChanged);
            // 
            // btnClearDebug
            // 
            this.btnClearDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearDebug.Location = new System.Drawing.Point(188, 226);
            this.btnClearDebug.Name = "btnClearDebug";
            this.btnClearDebug.Size = new System.Drawing.Size(148, 35);
            this.btnClearDebug.TabIndex = 9;
            this.btnClearDebug.Text = "Löschen";
            this.btnClearDebug.UseVisualStyleBackColor = true;
            this.btnClearDebug.Click += new System.EventHandler(this.btnClearDebug_Click);
            // 
            // cbAddSTX_ETX
            // 
            this.cbAddSTX_ETX.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAddSTX_ETX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAddSTX_ETX.Location = new System.Drawing.Point(22, 158);
            this.cbAddSTX_ETX.Name = "cbAddSTX_ETX";
            this.cbAddSTX_ETX.Size = new System.Drawing.Size(148, 35);
            this.cbAddSTX_ETX.TabIndex = 10;
            this.cbAddSTX_ETX.Text = "+ <STX><ETX>";
            this.cbAddSTX_ETX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAddSTX_ETX.UseVisualStyleBackColor = true;
            this.cbAddSTX_ETX.CheckedChanged += new System.EventHandler(this.cbAddSTX_ETX_CheckedChanged);
            // 
            // cbRecDebug
            // 
            this.cbRecDebug.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbRecDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRecDebug.Location = new System.Drawing.Point(22, 226);
            this.cbRecDebug.Name = "cbRecDebug";
            this.cbRecDebug.Size = new System.Drawing.Size(148, 35);
            this.cbRecDebug.TabIndex = 7;
            this.cbRecDebug.Text = "Aufnahme aus";
            this.cbRecDebug.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbRecDebug.UseVisualStyleBackColor = true;
            this.cbRecDebug.CheckedChanged += new System.EventHandler(this.cbRecDebug_CheckedChanged);
            // 
            // tbDebugFilter
            // 
            this.tbDebugFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDebugFilter.Location = new System.Drawing.Point(89, 104);
            this.tbDebugFilter.MinimumSize = new System.Drawing.Size(4, 35);
            this.tbDebugFilter.Name = "tbDebugFilter";
            this.tbDebugFilter.Size = new System.Drawing.Size(247, 35);
            this.tbDebugFilter.TabIndex = 4;
            this.tbDebugFilter.Text = "Filter";
            this.tbDebugFilter.TextChanged += new System.EventHandler(this.tbDebugFilter_TextChanged);
            // 
            // cbAddNext
            // 
            this.cbAddNext.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAddNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAddNext.Location = new System.Drawing.Point(188, 158);
            this.cbAddNext.Name = "cbAddNext";
            this.cbAddNext.Size = new System.Drawing.Size(148, 35);
            this.cbAddNext.TabIndex = 6;
            this.cbAddNext.Text = "+ nächstes Frame";
            this.cbAddNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAddNext.UseVisualStyleBackColor = true;
            this.cbAddNext.CheckedChanged += new System.EventHandler(this.cbAddNext_CheckedChanged);
            // 
            // lblDebugFilter
            // 
            this.lblDebugFilter.BackColor = System.Drawing.Color.DimGray;
            this.lblDebugFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebugFilter.Location = new System.Drawing.Point(18, 104);
            this.lblDebugFilter.Name = "lblDebugFilter";
            this.lblDebugFilter.Size = new System.Drawing.Size(69, 26);
            this.lblDebugFilter.TabIndex = 5;
            this.lblDebugFilter.Text = "enthält";
            this.lblDebugFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Debugging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.grpDebugging);
            this.Controls.Add(this.lvDebug);
            this.Name = "Debugging";
            this.Size = new System.Drawing.Size(1779, 283);
            this.grpDebugging.ResumeLayout(false);
            this.grpDebugging.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvDebug;
        private System.Windows.Forms.GroupBox grpDebugging;
        private System.Windows.Forms.CheckBox cbDebugOnOff;
        private System.Windows.Forms.Button btnClearDebug;
        private System.Windows.Forms.CheckBox cbAddSTX_ETX;
        private System.Windows.Forms.CheckBox cbRecDebug;
        private System.Windows.Forms.TextBox tbDebugFilter;
        private System.Windows.Forms.CheckBox cbAddNext;
        private System.Windows.Forms.Label lblDebugFilter;
    }
}
