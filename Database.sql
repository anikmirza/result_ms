CREATE TABLE Class
(
    ClassId INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
    Name VARCHAR(50) NULL,
    Section VARCHAR(50) NULL,
    DateOfEntry DATETIME NOT NULL
)

CREATE TABLE Student
(
    StudentId INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
    ClassId INT NOT NULL,
    Name VARCHAR(50) NULL,
    Roll INT NOT NULL,
    Phone VARCHAR(50) NULL,
    Email VARCHAR(50) NULL,
    DateOfEntry DATETIME NOT NULL
)

CREATE TABLE Subject
(
    SubjectId INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
    Name VARCHAR(50) NULL,
    DateOfEntry DATETIME NOT NULL
)

CREATE TABLE Result
(
    ResultId INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
    StudentId INT NOT NULL,
    SubjectId INT NOT NULL,
    Mark FLOAT NULL,
    DateOfEntry DATETIME NOT NULL
)


ALTER TABLE Student WITH CHECK ADD CONSTRAINT FK_Student_ClassId
FOREIGN KEY (ClassId) REFERENCES Class(ClassId);

ALTER TABLE Result WITH CHECK ADD CONSTRAINT FK_Result_StudentId
FOREIGN KEY (StudentId) REFERENCES Student(StudentId);

ALTER TABLE Result WITH CHECK ADD CONSTRAINT FK_Result_SubjectId
FOREIGN KEY (SubjectId) REFERENCES Subject(SubjectId);


INSERT INTO Class (Name, Section, DateOfEntry) VALUES
('Class 1', 'Section A', GETDATE()),
('Class 1', 'Section B', GETDATE()),
('Class 2', 'Section A', GETDATE()),
('Class 2', 'Section B', GETDATE()),
('Class 3', 'Section A', GETDATE()),
('Class 3', 'Section B', GETDATE()),
('Class 4', 'Section A', GETDATE()),
('Class 4', 'Section B', GETDATE()),
('Class 5', 'Section A', GETDATE()),
('Class 5', 'Section B', GETDATE());
