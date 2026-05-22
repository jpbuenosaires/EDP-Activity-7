using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EcommerceIS
{
    public class FrmLogin : Form
    {
        // ── Controls ──────────────────────────────────────────
        private Panel      pnlHeader;
        private Label      lblAppName;
        private Label      lblSubtitle;
        private Label      lblWelcome;
        private Label      lblSub;
        private Label      lblUser;
        private TextBox    txtUsername;
        private Label      lblPass;
        private TextBox    txtPassword;
        private CheckBox   chkRemember;
        private LinkLabel  lnkForgot;
        private Button     btnLogin;
        private Label      lblStatus;
        private Panel      pnlStatus;
        private Label      lblStatusDot;
        private Label      lblStatusText;
        private PictureBox picLogo;

        public FrmLogin()
        {
            InitializeComponent();
            CheckDBConnection();
        }

        private void InitializeComponent()
        {
            // ── Form ──────────────────────────────────────────
            this.Text            = "E-Commerce IS  —  Login";
            this.Size            = new Size(460, 540);
            this.StartPosition  = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.BackColor       = Color.White;
            this.Font            = new Font("Segoe UI", 9f);

            // ── Header panel ─────────────────────────────────
            pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 140,
                BackColor = Color.Transparent
            };
            pnlHeader.Paint += PnlHeader_Paint;

            picLogo = new PictureBox
            {
                Size      = new Size(56, 56),
                Location  = new Point(198, 18),
                BackColor = Color.Transparent,
                Image     = DrawShieldIcon(56)
            };

            lblAppName = new Label
            {
                Text      = "EcommerceDB System",
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 13f, FontStyle.Bold),
                AutoSize  = true,
                BackColor = Color.Transparent
            };
            lblAppName.Location = new Point(
                (pnlHeader.Width - lblAppName.PreferredWidth) / 2, 82);

            lblSubtitle = new Label
            {
                Text      = "BICOL UNIVERSITY  ·  IT 120",
                ForeColor = Color.FromArgb(200, 220, 255),
                Font      = new Font("Segoe UI", 7.5f, FontStyle.Regular),
                AutoSize  = true,
                BackColor = Color.Transparent
            };
            lblSubtitle.Location = new Point(
                (pnlHeader.Width - lblSubtitle.PreferredWidth) / 2, 108);

            pnlHeader.Controls.AddRange(new Control[]
                { picLogo, lblAppName, lblSubtitle });

            // ── Welcome text ──────────────────────────────────
            lblWelcome = new Label
            {
                Text     = "Welcome back!",
                Font     = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 77, 140),
                Location = new Point(40, 158),
                AutoSize = true
            };

            lblSub = new Label
            {
                Text      = "Please sign in to continue to the system.",
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.Gray,
                Location  = new Point(40, 180),
                AutoSize  = true
            };

            // ── Username ──────────────────────────────────────
            lblUser = MakeLabel("USERNAME", new Point(40, 210));
            txtUsername = MakeTextBox(new Point(40, 230), "admin");

            // ── Password ──────────────────────────────────────
            lblPass = MakeLabel("PASSWORD", new Point(40, 270));
            txtPassword = MakeTextBox(new Point(40, 290), "");
            txtPassword.PasswordChar = '●';
            txtPassword.Text         = "password";

            // ── Remember me / Forgot ──────────────────────────
            chkRemember = new CheckBox
            {
                Text     = "Remember me",
                Location = new Point(40, 335),
                Font     = new Font("Segoe UI", 8.5f),
                AutoSize = true
            };

            lnkForgot = new LinkLabel
            {
                Text      = "Forgot password?",
                Location  = new Point(295, 337),
                Font      = new Font("Segoe UI", 8.5f),
                AutoSize  = true,
                LinkColor = Color.FromArgb(30, 77, 140)
            };
            lnkForgot.LinkClicked += (s, e) =>
            {
                var frm = new FrmPasswordRecovery();
                frm.Show();
            };

            // ── Login button ──────────────────────────────────
            btnLogin = new Button
            {
                Text      = "SIGN IN",
                Location  = new Point(40, 365),
                Size      = new Size(370, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 77, 140),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            btnLogin.MouseEnter += (s, e) =>
                btnLogin.BackColor = Color.FromArgb(46, 109, 180);
            btnLogin.MouseLeave += (s, e) =>
                btnLogin.BackColor = Color.FromArgb(30, 77, 140);

            // ── Status bar ────────────────────────────────────
            pnlStatus = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 28,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            lblStatusDot = new Label
            {
                Size      = new Size(10, 10),
                Location  = new Point(10, 9),
                BackColor = Color.LimeGreen
            };

            lblStatusText = new Label
            {
                Text      = "Connected  ·  localhost  ·  EcommerceDB  ·  MySQL 8.0",
                Font      = new Font("Segoe UI", 7.5f),
                ForeColor = Color.DimGray,
                Location  = new Point(26, 7),
                AutoSize  = true
            };

            Label lblVer = new Label
            {
                Text      = "v2.0.0",
                Font      = new Font("Segoe UI", 7.5f),
                ForeColor = Color.Gray,
                Location  = new Point(380, 7),
                AutoSize  = true
            };

            pnlStatus.Controls.AddRange(new Control[]
                { lblStatusDot, lblStatusText, lblVer });

            // ── Assemble ──────────────────────────────────────
            this.Controls.AddRange(new Control[]
            {
                pnlHeader, lblWelcome, lblSub,
                lblUser,   txtUsername,
                lblPass,   txtPassword,
                chkRemember, lnkForgot,
                btnLogin,  pnlStatus
            });
        }

        // ── Paint gradient on header ──────────────────────────
        private void PnlHeader_Paint(object sender, PaintEventArgs e)
        {
            var rc = pnlHeader.ClientRectangle;
            using (var br = new LinearGradientBrush(
                rc,
                Color.FromArgb(22, 57, 120),
                Color.FromArgb(46, 109, 180),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(br, rc);
            }

            // Re-position labels after paint (safe spot)
            lblAppName.Left  = (rc.Width - lblAppName.PreferredWidth)  / 2;
            lblSubtitle.Left = (rc.Width - lblSubtitle.PreferredWidth) / 2;
            picLogo.Left     = (rc.Width - picLogo.Width)              / 2;
        }

        // ── Login logic ───────────────────────────────────────
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Please enter both username and password.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Simple credential check — extend with DB query as needed
            if (user == "admin" && pass == "password")
            {
                this.Hide();
                new FrmDashboard().ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.",
                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        // ── DB connection indicator ───────────────────────────
        private void CheckDBConnection()
        {
            if (!DBHelper.TestConnection())
            {
                lblStatusDot.BackColor = Color.OrangeRed;
                lblStatusText.Text     = "Not connected  ·  Check MySQL settings";
            }
        }

        // ── Helpers ───────────────────────────────────────────
        private Label MakeLabel(string text, Point loc)
        {
            return new Label
            {
                Text      = text,
                Font      = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 100),
                Location  = loc,
                AutoSize  = true
            };
        }

        private TextBox MakeTextBox(Point loc, string text)
        {
            var tb = new TextBox
            {
                Location  = loc,
                Size      = new Size(370, 28),
                Font      = new Font("Segoe UI", 10f),
                Text      = text,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 251, 252)
            };
            tb.Enter += (s, e) => ((TextBox)s).BackColor = Color.White;
            tb.Leave += (s, e) => ((TextBox)s).BackColor = Color.FromArgb(250, 251, 252);
            return tb;
        }

        private Image DrawShieldIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // Circle background
                using (var br = new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                    g.FillEllipse(br, 2, 2, size - 4, size - 4);
                using (var pen = new Pen(Color.FromArgb(160, 255, 255, 255), 2))
                    g.DrawEllipse(pen, 2, 2, size - 4, size - 4);

                // Simple lock icon
                int cx = size / 2, cy = size / 2 + 4;
                using (var br = new SolidBrush(Color.White))
                {
                    g.FillRectangle(br, cx - 10, cy - 6, 20, 14);
                    using (var p = new Pen(Color.White, 3))
                    {
                        g.DrawArc(p, cx - 7, cy - 18, 14, 14, 180, 180);
                    }
                    g.FillEllipse(new SolidBrush(Color.FromArgb(30, 77, 140)), cx - 3, cy - 2, 6, 6);
                }
            }
            return bmp;
        }
    }
}