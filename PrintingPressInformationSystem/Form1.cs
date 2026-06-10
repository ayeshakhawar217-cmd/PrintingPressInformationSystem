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
    public partial class Form1 : Form
    {
        int selectedId = 0;
        public Form1()
        {
            InitializeComponent();
          

        }

      
         

          
        
         


        private void FrmCustomers_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        void LoadCustomers()
        {
            using (var db = new PrintingPressdbEntities())
            {
                dgvCustomers.DataSource = db.Customers
                    .Select(c => new
                    {
                        c.CustomerID,
                        c.CustomerName,
                        c.Phone,
                        c.Address
                    })
                    .ToList();
            }
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; 

            selectedId = Convert.ToInt32(
                dgvCustomers.Rows[e.RowIndex].Cells["CustomerID"].Value
            );

            txtName.Text = dgvCustomers.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString();
            txtPhone.Text = dgvCustomers.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
            txtAddress.Text = dgvCustomers.Rows[e.RowIndex].Cells["Address"].Value.ToString();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var db = new PrintingPressdbEntities())
            {
                Customer c = new Customer
                {
                    CustomerName = txtName.Text,
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text
                };

                db.Customers.Add(c);
                db.SaveChanges();
            }

            LoadCustomers();
            ClearFields();
            MessageBox.Show("Customer saved");
        }



        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select a customer first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                var customer = db.Customers.Find(selectedId);
                if (customer == null) return;

                customer.CustomerName = txtName.Text;
                customer.Phone = txtPhone.Text;
                customer.Address = txtAddress.Text;

                db.SaveChanges();
            }

            LoadCustomers();
            ClearFields();
            MessageBox.Show("Customer updated");
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select a customer first");
                return;
            }

            using (var db = new PrintingPressdbEntities())
            {
                var customer = db.Customers.Find(selectedId);
                if (customer == null) return;

                // Optional safety: check related orders
                if (db.Orders.Any(o => o.CustomerID == selectedId))
                {
                    MessageBox.Show("Cannot delete customer with existing orders");
                    return;
                }

                db.Customers.Remove(customer);
                db.SaveChanges();
            }

            LoadCustomers();
            ClearFields();
            MessageBox.Show("Customer deleted");
        }

        void ClearFields()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            selectedId = 0;
        }

        public void PopulateGrid()
        {
            using (var db = new PrintingPressdbEntities())
            {
                dgvCustomers.DataSource = db.Customers
                    .Select(c => new
                    {
                        c.CustomerID,
                        c.CustomerName,
                        c.Phone,
                        c.Address
                    })
                    .ToList();

                dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvCustomers.RowTemplate.Height = 40;  // taller rows
                dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvCustomers.ReadOnly = true;
                dgvCustomers.AllowUserToAddRows = false;

            }
        }



    }
}
