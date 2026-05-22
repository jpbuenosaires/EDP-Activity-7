using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace InformationSystem
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text            = "About the Program";
            Size            = new Size(520, 600);
            StartPosition   = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            BackColor       = Color.White;
            Font            = new Font("Segoe UI", 9.5f);

            // ── Header banner ───────────────────────────────────────────
            var pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 160,
                BackColor = Color.FromArgb(26, 58, 92)
            };
            pnlHeader.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // subtle gradient circle decoration
                using var b1 = new SolidBrush(Color.FromArgb(20, 255, 255, 255));
                g.FillEllipse(b1, -40, -40, 200, 200);
                g.FillEllipse(b1, 300, 60, 180, 180);

                // logo circle
                using var logoBrush = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
                g.FillEllipse(logoBrush, 210, 30, 100, 100);
                using var logoPen = new Pen(Color.White, 2f);
                g.DrawEllipse(logoPen, 210, 30, 100, 100);

                using var fLogo = new Font("Segoe UI", 26, FontStyle.Bold);
                g.DrawString("IS", fLogo, Brushes.White, new PointF(228, 55));
            };

            var lblProductName = new Label
            {
                Text      = "InfoSys",
                Font      = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = true,
                Location  = new Point(30, 50)
            };
            var lblVersion = new Label
            {
                Text      = "Version 1.0.0  •  Build 2024.06",
                ForeColor = Color.FromArgb(180, 210, 240),
                AutoSize  = true,
                Location  = new Point(30, 90)
            };
            var lblEdition = new Label
            {
                Text      = "Integrated Information Management System",
                ForeColor = Color.FromArgb(200, 220, 240),
                Font      = new Font("Segoe UI", 8.5f),
                AutoSize  = true,
                Location  = new Point(30, 115)
            };
            pnlHeader.Controls.AddRange(new Control[] { lblProductName, lblVersion, lblEdition });

            // ── Body ─────────────────────────────────────────────────────
            var pnlBody = new Panel
            {
                Location  = new Point(0, 160),
                Size      = new Size(520, 380),
                BackColor = Color.White
            };

            // Description
            var lblDescTitle = SectionLabel("Description", 30, 20);
            var lblDesc = BodyLabel(
                "InfoSys is a comprehensive information management system designed to " +
                "streamline data handling, reporting, and administrative tasks for " +
                "organizations of all sizes. It provides a centralized platform for " +
                "managing records, generating reports, and monitoring system activity.",
                30, 44, 460, 70);

            // Divider line
            var sep1 = Separator(30, 124);

            // System info grid
            var lblInfoTitle = SectionLabel("System Information", 30, 135);

            string[,] info =
            {
                { "Developer",    "Your Name / Organization" },
                { "Framework",    ".NET 6.0  |  Windows Forms" },
                { "Database",     "SQL Server 2019" },
                { "License",      "Proprietary – All rights reserved" },
                { "Last Updated", "June 2024" }
            };

            int row = 158;
            for (int i = 0; i < info.GetLength(0); i++)
            {
                var lKey = new Label
                {
                    Text      = info[i, 0],
                    ForeColor = Color.FromArgb(100, 120, 140),
                    Font      = new Font("Segoe UI", 9f),
                    Size      = new Size(120, 22),
                    Location  = new Point(30, row)
                };
                var lVal = new Label
                {
                    Text      = info[i, 1],
                    ForeColor = Color.FromArgb(30, 40, 55),
                    Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                    AutoSize  = true,
                    Location  = new Point(155, row)
                };
                pnlBody.Controls.AddRange(new Control[] { lKey, lVal });
                row += 26;
            }

            var sep2 = Separator(30, row + 10);

            // Credits
            var lblCredTitle = SectionLabel("Acknowledgments", 30, row + 22);
            var lblCred = BodyLabel(
                "Special thanks to the development team, system administrators, " +
                "and all end-users who contributed feedback during testing.",
                30, row + 46, 460, 42);

            // Close button
            var btnClose = new Button
            {
                Text      = "Close",
                Size      = new Size(120, 38),
                Location  = new Point(190, row + 100),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(26, 58, 92),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();

            pnlBody.Controls.AddRange(new Control[]
            {
                lblDescTitle, lblDesc, sep1,
                lblInfoTitle, sep2,
                lblCredTitle, lblCred, btnClose
            });

            Controls.AddRange(new Control[] { pnlHeader, pnlBody });
        }

        // ── Helpers ─────────────────────────────────────────────────────
        private Label SectionLabel(string text, int x, int y) => new Label
        {
            Text      = text,
            Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(26, 58, 92),
            AutoSize  = true,
            Location  = new Point(x, y)
        };

        private Label BodyLabel(string text, int x, int y, int w, int h) => new Label
        {
            Text      = text,
            ForeColor = Color.FromArgb(70, 80, 95),
            Font      = new Font("Segoe UI", 9f),
            Size      = new Size(w, h),
            Location  = new Point(x, y)
        };

        private Panel Separator(int x, int y) => new Panel
        {
            BackColor = Color.FromArgb(220, 228, 236),
            Size      = new Size(460, 1),
            Location  = new Point(x, y)
        };
    }
}