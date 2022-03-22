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
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection();

        private void ProductForm_Load(object sender, EventArgs e)
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
            SqlCommand sql_select = new SqlCommand("showProduct", conn);
            sql_select.CommandType = CommandType.Text;

            SqlDataAdapter adapt = new SqlDataAdapter(sql_select);
            DataTable dt = new DataTable();
            adapt.Fill(dt);

            dgvProduct.DataSource = dt;
            dgvProduct.Columns[10].Visible = false;
            DataGridViewImageColumn imgcolumn = new DataGridViewImageColumn();
            dgvProduct.RowTemplate.Height = 70;
            imgcolumn = (DataGridViewImageColumn)dgvProduct.Columns[9];
            imgcolumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtProductID.Text = dgvProduct.CurrentRow.Cells[0].Value.ToString();
            txtProductName.Text = dgvProduct.CurrentRow.Cells[1].Value.ToString();
            txtCategoryID.Text = dgvProduct.CurrentRow.Cells[2].Value.ToString();
            txtBarcode.Text = dgvProduct.CurrentRow.Cells[3].Value.ToString();
            dtpExpireDate.Text = dgvProduct.CurrentRow.Cells[4].Value.ToString();
            txtQuantity.Text = dgvProduct.CurrentRow.Cells[5].Value.ToString();
            txtUnitPriceIn.Text = dgvProduct.CurrentRow.Cells[6].Value.ToString();
            txtUnitPriceOut.Text = dgvProduct.CurrentRow.Cells[7].Value.ToString();
            txtDescription.Text = dgvProduct.CurrentRow.Cells[8].Value.ToString();

            //get and show image
            byte[] img = (byte[])dgvProduct.CurrentRow.Cells[9].Value;
            MemoryStream ms = new MemoryStream(img);
            pbProduct.Image = Image.FromStream(ms);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SqlCommand sql_search = new SqlCommand("SELECT * FROM tblProducts WHERE" + " CONCAT('|', ProductName, Description)" + " LIKE '%" + txtSearch.Text + "%';", conn);
            sql_search.CommandType = CommandType.Text;

            //Get Data with datatable (Shorter)
            SqlDataAdapter adapt = new SqlDataAdapter(sql_search);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            dgvProduct.DataSource = dt;
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
                if(string.IsNullOrEmpty(txtProductName.Text))
                {
                    MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(string.IsNullOrEmpty(txtCategoryID.Text))
                {
                    MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(string.IsNullOrEmpty(txtBarcode.Text))
                {
                    MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(string.IsNullOrEmpty(dtpExpireDate.Text))
                {
                    MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(string.IsNullOrEmpty(txtQuantity.Text))
                {
                    MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if(string.IsNullOrEmpty(txtUnitPriceIn.Text))
                {
                    MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (string.IsNullOrEmpty(txtUnitPriceOut.Text))
                {
                    MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand sql_create = new SqlCommand("addNewProduct", conn);
                    sql_create.CommandType = CommandType.StoredProcedure;

                    sql_create.Parameters.AddWithValue("@productName", txtProductName.Text);
                    sql_create.Parameters.AddWithValue("@productDescription", txtDescription.Text);
                    sql_create.Parameters.AddWithValue("@categoryID", SqlDbType.Int).Value = Int32.Parse(txtCategoryID.Text);
                    sql_create.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    sql_create.Parameters.AddWithValue("@expireDate", SqlDbType.Date).Value = dtpExpireDate.Value;
                    sql_create.Parameters.AddWithValue("@qty", SqlDbType.Int).Value = Int32.Parse(txtQuantity.Text);
                    sql_create.Parameters.AddWithValue("@priceIN", SqlDbType.Decimal).Value = Double.Parse(txtUnitPriceIn.Text);
                    sql_create.Parameters.AddWithValue("@priceOut", SqlDbType.Decimal).Value = Double.Parse(txtUnitPriceOut.Text);

                    MemoryStream ms = new MemoryStream();
                    pbProduct.Image.Save(ms, pbProduct.Image.RawFormat);
                    sql_create.Parameters.AddWithValue("@productImage", SqlDbType.Image).Value = ms.ToArray();

                    if(sql_create.ExecuteNonQuery() == 1)
                    {
                        RetrieveData();
                        btnCreate.Text = "Create";
                        btnUpdate.Enabled = true;
                        btnDelete.Text = "Delete";
                        MessageBox.Show("One product has added.", "New Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if(MessageBox.Show("Do you want to delete " + txtProductName.Text + "?", "Delete " + txtProductName.Text + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand sql_delete = new SqlCommand("deleteProduct", conn);
                    sql_delete.CommandType = CommandType.StoredProcedure;

                    sql_delete.Parameters.AddWithValue("@productID", SqlDbType.Int).Value = Int32.Parse(txtProductID.Text);

                    if (sql_delete.ExecuteNonQuery() == 1)
                    {
                        RetrieveData();
                        MessageBox.Show("One product has deleted.", "Deleted Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        void ClearField()
        {
            txtProductID.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtCategoryID.Text = string.Empty;
            txtBarcode.Text = string.Empty;
            dtpExpireDate.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtUnitPriceIn.Text = string.Empty;
            txtUnitPriceOut.Text = string.Empty;
            txtDescription.Text = string.Empty;
            pbProduct.Image = Properties.Resources.ProductPhoto;
        }

        private void pbProduct_Click(object sender, EventArgs e)
        {
            opfdProductPhoto.FilterIndex = 4;
            opfdProductPhoto.Filter = ("Images | *.png; *.jpg; *.jpeg; *.bmp;");
            opfdProductPhoto.FileName = string.Empty;

            //get image and show back in openFileDialog
            if (opfdProductPhoto.ShowDialog() == DialogResult.OK)
            {
                pbProduct.Image = Image.FromFile(opfdProductPhoto.FileName);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductName.Text))
            {
                MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(txtCategoryID.Text))
            {
                MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(txtBarcode.Text))
            {
                MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(dtpExpireDate.Text))
            {
                MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(txtUnitPriceIn.Text))
            {
                MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(txtUnitPriceOut.Text))
            {
                MessageBox.Show("Category name is required.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlCommand sql_update = new SqlCommand("updateProduct", conn);
                sql_update.CommandType = CommandType.StoredProcedure;

                sql_update.Parameters.AddWithValue("@productID", SqlDbType.Int).Value = Int32.Parse(txtProductID.Text);
                sql_update.Parameters.AddWithValue("@productName", txtProductName.Text);
                sql_update.Parameters.AddWithValue("@productDescription", txtDescription.Text);
                sql_update.Parameters.AddWithValue("@categoryID", SqlDbType.Int).Value = Int32.Parse(txtCategoryID.Text);
                sql_update.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                sql_update.Parameters.AddWithValue("@expireDate", SqlDbType.Date).Value = dtpExpireDate.Value;
                sql_update.Parameters.AddWithValue("@qty", SqlDbType.Int).Value = Int32.Parse(txtQuantity.Text);
                sql_update.Parameters.AddWithValue("@priceIN", SqlDbType.Decimal).Value = Double.Parse(txtUnitPriceIn.Text);
                sql_update.Parameters.AddWithValue("@priceOut", SqlDbType.Decimal).Value = Double.Parse(txtUnitPriceOut.Text);

                MemoryStream ms = new MemoryStream();
                pbProduct.Image.Save(ms, pbProduct.Image.RawFormat);
                sql_update.Parameters.AddWithValue("@productImage", SqlDbType.Image).Value = ms.ToArray();

                if (sql_update.ExecuteNonQuery() == 1)
                {
                    RetrieveData();
                    MessageBox.Show("One product has updated.", "Updated Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}