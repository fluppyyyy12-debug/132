-- 1. Таблица Products (если еще не создана)
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Manufacturer NVARCHAR(100),
    Price DECIMAL(10,2) NOT NULL,
    InStock INT NOT NULL DEFAULT 0,
    Image VARBINARY(MAX),
    CreatedDate DATETIME DEFAULT GETDATE()
);
