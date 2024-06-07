

use DB_Project;


-- Create MEMBER table
CREATE TABLE MEMBER (
    ID INT IDENTITY(1, 1) PRIMARY KEY,
    FIRSTNAME VARCHAR(50) NOT NULL,
    LASTNAME VARCHAR(50) NOT NULL,
    DOB DATE NOT NULL,
    EMAIL VARCHAR(100) NOT NULL,
    GENDER VARCHAR(20) NOT NULL,
	MEMBERSHIPTYPE VARCHAR(100), 
	REGISTRATIONDATE DATE NOT NULL,
    PASSWORD VARCHAR(100),
	Status VARCHAR(100) NOT NULL DEFAULT 'Active',
);

CREATE TABLE MEMBERSHIP_TYPE (
    NAME VARCHAR(100) PRIMARY KEY,
    MEMBERID INT,
    DESCRIPTION TEXT,
	FOREIGN KEY (MEMBERID) REFERENCES MEMBER(ID),
    COST DECIMAL(10, 2)
);


CREATE TABLE MemberGetTrained (
    MemberID INT NOt null,
    TrainerID INT not null,
	FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID)
);

Create Table MemberGetAddGym (
    MemberID INT NOt null,
    GymID INT not null,
	FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (GymID) REFERENCES  GYM(GYMID)
);


CREATE TABLE MemberFeedback (
    Rating INT,
	Comment VARCHAR(100),
    TrainerID INT not null,
	MemberID INT NOt null,
	FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID)
);

--Diet Plan
CREATE TABLE MemberDietPlan (    
	 MemberID INT not null,
	 TrainerID INT not null,
	 Typeofdiet VARCHAR(200) NOt null,
	 Nutrition VARCHAR(200) not null,
	 Purpose VARCHAR(200) NOT NULL,
	 Peanuts INT  NULL,
	 Gluten INT  NULL,
	 Lactose INT  NULL,
	 Carbs INT  NULL,
	 Protein INT  null,
	 Fat INT  Null,
	 Fibre INT  null,
    FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID)
);


--Member WorkOut Plan

CREATE TABLE MemberWorkout (
    MemberID INT not null,
	TrainerID INT not null,
  	Title VARCHAR(200) NULL,
	Goal VARCHAR(200) Not null,
	Experience VARCHAR(200) not null,
	Schedule VARCHAR(200) not null,
	FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID)

);

--Member Exercise Plan
CREATE TABLE MemberExercise (
    MemberID INT not null,
	TrainerID INT not null,
  	ExerciseName VARCHAR(200) not NULL,
	ESets INT null,
	EReps INT Null,
	TargetMuscle VARCHAR(200)  null,
    Machine VARCHAR(200)  null,
    RestInterval VARCHAR(200) not null,
	FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID)
);


CREATE TABLE Trainer (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FIRSTNAME VARCHAR(50) NOT NULL,
    LASTNAME VARCHAR(50) NOT NULL,
    DOB DATE NOT NULL,
    EMAIL VARCHAR(100) NOT NULL,
    GENDER VARCHAR(20) NOT NULL,
    PASSWORD VARCHAR(100) NOT NULL,
    Qualification VARCHAR(100) NOT NULL,
    Experience VARCHAR(100) NOT NULL,
    Status VARCHAR(100) NOT NULL DEFAULT 'Inactive',
    Speciality VARCHAR(100) NOT NULL
);

--trainer member
CREATE TABLE TrainerMembers (
    TrainerID INT,
    MemberID INT,
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID),
    FOREIGN KEY (MemberID) REFERENCES Member(ID)
);

--TrainerGetAddGym
CREATE TABLE TrainerGetAddGym (
    TrainerID INT,
    GymID INT,
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID),
    FOREIGN KEY (GymID) REFERENCES GYM(GYMID)
);

--Trainer Session and Appointment
CREATE TABLE TrainerSession (
     TrainerID INT not null,
	 MemberID INT not null,
     Duration VARCHAR(255) not null,
     Appointmentstatus VARCHAR(255) not null,
     FOREIGN KEY (MemberID) REFERENCES Member(ID),
     FOREIGN KEY (TrainerID) REFERENCES Trainer(ID),

);

CREATE TABLE GYMOWNER (
    OWNER_ID INT IDENTITY(1,1) PRIMARY KEY,
    FIRSTNAME VARCHAR(50) NOT NULL,
    LASTNAME VARCHAR(50) NOT NULL,
    EMAIL VARCHAR(100) NOT NULL UNIQUE,
    PASSWORD VARCHAR(100) NOT NULL,
    DOB DATE,
    GENDER VARCHAR(10)
);


-- Create LOCATION table
CREATE TABLE LOCATION (
    LOCATION_ID INT IDENTITY(1,1) PRIMARY KEY,
    STREETNO VARCHAR(50),
    AREACODE VARCHAR(50),
    CITY VARCHAR(50)
);

-- Create GYM table
CREATE TABLE GYM (
    GYMID INT IDENTITY(1,1) PRIMARY KEY,
    GYMNAME VARCHAR(100),
    OWNERID INT,
    LOCATION_ID INT,
	STATUS VARCHAR(100) NOT NULL DEFAULT 'INActive',
    FOREIGN KEY (OWNERID) REFERENCES GYMOWNER(OWNER_ID),
    FOREIGN KEY (LOCATION_ID) REFERENCES LOCATION(LOCATION_ID)
);

CREATE TABLE GymTrainers (
   
	TrainerID INT,
    GymID INT,
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID),
    FOREIGN KEY (GymID) REFERENCES GYM(GYMID)
);


CREATE TABLE GymMembers (
   
	MemberID INT,
    GymID INT,
    REGISTRATIONDATE DATE NOT NULL,
    FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (GymID) REFERENCES GYM(GYMID)
);

CREATE TABLE ADMIN (
    ADMIN_ID INT IDENTITY(1,1) PRIMARY KEY,
    USERNAME VARCHAR(50) NOT NULL,
    PASSWORD VARCHAR(100) NOT NULL,
    EMAIL VARCHAR(100),
);

-- Create GYM_REQUEST table
CREATE TABLE GYM_REQUEST (
    REQ_ID INT IDENTITY (1,1) PRIMARY KEY,
    GYM_ID INT,
    OWNER_ID INT,
    REQ_DATE DATETIME,
    STATUS VARCHAR(100),

	FOREIGN KEY (GYM_ID) REFERENCES GYM(GYMID),
    FOREIGN KEY (OWNER_ID) REFERENCES GYMOWNER(OWNER_ID)
);


--------------------------------------------------TRIGGERS----------------------------------------------------


--Trigger for Added Member Trainer->Member
CREATE TABLE AuditTrailMember (
    AuditID INT PRIMARY KEY IDENTITY(1,1),
    MemberID INT,
    TrainerID INT,
    Description VARCHAR(255),
    Timestamp DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID)
);

drop table AuditTrailMember;
CREATE TRIGGER trg_AuditTrainerMembers
ON TrainerMembers
AFTER INSERT
AS
BEGIN
    INSERT INTO AuditTrailMember (MemberID, TrainerID, Description)
    SELECT MemberID, TrainerID,'Trainer added member to training list'
    FROM inserted;
END

Select* from AuditTrailMember;

--Audit Trail Appointment -> Trainer -> Member
CREATE TABLE AuditTrailAppointment (
    AuditID INT PRIMARY KEY IDENTITY(1,1),
    MemberID INT,
    TrainerID INT,
    Description VARCHAR(255),
    Timestamp DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID)
);

drop table AuditTrailAppointment;
CREATE TRIGGER trg_AuditAppointment
ON TrainerSession
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @actionTaken VARCHAR(50)

    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
        SET @actionTaken = 'created'
    ELSE IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @actionTaken = 'updated'

    INSERT INTO AuditTrailAppointment (MemberID, TrainerID, Description)
    SELECT MemberID, TrainerID, 'Trainer ' + CAST(TrainerID AS VARCHAR(10)) + ' ' + @actionTaken + ' an appointment with Member ' + CAST(MemberID AS VARCHAR(10))
    FROM inserted;
END


Select* from AuditTrailAppointment;

--AuditTrial Feeback Member -> Trainer
CREATE TABLE AuditTrailFeedback (
    AuditID INT PRIMARY KEY IDENTITY(1,1),
    MemberID INT,
    TrainerID INT,
    Description VARCHAR(255),
    ActionTaken VARCHAR(50),
    Timestamp DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID)
);

drop table AuditTrailFeedback;
CREATE TRIGGER trg_AuditFeedback
ON MemberFeedback
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @actionTaken VARCHAR(50)

    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
        SET @actionTaken = 'Insert'
    ELSE IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @actionTaken = 'Update'

    INSERT INTO AuditTrailFeedback (MemberID, TrainerID, Description, ActionTaken)
    SELECT MemberID, TrainerID, 
           'Feedback ' + @actionTaken + ' for Member ' + CAST(MemberID AS VARCHAR(10)) + ' by Trainer ' + CAST(TrainerID AS VARCHAR(10)), 
           @actionTaken
    FROM inserted;
END

Select* from AuditTrailFeedback;


--Gym Members
CREATE TABLE AuditGymMembers (
    AuditID INT PRIMARY KEY IDENTITY(1,1),
    MemberID INT,
    GymID INT,
    Description VARCHAR(255),
    Timestamp DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MemberID) REFERENCES Member(ID),
    FOREIGN KEY (GymID) REFERENCES GYM(GYMID)
);

CREATE TRIGGER trg_AuditGymMembers
ON GymMembers
AFTER INSERT
AS
BEGIN
    INSERT INTO AuditGymMembers (MemberID, GymID, Description)
    SELECT MemberID, GymID, 'Member ID ' + CAST(MemberID AS VARCHAR) + ' added to Gym ID ' + CAST(GymID AS VARCHAR)
    FROM inserted;
END

Select* from AuditGymMembers;
--Gym Trainers
CREATE TABLE AuditGymTrainers (
    AuditID INT PRIMARY KEY IDENTITY(1,1),
    TrainerID INT,
    GymID INT,
    Description VARCHAR(255),
    Timestamp DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TrainerID) REFERENCES Trainer(ID),
    FOREIGN KEY (GymID) REFERENCES GYM(GYMID)
);

CREATE TRIGGER trg_AuditGymTrainers
ON GymTrainers
AFTER INSERT
AS
BEGIN
    INSERT INTO AuditGymTrainers (TrainerID, GymID, Description)
    SELECT TrainerID, GymID, 'Trainer ID ' + CAST(TrainerID AS VARCHAR) + ' added to Gym ID ' + CAST(GymID AS VARCHAR)
    FROM inserted;
END

Select* from AuditGymTrainers;

--------------------------------------------------DROP--------------------------------------------------


DROP TABLE IF EXISTS Member;
DROP TABLE IF EXISTS MEMBERSHIP_TYPE;
DROP TABLE IF EXISTS MemberGetTrained;
DROP TABLE IF EXISTS MemberGetAddGym;
DROP TABLE IF EXISTS MemberFeedback;
DROP TABLE IF EXISTS MemberDietPlan;
DROP TABLE IF EXISTS MemberWorkout;
DROP TABLE IF EXISTS MemberExercise;
DROP TABLE IF EXISTS Trainer;
DROP TABLE IF EXISTS TrainerMembers;
DROP TABLE IF EXISTS TrainerGetAddGym;
DROP TABLE IF EXISTS TrainerSession;
DROP TABLE IF EXISTS GYMOWNER;
DROP TABLE IF EXISTS LOCATION;
DROP TABLE IF EXISTS GYM;
DROP TABLE IF EXISTS GymTrainers;
DROP TABLE IF EXISTS GymMembers;
DROP TABLE IF EXISTS ADMIN;
DROP TABLE IF EXISTS GYM_REQUEST;
DROP TABLE IF EXISTS REQUEST_APPROVAL;
DROP TABLE IF EXISTS AuditTrailMember;
DROP TABLE IF EXISTS AuditTrailAppointment;
DROP TABLE IF EXISTS AuditTrailFeedback;
DROP TABLE IF EXISTS AuditGymMembers;
DROP TABLE IF EXISTS AuditGymTrainers;

---------------------------------------Resetting identities for all tables-------------------------------------------
DBCC CHECKIDENT ('Member', RESEED, 1);
DBCC CHECKIDENT ('MEMBERSHIP_TYPE', RESEED, 1);
DBCC CHECKIDENT ('MemberGetTrained', RESEED, 1);
DBCC CHECKIDENT ('MemberGetAddGym', RESEED, 1);
DBCC CHECKIDENT ('MemberFeedback', RESEED, 1);
DBCC CHECKIDENT ('MemberDietPlan', RESEED, 1);
DBCC CHECKIDENT ('MemberWorkout', RESEED, 1);
DBCC CHECKIDENT ('MemberExercise', RESEED, 1);
DBCC CHECKIDENT ('Trainer', RESEED, 1);
DBCC CHECKIDENT ('TrainerMembers', RESEED, 1);
DBCC CHECKIDENT ('TrainerGetAddGym', RESEED, 1);
DBCC CHECKIDENT ('TrainerSession', RESEED, 1);
DBCC CHECKIDENT ('GYMOWNER', RESEED, 1);
DBCC CHECKIDENT ('LOCATION', RESEED, 1);
DBCC CHECKIDENT ('GYM', RESEED, 1);
DBCC CHECKIDENT ('GymTrainers', RESEED, 1);
DBCC CHECKIDENT ('GymMembers', RESEED, 1);
DBCC CHECKIDENT ('ADMIN', RESEED, 1);
DBCC CHECKIDENT ('GYM_REQUEST', RESEED, 1);
DBCC CHECKIDENT ('AuditTrailMember', RESEED, 1);
DBCC CHECKIDENT ('AuditTrailAppointment', RESEED, 1);
DBCC CHECKIDENT ('AuditTrailFeedback', RESEED, 1);
DBCC CHECKIDENT ('AuditGymMembers', RESEED, 1);
DBCC CHECKIDENT ('AuditGymTrainers', RESEED, 1);


---------------------------------------------CHECK TABLES----------------------------------------------------

SELECT * FROM Member;
SELECT * FROM MEMBERSHIP_TYPE;
SELECT * FROM MemberGetTrained;
SELECT * FROM MemberGetAddGym;
SELECT * FROM MemberFeedback;
SELECT * FROM MemberDietPlan;
SELECT * FROM MemberWorkout;
SELECT * FROM MemberExercise;
SELECT * FROM Trainer;
SELECT * FROM TrainerMembers;
SELECT * FROM TrainerGetAddGym;
SELECT * FROM TrainerSession;
SELECT * FROM GYMOWNER;
SELECT * FROM LOCATION;
SELECT * FROM GYM;
SELECT * FROM GymTrainers;
SELECT * FROM GymMembers;
SELECT * FROM ADMIN;
SELECT * FROM GYM_REQUEST;
SELECT * FROM AuditTrailMember;
SELECT * FROM AuditTrailAppointment;
SELECT * FROM AuditTrailFeedback;
SELECT * FROM AuditGymMembers;
SELECT * FROM AuditGymTrainers;



------------------------------------------------------INSERTIONS----------------------------------------------

--------------------------------------------------------------------------------------------------------------
-------------------------------------------MEMBER-------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------

INSERT INTO Member (ID, FIRSTNAME, LASTNAME, DOB, EMAIL, GENDER, MEMBERSHIPTYPE, REGISTRATIONDATE, PASSWORD, STATUS)
VALUES 
(1, 'M1', 'm', '2003-01-01', 'm1@m1.com', 'Male', 'Basic Membership', '2023-01-01', '111', 'Active'),
(2, 'M2', 'm', '2003-01-02', 'm2@m1.com', 'Female', 'Premium Membership', '2023-01-02', '112', 'Active'),
(3, 'M3', 'm', '2003-01-03', 'm3@m1.com', 'Male', 'Standard Membership', '2023-01-03', '113', 'Active'),
(4, 'M4', 'm', '2003-01-04', 'm4@m1.com', 'Female', 'Basic Membership', '2023-01-04', '114', 'Active'),
(5, 'M5', 'm', '2003-01-05', 'm5@m1.com', 'Male', 'Premium Membership', '2023-01-05', '115', 'Active'),
(6, 'M6', 'm', '2003-01-06', 'm6@m1.com', 'Female', 'Standard Membership', '2023-01-06', '116', 'Active'),
(7, 'M7', 'm', '2003-01-07', 'm7@m1.com', 'Male', 'Basic Membership', '2023-01-07', '117', 'Active'),
(8, 'M8', 'm', '2003-01-08', 'm8@m1.com', 'Female', 'Premium Membership', '2023-01-08', '118', 'Active'),
(9, 'M9', 'm', '2003-01-09', 'm9@m1.com', 'Male', 'Standard Membership', '2023-01-09', '119', 'Active'),
(10, 'M10', 'm', '2003-01-10', 'm10@m1.com', 'Female', 'Basic Membership', '2023-01-10', '120', 'Active'),
(11, 'M11', 'm', '2003-01-11', 'm11@m1.com', 'Male', 'Premium Membership', '2023-01-11', '121', 'Active'),
(12, 'M12', 'm', '2003-01-12', 'm12@m1.com', 'Female', 'Standard Membership', '2023-01-12', '122', 'Active'),
(13, 'M13', 'm', '2003-01-13', 'm13@m1.com', 'Male', 'Basic Membership', '2023-01-13', '123', 'Active'),
(14, 'M14', 'm', '2003-01-14', 'm14@m1.com', 'Female', 'Premium Membership', '2023-01-14', '124', 'Active'),
(15, 'M15', 'm', '2003-01-15', 'm15@m1.com', 'Male', 'Standard Membership', '2023-01-15', '125', 'Active'),
(16,'M16', 'm', '2003-01-16', 'm16@m1.com', 'Female', 'Basic Membership', '2023-01-16', '126', 'Active'),
(17,'M17', 'm', '2003-01-17', 'm17@m1.com', 'Male', 'Premium Membership', '2023-01-17', '127', 'Active'),
(18,'M18', 'm', '2003-01-18', 'm18@m1.com', 'Female', 'Standard Membership', '2023-01-18', '128', 'Active'),
(19,'M19', 'm', '2003-01-19', 'm19@m1.com', 'Male', 'Basic Membership', '2023-01-19', '129', 'Active'),
(20,'M20', 'm', '2003-01-20', 'm20@m1.com', 'Female', 'Premium Membership', '2023-01-20', '130', 'Active'),
(21,'M21', 'm', '2003-01-21', 'm21@m1.com', 'Male', 'Standard Membership', '2023-01-21', '131', 'Active'),
(22,'M22', 'm', '2003-01-22', 'm22@m1.com', 'Female', 'Basic Membership', '2023-01-22', '132', 'Active'),
(23,'M23', 'm', '2003-01-23', 'm23@m1.com', 'Male', 'Premium Membership', '2023-01-23', '133', 'Active'),
(24,'M24', 'm', '2003-01-24', 'm24@m1.com', 'Female', 'Standard Membership', '2023-01-24', '134', 'Active'),
(25,'M25', 'm', '2003-01-25', 'm25@m1.com', 'Male', 'Basic Membership', '2023-01-25', '135', 'Active'),
(26,'M26', 'm', '2003-01-26', 'm26@m1.com', 'Female', 'Premium Membership', '2023-01-26', '136', 'Active'),
(27,'M27', 'm', '2003-01-27', 'm27@m1.com', 'Male', 'Standard Membership', '2023-01-27', '137', 'Active'),
(28,'M28', 'm', '2003-01-28', 'm28@m1.com', 'Female', 'Basic Membership', '2023-01-28', '138', 'Active'),
(29,'M29', 'm', '2003-01-29', 'm29@m1.com', 'Male', 'Premium Membership', '2023-01-29', '139', 'Active'),
(30,'M30', 'm', '2003-01-30', 'm30@m1.com', 'Female', 'Standard Membership', '2023-01-30', '140', 'Active'),
(31,'M31', 'm', '2003-01-31', 'm31@m1.com', 'Male', 'Basic Membership', '2023-01-31', '141', 'Active'),
(32,'M32', 'm', '2003-02-01', 'm32@m1.com', 'Female', 'Premium Membership', '2023-02-01', '142', 'Active'),
(33,'M33', 'm', '2003-02-02', 'm33@m1.com', 'Male', 'Standard Membership', '2023-02-02', '143', 'Active'),
(34,'M34', 'm', '2003-02-03', 'm34@m1.com', 'Female', 'Basic Membership', '2023-02-03', '144', 'Active'),
(35,'M35', 'm', '2003-02-04', 'm35@m1.com', 'Male', 'Premium Membership', '2023-02-04', '145', 'Active'),
(36,'M36', 'm', '2003-02-05', 'm36@m1.com', 'Female', 'Standard Membership', '2023-02-05', '146', 'Active'),
(37,'M37', 'm', '2003-02-06', 'm37@m1.com', 'Male', 'Basic Membership', '2023-02-06', '147', 'Active'),
(38,'M38', 'm', '2003-02-07', 'm38@m1.com', 'Female', 'Premium Membership', '2023-02-07', '148', 'Active'),
(39,'M39', 'm', '2003-02-08', 'm39@m1.com', 'Male', 'Standard Membership', '2023-02-08', '149', 'Active'),
(40,'M40', 'm', '2003-02-09', 'm40@m1.com', 'Female', 'Basic Membership', '2023-02-09', '150', 'Active'),
(41,'M41', 'm', '2003-02-10', 'm41@m1.com', 'Male', 'Premium Membership', '2023-02-10', '151', 'Active'),
(42,'M42', 'm', '2003-02-11', 'm42@m1.com', 'Female', 'Standard Membership', '2023-02-11', '152', 'Active'),
(43,'M43', 'm', '2003-02-12', 'm43@m1.com', 'Male', 'Basic Membership', '2023-02-12', '153', 'Active'),
(44,'M44', 'm', '2003-02-13', 'm44@m1.com', 'Female', 'Premium Membership', '2023-02-13', '154', 'Active'),
(45,'M45', 'm', '2003-02-14', 'm45@m1.com', 'Male', 'Standard Membership', '2023-02-14', '155', 'Active'),
(46,'M46', 'm', '2003-02-15', 'm46@m1.com', 'Female', 'Basic Membership', '2023-02-15', '156', 'Active'),
(47,'M47', 'm', '2003-02-16', 'm47@m1.com', 'Male', 'Premium Membership', '2023-02-16', '157', 'Active'),
(48,'M48', 'm', '2003-02-17', 'm48@m1.com', 'Female', 'Standard Membership', '2023-02-17', '158', 'Active'),
(49,'M49', 'm', '2003-02-18', 'm49@m1.com', 'Male', 'Basic Membership', '2023-02-18', '159', 'Active'),
(50, 'M50', 'm', '2003-02-19', 'm50@m1.com', 'Female', 'Premium Membership', '2023-02-19', '160', 'Active');

--------------------------------------------------------------------------------------------------------------
--------------------------------------------------TRAINER--------------------------------------------------------
--------------------------------------------------------------------------------------------------------------
INSERT INTO Trainer (ID, FIRSTNAME, LASTNAME, DOB, EMAIL, GENDER, PASSWORD, Qualification, Experience, Status, Speciality)
VALUES 
(1, 'T1', 't', '1980-01-01', 't1@example.com', 'Male', '111', 'Certified Fitness Coach', '10 years', 'Active', 'Weightlifting'),
(2, 'T2', 't', '1980-01-02', 't2@example.com', 'Female', '112', 'Certified Fitness Coach', '9 years', 'Active', 'Cardio'),
(3, 'T3', 't', '1980-01-03', 't3@example.com', 'Male', '113', 'Certified Fitness Coach', '8 years', 'Active', 'Nutrition'),
(4, 'T4', 't', '1980-01-04', 't4@example.com', 'Female', '114', 'Certified Fitness Coach', '7 years', 'Active', 'Yoga'),
(5, 'T5', 't', '1980-01-05', 't5@example.com', 'Male', '115', 'Certified Fitness Coach', '6 years', 'Active', 'Crossfit'),
(6, 'T6', 't', '1980-01-06', 't6@example.com', 'Female', '116', 'Certified Fitness Coach', '5 years', 'Active', 'Aerobics'),
(7, 'T7', 't', '1980-01-07', 't7@example.com', 'Male', '117', 'Certified Fitness Coach', '4 years', 'Active', 'Bodybuilding'),
(8, 'T8', 't', '1980-01-08', 't8@example.com', 'Female', '118', 'Certified Fitness Coach', '10 years', 'Active', 'Pilates'),
(9, 'T9', 't', '1980-01-09', 't9@example.com', 'Male', '119', 'Certified Fitness Coach', '9 years', 'Active', 'Stretching'),
(10, 'T10', 't', '1980-01-10', 't10@example.com', 'Female', '120', 'Certified Fitness Coach', '8 years', 'Active', 'Kickboxing'),
(11, 'T11', 't', '1980-01-11', 't11@example.com', 'Male', '121', 'Certified Fitness Coach', '7 years', 'Active', 'Rehabilitation'),
(12, 'T12', 't', '1980-01-12', 't12@example.com', 'Female', '122', 'Certified Fitness Coach', '6 years', 'Active', 'Strength Training'),
(13, 'T13', 't', '1980-01-13', 't13@example.com', 'Male', '123', 'Certified Fitness Coach', '5 years', 'Active', 'Running'),
(14, 'T14', 't', '1980-01-14', 't14@example.com', 'Female', '124', 'Certified Fitness Coach', '4 years', 'Active', 'Cycling'),
(15, 'T15', 't', '1980-01-15', 't15@example.com', 'Male', '125', 'Certified Fitness Coach', '3 years', 'Active', 'Triathlon Training'),
(16, 'T16', 't', '1980-01-16', 't16@example.com', 'Female', '126', 'Certified Fitness Coach', '2 years', 'Active', 'Zumba'),
(17, 'T17', 't', '1980-01-17', 't17@example.com', 'Male', '127', 'Certified Fitness Coach', '1 year', 'Active', 'HIIT'),
(18, 'T18', 't', '1980-01-18', 't18@example.com', 'Female', '128', 'Certified Fitness Coach', '10 years', 'Active', 'Martial Arts'),
(19, 'T19', 't', '1980-01-19', 't19@example.com', 'Male', '129', 'Certified Fitness Coach', '9 years', 'Active', 'Powerlifting'),
(20, 'T20', 't', '1980-01-20', 't20@example.com', 'Female', '130', 'Certified Fitness Coach', '8 years', 'Active', 'Dance Fitness'),
(21, 'T21', 't', '1980-01-21', 't21@example.com', 'Male', '131', 'Certified Fitness Coach', '7 years', 'Active', 'Swimming Coaching'),
(22, 'T22', 't', '1980-01-22', 't22@example.com', 'Female', '132', 'Certified Fitness Coach', '6 years', 'Active', 'Boxing'),
(23, 'T23', 't', '1980-01-23', 't23@example.com', 'Male', '133', 'Certified Fitness Coach', '5 years', 'Active', 'Gymnastics'),
(24, 'T24', 't', '1980-01-24', 't24@example.com', 'Female', '134', 'Certified Fitness Coach', '4 years', 'Active', 'Wellness Coaching'),
(25, 'T25', 't', '1980-01-25', 't25@example.com', 'Male', '135', 'Certified Fitness Coach', '3 years', 'Active', 'Sport Specific Training'),
(26, 'T26', 't', '1980-01-26', 't26@example.com', 'Female', '136', 'Certified Fitness Coach', '2 years', 'Active', 'Personal Training'),
(27, 'T27', 't', '1980-01-27', 't27@example.com', 'Male', '137', 'Certified Fitness Coach', '1 year', 'Active', 'Senior Fitness'),
(28, 'T28', 't', '1980-01-28', 't28@example.com', 'Female', '138', 'Certified Fitness Coach', '10 years', 'Active', 'Pre/Post Natal Fitness'),
(29, 'T29', 't', '1980-01-29', 't29@example.com', 'Male', '139', 'Certified Fitness Coach', '9 years', 'Active', 'Functional Training'),
(30, 'T30', 't', '1980-01-30', 't30@example.com', 'Female', '140', 'Certified Fitness Coach', '8 years', 'Active', 'Sports Nutrition'),
(31, 'T31', 't', '1980-01-31', 't31@example.com', 'Male', '141', 'Certified Fitness Coach', '7 years', 'Active', 'Obstacle Course Training'),
(32, 'T32', 't', '1980-02-01', 't32@example.com', 'Female', '142', 'Certified Fitness Coach', '6 years', 'Active', 'Kettlebell Training'),
(33, 'T33', 't', '1980-02-02', 't33@example.com', 'Male', '143', 'Certified Fitness Coach', '5 years', 'Active', 'Sports Performance'),
(34, 'T34', 't', '1980-02-03', 't34@example.com', 'Female', '144', 'Certified Fitness Coach', '4 years', 'Active', 'Core Training'),
(35, 'T35', 't', '1980-02-04', 't35@example.com', 'Male', '145', 'Certified Fitness Coach', '3 years', 'Active', 'Resistance Training'),
(36, 'T36', 't', '1980-02-05', 't36@example.com', 'Female', '146', 'Certified Fitness Coach', '2 years', 'Active', 'Mobility Training'),
(37, 'T37', 't', '1980-02-06', 't37@example.com', 'Male', '147', 'Certified Fitness Coach', '1 year', 'Active', 'Aquatic Fitness'),
(38, 'T38', 't', '1980-02-07', 't38@example.com', 'Female', '148', 'Certified Fitness Coach', '10 years', 'Active', 'Spin Classes'),
(39, 'T39', 't', '1980-02-08', 't39@example.com', 'Male', '149', 'Certified Fitness Coach', '9 years', 'Active', 'Barre Classes'),
(40, 'T40', 't', '1980-02-09', 't40@example.com', 'Female', '150', 'Certified Fitness Coach', '8 years', 'Active', 'Parkour'),
(41, 'T41', 't', '1980-02-10', 't41@example.com', 'Male', '151', 'Certified Fitness Coach', '7 years', 'Active', 'Capoeira'),
(42, 'T42', 't', '1980-02-11', 't42@example.com', 'Female', '152', 'Certified Fitness Coach', '6 years', 'Active', 'Mountain Biking'),
(43, 'T43', 't', '1980-02-12', 't43@example.com', 'Male', '153', 'Certified Fitness Coach', '5 years', 'Active', 'Skiing and Snowboarding Coaching'),
(44, 'T44', 't', '1980-02-13', 't44@example.com', 'Female', '154', 'Certified Fitness Coach', '4 years', 'Active', 'Rock Climbing'),
(45, 'T45', 't', '1980-02-14', 't45@example.com', 'Male', '155', 'Certified Fitness Coach', '3 years', 'Active', 'Rollerblading and Skating'),
(46, 'T46', 't', '1980-02-15', 't46@example.com', 'Female', '156', 'Certified Fitness Coach', '2 years', 'Active', 'Surfing Instruction'),
(47, 'T47', 't', '1980-02-16', 't47@example.com', 'Male', '157', 'Certified Fitness Coach', '1 year', 'Active', 'Adventure Racing'),
(48, 'T48', 't', '1980-02-17', 't48@example.com', 'Female', '158', 'Certified Fitness Coach', '10 years', 'Active', 'Motor Sports Coaching'),
(49, 'T49', 't', '1980-02-18', 't49@example.com', 'Male', '159', 'Certified Fitness Coach', '9 years', 'Active', 'Hiking Guide'),
(50, 'T50', 't', '1980-02-19', 't50@example.com', 'Female', '160', 'Certified Fitness Coach', '8 years', 'Active', 'Snowshoeing');
--------------------------------------------------------------------------------------------------------------
------------------------------------------------LOCATION------------------------------------------------------
--------------------------------------------------------------------------------------------------------------
INSERT INTO LOCATION (LOCATION_ID, STREETNO, AREACODE, CITY)
VALUES 
(1, '123 Main St', '111', 'New York'),
(2, '124 Main St', '112', 'Los Angeles'),
(3, '125 Main St', '113', 'Chicago'),
(4, '126 Main St', '114', 'Houston'),
(5, '127 Main St', '115', 'Phoenix'),
(6, '128 Main St', '116', 'Philadelphia'),
(7, '129 Main St', '117', 'San Antonio'),
(8, '130 Main St', '118', 'San Diego'),
(9, '131 Main St', '119', 'Dallas'),
(10, '132 Main St', '120', 'San Jose'),
(11, '133 Main St', '121', 'Austin'),
(12, '134 Main St', '122', 'Jacksonville'),
(13, '135 Main St', '123', 'Fort Worth'),
(14, '136 Main St', '124', 'Columbus'),
(15, '137 Main St', '125', 'Charlotte'),
(16, '138 Main St', '126', 'San Francisco'),
(17, '139 Main St', '127', 'Indianapolis'),
(18, '140 Main St', '128', 'Seattle'),
(19, '141 Main St', '129', 'Denver'),
(20, '142 Main St', '130', 'Washington'),
(21, '143 Main St', '131', 'Boston'),
(22, '144 Main St', '132', 'El Paso'),
(23, '145 Main St', '133', 'Nashville'),
(24, '146 Main St', '134', 'Detroit'),
(25, '147 Main St', '135', 'Oklahoma City'),
(26, '148 Main St', '136', 'Portland'),
(27, '149 Main St', '137', 'Las Vegas'),
(28, '150 Main St', '138', 'Memphis'),
(29, '151 Main St', '139', 'Louisville'),
(30, '152 Main St', '140', 'Baltimore');

---------------------------------------------------------------------------------------------------------------
----------------------------------------------------OWNER--------------------------------------------------------
---------------------------------------------------------------------------------------------------------------

-- Inserting 30 gym owners into the GYMOWNER table
INSERT INTO GYMOWNER (FIRSTNAME, LASTNAME, EMAIL, PASSWORD, DOB, GENDER)
VALUES 
('O1', 'o', 'o1@example.com', '111', '1980-01-01', 'Male'),
('O2', 'o', 'o2@example.com', '112', '1981-02-01', 'Female'),
('O3', 'o', 'o3@example.com', '113', '1982-03-01', 'Male'),
('O4', 'o', 'o4@example.com', '114', '1983-04-01', 'Female'),
('O5', 'o', 'o5@example.com', '115', '1984-05-01', 'Male'),
('O6', 'o', 'o6@example.com', '116', '1985-06-01', 'Female'),
('O7', 'o', 'o7@example.com', '117', '1986-07-01', 'Male'),
('O8', 'o', 'o8@example.com', '118', '1987-08-01', 'Female'),
('O9', 'o', 'o9@example.com', '119', '1988-09-01', 'Male'),
('O10', 'o', 'o10@example.com', '120', '1989-10-01', 'Female'),
('O11', 'o', 'o11@example.com', '121', '1990-11-01', 'Male'),
('O12', 'o', 'o12@example.com', '122', '1991-12-01', 'Female'),
('O13', 'o', 'o13@example.com', '123', '1992-01-01', 'Male'),
('O14', 'o', 'o14@example.com', '124', '1993-02-01', 'Female'),
('O15', 'o', 'o15@example.com', '125', '1994-03-01', 'Male'),
('O16', 'o', 'o16@example.com', '126', '1995-04-01', 'Female'),
('O17', 'o', 'o17@example.com', '127', '1996-05-01', 'Male'),
('O18', 'o', 'o18@example.com', '128', '1997-06-01', 'Female'),
('O19', 'o', 'o19@example.com', '129', '1998-07-01', 'Male'),
('O20', 'o', 'o20@example.com', '130', '1999-08-01', 'Female'),
('O21', 'o', 'o21@example.com', '131', '2000-09-01', 'Male'),
('O22', 'o', 'o22@example.com', '132', '2001-10-01', 'Female'),
('O23', 'o', 'o23@example.com', '133', '2002-11-01', 'Male'),
('O24', 'o', 'o24@example.com', '134', '2003-12-01', 'Female'),
('O25', 'o', 'o25@example.com', '135', '2004-01-01', 'Male'),
('O26', 'o', 'o26@example.com', '136', '2005-02-01', 'Female'),
('O27', 'o', 'o27@example.com', '137', '2006-03-01', 'Male'),
('O28', 'o', 'o28@example.com', '138', '2007-04-01', 'Female'),
('O29', 'o', 'o29@example.com', '139', '2008-05-01', 'Male'),
('O30', 'o', 'o30@example.com', '140', '2009-06-01', 'Female');


---------------------------------------------------------------------------------------------------------------
----------------------------------------------------GYM'S--------------------------------------------------------
---------------------------------------------------------------------------------------------------------------


INSERT INTO GYM (GYMID, GYMNAME, OWNERID, LOCATION_ID, STATUS)
VALUES 
(1, 'G1', 1, 1, 'Active'),
(2, 'G2', 2, 2, 'Active'),
(3, 'G3', 1, 3, 'Active'),
(4, 'G4', 3, 4, 'Active'),
(5, 'G5', 1, 5, 'Active'),
(6, 'G6', 4, 6, 'Active'),
(7, 'G7', 7, 7, 'Active'),
(8, 'G8', 5, 8, 'Active'),
(9, 'G9', 8, 9, 'Active'),
(10, 'G10', 9, 10, 'Active'),
(11, 'G11', 9, 11, 'Active'),
(12, 'G12', 1, 12, 'Active'),
(13, 'G13', 1, 13, 'Active'),
(14, 'G14', 1, 14, 'Active'),
(15, 'G15', 11, 15, 'Active'),
(16, 'G16', 12, 16, 'Active'),
(17, 'G17', 13, 17, 'Active'),
(18, 'G18', 14, 18, 'Active'),
(19, 'G19', 18, 19, 'Active'),
(20, 'G20', 29, 20, 'Active'),
(21, 'G21', 21, 21, 'Active'),
(22, 'G22', 22, 22, 'Active'),
(23, 'G23', 26, 23, 'Active'),
(24, 'G24', 24, 24, 'Active'),
(25, 'G25', 25, 25, 'Active'),
(26, 'G26', 2, 26, 'Active'),
(27, 'G27', 2, 27, 'Active'),
(28, 'G28', 2, 28, 'Active'),
(29, 'G29', 29, 29, 'Active'),
(30, 'G30', 3, 30, 'Active');

----------------------------------------------------------------------------------------------------------
--------------------------------------------TRAINER GYMS--------------------------------------------------
----------------------------------------------------------------------------------------------------------

INSERT INTO GymTrainers (TrainerID, GymID) VALUES (1, 1);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (2, 1);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (3, 2);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (4, 2);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (5, 3);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (6, 3);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (7, 4);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (8, 4);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (9, 5);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (10, 5);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (11, 6);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (12, 6);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (13, 7);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (14, 7);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (15, 8);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (16, 8);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (17, 9);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (18, 9);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (19, 10);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (20, 10);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (21, 11);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (22, 11);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (23, 12);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (24, 12);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (25, 13);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (26, 13);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (27, 14);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (28, 14);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (29, 15);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (30, 15);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (31, 16);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (32, 16);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (33, 17);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (34, 17);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (35, 18);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (36, 18);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (37, 19);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (38, 19);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (39, 20);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (40, 20);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (41, 21);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (42, 21);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (43, 22);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (44, 22);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (45, 23);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (46, 23);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (47, 24);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (48, 24);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (49, 25);
INSERT INTO GymTrainers (TrainerID, GymID) VALUES (50, 25);




---------------------------------------------------------------------------------------------------
----------------------------------------------GYM MEMBERS------------------------------------------
---------------------------------------------------------------------------------------------------


INSERT INTO GymMembers (MemberID, GymID, REGISTRATIONDATE)
VALUES
(1, 1, '2023-01-01'),
(2, 2, '2023-01-02'),
(3, 3, '2023-01-03'),
(4, 4, '2023-01-04'),
(5, 5, '2023-01-05'),
(6, 6, '2023-01-06'),
(7, 7, '2023-01-07'),
(8, 8, '2023-01-08'),
(9, 9, '2023-01-09'),
(10, 10, '2023-01-10'),
(11, 11, '2023-01-11'),
(12, 12, '2023-01-12'),
(13, 13, '2023-01-13'),
(14, 14, '2023-01-14'),
(15, 15, '2023-01-15'),
(16, 16, '2023-01-16'),
(17, 17, '2023-01-17'),
(18, 18, '2023-01-18'),
(19, 19, '2023-01-19'),
(20, 20, '2023-01-20'),
(21, 21, '2023-01-21'),
(22, 22, '2023-01-22'),
(23, 23, '2023-01-23'),
(24, 24, '2023-01-24'),
(25, 25, '2023-01-25'),
(26, 26, '2023-01-26'),
(27, 27, '2023-01-27'),
(28, 28, '2023-01-28'),
(29, 29, '2023-01-29'),
(30, 30, '2023-01-30'),
(31, 1, '2023-02-01'),
(32, 2, '2023-02-02'),
(33, 3, '2023-02-03'),
(34, 4, '2023-02-04'),
(35, 5, '2023-02-05'),
(36, 6, '2023-02-06'),
(37, 7, '2023-02-07'),
(38, 8, '2023-02-08'),
(39, 9, '2023-02-09'),
(40, 10, '2023-02-10'),
(41, 11, '2023-02-11'),
(42, 12, '2023-02-12'),
(43, 13, '2023-02-13'),
(44, 14, '2023-02-14'),
(45, 15, '2023-02-15'),
(46, 16, '2023-02-16'),
(47, 17, '2023-02-17'),
(48, 18, '2023-02-18'),
(49, 19, '2023-02-19'),
(50, 20, '2023-02-20');

-----------------------------------------------------TRIANERS MEMBERS----------------------------------------------

-- Insert manual entries into TrainerMembers
INSERT INTO TrainerMembers (TrainerID, MemberID)
VALUES
(1, 1), -- Trainer 1 at Gym 1 with Member 1
(1, 31), -- Trainer 1 at Gym 1 with Member 31
(2, 1), -- Trainer 2 at Gym 1 with Member 1
(2, 31), -- Trainer 2 at Gym 1 with Member 31

(3, 2), -- Trainer 3 at Gym 2 with Member 2
(3, 32), -- Trainer 3 at Gym 2 with Member 32
(4, 2), -- Trainer 4 at Gym 2 with Member 2
(4, 32), -- Trainer 4 at Gym 2 with Member 32

(5, 3), -- Trainer 5 at Gym 3 with Member 3
(5, 33), -- Trainer 5 at Gym 3 with Member 33
(6, 3), -- Trainer 6 at Gym 3 with Member 3
(6, 33), -- Trainer 6 at Gym 3 with Member 33

(7, 4), -- Trainer 7 at Gym 4 with Member 4
(7, 34), -- Trainer 7 at Gym 4 with Member 34
(8, 4), -- Trainer 8 at Gym 4 with Member 4
(8, 34), -- Trainer 8 at Gym 4 with Member 34

(9, 5), -- Trainer 9 at Gym 5 with Member 5
(9, 35), -- Trainer 9 at Gym 5 with Member 35
(10, 5), -- Trainer 10 at Gym 5 with Member 5
(10, 35), -- Trainer 10 at Gym 5 with Member 35

(11, 6), -- Trainer 11 at Gym 6 with Member 6
(11, 36), -- Trainer 11 at Gym 6 with Member 36
(12, 6), -- Trainer 12 at Gym 6 with Member 6
(12, 36), -- Trainer 12 at Gym 6 with Member 36

(13, 7), -- Trainer 13 at Gym 7 with Member 7
(13, 37), -- Trainer 13 at Gym 7 with Member 37
(14, 7), -- Trainer 14 at Gym 7 with Member 7
(14, 37); -- Trainer 14 at Gym 7 with Member 37

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------TRIANER SESSION------------------------------------------


-- Inserting session data for the assigned trainer-member pairs
INSERT INTO TrainerSession (TrainerID, MemberID, Duration, Appointmentstatus)
VALUES
(1, 1, '30 minutes', 'Scheduled'),
(1, 31, '45 minutes', 'Scheduled'),
(2, 1, '30 minutes', 'Scheduled'),
(2, 31, '45 minutes', 'Scheduled'),

(3, 2, '30 minutes', 'Scheduled'),
(3, 32, '45 minutes', 'Scheduled'),
(4, 2, '30 minutes', 'Scheduled'),
(4, 32, '45 minutes', 'Scheduled'),

(5, 3, '30 minutes', 'Scheduled'),
(5, 33, '45 minutes', 'Scheduled'),
(6, 3, '30 minutes', 'Scheduled'),
(6, 33, '45 minutes', 'Scheduled'),

(7, 4, '30 minutes', 'Scheduled'),
(7, 34, '45 minutes', 'Scheduled'),
(8, 4, '30 minutes', 'Scheduled'),
(8, 34, '45 minutes', 'Scheduled'),

(9, 5, '30 minutes', 'Scheduled'),
(9, 35, '45 minutes', 'Scheduled'),
(10, 5, '30 minutes', 'Scheduled'),
(10, 35, '45 minutes', 'Scheduled'),

(11, 6, '30 minutes', 'Scheduled'),
(11, 36, '45 minutes', 'Scheduled'),
(12, 6, '30 minutes', 'Scheduled'),
(12, 36, '45 minutes', 'Scheduled'),

(13, 7, '30 minutes', 'Scheduled'),
(13, 37, '45 minutes', 'Scheduled'),
(14, 7, '30 minutes', 'Scheduled'),
(14, 37, '45 minutes', 'Scheduled');

----------------------------------------------------FEEDBACK -------------------------------------------------

-- Inserting feedback from members to their trainers
INSERT INTO MemberFeedback (Rating, Comment, TrainerID, MemberID)
VALUES
(5, 'Outstanding.', 1, 1),
(4, 'Very knowledgeable.', 3, 2),
(3, 'Needs to improve.', 5, 3),
(5, 'Great.', 7, 4),
(2, 'Not very punctual.', 8,4),
(4, 'Excellent sessions.', 6, 33),
(5, 'Really good.', 4, 32),
(3, 'Good.', 2, 31),
(5, 'Perfect diet plan.', 14, 7),
(4, 'Very strict.',10, 35);


-------------------------------------------------WORKOUT PLAN--------------------------------------------------

INSERT INTO MemberWorkout (MemberID, TrainerID, Title, Goal, Experience, Schedule)
VALUES
(1, 1, 'Beginner Strength', 'Gain Muscle', 'Beginner', 'Mon, Wed, Fri'),
(2, 3, 'Cardio Blast', 'Lose Weight', 'Intermediate', 'Daily 30 mins'),
(3, 5, 'Flexibility Focus', 'Improve Flexibility', 'Beginner', 'Tue, Thu'),
(4, 7, 'Advanced Yoga', 'Enhance Balance', 'Advanced', 'Wed, Sat'),
(4, 8, 'Bootcamp', 'High Intensity', 'Advanced', 'Mon, Wed, Fri'),
(33, 6, 'Pilates Introduction', 'Core Strength', 'Beginner', 'Tue, Thu, Sat'),
(32, 4, 'Marathon Prep', 'Increase Stamina', 'Intermediate', 'Daily'),
(31, 2, 'Senior Fitness', 'Maintain Mobility', 'Senior', 'Mon, Wed, Fri'),
(7, 14, 'Teen Athlete', 'Sports Performance', 'Intermediate', 'Tue, Thu, Sat'),
(35, 10 , 'Pregnancy Yoga', 'Safe Exercise', 'Beginner', 'Mon, Wed');


--------------------------------------------------MEMBER EXERCISE------------------------------------------------

INSERT INTO MemberExercise (MemberID, TrainerID, ExerciseName, ESets, EReps, TargetMuscle, Machine, RestInterval)
VALUES
(1, 1, 'Squats', 3, 12, 'Legs', 'None', '2 mins'),
(2, 3, 'Bench Press', 4, 10, 'Chest', 'Bench Press', '3 mins'),
(3, 5, 'Deadlifts', 3, 8, 'Back', 'None', '2 mins'),
(4, 7, 'Pull-ups', 3, 10, 'Arms', 'Pull-up Bar', '1 min'),
(4, 8, 'Push-ups', 5, 20, 'Chest', 'None', '1 min'),
(33, 6, 'Leg Press', 4, 15, 'Legs', 'Leg Press Machine', '3 mins'),
(32, 4, 'Cycling', 1, 30, 'Cardio', 'Stationary Bike', 'None'),
(31, 2, 'Plank', 4, 60, 'Core', 'None', '1 min'),
(7, 14, 'Lunges', 5, 15, 'Legs', 'None', '2 mins'),
(35, 10, 'Rowing', 2, 15, 'Back', 'Rowing Machine', '2 mins');


------------------------------------------------MEMBER DIET PLAN------------------------------------------------
INSERT INTO MemberDietPlan (MemberID, TrainerID, Typeofdiet, Nutrition, Purpose, Peanuts, Gluten, Lactose, Carbs, Protein, Fat, Fibre)
VALUES
(1, 1, 'Low Carb', 'High Protein, Low Carb', 'Weight Loss', 0, 1, 0, 20, 50, 30, 5),
(2, 3, 'High Protein', 'High Protein', 'Muscle Gain', 0, 0, 1, 30, 50, 20, 5),
(3, 5, 'Vegan', 'Balanced', 'Health Maintenance', 0, 0, 0, 50, 20, 20, 10),
(4, 7, 'Mediterranean', 'Diverse', 'Heart Health', 0, 1, 0, 40, 30, 30, 5),
(4, 8, 'Paleo', 'Natural Foods', 'Energy Boost', 0, 0, 0, 30, 40, 30, 10),
(33, 6, 'Ketogenic', 'High Fat, Low Carb', 'Weight Loss', 1, 0, 0, 10, 20, 70, 5),
(32, 4, 'Balanced', 'Balanced Nutrients', 'General Health', 0, 0, 0, 40, 30, 30, 15),
(31, 2, 'Detox', 'Low Calorie, Nutrient Rich', 'Detoxification', 0, 1, 0, 25, 25, 10, 15),
(7, 13, 'Gluten-Free', 'No Gluten', 'Allergy Management', 0, 0, 1, 40, 30, 30, 10),
(35, 10, 'Intermittent Fasting', 'Varied', 'Weight Management', 0, 0, 0, 30, 20, 50, 10);