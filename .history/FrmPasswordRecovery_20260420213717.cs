using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EcommerceIS
{
    public class FrmPasswordRecovery : Form
    {
        private Panel     pnlHeader;
        private Label     lblTitle;
        private Label     lblHeaderSub;
        private Label     lblStep1;
        private Label     lblStep2;
        private Label     lblStep3;
        private Panel     pnlStep1;
        private Panel     pnlStep2;
        private Panel     pnlStep3;
        private int       currentStep = 1;

        // Step 1 controls
        private Label     lblEmailInfo;
        private Label     lblEmail;
        private TextBox   txtEmail;
        private Button    btnSendCode;

        // Step 2 controls
        private Label     lblCodeInfo;
        private Label     lblCode;
        private TextBox[] txtCode = new TextBox[6];
        private Button    btnVerify;
        private LinkLabel lnkResend;

        // Step 3 controls
        private Label     lblNewPassInfo;
        private Label     lblNewPass;
        private TextBox   txtNewPass;
        private Label     lblConfirmPass;
        private TextBox   txtConfirmPass;
        private Button    btnReset;

        // Navigation
        private Button    btnBack;
        private Panel     pnlStepBar;

        public FrmPasswordRecovery()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text            = "E-Commerce IS  —  Password Recovery";
            this.Size            = new Size(520, 520);
            this.StartPosition  = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox     = true;
this.WindowState     = FormWindowState.Maximized;
            this.BackColor       = Color.White;
            this.Font            = new Font("Segoe UI", 9f);

            // ── Header ────────────────────────────────────────
            pnlHeader = new Panel
            {
                Dock   = DockStyle.Top,
                Height = 110,
                BackColor = Color.Transparent
            };
            pnlHeader.Paint += (s, e) =>
            {
                var rc = ((Panel)s).ClientRectangle;
                using (var br = new LinearGradientBrush(rc,
                    Color.FromArgb(22, 57, 120),
                    Color.FromArgb(46, 109, 180),
                    LinearGradientMode.Horizontal))
                    e.Graphics.FillRectangle(br, rc);
            };

            lblTitle = new Label
            {
                Text      = "🔑  Password Recovery",
                Font      = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                Location  = new Point(30, 26),
                AutoSize  = true,
                BackColor = Color.Transparent
            };

            lblHeaderSub = new Label
            {
                Text      = "Follow the steps below to reset your password.",
                Font      = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(200, 220, 255),
                Location  = new Point(30, 60),
                AutoSize  = true,
                BackColor = Color.Transparent
            };

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblHeaderSub });

            // ── Step indicator bar ────────────────────────────
            pnlStepBar = new Panel
            {
                Location  = new Point(0, 110),
                Size      = new Size(520, 60),
                BackColor = Color.FromArgb(245, 248, 255)
            };
            pnlStepBar.Paint += PnlStepBar_Paint;

            // ── Step panels ───────────────────────────────────
            BuildStep1Panel();
            BuildStep2Panel();
            BuildStep3Panel();

            // ── Back button ───────────────────────────────────
            btnBack = new Button
            {
                Text      = "← Back to Login",
                Location  = new Point(30, 450),
                Size      = new Size(140, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(30, 77, 140),
                BackColor = Color.White,
                Font      = new Font("Segoe UI", 8.5f),
                Cursor    = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(200, 210, 230);
            btnBack.Click += (s, e) => this.Close();

            // ── Status bar ────────────────────────────────────
            var pnlStatus = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 28,
                BackColor = Color.FromArgb(240, 240, 240)
            };
            var lblStatusTxt = new Label
            {
                Text     = "EcommerceDB System  ·  Password Recovery  ·  Secured",
                Font     = new Font("Segoe UI", 7.5f),
                ForeColor = Color.DimGray,
                Location = new Point(10, 7),
                AutoSize = true
            };
            pnlStatus.Controls.Add(lblStatusTxt);

            this.Controls.AddRange(new Control[]
            {
                pnlHeader, pnlStepBar,
                pnlStep1, pnlStep2, pnlStep3,
                btnBack, pnlStatus
            });

            ShowStep(1);
        }

        // ── Step bar painter ──────────────────────────────────
        private void PnlStepBar_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            string[] steps = { "1  Verify Email", "2  Enter Code", "3  New Password" };
            int[] xPos     = { 60, 200, 350 };

            for (int i = 0; i < 3; i++)
            {
                bool active   = (i + 1) == currentStep;
                bool done     = (i + 1) < currentStep;

                // Connector line
                if (i < 2)
                {
                    using (var p = new Pen(done
                        ? Color.FromArgb(30, 77, 140)
                        : Color.FromArgb(200, 210, 225), 2))
                    {
                        g.DrawLine(p, xPos[i] + 20, 20, xPos[i + 1] - 20, 20);
                    }
                }

                // Circle
                var circleRect = new Rectangle(xPos[i] - 14, 6, 28, 28);
                if (active)
                {
                    using (var br = new SolidBrush(Color.FromArgb(30, 77, 140)))
                        g.FillEllipse(br, circleRect);
                }
                else if (done)
                {
                    using (var br = new SolidBrush(Color.FromArgb(40, 167, 69)))
                        g.FillEllipse(br, circleRect);
                }
                else
                {
                    using (var br = new SolidBrush(Color.FromArgb(210, 215, 225)))
                        g.FillEllipse(br, circleRect);
                }

                string numText = done ? "✓" : (i + 1).ToString();
                using (var f = new Font("Segoe UI", 9f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.White))
                {
                    var sz = g.MeasureString(numText, f);
                    g.DrawString(numText, f, br,
                        xPos[i] - sz.Width / 2,
                        circleRect.Top + (circleRect.Height - sz.Height) / 2);
                }

                // Label below
                string[] labels = { "Verify Email", "Enter Code", "New Password" };
                using (var f2 = new Font("Segoe UI", 7.5f, active ? FontStyle.Bold : FontStyle.Regular))
                using (var br2 = new SolidBrush(active
                    ? Color.FromArgb(30, 77, 140)
                    : Color.FromArgb(130, 140, 160)))
                {
                    var sz = g.MeasureString(labels[i], f2);
                    g.DrawString(labels[i], f2, br2, xPos[i] - sz.Width / 2, 37);
                }
            }
        }

        // ── Step 1 – Enter email ──────────────────────────────
        private void BuildStep1Panel()
        {
            pnlStep1 = new Panel
            {
                Location  = new Point(0, 170),
                Size      = new Size(520, 270),
                BackColor = Color.White
            };

            var lblInfo = MakeInfoBox(
                "Enter the email address linked to your account.\nWe will send a verification code to that address.",
                new Point(30, 10));

            var lblEmail = MakeFieldLabel("EMAIL ADDRESS", new Point(30, 80));
            var txtEmail = MakeTextBox(new Point(30, 100), "john.doe@example.com");
            txtEmail.Size = new Size(450, 30);

            var btnSend = new Button
            {
                Text      = "SEND VERIFICATION CODE  →",
                Location  = new Point(30, 155),
                Size      = new Size(450, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 77, 140),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += (s, e) => { currentStep = 2; ShowStep(2); };
            btnSend.MouseEnter += (s, e) => btnSend.BackColor = Color.FromArgb(46, 109, 180);
            btnSend.MouseLeave += (s, e) => btnSend.BackColor = Color.FromArgb(30, 77, 140);

            pnlStep1.Controls.AddRange(new Control[] { lblInfo, lblEmail, txtEmail, btnSend });
        }

        // ── Step 2 – Enter OTP code ───────────────────────────
        private void BuildStep2Panel()
        {
            pnlStep2 = new Panel
            {
                Location  = new Point(0, 170),
                Size      = new Size(520, 270),
                BackColor = Color.White,
                Visible   = false
            };

            var lblInfo = MakeInfoBox(
                "A 6-digit code was sent to your email address.\nEnter it below. The code expires in 10 minutes.",
                new Point(30, 10));

            var lblCode = MakeFieldLabel("VERIFICATION CODE", new Point(30, 80));

            int[] sampleDigits = { 4, 8, 2, 9, 1, 7 };
            for (int i = 0; i < 6; i++)
            {
                int idx = i;
                txtCode[i] = new TextBox
                {
                    Location    = new Point(30 + i * 70, 100),
                    Size        = new Size(56, 44),
                    Font        = new Font("Segoe UI", 18f, FontStyle.Bold),
                    MaxLength   = 1,
                    TextAlign   = HorizontalAlignment.Center,
                    Text        = sampleDigits[i].ToString(),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor   = Color.FromArgb(245, 248, 255)
                };
                pnlStep2.Controls.Add(txtCode[i]);
            }

            var lnkRes = new LinkLabel
            {
                Text      = "Didn't receive the code? Resend",
                Location  = new Point(30, 158),
                AutoSize  = true,
                Font      = new Font("Segoe UI", 8.5f),
                LinkColor = Color.FromArgb(30, 77, 140)
            };

            var btnVer = new Button
            {
                Text      = "VERIFY CODE  →",
                Location  = new Point(30, 185),
                Size      = new Size(450, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 77, 140),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnVer.FlatAppearance.BorderSize = 0;
            btnVer.Click += (s, e) => { currentStep = 3; ShowStep(3); };
            btnVer.MouseEnter += (s, e) => btnVer.BackColor = Color.FromArgb(46, 109, 180);
            btnVer.MouseLeave += (s, e) => btnVer.BackColor = Color.FromArgb(30, 77, 140);

            pnlStep2.Controls.AddRange(new Control[] { lblInfo, lblCode, lnkRes, btnVer });
        }

        // ── Step 3 – New password ─────────────────────────────
        private void BuildStep3Panel()
        {
            pnlStep3 = new Panel
            {
                Location  = new Point(0, 170),
                Size      = new Size(520, 270),
                BackColor = Color.White,
                Visible   = false
            };

            var lblInfo = MakeInfoBox(
                "Create a strong new password.\nUse at least 8 characters, including numbers and symbols.",
                new Point(30, 10));

            var lblNew  = MakeFieldLabel("NEW PASSWORD",     new Point(30, 80));
            var txtNew  = MakeTextBox(new Point(30, 100), "");
            txtNew.PasswordChar = '●';
            txtNew.Size         = new Size(450, 30);

            var lblConf = MakeFieldLabel("CONFIRM PASSWORD", new Point(30, 145));
            var txtConf = MakeTextBox(new Point(30, 165), "");
            txtConf.PasswordChar = '●';
            txtConf.Size         = new Size(450, 30);

            var btnRst = new Button
            {
                Text      = "✔  RESET PASSWORD",
                Location  = new Point(30, 215),
                Size      = new Size(450, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnRst.FlatAppearance.BorderSize = 0;
            btnRst.Click += (s, e) =>
            {
                MessageBox.Show("Password reset successfully!\nPlease log in with your new password.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            };

            pnlStep3.Controls.AddRange(new Control[]
                { lblInfo, lblNew, txtNew, lblConf, txtConf, btnRst });
        }

        private void ShowStep(int step)
        {
            currentStep    = step;
            pnlStep1.Visible = (step == 1);
            pnlStep2.Visible = (step == 2);
            pnlStep3.Visible = (step == 3);
            pnlStepBar.Invalidate();
        }

        // ── Helpers ───────────────────────────────────────────
        private Label MakeInfoBox(string text, Point loc)
        {
            var lbl = new Label
            {
                Text      = text,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(60, 70, 90),
                Location  = loc,
                Size      = new Size(450, 60),
                BackColor = Color.FromArgb(240, 245, 255)
            };
            lbl.Paint += (s, e) =>
            {
                var rc = ((Label)s).ClientRectangle;
                using (var p = new Pen(Color.FromArgb(200, 215, 240), 1))
                    e.Graphics.DrawRectangle(p, 0, 0, rc.Width - 1, rc.Height - 1);
                using (var br = new SolidBrush(Color.FromArgb(240, 245, 255)))
                    e.Graphics.FillRectangle(br, 1, 1, rc.Width - 2, rc.Height - 2);
                using (var br2 = new SolidBrush(Color.FromArgb(60, 70, 90)))
                using (var f = new Font("Segoe UI", 9f))
                    e.Graphics.DrawString(text, f, br2, new RectangleF(8, 6, rc.Width - 16, rc.Height));
            };
            return lbl;
        }

        private Label MakeFieldLabel(string text, Point loc) =>
            new Label
            {
                Text      = text,
                Font      = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 100),
                Location  = loc,
                AutoSize  = true
            };

        private TextBox MakeTextBox(Point loc, string text) =>
            new TextBox
            {
                Location    = loc,
                Size        = new Size(450, 28),
                Font        = new Font("Segoe UI", 10f),
                Text        = text,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor   = Color.FromArgb(250, 251, 252)
            };
    }
}