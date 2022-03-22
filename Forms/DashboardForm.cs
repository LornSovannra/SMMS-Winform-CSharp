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
    public partial class DashboardForm : Form
    {
        public DashboardForm()
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
            dgvEmployee.Columns[2].Visible = false;
            dgvEmployee.Columns[3].Visible = false;
            imgcolumn = (DataGridViewImageColumn)dgvEmployee.Columns[15];
            imgcolumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            Connection();
            RetrieveData();

            SqlCommand sql_select_categories = new SqlCommand("getCategory", conn);
            sql_select_categories.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapt_categories = new SqlDataAdapter(sql_select_categories);
            DataTable dt_categoires = new DataTable();
            adapt_categories.Fill(dt_categoires);
            lblCategories.Text = dt_categoires.Rows.Count.ToString();

            SqlCommand sql_select_products = new SqlCommand("SELECT SUM(Qty) AS TotalQty FROM tblProducts;", conn);
            sql_select_products.CommandType = CommandType.Text;
            SqlDataAdapter adapt_products = new SqlDataAdapter(sql_select_products);
            DataTable dt_products = new DataTable();
            adapt_products.Fill(dt_products);
            lblProducts.Text = dt_products.Rows[0]["TotalQty"].ToString();

            SqlCommand sql_select_shippers = new SqlCommand("showShipper", conn);
            sql_select_shippers.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter_shippers = new SqlDataAdapter(sql_select_shippers);
            DataTable dt_shippers = new DataTable();
            adapter_shippers.Fill(dt_shippers);
            lblShippers.Text = dt_shippers.Rows.Count.ToString();

            SqlCommand sql_select_customers = new SqlCommand("showCustomer", conn);
            sql_select_customers.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter_customers = new SqlDataAdapter(sql_select_customers);
            DataTable dt_customers = new DataTable();
            adapter_customers.Fill(dt_customers);
            lblCustomers.Text = dt_customers.Rows.Count.ToString();

            SqlCommand sql_select_employees = new SqlCommand("showEmployee", conn);
            sql_select_employees.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter_employees = new SqlDataAdapter(sql_select_employees);
            DataTable dt_employees = new DataTable();
            adapter_employees.Fill(dt_employees);
            lblEmployees.Text = dt_employees.Rows.Count.ToString();
        }


    }
}
