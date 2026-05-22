using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace EcommerceIS
{
    public static class TransactionService
    {
        // ══════════════════════════════════════════════════════
        //  SALES TRANSACTION
        // ══════════════════════════════════════════════════════

        public static DataTable GetCustomerList()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var da = new MySqlDataAdapter(
                        "SELECT CustomerID, CONCAT(FirstName,' ',LastName) AS CustomerName, Email FROM customers ORDER BY FirstName", conn);
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch { return FallbackCustomers(); }
        }

        public static DataTable GetAvailableProducts(string search = "")
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT p.ProductID, p.ProductName, c.CategoryName AS Category, " +
                                 "p.Price, p.StockQuantity AS Stock " +
                                 "FROM products p JOIN categories c ON p.CategoryID=c.CategoryID " +
                                 "WHERE p.StockQuantity > 0";
                    if (!string.IsNullOrEmpty(search))
                        sql += " AND (p.ProductName LIKE @s OR c.CategoryName LIKE @s)";
                    sql += " ORDER BY p.ProductName";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        if (!string.IsNullOrEmpty(search))
                            cmd.Parameters.AddWithValue("@s", "%" + search + "%");
                        var da = new MySqlDataAdapter(cmd);
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch { return FallbackProducts(); }
        }

        public static int CreateSalesOrder(int customerId, DataTable cartItems)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        int orderId;
                        using (var cmd = new MySqlCommand(
                            "INSERT INTO orders (CustomerID, OrderDate, Status) VALUES (@cid, @dt, 'Pending')", conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@cid", customerId);
                            cmd.Parameters.AddWithValue("@dt", DateTime.Now.ToString("yyyy-MM-dd"));
                            cmd.ExecuteNonQuery();
                            orderId = (int)cmd.LastInsertedId;
                        }
                        foreach (DataRow row in cartItems.Rows)
                        {
                            using (var cmd = new MySqlCommand(
                                "INSERT INTO orderdetails (OrderID,ProductID,Quantity,UnitPrice) VALUES (@oid,@pid,@qty,@pr)", conn, tx))
                            {
                                cmd.Parameters.AddWithValue("@oid", orderId);
                                cmd.Parameters.AddWithValue("@pid", row["ProductID"]);
                                cmd.Parameters.AddWithValue("@qty", row["Quantity"]);
                                cmd.Parameters.AddWithValue("@pr", row["UnitPrice"]);
                                cmd.ExecuteNonQuery();
                            }
                            // NOTE: Stock deduction is handled automatically by the
                            // database trigger 'trg_AfterInsertOrderDetail'
                        }
                        tx.Commit();
                        return orderId;
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }

        // ══════════════════════════════════════════════════════
        //  ORDER PROCESSING
        // ══════════════════════════════════════════════════════

        public static DataTable GetAllOrders(string statusFilter = "All", string search = "")
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT o.OrderID, CONCAT(c.FirstName,' ',c.LastName) AS Customer, " +
                                 "o.OrderDate, o.Status, " +
                                 "CONCAT('₱',FORMAT(SUM(od.Quantity*od.UnitPrice),2)) AS Total " +
                                 "FROM orders o JOIN customers c ON o.CustomerID=c.CustomerID " +
                                 "JOIN orderdetails od ON o.OrderID=od.OrderID WHERE 1=1";
                    if (statusFilter != "All") sql += " AND o.Status=@st";
                    if (!string.IsNullOrEmpty(search)) sql += " AND (CONCAT(c.FirstName,' ',c.LastName) LIKE @s)";
                    sql += " GROUP BY o.OrderID ORDER BY o.OrderDate DESC";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        if (statusFilter != "All") cmd.Parameters.AddWithValue("@st", statusFilter);
                        if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@s", "%" + search + "%");
                        var da = new MySqlDataAdapter(cmd);
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch { return FallbackOrders(); }
        }

        public static DataTable GetOrderDetails(int orderId)
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "SELECT od.OrderDetailID, p.ProductName, od.Quantity, " +
                        "CONCAT('₱',FORMAT(od.UnitPrice,2)) AS UnitPrice, " +
                        "CONCAT('₱',FORMAT(od.Quantity*od.UnitPrice,2)) AS Subtotal " +
                        "FROM orderdetails od JOIN products p ON od.ProductID=p.ProductID WHERE od.OrderID=@oid", conn))
                    {
                        cmd.Parameters.AddWithValue("@oid", orderId);
                        var da = new MySqlDataAdapter(cmd);
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch { return FallbackOrderDetails(); }
        }

        public static void UpdateOrderStatus(int orderId, string newStatus)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("UPDATE orders SET Status=@s WHERE OrderID=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@s", newStatus);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ══════════════════════════════════════════════════════
        //  INVENTORY MANAGEMENT
        // ══════════════════════════════════════════════════════

        public static DataTable GetInventory(string search = "", string categoryFilter = "All")
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT p.ProductID, p.ProductName, c.CategoryName AS Category, " +
                                 "CONCAT('₱',FORMAT(p.Price,2)) AS Price, p.StockQuantity AS Stock, " +
                                 "p.Price AS RawPrice, " +
                                 "CASE WHEN p.StockQuantity<=5 THEN 'LOW' WHEN p.StockQuantity<=15 THEN 'MEDIUM' ELSE 'GOOD' END AS StockStatus " +
                                 "FROM products p JOIN categories c ON p.CategoryID=c.CategoryID WHERE 1=1";
                    if (categoryFilter != "All") sql += " AND c.CategoryName=@cat";
                    if (!string.IsNullOrEmpty(search)) sql += " AND p.ProductName LIKE @s";
                    sql += " ORDER BY p.ProductName";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        if (categoryFilter != "All") cmd.Parameters.AddWithValue("@cat", categoryFilter);
                        if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@s", "%" + search + "%");
                        var da = new MySqlDataAdapter(cmd);
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch { return FallbackInventory(); }
        }

        public static DataTable GetCategoryList()
        {
            try
            {
                using (var conn = DBHelper.GetConnection())
                {
                    conn.Open();
                    var da = new MySqlDataAdapter("SELECT CategoryID, CategoryName FROM categories ORDER BY CategoryName", conn);
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch { return FallbackCategories(); }
        }

        public static void AddProduct(string name, int categoryId, decimal price, int stock)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    "INSERT INTO products (ProductName,CategoryID,Price,StockQuantity) VALUES (@n,@c,@p,@s)", conn))
                {
                    cmd.Parameters.AddWithValue("@n", name);
                    cmd.Parameters.AddWithValue("@c", categoryId);
                    cmd.Parameters.AddWithValue("@p", price);
                    cmd.Parameters.AddWithValue("@s", stock);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateProduct(int productId, string name, int categoryId, decimal price, int stock)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    "UPDATE products SET ProductName=@n,CategoryID=@c,Price=@p,StockQuantity=@s WHERE ProductID=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@n", name);
                    cmd.Parameters.AddWithValue("@c", categoryId);
                    cmd.Parameters.AddWithValue("@p", price);
                    cmd.Parameters.AddWithValue("@s", stock);
                    cmd.Parameters.AddWithValue("@id", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void RestockProduct(int productId, int additionalQty)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(
                    "UPDATE products SET StockQuantity=StockQuantity+@q WHERE ProductID=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@q", additionalQty);
                    cmd.Parameters.AddWithValue("@id", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ══════════════════════════════════════════════════════
        //  FALLBACK DATA (offline mode)
        // ══════════════════════════════════════════════════════

        private static DataTable FallbackCustomers()
        {
            var dt = new DataTable();
            dt.Columns.Add("CustomerID", typeof(int));
            dt.Columns.Add("CustomerName");
            dt.Columns.Add("Email");
            dt.Rows.Add(1, "Maria Santos", "maria@email.com");
            dt.Rows.Add(2, "Juan dela Cruz", "juan@email.com");
            dt.Rows.Add(3, "Ana Reyes", "ana@email.com");
            dt.Rows.Add(4, "Carlos Mendoza", "carlos@email.com");
            dt.Rows.Add(5, "Rosa Villanueva", "rosa@email.com");
            return dt;
        }

        private static DataTable FallbackProducts()
        {
            var dt = new DataTable();
            dt.Columns.Add("ProductID", typeof(int));
            dt.Columns.Add("ProductName");
            dt.Columns.Add("Category");
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Stock", typeof(int));
            dt.Rows.Add(1, "Wireless Mouse", "Electronics", 450.00m, 25);
            dt.Rows.Add(2, "USB-C Hub", "Electronics", 1200.00m, 15);
            dt.Rows.Add(3, "Notebook A5", "Stationery", 85.00m, 50);
            dt.Rows.Add(4, "Desk Lamp LED", "Furniture", 780.00m, 12);
            dt.Rows.Add(5, "Keyboard Cover", "Accessories", 150.00m, 30);
            dt.Rows.Add(6, "HDMI Cable 2m", "Electronics", 320.00m, 40);
            dt.Rows.Add(7, "Mouse Pad XL", "Accessories", 250.00m, 20);
            dt.Rows.Add(8, "Ballpen Set", "Stationery", 65.00m, 100);
            return dt;
        }

        private static DataTable FallbackOrders()
        {
            var dt = new DataTable();
            dt.Columns.Add("OrderID", typeof(int)); dt.Columns.Add("Customer"); dt.Columns.Add("OrderDate"); dt.Columns.Add("Status"); dt.Columns.Add("Total");
            dt.Rows.Add(1, "Maria Santos", "2025-03-10", "Delivered", "₱3,200.00");
            dt.Rows.Add(2, "Juan dela Cruz", "2025-03-12", "Shipped", "₱1,540.00");
            dt.Rows.Add(3, "Ana Reyes", "2025-03-14", "Pending", "₱870.50");
            dt.Rows.Add(4, "Carlos Mendoza", "2025-03-15", "Delivered", "₱5,100.00");
            dt.Rows.Add(5, "Rosa Villanueva", "2025-03-16", "Pending", "₱420.00");
            dt.Rows.Add(6, "Jose Bautista", "2025-03-17", "Shipped", "₱2,890.00");
            dt.Rows.Add(7, "Elena Torres", "2025-03-18", "Delivered", "₱1,200.00");
            dt.Rows.Add(8, "Miguel Lim", "2025-03-19", "Pending", "₱3,660.00");
            return dt;
        }

        private static DataTable FallbackOrderDetails()
        {
            var dt = new DataTable();
            dt.Columns.Add("OrderDetailID", typeof(int)); dt.Columns.Add("ProductName"); dt.Columns.Add("Quantity", typeof(int)); dt.Columns.Add("UnitPrice"); dt.Columns.Add("Subtotal");
            dt.Rows.Add(1, "Wireless Mouse", 2, "₱450.00", "₱900.00");
            dt.Rows.Add(2, "USB-C Hub", 1, "₱1,200.00", "₱1,200.00");
            dt.Rows.Add(3, "Notebook A5", 5, "₱85.00", "₱425.00");
            return dt;
        }

        private static DataTable FallbackInventory()
        {
            var dt = new DataTable();
            dt.Columns.Add("ProductID", typeof(int)); dt.Columns.Add("ProductName"); dt.Columns.Add("Category");
            dt.Columns.Add("Price"); dt.Columns.Add("Stock", typeof(int)); dt.Columns.Add("RawPrice", typeof(decimal)); dt.Columns.Add("StockStatus");
            dt.Rows.Add(1, "Wireless Mouse", "Electronics", "₱450.00", 2, 450.00m, "LOW");
            dt.Rows.Add(2, "USB-C Hub", "Electronics", "₱1,200.00", 3, 1200.00m, "LOW");
            dt.Rows.Add(3, "Notebook A5", "Stationery", "₱85.00", 5, 85.00m, "LOW");
            dt.Rows.Add(4, "Ballpen Set", "Stationery", "₱65.00", 6, 65.00m, "MEDIUM");
            dt.Rows.Add(5, "Desk Lamp LED", "Furniture", "₱780.00", 7, 780.00m, "MEDIUM");
            dt.Rows.Add(6, "Keyboard Cover", "Accessories", "₱150.00", 8, 150.00m, "MEDIUM");
            dt.Rows.Add(7, "Mouse Pad XL", "Accessories", "₱250.00", 20, 250.00m, "GOOD");
            dt.Rows.Add(8, "HDMI Cable 2m", "Electronics", "₱320.00", 40, 320.00m, "GOOD");
            return dt;
        }

        private static DataTable FallbackCategories()
        {
            var dt = new DataTable();
            dt.Columns.Add("CategoryID", typeof(int)); dt.Columns.Add("CategoryName");
            dt.Rows.Add(1, "Electronics"); dt.Rows.Add(2, "Stationery"); dt.Rows.Add(3, "Furniture"); dt.Rows.Add(4, "Accessories");
            return dt;
        }
    }
}
