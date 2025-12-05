-- 4. “аблица RelatedProducts (если нужна дл€ дополнительных товаров)
CREATE TABLE RelatedProducts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MainProductId INT NOT NULL,
    RelatedProductId INT NOT NULL,
    FOREIGN KEY (MainProductId) REFERENCES Products(Id),
    FOREIGN KEY (RelatedProductId) REFERENCES Products(Id)
);
