using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectSettings
{
    public partial class ProjectInfo : Form
    {
        public ProjectInfo()
        {
            InitializeComponent();
        }
        public string Info
        {
            get 
            {
                return txtProjectInfo.Text;
            }
            set
            {
                txtProjectInfo.Text = value;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtProjectInfo.Text = "";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
