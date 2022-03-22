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
using System.IO;

namespace SMMS.Forms
{
    public partial class EmployeeForm : Form
    {
        public EmployeeForm()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection();


        void Connection()
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectToDB"].ToString();
            conn.Open();
        }

        void RetrieveData()
        {
            SqlCommand sql_select = new SqlCommand("showEmployee", conn);
            sql_select.CommandType = CommandType.Text;
            SqlDataAdapter adapt = new SqlDataAdapter(sql_select);
            DataTable dt = new DataTable();
            adapt.Fill(dt);

            dgvEmployee.DataSource = dt;
            DataGridViewImageColumn imgcolumn = new DataGridViewImageColumn();
            dgvEmployee.RowTemplate.Height = 70;
            imgcolumn = (DataGridViewImageColumn)dgvEmployee.Columns[15];
            imgcolumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        void ClearFields()
        {
            //foreach(Control ctrl in this.Controls)
            //{
            //    if(ctrl is TextBox)
            //    {
            //        ctrl.Text = string.Empty;
            //    }else if(ctrl is ComboBox)
            //    {
            //        ctrl.Text = string.Empty;
            //    }
            //}

            txtEmployeeID.Text = string.Empty;
            txtEmployeeName.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            comboBoxUserType.Text = string.Empty;
            txtRole.Text = string.Empty;
            comboBoxJobTitle.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtPhoneHome.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtStateProvince.Text = string.Empty;
            txtZipPostalCode.Text = string.Empty;
            txtCountryRegion.Text = string.Empty;
            txtEmployeeID.Focus();
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            List<string> userTypes = new List<string>();
            userTypes.Add("admin");
            userTypes.Add("user");

            foreach (string userTypeList in userTypes)
            {
                comboBoxUserType.Items.Add(userTypeList);
            }

            List<string> jobTitles = new List<string>();
            jobTitles.Add("Founder");
            jobTitles.Add("CEO");
            jobTitles.Add("Vice President");
            jobTitles.Add("IT Manager");
            jobTitles.Add("Accountant Manager");
            jobTitles.Add("HR Manager");
            jobTitles.Add("Seller");

            foreach (string jobTitleList in jobTitles)
            {
                comboBoxJobTitle.Items.Add(jobTitleList);
            }

            //Invoke method
            Connection();
            RetrieveData();
        }

        private void dgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtEmployeeID.Text = dgvEmployee.CurrentRow.Cells[0].Value.ToString();
            txtEmployeeName.Text = dgvEmployee.CurrentRow.Cells[1].Value.ToString();
            txtUsername.Text = dgvEmployee.CurrentRow.Cells[2].Value.ToString();
            txtPassword.Text = dgvEmployee.CurrentRow.Cells[3].Value.ToString();
            comboBoxUserType.Text = dgvEmployee.CurrentRow.Cells[4].Value.ToString();
            txtRole.Text = dgvEmployee.CurrentRow.Cells[5].Value.ToString();
            comboBoxJobTitle.Text = dgvEmployee.CurrentRow.Cells[6].Value.ToString();
            txtEmail.Text = dgvEmployee.CurrentRow.Cells[7].Value.ToString();
            txtPhoneNumber.Text = dgvEmployee.CurrentRow.Cells[8].Value.ToString();
            txtPhoneHome.Text = dgvEmployee.CurrentRow.Cells[9].Value.ToString();
            txtAddress.Text = dgvEmployee.CurrentRow.Cells[10].Value.ToString();
            txtCity.Text = dgvEmployee.CurrentRow.Cells[11].Value.ToString();
            txtStateProvince.Text = dgvEmployee.CurrentRow.Cells[12].Value.ToString();
            txtZipPostalCode.Text = dgvEmployee.CurrentRow.Cells[13].Value.ToString();
            txtCountryRegion.Text = dgvEmployee.CurrentRow.Cells[14].Value.ToString();

            //get and show image
            byte[] img = (byte[])dgvEmployee.CurrentRow.Cells[15].Value;
            MemoryStream ms = new MemoryStream(img);
            pbEmployee.Image = Image.FromStream(ms);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if(btnRegister.Text == "Register")
            {
                btnRegister.Text = "Save";
                btnDelete.Text = "Cancel";
                btnUpdate.Enabled = false;

                //Invoke Method
                ClearFields();
            }else if(btnRegister.Text == "Save")
            {
                if (string.IsNullOrEmpty(txtEmployeeID.Text))
                {
                    MessageBox.Show("Employee ID is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtEmployeeName.Text))
                {
                    MessageBox.Show("Employee Name is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    MessageBox.Show("Username is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Password is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(comboBoxUserType.Text))
                {
                    MessageBox.Show("User Type is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtRole.Text))
                {
                    MessageBox.Show("Role is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(comboBoxJobTitle.Text))
                {
                    MessageBox.Show("Job Title is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Email is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtPhoneNumber.Text))
                {
                    MessageBox.Show("Phone number is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtPhoneHome.Text))
                {
                    MessageBox.Show("Phone home is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    MessageBox.Show("Address is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtCity.Text))
                {
                    MessageBox.Show("City is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtStateProvince.Text))
                {
                    MessageBox.Show("State/Province is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtZipPostalCode.Text))
                {
                    MessageBox.Show("Zip/Postal/Code is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtCountryRegion.Text))
                {
                    MessageBox.Show("Country/Region is required", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand sql_register = new SqlCommand("addNewEmployee", conn);
                    sql_register.CommandType = CommandType.StoredProcedure;

                    sql_register.Parameters.AddWithValue("@employeeID", txtEmployeeID.Text);
                    sql_register.Parameters.AddWithValue("@employeeName", txtEmployeeName.Text);
                    sql_register.Parameters.AddWithValue("@userName", txtUsername.Text);
                    sql_register.Parameters.AddWithValue("@password", txtPassword.Text);
                    sql_register.Parameters.AddWithValue("@userType", comboBoxUserType.Text);
                    sql_register.Parameters.AddWithValue("@role", txtRole.Text);
                    sql_register.Parameters.AddWithValue("@jobTitle", comboBoxJobTitle.Text);
                    sql_register.Parameters.AddWithValue("@email", txtEmail.Text);
                    sql_register.Parameters.AddWithValue("@phone", txtPhoneNumber.Text);
                    sql_register.Parameters.AddWithValue("@homePhone", txtPhoneHome.Text);
                    sql_register.Parameters.AddWithValue("@address", txtAddress.Text);
                    sql_register.Parameters.AddWithValue("@city", txtCity.Text);
                    sql_register.Parameters.AddWithValue("@stateProvince", txtStateProvince.Text);
                    sql_register.Parameters.AddWithValue("@zipPostalCode", SqlDbType.Int).Value = Int32.Parse(txtZipPostalCode.Text);
                    sql_register.Parameters.AddWithValue("@countryRegion", txtCountryRegion.Text);

                    MemoryStream ms = new MemoryStream();
                    pbEmployee.Image.Save(ms, pbEmployee.Image.RawFormat);
                    sql_register.Parameters.AddWithValue("@photo", SqlDbType.Image).Value = ms.ToArray();

                    if(sql_register.ExecuteNonQuery() == 1)
                    {
                        RetrieveData();
                        btnRegister.Text = "Register";
                        btnUpdate.Enabled = true;
                        btnDelete.Text = "Delete";
                        MessageBox.Show("One employee has added.", "New Employee", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(btnDelete.Text == "Cancel")
            {
                btnRegister.Text = "Register";
                btnDelete.Text = "Delete";
                btnUpdate.Enabled = true;
            }else if(btnDelete.Text == "Delete")
            {
                if(MessageBox.Show("Do you want to delete " + txtEmployeeName.Text + "?", "Delete " + txtEmployeeName.Text + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand sql_delete = new SqlCommand("deleteEmployee", conn);
                    sql_delete.CommandType = CommandType.StoredProcedure;

                    sql_delete.Parameters.AddWithValue("@employeeID", txtEmployeeID.Text);

                    if(sql_delete.ExecuteNonQuery() == 1)
                    {
                        RetrieveData();
                        MessageBox.Show("One employee has deleted.", "Deleted Employee", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Invoke Method
            ClearFields();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SqlCommand sql_search = new SqlCommand("SELECT * FROM tblEmployees WHERE" + " CONCAT('|', EmployeeName, UserName, UserType, Role, JobTitle)" + " LIKE '%" + txtSearch.Text + "%';", conn);
            sql_search.CommandType = CommandType.Text;

            //Get Data with datatable (Shorter)
            SqlDataAdapter adapt = new SqlDataAdapter(sql_search);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            dgvEmployee.DataSource = dt;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand sql_update = new SqlCommand("updateEmployee", conn);
            sql_update.CommandType = CommandType.StoredProcedure;

            sql_update.Parameters.AddWithValue("@employeeID", txtEmployeeID.Text);
            sql_update.Parameters.AddWithValue("@employeeName", txtEmployeeName.Text);
            sql_update.Parameters.AddWithValue("@userName", txtUsername.Text);
            sql_update.Parameters.AddWithValue("@password", txtPassword.Text);
            sql_update.Parameters.AddWithValue("@userType", comboBoxUserType.Text);
            sql_update.Parameters.AddWithValue("@role", txtRole.Text);
            sql_update.Parameters.AddWithValue("@jobTitle", comboBoxJobTitle.Text);
            sql_update.Parameters.AddWithValue("@email", txtEmail.Text);
            sql_update.Parameters.AddWithValue("@phone", txtPhoneNumber.Text);
            sql_update.Parameters.AddWithValue("@homePhone", txtPhoneHome.Text);
            sql_update.Parameters.AddWithValue("@address", txtAddress.Text);
            sql_update.Parameters.AddWithValue("@city", txtCity.Text);
            sql_update.Parameters.AddWithValue("@stateProvince", txtStateProvince.Text);
            sql_update.Parameters.AddWithValue("@zipPostalCode", SqlDbType.Int).Value = Int32.Parse(txtZipPostalCode.Text);
            sql_update.Parameters.AddWithValue("@countryRegion", txtCountryRegion.Text);

            MemoryStream ms = new MemoryStream();
            pbEmployee.Image.Save(ms, pbEmployee.Image.RawFormat);
            sql_update.Parameters.AddWithValue("@photo", SqlDbType.Image).Value = ms.ToArray();

            if (sql_update.ExecuteNonQuery() == 1)
            {
                RetrieveData();
                MessageBox.Show("One employee has added.", "New Employee", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pbEmployee_Click(object sender, EventArgs e)
        {
            opfdEmployeePhoto.FilterIndex = 4;
            opfdEmployeePhoto.Filter = ("Images | *.png; *.jpg; *.jpeg; *.bmp;");
            opfdEmployeePhoto.FileName = string.Empty;

            //get image and show back in openFileDialog
            if (opfdEmployeePhoto.ShowDialog() == DialogResult.OK)
            {
                pbEmployee.Image = Image.FromFile(opfdEmployeePhoto.FileName);
            }
        }
    }
}
