using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Practical_9.Data
{
    public class PlacementRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["PlacementDb"].ConnectionString;

        public DashboardStats GetDashboardStats()
        {
            const string sql = @"
SELECT COUNT(*) AS TotalStudents,
       SUM(CASE WHEN PlacementStatus = 'Placed' THEN 1 ELSE 0 END) AS PlacedStudents,
       CAST(CASE WHEN COUNT(*) = 0 THEN 0 ELSE
            (100.0 * SUM(CASE WHEN PlacementStatus = 'Placed' THEN 1 ELSE 0 END) / COUNT(*)) END AS DECIMAL(6,2)) AS PlacementRate,
       CAST(ISNULL(AVG(CASE WHEN PlacementStatus = 'Placed' THEN PackageLPA END),0) AS DECIMAL(7,2)) AS AveragePackage,
       CAST(ISNULL(MAX(CASE WHEN PlacementStatus = 'Placed' THEN PackageLPA END),0) AS DECIMAL(7,2)) AS HighestPackage
FROM dbo.PlacementStudents;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return new DashboardStats();
                    return new DashboardStats
                    {
                        TotalStudents = Convert.ToInt32(reader["TotalStudents"]),
                        PlacedStudents = Convert.ToInt32(reader["PlacedStudents"]),
                        PlacementRate = Convert.ToDecimal(reader["PlacementRate"]),
                        AveragePackage = Convert.ToDecimal(reader["AveragePackage"]),
                        HighestPackage = Convert.ToDecimal(reader["HighestPackage"])
                    };
                }
            }
        }

        public DataTable GetBatchYears()
        {
            const string sql = "SELECT DISTINCT BatchYear FROM dbo.PlacementStudents ORDER BY BatchYear DESC;";
            return ExecuteTable(sql, null);
        }

        public DataTable GetDepartmentSummary()
        {
            const string sql = @"
SELECT Department,
       COUNT(*) AS TotalStudents,
       SUM(CASE WHEN PlacementStatus = 'Placed' THEN 1 ELSE 0 END) AS PlacedStudents,
       CAST(CASE WHEN COUNT(*) = 0 THEN 0 ELSE
            (100.0 * SUM(CASE WHEN PlacementStatus = 'Placed' THEN 1 ELSE 0 END) / COUNT(*)) END AS DECIMAL(6,1)) AS PlacementRate
FROM dbo.PlacementStudents
GROUP BY Department
ORDER BY Department;";
            return ExecuteTable(sql, null);
        }

        public DataTable GetPlacements(string department, int batchYear, string search, string sortExpression, string sortDirection)
        {
            const string sql = @"
SELECT StudentId, EnrollmentNo, StudentName, Department, BatchYear, CGPA,
       CompanyName, JobRole, PackageLPA, PlacementDate, PlacementStatus
FROM dbo.PlacementStudents
WHERE (@Department = '' OR Department = @Department)
  AND (@BatchYear = 0 OR BatchYear = @BatchYear)
  AND (@Search = '' OR StudentName LIKE '%' + @Search + '%'
       OR EnrollmentNo LIKE '%' + @Search + '%'
       OR ISNULL(CompanyName, '') LIKE '%' + @Search + '%'
       OR ISNULL(JobRole, '') LIKE '%' + @Search + '%');";
            DataTable table;
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Department", SqlDbType.NVarChar, 60).Value = department ?? String.Empty;
                command.Parameters.Add("@BatchYear", SqlDbType.Int).Value = batchYear;
                command.Parameters.Add("@Search", SqlDbType.NVarChar, 100).Value = (search ?? String.Empty).Trim();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    table = new DataTable();
                    adapter.Fill(table);
                }
            }

            string allowedSort = "StudentName";
            string[] allowed = { "StudentId", "EnrollmentNo", "StudentName", "Department", "BatchYear", "CGPA", "CompanyName", "PackageLPA", "PlacementDate", "PlacementStatus" };
            foreach (string column in allowed)
                if (String.Equals(sortExpression, column, StringComparison.OrdinalIgnoreCase)) allowedSort = column;
            string direction = String.Equals(sortDirection, "DESC", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
            table.DefaultView.Sort = "[" + allowedSort + "] " + direction;
            return table.DefaultView.ToTable();
        }

        public DataTable GetStudentById(int studentId)
        {
            const string sql = @"SELECT StudentId, EnrollmentNo, StudentName, Department, BatchYear, CGPA,
CompanyName, JobRole, PackageLPA, PlacementDate, PlacementStatus
FROM dbo.PlacementStudents WHERE StudentId = @StudentId;";
            SqlParameter parameter = new SqlParameter("@StudentId", SqlDbType.Int) { Value = studentId };
            return ExecuteTable(sql, parameter);
        }

        public DataSet GetReportDataSet()
        {
            const string sql = @"SELECT StudentId, EnrollmentNo, StudentName, Department, BatchYear, CGPA,
CompanyName, JobRole, PackageLPA, PlacementDate, PlacementStatus
FROM dbo.PlacementStudents
WHERE PlacementStatus = 'Placed'
ORDER BY Department, CompanyName, StudentName;";
            DataTable table = ExecuteTable(sql, null);
            table.TableName = "PlacementRecords";
            DataSet dataSet = new DataSet("PlacementDataSet");
            dataSet.Tables.Add(table);
            return dataSet;
        }

        private DataTable ExecuteTable(string sql, SqlParameter parameter)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameter != null) command.Parameters.Add(parameter);
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }
    }
}
