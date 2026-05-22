using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace InformationSystem
{
    public class PasswordRecoveryForm : Form
    {
        private Panel  pnlCard;
        private Label  lblTitle, lblSub, lblStep1, lblStep2, lblStep3;
        private Label  lblEmail, lblCode, lblNewPwd, lblConfirmPwd;
        private TextBox txtEmail, txtCode, txtNewPwd, txtConfirmPwd;
        private Button  btnSendCode, btnVerify, btnReset, btnBack;
        private Label  lblStatus;
        private int    currentStep = 1;

        public PasswordRecoveryForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text            = "Password Recovery";
            Size            = new Size(480, 580);
            StartPosition   = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            BackColor       = Color.FromArgb(240, 244, 248);
            Font            = new Font("Segoe UI", 9.5f);

            // ── Card ────────────────────────────────────────────────────
            pnlCard = new Panel
            {
                Size      = new Size(400, 490),
                Location  = new Point(40, 40),
                BackColor = Color.White
            };
            pnlCard.Paint += (s, e) =>
            {
                // top accent bar
                using var b = new SolidBrush(Color.FromArgb(26, 115, 232));
                e.Graphics.FillRectangle(b, 0, 0, pnlCard.Width, 5);
            };

            lblTitle = new Label
            {
                Text     = "Password Recovery",
                Font     = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 58, 92),
                AutoSize  = true,
                Location  = new Point(30, 28)
            };

            lblSub = new Label
            {
                Text      = "Follow the steps below to reset your password.",
                ForeColor = Color.FromArgb(100, 120, 140),
                Font      = new Font("Segoe UI", 9f),
                Size      = new Size(340, 34),
                Location  = new Point(30, 58)
            };

            // ── Step indicators ─────────────────────────────────────────
            var pnlSteps = new Panel
            {
                Size      = new Size(340, 50),
                Location  = new Point(30, 95),
                BackColor = Color.Transparent
            };
            pnlSteps.Paint += (s, e) => DrawStepIndicator(e.Graphics, pnlSteps.Width);

            // ── Step 1: Email ───────────────────────────────────────────
            lblEmail = new Label { Text = "Registered Email Address",
                ForeColor = Color.FromArgb(55,65,81), AutoSize = true, Location = new Point(30, 158) };

            txtEmail = new TextBox
            {
                Size            = new Size(340, 36),
                Location        = new Point(30, 178),
                BorderStyle     = BorderStyle.FixedSingle,
                Font            = new Font("Segoe UI", 10f),
                BackColor       = Color.FromArgb(248, 250, 252),
                PlaceholderText = "e.g. user@example.com"
            };

            btnSendCode = CreateButton("Send Verification Code", 30, 228,
                Color.FromArgb(26, 115, 232), Color.White);
            btnSendCode.Click += BtnSendCode_Click;

            // ── Step 2: Verify code ─────────────────────────────────────
            lblCode = new Label { Text = "Verification Code",
                ForeColor = Color.FromArgb(55,65,81), AutoSize = true,
                Location = new Point(30, 280), Visible = false };

            txtCode = new TextBox
            {
                Size            = new Size(340, 36),
                Location        = new Point(30, 300),
                BorderStyle     = BorderStyle.FixedSingle,
                Font            = new Font("Segoe UI", 10f),
                BackColor       = Color.FromArgb(248, 250, 252),
                PlaceholderText = "Enter 6-digit code",
                Visible         = false
            };

            btnVerify = CreateButton("Verify Code", 30, 350,
                Color.FromArgb(26, 115, 232), Color.White);
            btnVerify.Visible = false;
            btnVerify.Click  += BtnVerify_Click;

            // ── Step 3: New password ────────────────────────────────────
            lblNewPwd = new Label { Text = "New Password",
                ForeColor = Color.FromArgb(55,65,81), AutoSize = true,
                Location = new Point(30, 158), Visible = false };

            txtNewPwd = new TextBox
            {
                Size            = new Size(340, 36),
                Location        = new Point(30, 178),
                BorderStyle     = BorderStyle.FixedSingle,
                Font            = new Font("Segoe UI", 10f),
                BackColor       = Color.FromArgb(248, 250, 252),
                PlaceholderText = "Enter new password",
                PasswordChar    = '●',
                Visible         = false
            };

            lblConfirmPwd = new Label { Text = "Confirm New Password",
                ForeColor = Color.FromArgb(55,65,81), AutoSize = true,
                Location = new Point(30, 228), Visible = false };

            txtConfirmPwd = new TextBox
            {
                Size            = new Size(340, 36),
                Location        = new Point(30, 248),
                BorderStyle     = BorderStyle.FixedSingle,
                Font            = new Font("Segoe UI", 10f),
                BackColor       = Color.FromArgb(248, 250, 252),
                PlaceholderText = "Re-enter new password",
                PasswordChar    = '●',
                Visible         = false
            };

            btnReset = CreateButton("Reset Password", 30, 300,
                Color.FromArgb(22, 163, 74), Color.White);
            btnReset.Visible = false;
            btnReset.Click  += BtnReset_Click;

            // ── Status / error label ────────────────────────────────────
            lblStatus = new Label
            {
                Text      = "",
                AutoSize  = true,
                Location  = new Point(30, 405),
                ForeColor = Color.Crimson
            };

            // ── Back button ─────────────────────────────────────────────
            btnBack = new Button
            {
                Text      = "← Back to Login",
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(26, 115, 232),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(30, 440),
                Cursor    = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, e) => Close();

            pnlCard.Controls.AddRange(new Control[]
            {
                lblTitle, lblSub, pnlSteps,
                lblEmail, txtEmail, btnSendCode,
                lblCode, txtCode, btnVerify,
                lblNewPwd, txtNewPwd,
                lblConfirmPwd, txtConfirmPwd,
                btnReset, lblStatus, btnBack
            });

            Controls.Add(pnlCard);
        }

        private void DrawStepIndicator(Graphics g, int w)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            string[] labels = { "Email", "Verify", "Reset" };
            int circleR = 18;
            int[] cx = { 30, 170, 310 };

            // connector lines
            for (int i = 0; i < 2; i++)
            {
                Color lc = (currentStep > i + 1)
                    ? Color.FromArgb(26, 115, 232)
                    : Color.FromArgb(200, 210, 220);
                using var pen = new Pen(lc, 2);
                g.DrawLine(pen, cx[i] + circleR, 18, cx[i + 1] - circleR, 18);
            }

            // circles
            for (int i = 0; i < 3; i++)
            {
                bool done   = currentStep > i + 1;
                bool active = currentStep == i + 1;
                Color fill = done || active
                    ? Color.FromArgb(26, 115, 232)
                    : Color.FromArgb(220, 228, 236);

                using var b = new SolidBrush(fill);
                g.FillEllipse(b, cx[i] - circleR, 0, circleR * 2, circleR * 2);

                string txt = done ? "✓" : (i + 1).ToString();
                using var f = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                var sz  = g.MeasureString(txt, f);
                Color fc = (done || active) ? Color.White : Color.FromArgb(130, 145, 160);
                using var fb = new SolidBrush(fc);
                g.DrawString(txt, f, fb,
                    cx[i] - sz.Width / 2,
                    18 - sz.Height / 2);

                // step label
                using var lf = new Font("Segoe UI", 7.5f);
                var lsz = g.MeasureString(labels[i], lf);
                using var lb = new SolidBrush(Color.FromArgb(90, 105, 120));
                g.DrawString(labels[i], lf, lb, cx[i] - lsz.Width / 2, 38);
            }
        }

        private Button CreateButton(string text, int x, int y, Color back, Color fore)
        {
            var btn = new Button
            {
                Text      = text,
                Size      = new Size(340, 42),
                Location  = new Point(x, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void BtnSendCode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblStatus.ForeColor = Color.Crimson;
                lblStatus.Text      = "Please enter your email address.";
                return;
            }
            // TODO: send real code via email service
            currentStep         = 2;
            lblStatus.ForeColor = Color.FromArgb(22, 163, 74);
            lblStatus.Text      = "✔ Code sent to " + txtEmail.Text;

            lblEmail.Visible    = false;
            txtEmail.Visible    = false;
            btnSendCode.Visible = false;
            lblCode.Visible     = true;
            txtCode.Visible     = true;
            btnVerify.Visible   = true;
            pnlCard.Refresh();
        }

        private void BtnVerify_Click(object sender, EventArgs e)
        {
            // TODO: validate real code
            if (txtCode.Text.Trim() == "123456")
            {
                currentStep           = 3;
                lblStatus.Text        = "";
                lblCode.Visible       = false;
                txtCode.Visible       = false;
                btnVerify.Visible     = false;
                lblNewPwd.Visible     = true;
                txtNewPwd.Visible     = true;
                lblConfirmPwd.Visible = true;
                txtConfirmPwd.Visible = true;
                btnReset.Visible      = true;
                pnlCard.Refresh();
            }
            else
            {
                lblStatus.ForeColor = Color.Crimson;
                lblStatus.Text      = "✖ Incorrect verification code.";
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (txtNewPwd.Text != txtConfirmPwd.Text)
            {
                lblStatus.ForeColor = Color.Crimson;
                lblStatus.Text      = "✖ Passwords do not match.";
                return;
            }
            if (txtNewPwd.Text.Length < 8)
            {
                lblStatus.ForeColor = Color.Crimson;
                lblStatus.Text      = "✖ Password must be at least 8 characters.";
                return;
            }
            // TODO: update password in database
            MessageBox.Show("Password successfully reset!\nYou may now log in with your new password.",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}