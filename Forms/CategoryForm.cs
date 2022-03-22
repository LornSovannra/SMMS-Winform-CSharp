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
    public partial class CategoryForm : Form
    {
        public CategoryForm()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection();

        private void CategoryForm_Load(object sender, EventArgs e)
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
            SqlCommand sql_select = new SqlCommand("getCategory", conn);
            sql_select.CommandType = CommandType.Text;
            SqlDataAdapter adapt = new SqlDataAdapter(sql_select);
            DataTable dt = new DataTable();
            adapt.Fill(dt);

            dgvCategory.DataSource = dt;
        }

        private void dgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCategoryID.Text = dgvCategory.CurrentRow.Cells[0].Value.ToString();
            txtCategoryName.Text = dgvCategory.CurrentRow.Cells[1].Value.ToString();
            txtDescription.Text = dgvCategory.CurrentRow.Cells[2].Value.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SqlCommand sql_search = new SqlCommand("SELECT * FROM tblCategories WHERE" + " CONCAT('|', CategoryName, Description)" + " LIKE '%" + txtSearch.Text + "%';", conn);
            sql_search.CommandType = CommandType.Text;

            //Get Data with datatable (Shorter)
            SqlDataAdapter adapt = new SqlDataAdapter(sql_search);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            dgvCategory.DataSource = dt;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if(btnCreate.Text == "Create")
            {
                btnCreate.Text = "Save";
                btnUpdate.Enabled = false;
                btnDelete.Text = "Cancel";
                ClearField();
            }else if(btnCreate.Text == "Save")
            {
                if(string.IsNullOrEmpty(txtCategoryName.Text))
                {
                    MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand sql_create = new SqlCommand("addNewCategory", conn);
                    sql_create.CommandType = CommandType.StoredProcedure;

                    sql_create.Parameters.AddWithValue("@catName", SqlDbType.VarChar).Value = txtCategoryName.Text;
                    sql_create.Parameters.AddWithValue("@desc", SqlDbType.VarChar).Value = txtDescription.Text;
                    sql_create.Parameters.AddWithValue("@createdBy", UserLogin.getUserName());

                    if (sql_create.ExecuteNonQuery() >= 1)
                    {
                        RetrieveData();
                        btnCreate.Text = "Create";
                        btnUpdate.Enabled = true;
                        btnDelete.Text = "Delete";
                        MessageBox.Show("One category has been added.", "New Category", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(btnDelete.Text == "Cancel")
            {
                btnCreate.Text = "Create";
                btnUpdate.Enabled = true;
                btnDelete.Text = "Delete";
            }else if(btnDelete.Text == "Delete")
            {
                if(MessageBox.Show("Do you want to delete " + txtCategoryName.Text + "?", "Delete " + txtCategoryName.Text + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand sql_delete = new SqlCommand("deleteCategory", conn);
                    sql_delete.CommandType = CommandType.StoredProcedure;

                    sql_delete.Parameters.AddWithValue("@catID", SqlDbType.Int).Value = Int32.Parse(txtCategoryID.Text);

                    if (sql_delete.ExecuteNonQuery() >= 1)
                    {
                        RetrieveData();
                        MessageBox.Show("One category has been deleted.", "Deleted Category", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete " + txtCategoryName.Text + "?", "Delete " + txtCategoryName.Text + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlCommand sql_update = new SqlCommand("updateCategory", conn);
                sql_update.CommandType = CommandType.StoredProcedure;

                sql_update.Parameters.AddWithValue("@catID", SqlDbType.Int).Value = Int32.Parse(txtCategoryID.Text);
                sql_update.Parameters.AddWithValue("@catName", SqlDbType.VarChar).Value = txtCategoryName.Text;
                sql_update.Parameters.AddWithValue("@desc", SqlDbType.VarChar).Value = txtDescription.Text;
                sql_update.Parameters.AddWithValue("@updatedBy", UserLogin.getUserName());

                if (sql_update.ExecuteNonQuery() >= 1)
                {
                    RetrieveData();
                    MessageBox.Show("One category has been updated.", "Updated Category", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        void ClearField()
        {
            //foreach(Control ctrl in this.Controls)
            //{
            //    if(ctrl is TextBox)
            //    {
            //        (ctrl as TextBox).Clear();
            //    }
            //}

            txtCategoryID.Text = string.Empty;
            txtCategoryName.Text = string.Empty;
            txtDescription.Text = string.Empty;
        }
    }
}
