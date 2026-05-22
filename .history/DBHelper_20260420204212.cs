using System;
using MySql.Data.MySqlClient;

namespace EcommerceIS
{
    public static class DBHelper
    {
        private static readonly string connStr =
            "Server=localhost;Database=EcommerceDB;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch { return false; }
        }
    }
}