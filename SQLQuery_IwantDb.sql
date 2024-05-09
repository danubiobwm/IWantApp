-- Inserção de dados na tabela Categories
INSERT INTO Categories (Id, Name, Active, CreatedBy, CreatedOn, EditedBy, EditedOn)
VALUES
    (NEWID(), 'Eletrônicos', 1, 'System', GETUTCDATE(), 'System', GETUTCDATE()),
    (NEWID(), 'Roupas', 1, 'System', GETUTCDATE(), 'System', GETUTCDATE()),
    (NEWID(), 'Livros', 1, 'System', GETUTCDATE(), 'System', GETUTCDATE());

-- Inserção de dados na tabela Products
INSERT INTO Products (Id, Name, CategoryId, Description, HasStock, Active, CreatedBy, CreatedOn, EditedBy, EditedOn)
VALUES
    (NEWID(), 'Smartphone', (SELECT Id FROM Categories WHERE Name = 'Eletrônicos'), 'Um ótimo smartphone', 1, 1, 'System', GETUTCDATE(), 'System', GETUTCDATE()),
    (NEWID(), 'Camiseta', (SELECT Id FROM Categories WHERE Name = 'Roupas'), 'Camiseta confortável', 1, 1, 'System', GETUTCDATE(), 'System', GETUTCDATE()),
    (NEWID(), 'Livro de Ficção', (SELECT Id FROM Categories WHERE Name = 'Livros'), 'Um ótimo livro de ficção', 1, 1, 'System', GETUTCDATE(), 'System', GETUTCDATE());

-- Inserção de dados na tabela AspNetRoles
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES
    ('1', 'Admin', 'ADMIN', NEWID()),
    ('2', 'User', 'USER', NEWID());

-- Inserção de dados na tabela AspNetUsers
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES
    ('1', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 1, '<hashed_password>', '<security_stamp>', NEWID(), NULL, 0, 0, NULL, 1, 0),
    ('2', 'user@example.com', 'USER@EXAMPLE.COM', 'user@example.com', 'USER@EXAMPLE.COM', 1, '<hashed_password>', '<security_stamp>', NEWID(), NULL, 0, 0, NULL, 1, 0);

-- Inserção de dados na tabela AspNetUserRoles
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES
    ('1', '1'), -- admin possui papel de admin
    ('2', '2'); -- user possui papel de usuário

-- Inserção de dados na tabela Orders
INSERT INTO Orders (Id, ClientId, Total, DeliveryAddress, CreatedBy, CreatedOn, EditedBy, EditedOn)
VALUES
    ('1', 'client1', 100.00, 'Address 1', 'System', GETUTCDATE(), 'System', GETUTCDATE()),
    ('2', 'client2', 150.00, 'Address 2', 'System', GETUTCDATE(), 'System', GETUTCDATE());

-- Inserção de dados na tabela OrderProducts
INSERT INTO OrderProducts (OrdersId, ProductsId)
VALUES
    ('1', (SELECT Id FROM Products WHERE Name = 'Smartphone')), -- Adiciona Smartphone ao Pedido 1
    ('2', (SELECT Id FROM Products WHERE Name = 'Camiseta')),  -- Adiciona Camiseta ao Pedido 2
    ('2', (SELECT Id FROM Products WHERE Name = 'Livro de Ficção')); -- Adiciona Livro de Ficção ao Pedido 2

-- Inserção de dados na tabela Categories
INSERT INTO Categories (Id, Name, Active, CreatedBy, CreatedOn, EditedBy, EditedOn)
VALUES
    ('1', 'Eletrônicos', 1, 'System', GETUTCDATE(), 'System', GETUTCDATE()),
    ('2', 'Roupas', 1, 'System', GETUTCDATE(), 'System', GETUTCDATE()),
    ('3', 'Livros', 1, 'System', GETUTCDATE(), 'System', GETUTCDATE());

-- Inserção de dados na tabela Products
INSERT INTO Products (Id, Name, CategoryId, Description, HasStock, Active, CreatedBy, CreatedOn, EditedBy, EditedOn, Price)
VALUES
    ('1', 'Smartphone', '1', 'Um ótimo smartphone', 1, 1, 'System', GETUTCDATE(), 'System', GETUTCDATE(), 1000.00),
    ('2', 'Camiseta', '2', 'Camiseta confortável', 1, 1, 'System', GETUTCDATE(), 'System', GETUTCDATE(), 50.00),
    ('3', 'Livro de Ficção', '3', 'Um ótimo livro de ficção', 1, 1, 'System', GETUTCDATE(), 'System', GETUTCDATE(), 30.00);


-- Inserção de dados na tabela Orders
INSERT INTO Orders (Id, ClientId, Total, DeliveryAddress, CreatedBy, CreatedOn, EditedBy, EditedOn)
VALUES
    ('1', 'client1', 100.00, 'Endereço 1', 'System', GETUTCDATE(), 'System', GETUTCDATE()),
    ('2', 'client2', 150.00, 'Endereço 2', 'System', GETUTCDATE(), 'System', GETUTCDATE());

-- Inserção de dados na tabela OrderProducts
INSERT INTO OrderProducts (OrdersId, ProductsId)
VALUES
    ('1', '1'), -- Produto 1 no Pedido 1 (Smartphone)
    ('2', '2'), -- Produto 2 no Pedido 2 (Camiseta)
    ('2', '3'); -- Produto 3 no Pedido 2 (Livro de Ficção)
