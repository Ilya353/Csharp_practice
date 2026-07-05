USE AutoPartsStore
GO
--Добавление клиентов.

INSERT INTO Client (Name, Phone, Email, IsActive)
VALUES 
    ('Алексей Смирнов', '79161234567', 'alex.smirnov@mail.ru', 1),
    ('Ольга Кузнецова', '79162345678', 'olga.kuz@mail.ru', 1),
    ('Дмитрий Попов', '79163456789', 'dima.popov@mail.ru', 1),
    ('Анна Васильева', '79164567890', 'anna.vas@mail.ru', 0),
    ('Иван Морозов', '79165678901', 'ivan.morozov@mail.ru', 1),
    ('Елена Соколова', '79166789012', 'elena.sokol@mail.ru', 1),
    ('Павел Новиков', '79167890123', 'pavel.nov@mail.ru', 0),
    ('Мария Федорова', '79168901234', 'maria.fed@mail.ru', 1),
    ('Сергей Орлов', '79169012345', 'sergey.orlov@mail.ru', 1),
    ('Татьяна Волкова', '79160123456', 'tatiana.volk@mail.ru', 1);

--Добавление поставщиков.

INSERT INTO Suppliers (CompanyName, Phone, Email)
VALUES 
    ('ООО "АвтоЗапчасти-М"', '74951112233', 'info@autozap-m.ru'),
    ('Bosch Auto Parts', '74952223344', 'sales@bosch.ru'),
    ('ООО "Манн Фильтр"', '78123334455', 'spb@mann-filter.ru'),
    ('Gates Corporation', '74953334466', 'russia@gates.com'),
    ('NGK Spark Plugs', '74954445577', 'sales@ngk.ru'),
    ('ООО "Тормозные Системы"', '74955556688', 'info@brake-sys.ru'),
    ('Varta Batteries', '74956667799', 'russia@varta.com'),
    ('ZF Friedrichshafen', '74957778800', 'info@zf.com'),
    ('Continental AG', '74958889911', 'russia@continental.com'),
    ('Denso Corporation', '74959990022', 'sales@denso.ru');

--Добавление товаров.

INSERT INTO Products (Name, Price, StockQuantity)
VALUES 
    ('Масляный фильтр Mann Toyota Camry', 850.00, 45),
    ('Масляный фильтр Mann Hyundai Solaris', 790.00, 32),
    ('Воздушный фильтр Bosch Ford Focus', 1200.00, 18),
    ('Тормозные колодки передние Bosch', 2350.00, 12),
    ('ГРМ комплект Gates Renault Logan', 4500.00, 5),
    ('Свечи зажигания NGK (4шт)', 1800.00, 25),
    ('Ремень ГРМ Gates', 3200.00, 15),
    ('Топливный фильтр Bosch', 950.00, 20),
    ('Аккумулятор Varta 60Ач', 8900.00, 8),
    ('Тормозные диски передние Bosch', 4200.00, 10),
    ('Масляный фильтр Mann Ford Focus', 820.00, 30),
    ('Свечи зажигания Denso (4шт)', 2100.00, 18),
    ('Ремень поликлиновой Gates', 1500.00, 22),
    ('Термостат Bosch', 1800.00, 14),
    ('Стойка стабилизатора ZF', 1200.00, 25),
    ('Шаровая опора ZF', 1600.00, 18),
    ('Ремень ГРМ Continental', 3800.00, 10),
    ('Тормозная жидкость Bosch 1л', 550.00, 50),
    ('Антифриз Mann 5л', 1200.00, 30),
    ('Масло моторное Mann 4л', 2500.00, 40);

--Добавление заказов.

DECLARE @C1 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79161234567');
DECLARE @C2 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79162345678');
DECLARE @C3 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79163456789');
DECLARE @C4 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79164567890');
DECLARE @C5 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79165678901');
DECLARE @C6 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79166789012');
DECLARE @C7 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79167890123');
DECLARE @C8 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79168901234');
DECLARE @C9 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79169012345');
DECLARE @C10 UNIQUEIDENTIFIER = (SELECT ClientID FROM Client WHERE Phone = '79160123456');

INSERT INTO Orders (OrderNumber, ClientID, OrderDate, IsPaid)
VALUES 
    (1001, @C1, '2025-01-10T10:30:00', 1),
    (1002, @C1, '2025-01-20T14:15:00', 1),
    (1003, @C2, '2025-01-15T09:00:00', 1),
    (1004, @C3, '2025-01-18T16:45:00', 0),
    (1005, @C3, '2025-02-01T13:00:00', 0),
    (1006, @C4, '2025-02-05T11:30:00', 1),
    (1007, @C5, '2025-02-10T14:20:00', 1),
    (1008, @C2, '2025-02-15T10:00:00', 1),
    (1009, @C6, '2025-02-20T09:45:00', 0),
    (1010, @C7, '2025-02-25T16:30:00', 1),
    (1011, @C8, '2025-03-01T12:00:00', 1),
    (1012, @C9, '2025-03-05T11:15:00', 0),
    (1013, @C10, '2025-03-10T15:30:00', 1),
    (1014, @C1, '2025-03-12T10:00:00', 1),
    (1015, @C3, '2025-03-15T14:00:00', 0);

--Добавление информации для продавца.

DECLARE @O1001 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1001);
DECLARE @O1002 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1002);
DECLARE @O1003 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1003);
DECLARE @O1004 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1004);
DECLARE @O1005 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1005);
DECLARE @O1006 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1006);
DECLARE @O1007 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1007);
DECLARE @O1008 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1008);
DECLARE @O1009 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1009);
DECLARE @O1010 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1010);
DECLARE @O1011 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1011);
DECLARE @O1012 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1012);
DECLARE @O1013 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1013);
DECLARE @O1014 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1014);
DECLARE @O1015 UNIQUEIDENTIFIER = (SELECT OrderID FROM Orders WHERE OrderNumber = 1015);

DECLARE @P1 INT = (SELECT ProductID FROM Products WHERE Name = 'Масляный фильтр Mann Toyota Camry');
DECLARE @P2 INT = (SELECT ProductID FROM Products WHERE Name = 'Свечи зажигания NGK (4шт)');
DECLARE @P3 INT = (SELECT ProductID FROM Products WHERE Name = 'Масляный фильтр Mann Hyundai Solaris');
DECLARE @P4 INT = (SELECT ProductID FROM Products WHERE Name = 'Тормозные колодки передние Bosch');
DECLARE @P5 INT = (SELECT ProductID FROM Products WHERE Name = 'Воздушный фильтр Bosch Ford Focus');
DECLARE @P6 INT = (SELECT ProductID FROM Products WHERE Name = 'ГРМ комплект Gates Renault Logan');
DECLARE @P7 INT = (SELECT ProductID FROM Products WHERE Name = 'Аккумулятор Varta 60Ач');
DECLARE @P8 INT = (SELECT ProductID FROM Products WHERE Name = 'Тормозные диски передние Bosch');
DECLARE @P9 INT = (SELECT ProductID FROM Products WHERE Name = 'Ремень ГРМ Gates');
DECLARE @P10 INT = (SELECT ProductID FROM Products WHERE Name = 'Масляный фильтр Mann Ford Focus');
DECLARE @P11 INT = (SELECT ProductID FROM Products WHERE Name = 'Свечи зажигания Denso (4шт)');
DECLARE @P12 INT = (SELECT ProductID FROM Products WHERE Name = 'Ремень поликлиновой Gates');
DECLARE @P13 INT = (SELECT ProductID FROM Products WHERE Name = 'Термостат Bosch');
DECLARE @P14 INT = (SELECT ProductID FROM Products WHERE Name = 'Стойка стабилизатора ZF');
DECLARE @P15 INT = (SELECT ProductID FROM Products WHERE Name = 'Шаровая опора ZF');
DECLARE @P16 INT = (SELECT ProductID FROM Products WHERE Name = 'Ремень ГРМ Continental');

INSERT INTO Sellers (OrderID, ProductID, ProductCount, DiscountPercent) 
VALUES 
    (@O1001, @P1, 2, 0),
    (@O1001, @P2, 1, 0),
    (@O1002, @P3, 1, 0),
    (@O1003, @P4, 1, 5),
    (@O1004, @P5, 1, 0),
    (@O1005, @P6, 1, 0),
    (@O1006, @P7, 1, 0),
    (@O1007, @P8, 1, 10),
    (@O1008, @P9, 1, 0),
    (@O1009, @P10, 1, 0),
    (@O1010, @P11, 1, 0),
    (@O1011, @P12, 1, 0),
    (@O1012, @P13, 1, 0),
    (@O1013, @P14, 1, 0),
    (@O1014, @P15, 1, 0),
    (@O1015, @P16, 1, 0);

--Добавление информации о поставках товара.

DECLARE @S1 INT = (SELECT SupplierID FROM Suppliers WHERE CompanyName = 'ООО "АвтоЗапчасти-М"');
DECLARE @S2 INT = (SELECT SupplierID FROM Suppliers WHERE CompanyName = 'Bosch Auto Parts');
DECLARE @S3 INT = (SELECT SupplierID FROM Suppliers WHERE CompanyName = 'ООО "Манн Фильтр"');

DECLARE @ProdMannToyota INT = (SELECT ProductID FROM Products WHERE Name = 'Масляный фильтр Mann Toyota Camry');
DECLARE @ProdMannHyundai INT = (SELECT ProductID FROM Products WHERE Name = 'Масляный фильтр Mann Hyundai Solaris');
DECLARE @ProdBoschFilter INT = (SELECT ProductID FROM Products WHERE Name = 'Воздушный фильтр Bosch Ford Focus');
DECLARE @ProdBoschKolodki INT = (SELECT ProductID FROM Products WHERE Name = 'Тормозные колодки передние Bosch');
DECLARE @ProdGates INT = (SELECT ProductID FROM Products WHERE Name = 'ГРМ комплект Gates Renault Logan');
DECLARE @ProdNGK INT = (SELECT ProductID FROM Products WHERE Name = 'Свечи зажигания NGK (4шт)');

INSERT INTO ProductSupplies (SupplierID, ProductID, SupplyDate, SupplyCount, SupplyPrice) 
VALUES 
    (@S3, @ProdMannToyota, GETDATE(), 100, 467.50),
    (@S3, @ProdMannHyundai, GETDATE(), 100, 434.50),
    (@S2, @ProdBoschFilter, GETDATE(), 100, 720.00),
    (@S2, @ProdBoschKolodki, GETDATE(), 100, 1410.00),
    (@S1, @ProdGates, GETDATE(), 100, 2250.00),
    (@S2, @ProdNGK, GETDATE(), 100, 1170.00);




