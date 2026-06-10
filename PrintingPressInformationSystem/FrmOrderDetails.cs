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
    public partial class FrmOrderDetails : Form
    {
        public int CurrentOrderId { get; set; }

        int selectedDetailId = 0;
        public FrmOrderDetails()
        {
            InitializeComponent();
            
            this.Load += FrmOrderDetails_Load;
        }

        private void FrmOrderDetails_Load(object sender, EventArgs e)
        {
            if (CurrentOrderId == 0)
            {
                MessageBox.Show("Order not selected");
                Close();
                return;
            }

            LoadServices();
            LoadOrderDetails();
        }

        void LoadServices()
        {
            using (var db = new PrintingPressdbEntities())
            {
                var services = db.Services
                    .Select(s => new
                    {
                        s.ServiceID,
                        Display = s.ServiceName
                    })
                    .ToList();

                cmbService.DataSource = services;
                cmbService.DisplayMember = "Display";
                cmbService.ValueMember = "ServiceID";
                cmbService.SelectedIndex = -1;
            }

        }

        void LoadOrderDetails()
        {
            using (var db = new PrintingPressdbEntities())
            {
                dgvDetails.DataSource =
                    (from d in db.OrderDetails
                     join s in db.Services on d.ServiceID equals s.ServiceID
                     where d.OrderID == CurrentOrderId
                     select new
                     {
                         d.OrderDetailID,
                         Service = s.ServiceName,
                         d.Quantity,
                         d.Rate,
                         Total = d.Quantity * d.Rate
                     }).ToList();
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbService.SelectedValue == null)
            {
                MessageBox.Show("Select a service");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                db.OrderDetails.Add(new OrderDetail
                {
                    OrderID = CurrentOrderId,
                    ServiceID = (int)cmbService.SelectedValue,
                    Quantity = (int)numQty.Value,
                    Rate = Convert.ToDecimal(txtRate.Text)
                });

                db.SaveChanges();
            }

            LoadOrderDetails();
            ClearFields();
        }

        private void dgvDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvDetails.Rows[e.RowIndex];
            selectedDetailId = Convert.ToInt32(row.Cells["OrderDetailID"].Value);

            cmbService.Text = row.Cells["Service"].Value.ToString();
            numQty.Value = Convert.ToDecimal(row.Cells["Quantity"].Value);
            txtRate.Text = row.Cells["Rate"].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedDetailId == 0)
            {
                MessageBox.Show("Select a detail first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                var d = db.OrderDetails.Find(selectedDetailId);
                if (d == null) return;

                db.OrderDetails.Remove(d);
                db.SaveChanges();
            }

            LoadOrderDetails();
            ClearFields();
        }

        void ClearFields()
        {
            cmbService.SelectedIndex = -1;
            numQty.Value = 1;
            txtRate.Clear();
            selectedDetailId = 0;
        }
    }
}
