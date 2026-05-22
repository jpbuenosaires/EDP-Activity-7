using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace EcommerceIS
{
    public class FrmOrderProcessing : Form
    {
        private DataGridView dgvOrders, dgvDetails;
        private TextBox txtSearch;
        private ComboBox cboStatus, cboUpdateStatus;
        private Button btnUpdateStatus, btnRefresh;
        private Label lblInfo;
        private int selectedOrderId = -1;

        public FrmOrderProcessing()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void InitializeComponent()
        {
            this.Text = "Order Processing";
            this.Size = new Size(1060, 620);
            this.BackColor = Color.FromArgb(245, 247, 252);
            this.Font = new Font("Segoe UI", 9f);

            // ── Top filter bar ────────────────────────────
            var pnlFilter = new Panel { Location = new Point(16, 16), Size = new Size(1028, 52), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            pnlFilter.Paint += (s, e) => { using (var p = new Pen(Color.FromArgb(220, 225, 240))) e.Graphics.DrawRectangle(p, 0, 0, pnlFilter.Width - 1, pnlFilter.Height - 1); };

            pnlFilter.Controls.Add(new Label { Text = "Search:", Location = new Point(16, 16), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) });
            txtSearch = new TextBox { Location = new Point(72, 12), Size = new Size(180, 28), Font = new Font("Segoe UI", 9.5f), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(250, 251, 252) };
            txtSearch.TextChanged += (s, e) => LoadOrders();
            pnlFilter.Controls.Add(txtSearch);

            pnlFilter.Controls.Add(new Label { Text = "Status:", Location = new Point(270, 16), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) });
            cboStatus = new ComboBox { Location = new Point(322, 12), Size = new Size(120, 28), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f), FlatStyle = FlatStyle.Flat };
            cboStatus.Items.AddRange(new object[] { "All", "Pending", "Shipped", "Delivered" });
            cboStatus.SelectedIndex = 0;
            cboStatus.SelectedIndexChanged += (s, e) => LoadOrders();
            pnlFilter.Controls.Add(cboStatus);

            btnRefresh = MakeButton("⟳ Refresh", new Point(460, 10), new Size(100, 32), Color.FromArgb(100, 110, 140));
            btnRefresh.Click += (s, e) => LoadOrders();
            pnlFilter.Controls.Add(btnRefresh);

            this.Controls.Add(pnlFilter);

            // ── Left: Orders list ─────────────────────────
            var pnlOrders = new Panel { Location = new Point(16, 78), Size = new Size(560, 498), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };
            pnlOrders.Paint += (s, e) => { using (var p = new Pen(Color.FromArgb(220, 225, 240))) e.Graphics.DrawRectangle(p, 0, 0, pnlOrders.Width - 1, pnlOrders.Height - 1); };
            pnlOrders.Controls.Add(new Label { Text = "📋  Orders", Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 70), Location = new Point(16, 12), AutoSize = true });

            dgvOrders = BuildGrid(new Point(16, 42), new Size(528, 440));
            dgvOrders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrders.CellClick += DgvOrders_CellClick;
            pnlOrders.Controls.Add(dgvOrders);
            this.Controls.Add(pnlOrders);

            // ── Right: Order details + actions ────────────
            var pnlDetails = new Panel { Location = new Point(590, 78), Size = new Size(454, 498), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            pnlDetails.Paint += (s, e) => { using (var p = new Pen(Color.FromArgb(220, 225, 240))) e.Graphics.DrawRectangle(p, 0, 0, pnlDetails.Width - 1, pnlDetails.Height - 1); };
            pnlDetails.Controls.Add(new Label { Text = "📦  Order Details", Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 70), Location = new Point(16, 12), AutoSize = true });

            lblInfo = new Label { Text = "Select an order to view details", Location = new Point(16, 38), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = Color.Gray };
            pnlDetails.Controls.Add(lblInfo);

            dgvDetails = BuildGrid(new Point(16, 60), new Size(422, 320));
            dgvDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlDetails.Controls.Add(dgvDetails);

            // Action panel
            var pnlAction = new Panel { Location = new Point(16, 392), Size = new Size(422, 90), BackColor = Color.FromArgb(245, 247, 252), Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            pnlAction.Paint += (s, e) => { using (var pen = new Pen(Color.FromArgb(30, 77, 140), 1)) e.Graphics.DrawRectangle(pen, 0, 0, pnlAction.Width - 1, pnlAction.Height - 1); };

            pnlAction.Controls.Add(new Label { Text = "UPDATE STATUS", Font = new Font("Segoe UI", 7.5f, FontStyle.Bold), ForeColor = Color.FromArgb(80, 90, 120), Location = new Point(12, 10), AutoSize = true });

            cboUpdateStatus = new ComboBox { Location = new Point(12, 30), Size = new Size(180, 28), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f), FlatStyle = FlatStyle.Flat };
            cboUpdateStatus.Items.AddRange(new object[] { "Pending", "Shipped", "Delivered" });
            cboUpdateStatus.SelectedIndex = 0;
            pnlAction.Controls.Add(cboUpdateStatus);

            btnUpdateStatus = MakeButton("✓ Update Status", new Point(200, 26), new Size(160, 36), Color.FromArgb(30, 77, 140));
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            pnlAction.Controls.Add(btnUpdateStatus);

            pnlDetails.Controls.Add(pnlAction);
            this.Controls.Add(pnlDetails);
        }

        private void LoadOrders()
        {
            dgvOrders.DataSource = TransactionService.GetAllOrders(cboStatus.Text, txtSearch.Text);
            StyleStatusColumn();
        }

        private void DgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            selectedOrderId = Convert.ToInt32(dgvOrders.Rows[e.RowIndex].Cells["OrderID"].Value);
            string customer = dgvOrders.Rows[e.RowIndex].Cells["Customer"].Value.ToString();
            string status = dgvOrders.Rows[e.RowIndex].Cells["Status"].Value.ToString();
            string total = dgvOrders.Rows[e.RowIndex].Cells["Total"].Value.ToString();

            lblInfo.Text = $"Order #{selectedOrderId}  ·  {customer}  ·  {status}  ·  {total}";
            lblInfo.ForeColor = Color.FromArgb(30, 77, 140);

            dgvDetails.DataSource = TransactionService.GetOrderDetails(selectedOrderId);

            // Set the combo to current status
            int idx = cboUpdateStatus.Items.IndexOf(status);
            if (idx >= 0) cboUpdateStatus.SelectedIndex = idx;
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (selectedOrderId < 0) { MessageBox.Show("Please select an order first."); return; }

            string newStatus = cboUpdateStatus.Text;
            var result = MessageBox.Show($"Update Order #{selectedOrderId} to '{newStatus}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                TransactionService.UpdateOrderStatus(selectedOrderId, newStatus);
                MessageBox.Show($"✅ Order #{selectedOrderId} updated to '{newStatus}'.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrders();
                selectedOrderId = -1;
                lblInfo.Text = "Select an order to view details";
                lblInfo.ForeColor = Color.Gray;
                dgvDetails.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void StyleStatusColumn()
        {
            if (!dgvOrders.Columns.Contains("Status")) return;
            foreach (DataGridViewRow row in dgvOrders.Rows)
            {
                var cell = row.Cells["Status"];
                if (cell?.Value == null) continue;
                switch (cell.Value.ToString())
                {
                    case "Delivered":
                        cell.Style.ForeColor = Color.FromArgb(40, 167, 69);
                        cell.Style.BackColor = Color.FromArgb(212, 237, 218);
                        cell.Style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                        break;
                    case "Shipped":
                        cell.Style.ForeColor = Color.FromArgb(0, 123, 255);
                        cell.Style.BackColor = Color.FromArgb(204, 229, 255);
                        cell.Style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                        break;
                    case "Pending":
                        cell.Style.ForeColor = Color.FromArgb(133, 100, 4);
                        cell.Style.BackColor = Color.FromArgb(255, 243, 205);
                        cell.Style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                        break;
                }
            }
        }

        private DataGridView BuildGrid(Point loc, Size size)
        {
            var dgv = new DataGridView
            {
                Location = loc, Size = size, BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White, RowHeadersVisible = false,
                AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 8.5f), GridColor = Color.FromArgb(230, 235, 245),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(22, 44, 90);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 32;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 28;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 225, 250);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 30, 60);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 255);
            return dgv;
        }

        private Button MakeButton(string text, Point loc, Size size, Color color)
        {
            var btn = new Button { Text = text, Location = loc, Size = size, FlatStyle = FlatStyle.Flat, BackColor = color, ForeColor = Color.White, Font = new Font("Segoe UI", 9f, FontStyle.Bold), Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(color, 0.15f);
            btn.MouseLeave += (s, e) => btn.BackColor = color;
            return btn;
        }
    }
}
