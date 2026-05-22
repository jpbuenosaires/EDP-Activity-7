using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EcommerceIS
{
    public class FrmUserManagement : Form
    {
        private DataGridView dgvUsers;
        private TextBox txtSearch;
        private TextBox txtUsername, txtPassword, txtEmail, txtFirstName, txtLastName;
        private ComboBox cbRole, cbStatus, cbFilterStatus, cbFilterRole;
        private Button btnAdd, btnUpdate, btnClear;
        private string selectedUserId = "";

        public FrmUserManagement()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.Text = "E-Commerce IS - User Management";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 252);
            this.Font = new Font("Segoe UI", 9f);

            var pnlInput = new Panel { Location = new Point(16, 20), Size = new Size(320, 540), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };
            var pnlGrid = new Panel { Location = new Point(350, 20), Size = new Size(520, 540), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };

            // Title
            Label lblTitle = new Label { Text = "User Management", Font = new Font("Segoe UI", 16f, FontStyle.Bold), Location = new Point(20, 15), AutoSize = true, ForeColor = Color.FromArgb(30, 77, 140) };
            pnlInput.Controls.Add(lblTitle);

            // Input Fields
            int startX = 20, startY = 60, yGap = 40;
            pnlInput.Controls.Add(new Label { Text = "Username:", Location = new Point(startX, startY + 4), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            txtUsername = MakeTextBox(new Point(startX + 100, startY), 190);
            pnlInput.Controls.Add(txtUsername);

            pnlInput.Controls.Add(new Label { Text = "Password:", Location = new Point(startX, startY + yGap + 4), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            var pnlPass = new Panel { Location = new Point(startX + 100, startY + yGap), Size = new Size(190, 23), BorderStyle = BorderStyle.Fixed3D, BackColor = SystemColors.Window };
            txtPassword = new TextBox { Location = new Point(2, 2), Size = new Size(160, 16), BorderStyle = BorderStyle.None, BackColor = SystemColors.Window, PasswordChar = '●' };
            Label lblShowPass = new Label { Text = "👁", Font = new Font("Segoe UI", 9.5f), Location = new Point(164, 0), AutoSize = true, Cursor = Cursors.Hand, ForeColor = Color.Gray, BackColor = Color.Transparent };
            lblShowPass.Click += (s, e) => { txtPassword.PasswordChar = txtPassword.PasswordChar == '●' ? '\0' : '●'; lblShowPass.ForeColor = txtPassword.PasswordChar == '●' ? Color.Gray : Color.FromArgb(30, 77, 140); };
            pnlPass.Controls.Add(txtPassword);
            pnlPass.Controls.Add(lblShowPass);
            pnlInput.Controls.Add(pnlPass);

            pnlInput.Controls.Add(new Label { Text = "Email:", Location = new Point(startX, startY + yGap * 2 + 4), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            txtEmail = MakeTextBox(new Point(startX + 100, startY + yGap * 2), 190);
            pnlInput.Controls.Add(txtEmail);

            pnlInput.Controls.Add(new Label { Text = "First Name:", Location = new Point(startX, startY + yGap * 3 + 4), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            txtFirstName = MakeTextBox(new Point(startX + 100, startY + yGap * 3), 190);
            pnlInput.Controls.Add(txtFirstName);

            pnlInput.Controls.Add(new Label { Text = "Last Name:", Location = new Point(startX, startY + yGap * 4 + 4), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            txtLastName = MakeTextBox(new Point(startX + 100, startY + yGap * 4), 190);
            pnlInput.Controls.Add(txtLastName);

            pnlInput.Controls.Add(new Label { Text = "Role:", Location = new Point(startX, startY + yGap * 5 + 4), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            cbRole = MakeComboBox(new Point(startX + 100, startY + yGap * 5), 190);
            cbRole.Items.AddRange(new string[] { "Admin", "Staff", "Customer" });
            cbRole.SelectedIndex = 0;
            pnlInput.Controls.Add(cbRole);

            pnlInput.Controls.Add(new Label { Text = "Status:", Location = new Point(startX, startY + yGap * 6 + 4), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            cbStatus = MakeComboBox(new Point(startX + 100, startY + yGap * 6), 190);
            cbStatus.Items.AddRange(new string[] { "Active", "Inactive" });
            cbStatus.SelectedIndex = 0;
            pnlInput.Controls.Add(cbStatus);

            // Buttons
            btnAdd = new Button { Text = "Add", Location = new Point(startX, startY + yGap * 7 + 10), Width = 85, BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatAppearance.MouseOverBackColor = Color.FromArgb(33, 136, 56);
            btnAdd.Click += BtnAdd_Click;
            pnlInput.Controls.Add(btnAdd);

            btnUpdate = new Button { Text = "Update", Location = new Point(startX + 95, startY + yGap * 7 + 10), Width = 85, BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 105, 217);
            btnUpdate.Click += BtnUpdate_Click;
            pnlInput.Controls.Add(btnUpdate);

            btnClear = new Button { Text = "Clear", Location = new Point(startX + 190, startY + yGap * 7 + 10), Width = 85, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatAppearance.MouseOverBackColor = Color.DimGray;
            btnClear.Click += BtnClear_Click;
            pnlInput.Controls.Add(btnClear);

            // Grid Panel Components
            pnlGrid.Controls.Add(new Label { Text = "Search:", Location = new Point(20, 22), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            txtSearch = MakeTextBox(new Point(75, 18), 150);
            txtSearch.TextChanged += (s, e) => LoadUsers(txtSearch.Text);
            pnlGrid.Controls.Add(txtSearch);

            pnlGrid.Controls.Add(new Label { Text = "Role:", Location = new Point(235, 22), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            cbFilterRole = MakeComboBox(new Point(275, 18), 85);
            cbFilterRole.Items.AddRange(new string[] { "All", "Admin", "Staff", "Customer" });
            cbFilterRole.SelectedIndex = 0;
            cbFilterRole.SelectedIndexChanged += (s, e) => LoadUsers(txtSearch.Text);
            pnlGrid.Controls.Add(cbFilterRole);

            pnlGrid.Controls.Add(new Label { Text = "Status:", Location = new Point(370, 22), AutoSize = true, ForeColor = Color.FromArgb(60, 70, 90), Font = new Font("Segoe UI", 9f, FontStyle.Bold) });
            cbFilterStatus = MakeComboBox(new Point(420, 18), 80);
            cbFilterStatus.Items.AddRange(new string[] { "All", "Active", "Inactive" });
            cbFilterStatus.SelectedIndex = 0;
            cbFilterStatus.SelectedIndexChanged += (s, e) => LoadUsers(txtSearch.Text);
            pnlGrid.Controls.Add(cbFilterStatus);

            dgvUsers = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(480, 470),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                RowTemplate = { Height = 40 },
                GridColor = Color.FromArgb(230, 235, 245),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(80, 90, 110);
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvUsers.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 247, 252);
            dgvUsers.ColumnHeadersHeight = 40;
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 242, 255);
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 77, 140);
            dgvUsers.DefaultCellStyle.ForeColor = Color.FromArgb(50, 60, 80);
            dgvUsers.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);
            dgvUsers.CellClick += DgvUsers_CellClick;
            pnlGrid.Controls.Add(dgvUsers);

            this.Controls.Add(pnlInput);
            this.Controls.Add(pnlGrid);
        }

        private void LoadUsers(string searchTerm = "")
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string statusFilter = cbFilterStatus != null ? cbFilterStatus.Text : "All";
                string roleFilter = cbFilterRole != null ? cbFilterRole.Text : "All";
                dgvUsers.DataSource = UserService.GetAllUsers(searchTerm, statusFilter, roleFilter);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Username and Password are required.");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPassword.Text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                UserService.AddUser(txtUsername.Text.Trim(), txtPassword.Text, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtEmail.Text.Trim(), cbRole.Text, cbStatus.Text);
                MessageBox.Show("User added successfully.");
                LoadUsers();
                BtnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedUserId))
            {
                MessageBox.Show("Please select a user to update.");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtPassword.Text) && !System.Text.RegularExpressions.Regex.IsMatch(txtPassword.Text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                UserService.UpdateUser(selectedUserId, txtUsername.Text.Trim(), txtPassword.Text, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtEmail.Text.Trim(), cbRole.Text, cbStatus.Text);
                MessageBox.Show("User updated successfully.");
                LoadUsers();
                BtnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            selectedUserId = "";
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            cbRole.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            btnAdd.Enabled = true;
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                selectedUserId = row.Cells["UserID"].Value.ToString();
                txtUsername.Text = row.Cells["Username"].Value.ToString();
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                cbRole.Text = row.Cells["Role"].Value.ToString();
                cbStatus.Text = row.Cells["Status"].Value.ToString();
                txtPassword.Clear(); // Don't show password
                btnAdd.Enabled = false; // Disable add when updating
            }
        }

        // Helper UI Builders
        private TextBox MakeTextBox(Point loc, int width, string text = "")
        {
            var tb = new TextBox
            {
                Location = loc,
                Width = width,
                Text = text,
                Font = new Font("Segoe UI", 10f),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 251, 252)
            };
            tb.Enter += (s, e) => tb.BackColor = Color.White;
            tb.Leave += (s, e) => tb.BackColor = Color.FromArgb(250, 251, 252);
            return tb;
        }

        private ComboBox MakeComboBox(Point loc, int width)
        {
            var cb = new ComboBox
            {
                Location = loc,
                Width = width,
                Font = new Font("Segoe UI", 9.5f),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(250, 251, 252)
            };
            cb.Enter += (s, e) => cb.BackColor = Color.White;
            cb.Leave += (s, e) => cb.BackColor = Color.FromArgb(250, 251, 252);
            return cb;
        }
    }
}
