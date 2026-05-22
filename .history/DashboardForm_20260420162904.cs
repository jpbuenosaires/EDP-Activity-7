using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace InformationSystem
{
    public class DashboardForm : Form
    {
        private Panel pnlSidebar, pnlTopBar, pnlContent;
        private Label lblWelcome, lblDate;
        private string currentUser = "Administrator";

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text            = "InfoSys – Dashboard";
            Size            = new Size(1100, 680);
            StartPosition   = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize     = new Size(900, 580);
            BackColor       = Color.FromArgb(240, 244, 248);
            Font            = new Font("Segoe UI", 9.5f);

            BuildSidebar();
            BuildTopBar();
            BuildContent();
        }

        // ── SIDEBAR ─────────────────────────────────────────────────────
        private void BuildSidebar()
        {
            pnlSidebar = new Panel
            {
                Dock      = DockStyle.Left,
                Width     = 220,
                BackColor = Color.FromArgb(26, 58, 92)
            };

            // Logo area
            var pnlLogo = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 70,
                BackColor = Color.FromArgb(15, 40, 70)
            };
            var lblLogo = new Label
            {
                Text      = "  ⬡  InfoSys",
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 13, FontStyle.Bold),
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(16, 0, 0, 0)
            };
            pnlLogo.Controls.Add(lblLogo);

            // Nav items
            string[] navItems = { "🏠  Dashboard", "👥  Users", "📁  Records",
                                   "📊  Reports", "⚙  Settings", "❓  Help" };
            int[] navY = { 90, 138, 186, 234, 282, 330 };

            pnlSidebar.Controls.Add(pnlLogo);

            for (int i = 0; i < navItems.Length; i++)
            {
                bool isActive = i == 0;
                var btn = new Button
                {
                    Text      = "  " + navItems[i],
                    Size      = new Size(220, 42),
                    Location  = new Point(0, navY[i]),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = isActive
                        ? Color.FromArgb(37, 99, 235)
                        : Color.Transparent,
                    ForeColor = Color.White,
                    Font      = new Font("Segoe UI", 9.5f,
                        isActive ? FontStyle.Bold : FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor    = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize  = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
                pnlSidebar.Controls.Add(btn);

                // active indicator bar
                if (isActive)
                {
                    var bar = new Panel
                    {
                        BackColor = Color.FromArgb(96, 165, 250),
                        Size      = new Size(4, 42),
                        Location  = new Point(0, navY[i])
                    };
                    pnlSidebar.Controls.Add(bar);
                }
            }

            // Logout at bottom
            var btnLogout = new Button
            {
                Text      = "  🚪  Logout",
                Size      = new Size(220, 42),
                Location  = new Point(0, 560),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 9.5f),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor    = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Are you sure you want to logout?",
                    "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    new LoginForm().Show();
                    Close();
                }
            };
            pnlSidebar.Controls.Add(btnLogout);

            Controls.Add(pnlSidebar);
        }

        // ── TOP BAR ─────────────────────────────────────────────────────
        private void BuildTopBar()
        {
            pnlTopBar = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 58,
                BackColor = Color.White
            };
            pnlTopBar.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(220, 228, 236), 1);
                e.Graphics.DrawLine(pen, 0, pnlTopBar.Height - 1,
                    pnlTopBar.Width, pnlTopBar.Height - 1);
            };

            lblWelcome = new Label
            {
                Text      = $"Welcome back, {currentUser}!",
                Font      = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 58, 92),
                AutoSize  = true,
                Location  = new Point(20, 17)
            };

            lblDate = new Label
            {
                Text      = DateTime.Now.ToString("dddd, MMMM dd, yyyy"),
                ForeColor = Color.FromArgb(100, 120, 140),
                AutoSize  = true,
                Location  = new Point(820, 20)
            };

            var btnAbout = new Button
            {
                Text      = "ℹ About",
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(26, 115, 232),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(700, 15),
                Cursor    = Cursors.Hand
            };
            btnAbout.FlatAppearance.BorderSize = 0;
            btnAbout.Click += (s, e) => new AboutForm().ShowDialog();

            pnlTopBar.Controls.AddRange(new Control[] { lblWelcome, lblDate, btnAbout });
            Controls.Add(pnlTopBar);
        }

        // ── CONTENT AREA ────────────────────────────────────────────────
        private void BuildContent()
        {
            pnlContent = new Panel
            {
                Location  = new Point(220, 58),
                Size      = new Size(880, 622),
                BackColor = Color.FromArgb(240, 244, 248),
                AutoScroll = true
            };

            // ── KPI Cards ───────────────────────────────────────────────
            var kpiData = new (string Title, string Value, string Change, Color Accent)[]
            {
                ("Total Users",     "1,284",  "↑ 12%  this month",  Color.FromArgb(37, 99, 235)),
                ("Active Records",  "8,745",  "↑ 5%   this week",   Color.FromArgb(22, 163, 74)),
                ("Reports Today",   "34",     "↓ 3%   vs yesterday", Color.FromArgb(217, 119, 6)),
                ("System Alerts",   "2",      "▼ Resolved: 10",      Color.FromArgb(220, 38, 38))
            };

            for (int i = 0; i < kpiData.Length; i++)
            {
                var (title, value, change, accent) = kpiData[i];
                pnlContent.Controls.Add(CreateKpiCard(title, value, change, accent,
                    20 + i * 210, 20));
            }

            // ── Chart panel ─────────────────────────────────────────────
            var pnlChart = new Panel
            {
                Size      = new Size(530, 240),
                Location  = new Point(20, 160),
                BackColor = Color.White
            };
            pnlChart.Paint += DrawBarChart;

            // ── Recent activity panel ────────────────────────────────────
            var pnlActivity = new Panel
            {
                Size      = new Size(295, 240),
                Location  = new Point(565, 160),
                BackColor = Color.White
            };
            pnlActivity.Paint += DrawActivityFeed;

            // ── Recent records table ─────────────────────────────────────
            var pnlTable = new Panel
            {
                Size      = new Size(840, 210),
                Location  = new Point(20, 415),
                BackColor = Color.White
            };
            pnlTable.Paint += DrawRecentTable;

            pnlContent.Controls.AddRange(new Control[]
                { pnlChart, pnlActivity, pnlTable });

            Controls.Add(pnlContent);

            // keep content panel sized with form
            Resize += (s, e) =>
            {
                pnlContent.Size = new Size(
                    Width - 220 - 16,
                    Height - 58 - 39);
                pnlTable.Width  = pnlContent.Width - 40;
            };
        }

        // ── KPI Card Factory ─────────────────────────────────────────────
        private Panel CreateKpiCard(string title, string value, string change,
            Color accent, int x, int y)
        {
            var card = new Panel
            {
                Size      = new Size(195, 120),
                Location  = new Point(x, y),
                BackColor = Color.White
            };

            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var b = new SolidBrush(accent);
                e.Graphics.FillRectangle(b, 0, 0, 5, card.Height);
                using var pen = new Pen(Color.FromArgb(220, 228, 236), 1);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);

                using var ft = new Font("Segoe UI", 8.5f);
                using var fv = new Font("Segoe UI", 22, FontStyle.Bold);
                using var fc = new Font("Segoe UI", 7.5f);
                e.Graphics.DrawString(title,  ft, new SolidBrush(Color.FromArgb(100,120,140)),  12, 14);
                e.Graphics.DrawString(value,  fv, new SolidBrush(Color.FromArgb(26, 58, 92)),   12, 38);
                e.Graphics.DrawString(change, fc, new SolidBrush(Color.FromArgb(80, 100, 120)), 12, 90);
            };
            return card;
        }

        // ── Bar Chart ────────────────────────────────────────────────────
        private void DrawBarChart(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var p = (Panel)sender;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // border
            using var pen = new Pen(Color.FromArgb(220, 228, 236), 1);
            g.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);

            // title
            using var fTitle = new Font("Segoe UI", 10, FontStyle.Bold);
            g.DrawString("Monthly Records Overview", fTitle,
                new SolidBrush(Color.FromArgb(26, 58, 92)), 16, 14);

            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun" };
            int[]    vals1  = { 42, 68, 55, 80, 63, 91 };
            int[]    vals2  = { 30, 50, 40, 60, 55, 75 };

            int chartL = 50, chartB = p.Height - 40, chartW = p.Width - 80, chartH = 150;
            int barW = chartW / months.Length;
            int maxV = 120;

            // grid lines
            for (int g2 = 0; g2 <= 4; g2++)
            {
                int gy = chartB - (int)(g2 * chartH / 4.0);
                using var gpen = new Pen(Color.FromArgb(230, 234, 240), 1);
                g.DrawLine(gpen, chartL, gy, chartL + chartW, gy);
                g.DrawString((g2 * maxV / 4).ToString(),
                    new Font("Segoe UI", 7f), new SolidBrush(Color.FromArgb(160, 170, 180)),
                    chartL - 36, gy - 8);
            }

            // bars
            Color[] barColors1 = { Color.FromArgb(37, 99, 235) };
            Color[] barColors2 = { Color.FromArgb(96, 165, 250) };

            for (int i = 0; i < months.Length; i++)
            {
                int x1 = chartL + i * barW + 8;
                int h1 = (int)((double)vals1[i] / maxV * chartH);
                int h2 = (int)((double)vals2[i] / maxV * chartH);

                using (var b1 = new SolidBrush(Color.FromArgb(37, 99, 235)))
                    g.FillRectangle(b1, x1, chartB - h1, 16, h1);
                using (var b2 = new SolidBrush(Color.FromArgb(96, 165, 250)))
                    g.FillRectangle(b2, x1 + 18, chartB - h2, 16, h2);

                g.DrawString(months[i], new Font("Segoe UI", 7.5f),
                    new SolidBrush(Color.FromArgb(100, 120, 140)), x1, chartB + 6);
            }

            // Legend
            using var fb = new SolidBrush(Color.FromArgb(37, 99, 235));
            g.FillRectangle(fb, 320, 14, 12, 12);
            g.DrawString("New Records", new Font("Segoe UI", 8f),
                new SolidBrush(Color.FromArgb(70, 85, 100)), 336, 13);
            using var fb2 = new SolidBrush(Color.FromArgb(96, 165, 250));
            g.FillRectangle(fb2, 420, 14, 12, 12);
            g.DrawString("Processed", new Font("Segoe UI", 8f),
                new SolidBrush(Color.FromArgb(70, 85, 100)), 436, 13);
        }

        // ── Activity Feed ────────────────────────────────────────────────
        private void DrawActivityFeed(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var p = (Panel)sender;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var pen = new Pen(Color.FromArgb(220, 228, 236), 1);
            g.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);

            using var fTitle = new Font("Segoe UI", 10, FontStyle.Bold);
            g.DrawString("Recent Activity", fTitle,
                new SolidBrush(Color.FromArgb(26, 58, 92)), 14, 14);

            var activities = new (string Icon, string Action, string Time, Color Dot)[]
            {
                ("👤", "User Juan dela Cruz logged in",  "2 min ago",  Color.FromArgb(22, 163, 74)),
                ("📁", "Record #4821 was updated",       "15 min ago", Color.FromArgb(37, 99, 235)),
                ("📊", "Monthly report generated",       "1 hr ago",   Color.FromArgb(217, 119, 6)),
                ("⚠",  "Failed login attempt detected",  "2 hrs ago",  Color.FromArgb(220, 38, 38)),
                ("✔",  "Backup completed successfully",  "3 hrs ago",  Color.FromArgb(22, 163, 74)),
            };

            int y = 46;
            foreach (var (icon, action, time, dot) in activities)
            {
                using (var b = new SolidBrush(dot))
                    g.FillEllipse(b, 14, y + 3, 9, 9);

                using var fAction = new Font("Segoe UI", 8.5f);
                using var fTime   = new Font("Segoe UI", 7.5f);
                g.DrawString(action, fAction, new SolidBrush(Color.FromArgb(40, 55, 70)), 30, y);
                g.DrawString(time,   fTime,   new SolidBrush(Color.FromArgb(130, 145, 160)), 30, y + 16);

                using var linePen = new Pen(Color.FromArgb(235, 239, 244), 1);
                g.DrawLine(linePen, 14, y + 38, p.Width - 14, y + 38);
                y += 42;
            }
        }

        // ── Recent Records Table ─────────────────────────────────────────
        private void DrawRecentTable(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var p = (Panel)sender;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var pen = new Pen(Color.FromArgb(220, 228, 236), 1);
            g.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);

            using var fTitle = new Font("Segoe UI", 10, FontStyle.Bold);
            g.DrawString("Recent Records", fTitle,
                new SolidBrush(Color.FromArgb(26, 58, 92)), 14, 14);

            // Table header
            string[] cols  = { "ID", "Name", "Department", "Date Added", "Status" };
            int[]    colX  = { 14, 70, 220, 370, 510 };

            using var headerBrush = new SolidBrush(Color.FromArgb(219, 234, 254));
            g.FillRectangle(headerBrush, 0, 40, p.Width, 26);

            using var fH = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            for (int c = 0; c < cols.Length; c++)
                g.DrawString(cols[c], fH,
                    new SolidBrush(Color.FromArgb(26, 58, 92)), colX[c], 50);

            // Rows
            var rows = new (string ID, string Name, string Dept, string Date, string Status, Color SC)[]
            {
                ("#4825", "Maria Santos",   "Finance",    "Jun 12, 2024", "Active",   Color.FromArgb(22,163,74)),
                ("#4826", "Jose Reyes",     "IT",         "Jun 12, 2024", "Pending",  Color.FromArgb(217,119,6)),
                ("#4827", "Ana Gonzales",   "HR",         "Jun 11, 2024", "Active",   Color.FromArgb(22,163,74)),
                ("#4828", "Pedro Mendoza",  "Operations", "Jun 11, 2024", "Inactive", Color.FromArgb(220,38,38)),
                ("#4829", "Rosa Villanueva","Admin",      "Jun 10, 2024", "Active",   Color.FromArgb(22,163,74)),
            };

            int ry = 66;
            using var fRow = new Font("Segoe UI", 8.5f);
            for (int r = 0; r < rows.Length; r++)
            {
                if (r % 2 == 1)
                {
                    using var altBrush = new SolidBrush(Color.FromArgb(249, 250, 251));
                    g.FillRectangle(altBrush, 0, ry, p.Width, 26);
                }
                var row = rows[r];
                string[] vals = { row.ID, row.Name, row.Dept, row.Date };
                for (int c = 0; c < vals.Length; c++)
                    g.DrawString(vals[c], fRow,
                        new SolidBrush(Color.FromArgb(40, 55, 70)), colX[c], ry + 7);

                // status pill
                using var statusBrush = new SolidBrush(Color.FromArgb(30, row.SC.R, row.SC.G, row.SC.B));
                g.FillRectangle(statusBrush, colX[4], ry + 5, 60, 17);
                using var fStatus = new Font("Segoe UI", 7.5f, FontStyle.Bold);
                g.DrawString(row.Status, fStatus, new SolidBrush(row.SC), colX[4] + 4, ry + 7);

                using var rowPen = new Pen(Color.FromArgb(235, 239, 244), 1);
                g.DrawLine(rowPen, 0, ry + 26, p.Width, ry + 26);
                ry += 26;
            }
        }
    }
}