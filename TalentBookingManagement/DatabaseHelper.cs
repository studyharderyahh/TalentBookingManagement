using System;
using System.Configuration;
using System.Data.SqlClient;

namespace TalentBookingManagement
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["TalentBookingManagementDB"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static bool ValidateUser(string userId, string password, out string userRole)
        {
            userRole = string.Empty;
            bool isValid = false;

            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT UserRole FROM Users WHERE UserID = @UserID AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userRole = reader["UserRole"].ToString();
                    isValid = true;
                }

                reader.Close();
            }

            return isValid;
        }
    }
}
