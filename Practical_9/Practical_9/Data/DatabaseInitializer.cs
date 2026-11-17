using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Practical_9.Data
{
    public static class DatabaseInitializer
    {
        private static readonly object SyncLock = new object();
        private static bool initialized;

        private sealed class SeedStudent
        {
            public string EnrollmentNo;
            public string StudentName;
            public string Department;
            public int BatchYear;
            public decimal CGPA;
            public string CompanyName;
            public string JobRole;
            public decimal? PackageLPA;
            public string PlacementDate;
            public string PlacementStatus;
        }

        public static void EnsureCreated()
        {
            if (initialized) return;
            lock (SyncLock)
            {
                if (initialized) return;
                string masterConnection = ConfigurationManager.ConnectionStrings["MasterDb"].ConnectionString;
                string placementConnection = ConfigurationManager.ConnectionStrings["PlacementDb"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(masterConnection))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "IF DB_ID(N'PlacementDB') IS NULL CREATE DATABASE [PlacementDB];";
                    command.ExecuteNonQuery();
                }

                using (SqlConnection connection = new SqlConnection(placementConnection))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
IF OBJECT_ID(N'dbo.PlacementStudents', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PlacementStudents
    (
        StudentId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        EnrollmentNo NVARCHAR(20) NOT NULL UNIQUE,
        StudentName NVARCHAR(100) NOT NULL,
        Department NVARCHAR(60) NOT NULL,
        BatchYear INT NOT NULL,
        CGPA DECIMAL(4,2) NOT NULL,
        CompanyName NVARCHAR(100) NULL,
        JobRole NVARCHAR(100) NULL,
        PackageLPA DECIMAL(7,2) NULL,
        PlacementDate DATE NULL,
        PlacementStatus NVARCHAR(20) NOT NULL
            CONSTRAINT CK_PlacementStudents_Status CHECK (PlacementStatus IN ('Placed','Not Placed'))
    );
    CREATE INDEX IX_PlacementStudents_Department ON dbo.PlacementStudents(Department);
END";
                        command.ExecuteNonQuery();
                    }

                    using (SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM dbo.PlacementStudents", connection))
                    {
                        int count = Convert.ToInt32(countCommand.ExecuteScalar());
                        if (count == 0) Seed(connection);
                    }
                }
                initialized = true;
            }
        }

        private static void Seed(SqlConnection connection)
        {
            List<SeedStudent> students = new List<SeedStudent>
            {
                new SeedStudent { EnrollmentNo = "CE25001", StudentName = "Aarav Patel", Department = "Computer Engineering", BatchYear = 2025, CGPA = 9.1m, CompanyName = "TCS", JobRole = "Assistant System Engineer", PackageLPA = 4.5m, PlacementDate = "2025-06-04", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "CE25002", StudentName = "Diya Shah", Department = "Computer Engineering", BatchYear = 2025, CGPA = 8.8m, CompanyName = "Infosys", JobRole = "Specialist Programmer", PackageLPA = 7.0m, PlacementDate = "2025-06-06", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "IT25003", StudentName = "Krish Mehta", Department = "Information Technology", BatchYear = 2025, CGPA = 8.4m, CompanyName = "Amazon", JobRole = "Software Development Engineer", PackageLPA = 18.0m, PlacementDate = "2025-06-08", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "IT25004", StudentName = "Anaya Desai", Department = "Information Technology", BatchYear = 2025, CGPA = 9.0m, CompanyName = "Microsoft", JobRole = "Associate Software Engineer", PackageLPA = 22.0m, PlacementDate = "2025-06-10", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "CE25005", StudentName = "Vivaan Joshi", Department = "Computer Engineering", BatchYear = 2025, CGPA = 7.9m, CompanyName = "Wipro", JobRole = "Project Engineer", PackageLPA = 4.0m, PlacementDate = "2025-06-12", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "ME25006", StudentName = "Aadhya Trivedi", Department = "Mechanical Engineering", BatchYear = 2025, CGPA = 8.2m, CompanyName = "Larsen & Toubro", JobRole = "Graduate Engineer Trainee", PackageLPA = 6.5m, PlacementDate = "2025-06-14", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "CE25007", StudentName = "Reyansh Patel", Department = "Civil Engineering", BatchYear = 2025, CGPA = 7.5m, CompanyName = null, JobRole = null, PackageLPA = null, PlacementDate = null, PlacementStatus = "Not Placed" },
                new SeedStudent { EnrollmentNo = "EC25008", StudentName = "Myra Shah", Department = "Electronics Engineering", BatchYear = 2025, CGPA = 8.7m, CompanyName = "Samsung", JobRole = "Graduate Engineer", PackageLPA = 8.0m, PlacementDate = "2025-06-17", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "CE25009", StudentName = "Arjun Parmar", Department = "Computer Engineering", BatchYear = 2025, CGPA = 7.2m, CompanyName = null, JobRole = null, PackageLPA = null, PlacementDate = null, PlacementStatus = "Not Placed" },
                new SeedStudent { EnrollmentNo = "IT25010", StudentName = "Isha Mehta", Department = "Information Technology", BatchYear = 2025, CGPA = 8.5m, CompanyName = "Accenture", JobRole = "Associate Software Engineer", PackageLPA = 5.5m, PlacementDate = "2025-06-19", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "ME25011", StudentName = "Advik Desai", Department = "Mechanical Engineering", BatchYear = 2025, CGPA = 8.0m, CompanyName = "Tata Motors", JobRole = "Graduate Engineer Trainee", PackageLPA = 6.0m, PlacementDate = "2025-06-21", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "CE25012", StudentName = "Sara Patel", Department = "Civil Engineering", BatchYear = 2025, CGPA = 7.8m, CompanyName = null, JobRole = null, PackageLPA = null, PlacementDate = null, PlacementStatus = "Not Placed" },
                new SeedStudent { EnrollmentNo = "EC25013", StudentName = "Kabir Shah", Department = "Electronics Engineering", BatchYear = 2025, CGPA = 8.3m, CompanyName = "Bosch", JobRole = "Engineering Trainee", PackageLPA = 6.8m, PlacementDate = "2025-06-23", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "CE25014", StudentName = "Kiara Joshi", Department = "Computer Engineering", BatchYear = 2025, CGPA = 9.2m, CompanyName = "Google", JobRole = "Software Engineer", PackageLPA = 24.0m, PlacementDate = "2025-06-25", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "IT25015", StudentName = "Rudra Mehta", Department = "Information Technology", BatchYear = 2025, CGPA = 7.7m, CompanyName = null, JobRole = null, PackageLPA = null, PlacementDate = null, PlacementStatus = "Not Placed" },
                new SeedStudent { EnrollmentNo = "CE25016", StudentName = "Riya Trivedi", Department = "Computer Engineering", BatchYear = 2025, CGPA = 8.6m, CompanyName = "Cognizant", JobRole = "Programmer Analyst", PackageLPA = 5.0m, PlacementDate = "2025-06-27", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "ME25017", StudentName = "Dhruv Desai", Department = "Mechanical Engineering", BatchYear = 2025, CGPA = 8.9m, CompanyName = "Reliance Industries", JobRole = "Graduate Engineer", PackageLPA = 7.5m, PlacementDate = "2025-06-29", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "EC25018", StudentName = "Anvi Patel", Department = "Electronics Engineering", BatchYear = 2025, CGPA = 7.9m, CompanyName = null, JobRole = null, PackageLPA = null, PlacementDate = null, PlacementStatus = "Not Placed" },
                new SeedStudent { EnrollmentNo = "CE25019", StudentName = "Atharv Shah", Department = "Computer Engineering", BatchYear = 2025, CGPA = 8.1m, CompanyName = "Capgemini", JobRole = "Analyst", PackageLPA = 4.2m, PlacementDate = "2025-07-02", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "IT25020", StudentName = "Navya Joshi", Department = "Information Technology", BatchYear = 2025, CGPA = 8.4m, CompanyName = "Deloitte", JobRole = "Technology Analyst", PackageLPA = 6.0m, PlacementDate = "2025-07-04", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "CE25021", StudentName = "Om Mehta", Department = "Civil Engineering", BatchYear = 2025, CGPA = 8.6m, CompanyName = "Shapoorji Pallonji", JobRole = "Site Engineer", PackageLPA = 5.8m, PlacementDate = "2025-07-06", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "CE25022", StudentName = "Saanvi Patel", Department = "Computer Engineering", BatchYear = 2025, CGPA = 7.6m, CompanyName = null, JobRole = null, PackageLPA = null, PlacementDate = null, PlacementStatus = "Not Placed" },
                new SeedStudent { EnrollmentNo = "IT25023", StudentName = "Yash Desai", Department = "Information Technology", BatchYear = 2025, CGPA = 9.1m, CompanyName = "Oracle", JobRole = "Associate Consultant", PackageLPA = 12.0m, PlacementDate = "2025-07-09", PlacementStatus = "Placed" },
                new SeedStudent { EnrollmentNo = "EC25024", StudentName = "Tara Shah", Department = "Electronics Engineering", BatchYear = 2025, CGPA = 8.0m, CompanyName = "Zydus Lifesciences", JobRole = "Graduate Trainee", PackageLPA = 5.2m, PlacementDate = "2025-07-11", PlacementStatus = "Placed" },
            };

            const string sql = @"INSERT INTO dbo.PlacementStudents
                (EnrollmentNo, StudentName, Department, BatchYear, CGPA, CompanyName, JobRole, PackageLPA, PlacementDate, PlacementStatus)
                VALUES (@EnrollmentNo, @StudentName, @Department, @BatchYear, @CGPA, @CompanyName, @JobRole, @PackageLPA, @PlacementDate, @PlacementStatus)";

            foreach (SeedStudent student in students)
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@EnrollmentNo", SqlDbType.NVarChar, 20).Value = student.EnrollmentNo;
                    command.Parameters.Add("@StudentName", SqlDbType.NVarChar, 100).Value = student.StudentName;
                    command.Parameters.Add("@Department", SqlDbType.NVarChar, 60).Value = student.Department;
                    command.Parameters.Add("@BatchYear", SqlDbType.Int).Value = student.BatchYear;
                    command.Parameters.Add("@CGPA", SqlDbType.Decimal).Value = student.CGPA;
                    command.Parameters["@CGPA"].Precision = 4;
                    command.Parameters["@CGPA"].Scale = 2;
                    command.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 100).Value = (object)student.CompanyName ?? DBNull.Value;
                    command.Parameters.Add("@JobRole", SqlDbType.NVarChar, 100).Value = (object)student.JobRole ?? DBNull.Value;
                    SqlParameter package = command.Parameters.Add("@PackageLPA", SqlDbType.Decimal);
                    package.Precision = 7;
                    package.Scale = 2;
                    package.Value = (object)student.PackageLPA ?? DBNull.Value;
                    command.Parameters.Add("@PlacementDate", SqlDbType.Date).Value = student.PlacementDate == null ? (object)DBNull.Value : DateTime.ParseExact(student.PlacementDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    command.Parameters.Add("@PlacementStatus", SqlDbType.NVarChar, 20).Value = student.PlacementStatus;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
