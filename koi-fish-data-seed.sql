USE [KoiFarm]
GO

BEGIN TRANSACTION;

-- Check and Delete if any data exists in the User table
IF EXISTS (SELECT 1 FROM [dbo].[User])
BEGIN
    DELETE FROM [dbo].[User];
END
GO

-- Check and Delete if any data exists in the Role table
IF EXISTS (SELECT 1 FROM [dbo].[Role])
BEGIN
    DELETE FROM [dbo].[Role];
END
GO

-- Check and Delete if any data exists in the Product table
IF EXISTS (SELECT 1 FROM [dbo].[Product])
BEGIN
    DELETE FROM [dbo].[Product];
END
GO

-- Insert data into Role table
INSERT INTO [dbo].[Role]
			([Id]
			,[Name]
			,[CreatedTime]
			,[LastUpdatedTime]
			,[DeletedTime]
			,[IsDeleted]
			,[DeletedAt])
VALUES
			('0', 'Customer', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),  
			('1', 'Manager', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
			('2', 'Staff', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL)
GO


-- Insert manager into User table
INSERT INTO [dbo].[User]
           ([Id]
           ,[Name]
           ,[Email]
           ,[Password]
           ,[Address]
           ,[Phone]
           ,[RoleId]
           ,[CreatedTime]
           ,[LastUpdatedTime]
           ,[DeletedTime]
           ,[IsDeleted]
           ,[DeletedAt])
VALUES
           ('aa5fd505e603484ba3abd30223d0c29f' ,'manager', 'manager@gmail.com', '123456', NULL, '0934140524', '1', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL)
GO

-- Insert products into Product table
INSERT INTO [dbo].[Product] 
    ([Id], 
	[Name], 
	[Quantity],
	[ImageUrl],
	[Description],
	[CreatedTime], 
	[LastUpdatedTime], 
	[DeletedTime], 
	[IsDeleted], 
	[DeletedAt])
VALUES
    ('41c2fbe4b02549c587837a3c4658e02a', 'Koi Kohaku', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('d93645edc83a4a0e8cfcbb9f2d819cc9', 'Koi Ogon', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4f89cbd2af1e422fbfbfb1b78eb5d620', 'Koi Showa', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('3369c7cbba56453098a3d9487f22e4bc', 'Koi Tancho', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('64eb0df76e7b4c7ab2cdb7e94ddc8d8f', 'Koi Bekko', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4ef098cfc4f94649bbb7c48b280cd31a', 'Koi Doitsu', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 1, NULL),
    ('4e55d312c48d4071af5b678b391646d5', 'Koi Ginrin', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('bc1b07abc67945cabcfb9dbb4cd26763', 'Koi Goshiki', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('01b364370e6b428581d3d2a344a197b2', 'Koi Benigo', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('5b160e9cee4c40a79e1d8a7cf80f3d0b', 'Koi Asagi', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('e4bfaf2848c2429ab06c85405707a925', 'Koi Platinum', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('e4048e4bbf8d480fadfc8c7d0e3e981d', 'Koi Shusui', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('ca0884736a6542a98d0206716c2d6846', 'Koi Taisho Sanke', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('5a84123588704afb83ca029c8eca218b', 'Koi Utsurimono', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('2f7a5273a19240c58db1f9e0b157b208', 'Koi Chagoi', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('611ce6b1bca74c909b42609ee18f674d', 'Koi Karashi', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('ca226aab2bc946ce839057c492574219', 'Koi Kawarimono', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('1368ee1fa303461b87ddbb5b26d6d8d0', 'Koi Kujaku', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('2db74b614ecc44dd9b2c10429ff49cb3', 'Koi Matsuba', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('af42c9de1e8048c7a7d8b254dc71afea', 'Koi KikoKryu', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('af301c6d526849e4bfde6e2ead5be943', 'Koi Ochiba', 0, NULL, NULL, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);

COMMIT;
