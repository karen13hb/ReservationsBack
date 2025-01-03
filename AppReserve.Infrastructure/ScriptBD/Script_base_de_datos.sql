USE reserveCompanyDB;

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Spaces (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
	MinReservationDuration TIME,
    MaxReservationDuration TIME
);

CREATE TABLE Reservations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    SpaceId INT NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Reservations_Users FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Reservations_Spaces FOREIGN KEY (SpaceId) REFERENCES Spaces (Id) ON DELETE CASCADE,
    CONSTRAINT CH_Reservations_StartBeforeEnd CHECK (StartDate < EndDate)
);

CREATE INDEX IX_Reservations_SpaceId ON Reservations (SpaceId);
CREATE INDEX IX_Reservations_UserId ON Reservations (UserId);
CREATE INDEX IX_Reservations_StartDate_EndDate ON Reservations (StartDate, EndDate);


INSERT INTO Users (Name, Email) VALUES 
('Karen Herrera', 'karen@example.com'),
('Pepito Perez', 'pepito@example.com');


INSERT INTO Spaces  (Name,Description, MinReservationDuration, MaxReservationDuration)
VALUES 
    ( 'Sala A', 'Main building, ground floor', '01:00:00', '02:00:00'),
    ( 'Sala B', 'Main building, first floor', '00:30:00', '01:30:00'),
    ( 'Sala C', 'Main building, third floor' ,'01:00:00', '03:00:00');



INSERT INTO Reservations (UserId, SpaceId, StartDate, EndDate) VALUES 
(1, 1, '2024-12-01 09:00', '2024-12-01 11:00'),
(2, 2, '2024-12-01 13:00', '2024-12-01 14:00');

select * from Reservations
select * from Spaces
select * from Users
delete from Reservations
delete from Users
delete from Spaces



