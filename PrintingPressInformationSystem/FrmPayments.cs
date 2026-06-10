using System;
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
    public partial class FrmPayments : Form
    {
        int selectedId = 0;
        public FrmPayments()
        {
            InitializeComponent();
            this.Load += FrmPayments_Load;

            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FrmPayments_Load(object sender, EventArgs e)
        {
            LoadOrders();
            LoadPayments();
        }

        void LoadOrders()
        {
            using (var db = new PrintingPressdbEntities())
            {
                var orders = db.Orders
                    .Include("Customer")   // IMPORTANT
                    .Select(o => new
                    {
                        o.OrderID,
                        Display = o.OrderID + " - " + o.Customer.CustomerName
                    })
                    .ToList();

                cmbOrders.DataSource = orders;
                cmbOrders.DisplayMember = "Display";
                cmbOrders.ValueMember = "OrderID";
                cmbOrders.SelectedIndex = -1;
            }
        }

        // Load payments into DataGridView
        void LoadPayments()
        {
            using (var db = new PrintingPressdbEntities())
            {
                dgvPayments.DataSource = db.Payments
                    .Select(p => new
                    {
                        p.PaymentID,
                        p.OrderID,
                        Customer = p.Order.Customer.CustomerName,
                        p.PaidAmount,
                        p.PaymentDate
                    })
                    .ToList();
            }
        }

        // DataGridView CellClick
        private void dgvPayments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvPayments.Rows[e.RowIndex];
            selectedId = Convert.ToInt32(row.Cells["PaymentID"].Value);
            cmbOrders.SelectedValue = Convert.ToInt32(row.Cells["OrderID"].Value);
            txtPaidAmount.Text = row.Cells["PaidAmount"].Value.ToString();
            dtPaymentDate.Value = Convert.ToDateTime(row.Cells["PaymentDate"].Value);
        }
        private void dgvPayments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbOrders.SelectedIndex < 0)
            {
                MessageBox.Show("Select an order first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                Payment p = new Payment
                {
                    OrderID = (int)cmbOrders.SelectedValue,
                    PaidAmount = Convert.ToDecimal(txtPaidAmount.Text),
                    PaymentDate = dtPaymentDate.Value
                };

                db.Payments.Add(p);
                db.SaveChanges();
            }

            LoadPayments();
            ClearFields();
            MessageBox.Show("Payment saved");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select a payment first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                var p = db.Payments.Find(selectedId);
                if (p == null) return;

                p.OrderID = (int)cmbOrders.SelectedValue;
                p.PaidAmount = Convert.ToDecimal(txtPaidAmount.Text);
                p.PaymentDate = dtPaymentDate.Value;

                db.SaveChanges();
            }

            LoadPayments();
            ClearFields();
            MessageBox.Show("Payment updated");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select a payment first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                var p = db.Payments.Find(selectedId);
                if (p == null) return;

                db.Payments.Remove(p);
                db.SaveChanges();
            }

            LoadPayments();
            ClearFields();
            MessageBox.Show("Payment deleted");
        }

        void ClearFields()
        {
            txtPaidAmount.Clear();
            selectedId = 0;
            cmbOrders.SelectedIndex = -1;
            dtPaymentDate.Value = DateTime.Now;
        }

        public void PopulateGrid()
        {
            using (var db = new PrintingPressdbEntities())
            {
                dgvPayments.DataSource = db.Payments
                    .Select(p => new
                    {
                        p.PaymentID,
                        p.OrderID,       // use FK directly
                        p.PaidAmount,
                        p.PaymentDate
                    })
                    .ToList();
            }
            dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvPayments.RowTemplate.Height = 40;
            dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayments.ReadOnly = true;
            dgvPayments.AllowUserToAddRows = false;
        }

    }

}
