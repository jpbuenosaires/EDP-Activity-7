using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EcommerceIS
{
    public class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text            = "E-Commerce IS  —  About";
            this.Size            = new Size(580, 560);
            this.StartPosition  = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox     = true;
this.WindowState     = FormWindowState.Maximized;
            this.BackColor       = Color.White;
            this.Font            = new Font("Segoe UI", 9f);

            // ── Top banner ────────────────────────────────────
            var pnlBanner = new Panel
            {
                Location  = new Point(0, 0),
                Size      = new Size(580, 180),
                BackColor = Color.Transparent
            };
            pnlBanner.Paint += (s, e) =>
            {
                var g  = e.Graphics;
                var rc = ((Panel)s).ClientRectangle;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Gradient background
                using (var br = new LinearGradientBrush(rc,
                    Color.FromArgb(16, 34, 80),
                    Color.FromArgb(46, 90, 170),
                    LinearGradientMode.ForwardDiagonal))
                    g.FillRectangle(br, rc);

                // Decorative circles
                using (var br = new SolidBrush(Color.FromArgb(15, 255, 255, 255)))
                {
                    g.FillEllipse(br, -60,  -60, 200, 200);
                    g.FillEllipse(br,  380,  80, 220, 220);
                    g.FillEllipse(br,  180, -40, 120, 120);
                }

                // Shield icon background
                using (var br = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
                    g.FillEllipse(br, 246, 20, 88, 88);
                using (var pen = new Pen(Color.FromArgb(80, 255, 255, 255), 2))
                    g.DrawEllipse(pen, 246, 20, 88, 88);

                // App icon text
                using (var f = new Font("Segoe UI Emoji", 30f))
                using (var br = new SolidBrush(Color.White))
                    g.DrawString("🛒", f, br, 262, 30);

                // App name
                using (var f = new Font("Segoe UI", 16f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.White))
                {
                    var sz = g.MeasureString("EcommerceDB System", f);
                    g.DrawString("EcommerceDB System", f, br,
                        (rc.Width - sz.Width) / 2, 120);
                }

                // Version
                using (var f = new Font("Segoe UI", 9f))
                using (var br = new SolidBrush(Color.FromArgb(200, 220, 255)))
                {
                    var sz = g.MeasureString("Version 2.0.0  ·  2025", f);
                    g.DrawString("Version 2.0.0  ·  2025", f, br,
                        (rc.Width - sz.Width) / 2, 150);
                }
            };
            this.Controls.Add(pnlBanner);

            // ── Info rows ─────────────────────────────────────
            int y = 196;
            AddInfoRow("👤  Developed by",  "Jayson Buenosaires",                 ref y);
            AddInfoRow("🏫  Institution",   "Bicol University — College of Science", ref y);
            AddInfoRow("📚  Program",       "BS Information Technology",          ref y);
            AddInfoRow("📘  Course",        "IT 120 — Event Driven Programming",  ref y);
            AddInfoRow("📅  School Year",   "2025–2026  |  Second Semester",      ref y);
            AddInfoRow("🗄️  Database",      "MySQL 8.0  ·  EcommerceDB",          ref y);
            AddInfoRow("💻  Platform",      "C#  ·  .NET Framework  ·  WinForms", ref y);

            // ── Separator ─────────────────────────────────────
            var sep = new Panel
            {
                Location  = new Point(20, y + 8),
                Size      = new Size(540, 1),
                BackColor = Color.FromArgb(220, 225, 240)
            };
            this.Controls.Add(sep);
            y += 20;

            // ── Description ───────────────────────────────────
            var lblDesc = new Label
            {
                Text =
                    "This system implements a fully normalized E-Commerce database (3NF) with " +
                    "triggers, stored procedures, views, and a Windows Forms front-end built in C#. " +
                    "It demonstrates event-driven programming concepts in database automation.",
                Location  = new Point(30, y + 10),
                Size      = new Size(520, 60),
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(80, 90, 110)
            };
            this.Controls.Add(lblDesc);

            // ── Close button ──────────────────────────────────
            var btnClose = new Button
            {
                Text      = "Close",
                Location  = new Point(220, y + 82),
                Size      = new Size(140, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 77, 140),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            // ── Status bar ────────────────────────────────────
            var pnlStatus = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 28,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            pnlStatus.Paint += (s, e) =>
            {
                using (var p = new Pen(Color.FromArgb(215, 220, 230)))
                    e.Graphics.DrawLine(p, 0, 0, 580, 0);
                using (var f = new Font("Segoe UI", 7.5f))
                using (var br = new SolidBrush(Color.DimGray))
                    e.Graphics.DrawString(
                        "  EcommerceDB System  ·  All Rights Reserved  ·  2025",
                        f, br, 4, 7);
            };
            this.Controls.Add(pnlStatus);
        }

        private void AddInfoRow(string label, string value, ref int y)
        {
            // Row background (alternating)
            bool isAlt = (y % 2 == 0);
            var pnlRow = new Panel
            {
                Location  = new Point(0, y),
                Size      = new Size(580, 30),
                BackColor = isAlt
                    ? Color.FromArgb(248, 250, 255)
                    : Color.White
            };

            var lblKey = new Label
            {
                Text      = label,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 77, 140),
                Location  = new Point(30, 7),
                Size      = new Size(200, 20)
            };

            var lblVal = new Label
            {
                Text      = value,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(50, 60, 80),
                Location  = new Point(230, 7),
                Size      = new Size(330, 20)
            };

            pnlRow.Controls.AddRange(new Control[] { lblKey, lblVal });
            this.Controls.Add(pnlRow);
            y += 30;
        }
    }
}