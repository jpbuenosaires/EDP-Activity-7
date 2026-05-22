using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Timer = System.Windows.Forms.Timer;

namespace EcommerceIS
{
    public class FrmDashboard : Form
    {
        private Panel  pnlSidebar;
        private Panel  pnlTopBar;
        private Panel  pnlContent;
        private Panel  pnlCards;
        private DataGridView dgvOrders;
        private DataGridView dgvProducts;
        private Label  lblGreeting;
        private Label  lblDateTime;
        private Timer  clockTimer;

        public FrmDashboard()
        {
            InitializeComponent();
            LoadData();
            StartClock();
        }

        private void InitializeComponent()
        {
            this.Text            = "E-Commerce IS  —  Dashboard";
            this.Size            = new Size(1280, 720);
            this.StartPosition  = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox     = false;
            this.BackColor       = Color.FromArgb(245, 247, 252);
            this.Font            = new Font("Segoe UI", 9f);

            BuildSidebar();
            BuildTopBar();
            BuildContent();
        }

        // ── Sidebar ───────────────────────────────────────────
        private void BuildSidebar()
        {
            pnlSidebar = new Panel
            {
                Location  = new Point(0, 0),
                Size      = new Size(220, 720),
                BackColor = Color.FromArgb(22, 44, 90)
            };

            // Logo area
            var pnlLogo = new Panel
            {
                Location  = new Point(0, 0),
                Size      = new Size(220, 70),
                BackColor = Color.FromArgb(16, 34, 72)
            };
            pnlLogo.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using (var f = new Font("Segoe UI", 11f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.White))
                    g.DrawString("🛒  EcommerceDB", f, br, 16, 14);
                using (var f2 = new Font("Segoe UI", 7.5f))
                using (var br2 = new SolidBrush(Color.FromArgb(160, 180, 220)))
                    g.DrawString("Information System", f2, br2, 16, 38);
            };
            pnlSidebar.Controls.Add(pnlLogo);

            // Divider
            var lblDiv = new Label
            {
                Text      = "MAIN NAVIGATION",
                Font      = new Font("Segoe UI", 7f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 130, 170),
                Location  = new Point(16, 80),
                AutoSize  = true,
                BackColor = Color.Transparent
            };
            pnlSidebar.Controls.Add(lblDiv);

            // Nav items
            string[][] navItems =
            {
                new[] { "📊", "Dashboard",       "true"  },
                new[] { "👥", "Customers",        "false" },
                new[] { "📦", "Products",         "false" },
                new[] { "📋", "Orders",           "false" },
                new[] { "🏷️",  "Categories",      "false" },
                new[] { "📈", "Report Generator", "false" },
            };

            int yStart = 104;
            foreach (var item in navItems)
            {
                bool isActive = item[2] == "true";
                var btn = new Button
                {
                    Text      = $"  {item[0]}  {item[1]}",
                    Location  = new Point(8, yStart),
                    Size      = new Size(204, 38),
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font      = new Font("Segoe UI", 9.5f, isActive
                        ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = isActive ? Color.White : Color.FromArgb(180, 200, 230),
                    BackColor = isActive
                        ? Color.FromArgb(46, 90, 170)
                        : Color.Transparent,
                    Cursor    = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize  = 0;
                btn.FlatAppearance.MouseOverBackColor =
                    Color.FromArgb(35, 65, 130);

                if (item[1] == "Report Generator")
                    btn.Click += (s, e) =>
                    {
                        new FrmReportGenerator().ShowDialog();
                    };

                pnlSidebar.Controls.Add(btn);
                yStart += 42;
            }

            // Divider 2
            var lblDiv2 = new Label
            {
                Text      = "ACCOUNT",
                Font      = new Font("Segoe UI", 7f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 130, 170),
                Location  = new Point(16, yStart + 10),
                AutoSize  = true,
                BackColor = Color.Transparent
            };
            pnlSidebar.Controls.Add(lblDiv2);

            string[][] accountItems =
            {
                new[] { "ℹ️",  "About",   "false" },
                new[] { "🔒", "Logout",  "false" }
            };
            yStart += 34;
            foreach (var item in accountItems)
            {
                var btn = new Button
                {
                    Text      = $"  {item[0]}  {item[1]}",
                    Location  = new Point(8, yStart),
                    Size      = new Size(204, 38),
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font      = new Font("Segoe UI", 9.5f),
                    ForeColor = Color.FromArgb(180, 200, 230),
                    BackColor = Color.Transparent,
                    Cursor    = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize  = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 65, 130);

                if (item[1] == "About")
                    btn.Click += (s, e) => new FrmAbout().ShowDialog();
                if (item[1] == "Logout")
                    btn.Click += (s, e) =>
                    {
                        if (MessageBox.Show("Are you sure you want to logout?",
                            "Logout", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                            this.Close();
                    };

                pnlSidebar.Controls.Add(btn);
                yStart += 42;
            }

            // User card at bottom
            var pnlUser = new Panel
            {
                Location  = new Point(0, 645),
                Size      = new Size(220, 75),
                BackColor = Color.FromArgb(16, 34, 72)
            };
            pnlUser.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Avatar circle
                using (var br = new SolidBrush(Color.FromArgb(46, 90, 170)))
                    g.FillEllipse(br, 14, 14, 40, 40);
                using (var f = new Font("Segoe UI", 14f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.White))
                    g.DrawString("A", f, br, 26, 20);

                using (var f = new Font("Segoe UI", 9.5f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.White))
                    g.DrawString("Administrator", f, br, 62, 18);
                using (var f2 = new Font("Segoe UI", 8f))
                using (var br2 = new SolidBrush(Color.FromArgb(140, 170, 210)))
                    g.DrawString("admin@ecommerce.local", f2, br2, 62, 37);
            };
            pnlSidebar.Controls.Add(pnlUser);

            this.Controls.Add(pnlSidebar);
        }

        // ── Top bar ───────────────────────────────────────────
        private void BuildTopBar()
        {
            pnlTopBar = new Panel
            {
                Location  = new Point(220, 0),
                Size      = new Size(1060, 60),
                BackColor = Color.White
            };
            pnlTopBar.Paint += (s, e) =>
            {
                using (var p = new Pen(Color.FromArgb(230, 235, 245), 1))
                    e.Graphics.DrawLine(p, 0, 59, 1060, 59);
            };

            lblGreeting = new Label
            {
                Text      = "Good morning, Administrator 👋",
                Font      = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 40, 70),
                Location  = new Point(20, 12),
                AutoSize  = true
            };

            lblDateTime = new Label
            {
                Text      = DateTime.Now.ToString("dddd, MMMM dd, yyyy  ·  hh:mm tt"),
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.Gray,
                Location  = new Point(20, 36),
                AutoSize  = true
            };

            // Search box
            var pnlSearch = new Panel
            {
                Location  = new Point(580, 14),
                Size      = new Size(240, 32),
                BackColor = Color.FromArgb(245, 247, 252)
            };
            pnlSearch.Paint += (s, e) =>
            {
                using (var p = new Pen(Color.FromArgb(210, 215, 230), 1))
                    e.Graphics.DrawRectangle(p, 0, 0,
                        pnlSearch.Width - 1, pnlSearch.Height - 1);
            };
            var txtSearch = new TextBox
            {
                Text        = "Search...",
                Location    = new Point(6, 6),
                Size        = new Size(200, 20),
                BorderStyle = BorderStyle.None,
                Font        = new Font("Segoe UI", 9f),
                ForeColor   = Color.Gray,
                BackColor   = Color.FromArgb(245, 247, 252)
            };
            pnlSearch.Controls.Add(txtSearch);

            // Notification bell
            var btnNotif = new Button
            {
                Text      = "🔔",
                Location  = new Point(836, 14),
                Size      = new Size(34, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(245, 247, 252),
                Cursor    = Cursors.Hand,
                Font      = new Font("Segoe UI", 11f)
            };
            btnNotif.FlatAppearance.BorderSize = 0;

            // Refresh
            var btnRefresh = new Button
            {
                Text      = "⟳",
                Location  = new Point(876, 14),
                Size      = new Size(34, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(245, 247, 252),
                Cursor    = Cursors.Hand,
                Font      = new Font("Segoe UI", 13f)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();

            pnlTopBar.Controls.AddRange(new Control[]
                { lblGreeting, lblDateTime, pnlSearch, btnNotif, btnRefresh });

            this.Controls.Add(pnlTopBar);
        }

        // ── Main content ──────────────────────────────────────
        private void BuildContent()
        {
            pnlContent = new Panel
            {
                Location  = new Point(220, 60),
                Size      = new Size(1060, 660),
                BackColor = Color.FromArgb(245, 247, 252),
                AutoScroll = true
            };

            // Section label – KPI Cards
            var lblCards = SectionLabel("📊  Overview", new Point(16, 10));
            pnlContent.Controls.Add(lblCards);

            // KPI Cards row
            BuildKPICards();

            // Section – Recent Orders
            var lblOrders = SectionLabel("📋  Recent Orders", new Point(16, 220));
            pnlContent.Controls.Add(lblOrders);

            dgvOrders = BuildGrid(new Point(16, 248), new Size(640, 220));
            pnlContent.Controls.Add(dgvOrders);

            // Section – Low Stock Products
            var lblProd = SectionLabel("📦  Low Stock Products", new Point(674, 220));
            pnlContent.Controls.Add(lblProd);

            dgvProducts = BuildGrid(new Point(674, 248), new Size(370, 220));
            pnlContent.Controls.Add(dgvProducts);

            // Mini stat bar
            BuildStatBar();

            this.Controls.Add(pnlContent);
        }

        // ── KPI cards ─────────────────────────────────────────
        private void BuildKPICards()
        {
            var cards = new[]
            {
                new { Title="Total Customers", Value="--", Icon="👥", Color1="255,107,107", Color2="255,75,75"  },
                new { Title="Total Orders",    Value="--", Icon="📋", Color1="54,162,235",  Color2="26,140,215" },
                new { Title="Total Products",  Value="--", Icon="📦", Color1="75,192,192",  Color2="45,170,170" },
                new { Title="Total Revenue",   Value="--", Icon="💰", Color1="255,159,64",  Color2="235,135,40" },
            };

            for (int i = 0; i < cards.Length; i++)
            {
                int idx      = i;
                var cardData = cards[i];
                var pnl      = new Panel
                {
                    Location  = new Point(16 + i * 256, 38),
                    Size      = new Size(240, 170),
                    BackColor = Color.White,
                    Tag       = cardData.Title
                };

                // Drop shadow effect
                pnl.Paint += (s, e) =>
                {
                    var g  = e.Graphics;
                    var rc = ((Panel)s).ClientRectangle;

                    // Card body
                    using (var br = new SolidBrush(Color.White))
                        g.FillRectangle(br, rc);

                    // Top accent bar with gradient
                    string[] parts1 = cardData.Color1.Split(',');
                    string[] parts2 = cardData.Color2.Split(',');
                    var c1 = Color.FromArgb(
                        int.Parse(parts1[0]),
                        int.Parse(parts1[1]),
                        int.Parse(parts1[2]));
                    var c2 = Color.FromArgb(
                        int.Parse(parts2[0]),
                        int.Parse(parts2[1]),
                        int.Parse(parts2[2]));

                    using (var gbr = new LinearGradientBrush(
                        new Rectangle(0, 0, rc.Width, 6), c1, c2,
                        LinearGradientMode.Horizontal))
                        g.FillRectangle(gbr, 0, 0, rc.Width, 6);

                    // Icon
                    using (var f = new Font("Segoe UI Emoji", 26f))
                    using (var br = new SolidBrush(c1))
                        g.DrawString(cardData.Icon, f, br, 16, 18);

                    // Title
                    using (var f = new Font("Segoe UI", 8.5f))
                    using (var br = new SolidBrush(Color.FromArgb(140, 145, 165)))
                        g.DrawString(cardData.Title.ToUpper(), f, br, 16, 78);

                    // Value (read from Tag if populated)
                    string val = ((Panel)s).AccessibleName ?? "--";
                    using (var f = new Font("Segoe UI", 26f, FontStyle.Bold))
                    using (var br = new SolidBrush(Color.FromArgb(30, 40, 70)))
                        g.DrawString(val, f, br, 14, 96);

                    // Border
                    using (var pen = new Pen(Color.FromArgb(230, 235, 245), 1))
                        g.DrawRectangle(pen, 0, 0, rc.Width - 1, rc.Height - 1);
                };

                pnlContent.Controls.Add(pnl);
            }
        }

        // ── Stat bar at bottom ────────────────────────────────
        private void BuildStatBar()
        {
            var pnlStat = new Panel
            {
                Location  = new Point(16, 486),
                Size      = new Size(1028, 50),
                BackColor = Color.White
            };
            pnlStat.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using (var pen = new Pen(Color.FromArgb(230, 235, 245), 1))
                    g.DrawRectangle(pen, 0, 0, pnlStat.Width - 1, pnlStat.Height - 1);
                using (var f = new Font("Segoe UI", 8.5f))
                using (var br = new SolidBrush(Color.FromArgb(100, 110, 140)))
                {
                    g.DrawString("🟢  System Online", f, br, 14, 16);
                    g.DrawString("🗄️  EcommerceDB · MySQL 8.0", f, br, 180, 16);
                    g.DrawString("👤  Logged in: Administrator", f, br, 420, 16);
                    g.DrawString($"🕐  {DateTime.Now:hh:mm tt}", f, br, 680, 16);
                    g.DrawString("📅  " + DateTime.Now.ToString("MMM dd, yyyy"), f, br, 800, 16);
                }
            };
            pnlContent.Controls.Add(pnlStat);
        }

        // ── DataGridView helper ───────────────────────────────
        private DataGridView BuildGrid(Point loc, Size size)
        {
            var dgv = new DataGridView
            {
                Location            = loc,
                Size                = size,
                BorderStyle         = BorderStyle.None,
                BackgroundColor     = Color.White,
                RowHeadersVisible   = false,
                AllowUserToAddRows  = false,
                AllowUserToDeleteRows = false,
                ReadOnly            = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode       = DataGridViewSelectionMode.FullRowSelect,
                Font                = new Font("Segoe UI", 8.5f),
                GridColor           = Color.FromArgb(230, 235, 245),
                CellBorderStyle     = DataGridViewCellBorderStyle.SingleHorizontal
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor  = Color.FromArgb(22, 44, 90);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor  = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font       = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding    = new Padding(4, 0, 0, 0);
            dgv.ColumnHeadersHeight                      = 32;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.EnableHeadersVisualStyles   = false;
            dgv.RowTemplate.Height           = 26;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 220, 245);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 30, 60);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 255);

            return dgv;
        }

        // ── Load data from MySQL ──────────────────────────────
        private void LoadData()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();

                    // KPI: Customers
                    SetKPI(conn, 0, "SELECT COUNT(*) FROM customers", "👥");
                    SetKPI(conn, 1, "SELECT COUNT(*) FROM orders",    "📋");
                    SetKPI(conn, 2, "SELECT COUNT(*) FROM products",  "📦");
                    SetKPI(conn, 3,
                        "SELECT CONCAT('₱', FORMAT(SUM(od.Quantity * od.UnitPrice),2)) " +
                        "FROM orderdetails od",
                        "💰");

                    // Recent Orders
                    string sqlOrders =
                        "SELECT o.OrderID, CONCAT(c.FirstName,' ',c.LastName) AS Customer, " +
                        "o.OrderDate, o.Status, " +
                        "CONCAT('₱',FORMAT(SUM(od.Quantity*od.UnitPrice),2)) AS Total " +
                        "FROM orders o " +
                        "JOIN customers c ON o.CustomerID=c.CustomerID " +
                        "JOIN orderdetails od ON o.OrderID=od.OrderID " +
                        "GROUP BY o.OrderID ORDER BY o.OrderDate DESC LIMIT 10";

                    var daOrders = new MySqlDataAdapter(sqlOrders, conn);
                    var dtOrders = new DataTable();
                    daOrders.Fill(dtOrders);
                    dgvOrders.DataSource = dtOrders;

                    // Low Stock Products
                    string sqlProd =
                        "SELECT p.ProductName AS Product, p.StockQuantity AS Stock, " +
                        "c.CategoryName AS Category " +
                        "FROM products p JOIN categories c ON p.CategoryID=c.CategoryID " +
                        "ORDER BY p.StockQuantity ASC LIMIT 10";
                    var daProd = new MySqlDataAdapter(sqlProd, conn);
                    var dtProd = new DataTable();
                    daProd.Fill(dtProd);
                    dgvProducts.DataSource = dtProd;

                    StyleStatusColumn();
                    lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy  ·  hh:mm tt");
                }
            }
            catch (Exception ex)
            {
                // Offline fallback – show sample data
                LoadFallbackData();
            }
        }

        private void SetKPI(MySqlConnection conn, int cardIndex, string sql, string icon)
        {
            using (var cmd = new MySqlCommand(sql, conn))
            {
                var val = cmd.ExecuteScalar()?.ToString() ?? "0";
                // Find the card panel by index
                int count = 0;
                foreach (Control c in pnlContent.Controls)
                {
                    if (c is Panel p && p.Tag is string)
                    {
                        if (count == cardIndex)
                        {
                            p.AccessibleName = val;
                            p.Invalidate();
                            break;
                        }
                        count++;
                    }
                }
            }
        }

        private void LoadFallbackData()
        {
            // KPI fallback
            string[] vals = { "20", "25", "25", "₱48,205.50" };
            int count = 0;
            foreach (Control c in pnlContent.Controls)
            {
                if (c is Panel p && p.Tag is string && count < 4)
                {
                    p.AccessibleName = vals[count++];
                    p.Invalidate();
                }
            }

            // Orders fallback
            var dtO = new DataTable();
            dtO.Columns.AddRange(new[]
            {
                new DataColumn("OrderID"),
                new DataColumn("Customer"),
                new DataColumn("OrderDate"),
                new DataColumn("Status"),
                new DataColumn("Total")
            });
            object[][] rows =
            {
                new object[] { 1,  "Maria Santos",   "2025-03-10", "Delivered", "₱3,200.00" },
                new object[] { 2,  "Juan dela Cruz",  "2025-03-12", "Shipped",   "₱1,540.00" },
                new object[] { 3,  "Ana Reyes",       "2025-03-14", "Pending",   "₱870.50"  },
                new object[] { 4,  "Carlos Mendoza",  "2025-03-15", "Delivered", "₱5,100.00" },
                new object[] { 5,  "Rosa Villanueva", "2025-03-16", "Pending",   "₱420.00"  },
                new object[] { 6,  "Jose Bautista",   "2025-03-17", "Shipped",   "₱2,890.00" },
                new object[] { 7,  "Elena Torres",    "2025-03-18", "Delivered", "₱1,200.00" },
                new object[] { 8,  "Miguel Lim",      "2025-03-19", "Pending",   "₱3,660.00" },
                new object[] { 9,  "Gloria Ramos",    "2025-03-20", "Shipped",   "₱990.00"  },
                new object[] { 10, "Pedro Aquino",    "2025-03-21", "Pending",   "₱450.00"  },
            };
            foreach (var r in rows) dtO.Rows.Add(r);
            dgvOrders.DataSource = dtO;

            // Products fallback
            var dtP = new DataTable();
            dtP.Columns.AddRange(new[]
            {
                new DataColumn("Product"),
                new DataColumn("Stock"),
                new DataColumn("Category")
            });
            object[][] pRows =
            {
                new object[] { "Wireless Mouse",   2,  "Electronics"  },
                new object[] { "USB-C Hub",        3,  "Electronics"  },
                new object[] { "Notebook A5",      5,  "Stationery"   },
                new object[] { "Ballpen Set",      6,  "Stationery"   },
                new object[] { "Desk Lamp LED",    7,  "Furniture"    },
                new object[] { "Keyboard Cover",   8,  "Accessories"  },
                new object[] { "Mouse Pad XL",     9,  "Accessories"  },
                new object[] { "HDMI Cable 2m",    10, "Electronics"  },
            };
            foreach (var r in pRows) dtP.Rows.Add(r);
            dgvProducts.DataSource = dtP;

            StyleStatusColumn();
        }

        private void StyleStatusColumn()
        {
            if (dgvOrders.Columns.Contains("Status"))
            {
                foreach (DataGridViewRow row in dgvOrders.Rows)
                {
                    var cell = row.Cells["Status"];
                    if (cell?.Value == null) continue;
                    switch (cell.Value.ToString())
                    {
                        case "Delivered":
                            cell.Style.ForeColor   = Color.FromArgb(40, 167, 69);
                            cell.Style.BackColor   = Color.FromArgb(212, 237, 218);
                            cell.Style.Font        = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                            break;
                        case "Shipped":
                            cell.Style.ForeColor   = Color.FromArgb(0, 123, 255);
                            cell.Style.BackColor   = Color.FromArgb(204, 229, 255);
                            cell.Style.Font        = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                            break;
                        case "Pending":
                            cell.Style.ForeColor   = Color.FromArgb(133, 100, 4);
                            cell.Style.BackColor   = Color.FromArgb(255, 243, 205);
                            cell.Style.Font        = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                            break;
                    }
                }
            }
        }

        private void StartClock()
        {
            clockTimer          = new Timer { Interval = 1000 };
            clockTimer.Tick    += (s, e) =>
            {
                string hour = DateTime.Now.Hour < 12 ? "morning" :
                              DateTime.Now.Hour < 18 ? "afternoon" : "evening";
                lblGreeting.Text  = $"Good {hour}, Administrator 👋";
                lblDateTime.Text  = DateTime.Now.ToString("dddd, MMMM dd, yyyy  ·  hh:mm tt");
                pnlContent.Controls
                    .OfType<Panel>()
                    .Where(p => p.Tag?.ToString() == null)
                    .ToList()
                    .ForEach(p => p.Invalidate());
            };
            clockTimer.Start();
        }

        private Label SectionLabel(string text, Point loc) =>
            new Label
            {
                Text      = text,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 40, 70),
                Location  = loc,
                AutoSize  = true
            };

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            clockTimer?.Stop();
            base.OnFormClosed(e);
        }
    }
}