using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace InformationSystem
{
    public class LoginForm : Form
    {
        // ── Controls ──────────────────────────────────────────────────
        private Panel  pnlLeft, pnlRight;
        private Label  lblAppName, lblTagline, lblWelcome, lblSub;
        private Label  lblUsername, lblPassword, lblError;
        private TextBox txtUsername, txtPassword;
        private Button  btnLogin, btnForgot;
        private CheckBox chkRemember;
        private PictureBox picLogo;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // ── Form ────────────────────────────────────────────────────
            Text            = "Information System – Login";
            Size            = new Size(860, 520);
            StartPosition   = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox     = false;
            BackColor       = Color.White;
            Font            = new Font("Segoe UI", 9.5f);

            // ── Left panel (branding) ───────────────────────────────────
            pnlLeft = new Panel
            {
                Dock      = DockStyle.Left,
                Width     = 340,
                BackColor = Color.FromArgb(26, 58, 92)   // navy
            };

            picLogo = new PictureBox
            {
                Size      = new Size(72, 72),
                Location  = new Point(134, 90),
                BackColor = Color.Transparent
            };
            picLogo.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var b = new SolidBrush(Color.FromArgb(255, 255, 255, 80));
                e.Graphics.FillEllipse(b, 0, 0, 72, 72);
                using var pen = new Pen(Color.White, 2.5f);
                e.Graphics.DrawString("IS", new Font("Segoe UI", 22, FontStyle.Bold),
                    Brushes.White, new PointF(14, 18));
            };

            lblAppName = new Label
            {
                Text      = "InfoSys",
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 22, FontStyle.Bold),
                AutoSize  = true,
                Location  = new Point(105, 175)
            };

            lblTagline = new Label
            {
                Text      = "Integrated Information Management",
                ForeColor = Color.FromArgb(180, 210, 240),
                Font      = new Font("Segoe UI", 9f),
                AutoSize  = true,
                Location  = new Point(60, 215)
            };

            // decorative dots
            var pnlDots = new Panel
            {
                Size      = new Size(200, 20),
                Location  = new Point(70, 380),
                BackColor = Color.Transparent
            };
            pnlDots.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int[] alphas = { 255, 160, 90 };
                for (int i = 0; i < 3; i++)
                    using (var b = new SolidBrush(Color.FromArgb(alphas[i], 255, 255, 255)))
                        e.Graphics.FillEllipse(b, i * 28, 2, 14, 14);
            };

            pnlLeft.Controls.AddRange(new Control[]
                { picLogo, lblAppName, lblTagline, pnlDots });

            // ── Right panel (form) ──────────────────────────────────────
            pnlRight = new Panel
            {
                Location  = new Point(340, 0),
                Size      = new Size(520, 520),
                BackColor = Color.White
            };

            lblWelcome = new Label
            {
                Text      = "Welcome Back!",
                Font      = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 58, 92),
                AutoSize  = true,
                Location  = new Point(90, 70)
            };

            lblSub = new Label
            {
                Text      = "Sign in to your account to continue.",
                ForeColor = Color.FromArgb(100, 120, 140),
                AutoSize  = true,
                Location  = new Point(90, 107)
            };

            lblUsername = new Label { Text = "Username", ForeColor = Color.FromArgb(55,65,81),
                AutoSize = true, Location = new Point(90, 148) };

            txtUsername = CreateTextBox(90, 168);
            txtUsername.PlaceholderText = "Enter your username";

            lblPassword = new Label { Text = "Password", ForeColor = Color.FromArgb(55,65,81),
                AutoSize = true, Location = new Point(90, 218) };

            txtPassword = CreateTextBox(90, 238);
            txtPassword.PlaceholderText = "Enter your password";
            txtPassword.PasswordChar   = '●';

            chkRemember = new CheckBox
            {
                Text      = "Remember me",
                ForeColor = Color.FromArgb(75, 85, 99),
                AutoSize  = true,
                Location  = new Point(90, 285)
            };

            btnForgot = new Button
            {
                Text        = "Forgot Password?",
                FlatStyle   = FlatStyle.Flat,
                ForeColor   = Color.FromArgb(26, 115, 232),
                BackColor   = Color.Transparent,
                Font        = new Font("Segoe UI", 9f),
                AutoSize    = true,
                Location    = new Point(280, 282),
                Cursor      = Cursors.Hand
            };
            btnForgot.FlatAppearance.BorderSize = 0;
            btnForgot.Click += (s, e) => { new PasswordRecoveryForm().ShowDialog(); };

            lblError = new Label
            {
                Text      = "",
                ForeColor = Color.Crimson,
                AutoSize  = true,
                Location  = new Point(90, 320),
                Visible   = false
            };

            btnLogin = new Button
            {
                Text      = "Sign In",
                Size      = new Size(340, 44),
                Location  = new Point(90, 340),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(26, 115, 232),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // hover effects
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(13, 71, 161);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.FromArgb(26, 115, 232);

            pnlRight.Controls.AddRange(new Control[]
            {
                lblWelcome, lblSub,
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                chkRemember, btnForgot,
                lblError, btnLogin
            });

            Controls.AddRange(new Control[] { pnlLeft, pnlRight });
        }

        private TextBox CreateTextBox(int x, int y)
        {
            return new TextBox
            {
                Size        = new Size(340, 38),
                Location    = new Point(x, y),
                BorderStyle = BorderStyle.FixedSingle,
                Font        = new Font("Segoe UI", 10f),
                BackColor   = Color.FromArgb(248, 250, 252)
            };
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Simple credential check (replace with DB logic)
            if (txtUsername.Text.Trim() == "admin" && txtPassword.Text == "admin123")
            {
                new DashboardForm().Show();
                Hide();
            }
            else
            {
                lblError.Text    = "✖  Invalid username or password.";
                lblError.Visible = true;
                txtPassword.Clear();
            }
        }
    }
}