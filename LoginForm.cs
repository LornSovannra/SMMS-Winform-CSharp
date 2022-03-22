using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace SMMS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection();

        private void LoginForm_Load(object sender, EventArgs e)
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectToDB"].ToString();
            conn.Open();

            txtUsername.BorderStyle = BorderStyle.None;
            txtPassword.BorderStyle = BorderStyle.None;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Please enter username", "Require Username", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Focus();
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter password", "Require Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();
            }
            else
            {
                string sql = "SELECT * FROM tblEmployees WHERE Username = '" + txtUsername.Text + "' AND Password = '" + txtPassword.Text + "';";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();

                adapter.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    //Invoke class from UserLogin Class
                    //Save User Login Info
                    UserLogin.setUserID(dt.Rows[0]["EmployeeID"].ToString());
                    UserLogin.setUserName(dt.Rows[0]["EmployeeName"].ToString());
                    UserLogin.setPassword(dt.Rows[0]["Password"].ToString());
                    UserLogin.setUserType(dt.Rows[0]["UserType"].ToString());

                    //byte Photo = (byte)dt.Rows[0]["Photo"];
                    //UserLogin.setUserPhoto(Photo);

                    //Show MainForm
                    DashboardForm frm = new DashboardForm();
                    this.Hide();
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Failed to login", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you wanna exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}