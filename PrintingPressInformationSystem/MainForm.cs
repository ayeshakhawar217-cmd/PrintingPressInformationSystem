using NetProj;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PrintingPressInformationSystem
{
    public partial class MainForm : Form
    {
        private RoundedPanel panelSidebar;
        private RoundedPanel panelTop;
        private RoundedPanel panelCenter;
        private RoundedPanel panelRight;

        public MainForm()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(245, 248, 245);
            this.DoubleBuffered = true;
            this.ClientSize = new Size(1200, 700);

            InitializeSidebar();
            InitializeTopPanel();
            InitializeRightPanel();
            InitializeCenterPanel();
        }

        #region Sidebar (UNCHANGED)
        private void InitializeSidebar()
        {
            panelSidebar = new RoundedPanel
            {
                Location = new Point(0, 0),
                Size = new Size(250, this.ClientSize.Height),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
                BackColor = Color.FromArgb(47, 83, 79),
                BorderRadius = 50,
                BorderSize = 0
            };

            this.Controls.Add(panelSidebar);

            Label lblTitle = new Label
            {
                Text = "Printing Press\nInformation System",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(panelSidebar.Width, 80),
                Location = new Point(0, 20)
            };
            panelSidebar.Controls.Add(lblTitle);

            int y = lblTitle.Bottom + 20;
            int h = 80;

            AddSidebarButton("Home", y, h);
            AddSidebarButton("Customers", y + h, h);
            AddSidebarButton("Services", y + h * 2, h);
            AddSidebarButton("Orders", y + h * 3, h);
            AddSidebarButton("Payments", y + h * 4, h);
        }

        private void AddSidebarButton(string text, int top, int height)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(panelSidebar.Width, height),
                Location = new Point(0, top),
                BackColor = Color.FromArgb(60, 95, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.Click += (s, e) =>
            {
                panelCenter.Controls.Clear();

                if (text == "Home") AddDashboardCards();
                else if (text == "Customers") { Form1 f = new Form1(); f.PopulateGrid(); ShowGrid(f.dgvCustomers); }
                else if (text == "Services") { Services sForm = new Services(); sForm.PopulateGrid(); ShowGrid(sForm.dgvServices); }
                else if (text == "Orders") { FrmOrders o = new FrmOrders(); o.PopulateGrid(); ShowGrid(o.dgvOrders); }
                else if (text == "Payments") { FrmPayments p = new FrmPayments(); p.PopulateGrid(); ShowGrid(p.dgvPayments); }
            };

            panelSidebar.Controls.Add(btn);
        }
        #endregion

        #region Top Panel (UNCHANGED FROM WORKING VERSION)
        private void InitializeTopPanel()
        {
            panelTop = new RoundedPanel
            {
                Location = new Point(panelSidebar.Right + 20, 20),
                Size = new Size(this.ClientSize.Width - panelSidebar.Width - 40, 110),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                BorderRadius = 30,
                BorderSize = 0
            };
            this.Controls.Add(panelTop);

            Label lblDashboard = new Label
            {
                Text = "Dashboard Overview",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(30, 18),
                AutoSize = true
            };

            Label lblInfo = new Label
            {
                Text = "Offset Printing • Digital Printing • Binding • Large Format • Fast Delivery",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(32, 50),
                AutoSize = true
            };

            // Dummy visual buttons (NO logic)
            Button btnReports = CreateTopDummyButton("Reports", panelTop.Width - 300);
            Button btnAnalytics = CreateTopDummyButton("Analytics", panelTop.Width - 210);
            Button btnNotify = CreateTopDummyButton("🔔", panelTop.Width - 120);

            panelTop.Controls.Add(lblDashboard);
            panelTop.Controls.Add(lblInfo);
            panelTop.Controls.Add(btnReports);
            panelTop.Controls.Add(btnAnalytics);
            panelTop.Controls.Add(btnNotify);
        }

        private Button CreateTopDummyButton(string text, int left)
        {
            return new Button
            {
                Text = text,
                Size = new Size(80, 35),
                Location = new Point(left, 35),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(250, 255, 250),
                ForeColor = Color.FromArgb(47, 83, 79),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
        }
        #endregion

        #region Right Panel (UNCHANGED)
        private void InitializeRightPanel()
        {
            panelRight = new RoundedPanel
            {
                Size = new Size(220, this.ClientSize.Height - 160),
                Location = new Point(this.ClientSize.Width - 240, panelTop.Bottom + 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.FromArgb(47, 83, 79),
                BorderRadius = 30,
                BorderSize = 0
            };
            this.Controls.Add(panelRight);

            panelRight.Controls.Add(CreateStat("Today's Orders", "8", 40));
            panelRight.Controls.Add(CreateStat("Pending Jobs", "3", 100));
            panelRight.Controls.Add(CreateStat("Revenue", "₨ 42,000", 160));

            panelRight.Controls.Add(new Label
            {
                Text = "★★★★★",
                ForeColor = Color.Gold,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                Location = new Point(20, 245),
                AutoSize = true
            });
        }

        private Control CreateStat(string title, string value, int top)
        {
            Panel p = new Panel { Size = new Size(180, 50), Location = new Point(20, top) };

            p.Controls.Add(new Label
            {
                Text = title,
                ForeColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 9),
                AutoSize = true
            });

            p.Controls.Add(new Label
            {
                Text = value,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(0, 18),
                AutoSize = true
            });

            return p;
        }
        #endregion

        #region Center Panel (CARD DASHBOARD)
        private void InitializeCenterPanel()
        {
            int horizontalGap = 10; // gap between side panels and center panel
            int verticalGap = 10;   // gap from top panel

            int centerX = panelSidebar.Right + horizontalGap;
            int centerY = panelTop.Bottom + verticalGap;

            int centerWidth = panelRight.Left - horizontalGap - centerX; // distance from left of center to left of right panel minus gap
            int centerHeight = this.ClientSize.Height - centerY - verticalGap; // distance to bottom minus gap

            panelCenter = new RoundedPanel
            {
                BorderRadius = 30,
                BorderSize = 0,
                BackColor = this.BackColor,
                Location = new Point(centerX, centerY),
                Size = new Size(centerWidth, centerHeight),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            this.Controls.Add(panelCenter);

            AddDashboardCards();
        }


        private void AddDashboardCards()
        {
            panelCenter.Controls.Clear();

            CreateCard("👤", "Customers",
                "Create, view, update & manage customer records",
                0, () => OpenForm(new Form1()));

            CreateCard("🛠", "  Services",
                "  Add printing services, rates & job types",
                1, () => OpenForm(new Services()));

            CreateCard("📦", "  Orders",
                "  Create orders, assign services & track jobs",
                2, () => OpenForm(new FrmOrders()));

            CreateCard("💳", "  Payments",
                "  Manage invoices, payments & balances",
                3, () => OpenForm(new FrmPayments()));
        }

        private void CreateCard(string icon, string title, string desc, int index, Action onClick)
        {
            int cardHeight = 110;
            int spacing = 3;

            RoundedPanel card = new RoundedPanel
            {
                Size = new Size(panelCenter.Width - 40, cardHeight),
                Location = new Point(20, 20 + index * (cardHeight + spacing)),
                BackColor = Color.White,
                BorderRadius = 50,
                BorderSize = 0,
                Cursor = Cursors.Hand
            };

            Label lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 28),
                ForeColor = Color.FromArgb(200, 160, 0),
                Location = new Point(20, 25),
                AutoSize = true
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(47, 83, 79),
                Location = new Point(80, 25),
                AutoSize = true
            };

            Label lblDesc = new Label
            {
                Text = desc,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(82, 55),
                AutoSize = true
            };

            // Click forwarding (NO functionality loss)
            card.Click += (s, e) => onClick();
            lblIcon.Click += (s, e) => onClick();
            lblTitle.Click += (s, e) => onClick();
            lblDesc.Click += (s, e) => onClick();

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblDesc);

            panelCenter.Controls.Add(card);
        }

        private void OpenForm(Form f)
        {
            panelCenter.Controls.Clear();
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;
            panelCenter.Controls.Add(f);
            f.Show();
        }

        private void ShowGrid(DataGridView grid)
        {
            panelCenter.Controls.Clear();
            grid.Dock = DockStyle.Fill;
            panelCenter.Controls.Add(grid);
        }
        #endregion
    }
}
