-- 2. Таблица Orders (для проверки заказов)
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderDate DATETIME DEFAULT GETDATE(),
    CustomerId INT,
    TotalAmount DECIMAL(10,2)
);