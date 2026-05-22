# 🎬 Video Recording Script — Activity 6 Demo
## (With Actual Database Data)

---

## Pre-Recording Checklist

- [ ] **Start XAMPP** → Start **Apache** and **MySQL**
- [ ] Import `ecommercedb.sql` into phpMyAdmin (if not already done)
- [ ] Open terminal in `Desktop\EDP` folder
- [ ] Run: `dotnet restore EcommerceS.csproj`
- [ ] Run: `dotnet build EcommerceS.csproj`
- [ ] Run: `dotnet run --project EcommerceS.csproj`
- [ ] Confirm the app launches and shows the Login screen
- [ ] Open screen recorder (**Xbox Game Bar**: `Win + G`, or OBS/Bandicam)

---

## 🔑 Login Credentials (from your database)

| Username | Password | Role | Status |
|---|---|---|---|
| **`jbuenosaires`** | **`password`** | Admin | Active |
| `msantos` | `password` | Staff | Active |
| `jrizal` | `password` | Staff | Active |
| `gsilang` | `password` | Customer | Active |
| `amabini` | `password` | Staff | **Inactive** |

> [!IMPORTANT]
> Use **`jbuenosaires`** / **`password`** for the demo — this is your Admin account and shows ALL sidebar modules.

---

## 🎥 SCENE-BY-SCENE SCRIPT

---

### SCENE 1 — Introduction (30 seconds)

**Show a title slide or just narrate:**
> *"Good day! This is Jayson Buenosaires from Bicol University, BS Information Technology. This is my Activity 6 demo for IT 120 — Event Driven Programming. I will demonstrate the three primary transactions and the report generation module with Excel export."*

---

### SCENE 2 — Login (1 minute)

1. Show the **Login form** on screen
2. Point out: *"The status bar at the bottom shows we are connected to MySQL EcommerceDB"*
3. Type:
   - Username: **`jbuenosaires`**
   - Password: **`password`**
4. Click **SIGN IN**
5. Say: *"I'm logging in as Admin to access all modules including User Management from Activity 5"*

---

### SCENE 3 — Dashboard (1 minute)

1. Show the Dashboard with KPI cards:
   - **20 customers**, **25 orders**, **25 products**, **revenue total**
2. Say: *"The sidebar shows our 3 primary transactions — Sales, Order Processing, and Inventory — along with the Report Generator and User Management from Activity 5"*
3. Hover over **Recent Orders** grid — show recent May 2026 orders with status colors
4. Hover over **Low Stock Products** grid
5. Point to the bottom left showing: *"Logged in as Jayson Buenosaires, Admin"*

---

### SCENE 4 — 🛒 Transaction 1: Sales Transaction (2 minutes)

1. Click **"🛒 Sales"** in the sidebar
2. Say: *"This is our first primary transaction — Sales Transaction. Here we can process new customer orders."*

**Demo steps — type/click EXACTLY these:**

3. **Customer dropdown** → Select **"Alice Walker"** (first customer)
4. In the product search box, type **"wire"** → shows "Wireless Headphones" (₱99.99, Stock: 45)
5. Click **"Wireless Headphones"** in the product grid
6. Set Quantity to **3**
7. Click **"➕ Add to Cart"**
   - Cart shows: Wireless Headphones | ₱99.99 | Qty 3 | Subtotal ₱299.97
8. Clear search, type **"yoga"** → shows "Yoga Mat" (₱25.00, Stock: 100)
9. Click **"Yoga Mat"**, Quantity = **2**, click **"➕ Add to Cart"**
10. Clear search, type **"coffee"** → shows "Organic Coffee Beans" (₱14.99, Stock: 150)
11. Click it, Quantity = **5**, click **"➕ Add to Cart"**
12. Point out: *"The total shows ₱424.92 with 3 items in the cart"*
13. Click **"✓ PLACE ORDER"**
14. Confirmation dialog appears → Click **Yes**
15. Show success: *"Order #26 placed successfully!"*
16. Say: *"The stock quantities are automatically reduced by database triggers"*

---

### SCENE 5 — 📋 Transaction 2: Order Processing (2 minutes)

1. Click **"📋 Order Processing"** in the sidebar
2. Say: *"This is our second transaction — Order Processing. We can manage all customer orders and update their status."*

**Demo steps:**

3. Show the **25 orders** in the DataGridView
4. Point out the **color-coded status**:
   - 🟢 Green = Delivered
   - 🔵 Blue = Shipped
   - 🟡 Yellow = Pending
5. **Filter test**: Change Status dropdown to **"Pending"**
   - Shows only Pending orders (Order #2 Bob Harris May 1, #3 Charlie Clark May 2, #6 Fiona Gallagher May 4, etc.)
6. Set back to **"All"**
7. **Click on Order #2** (Bob Harris — Pending, ₱59.97)
   - Right panel shows: Cotton T-Shirt × 3 at ₱19.99 each
8. Say: *"Let's update this order from Pending to Shipped"*
9. Change Update Status dropdown to **"Shipped"**
10. Click **"✓ Update Status"**
11. Confirm → Show success: *"Order #2 updated to Shipped"*
12. Show Order #2 now appears with **blue "Shipped"** status

> [!WARNING]
> **DO NOT** try to change a "Delivered" order's status — the database trigger `trg_BeforeUpdateOrderStatus` will block it with an error. Only update Pending orders.

---

### SCENE 6 — 📦 Transaction 3: Inventory Management (2 minutes)

1. Click **"📦 Inventory"** in the sidebar
2. Say: *"This is our third transaction — Inventory Management. We can view stock levels, add products, and restock."*

**Demo steps:**

3. Show the **25 products** in the grid with stock status colors:
   - Products like "Acoustic Guitar" (Stock: 15, MEDIUM), "Ergonomic Office Chair" (Stock: 25, GOOD)
4. **Filter by category**: Select **"Electronics"** → shows Wireless Headphones, Smartphone Case
5. Set back to **"All"**
6. **Add a new product:**
   - Product Name: **`Bluetooth Speaker`**
   - Category: **`Electronics`**
   - Price: **`45.99`**
   - Stock: **`30`**
   - Click **"➕ Add Product"** → Show success
   - Point out the new product in the grid with "GOOD" stock status
7. **Quick Restock demo:**
   - Click on **"Acoustic Guitar"** (Stock: 15, MEDIUM)
   - In Quick Restock, enter **`10`**
   - Click **"⟳ Restock Selected"** → Show success
   - Point out stock changed from 15 → 25, status now "GOOD"

---

### SCENE 7 — 🧑‍💼 User Management / Activity 5 (1 minute)

1. Click **"🧑‍💼 User Management"** in the sidebar
2. Say: *"This is the User Management module from Activity 5, now integrated into the system."*
3. Show the **17 users** in the DataGridView:
   - Point out: *"We have Admins like myself, Staff members like Maria Santos and Jose Rizal, and Customer accounts"*
4. **Filter by Role**: Select **"Staff"** → shows 4 staff members
5. **Filter by Status**: Select **"Inactive"** → shows Apolinario Mabini (Staff, Inactive) and Gregorio Del Pilar (Customer, Inactive)
6. Set both back to **"All"**
7. Click on a user to show the form populates
8. Say: *"We can add, update, search, and filter users — this was our Activity 5 deliverable"*

---

### SCENE 8 — 📈 Report Generator + Excel Export (3 minutes) ⭐

1. Click **"📈 Report Generator"** in the sidebar
2. Say: *"This is the Report Generation module. It displays data in a DataGridView and can export to Excel."*

---

#### 📊 EXPORT 1 — Sales Summary Report

3. Report Type: **"Sales Summary Report"** (already selected)
4. Set Date From: **05/01/2026**
5. Set Date To: **05/20/2026**
6. Status: **"All"**
7. Click **"▶ GENERATE REPORT"**
8. Show the DataGridView with all 25 orders
9. Say: *"Now I will export this to Excel"*
10. Click **"📊 Export to Excel"**
11. Save as: **`Sales_Summary_Report.xlsx`**
12. **Excel opens — show these things clearly:**
    - 📌 **Sheet 1 top**: *"See the company header with logo — 'EcommerceDB System' and the Bicol University subtitle"*
    - 📌 **Data table**: *"All the sales data is formatted with styled headers"*
    - 📌 **Scroll down** to the signature block: *"Here is the signature placeholder — 'Prepared by: Jayson Buenosaires' on the left, and 'Approved by' on the right with signature lines"*
    - 📌 **Click Sheet 2 "Chart" tab**: *"Sheet 2 contains the chart data showing orders grouped by status — Pending, Shipped, and Delivered — with a visual bar chart"*
13. Close Excel, go back to the app

---

#### 📊 EXPORT 2 — Customer Report

14. Change Report Type to: **"Customer Report"**
15. Click **"▶ GENERATE REPORT"**
16. Show 20 customers in the grid with order counts and total spent
17. Click **"📊 Export to Excel"**
18. Save as: **`Customer_Report.xlsx`**
19. **Show in Excel:**
    - 📌 **Header with logo** ✅
    - 📌 **Customer data table** ✅
    - 📌 **Signature block** at bottom ✅
    - 📌 **Sheet 2**: *"The chart shows the top customers by their total spending"*
20. Close Excel

---

#### 📊 EXPORT 3 — Product Inventory Report

21. Change Report Type to: **"Product Inventory Report"**
22. Click **"▶ GENERATE REPORT"**
23. Show 25+ products with stock status (LOW/MEDIUM/GOOD)
24. Click **"📊 Export to Excel"**
25. Save as: **`Inventory_Report.xlsx`**
26. **Show in Excel:**
    - 📌 **Header with logo** ✅
    - 📌 **Product inventory data** ✅
    - 📌 **Signature block** ✅
    - 📌 **Sheet 2**: *"The chart shows total stock quantity grouped by category — Electronics, Clothing, Sports, etc."*
27. Say: *"All three Excel report templates have the company header with logo, the signature placeholder with my name, and Sheet 2 with a graph of the data."*

---

### SCENE 9 — About Page (30 seconds)

1. Click **"ℹ️ About"**
2. Show: Developer name, Bicol University, IT 120, MySQL 8.0
3. Say: *"Built with C# .NET 8.0 Windows Forms, MySQL database with triggers, views, and stored procedures"*

---

### SCENE 10 — Logout + Closing (30 seconds)

1. Click **"🔒 Logout"** → Confirm → Returns to Login
2. Say: *"That concludes my Activity 6 demonstration. Thank you for watching."*
3. **Stop recording**

---

## ✅ Requirements Checklist (mention these during recording)

| # | Requirement | Where to Show | What to Say |
|---|---|---|---|
| 1 | Activity 5 User Management integrated | Scene 7 | *"User Management from Activity 5 is integrated in the sidebar"* |
| 2 | Transaction 1: Sales | Scene 4 | *"First primary transaction — Sales"* |
| 3 | Transaction 2: Order Processing | Scene 5 | *"Second primary transaction — Order Processing"* |
| 4 | Transaction 3: Inventory | Scene 6 | *"Third primary transaction — Inventory Management"* |
| 5 | DataGridView control | Every scene | *"Data is displayed in a DataGridView control"* |
| 6 | Export to Excel button | Scene 8 | *"Export to Excel button generates an .xlsx file"* |
| 7 | Header (company name + logo) | Each Excel file | *"Company header with logo at the top"* |
| 8 | Signature placeholder | Each Excel file | *"Signature placeholder with my name"* |
| 9 | Sheet 2 with graph | Each Excel file | *"Sheet 2 contains the chart/graph"* |
| 10 | 3 Excel report templates | Scene 8 (3 exports) | *"Three different report templates"* |

---

## 🗃️ Your Database at a Glance

| Table | Records | Key Data |
|---|---|---|
| `customers` | 20 | Alice Walker, Bob Harris, Charlie Clark, Diana Prince... |
| `products` | 25 | Wireless Headphones (₱99.99), Cotton T-Shirt (₱19.99), Desk Lamp (₱35.50)... |
| `orders` | 25 | Mix of Pending, Shipped, Delivered (May 1–20, 2026) |
| `categories` | 15 | Electronics, Clothing, Home & Garden, Sports, Books... |
| `users` | 17 | jbuenosaires (Admin), msantos (Staff), jrizal (Staff)... |
| `roles` | 3 | Admin, Staff, Customer |

**Database Triggers:**
- `trg_AfterInsertOrderDetail` → Auto-reduces stock when order detail is inserted
- `trg_AfterDeleteOrderDetail` → Auto-restores stock when order detail is deleted
- `trg_AfterUpdateOrderStatus` → Logs status changes to OrderStatusLog
- `trg_BeforeDeleteOrder` → Prevents deleting Shipped/Delivered orders
- `trg_BeforeUpdateOrderStatus` → Prevents changing Delivered orders
- `trg_BeforeInsertProduct` → Prevents negative prices
- `trg_BeforeUpdateProductStock` → Prevents negative stock
