using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace EcommerceIS
{
    public class FrmInventoryManagement : Form
    {
        private DataGridView dgvInventory;
        private TextBox txtSearch, txtName, txtPrice, txtStock, txtRestock;
        private ComboBox cboCategory, cboCategoryFilter;
        private Button btnAdd, btnUpdate, btnRestock, btnClear;
        private string selectedProductId = "";

        public FrmInventoryManagement()
        {
            InitializeComponent();
            LoadInventory();
        }

        private void InitializeComponent()
        {
            this.Text = "Inventory Management";
            this.Size = new Size(1060, 620);
            this.BackColor = Color.FromArgb(245, 247, 252);
            this.Font = new Font("Segoe UI", 9f);

            // ── Left: Input Panel ─────────────────────────
            var pnlInput = new Panel { Location = new Point(16, 16), Size = new Size(320, 560), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };
            pnlInput.Paint += (s, e) => { using (var p = new Pen(Color.FromArgb(220, 225, 240))) e.Graphics.DrawRectangle(p, 0, 0, pnlInput.Width - 1, pnlInput.Height - 1); };

            pnlInput.Controls.Add(new Label { Text = "📦  Product Details", Font = new Font("Segoe UI", 12f, FontStyle.Bold), ForeColor = Color.FromArgb(30, 40, 70), Location = new Point(16, 12), AutoSize = true });

            int y = 50;
            pnlInput.Controls.Add(FL("PRODUCT NAME", new Point(16, y))); y += 20;
            txtName = MakeTB(new Point(16, y), 288); pnlInput.Controls.Add(txtName); y += 38;

            pnlInput.Controls.Add(FL("CATEGORY", new Point(16, y))); y += 20;
            cboCategory = new ComboBox { Location = new Point(16, y), Size = new Size(288, 28), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(250, 251, 252) };
            pnlInput.Controls.Add(cboCategory); y += 38;

            pnlInput.Controls.Add(FL("PRICE (₱)", new Point(16, y))); y += 20;
            txtPrice = MakeTB(new Point(16, y), 288); pnlInput.Controls.Add(txtPrice); y += 38;

            pnlInput.Controls.Add(FL("STOCK QUANTITY", new Point(16, y))); y += 20;
            txtStock = MakeTB(new Point(16, y), 288); pnlInput.Controls.Add(txtStock); y += 38;

            // Buttons
            y += 8;
            btnAdd = MakeButton("➕ Add Product", new Point(16, y), new Size(136, 36), Color.FromArgb(40, 167, 69));
            btnAdd.Click += BtnAdd_Click;
            pnlInput.Controls.Add(btnAdd);

            btnUpdate = MakeButton("✏ Update", new Point(160, y), new Size(136, 36), Color.FromArgb(0, 123, 255));
            btnUpdate.Click += BtnUpdate_Click;
            pnlInput.Controls.Add(btnUpdate);
            y += 46;

            btnClear = MakeButton("✕ Clear", new Point(16, y), new Size(280, 32), Color.Gray);
            btnClear.Click += BtnClear_Click;
            pnlInput.Controls.Add(btnClear);

            // ── Restock section ───────────────────────────
            y += 50;
            var pnlRestock = new Panel { Location = new Point(16, y), Size = new Size(288, 110), BackColor = Color.FromArgb(245, 247, 252) };
            pnlRestock.Paint += (s, e) => { using (var pen = new Pen(Color.FromArgb(40, 167, 69), 1)) e.Graphics.DrawRectangle(pen, 0, 0, pnlRestock.Width - 1, pnlRestock.Height - 1); };

            pnlRestock.Controls.Add(new Label { Text = "🔄  QUICK RESTOCK", Font = new Font("Segoe UI", 8f, FontStyle.Bold), ForeColor = Color.FromArgb(40, 167, 69), Location = new Point(10, 10), AutoSize = true });
            pnlRestock.Controls.Add(new Label { Text = "Additional Qty:", Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(60, 70, 90), Location = new Point(10, 38), AutoSize = true });
            txtRestock = MakeTB(new Point(110, 34), 160); pnlRestock.Controls.Add(txtRestock);

            btnRestock = MakeButton("⟳ Restock Selected", new Point(10, 68), new Size(260, 32), Color.FromArgb(40, 167, 69));
            btnRestock.Click += BtnRestock_Click;
            pnlRestock.Controls.Add(btnRestock);

            pnlInput.Controls.Add(pnlRestock);
            this.Controls.Add(pnlInput);

            // ── Right: Inventory Grid ─────────────────────
            var pnlGrid = new Panel { Location = new Point(350, 16), Size = new Size(694, 560), BackColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            pnlGrid.Paint += (s, e) => { using (var p = new Pen(Color.FromArgb(220, 225, 240))) e.Graphics.DrawRectangle(p, 0, 0, pnlGrid.Width - 1, pnlGrid.Height - 1); };

            // Filter bar
            pnlGrid.Controls.Add(new Label { Text = "Search:", Location = new Point(16, 18), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) });
            txtSearch = MakeTB(new Point(72, 14), 180);
            txtSearch.TextChanged += (s, e) => LoadInventory();
            pnlGrid.Controls.Add(txtSearch);

            pnlGrid.Controls.Add(new Label { Text = "Category:", Location = new Point(268, 18), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(60, 70, 90) });
            cboCategoryFilter = new ComboBox { Location = new Point(338, 14), Size = new Size(140, 28), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f), FlatStyle = FlatStyle.Flat };
            cboCategoryFilter.Items.Add("All");
            cboCategoryFilter.SelectedIndex = 0;
            cboCategoryFilter.SelectedIndexChanged += (s, e) => LoadInventory();
            pnlGrid.Controls.Add(cboCategoryFilter);

            dgvInventory = BuildGrid(new Point(16, 50), new Size(662, 494));
            dgvInventory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvInventory.CellClick += DgvInventory_CellClick;
            pnlGrid.Controls.Add(dgvInventory);

            this.Controls.Add(pnlGrid);

            // Load categories
            LoadCategories();
        }

        private void LoadCategories()
        {
            var cats = TransactionService.GetCategoryList();
            cboCategory.Items.Clear();
            cboCategoryFilter.Items.Clear();
            cboCategoryFilter.Items.Add("All");
            foreach (DataRow row in cats.Rows)
            {
                cboCategory.Items.Add(new CatItem(Convert.ToInt32(row["CategoryID"]), row["CategoryName"].ToString()));
                cboCategoryFilter.Items.Add(row["CategoryName"].ToString());
            }
            if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
            cboCategoryFilter.SelectedIndex = 0;
        }

        private void LoadInventory()
        {
            dgvInventory.DataSource = TransactionService.GetInventory(txtSearch.Text, cboCategoryFilter.Text);
            if (dgvInventory.Columns.Contains("RawPrice")) dgvInventory.Columns["RawPrice"].Visible = false;
            StyleStockColumn();
        }

        private void DgvInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvInventory.Rows[e.RowIndex];
            selectedProductId = row.Cells["ProductID"].Value.ToString();
            txtName.Text = row.Cells["ProductName"].Value.ToString();
            txtStock.Text = row.Cells["Stock"].Value.ToString();

            // Try to get raw price, fallback to parsing formatted price
            if (dgvInventory.Columns.Contains("RawPrice"))
                txtPrice.Text = row.Cells["RawPrice"].Value.ToString();
            else
            {
                string p = row.Cells["Price"].Value.ToString().Replace("₱", "").Replace(",", "").Trim();
                txtPrice.Text = p;
            }

            // Set category
            string cat = row.Cells["Category"].Value.ToString();
            for (int i = 0; i < cboCategory.Items.Count; i++)
                if (cboCategory.Items[i].ToString() == cat) { cboCategory.SelectedIndex = i; break; }

            btnAdd.Enabled = false;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            try
            {
                var cat = (CatItem)cboCategory.SelectedItem;
                TransactionService.AddProduct(txtName.Text.Trim(), cat.Id, decimal.Parse(txtPrice.Text), int.Parse(txtStock.Text));
                MessageBox.Show("✅ Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();
                BtnClear_Click(null, null);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedProductId)) { MessageBox.Show("Select a product to update."); return; }
            if (!ValidateInput()) return;
            try
            {
                var cat = (CatItem)cboCategory.SelectedItem;
                TransactionService.UpdateProduct(int.Parse(selectedProductId), txtName.Text.Trim(), cat.Id, decimal.Parse(txtPrice.Text), int.Parse(txtStock.Text));
                MessageBox.Show("✅ Product updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();
                BtnClear_Click(null, null);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void BtnRestock_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedProductId)) { MessageBox.Show("Select a product to restock."); return; }
            if (!int.TryParse(txtRestock.Text, out int qty) || qty <= 0) { MessageBox.Show("Enter a valid quantity."); return; }
            try
            {
                TransactionService.RestockProduct(int.Parse(selectedProductId), qty);
                MessageBox.Show($"✅ Restocked +{qty} units!", "Restocked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();
                txtRestock.Clear();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            selectedProductId = "";
            txtName.Clear(); txtPrice.Clear(); txtStock.Clear(); txtRestock.Clear();
            if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
            btnAdd.Enabled = true;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Product name is required."); return false; }
            if (!decimal.TryParse(txtPrice.Text, out _)) { MessageBox.Show("Enter a valid price."); return false; }
            if (!int.TryParse(txtStock.Text, out _)) { MessageBox.Show("Enter a valid stock quantity."); return false; }
            if (cboCategory.SelectedItem == null) { MessageBox.Show("Select a category."); return false; }
            return true;
        }

        private void StyleStockColumn()
        {
            if (!dgvInventory.Columns.Contains("StockStatus")) return;
            foreach (DataGridViewRow row in dgvInventory.Rows)
            {
                var cell = row.Cells["StockStatus"];
                if (cell?.Value == null) continue;
                switch (cell.Value.ToString())
                {
                    case "LOW":
                        cell.Style.ForeColor = Color.FromArgb(196, 43, 36);
                        cell.Style.BackColor = Color.FromArgb(255, 220, 220);
                        cell.Style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                        break;
                    case "MEDIUM":
                        cell.Style.ForeColor = Color.FromArgb(133, 100, 4);
                        cell.Style.BackColor = Color.FromArgb(255, 243, 205);
                        cell.Style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                        break;
                    case "GOOD":
                        cell.Style.ForeColor = Color.FromArgb(40, 167, 69);
                        cell.Style.BackColor = Color.FromArgb(212, 237, 218);
                        cell.Style.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                        break;
                }
            }
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

        private Label FL(string text, Point loc) => new Label { Text = text, Font = new Font("Segoe UI", 7.5f, FontStyle.Bold), ForeColor = Color.FromArgb(80, 90, 120), Location = loc, AutoSize = true };

        private TextBox MakeTB(Point loc, int width)
        {
            var tb = new TextBox { Location = loc, Width = width, Font = new Font("Segoe UI", 10f), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(250, 251, 252) };
            tb.Enter += (s, e) => tb.BackColor = Color.White;
            tb.Leave += (s, e) => tb.BackColor = Color.FromArgb(250, 251, 252);
            return tb;
        }

        private Button MakeButton(string text, Point loc, Size size, Color color)
        {
            var btn = new Button { Text = text, Location = loc, Size = size, FlatStyle = FlatStyle.Flat, BackColor = color, ForeColor = Color.White, Font = new Font("Segoe UI", 9f, FontStyle.Bold), Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(color, 0.15f);
            btn.MouseLeave += (s, e) => btn.BackColor = color;
            return btn;
        }

        private class CatItem
        {
            public int Id; public string Name;
            public CatItem(int id, string name) { Id = id; Name = name; }
            public override string ToString() => Name;
        }
    }
}
