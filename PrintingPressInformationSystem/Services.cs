using System;
using System.Linq;
using System.Windows.Forms;

namespace PrintingPressInformationSystem
{
    public partial class Services : Form
    {
        int selectedId = 0;

        public Services()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
        }

        private void Services_Load(object sender, EventArgs e)
        {
            LoadServices();
        }

        // Load services into grid
        void LoadServices()
        {
            using (var db = new PrintingPressdbEntities())
            {
                dgvServices.DataSource = db.Services
                    .Select(s => new
                    {
                        s.ServiceID,
                        s.ServiceName,
                        s.Rate
                    })
                    .ToList();
            }
        }

        // Select row
        private void dgvServices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvServices.Rows[e.RowIndex];

            selectedId = Convert.ToInt32(row.Cells[0].Value);
            txtServiceName.Text = row.Cells[1].Value.ToString();
            txtRate.Text = row.Cells[2].Value.ToString();
        }

        // Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var db = new PrintingPressdbEntities())
            {
                Service s = new Service
                {
                    ServiceName = txtServiceName.Text,
                    Rate = Convert.ToDecimal(txtRate.Text)
                };

                db.Services.Add(s);
                db.SaveChanges();
            }

            LoadServices();
            ClearFields();
            MessageBox.Show("Service saved");
        }

        // Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select a service first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                var s = db.Services.Find(selectedId);
                if (s == null) return;

                s.ServiceName = txtServiceName.Text;
                s.Rate = Convert.ToDecimal(txtRate.Text);
                db.SaveChanges();
            }

            LoadServices();
            ClearFields();
            MessageBox.Show("Service updated");
        }

        // Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select a service first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                var s = db.Services.Find(selectedId);
                if (s == null) return;

                if (db.OrderDetails.Any(o => o.ServiceID == selectedId))
                {
                    MessageBox.Show("Cannot delete service used in orders");
                    return;
                }

                db.Services.Remove(s);
                db.SaveChanges();
            }

            LoadServices();
            ClearFields();
            MessageBox.Show("Service deleted");
        }

        // Clear form
        void ClearFields()
        {
            txtServiceName.Clear();
            txtRate.Clear();
            selectedId = 0;
        }

        public void PopulateGrid()
        {
            using (var db = new PrintingPressdbEntities())
            {
                dgvServices.DataSource = db.Services
                    .Select(s => new
                    {
                        s.ServiceID,
                        s.ServiceName,
                        s.Rate
                    })
                    .ToList();
            }
           // dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvServices.RowTemplate.Height = 40;
            dgvServices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvServices.ReadOnly = true;
            dgvServices.AllowUserToAddRows = false;
        }

        private void txtServiceName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}