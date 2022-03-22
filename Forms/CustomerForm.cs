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

namespace SMMS.Forms
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection();

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            //Invoke Method
            Connection();
            RetrieveData();
        }

        void Connection()
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectToDB"].ToString();
            conn.Open();
        }

        void RetrieveData()
        {
            SqlCommand sql_select = new SqlCommand("showCustomer", conn);
            sql_select.CommandType = CommandType.Text;
            SqlDataAdapter adapt = new SqlDataAdapter(sql_select);
            DataTable dt = new DataTable();
            adapt.Fill(dt);

            dgvCustomer.DataSource = dt;
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCustomerID.Text = dgvCustomer.CurrentRow.Cells[0].Value.ToString();
            txtCustomerName.Text = dgvCustomer.CurrentRow.Cells[1].Value.ToString();
            txtJobTitlte.Text = dgvCustomer.CurrentRow.Cells[2].Value.ToString();
            txtPhone.Text = dgvCustomer.CurrentRow.Cells[3].Value.ToString();
            txtEmail.Text = dgvCustomer.CurrentRow.Cells[4].Value.ToString();
            txtAddress.Text = dgvCustomer.CurrentRow.Cells[5].Value.ToString();
            txtMumberID.Text = dgvCustomer.CurrentRow.Cells[6].Value.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SqlCommand sql_search = new SqlCommand("SELECT * FROM tblCustomers WHERE" + " CONCAT('|', CustomerName, JobTitle)" + " LIKE '%" + txtSearch.Text + "%';", conn);
            sql_search.CommandType = CommandType.Text;

            //Get Data with datatable (Shorter)
            SqlDataAdapter adapt = new SqlDataAdapter(sql_search);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            dgvCustomer.DataSource = dt;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (btnCreate.Text == "Create")
            {
                btnCreate.Text = "Save";
                btnUpdate.Enabled = false;
                btnDelete.Text = "Cancel";

                ClearField();
            }else if(btnCreate.Text == "Save")
            {
                if(string.IsNullOrEmpty(txtCustomerName.Text))
                {
                    MessageBox.Show("Customer name is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtJobTitlte.Text))
                {
                    MessageBox.Show("Job title is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtPhone.Text))
                {
                    MessageBox.Show("Phone is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Email is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(string.IsNullOrEmpty(txtAddress.Text))
                {
                    MessageBox.Show("Address is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(string.IsNullOrEmpty(txtMumberID.Text))
                {
                    MessageBox.Show("member ID is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand sql_create = new SqlCommand("addNewCustomer", conn);
                    sql_create.CommandType = CommandType.StoredProcedure;

                    sql_create.Parameters.AddWithValue("@cutomerName", txtCustomerName.Text);
                    sql_create.Parameters.AddWithValue("@jobTitile", txtJobTitlte.Text);
                    sql_create.Parameters.AddWithValue("@phone", txtPhone.Text);
                    sql_create.Parameters.AddWithValue("@email", txtEmail.Text);
                    sql_create.Parameters.AddWithValue("@address", txtAddress.Text);
                    sql_create.Parameters.AddWithValue("@memberID", SqlDbType.Int).Value = Int32.Parse(txtMumberID.Text);

                    if(sql_create.ExecuteNonQuery() == 1)
                    {
                        RetrieveData();
                        btnCreate.Text = "Create";
                        btnUpdate.Enabled = true;
                        btnDelete.Text = "Delete";
                        MessageBox.Show("One customer has added", "New Customer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (btnDelete.Text == "Cancel")
            {
                btnCreate.Text = "Create";
                btnUpdate.Enabled = true;
                btnDelete.Text = "Delete";
            }else if(btnDelete.Text == "Delete")
            {
                if(MessageBox.Show("Do you want to delete " + txtCustomerName.Text + "?", "Delete " + txtCustomerName.Text + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand sql_delete = new SqlCommand("deleteCustomer", conn);
                    sql_delete.CommandType = CommandType.StoredProcedure;

                    sql_delete.Parameters.AddWithValue("@customerID", SqlDbType.Int).Value = Int32.Parse(txtCustomerID.Text);

                    if(sql_delete.ExecuteNonQuery() == 1)
                    {
                        RetrieveData();
                        MessageBox.Show("One customer has deleted.", "Deleted Customer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        void ClearField()
        {
            txtCustomerID.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtJobTitlte.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtMumberID.Text = string.Empty;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand sql_update = new SqlCommand("updateCustomer", conn);
            sql_update.CommandType = CommandType.StoredProcedure;

            sql_update.Parameters.AddWithValue("@customerID", SqlDbType.Int).Value = Int32.Parse(txtCustomerID.Text);
            sql_update.Parameters.AddWithValue("@customerName", txtCustomerName.Text);
            sql_update.Parameters.AddWithValue("@jobTitile", txtJobTitlte.Text);
            sql_update.Parameters.AddWithValue("@phone", txtPhone.Text);
            sql_update.Parameters.AddWithValue("@email", txtEmail.Text);
            sql_update.Parameters.AddWithValue("@address", txtAddress.Text);
            sql_update.Parameters.AddWithValue("@memberID", SqlDbType.Int).Value = Int32.Parse(txtMumberID.Text);

            if (sql_update.ExecuteNonQuery() == 1)
            {
                RetrieveData();
                MessageBox.Show("One customer has updated", "Updated Customer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
