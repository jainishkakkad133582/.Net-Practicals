USE master;
GO
IF DB_ID(N'PlacementDB') IS NULL
    CREATE DATABASE PlacementDB;
GO
USE PlacementDB;
GO
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
END
GO
-- Re-run this script to reset the demo data to the supplied sample records.
BEGIN TRANSACTION;
DELETE FROM dbo.PlacementStudents;
DBCC CHECKIDENT ('dbo.PlacementStudents', RESEED, 0);
INSERT INTO dbo.PlacementStudents
    (EnrollmentNo, StudentName, Department, BatchYear, CGPA, CompanyName, JobRole, PackageLPA, PlacementDate, PlacementStatus)
VALUES
(N'CE25001', N'Aarav Patel', N'Computer Engineering', 2025, 9.1, N'TCS', N'Assistant System Engineer', 4.5, '2025-06-04', N'Placed'),
(N'CE25002', N'Diya Shah', N'Computer Engineering', 2025, 8.8, N'Infosys', N'Specialist Programmer', 7.0, '2025-06-06', N'Placed'),
(N'IT25003', N'Krish Mehta', N'Information Technology', 2025, 8.4, N'Amazon', N'Software Development Engineer', 18.0, '2025-06-08', N'Placed'),
(N'IT25004', N'Anaya Desai', N'Information Technology', 2025, 9.0, N'Microsoft', N'Associate Software Engineer', 22.0, '2025-06-10', N'Placed'),
(N'CE25005', N'Vivaan Joshi', N'Computer Engineering', 2025, 7.9, N'Wipro', N'Project Engineer', 4.0, '2025-06-12', N'Placed'),
(N'ME25006', N'Aadhya Trivedi', N'Mechanical Engineering', 2025, 8.2, N'Larsen & Toubro', N'Graduate Engineer Trainee', 6.5, '2025-06-14', N'Placed'),
(N'CE25007', N'Reyansh Patel', N'Civil Engineering', 2025, 7.5, NULL, NULL, NULL, NULL, N'Not Placed'),
(N'EC25008', N'Myra Shah', N'Electronics Engineering', 2025, 8.7, N'Samsung', N'Graduate Engineer', 8.0, '2025-06-17', N'Placed'),
(N'CE25009', N'Arjun Parmar', N'Computer Engineering', 2025, 7.2, NULL, NULL, NULL, NULL, N'Not Placed'),
(N'IT25010', N'Isha Mehta', N'Information Technology', 2025, 8.5, N'Accenture', N'Associate Software Engineer', 5.5, '2025-06-19', N'Placed'),
(N'ME25011', N'Advik Desai', N'Mechanical Engineering', 2025, 8.0, N'Tata Motors', N'Graduate Engineer Trainee', 6.0, '2025-06-21', N'Placed'),
(N'CE25012', N'Sara Patel', N'Civil Engineering', 2025, 7.8, NULL, NULL, NULL, NULL, N'Not Placed'),
(N'EC25013', N'Kabir Shah', N'Electronics Engineering', 2025, 8.3, N'Bosch', N'Engineering Trainee', 6.8, '2025-06-23', N'Placed'),
(N'CE25014', N'Kiara Joshi', N'Computer Engineering', 2025, 9.2, N'Google', N'Software Engineer', 24.0, '2025-06-25', N'Placed'),
(N'IT25015', N'Rudra Mehta', N'Information Technology', 2025, 7.7, NULL, NULL, NULL, NULL, N'Not Placed'),
(N'CE25016', N'Riya Trivedi', N'Computer Engineering', 2025, 8.6, N'Cognizant', N'Programmer Analyst', 5.0, '2025-06-27', N'Placed'),
(N'ME25017', N'Dhruv Desai', N'Mechanical Engineering', 2025, 8.9, N'Reliance Industries', N'Graduate Engineer', 7.5, '2025-06-29', N'Placed'),
(N'EC25018', N'Anvi Patel', N'Electronics Engineering', 2025, 7.9, NULL, NULL, NULL, NULL, N'Not Placed'),
(N'CE25019', N'Atharv Shah', N'Computer Engineering', 2025, 8.1, N'Capgemini', N'Analyst', 4.2, '2025-07-02', N'Placed'),
(N'IT25020', N'Navya Joshi', N'Information Technology', 2025, 8.4, N'Deloitte', N'Technology Analyst', 6.0, '2025-07-04', N'Placed'),
(N'CE25021', N'Om Mehta', N'Civil Engineering', 2025, 8.6, N'Shapoorji Pallonji', N'Site Engineer', 5.8, '2025-07-06', N'Placed'),
(N'CE25022', N'Saanvi Patel', N'Computer Engineering', 2025, 7.6, NULL, NULL, NULL, NULL, N'Not Placed'),
(N'IT25023', N'Yash Desai', N'Information Technology', 2025, 9.1, N'Oracle', N'Associate Consultant', 12.0, '2025-07-09', N'Placed'),
(N'EC25024', N'Tara Shah', N'Electronics Engineering', 2025, 8.0, N'Zydus Lifesciences', N'Graduate Trainee', 5.2, '2025-07-11', N'Placed');
COMMIT TRANSACTION;
GO
SELECT * FROM dbo.PlacementStudents ORDER BY StudentId;
SELECT COUNT(*) AS TotalStudents,
       SUM(CASE WHEN PlacementStatus='Placed' THEN 1 ELSE 0 END) AS PlacedStudents,
       CAST(100.0 * SUM(CASE WHEN PlacementStatus='Placed' THEN 1 ELSE 0 END) / NULLIF(COUNT(*),0) AS DECIMAL(6,2)) AS PlacementRate
FROM dbo.PlacementStudents;
GO
