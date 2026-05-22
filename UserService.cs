using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace EcommerceIS
{
    public class User
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }

    public static class UserService
    {
        public static User AuthenticateUser(string username, string rawPassword)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT u.UserID, u.Username, u.FirstName, u.LastName, u.Email, r.RoleName as Role, s.StatusName as Status FROM users u JOIN roles r ON u.RoleID = r.RoleID JOIN user_status s ON u.StatusID = s.StatusID WHERE u.Username=@u AND u.Password=@p";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", DBHelper.HashPassword(rawPassword));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserID = reader["UserID"].ToString(),
                                Username = reader["Username"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                Status = reader["Status"].ToString()
                            };
                        }
                    }
                }
            }
            return null; // Invalid credentials
        }

        public static bool CheckEmailExists(string email)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT UserID FROM users WHERE Email=@e";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@e", email);
                    return cmd.ExecuteScalar() != null;
                }
            }
        }

        public static bool ResetPassword(string email, string newRawPassword)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string q = "UPDATE users SET Password=@p WHERE Email=@e";
                using (var cmd = new MySqlCommand(q, conn))
                {
                    cmd.Parameters.AddWithValue("@p", DBHelper.HashPassword(newRawPassword));
                    cmd.Parameters.AddWithValue("@e", email);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static DataTable GetAllUsers(string searchTerm = "", string statusFilter = "All", string roleFilter = "All")
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT u.UserID, u.Username, u.FirstName, u.LastName, u.Email, r.RoleName as Role, s.StatusName as Status FROM users u JOIN roles r ON u.RoleID = r.RoleID JOIN user_status s ON u.StatusID = s.StatusID WHERE 1=1";
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += " AND (u.Username LIKE @s OR u.FirstName LIKE @s OR u.LastName LIKE @s OR u.Email LIKE @s)";
                }
                if (statusFilter != "All")
                {
                    query += " AND s.StatusName = @status";
                }
                if (roleFilter != "All")
                {
                    query += " AND r.RoleName = @role";
                }
                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                        cmd.Parameters.AddWithValue("@s", "%" + searchTerm + "%");
                    if (statusFilter != "All")
                        cmd.Parameters.AddWithValue("@status", statusFilter);
                    if (roleFilter != "All")
                        cmd.Parameters.AddWithValue("@role", roleFilter);

                    var da = new MySqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static void AddUser(string username, string rawPassword, string firstName, string lastName, string email, string role, string status)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO users (Username, Password, FirstName, LastName, Email, RoleID, StatusID) VALUES (@u, @p, @fn, @ln, @e, (SELECT RoleID FROM roles WHERE RoleName=@r), (SELECT StatusID FROM user_status WHERE StatusName=@s))";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", DBHelper.HashPassword(rawPassword));
                    cmd.Parameters.AddWithValue("@fn", firstName);
                    cmd.Parameters.AddWithValue("@ln", lastName);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@r", role);
                    cmd.Parameters.AddWithValue("@s", status);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateUser(string userId, string username, string rawPassword, string firstName, string lastName, string email, string role, string status)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                string query = "UPDATE users SET Username=@u, FirstName=@fn, LastName=@ln, Email=@e, RoleID=(SELECT RoleID FROM roles WHERE RoleName=@r), StatusID=(SELECT StatusID FROM user_status WHERE StatusName=@s)";
                if (!string.IsNullOrWhiteSpace(rawPassword))
                    query += ", Password=@p";
                query += " WHERE UserID=@id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    if (!string.IsNullOrWhiteSpace(rawPassword))
                        cmd.Parameters.AddWithValue("@p", DBHelper.HashPassword(rawPassword));
                    cmd.Parameters.AddWithValue("@fn", firstName);
                    cmd.Parameters.AddWithValue("@ln", lastName);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@r", role);
                    cmd.Parameters.AddWithValue("@s", status);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
