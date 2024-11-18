IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Certificate] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Certificate] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Product] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Quantity] int NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Product] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Promotion] (
        [Id] nvarchar(450) NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        [Amount] int NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Promotion] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Role] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(50) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [ProductItem] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Category] nvarchar(max) NOT NULL,
        [Origin] nvarchar(max) NOT NULL,
        [Sex] nvarchar(max) NOT NULL,
        [Age] int NOT NULL,
        [Size] nvarchar(max) NOT NULL,
        [Species] nvarchar(max) NOT NULL,
        [Personality] nvarchar(max) NOT NULL,
        [FoodAmount] nvarchar(max) NOT NULL,
        [WaterTemp] nvarchar(max) NOT NULL,
        [MineralContent] nvarchar(max) NOT NULL,
        [PH] nvarchar(max) NOT NULL,
        [ImageUrl] nvarchar(max) NOT NULL,
        [Quantity] int NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [ProductId] nvarchar(450) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ProductItem] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductItem_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [User] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Address] nvarchar(200) NULL,
        [Phone] nvarchar(max) NULL,
        [RoleId] nvarchar(450) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_User] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_User_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [ProductCertificate] (
        [Id] nvarchar(450) NOT NULL,
        [Provider] nvarchar(max) NOT NULL,
        [CertificateId] nvarchar(450) NOT NULL,
        [ProductItemId] nvarchar(450) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ProductCertificate] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductCertificate_Certificate_CertificateId] FOREIGN KEY ([CertificateId]) REFERENCES [Certificate] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProductCertificate_ProductItem_ProductItemId] FOREIGN KEY ([ProductItemId]) REFERENCES [ProductItem] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Blog] (
        [Id] nvarchar(450) NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [ImageUrl] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Blog] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Blog_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Cart] (
        [Id] nvarchar(450) NOT NULL,
        [Total] decimal(18,2) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Cart] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Cart_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Consignment] (
        [Id] nvarchar(450) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Consignment] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Consignment_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Review] (
        [Id] nvarchar(450) NOT NULL,
        [Rating] int NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [ProductItemId] nvarchar(450) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Review] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Review_ProductItem_ProductItemId] FOREIGN KEY ([ProductItemId]) REFERENCES [ProductItem] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Review_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [UserRefreshToken] (
        [Id] nvarchar(450) NOT NULL,
        [User_Id] nvarchar(450) NOT NULL,
        [RefreshToken] nvarchar(max) NOT NULL,
        [JwtId] nvarchar(max) NOT NULL,
        [isUsed] bit NOT NULL,
        [CreateTime] datetime2 NOT NULL,
        [ExpireTime] datetime2 NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_UserRefreshToken] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserRefreshToken_User_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [CartItem] (
        [Id] nvarchar(450) NOT NULL,
        [Quantity] int NOT NULL,
        [CartId] nvarchar(450) NOT NULL,
        [ProductItemId] nvarchar(450) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_CartItem] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CartItem_Cart_CartId] FOREIGN KEY ([CartId]) REFERENCES [Cart] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CartItem_ProductItem_ProductItemId] FOREIGN KEY ([ProductItemId]) REFERENCES [ProductItem] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Order] (
        [Id] nvarchar(450) NOT NULL,
        [Total] decimal(18,2) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [StaffId] nvarchar(450) NULL,
        [PromotionId] nvarchar(450) NULL,
        [Address] nvarchar(200) NULL,
        [IsDelivered] bit NULL,
        [ConsignmentId] nvarchar(450) NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Order] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Order_Consignment_ConsignmentId] FOREIGN KEY ([ConsignmentId]) REFERENCES [Consignment] ([Id]),
        CONSTRAINT [FK_Order_Promotion_PromotionId] FOREIGN KEY ([PromotionId]) REFERENCES [Promotion] ([Id]),
        CONSTRAINT [FK_Order_User_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [User] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Order_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [Payment] (
        [Id] nvarchar(450) NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [Method] nvarchar(max) NOT NULL,
        [OrderId] nvarchar(450) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Payment] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Payment_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [ConsignmentItem] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Category] nvarchar(max) NOT NULL,
        [Origin] nvarchar(max) NOT NULL,
        [Sex] nvarchar(max) NOT NULL,
        [Age] int NOT NULL,
        [Size] nvarchar(max) NOT NULL,
        [Species] nvarchar(max) NOT NULL,
        [Checkedout] bit NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        [ConsignmentId] nvarchar(450) NOT NULL,
        [OrderItemId] nvarchar(450) NULL,
        [Personality] nvarchar(max) NOT NULL,
        [FoodAmount] nvarchar(max) NOT NULL,
        [WaterTemp] nvarchar(max) NOT NULL,
        [MineralContent] nvarchar(max) NOT NULL,
        [PH] nvarchar(max) NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ConsignmentItem] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ConsignmentItem_Consignment_ConsignmentId] FOREIGN KEY ([ConsignmentId]) REFERENCES [Consignment] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE TABLE [OrderItem] (
        [Id] nvarchar(450) NOT NULL,
        [OrderID] nvarchar(450) NOT NULL,
        [Quantity] int NOT NULL,
        [ProductItemId] nvarchar(450) NULL,
        [ConsignmentItemId] nvarchar(450) NULL,
        [CreatedTime] datetimeoffset NOT NULL,
        [LastUpdatedTime] datetimeoffset NOT NULL,
        [DeletedTime] datetimeoffset NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_OrderItem] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItem_ConsignmentItem_ConsignmentItemId] FOREIGN KEY ([ConsignmentItemId]) REFERENCES [ConsignmentItem] ([Id]),
        CONSTRAINT [FK_OrderItem_Order_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Order] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderItem_ProductItem_ProductItemId] FOREIGN KEY ([ProductItemId]) REFERENCES [ProductItem] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Blog_UserId] ON [Blog] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Cart_UserId] ON [Cart] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_CartItem_CartId] ON [CartItem] ([CartId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_CartItem_ProductItemId] ON [CartItem] ([ProductItemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Consignment_UserId] ON [Consignment] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_ConsignmentItem_ConsignmentId] ON [ConsignmentItem] ([ConsignmentId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_ConsignmentItem_OrderItemId] ON [ConsignmentItem] ([OrderItemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Order_ConsignmentId] ON [Order] ([ConsignmentId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Order_PromotionId] ON [Order] ([PromotionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Order_StaffId] ON [Order] ([StaffId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Order_UserId] ON [Order] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_OrderItem_ConsignmentItemId] ON [OrderItem] ([ConsignmentItemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_OrderItem_OrderID] ON [OrderItem] ([OrderID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_OrderItem_ProductItemId] ON [OrderItem] ([ProductItemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Payment_OrderId] ON [Payment] ([OrderId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_ProductCertificate_CertificateId] ON [ProductCertificate] ([CertificateId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_ProductCertificate_ProductItemId] ON [ProductCertificate] ([ProductItemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_ProductItem_ProductId] ON [ProductItem] ([ProductId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Review_ProductItemId] ON [Review] ([ProductItemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_Review_UserId] ON [Review] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_User_RoleId] ON [User] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    CREATE INDEX [IX_UserRefreshToken_User_Id] ON [UserRefreshToken] ([User_Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    ALTER TABLE [ConsignmentItem] ADD CONSTRAINT [FK_ConsignmentItem_OrderItem_OrderItemId] FOREIGN KEY ([OrderItemId]) REFERENCES [OrderItem] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20241104102106_AddAdditionalFieldsToConsignmentItem')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241104102106_AddAdditionalFieldsToConsignmentItem', N'6.0.31');
END;
GO

COMMIT;
GO