USE MASTER
GO
DROP DATABASE AnimalCareClinicDb

CREATE DATABASE AnimalCareClinicDb
GO

USE AnimalCareClinicDb
GO


CREATE TABLE Administrator (
UserID INT PRIMARY KEY NOT NULL,
FirstName NVARCHAR(50) NOT NULL,
LastName NVARCHAR(50) NOT NULL,
Email NVARCHAR(100) UNIQUE NOT NULL,
Password NVARCHAR(255) NOT NULL,
Role NVARCHAR(20)
);

CREATE TABLE Veterinarian (
UserID INT PRIMARY KEY NOT NULL,
FirstName NVARCHAR(50) NOT NULL,
LastName NVARCHAR(50) NOT NULL,
Email NVARCHAR(100) UNIQUE NOT NULL,
Password NVARCHAR(255) NOT NULL,
Speciality NVARCHAR(100),
Availability NVARCHAR(255),
Role NVARCHAR(20)
);

CREATE TABLE Receptionist (
UserID INT PRIMARY KEY NOT NULL,
FirstName NVARCHAR(50) NOT NULL,
LastName NVARCHAR(50) NOT NULL,
Email NVARCHAR(100) UNIQUE NOT NULL,
Password NVARCHAR(255) NOT NULL,
Phone NVARCHAR(20),
Role NVARCHAR(20)
);

CREATE TABLE Owner
(
OwnerID INT PRIMARY KEY NOT NULL, 
FirstName NVARCHAR(50),
LastName NVARCHAR(50),
Email NVARCHAR(25),
Phone NVARCHAR(20)
);
	   
CREATE TABLE Pet
(
PetID INT PRIMARY KEY NOT NULL,
Name NVARCHAR(50) NOT NULL,
Species NVARCHAR(50),
Age INT,
OwnerID INT FOREIGN KEY REFERENCES Owner(OwnerID) ON DELETE SET NULL
);

CREATE TABLE Appointment
(
AppointmentID INT PRIMARY KEY NOT NULL,
Date DATE NOT NULL,
Time TIME NOT NULL,
Duration INT,
Status NVARCHAR(20) CHECK (Status IN ('Scheduled', 'Completed', 'Cancelled')) DEFAULT 'Scheduled',
PetID INT FOREIGN KEY REFERENCES Pet(PetID) ON DELETE SET NULL,
VeterinarianID INT FOREIGN KEY REFERENCES Veterinarian(UserID) ON DELETE SET NULL
);

 
CREATE TABLE ClinicalHistory (
HistoryID INT PRIMARY KEY NOT NULL,
Date DATE NOT NULL,
Description NVARCHAR(MAX),
Treatment NVARCHAR(MAX),
PetID INT FOREIGN KEY REFERENCES Pet(PetID) ON DELETE SET NULL,
VeterinarianID INT FOREIGN KEY REFERENCES Veterinarian(UserID) ON DELETE SET NULL,
CreatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Report 
(
ReportID INT PRIMARY KEY NOT NULL,
Month INT CHECK (Month BETWEEN 1 AND 12),
Year INT,
AppointmentsMade INT DEFAULT 0,
AppointmentsCancelled INT DEFAULT 0
);

CREATE TABLE TimeSlot (
Slot TIME PRIMARY KEY
);

-------INSERTS---------

-- Administrators
INSERT INTO Administrator (UserID, FirstName, LastName, Email, Password, Role) VALUES
(1, 'Admin', 'Root', 'admin@clinic.com', '1234', 'Admin');

-- Veterinarians
INSERT INTO Veterinarian (UserID, FirstName, LastName, Email, Password, Speciality, Availability, Role) VALUES
(2, 'Ana', 'García', 'ana@clinic.com', '1234', 'Small animals', 'Mon-Fri 09-17', 'Veterinarian'),
(3, 'Luis', 'Ruiz', 'luis@clinic.com', '1234', 'Exotics', 'Tue-Thu 10-16', 'Veterinarian');

-- Receptionists
INSERT INTO Receptionist (UserID, FirstName, LastName, Email, Password, Phone, Role) VALUES
(4, 'Marta', 'López', 'marta@clinic.com', '1234', '555-1234', 'Receptionist');

-- Owners 
INSERT INTO Owner (OwnerID, FirstName, LastName, Email, Phone) VALUES
(10, 'Carlos', 'Pérez', 'cp@mail.com', '5551111'),
(11, 'Lucía', 'Díaz', 'ld@mail.com', '5552222'),
(12, 'Sonia', 'Sánchez', 'ss@mail.com', '5553333');

-- Pets
INSERT INTO Pet (PetID, Name, Species, Age, OwnerID) VALUES
(100, 'Toby', 'Dog', 3, 10),
(101, 'Misha', 'Cat', 2, 11),
(102, 'Luna', 'Rabbit', 1, 12);

-- Appointments
INSERT INTO Appointment (AppointmentID, Date, Time, Duration, Status, PetID, VeterinarianID)
VALUES
(1, CAST(GETDATE() AS DATE), CAST('09:00' AS TIME), 60, 'Scheduled', 100, 2),
(2, CAST('2025-10-09' AS DATE), CAST('10:30' AS TIME), 45, 'Scheduled', 101, 3),
(3, CAST('2025-11-09' AS DATE), CAST('11:15' AS TIME), 30, 'Completed', 100, 2),
(4, CAST('2025-11-29' AS DATE), CAST('12:00' AS TIME), 60, 'Cancelled', 102, 3),
(5, CAST('2025-11-19' AS DATE), CAST('15:45' AS TIME), 45, 'Scheduled', 101, 2),
(6, CAST(GETDATE() AS DATE), CAST('14:00' AS TIME), 30, 'Completed', 102, 3);

-- Clinical History
INSERT INTO ClinicalHistory (HistoryID, Date, Description, Treatment, PetID, VeterinarianID)
VALUES
(1, GETDATE(), 'Annual check-up', 'Vaccine booster', 100, 2),
(2, DATEADD(DAY,-1,GETDATE()), 'Ear infection', 'Antibiotic drops 7d', 101, 3),
(3, DATEADD(DAY,-2,GETDATE()), 'Dental cleaning', 'Scale & polish', 102, 2);

INSERT INTO Report (ReportID, Month, Year, AppointmentsMade, AppointmentsCancelled)
VALUES (1, MONTH(GETDATE()), YEAR(GETDATE()), 5, 1);

INSERT INTO TimeSlot VALUES
('09:00'),('10:00'),('11:00'),('12:00'),
('13:00'),('14:00'),('15:00'),('16:00'),('17:00');
GO


--------VIEWS-----------

-- ClinicUser View that combines all fields from all users (Admin, Vet, Recepcionist)
-- Union All is for joining all the tables and includes all duplicates records
CREATE OR ALTER VIEW ClinicUser
AS
SELECT 
    UserID,
    FirstName,
    LastName,
    Email,
    Password,
    Role,
    'Administrator' AS UserType,
    NULL AS Speciality,
    NULL AS Availability,
    NULL AS Phone
FROM Administrator

UNION ALL

SELECT 
    UserID,
    FirstName,
    LastName,
    Email,
    Password,
    Role,
    'Veterinarian' AS UserType,
    Speciality,
    Availability,
    NULL AS Phone
FROM Veterinarian

UNION ALL

SELECT 
    UserID,
    FirstName,
    LastName,
    Email,
    Password,
    Role,
    'Receptionist' AS UserType,
    NULL AS Speciality,
    NULL AS Availability,
    Phone
FROM Receptionist;
GO

--1. Appointments agenda
CREATE OR ALTER VIEW daily_appointments
AS 
SELECT 
    v.UserID AS VeterinarianID,
    u.FirstName + ' ' + u.LastName AS VetsName,
    a.Time as SlotTime, 
    a.AppointmentID, 
    p.Name as PetName, 
    o.FirstName + ' ' + o.LastName as OwnersName,
    a.Status
FROM Veterinarian v
JOIN ClinicUser u ON u.UserID = v.UserID
CROSS JOIN TimeSlot t
LEFT JOIN Appointment a ON a.VeterinarianID = u.UserID
    AND a.Date = CAST(GETDATE() AS DATE) 
    AND a.Time = t.Slot
LEFT JOIN Pet p ON p.PetID = a.PetID
LEFT JOIN Owner o ON o.OwnerID = p.OwnerID;
GO 


--2. Monthly vet's workload
CREATE OR ALTER VIEW vets_workload
AS 
SELECT 
    v.UserID, 
    u.FirstName + ' ' + u.LastName AS VetName,
    COUNT(a.AppointmentID) AS TotalAppointments, 
    SUM(CASE WHEN a.Status = 'Completed' THEN 1 ELSE 0 END) AS Completed,
    SUM(CASE WHEN a.Status = 'Cancelled' THEN 1 ELSE 0 END) AS Cancelled
FROM Veterinarian v
JOIN ClinicUser u ON u.UserID = v.UserID
JOIN Appointment a ON a.VeterinarianID = v.UserID
WHERE YEAR(a.Date) = YEAR(GETDATE())
    AND MONTH(a.Date) = MONTH(GETDATE())
GROUP BY v.UserID, u.FirstName, u.LastName;
GO

--3. Clinic History
CREATE OR ALTER VIEW v_pet_history
AS 
SELECT 
    p.PetID, 
    p.Name AS PetName,
    p.Species, 
    p.Age, 
    o.FirstName + ' ' + o.LastName as OwnersName,
    ch.Date, 
    ch.Description, 
    ch.Treatment, 
    vet.FirstName + ' ' + vet.LastName AS VetName
FROM Pet p
JOIN Owner o ON o.OwnerID = p.OwnerID
JOIN ClinicalHistory ch ON ch.PetID = p.PetID
JOIN Veterinarian v ON v.UserID = ch.VeterinarianID
JOIN ClinicUser vet ON vet.UserID = v.UserID;
GO

--4. Slots Available
CREATE OR ALTER VIEW vets_availability
AS 
SELECT 
    v.UserID, 
    u.FirstName + ' ' + u.LastName AS VetName,
    t.Slot
FROM Veterinarian v
JOIN ClinicUser u ON u.UserID = v.UserID
CROSS JOIN TimeSlot t
WHERE NOT EXISTS (
    SELECT 1
    FROM Appointment a
    WHERE a.VeterinarianID = v.UserID
        AND a.Date = CAST(GETDATE() AS DATE)
        AND a.Time = t.Slot
        AND a.Status <> 'Cancelled'
);
GO



----- PROCEDURES ---------

--1. New Appointment

CREATE PROCEDURE sp_new_appointment
@PetID          INT,
@VeterinarianID INT,
@Date           DATE,
@Time           TIME,
@Duration       INT = 60,
@AppointmentID INT 
AS
BEGIN 
IF EXISTS (SELECT 1
	FROM   Appointment
	WHERE  VeterinarianID = @VeterinarianID
	AND  Date  = @Date
	AND  Time  = @Time
	AND  Status <> 'Cancelled')
BEGIN;
	THROW 51000, 'Slot unavailable', 1;
END

INSERT INTO Appointment (AppointmentID, Date, Time, Duration, Status, PetID, VeterinarianID)
VALUES (@AppointmentID, @Date, @Time, @Duration, 'Scheduled', @PetID, @VeterinarianID);
END;
GO

--2 Cancel appointment
CREATE PROCEDURE sp_cancel_appointment
@AppointmentID INT
AS
BEGIN
SET NOCOUNT ON;
UPDATE Appointment
SET    Status = 'Cancelled'
WHERE  AppointmentID = @AppointmentID;
END;
GO

--3 Monthly report

CREATE PROCEDURE sp_monthly_report
@Year INT, @Month INT AS 
BEGIN
SELECT Date, COUNT(*) AS Total,
SUM (CASE WHEN Status='Completed' THEN 1 ELSE 0 END) AS Completed,
SUM (CASE WHEN Status='Cancelled' THEN 1 ELSE 0 END) AS Cancelled
FROM Appointment WHERE YEAR(Date) = @Year AND MONTH(Date) = @Month
GROUP BY Date 
ORDER BY Date;
END;
GO

--4 Upload clinical history

CREATE PROCEDURE sp_upload_history
@HistoryID INT,
@AppointmentID  INT,          
@Description NVARCHAR(MAX),  
@Treatment NVARCHAR(MAX),   
@VetID INT           
AS
BEGIN
  
IF NOT EXISTS (SELECT 1
FROM   Appointment
WHERE  AppointmentID = @AppointmentID
AND  Status = 'Scheduled')
BEGIN;
THROW 52000, 'This appointment was already completed', 1;
END

INSERT INTO ClinicalHistory (HistoryID, Date, Description, Treatment, PetID, VeterinarianID)
SELECT
@HistoryID,
CAST(GETDATE() AS DATE),
@Description,
@Treatment,
PetID,
@VetID
FROM   Appointment
WHERE  AppointmentID = @AppointmentID;


UPDATE Appointment
SET Status = 'Completed'
WHERE AppointmentID = @AppointmentID;
END;
GO

----- TRIGGERS ---------

--1 When delete an owner deletes pets
CREATE OR ALTER TRIGGER delete_pets_from_owner
ON Owner
AFTER DELETE
AS
BEGIN
    DELETE FROM Pet
    WHERE OwnerID IN (SELECT OwnerID FROM deleted);
END;
GO

--2 When deleting a pet cancel appointments
CREATE OR ALTER TRIGGER cancel_pet_future_appointments
ON Owner
AFTER DELETE
AS
BEGIN
    UPDATE Appointment
    SET Status = 'Cancelled'
    WHERE PetID IN (
        SELECT PetID 
        FROM Pet
        WHERE OwnerID IN (SELECT OwnerID FROM deleted)
    )
    AND Date >= CAST(GETDATE() AS DATE);
END;
GO

--3 When deleting a Vet, all appointments are cancelled

CREATE OR ALTER TRIGGER cancel_appointments_afterDelete
ON Veterinarian
INSTEAD OF DELETE
AS
BEGIN
UPDATE Appointment
SET Status = 'Cancelled'
WHERE VeterinarianID IN (SELECT UserID FROM deleted);
DELETE FROM Veterinarian
WHERE UserID IN (SELECT UserID FROM deleted);
END;
GO

--4 When pet is deleted, delete clinical history

CREATE OR ALTER TRIGGER delete_clinicalHistory_from_pet
ON Pet
AFTER DELETE
AS
BEGIN
    DELETE FROM ClinicalHistory
    WHERE PetID IN (SELECT PetID FROM deleted);
END;
GO


----------TEST VIEWS-----------
SELECT * FROM daily_appointments;

SELECT * FROM vets_workload;

SELECT * FROM v_pet_history;

SELECT * FROM vets_availability ORDER BY VetName, Slot;

----------TEST PROCEDURES-----------
--1
EXEC sp_new_appointment
    @PetID = 100,
    @VeterinarianID = 2,    
    @Date = '2025-11-17',
    @Time = '10:00',
    @Duration = 60,
    @AppointmentID = 7;

PRINT 'Appointment created with ID: 7';
GO

SELECT * FROM Appointment

--2
EXEC sp_cancel_appointment
    @AppointmentID = 3;

SELECT * FROM Appointment WHERE AppointmentID = 3;
GO

--3
EXEC sp_monthly_report
    @Year = '2025',
    @Month = '11';
GO

--4
EXEC sp_upload_history
    @HistoryID = 9,
    @AppointmentID = 1,
    @Description = N'Routine checkup results.',
    @Treatment = N'Antibiotics prescribed.',
    @VetID = 2;
GO

SELECT * FROM ClinicalHistory WHERE HistoryID = 9;
SELECT Status FROM Appointment WHERE AppointmentID = 1;

----------TEST TRIGGERS-----------

--1
SELECT * FROM Pet WHERE OwnerID = 10;
DELETE FROM Owner WHERE OwnerID = 10;
SELECT * FROM Pet WHERE OwnerID = 10;

--2
SELECT * FROM Appointment WHERE PetID IN (SELECT PetID FROM Pet WHERE OwnerID = 10);

DELETE FROM Owner WHERE OwnerID = 10;

SELECT * FROM Appointment WHERE PetID IN (SELECT PetID FROM Pet WHERE OwnerID = 10);

--3 When deleting a Vet, all appointments are cancelled
SELECT * FROM Appointment WHERE VeterinarianID = 3;

DELETE FROM Veterinarian WHERE UserID = 3;

SELECT * FROM Appointment;

--4
SELECT * FROM ClinicalHistory WHERE PetID = 101;

DELETE FROM Pet WHERE PetID = 101;

SELECT * FROM ClinicalHistory WHERE PetID = 101;