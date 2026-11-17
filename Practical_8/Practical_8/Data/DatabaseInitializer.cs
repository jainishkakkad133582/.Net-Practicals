using System.Configuration;
using System.Data.SqlClient;

namespace Practical_8.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            string masterConnection = ConfigurationManager.ConnectionStrings["MasterDb"].ConnectionString;
            string inventoryConnection = ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString;

            // Create InventoryDB if it does not exist.
            using (SqlConnection con = new SqlConnection(masterConnection))
            {
                con.Open();
                string databaseQuery =
                    "IF DB_ID(N'InventoryDB') IS NULL CREATE DATABASE [InventoryDB];";

                using (SqlCommand cmd = new SqlCommand(databaseQuery, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Create Products table if it does not exist.
            using (SqlConnection con = new SqlConnection(inventoryConnection))
            {
                con.Open();
                string tableQuery = @"
                    IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
                    BEGIN
                        CREATE TABLE dbo.Products
                        (
                            ProductId INT IDENTITY(1,1) PRIMARY KEY,
                            ProductName NVARCHAR(100) NOT NULL,
                            Quantity INT NOT NULL CHECK (Quantity >= 0),
                            Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0)
                        );
                    END";

                using (SqlCommand cmd = new SqlCommand(tableQuery, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
