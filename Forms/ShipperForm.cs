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
    public partial class ShipperForm : Form
    {
        public ShipperForm()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection();

        private void ShipperForm_Load(object sender, EventArgs e)
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
            SqlCommand sql_select = new SqlCommand("showShipper", conn);
            sql_select.CommandType = CommandType.Text;

            SqlDataAdapter adapt = new SqlDataAdapter(sql_select);
            DataTable dt = new DataTable();
            adapt.Fill(dt);

            dgvShipper.DataSource = dt;
        }

        private void dgvShipper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtShipperID.Text = dgvShipper.CurrentRow.Cells[0].Value.ToString();
            txtCompanyName.Text = dgvShipper.CurrentRow.Cells[1].Value.ToString();
            txtContactName.Text = dgvShipper.CurrentRow.Cells[2].Value.ToString();
            txtContactNumber.Text = dgvShipper.CurrentRow.Cells[3].Value.ToString();
            txtEmail.Text = dgvShipper.CurrentRow.Cells[4].Value.ToString();
            txtAddress.Text = dgvShipper.CurrentRow.Cells[5].Value.ToString();
            txtCountry.Text = dgvShipper.CurrentRow.Cells[6].Value.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SqlCommand sql_search = new SqlCommand("SELECT * FROM tblShippers WHERE" + " CONCAT('|', CompanyName, ContactName)" + " LIKE '%" + txtSearch.Text + "%';", conn);
            sql_search.CommandType = CommandType.Text;

            //Get Data with datatable (Shorter)
            SqlDataAdapter adapt = new SqlDataAdapter(sql_search);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            dgvShipper.DataSource = dt;
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
                if (string.IsNullOrEmpty(txtCompanyName.Text))
                {
                    MessageBox.Show("Company name is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtContactName.Text))
                {
                    MessageBox.Show("Contact name is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtContactNumber.Text))
                {
                    MessageBox.Show("Contact number is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Email is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtAddress.Text))
                {
                    MessageBox.Show("Address is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtCountry.Text))
                {
                    MessageBox.Show("Country is required.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else{
                    SqlCommand sql_create = new SqlCommand("addNewShipper", conn);
                    sql_create.CommandType = CommandType.StoredProcedure;

                    sql_create.Parameters.AddWithValue("@companyName", txtCompanyName.Text);
                    sql_create.Parameters.AddWithValue("@contactName", txtContactName.Text);
                    sql_create.Parameters.AddWithValue("@contactNumber", txtContactNumber.Text);
                    sql_create.Parameters.AddWithValue("@email", txtEmail.Text);
                    sql_create.Parameters.AddWithValue("@address", txtAddress.Text);
                    sql_create.Parameters.AddWithValue("@country", txtCountry.Text);

                    if (sql_create.ExecuteNonQuery() == 1)
                    {
                        RetrieveData();
                        btnCreate.Text = "Create";
                        btnUpdate.Enabled = true;
                        btnDelete.Text = "Delete";
                        MessageBox.Show("One shipper has added.", "New Shipper", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (MessageBox.Show("Do you want to delete " + txtContactName.Text + "?", "Delete " + txtContactName + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand sql_delete = new SqlCommand("deleteShipper", conn);
                    sql_delete.CommandType = CommandType.StoredProcedure;

                    sql_delete.Parameters.AddWithValue("@shipperID", SqlDbType.Int).Value = Int32.Parse(txtShipperID.Text);

                    if(sql_delete.ExecuteNonQuery() == 1)
                    {
                        RetrieveData();
                        MessageBox.Show("One shipper has deleted.", "Deleted Shipper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        void ClearField()
        {
            txtShipperID.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtContactName.Text = string.Empty;
            txtContactNumber.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtCountry.Text = string.Empty;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand sql_update = new SqlCommand("updateShipper", conn);
            sql_update.CommandType = CommandType.StoredProcedure;

            sql_update.Parameters.AddWithValue("@shipperID", SqlDbType.Int).Value = Int32.Parse(txtShipperID.Text);
            sql_update.Parameters.AddWithValue("@companyName", txtCompanyName.Text);
            sql_update.Parameters.AddWithValue("@contactName", txtContactName.Text);
            sql_update.Parameters.AddWithValue("@contactNumber", txtContactNumber.Text);
            sql_update.Parameters.AddWithValue("@email", txtEmail.Text);
            sql_update.Parameters.AddWithValue("@address", txtAddress.Text);
            sql_update.Parameters.AddWithValue("@country", txtCountry.Text);

            if (sql_update.ExecuteNonQuery() == 1)
            {
                RetrieveData();
                MessageBox.Show("One shipper has updated.", "Updated Shipper", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}