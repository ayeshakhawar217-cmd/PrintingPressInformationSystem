/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintingPressInformationSystem
{
    public partial class Orders : Form
    {
        public Orders()
        {
            InitializeComponent();
        }
    }
}
*/
using PrintingPressInformationSystem;
using System;
using System.Linq;
using System.Windows.Forms;

namespace NetProj
{
    public partial class FrmOrders : Form
    {
        int selectedOrderId = 0;

        public FrmOrders()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
        }

        private void FrmOrders_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            LoadOrders();

            cmbStatus.Items.AddRange(new string[]
            {
                "Pending", "In Progress", "Completed", "Delivered"
            });

            cmbStatus.SelectedIndex = 0;
        }

        // 🔹 Load customers into ComboBox
        void LoadCustomers()
        {
            using (var db = new PrintingPressdbEntities())
            {
                cmbCustomer.DataSource = db.Customers
                    .Select(c => new
                    {
                        c.CustomerID,
                        c.CustomerName
                    })
                    .ToList();

                cmbCustomer.DisplayMember = "CustomerName";
                cmbCustomer.ValueMember = "CustomerID";
            }
        }

        // 🔹 Load orders into DataGridView (NO lazy loading issue)
        void LoadOrders()
        {
            using (var db = new PrintingPressdbEntities())
            {
                dgvOrders.DataSource =
                    (from o in db.Orders
                     join c in db.Customers
                     on o.CustomerID equals c.CustomerID
                     select new
                     {
                         o.OrderID,
                         c.CustomerName,
                         o.OrderDate,
                         o.DeliveryDate,
                         o.Status
                     }).ToList();
            }

            dgvOrders.Columns[0].Visible = false; // Hide OrderID
        }

        // 🔹 Select order
        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvOrders.Rows[e.RowIndex];

            selectedOrderId = Convert.ToInt32(row.Cells[0].Value);

            cmbCustomer.Text = row.Cells[1].Value.ToString();
            dtOrderDate.Value = Convert.ToDateTime(row.Cells[2].Value);
            dtDeliveryDate.Value = Convert.ToDateTime(row.Cells[3].Value);
            cmbStatus.Text = row.Cells[4].Value.ToString();
        }

        // 🔹 SAVE
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var db = new PrintingPressdbEntities())
            {
                Order o = new Order
                {
                    CustomerID = (int)cmbCustomer.SelectedValue,
                    OrderDate = dtOrderDate.Value,
                    DeliveryDate = dtDeliveryDate.Value,
                    Status = cmbStatus.Text
                };

                db.Orders.Add(o);
                db.SaveChanges();
            }

            LoadOrders();
            ClearForm();
            MessageBox.Show("Order saved successfully");
        }

        // 🔹 UPDATE
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedOrderId == 0)
            {
                MessageBox.Show("Select an order first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                var o = db.Orders.Find(selectedOrderId);
                if (o == null) return;

                o.CustomerID = (int)cmbCustomer.SelectedValue;
                o.OrderDate = dtOrderDate.Value;
                o.DeliveryDate = dtDeliveryDate.Value;
                o.Status = cmbStatus.Text;

                db.SaveChanges();
            }

            LoadOrders();
            ClearForm();
            MessageBox.Show("Order updated successfully");
        }

        // 🔹 DELETE
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedOrderId == 0)
            {
                MessageBox.Show("Select an order first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                if (db.OrderDetails.Any(d => d.OrderID == selectedOrderId))
                {
                    MessageBox.Show("Delete order details first");
                    return;
                }

                var o = db.Orders.Find(selectedOrderId);
                if (o == null) return;

                db.Orders.Remove(o);
                db.SaveChanges();
            }

            LoadOrders();
            ClearForm();
            MessageBox.Show("Order deleted successfully");
        }

        // 🔹 Clear form
        void ClearForm()
        {
            selectedOrderId = 0;
            cmbStatus.SelectedIndex = 0;
            dtOrderDate.Value = DateTime.Now;
            dtDeliveryDate.Value = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedOrderId == 0)
            {
                MessageBox.Show("Select an order first");
                return;
            }

            FrmOrderDetails frm = new FrmOrderDetails();   // create form
            frm.CurrentOrderId = selectedOrderId;          // pass OrderID
            frm.ShowDialog();
        }

        public void PopulateGrid()
        {
            using (var db = new PrintingPressdbEntities())
            {
                dgvOrders.DataSource = db.Orders
                    .Select(o => new
                    {
                        o.OrderID,
                        Customer = o.Customer.CustomerName,   // assuming navigation property
                        o.OrderDate,
                        o.DeliveryDate,
                        o.Status
                    })
                    .ToList();
            }
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvOrders.RowTemplate.Height = 40;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.ReadOnly = true;
            dgvOrders.AllowUserToAddRows = false;
        }

    }
}