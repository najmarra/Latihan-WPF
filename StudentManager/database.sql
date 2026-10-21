-- Opsional: aplikasi sudah membuat database ini otomatis saat pertama dijalankan.
-- Jalankan script ini hanya jika ingin membuatnya manual lewat SSMS / Visual Studio.

CREATE DATABASE StudentDB;
GO

USE StudentDB;
GO

CREATE TABLE Students
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NIM VARCHAR(20) NOT NULL,
    Nama VARCHAR(100) NOT NULL,
    Jurusan VARCHAR(100) NOT NULL,
    Gender VARCHAR(20) NOT NULL,
    Email VARCHAR(100)
);
GO

INSERT INTO Students (NIM, Nama, Jurusan, Gender, Email)
VALUES
('23001','Budi Santoso','Informatika','Laki-laki','budi@gmail.com'),
('23002','Siti Aminah','Sistem Informasi','Perempuan','siti@gmail.com'),
('23003','Andi Wijaya','Informatika','Laki-laki','andi@gmail.com'),
('23004','Rina Sari','Sistem Informasi','Perempuan','rina@gmail.com');
GO
