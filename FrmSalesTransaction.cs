using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EcommerceIS
{
    public class FrmSalesTransaction : Form
    {
        private ComboBox cboCustomer;
        private TextBox txtProductSearch;
        private DataGridView dgvProducts;
        private DataGridView dgvCart;
        private NumericUpDown nudQty;
        private Button btnAddToCart, btnRemove, btnPlaceOrder, btnClear;
        private Label lblTotal, lblItemCount;
        private DataTable cartTable;

        public FrmSalesTransaction()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Sales Transaction";
            this.Size = new Size(1060, 620);
            this.BackColor = Color.FromArgb(245, 247, 252);
            this.Font = new Font("Segoe UI", 9f);

            // ── Left: Product Selection ───────────────────
            var pnlLeft = new Panel { Location = new Point(16, 16), Size = new Size(480, 560), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };
            pnlLeft.Paint += (s, e) => { using (var p = new Pen(Color.FromArgb(220, 225, 240))) e.Graphics.DrawRectangle(p, 0, 0, pnlLeft.Width - 1, pnlLeft.Height - 1); };

            pnlLeft.Controls.Add(SectionLabel("🛒  New Sale", new Point(16, 12)));

            // Customer
            pnlLeft.Controls.Add(FieldLabel("CUSTOMER", new Point(16, 48)));
            cboCustomer = new ComboBox { Location = new Point(16, 68), Size = new Size(448, 28), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f), FlatStyle = FlatStyle.Flat };
            pnlLeft.Controls.Add(cboCustomer);

            // Product search
            pnlLeft.Controls.Add(FieldLabel("SEARCH PRODUCTS", new Point(16, 100)));
            txtProductSearch = new TextBox { Location = new Point(16, 120), Size = new Size(448, 28), Font = new Font("Segoe UI", 9.5f), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(250, 251, 252) };
            txtProductSearch.TextChanged += (s, e) => LoadProducts();
            txtProductSearch.Enter += (s, e) => txtProductSearch.BackColor = Color.White;
            txtProductSearch.Leave += (s, e) => txtProductSearch.BackColor = Color.FromArgb(250, 251, 252);
            pnlLeft.Controls.Add(txtProductSearch);

            // Products grid
            pnlLeft.Controls.Add(FieldLabel("AVAILABLE PRODUCTS", new Point(16, 156)));
            dgvProducts = BuildGrid(new Point(16, 176), new Size(448, 260));
            dgvProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlLeft.Controls.Add(dgvProducts);

            // Quantity + Add button
            pnlLeft.Controls.Add(FieldLabel("QTY", new Point(16, 448)));
            nudQty = new NumericUpDown { Location = new Point(50, 444), Size = new Size(70, 28), Minimum = 1, Maximum = 999, Value = 1, Font = new Font("Segoe UI", 10f) };
            nudQty.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pnlLeft.Controls.Add(nudQty);

            btnAddToCart = MakeButton("➕ Add to Cart", new Point(130, 442), new Size(160, 34), Color.FromArgb(30, 77, 140));
            btnAddToCart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddToCart.Click += BtnAddToCart_Click;
            pnlLeft.Controls.Add(btnAddToCart);

            this.Controls.Add(pnlLeft);

            // ── Right: Cart ───────────────────────────────
            var pnlRight = new Panel { Location = new Point(510, 16), Size = new Size(530, 560), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            pnlRight.Paint += (s, e) => { using (var p = new Pen(Color.FromArgb(220, 225, 240))) e.Graphics.DrawRectangle(p, 0, 0, pnlRight.Width - 1, pnlRight.Height - 1); };

            pnlRight.Controls.Add(SectionLabel("📋  Order Cart", new Point(16, 12)));

            lblItemCount = new Label { Text = "0 items", Location = new Point(400, 16), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = Color.Gray, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            pnlRight.Controls.Add(lblItemCount);

            dgvCart = BuildGrid(new Point(16, 48), new Size(498, 360));
            dgvCart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlRight.Controls.Add(dgvCart);

            // Remove button
            btnRemove = MakeButton("✕ Remove Selected", new Point(16, 418), new Size(160, 32), Color.FromArgb(196, 43, 36));
            btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRemove.Click += BtnRemove_Click;
            pnlRight.Controls.Add(btnRemove);

            // Total display
            var pnlTotal = new Panel { Location = new Point(16, 462), Size = new Size(498, 80), BackColor = Color.FromArgb(245, 247, 252), Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            pnlTotal.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using (var pen = new Pen(Color.FromArgb(30, 77, 140), 2)) g.DrawRectangle(pen, 0, 0, pnlTotal.Width - 1, pnlTotal.Height - 1);
                using (var f = new Font("Segoe UI", 9f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.FromArgb(80, 90, 110)))
                    g.DrawString("TOTAL AMOUNT", f, br, 16, 10);
            };

            lblTotal = new Label { Text = "₱ 0.00", Location = new Point(16, 32), AutoSize = true, Font = new Font("Segoe UI", 22f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 77, 140) };
            pnlTotal.Controls.Add(lblTotal);

            btnPlaceOrder = MakeButton("✓  PLACE ORDER", new Point(300, 16), new Size(180, 50), Color.FromArgb(40, 167, 69));
            btnPlaceOrder.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnPlaceOrder.Anchor = AnchorStyles.Right;
            btnPlaceOrder.Click += BtnPlaceOrder_Click;
            pnlTotal.Controls.Add(btnPlaceOrder);

            pnlRight.Controls.Add(pnlTotal);
            this.Controls.Add(pnlRight);

            // Init cart table
            cartTable = new DataTable();
            cartTable.Columns.Add("ProductID", typeof(int));
            cartTable.Columns.Add("ProductName");
            cartTable.Columns.Add("UnitPrice", typeof(decimal));
            cartTable.Columns.Add("Quantity", typeof(int));
            cartTable.Columns.Add("Subtotal", typeof(decimal));
            dgvCart.DataSource = cartTable;
            if (dgvCart.Columns.Contains("ProductID")) dgvCart.Columns["ProductID"].Visible = false;
        }

        private void LoadData()
        {
            // Load customers
            var customers = TransactionService.GetCustomerList();
            cboCustomer.Items.Clear();
            foreach (DataRow row in customers.Rows)
                cboCustomer.Items.Add(new CustomerItem(Convert.ToInt32(row["CustomerID"]), row["CustomerName"].ToString()));
            if (cboCustomer.Items.Count > 0) cboCustomer.SelectedIndex = 0;

            LoadProducts();
        }

        private void LoadProducts()
        {
            dgvProducts.DataSource = TransactionService.GetAvailableProducts(txtProductSearch.Text);
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) { MessageBox.Show("Please select a product."); return; }

            int productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);
            string name = dgvProducts.CurrentRow.Cells["ProductName"].Value.ToString();
            decimal price = Convert.ToDecimal(dgvProducts.CurrentRow.Cells["Price"].Value);
            int stock = Convert.ToInt32(dgvProducts.CurrentRow.Cells["Stock"].Value);
            int qty = (int)nudQty.Value;

            if (qty > stock) { MessageBox.Show($"Insufficient stock! Available: {stock}"); return; }

            // Check if already in cart
            foreach (DataRow row in cartTable.Rows)
            {
                if (Convert.ToInt32(row["ProductID"]) == productId)
                {
                    int newQty = Convert.ToInt32(row["Quantity"]) + qty;
                    if (newQty > stock) { MessageBox.Show($"Total quantity exceeds stock! Available: {stock}"); return; }
                    row["Quantity"] = newQty;
                    row["Subtotal"] = price * newQty;
                    UpdateTotals();
                    return;
                }
            }

            cartTable.Rows.Add(productId, name, price, qty, price * qty);
            if (dgvCart.Columns.Contains("ProductID")) dgvCart.Columns["ProductID"].Visible = false;
            UpdateTotals();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow == null) return;
            cartTable.Rows.RemoveAt(dgvCart.CurrentRow.Index);
            UpdateTotals();
        }

        private void BtnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (cboCustomer.SelectedItem == null) { MessageBox.Show("Please select a customer."); return; }
            if (cartTable.Rows.Count == 0) { MessageBox.Show("Cart is empty. Add products first."); return; }

            var customer = (CustomerItem)cboCustomer.SelectedItem;
            var result = MessageBox.Show(
                $"Place order for {customer.Name}?\n\nItems: {cartTable.Rows.Count}\nTotal: {lblTotal.Text}",
                "Confirm Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                int orderId = TransactionService.CreateSalesOrder(customer.Id, cartTable);
                MessageBox.Show($"✅ Order #{orderId} placed successfully!\n\nCustomer: {customer.Name}\nTotal: {lblTotal.Text}",
                    "Order Placed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cartTable.Clear();
                UpdateTotals();
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error placing order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotals()
        {
            decimal total = 0;
            foreach (DataRow row in cartTable.Rows)
                total += Convert.ToDecimal(row["Subtotal"]);
            lblTotal.Text = $"₱ {total:N2}";
            lblItemCount.Text = $"{cartTable.Rows.Count} item(s)";
        }

        // ── Helpers ───────────────────────────────────────
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

        private Label SectionLabel(string text, Point loc) => new Label { Text = text, Font = new Font("Segoe UI", 12f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 70), Location = loc, AutoSize = true };
        private Label FieldLabel(string text, Point loc) => new Label { Text = text, Font = new Font("Segoe UI", 7.5f, FontStyle.Bold), ForeColor = Color.FromArgb(80, 90, 120), Location = loc, AutoSize = true };

        private Button MakeButton(string text, Point loc, Size size, Color color)
        {
            var btn = new Button { Text = text, Location = loc, Size = size, FlatStyle = FlatStyle.Flat, BackColor = color, ForeColor = Color.White, Font = new Font("Segoe UI", 9f, FontStyle.Bold), Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(color, 0.15f);
            btn.MouseLeave += (s, e) => btn.BackColor = color;
            return btn;
        }

        private class CustomerItem
        {
            public int Id; public string Name;
            public CustomerItem(int id, string name) { Id = id; Name = name; }
            public override string ToString() => Name;
        }
    }
}
