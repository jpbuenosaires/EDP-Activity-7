using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace InformationSystem
{
    public class DashboardForm : Form
    {
        private Panel  pnlSidebar, pnlTopBar, pnlContent;
        private Label  lblWelcome, lblDate;
        private Button[] navButtons;
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

            // Nav items: label, Y position, click action
            var navItems = new (string Label, int Y, Action OnClick)[]
            {
                ("🏠  Dashboard", 90,  () => LoadDashboardContent()),
                ("👥  Users",     138, () => MessageBox.Show("Users module – coming soon!", "InfoSys", MessageBoxButtons.OK, MessageBoxIcon.Information)),
                ("📁  Records",   186, () => MessageBox.Show("Records module – coming soon!", "InfoSys", MessageBoxButtons.OK, MessageBoxIcon.Information)),
                ("📊  Reports",   234, () => new ReportGeneratorForm().Show()),
                ("⚙  Settings",  282, () => MessageBox.Show("Settings module – coming soon!", "InfoSys", MessageBoxButtons.OK, MessageBoxIcon.Information)),
                ("❓  Help",      330, () => new AboutForm().ShowDialog()),
            };

            navButtons = new Button[navItems.Length];
            pnlSidebar.Controls.Add(pnlLogo);

            for (int i = 0; i < navItems.Length; i++)
            {
                var (label, y, onClick) = navItems[i];
                int index = i; // capture for closure

                var btn = new Button
                {
                    Text      = "  " + label,
                    Size      = new Size(220, 42),
                    Location  = new Point(0, y),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = index == 0
                        ? Color.FromArgb(37, 99, 235)
                        : Color.Transparent,
                    ForeColor = Color.White,
                    Font      = new Font("Segoe UI", 9.5f,
                        index == 0 ? FontStyle.Bold : FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor    = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize         = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);

                btn.Click += (s, e) =>
                {
                    SetActiveNav(index);
                    onClick();
                };

                navButtons[i] = btn;
                pnlSidebar.Controls.Add(btn);
            }

            // Logout button
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

        // Highlight active sidebar button
        private void SetActiveNav(int activeIndex)
        {
            for (int i = 0; i < navButtons.Length; i++)
            {
                navButtons[i].BackColor = i == activeIndex
                    ? Color.FromArgb(37, 99, 235)
                    : Color.Transparent;
                navButtons[i].Font = new Font("Segoe UI", 9.5f,
                    i == activeIndex ? FontStyle.Bold : FontStyle.Regular);
            }
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
                Location   = new Point(220, 58),
                Size       = new Size(880, 622),
                BackColor  = Color.FromArgb(240, 244, 248),
                AutoScroll = true
            };
            Controls.Add(pnlContent);

            Resize += (s, e) =>
                pnlContent.Size = new Size(Width - 220 - 16, Height - 58 - 39);

            LoadDashboardContent();
        }

        private void LoadDashboardContent()
        {
            pnlContent.Controls.Clear();

            var kpiData = new (string Title, string Value, string Change, Color Accent)[]
            {
                ("Total Users",    "1,284", "↑ 12%  this month",   Color.FromArgb(37, 99, 235)),
                ("Active Records", "8,745", "↑ 5%   this week",    Color.FromArgb(22, 163, 74)),
                ("Reports Today",  "34",    "↓ 3%   vs yesterday", Color.FromArgb(217, 119, 6)),
                ("System Alerts",  "2",     "▼ Resolved: 10",      Color.FromArgb(220, 38, 38)),
            };

            for (int i = 0; i < kpiData.Length; i++)
            {
                var (title, value, change, accent) = kpiData[i];
                pnlContent.Controls.Add(
                    CreateKpiCard(title, value, change, accent, 20 + i * 210, 20));
            }

            var pnlChart = new Panel
            {
                Size = new Size(530, 240), Location = new Point(20, 160), BackColor = Color.White
            };
            pnlChart.Paint += DrawBarChart;

            var pnlActivity = new Panel
            {
                Size = new Size(295, 240), Location = new Point(565, 160), BackColor = Color.White
            };
            pnlActivity.Paint += DrawActivityFeed;

            var pnlTable = new Panel
            {
                Size = new Size(840, 210), Location = new Point(20, 415), BackColor = Color.White
            };
            pnlTable.Paint += DrawRecentTable;

            pnlContent.Controls.AddRange(new Control[] { pnlChart, pnlActivity, pnlTable });
        }

        // ── KPI Card ─────────────────────────────────────────────────────
        private Panel CreateKpiCard(string title, string value, string change,
            Color accent, int x, int y)
        {
            var card = new Panel { Size = new Size(195, 120), Location = new Point(x, y), BackColor = Color.White };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var b   = new SolidBrush(accent);
                using var pen = new Pen(Color.FromArgb(220, 228, 236), 1);
                e.Graphics.FillRectangle(b, 0, 0, 5, card.Height);
                e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                e.Graphics.DrawString(title,  new Font("Segoe UI", 8.5f),             new SolidBrush(Color.FromArgb(100,120,140)), 12, 14);
                e.Graphics.DrawString(value,  new Font("Segoe UI", 22, FontStyle.Bold), new SolidBrush(Color.FromArgb(26, 58, 92)),  12, 38);
                e.Graphics.DrawString(change, new Font("Segoe UI", 7.5f),             new SolidBrush(Color.FromArgb(80, 100,120)), 12, 90);
            };
            return card;
        }

        // ── Bar Chart ────────────────────────────────────────────────────
        private void DrawBarChart(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var p = (Panel)sender;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var borderPen = new Pen(Color.FromArgb(220, 228, 236), 1);
            g.DrawRectangle(borderPen, 0, 0, p.Width - 1, p.Height - 1);
            g.DrawString("Monthly Records Overview", new Font("Segoe UI", 10, FontStyle.Bold),
                new SolidBrush(Color.FromArgb(26, 58, 92)), 16, 14);

            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun" };
            int[]    vals1  = { 42, 68, 55, 80, 63, 91 };
            int[]    vals2  = { 30, 50, 40, 60, 55, 75 };
            int chartL = 50, chartB = p.Height - 40, chartW = p.Width - 80, chartH = 150, maxV = 120;
            int barW = chartW / months.Length;

            for (int g2 = 0; g2 <= 4; g2++)
            {
                int gy = chartB - (int)(g2 * chartH / 4.0);
                using var gpen = new Pen(Color.FromArgb(230, 234, 240), 1);
                g.DrawLine(gpen, chartL, gy, chartL + chartW, gy);
                g.DrawString((g2 * maxV / 4).ToString(), new Font("Segoe UI", 7f),
                    new SolidBrush(Color.FromArgb(160, 170, 180)), chartL - 36, gy - 8);
            }

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

            using var fb1 = new SolidBrush(Color.FromArgb(37, 99, 235));
            g.FillRectangle(fb1, 320, 14, 12, 12);
            g.DrawString("New Records", new Font("Segoe UI", 8f), new SolidBrush(Color.FromArgb(70,85,100)), 336, 13);
            using var fb2 = new SolidBrush(Color.FromArgb(96, 165, 250));
            g.FillRectangle(fb2, 420, 14, 12, 12);
            g.DrawString("Processed", new Font("Segoe UI", 8f), new SolidBrush(Color.FromArgb(70,85,100)), 436, 13);
        }

        // ── Activity Feed ────────────────────────────────────────────────
        private void DrawActivityFeed(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var p = (Panel)sender;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var pen = new Pen(Color.FromArgb(220, 228, 236), 1);
            g.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            g.DrawString("Recent Activity", new Font("Segoe UI", 10, FontStyle.Bold),
                new SolidBrush(Color.FromArgb(26, 58, 92)), 14, 14);

            var activities = new (string Action, string Time, Color Dot)[]
            {
                ("User Juan dela Cruz logged in", "2 min ago",  Color.FromArgb(22, 163, 74)),
                ("Record #4821 was updated",      "15 min ago", Color.FromArgb(37, 99,  235)),
                ("Monthly report generated",      "1 hr ago",   Color.FromArgb(217,119,  6)),
                ("Failed login attempt detected", "2 hrs ago",  Color.FromArgb(220, 38,  38)),
                ("Backup completed successfully", "3 hrs ago",  Color.FromArgb(22, 163, 74)),
            };

            int y = 46;
            foreach (var (action, time, dot) in activities)
            {
                using (var b = new SolidBrush(dot))
                    g.FillEllipse(b, 14, y + 3, 9, 9);
                g.DrawString(action, new Font("Segoe UI", 8.5f), new SolidBrush(Color.FromArgb(40,55,70)),   30, y);
                g.DrawString(time,   new Font("Segoe UI", 7.5f), new SolidBrush(Color.FromArgb(130,145,160)), 30, y + 16);
                using var divPen = new Pen(Color.FromArgb(235, 239, 244), 1);
                g.DrawLine(divPen, 14, y + 38, p.Width - 14, y + 38);
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
            g.DrawString("Recent Records", new Font("Segoe UI", 10, FontStyle.Bold),
                new SolidBrush(Color.FromArgb(26, 58, 92)), 14, 14);

            string[] cols = { "ID", "Name", "Department", "Date Added", "Status" };
            int[]    colX = { 14, 70, 220, 370, 510 };

            using var headerBrush = new SolidBrush(Color.FromArgb(219, 234, 254));
            g.FillRectangle(headerBrush, 0, 40, p.Width, 26);
            for (int c = 0; c < cols.Length; c++)
                g.DrawString(cols[c], new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    new SolidBrush(Color.FromArgb(26, 58, 92)), colX[c], 50);

            var rows = new (string ID, string Name, string Dept, string Date, string Status, Color SC)[]
            {
                ("#4825", "Maria Santos",    "Finance",    "Jun 12, 2024", "Active",   Color.FromArgb(22, 163, 74)),
                ("#4826", "Jose Reyes",      "IT",         "Jun 12, 2024", "Pending",  Color.FromArgb(217,119,  6)),
                ("#4827", "Ana Gonzales",    "HR",         "Jun 11, 2024", "Active",   Color.FromArgb(22, 163, 74)),
                ("#4828", "Pedro Mendoza",   "Operations", "Jun 11, 2024", "Inactive", Color.FromArgb(220, 38, 38)),
                ("#4829", "Rosa Villanueva", "Admin",      "Jun 10, 2024", "Active",   Color.FromArgb(22, 163, 74)),
            };

            int ry = 66;
            for (int r = 0; r < rows.Length; r++)
            {
                if (r % 2 == 1)
                {
                    using var alt = new SolidBrush(Color.FromArgb(249, 250, 251));
                    g.FillRectangle(alt, 0, ry, p.Width, 26);
                }
                string[] vals = { rows[r].ID, rows[r].Name, rows[r].Dept, rows[r].Date };
                for (int c = 0; c < vals.Length; c++)
                    g.DrawString(vals[c], new Font("Segoe UI", 8.5f),
                        new SolidBrush(Color.FromArgb(40, 55, 70)), colX[c], ry + 7);

                using var sb = new SolidBrush(Color.FromArgb(30, rows[r].SC.R, rows[r].SC.G, rows[r].SC.B));
                g.FillRectangle(sb, colX[4], ry + 5, 60, 17);
                g.DrawString(rows[r].Status, new Font("Segoe UI", 7.5f, FontStyle.Bold),
                    new SolidBrush(rows[r].SC), colX[4] + 4, ry + 7);

                using var rowPen = new Pen(Color.FromArgb(235, 239, 244), 1);
                g.DrawLine(rowPen, 0, ry + 26, p.Width, ry + 26);
                ry += 26;
            }
        }
    }
}