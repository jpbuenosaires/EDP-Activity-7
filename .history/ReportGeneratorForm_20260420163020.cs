using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace InformationSystem
{
    public class ReportGeneratorForm : Form
    {
        private Panel     pnlLeft, pnlRight, pnlTopBar;
        private ComboBox  cboReportType, cboDepartment, cboFormat;
        private DateTimePicker dtpFrom, dtpTo;
        private CheckBox  chkIncludeChart, chkIncludeSummary, chkIncludeDetails;
        private Button    btnGenerate, btnPreview, btnExport, btnClose;
        private Label     lblStatus;
        private ProgressBar progressBar;
        private ListBox   lstRecentReports;

        public ReportGeneratorForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text            = "InfoSys – Report Generator";
            Size            = new Size(950, 640);
            StartPosition   = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize     = new Size(800, 560);
            BackColor       = Color.FromArgb(240, 244, 248);
            Font            = new Font("Segoe UI", 9.5f);

            BuildTopBar();
            BuildLeftPanel();
            BuildRightPanel();
        }

        // ── TOP BAR ─────────────────────────────────────────────────────
        private void BuildTopBar()
        {
            pnlTopBar = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 52,
                BackColor = Color.FromArgb(26, 58, 92)
            };
            pnlTopBar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var f = new Font("Segoe UI", 13, FontStyle.Bold);
                e.Graphics.DrawString("📊  Report Generator", f, Brushes.White, 20, 14);
            };

            btnClose = new Button
            {
                Text      = "✕  Close",
                Size      = new Size(90, 32),
                Location  = new Point(840, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 9f),
                Cursor    = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();

            pnlTopBar.Controls.Add(btnClose);
            Controls.Add(pnlTopBar);
        }

        // ── LEFT PANEL – Report Options ──────────────────────────────────
        private void BuildLeftPanel()
        {
            pnlLeft = new Panel
            {
                Location  = new Point(0, 52),
                Size      = new Size(490, 588),
                BackColor = Color.White
            };
            pnlLeft.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(220, 228, 236), 1);
                e.Graphics.DrawLine(pen, pnlLeft.Width - 1, 0,
                    pnlLeft.Width - 1, pnlLeft.Height);
            };

            int y = 20;

            // ── Section: Report Type ─────────────────────────────────────
            pnlLeft.Controls.Add(SectionHeader("Report Configuration", 20, y)); y += 34;

            pnlLeft.Controls.Add(FieldLabel("Report Type", 20, y)); y += 22;
            cboReportType = new ComboBox
            {
                Size          = new Size(440, 28),
                Location      = new Point(20, y),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = Color.FromArgb(248, 250, 252)
            };
            cboReportType.Items.AddRange(new object[]
            {
                "User Activity Report",
                "Department Summary Report",
                "Monthly Records Report",
                "Audit Trail Report",
                "Financial Overview Report",
                "System Performance Report"
            });
            cboReportType.SelectedIndex = 0;
            pnlLeft.Controls.Add(cboReportType); y += 40;

            pnlLeft.Controls.Add(FieldLabel("Department / Scope", 20, y)); y += 22;
            cboDepartment = new ComboBox
            {
                Size          = new Size(440, 28),
                Location      = new Point(20, y),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = Color.FromArgb(248, 250, 252)
            };
            cboDepartment.Items.AddRange(new object[]
                { "All Departments", "Finance", "Human Resources", "IT", "Operations", "Admin" });
            cboDepartment.SelectedIndex = 0;
            pnlLeft.Controls.Add(cboDepartment); y += 48;

            // ── Section: Date Range ──────────────────────────────────────
            pnlLeft.Controls.Add(Separator(20, y)); y += 14;
            pnlLeft.Controls.Add(SectionHeader("Date Range", 20, y)); y += 34;

            pnlLeft.Controls.Add(FieldLabel("From", 20, y));
            pnlLeft.Controls.Add(FieldLabel("To", 240, y)); y += 22;

            dtpFrom = new DateTimePicker
            {
                Size     = new Size(200, 28),
                Location = new Point(20, y),
                Format   = DateTimePickerFormat.Short,
                Value    = DateTime.Now.AddMonths(-1)
            };
            dtpTo = new DateTimePicker
            {
                Size     = new Size(200, 28),
                Location = new Point(240, y),
                Format   = DateTimePickerFormat.Short,
                Value    = DateTime.Now
            };
            pnlLeft.Controls.AddRange(new Control[] { dtpFrom, dtpTo }); y += 48;

            // ── Section: Output Format ───────────────────────────────────
            pnlLeft.Controls.Add(Separator(20, y)); y += 14;
            pnlLeft.Controls.Add(SectionHeader("Output Format", 20, y)); y += 34;

            pnlLeft.Controls.Add(FieldLabel("Export As", 20, y)); y += 22;
            cboFormat = new ComboBox
            {
                Size          = new Size(440, 28),
                Location      = new Point(20, y),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = Color.FromArgb(248, 250, 252)
            };
            cboFormat.Items.AddRange(new object[]
                { "📄  PDF Document", "📊  Excel Spreadsheet (.xlsx)",
                  "📝  Word Document (.docx)", "📋  CSV (Comma-Separated)" });
            cboFormat.SelectedIndex = 0;
            pnlLeft.Controls.Add(cboFormat); y += 48;

            // ── Section: Include in Report ───────────────────────────────
            pnlLeft.Controls.Add(Separator(20, y)); y += 14;
            pnlLeft.Controls.Add(SectionHeader("Include in Report", 20, y)); y += 34;

            chkIncludeChart   = CreateCheckbox("📊  Charts and Graphs", 20, y);
            chkIncludeSummary = CreateCheckbox("📋  Summary Statistics", 230, y); y += 32;
            chkIncludeDetails = CreateCheckbox("📁  Detailed Records", 20, y); y += 40;

            chkIncludeChart.Checked   = true;
            chkIncludeSummary.Checked = true;

            pnlLeft.Controls.AddRange(new Control[]
                { chkIncludeChart, chkIncludeSummary, chkIncludeDetails });

            // ── Action buttons ───────────────────────────────────────────
            pnlLeft.Controls.Add(Separator(20, y)); y += 16;

            btnGenerate = CreateActionBtn("⚙  Generate Report",
                Color.FromArgb(26, 115, 232), Color.White, 20, y, 200);
            btnPreview  = CreateActionBtn("👁  Preview",
                Color.FromArgb(240, 244, 248), Color.FromArgb(26, 58, 92), 230, y, 110);
            btnExport   = CreateActionBtn("⬇  Export",
                Color.FromArgb(22, 163, 74), Color.White, 350, y, 110);

            btnGenerate.Click += BtnGenerate_Click;
            btnExport.Click   += BtnExport_Click;

            pnlLeft.Controls.AddRange(new Control[] { btnGenerate, btnPreview, btnExport });
            y += 54;

            // ── Progress bar + status ────────────────────────────────────
            progressBar = new ProgressBar
            {
                Size     = new Size(440, 14),
                Location = new Point(20, y),
                Style    = ProgressBarStyle.Continuous,
                Visible  = false
            };
            lblStatus = new Label
            {
                Text      = "",
                AutoSize  = true,
                ForeColor = Color.FromArgb(22, 163, 74),
                Location  = new Point(20, y + 20),
                Visible   = false
            };
            pnlLeft.Controls.AddRange(new Control[] { progressBar, lblStatus });

            Controls.Add(pnlLeft);
        }

        // ── RIGHT PANEL – Recent Reports ─────────────────────────────────
        private void BuildRightPanel()
        {
            pnlRight = new Panel
            {
                Location  = new Point(490, 52),
                Size      = new Size(460, 588),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            pnlRight.Controls.Add(SectionHeader("Recent Reports", 16, 20));

            // filter row
            var txtSearch = new TextBox
            {
                Size            = new Size(280, 28),
                Location        = new Point(16, 54),
                BorderStyle     = BorderStyle.FixedSingle,
                PlaceholderText = "🔍  Search reports…",
                Font            = new Font("Segoe UI", 9f),
                BackColor       = Color.White
            };
            var cboFilter = new ComboBox
            {
                Size          = new Size(130, 28),
                Location      = new Point(306, 54),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = Color.White
            };
            cboFilter.Items.AddRange(new object[] { "All", "PDF", "Excel", "Word", "CSV" });
            cboFilter.SelectedIndex = 0;

            pnlRight.Controls.AddRange(new Control[] { txtSearch, cboFilter });

            // list
            lstRecentReports = new ListBox
            {
                Size          = new Size(428, 320),
                Location      = new Point(16, 90),
                BorderStyle   = BorderStyle.None,
                BackColor     = Color.FromArgb(248, 250, 252),
                Font          = new Font("Segoe UI", 9f),
                DrawMode      = DrawMode.OwnerDrawFixed,
                ItemHeight    = 58
            };
            lstRecentReports.DrawItem += DrawReportListItem;

            string[] recentReports =
            {
                "User Activity Report|June 2024|PDF|Jun 12, 2024  09:14 AM",
                "Monthly Records Report|All Departments|Excel|Jun 11, 2024  03:30 PM",
                "Audit Trail Report|IT Department|PDF|Jun 10, 2024  11:00 AM",
                "Financial Overview|Finance|Word|Jun 09, 2024  02:15 PM",
                "Department Summary|HR|CSV|Jun 08, 2024  08:45 AM",
                "System Performance|All|PDF|Jun 07, 2024  04:00 PM",
            };
            lstRecentReports.Items.AddRange(recentReports);
            pnlRight.Controls.Add(lstRecentReports);

            // Quick Stats
            var pnlStats = new Panel
            {
                Size      = new Size(428, 120),
                Location  = new Point(16, 425),
                BackColor = Color.White
            };
            pnlStats.Paint += DrawQuickStats;
            pnlRight.Controls.Add(pnlStats);

            Controls.Add(pnlRight);

            Resize += (s, e) =>
            {
                pnlLeft.Height  = Height - 52 - 39;
                pnlRight.Location = new Point(490, 52);
                pnlRight.Size = new Size(Width - 490 - 16, Height - 52 - 39);
                lstRecentReports.Width = pnlRight.Width - 32;
                pnlStats.Width = pnlRight.Width - 32;
                btnClose.Location = new Point(Width - 110, 10);
            };
        }

        // ── Drawing helpers ──────────────────────────────────────────────
        private void DrawReportListItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            var parts = lstRecentReports.Items[e.Index].ToString().Split('|');

            bool selected = (e.State & DrawItemState.Selected) != 0;
            using var bg = new SolidBrush(selected
                ? Color.FromArgb(219, 234, 254)
                : (e.Index % 2 == 0 ? Color.White : Color.FromArgb(248, 250, 252)));
            e.Graphics.FillRectangle(bg, e.Bounds);

            using var fName = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            using var fSub  = new Font("Segoe UI", 8.5f);
            using var fTime = new Font("Segoe UI", 8f);

            Color typeColor = parts[2] == "PDF"   ? Color.FromArgb(220, 38, 38) :
                              parts[2] == "Excel" ? Color.FromArgb(22, 163, 74) :
                              parts[2] == "Word"  ? Color.FromArgb(37, 99, 235) :
                                                    Color.FromArgb(217, 119, 6);

            e.Graphics.DrawString(parts[0], fName,
                new SolidBrush(Color.FromArgb(26, 58, 92)),
                new RectangleF(e.Bounds.X + 10, e.Bounds.Y + 7, 280, 20));
            e.Graphics.DrawString(parts[1], fSub,
                new SolidBrush(Color.FromArgb(100, 120, 140)),
                new RectangleF(e.Bounds.X + 10, e.Bounds.Y + 26, 280, 18));
            e.Graphics.DrawString("🕐 " + parts[3], fTime,
                new SolidBrush(Color.FromArgb(130, 145, 160)),
                new RectangleF(e.Bounds.X + 10, e.Bounds.Y + 42, 280, 16));

            // type badge
            using var badgeBrush = new SolidBrush(Color.FromArgb(30, typeColor.R, typeColor.G, typeColor.B));
            e.Graphics.FillRectangle(badgeBrush, e.Bounds.Right - 56, e.Bounds.Y + 16, 44, 18);
            using var fBadge = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            e.Graphics.DrawString(parts[2], fBadge, new SolidBrush(typeColor),
                e.Bounds.Right - 52, e.Bounds.Y + 18);

            using var divPen = new Pen(Color.FromArgb(230, 235, 242), 1);
            e.Graphics.DrawLine(divPen, e.Bounds.X, e.Bounds.Bottom - 1,
                e.Bounds.Right, e.Bounds.Bottom - 1);
        }

        private void DrawQuickStats(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var p = (Panel)sender;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var pen = new Pen(Color.FromArgb(220, 228, 236), 1);
            g.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);

            using var fTitle = new Font("Segoe UI", 9f, FontStyle.Bold);
            g.DrawString("Quick Stats – This Month", fTitle,
                new SolidBrush(Color.FromArgb(26, 58, 92)), 14, 12);

            var stats = new (string Label, string Value, Color C)[]
            {
                ("Reports Generated", "34",  Color.FromArgb(37,  99, 235)),
                ("Exports",           "22",  Color.FromArgb(22, 163,  74)),
                ("Scheduled",         "8",   Color.FromArgb(217,119,   6)),
                ("Failed",            "2",   Color.FromArgb(220,  38,  38)),
            };

            int sx = 14;
            foreach (var (label, val, color) in stats)
            {
                using var bv = new SolidBrush(color);
                using var fv = new Font("Segoe UI", 16, FontStyle.Bold);
                using var fl = new Font("Segoe UI", 7.5f);
                g.DrawString(val,   fv, bv,                                       sx, 36);
                g.DrawString(label, fl, new SolidBrush(Color.FromArgb(100,120,140)), sx, 66);
                sx += 95;
            }
        }

        // ── Button logic ─────────────────────────────────────────────────
        private async void BtnGenerate_Click(object sender, EventArgs e)
        {
            progressBar.Visible = true;
            progressBar.Value   = 0;
            lblStatus.Visible   = false;
            btnGenerate.Enabled = false;

            for (int i = 0; i <= 100; i += 5)
            {
                progressBar.Value = i;
                await System.Threading.Tasks.Task.Delay(60);
            }

            progressBar.Visible  = false;
            lblStatus.ForeColor  = Color.FromArgb(22, 163, 74);
            lblStatus.Text       = "✔  Report generated successfully!";
            lblStatus.Visible    = true;
            btnGenerate.Enabled  = true;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Title      = "Export Report",
                Filter     = "PDF Files|*.pdf|Excel Files|*.xlsx|Word Files|*.docx|CSV Files|*.csv",
                FileName   = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // TODO: write actual report to dlg.FileName
                MessageBox.Show($"Report exported to:\n{dlg.FileName}",
                    "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ── Factory helpers ──────────────────────────────────────────────
        private Label SectionHeader(string text, int x, int y) => new Label
        {
            Text      = text,
            Font      = new Font("Segoe UI", 10.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(26, 58, 92),
            AutoSize  = true,
            Location  = new Point(x, y)
        };

        private Label FieldLabel(string text, int x, int y) => new Label
        {
            Text      = text,
            ForeColor = Color.FromArgb(55, 65, 81),
            AutoSize  = true,
            Location  = new Point(x, y)
        };

        private Panel Separator(int x, int y) => new Panel
        {
            BackColor = Color.FromArgb(220, 228, 236),
            Size      = new Size(450, 1),
            Location  = new Point(x, y)
        };

        private CheckBox CreateCheckbox(string text, int x, int y) => new CheckBox
        {
            Text      = text,
            ForeColor = Color.FromArgb(40, 55, 70),
            AutoSize  = true,
            Location  = new Point(x, y)
        };

        private Button CreateActionBtn(string text, Color back, Color fore,
            int x, int y, int w)
        {
            var btn = new Button
            {
                Text      = text,
                Size      = new Size(w, 40),
                Location  = new Point(x, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize =
                back == Color.FromArgb(240, 244, 248) ? 1 : 0;
            if (back == Color.FromArgb(240, 244, 248))
                btn.FlatAppearance.BorderColor = Color.FromArgb(200, 210, 220);
            return btn;
        }
    }
}