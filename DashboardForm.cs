using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMMS
{
    public partial class DashboardForm : Form
    {
        //Fields
        private Form activeForm;

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you wanna logout?", "Logout?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoginForm lf = new LoginForm();
                this.Hide();
                lf.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you wanna exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void OpenChildForm(Form childForm, object btnSender)
        {
            if(activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktop.Controls.Add(childForm);
            this.panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblScreen.Text = childForm.Text;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            //Set User Login Name
            lblWelcomeUser.Text = "Welcome back, " + UserLogin.getUserName() + "!";

            //Set User Login Photo
            //byte[] img = (byte)UserLogin.getUserPhoto();
            //MemoryStream ms = new MemoryStream(img);
            //pbUserLoginPhoto.Image = Image.FromStream(ms);

            OpenChildForm(new Forms.DashboardForm(), sender);
            btnDashboard.BackColor = Color.CadetBlue;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            //btnDashboard.BackColor = Color.SlateGray;
            btnDashboard.BackColor = Color.CadetBlue;
            btnCategory.BackColor = Color.DarkSlateGray;
            btnProduct.BackColor = Color.DarkSlateGray;
            btnShipper.BackColor = Color.DarkSlateGray;
            btnCustomer.BackColor = Color.DarkSlateGray;
            btnEmployee.BackColor = Color.DarkSlateGray;

            OpenChildForm(new Forms.DashboardForm(), sender);

        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            btnDashboard.BackColor = Color.DarkSlateGray;
            btnCategory.BackColor = Color.CadetBlue;
            btnProduct.BackColor = Color.DarkSlateGray;
            btnShipper.BackColor = Color.DarkSlateGray;
            btnCustomer.BackColor = Color.DarkSlateGray;
            btnEmployee.BackColor = Color.DarkSlateGray;

            OpenChildForm(new Forms.CategoryForm(), sender);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            btnDashboard.BackColor = Color.DarkSlateGray;
            btnCategory.BackColor = Color.DarkSlateGray;
            //btnProduct.BackColor = Color.SteelBlue;
            btnProduct.BackColor = Color.CadetBlue;
            btnShipper.BackColor = Color.DarkSlateGray;
            btnCustomer.BackColor = Color.DarkSlateGray;
            btnEmployee.BackColor = Color.DarkSlateGray;

            OpenChildForm(new Forms.ProductForm(), sender);
        }

        private void btnShipper_Click(object sender, EventArgs e)
        {
            btnDashboard.BackColor = Color.DarkSlateGray;
            btnCategory.BackColor = Color.DarkSlateGray;
            btnProduct.BackColor = Color.DarkSlateGray;
            //btnShipper.BackColor = Color.SlateBlue;
            btnShipper.BackColor = Color.CadetBlue;
            btnCustomer.BackColor = Color.DarkSlateGray;
            btnEmployee.BackColor = Color.DarkSlateGray;

            OpenChildForm(new Forms.ShipperForm(), sender);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            if (UserLogin.getUserType() != "admin")
            {
                MessageBox.Show("What're you looking for? Only admin can see and manage these informations.", "Unauthorized!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                btnDashboard.BackColor = Color.DarkSlateGray;
                btnCategory.BackColor = Color.DarkSlateGray;
                btnProduct.BackColor = Color.DarkSlateGray;
                btnShipper.BackColor = Color.DarkSlateGray;
                //btnCustomer.BackColor = Color.DarkGoldenrod;
                btnCustomer.BackColor = Color.CadetBlue;
                btnEmployee.BackColor = Color.DarkSlateGray;

                OpenChildForm(new Forms.CustomerForm(), sender);
            }
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            if (UserLogin.getUserType() != "admin")
            {
                MessageBox.Show("What're you looking for? Only admin can see and manage these informations.", "Unauthorized!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                btnDashboard.BackColor = Color.DarkSlateGray;
                btnCategory.BackColor = Color.DarkSlateGray;
                btnProduct.BackColor = Color.DarkSlateGray;
                btnShipper.BackColor = Color.DarkSlateGray;
                btnCustomer.BackColor = Color.DarkSlateGray;
                //btnEmployee.BackColor = Color.Gray;
                btnEmployee.BackColor = Color.CadetBlue;

                OpenChildForm(new Forms.EmployeeForm(), sender);
            }
        }
    }
}