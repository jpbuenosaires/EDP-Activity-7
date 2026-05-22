using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EcommerceIS
{
    public class FrmReportGenerator : Form
    {
        private Panel       pnlLeft;
        private Panel       pnlRight;
        private ComboBox    cboReportType;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private ComboBox    cboStatus;
        private Button      btnGenerate;
        private Button      btnExportExcel;
        private Button      btnExportPDF;
        private Button      btnPrint;
        private DataGridView dgvReport;
        private Label       lblRecordCount;
        private Panel       pnlSummary;

        public FrmReportGenerator()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text            = "E-Commerce IS  —  Report Generator";
            this.Size            = new Size(1200, 700);
            this.StartPosition  = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.BackColor       = Color.FromArgb(245, 247, 252);
            this.Font            = new Font("Segoe UI", 9f);

            BuildLeftPanel();
            BuildRightPanel();
            BuildStatusBar();

            // Reorder controls to ensure proper docking z-order
            Control pnlStatus = this.Controls[2];
            this.Controls.Clear();
            this.Controls.AddRange(new Control[] { pnlRight, pnlLeft, pnlStatus });
        }



        // ── Left filter panel ─────────────────────────────────
        private void BuildLeftPanel()
        {
            pnlLeft = new Panel
            {
                Location  = new Point(0, 70),
                Size      = new Size(260, 600),
                BackColor = Color.White,
                Dock      = DockStyle.Left
            };
            pnlLeft.Paint += (s, e) =>
            {
                using (var p = new Pen(Color.FromArgb(230, 235, 245), 1))
                    e.Graphics.DrawLine(p, 259, 0, 259, 600);
            };

            int y = 16;

            var lblFilters = new Label
            {
                Text      = "⚙  REPORT FILTERS",
                Font      = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 44, 90),
                Location  = new Point(16, y),
                AutoSize  = true
            };
            y += 28;

            // Separator
            pnlLeft.Controls.Add(MakeSep(y)); y += 12;

            // Report Type
            pnlLeft.Controls.Add(FieldLabel("REPORT TYPE", new Point(16, y))); y += 20;
            cboReportType = MakeCombo(new Point(16, y), new Size(228, 28)); y += 40;
            cboReportType.Items.AddRange(new object[]
            {
                "Sales Summary Report",
                "Order Details Report",
                "Customer Report",
                "Product Inventory Report",
                "Invoice Report",
                "Low Stock Report"
            });
            cboReportType.SelectedIndex = 0;

            // Date From
            pnlLeft.Controls.Add(FieldLabel("DATE FROM", new Point(16, y))); y += 20;
            dtpFrom = new DateTimePicker
            {
                Location = new Point(16, y),
                Size     = new Size(228, 28),
                Font     = new Font("Segoe UI", 9f),
                Value    = DateTime.Now.AddMonths(-1),
                Format   = DateTimePickerFormat.Short
            };
            y += 40;

            // Date To
            pnlLeft.Controls.Add(FieldLabel("DATE TO", new Point(16, y))); y += 20;
            dtpTo = new DateTimePicker
            {
                Location = new Point(16, y),
                Size     = new Size(228, 28),
                Font     = new Font("Segoe UI", 9f),
                Value    = DateTime.Now,
                Format   = DateTimePickerFormat.Short
            };
            y += 40;

            // Status filter
            pnlLeft.Controls.Add(FieldLabel("ORDER STATUS", new Point(16, y))); y += 20;
            cboStatus = MakeCombo(new Point(16, y), new Size(228, 28)); y += 40;
            cboStatus.Items.AddRange(new object[]
                { "All", "Pending", "Shipped", "Delivered" });
            cboStatus.SelectedIndex = 0;

            pnlLeft.Controls.Add(MakeSep(y)); y += 16;

            // Generate button
            btnGenerate = new Button
            {
                Text      = "▶  GENERATE REPORT",
                Location  = new Point(16, y),
                Size      = new Size(228, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 77, 140),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.Click   += BtnGenerate_Click;
            btnGenerate.MouseEnter += (s, e) =>
                btnGenerate.BackColor = Color.FromArgb(46, 109, 180);
            btnGenerate.MouseLeave += (s, e) =>
                btnGenerate.BackColor = Color.FromArgb(30, 77, 140);
            y += 50;

            pnlLeft.Controls.Add(MakeSep(y)); y += 16;

            // Export buttons
            var lblExp = FieldLabel("EXPORT / PRINT", new Point(16, y)); y += 24;
            pnlLeft.Controls.Add(lblExp);

            btnExportExcel = MakeExportBtn("📊  Export to Excel", new Point(16, y),
                Color.FromArgb(33, 115, 70)); y += 44;
            btnExportExcel.Click += (s, e) =>
            {
                if (dgvReport.DataSource is DataTable dt && dt.Rows.Count > 0)
                    ExcelExportService.ExportReport(dt, cboReportType.Text, FrmDashboard.LoggedInUser, "Bar");
                else
                    MessageBox.Show("No data to export. Generate a report first.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };

            btnExportPDF = MakeExportBtn("📄  Export to PDF", new Point(16, y),
                Color.FromArgb(196, 43, 36)); y += 44;
            btnExportPDF.Click += (s, e) =>
                MessageBox.Show("Export to PDF feature is available.\nRequires iTextSharp NuGet package.",
                    "Export PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnPrint = MakeExportBtn("🖨️  Print Report", new Point(16, y),
                Color.FromArgb(100, 100, 120)); y += 44;
            btnPrint.Click += (s, e) =>
                MessageBox.Show("Print dialog will open here.",
                    "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);

            pnlLeft.Controls.AddRange(new Control[]
            {
                lblFilters,
                cboReportType,
                dtpFrom, dtpTo,
                cboStatus,
                btnGenerate,
                btnExportExcel, btnExportPDF, btnPrint
            });

            this.Controls.Add(pnlLeft);
        }

        // ── Right results panel ───────────────────────────────
        private void BuildRightPanel()
        {
            pnlRight = new Panel
            {
                Location  = new Point(260, 70),
                Size      = new Size(940, 600),
                BackColor = Color.FromArgb(245, 247, 252),
                Dock      = DockStyle.Fill
            };

            // Summary cards
            pnlSummary = new Panel
            {
                Location  = new Point(10, 10),
                Size      = new Size(920, 80),
                BackColor = Color.Transparent,
                Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlSummary.Paint += PnlSummary_Paint;
            pnlRight.Controls.Add(pnlSummary);

            // Grid title bar
            var pnlGridTitle = new Panel
            {
                Location  = new Point(10, 98),
                Size      = new Size(920, 36),
                BackColor = Color.FromArgb(22, 44, 90),
                Anchor    = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnlGridTitle.Paint += (s, e) =>
            {
                using (var f = new Font("Segoe UI", 9.5f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.White))
                    e.Graphics.DrawString(
                        $"  {cboReportType?.Text ?? "Sales Summary Report"}  ·  " +
                        $"{dtpFrom?.Value:MM/dd/yyyy} – {dtpTo?.Value:MM/dd/yyyy}",
                        f, br, 8, 10);
            };
            pnlRight.Controls.Add(pnlGridTitle);

            // Results grid
            dgvReport = new DataGridView
            {
                Location            = new Point(10, 134),
                Size                = new Size(920, 430),
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
                CellBorderStyle     = DataGridViewCellBorderStyle.SingleHorizontal,
                Anchor              = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 60, 110);
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReport.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dgvReport.ColumnHeadersHeight                     = 32;
            dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvReport.EnableHeadersVisualStyles   = false;
            dgvReport.RowTemplate.Height           = 26;
            dgvReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 225, 250);
            dgvReport.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 30, 60);
            dgvReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 255);
            pnlRight.Controls.Add(dgvReport);

            lblRecordCount = new Label
            {
                Text      = "Click 'Generate Report' to load data.",
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.Gray,
                Location  = new Point(10, 568),
                AutoSize  = true,
                Anchor    = AnchorStyles.Bottom | AnchorStyles.Left
            };
            pnlRight.Controls.Add(lblRecordCount);

            this.Controls.Add(pnlRight);

            // Auto-generate on open
            BtnGenerate_Click(null, EventArgs.Empty);
        }

        // ── Summary card painter ──────────────────────────────
        private void PnlSummary_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var cards = new[]
            {
                new { Label="TOTAL RECORDS", Val=dgvReport?.Rows.Count.ToString() ?? "0",
                      C1=Color.FromArgb(30,77,140), C2=Color.FromArgb(46,109,180) },
                new { Label="DATE RANGE",
                      Val=$"{dtpFrom?.Value:MM/dd} – {dtpTo?.Value:MM/dd}",
                      C1=Color.FromArgb(40,167,69), C2=Color.FromArgb(60,190,100) },
                new { Label="REPORT TYPE",
                      Val=ShortenReport(cboReportType?.Text ?? "Sales"),
                      C1=Color.FromArgb(255,140,0), C2=Color.FromArgb(255,165,40) },
                new { Label="STATUS FILTER",
                      Val=cboStatus?.Text ?? "All",
                      C1=Color.FromArgb(102,16,242), C2=Color.FromArgb(130,60,255) },
            };

            int gap = 10;
            int totalGap = gap * (cards.Length - 1);
            int cardW = Math.Max(150, (pnlSummary.Width - totalGap) / cards.Length);
            int cardH = 70;
            for (int i = 0; i < cards.Length; i++)
            {
                int x = i * (cardW + gap);
                var rc = new Rectangle(x, 0, cardW, cardH);

                // White card
                using (var br = new SolidBrush(Color.White))
                    g.FillRectangle(br, rc);

                // Left accent
                using (var gbr = new LinearGradientBrush(
                    new Rectangle(x, 0, 5, cardH), cards[i].C1, cards[i].C2,
                    LinearGradientMode.Vertical))
                    g.FillRectangle(gbr, x, 0, 5, cardH);

                // Border
                using (var pen = new Pen(Color.FromArgb(230, 235, 245), 1))
                    g.DrawRectangle(pen, x, 0, cardW - 1, cardH - 1);

                // Label
                using (var f = new Font("Segoe UI", 7f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.FromArgb(130, 140, 165)))
                    g.DrawString(cards[i].Label, f, br, x + 14, 10);

                // Value
                using (var f = new Font("Segoe UI", 16f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.FromArgb(30, 40, 70)))
                    g.DrawString(cards[i].Val, f, br, x + 12, 28);
            }
        }

        // ── Generate click ────────────────────────────────────
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string sql = GetReportSQL();
                    var da = new MySqlDataAdapter(sql, conn);
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvReport.DataSource = dt;
                    lblRecordCount.Text  =
                        $"✔  {dt.Rows.Count} record(s) found  ·  " +
                        $"Generated: {DateTime.Now:MM/dd/yyyy hh:mm tt}";
                }
            }
            catch
            {
                LoadFallbackReport();
            }

            pnlSummary.Invalidate();
            if (pnlRight.Controls.Contains(
                pnlRight.Controls.Find("pnlGridTitle", false)
                    .Length > 0 ? null : null))
            {
            }
            // Refresh grid title
            foreach (Control c in pnlRight.Controls)
                if (c is Panel p && p.BackColor == Color.FromArgb(22, 44, 90))
                    p.Invalidate();
        }

        private string GetReportSQL()
        {
            string from   = dtpFrom.Value.ToString("yyyy-MM-dd");
            string to     = dtpTo.Value.ToString("yyyy-MM-dd");
            string status = cboStatus.Text;

            switch (cboReportType.SelectedIndex)
            {
                case 0: // Sales Summary
                    return
                        "SELECT o.OrderID, CONCAT(c.FirstName,' ',c.LastName) AS Customer, " +
                        "o.OrderDate, o.Status, COUNT(od.OrderDetailID) AS Items, " +
                        "CONCAT('₱',FORMAT(SUM(od.Quantity*od.UnitPrice),2)) AS Total " +
                        "FROM orders o " +
                        "JOIN customers c ON o.CustomerID=c.CustomerID " +
                        "JOIN orderdetails od ON o.OrderID=od.OrderID " +
                       $"WHERE o.OrderDate BETWEEN '{from}' AND '{to}' " +
                       (status != "All" ? $"AND o.Status='{status}' " : "") +
                        "GROUP BY o.OrderID ORDER BY o.OrderDate DESC";

                case 1: // Order Details
                    return
                        "SELECT od.OrderDetailID, o.OrderID, p.ProductName, " +
                        "od.Quantity, CONCAT('₱',FORMAT(od.UnitPrice,2)) AS UnitPrice, " +
                        "CONCAT('₱',FORMAT(od.Quantity*od.UnitPrice,2)) AS LineTotal, " +
                        "o.Status " +
                        "FROM orderdetails od " +
                        "JOIN orders o ON od.OrderID=o.OrderID " +
                        "JOIN products p ON od.ProductID=p.ProductID " +
                       $"WHERE o.OrderDate BETWEEN '{from}' AND '{to}'";

                case 2: // Customer Report
                    return
                        "SELECT c.CustomerID, CONCAT(c.FirstName,' ',c.LastName) AS Name, " +
                        "c.Email, c.RegistrationDate, COUNT(o.OrderID) AS TotalOrders, " +
                        "CONCAT('₱',FORMAT(SUM(od.Quantity*od.UnitPrice),2)) AS TotalSpent " +
                        "FROM customers c " +
                        "LEFT JOIN orders o ON c.CustomerID=o.CustomerID " +
                        "LEFT JOIN orderdetails od ON o.OrderID=od.OrderID " +
                        "GROUP BY c.CustomerID ORDER BY TotalSpent DESC";

                case 3: // Product Inventory
                    return
                        "SELECT p.ProductID, p.ProductName, c.CategoryName, " +
                        "CONCAT('₱',FORMAT(p.Price,2)) AS Price, p.StockQuantity AS Stock, " +
                        "CASE WHEN p.StockQuantity<=5 THEN 'LOW STOCK' " +
                        "     WHEN p.StockQuantity<=15 THEN 'MEDIUM' ELSE 'GOOD' END AS StockStatus " +
                        "FROM products p JOIN categories c ON p.CategoryID=c.CategoryID " +
                        "ORDER BY p.StockQuantity ASC";

                case 4: // Invoice Report
                    return
                        "SELECT o.OrderID, CONCAT(c.FirstName,' ',c.LastName) AS Customer, " +
                        "c.Email, o.OrderDate, p.ProductName, od.Quantity, " +
                        "CONCAT('₱',FORMAT(od.UnitPrice,2)) AS UnitPrice, " +
                        "CONCAT('₱',FORMAT(od.Quantity*od.UnitPrice,2)) AS LineTotal " +
                        "FROM orders o JOIN customers c ON o.CustomerID=c.CustomerID " +
                        "JOIN orderdetails od ON o.OrderID=od.OrderID " +
                        "JOIN products p ON od.ProductID=p.ProductID " +
                       $"WHERE o.OrderDate BETWEEN '{from}' AND '{to}'";

                default: // Low Stock
                    return
                        "SELECT p.ProductName AS Product, p.StockQuantity AS Stock, " +
                        "CONCAT('₱',FORMAT(p.Price,2)) AS Price, c.CategoryName AS Category " +
                        "FROM products p JOIN categories c ON p.CategoryID=c.CategoryID " +
                        "WHERE p.StockQuantity<=10 ORDER BY p.StockQuantity ASC";
            }
        }

        private void LoadFallbackReport()
        {
            var dt = new DataTable();
            dt.Columns.AddRange(new[]
            {
                new DataColumn("OrderID"),    new DataColumn("Customer"),
                new DataColumn("OrderDate"),  new DataColumn("Status"),
                new DataColumn("Items"),      new DataColumn("Total")
            });
            object[][] rows =
            {
                new object[] { 1,  "Maria Santos",    "2025-03-10", "Delivered", 3, "₱3,200.00"  },
                new object[] { 2,  "Juan dela Cruz",  "2025-03-12", "Shipped",   2, "₱1,540.00"  },
                new object[] { 3,  "Ana Reyes",       "2025-03-14", "Pending",   1, "₱870.50"    },
                new object[] { 4,  "Carlos Mendoza",  "2025-03-15", "Delivered", 4, "₱5,100.00"  },
                new object[] { 5,  "Rosa Villanueva", "2025-03-16", "Pending",   2, "₱420.00"    },
                new object[] { 6,  "Jose Bautista",   "2025-03-17", "Shipped",   3, "₱2,890.00"  },
                new object[] { 7,  "Elena Torres",    "2025-03-18", "Delivered", 2, "₱1,200.00"  },
                new object[] { 8,  "Miguel Lim",      "2025-03-19", "Pending",   5, "₱3,660.00"  },
                new object[] { 9,  "Gloria Ramos",    "2025-03-20", "Shipped",   1, "₱990.00"    },
                new object[] { 10, "Pedro Aquino",    "2025-03-21", "Pending",   2, "₱450.00"    },
                new object[] { 11, "Linda Cruz",      "2025-03-22", "Delivered", 3, "₱2,100.00"  },
                new object[] { 12, "Rene Diaz",       "2025-03-23", "Shipped",   1, "₱780.00"    },
            };
            foreach (var r in rows) dt.Rows.Add(r);
            dgvReport.DataSource = dt;
            lblRecordCount.Text  =
                $"✔  {dt.Rows.Count} record(s) loaded (offline mode)  ·  " +
                $"{DateTime.Now:MM/dd/yyyy hh:mm tt}";
        }

        // ── Helpers ───────────────────────────────────────────
        private Label FieldLabel(string text, Point loc) =>
            new Label
            {
                Text      = text,
                Font      = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 90, 120),
                Location  = loc,
                AutoSize  = true
            };

        private ComboBox MakeCombo(Point loc, Size sz)
        {
            var cb = new ComboBox
            {
                Location      = loc,
                Size          = sz,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font          = new Font("Segoe UI", 9f),
                FlatStyle     = FlatStyle.Flat
            };
            return cb;
        }

        private Button MakeExportBtn(string text, Point loc, Color color)
        {
            var btn = new Button
            {
                Text      = text,
                Location  = loc,
                Size      = new Size(228, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) =>
                btn.BackColor = ControlPaint.Light(color, 0.15f);
            btn.MouseLeave += (s, e) => btn.BackColor = color;
            return btn;
        }

        private Panel MakeSep(int y) =>
            new Panel
            {
                Location  = new Point(16, y),
                Size      = new Size(228, 1),
                BackColor = Color.FromArgb(220, 225, 235)
            };

        private string ShortenReport(string full)
        {
            if (full.Length <= 10) return full;
            string[] words = full.Split(' ');
            return words.Length >= 2
                ? words[0] + " " + words[1]
                : full.Substring(0, 10);
        }

        private void BuildStatusBar()
        {
            var pnlStatus = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 28,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            pnlStatus.Paint += (s, e) =>
            {
                using (var p = new Pen(Color.FromArgb(215, 220, 230)))
                    e.Graphics.DrawLine(p, 0, 0, 1200, 0);
                using (var f = new Font("Segoe UI", 7.5f))
                using (var br = new SolidBrush(Color.DimGray))
                    e.Graphics.DrawString(
                        $"  🟢  EcommerceDB System  ·  Report Generator  ·  " +
                        $"{DateTime.Now:MM/dd/yyyy hh:mm tt}",
                        f, br, 4, 7);
            };
            this.Controls.Add(pnlStatus);
        }
    }
}