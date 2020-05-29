using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsStat.SystemFacade.Extensions;

namespace ServerTools
{
    public partial class frmSave : Form
    {
        public string PasswordHash { get; set; }
        public frmSave()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.IsEmpty())
            {
                validator.SetError(txtPassword, "Please enter password");
            }
            else if(!BCrypt.Net.BCrypt.Verify(txtPassword.Text, PasswordHash))
            {
                validator.SetError(txtPassword,"Wrong password");
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnHover(object sender, EventArgs e)
        {
            var label = (Label)sender;

            label.ForeColor = Color.DeepSkyBlue;
        }

        private void OnLeave(object sender, EventArgs e)
        {
            var label = (Label)sender;

            label.ForeColor = Color.Black;
        }

        private void lblShow_Click(object sender, EventArgs e)
        {
            if(lblShow.Text == "Show")
            {
                txtPassword.PasswordChar = '\0';
                lblShow.Text = "Hide";
            }
            else
            {
                txtPassword.PasswordChar = '*';
                lblShow.Text = "Show";
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick();
            }
        }
    }
}
