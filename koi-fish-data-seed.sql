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

-- Check and Delete if any data exists in the Blog table
IF EXISTS (SELECT 1 FROM [dbo].[Blog])
BEGIN
    DELETE FROM [dbo].[Blog];
END
GO


-- Check and Delete if any data exists in the Certificate table
IF EXISTS (SELECT 1 FROM [dbo].[Certificate])
BEGIN
    DELETE FROM [dbo].[Certificate];
END
GO

-- Check and Delete if any data exists in the ProductCertificate table
IF EXISTS (SELECT 1 FROM [dbo].[ProductCertificate])
BEGIN
    DELETE FROM [dbo].[ProductCertificate];
END
GO


-- Check and Delete if any data exists in the Role table
IF EXISTS (SELECT 1 FROM [dbo].[ProductItem])
BEGIN
    DELETE FROM [dbo].[ProductItem];
END
GO

-- Check and Delete if any data exists in the Role table
IF EXISTS (SELECT 1 FROM [dbo].[Batch])
BEGIN
    DELETE FROM [dbo].[Batch];
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
			('2', 'Staff', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);
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
           ('aa5fd505e603484ba3abd30223d0c29f' ,'manager', 'manager@gmail.com', '123456', NULL, '0934140524', '1', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
           ('bb6fd505e603484ba3abd30223d0c29f', 'staff', 'staff@gmail.com', '123456', NULL, '0934140525', '2', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL)
GO

-- Insert blogs into Blog table
INSERT INTO [KoiFarm].[dbo].[Blog] 
    ([Id], 
	[Title], 
	[ImageUrl], 
	[Description], 
	[UserId], 
	[CreatedTime], 
	[LastUpdatedTime], 
	[DeletedTime], 
	[IsDeleted], 
	[DeletedAt])
VALUES
    ('84d62a6fad8e44b3a86eb2075cf5997d', N'Kinh nghiệm nuôi cá Koi trong hồ ngoài trời', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSz2vIkk5DKxeUrIZYz9CxUTVdqkVmSdwDInQ&s', 
	N'Nuôi cá Koi ngoài trời là một trải nghiệm thú vị và có thể mang lại không gian thư giãn, thẩm mỹ cao cho ngôi nhà của bạn. Tuy nhiên, để cá Koi phát triển khỏe mạnh, bạn cần chú ý đến một số yếu tố quan trọng. Đầu tiên, chất lượng nước trong hồ là yếu tố then chốt. Cá Koi cần một môi trường nước sạch sẽ, có đủ oxy và hệ thống lọc nước mạnh để loại bỏ cặn bẩn và chất độc. Việc thay nước định kỳ, kiểm tra độ pH và nồng độ amoniac trong nước là cần thiết. Ngoài ra, bạn nên chọn vị trí hồ cá phù hợp. Hồ nên được đặt ở nơi có ánh sáng mặt trời nhưng không quá nóng. Ánh nắng buổi sáng sẽ giúp cá phát triển tốt, nhưng nếu hồ ở nơi quá nắng, bạn nên lắp đặt mái che hoặc cây xanh xung quanh để giảm thiểu nhiệt độ trong hồ. Về thức ăn, cá Koi có thể ăn các loại thức ăn công nghiệp hoặc thực phẩm tự nhiên như sâu, giun đất, rau xanh. Điều quan trọng là bạn không nên cho ăn quá nhiều để tránh làm ô nhiễm nước và gây bệnh cho cá. Nuôi cá Koi ngoài trời không chỉ đòi hỏi sự kiên nhẫn mà còn cần kiến thức sâu về chăm sóc và bảo dưỡng hồ cá. Với kinh nghiệm và sự chuẩn bị kỹ càng, bạn sẽ có một hồ cá Koi tuyệt đẹp và sinh động.', 
	'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    ('7782c89c73ba423a9369d9a9d077ae5a', N'Bí quyết chọn cá Koi đẹp và khỏe', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSD3aVI1ftuyN1T6vqv90G5vCEZyK-ACDGvxQ&s', 
	N'Chọn cá Koi đẹp và khỏe mạnh không phải là một nhiệm vụ dễ dàng, đặc biệt là đối với những người mới bắt đầu. Khi chọn cá, màu sắc là một trong những yếu tố quan trọng nhất. Một con cá Koi đẹp thường có màu sắc rõ nét, không bị pha lẫn. Những màu sắc phổ biến của cá Koi bao gồm đỏ, trắng, vàng, đen, và xanh. Màu sắc của cá Koi có thể thay đổi theo thời gian, nhưng cá con có màu sắc đậm và rõ thường sẽ giữ được màu sắc tốt khi trưởng thành. Yếu tố thứ hai là hình dáng và kích thước cơ thể. Một con cá Koi khỏe mạnh có thân hình cân đối, không quá mập cũng không quá gầy, đuôi và vây phải hoàn chỉnh và không bị rách. Đặc biệt, bạn nên chú ý đến sự di chuyển của cá. Cá Koi khỏe thường bơi nhanh, linh hoạt và không bị nghiêng hay lắc lư khi bơi. Ngoài ra, bạn nên chọn cá từ các cơ sở uy tín, nơi cá được nuôi dưỡng trong điều kiện sạch sẽ và không bị lây nhiễm bệnh. Trước khi mua, bạn có thể quan sát cá trong một khoảng thời gian để xem có dấu hiệu bệnh tật nào không, như nấm, đốm trắng hay các vết thương ngoài da. Chọn cá Koi không chỉ phụ thuộc vào mắt thẩm mỹ mà còn đòi hỏi sự tinh tế và kinh nghiệm.', 
	'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    ('e58fad80aad84c858a0d9ac0c3fc065f', N'Các loại thức ăn tốt nhất cho cá Koi', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTiRrJ2YYAcDB2skplKiWW3k8VjRTiFR9eYhA&s', 
	N'Thức ăn đóng vai trò quan trọng trong việc duy trì sức khỏe và sự phát triển của cá Koi. Một chế độ ăn hợp lý không chỉ giúp cá Koi phát triển tốt mà còn cải thiện màu sắc của chúng. Các loại thức ăn cho cá Koi thường được chia thành hai nhóm chính: thức ăn công nghiệp và thức ăn tự nhiên. Thức ăn công nghiệp dành cho cá Koi thường chứa đầy đủ các dưỡng chất như protein, chất béo, vitamin, và khoáng chất cần thiết cho sự phát triển của cá. Khi chọn thức ăn, bạn nên tìm các loại có hàm lượng protein cao, đặc biệt là dành cho cá non, vì protein giúp cá phát triển nhanh hơn. Ngoài ra, trong các sản phẩm thức ăn công nghiệp, bạn có thể tìm các loại có chất tăng cường màu sắc để giúp cá Koi có màu sắc rực rỡ. Bên cạnh đó, thức ăn tự nhiên như giun, tôm, hoặc rau xanh cũng là nguồn dưỡng chất tuyệt vời. Thức ăn tự nhiên không chỉ cung cấp dinh dưỡng mà còn giúp cá duy trì bản năng săn mồi và hoạt động. Khi cho cá Koi ăn, điều quan trọng là bạn không nên cho quá nhiều, vì điều này có thể làm ô nhiễm nước trong hồ, gây hại cho sức khỏe của cá. Cân nhắc cả hai loại thức ăn công nghiệp và tự nhiên sẽ giúp bạn xây dựng một chế độ dinh dưỡng hoàn chỉnh cho đàn cá của mình.', 
	'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    ('6f563c19dce848c089a0c8534c68e84c', N'Cách chăm sóc cá Koi trong mùa đông', 'https://i.ytimg.com/vi/0ezGC5uIwLg/maxresdefault.jpg', 
	N'Chăm sóc cá Koi vào mùa đông đòi hỏi sự cẩn trọng đặc biệt, vì khi nhiệt độ nước giảm xuống, cá Koi sẽ trở nên ít hoạt động và có thể gặp phải các vấn đề sức khỏe nghiêm trọng. Một trong những yếu tố quan trọng nhất là đảm bảo nhiệt độ nước trong hồ không quá thấp. Nhiệt độ lý tưởng cho cá Koi vào mùa đông là từ 10-15°C. Nếu bạn sống ở khu vực có mùa đông lạnh, bạn nên lắp đặt hệ thống sưởi hoặc sử dụng máy bơm nước để giữ cho nhiệt độ ổn định. Ngoài ra, hệ thống lọc nước vẫn cần hoạt động đều đặn để duy trì chất lượng nước. Nước lạnh làm giảm khả năng phân hủy chất hữu cơ trong hồ, nên nếu không lọc đúng cách, nước sẽ trở nên ô nhiễm, gây bệnh cho cá. Bạn cũng nên giảm lượng thức ăn cho cá Koi vào mùa đông, vì cá ít hoạt động hơn và không cần nhiều năng lượng. Việc cho cá ăn quá nhiều trong thời tiết lạnh có thể dẫn đến tình trạng dư thừa thức ăn, gây ra các vấn đề về tiêu hóa và làm ô nhiễm nước. Cuối cùng, bạn nên kiểm tra tình trạng sức khỏe của cá thường xuyên. Nếu cá có dấu hiệu bị bệnh, hãy xử lý kịp thời và tham khảo ý kiến từ chuyên gia để đảm bảo cá Koi của bạn vượt qua mùa đông một cách khỏe mạnh.', 
	'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    ('217c6a8482184196800afc5020b1cf9a', N'Làm thế nào để xây dựng hồ cá Koi hoàn hảo', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQC-00p63FEvh32z-kQ4HENh5OL6T6jreA_vA&s', 
	N'Xây dựng một hồ cá Koi hoàn hảo không chỉ đòi hỏi kỹ năng thiết kế mà còn cần sự hiểu biết về đặc điểm sinh học của cá. Trước tiên, bạn cần xác định vị trí và kích thước của hồ. Hồ nên được đặt ở nơi có ánh sáng tự nhiên, nhưng cũng phải có không gian đủ lớn để cá Koi phát triển. Kích thước hồ càng lớn thì càng tốt, vì nó giúp duy trì chất lượng nước ổn định và cung cấp không gian bơi lội cho cá. Về thiết kế, một hồ cá Koi tiêu chuẩn cần có hệ thống lọc nước mạnh mẽ để loại bỏ chất thải và duy trì nước sạch. Bạn nên cân nhắc lắp đặt thêm hệ thống tạo oxy, vì cá Koi cần rất nhiều oxy để duy trì sự sống. Bên cạnh đó, hệ thống thoát nước cũng là yếu tố quan trọng để giúp bạn dễ dàng vệ sinh hồ. Khi thiết kế hồ, việc tạo ra các khu vực sâu và nông khác nhau cũng là một ý tưởng hay. Các khu vực nông giúp cá tiếp cận gần hơn với ánh sáng mặt trời, trong khi khu vực sâu cung cấp nơi trú ẩn khi nhiệt độ thay đổi. Cuối cùng, bạn có thể trang trí hồ bằng cây cỏ, đá tự nhiên để tạo môi trường sống gần gũi với tự nhiên cho cá Koi. Một hồ cá được xây dựng hợp lý không chỉ giúp cá phát triển tốt mà còn mang lại giá trị thẩm mỹ cao cho ngôi nhà của bạn.', 
	'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL);
GO

-- Insert Products into Product table
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
    -- Product: Koi Kohaku
    ('1', 'Koi Kohaku', 6, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 
    N'Koi Kohaku Là dòng Koi được yêu thích nhất. Là dòng Koi được lai tạo đầu tiên tại Nhật...', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- Product: Koi Ogon
    ('2', 'Koi Ogon', 5, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 
    N'Cá Koi Ogon Là dòng koi màu ánh bạc kim loại, thuộc dòng đơn sắc, 1 màu...', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- Product: Koi Showa
    ('3', 'Koi Showa', 4, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 
    N'Cá Koi Showa (Showa sanshoku) Lai tạo thành công năm 1927 từ Koi kohaku...', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- Product: Koi Tancho
    ('4', 'Koi Tancho', 3, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 
    N'Cá Koi Tancho: Có mảng đỏ Hi to tròn trên đỉnh đầu. Mình trắng muốt...', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- Product: Koi Bekko
    ('5', 'Koi Bekko', 3, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg', 
    N'Cá Koi Bekko: Cùng họ với Koi Utsuri khác ở điểm Koi bekko có đốm đen...', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	 -- Product: Koi Asagi
    ('6', 'Koi Asagi', 2, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028100/products/asagi.jpg', 
    N'Cá Koi Asagi: Có màu xanh lam ở lưng với vảy xếp hình lưới, bụng và vây màu đỏ hoặc cam...', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- Product: Koi Shusui
    ('7', 'Koi Shusui', 2, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028120/products/shusui.jpg', 
    N'Cá Koi Shusui: Phiên bản không vảy của Asagi, có màu xanh lam nhạt với dải vảy lớn dọc theo lưng...', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- Product: Koi Utsuri
    ('8', 'Koi Utsurimono', 2, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028140/products/utsurimono.jpg', 
    N'Cá Koi Utsurimono (Hikariutsuri / Hikari Utsurimono) Dòng Koi da đen với các mảng màu trắng, đỏ hoặc vàng tựa mô hình hoa văn ngựa vằn. Hoa văn vàng trên nền đen phổ biến nhất, lâu đời nhất. Gồm 4 phiên bản Hi Utsuri (hai màu đen và đỏ); Ki Utsuri (hai màu đen và vàng); Kin Showa (3 màu đỏ đen, trắng bạc); Shiro Utsuri (hai màu đen và trắng).', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- Product: Koi Goshiki
    ('9', 'Koi Goshiki', 2, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028160/products/goshiki.jpg', 
    N'Cá Koi Goshiki: Kết hợp năm màu sắc: trắng, đỏ, đen, xanh lam và xám, tạo nên vẻ đẹp độc đáo...', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- Product: Koi Koromo
    ('10', 'Koi Kujaku', 2, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028180/products/kujaku.jpg', 
    N'Cá Koi Kujaku có màu nền bằng kim loại phủ bởi màu đỏ, cam hoặc màu vàng kim loại, tạo thành hiệu ứng đặc biệt', 
    SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('11', 'Koi Matsuba', 2, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027774/products/matsuba.jpg', 
	N'Matsuba là một loại cá Koi có màu kim loại với hoa văn lưới màu đen. Cơ thể của cá koi Matsuba có một màu với vảy hình nón thông. Loại cá này có lớp vảy óng ánh vô cùng đẹp mắt. Để tìm hiểu kỹ hơn về giống cá Koi Matsuba này hãy cùng tham khảo bài viết dưới đây của chúng tôi nhé', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    ('12', 'Koi KikoKryu', 2, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027737/products/kikokryu.jpg', 
	N'Cá Koi KikoKryu Kikokuryu thực chất là 1 kumonryu kim loại. Có làn da bạch kim sáng bóng, vây với sắc màu sumi đen sâu trong suốt. Được lai tạo từ Kim Ogon và kumonryu. Nhiệt độ, nước, di truyền là yếu tố quyết định mô hình màu sắc, đậm, nhạt của KikoKryu.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    ('13', 'Koi Ochiba', 2, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027785/products/ochiba.jpg', 
	N'Matsuba là một loại cá Koi có màu kim loại với hoa văn lưới màu đen. Cơ thể của cá koi Ochiba có một màu với vảy hình nón thông. Loại cá này có lớp vảy óng ánh vô cùng đẹp mắt. Để tìm hiểu kỹ hơn về giống cá Koi Matsuba này hãy cùng tham khảo bài viết dưới đây của chúng tôi nhé', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('14', 'Koi Doitsu', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027691/products/doitsu.jpg', 
	N'Cá Koi Doitsu Là dòng Koi da trơn, có hàng vảy rồng to, đều đối xứng nhau 2 bên lưng. Một số em Koi Doitsu còn cả hàng vảy rồng chạy dọc 2 bên hông', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('15', 'Koi Ginrin', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027702/products/ginrin.jpg', 
	N'Cá Koi Ginrin : Có vảy kim loại (lấp lánh, phát ánh kim). Tên tiếng anh là váy vàng bạc. Cá nổi bật với vảy lấp lánh tương phản với làn da mịn màng. Lớp vảy đồng đều, tạo hiệu ứng ánh sáng rất đẹp mắt.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('16', 'Koi Benigoi', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027660/products/benigoi.jpg', 
	N'Cá koi Benigoi (Tmud pond): có màu đỏ toàn thân (đỏ bình thường) từ chóp mũi đến đuôi (có con màu đỏ ớt). Chỉ có màu đỏ duy nhất, vây ngực không được có màu trắng. Nếu có là Koi Aka Hajio chứ không phải Koi Benigoi.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('17', 'Koi Platium', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027810/products/platium.jpg',
	N'Koi Platinum được ví như “thỏi bạc phát sáng” dưới hồ. Màu trắng bạch kim toàn thân là điểm nhấn độc đáo nhất của koi Platinum. Ai chơi Koi nhất định phải được sở hữu 1 em Bạch kim Platinum của riêng mình. Ở Việt Nam rất ít koi Platinum thuần chủng của Nhật , 80% bố mẹ là Koi F1 hoặc koi Việt. OnKoi Quang Minh là 1 trong số ít đối tác của Dainichi koi Farm Nhật Bản. Chúng tôi hiện đang có 1 vài em Koi Platinum với kết cấu body cực chuẩn, mạng lưới Fukurin đỉnh cao.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('18', 'Koi Taisho Sanke', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027825/products/sanke.jpg', 
	N'Cá Koi Taisho Sanke Được lai tạo từ Kohaku, có màu đen pha đỏ và trắng trên cơ thể. 1 em Koi taisho Sanke đẹp phải có màu trắng sạch, trắng rực rỡ như tuyết.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('19', 'Koi Chagoi', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027677/products/chagoi.jpg', 
	N'Chagoi là dòng Koi đơn sắc, không có ánh kim loại. Màu của nó có thể là màu trà cho Chagoi, màu lục nhạt cho Koi Midorigoi và xanh dương xám cho Soragoi. Koi Chagoi tính tình thân thiện, ngoan ngoãn, kích thước lớn nên tượng trưng cho sự may mắn.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('20', 'Koi Karashi', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027725/products/karashi.jpg', 
	N'Cá Koi Karashi có thể là da trơn (Doitsu), vảy chiếu (Ginrin) và loại vảy thông thường. Độ biến đổi màu sắc của nó có thể bắt đầu từ màu be, đến vàng nhạt, và cuối cùng là vàng đậm', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('21', 'Koi Kawarimono', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028056/products/kawarimono.jpg', 
	N'Cá Koi Kawarimono là 1 nhóm phân loại các loại Koi lai tạo với các nhóm Koi khác gọi chung là Kawarimono. Chúng được chia thành 3 nhóm Koi đơn sắc (single-colored Koi), màu đen tạp (black Koi Breeds) và Koi kawarimono', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);
GO

-- Insert ProductItems into ProductItem table
INSERT INTO [dbo].[ProductItem]
    ([Id], 
    [Name], 
    [Price], 
    [Category], 
    [Origin], 
    [Sex], 
    [Age], 
    [Size], 
    [Species], 
    [Personality], 
    [FoodAmount], 
    [WaterTemp], 
    [MineralContent], 
    [PH], 
    [ImageUrl], 
    [Quantity], 
    [Type], 
    [ProductId], 
    [CreatedTime], 
    [LastUpdatedTime], 
    [DeletedTime], 
    [IsDeleted], 
    [DeletedAt])
VALUES
    -- ProductItems for 'Koi Kohaku'
    ('1', 'Koi Kohaku - #0001', 1000000, 'Kohaku', 'Japan', 'Unknown', 2, 'Medium', 'Koi', 'Calm', '50', '20', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 1, 'Approved', 
    '1', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('2', 'Koi Kohaku - #0002', 1200000, 'Kohaku', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Calm', '55', '21', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 1, 'Approved', 
    '1', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('3', 'Koi Kohaku - #0002', 1400000, 'Kohaku', 'Japan', 'Male', 3, 'Large', 'Koi', 'Joyful', '55', '21', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 1, 'Approved', 
    '1', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4', 'Koi Kohaku - #0003', 1600000, 'Kohaku', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Joyful', '55', '21', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 1, 'Approved', 
    '1', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('5', 'Koi Kohaku - #0004', 200000, 'Kohaku', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Friendly', '55', '21', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 1, 'Approved', 
    '1', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('6', 'Koi Kohaku - #0005', 1100000, 'Kohaku', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Active', '55', '21', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 1, 'Approved', 
    '1', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Ogon'
    ('7', 'Koi Ogon - #0001', 1500000, 'Ogon', 'Japan', 'Unknown', 3, 'Large', 'Koi', 'Active', '70', '18', 'High', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 1, 'Approved', 
    '2', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('8', 'Koi Ogon - #0002', 1550000, 'Ogon', 'Japan', 'Female', 2, 'Large', 'Koi', 'Friendly', '75', '19', 'High', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 1, 'Approved', 
    '2', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('9', 'Koi Ogon - #0003', 1550000, 'Ogon', 'Japan', 'Male', 2, 'Large', 'Koi', 'Joyful', '75', '19', 'High', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 1, 'Approved', 
    '2', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('10', 'Koi Ogon - #0004', 1550000, 'Ogon', 'Japan', 'Male', 2, 'Large', 'Koi', 'Aggressive', '70', '22', 'High', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 1, 'Approved', 
    '2', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('11', 'Koi Ogon - #0005', 1550000, 'Ogon', 'Japan', 'Female', 2, 'Large', 'Koi', 'Friendly', '75', '19', 'High', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 1, 'Approved', 
    '2', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Showa'
    ('12', 'Koi Showa - #0001', 2000000, 'Showa', 'Japan', 'Male', 1, 'Large', 'Koi', 'Aggressive', '80', '22', 'Low', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 1, 'Approved', 
    '3', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('13', 'Koi Showa - #0002', 2000000, 'Showa', 'Japan', 'Male', 1, 'Large', 'Koi', 'Friendly', '80', '22', 'Low', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 1, 'Approved', 
    '3', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('14', 'Koi Showa - #0003', 2000000, 'Showa', 'Japan', 'Male', 1, 'Large', 'Koi', 'Shy', '80', '22', 'Low', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 1, 'Approved', 
    '3', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('15', 'Koi Showa - #0004', 2000000, 'Showa', 'Japan', 'Male', 1, 'Large', 'Koi', 'Aggressive', '80', '20', 'Low', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 1, 'Approved', 
    '3', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Tancho'
    ('16', 'Koi Tancho - #0001', 1200000, 'Tancho', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Shy', '60', '20', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 1, 'Approved', 
    '4', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
	('17', 'Koi Tancho - #0002', 1200000, 'Tancho', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Shy', '60', '20', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 1, 'Approved', 
    '4', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('18', 'Koi Tancho - #0003', 1200000, 'Tancho', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Shy', '60', '20', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 1, 'Approved', 
    '4', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Bekko'
	('21', 'Koi Bekko - #0001', 1200000, 'Tancho', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Shy', '60', '20', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 1, 'Approved', 
    '5', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
	('20', 'Koi Bekko - #0002', 1200000, 'Tancho', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Shy', '60', '20', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 1, 'Approved', 
    '5', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('19', 'Koi Bekko - #0003', 1100000, 'Bekko', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Aggressive', '60', '18', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg', 1, 'Approved', 
    '5', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
	
	 -- ProductItems for 'Koi Asagi'
    ('22', 'Koi Asagi - #0001', 1300000, 'Asagi', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Calm', '65', '22', 'Healthy', '8', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028100/products/asagi.jpg', 1, 'Approved', 
    '6', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('23', 'Koi Asagi - #0002', 1250000, 'Asagi', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Active', '63', '21', 'Healthy', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028100/products/asagi.jpg', 1, 'Approved', 
    '6', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Shusui'
    ('24', 'Koi Shusui - #0001', 1350000, 'Shusui', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Docile', '68', '23', 'Healthy', '8', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028120/products/shusui.jpg', 1, 'Approved', 
    '7', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('25', 'Koi Shusui - #0002', 1280000, 'Shusui', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Peaceful', '66', '22', 'Healthy', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028120/products/shusui.jpg', 1, 'Approved', 
    '7', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Utsuri'
    ('26', 'Koi Utsuri - #0001', 1400000, 'Utsuri', 'Japan', 'Female', 2, 'Large', 'Koi', 'Bold', '70', '25', 'Healthy', '9', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028140/products/utsurimono.jpg', 1, 'Approved', 
    '8', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('27', 'Koi Utsuri - #0002', 1320000, 'Utsuri', 'Japan', 'Male', 3, 'Large', 'Koi', 'Assertive', '68', '24', 'Healthy', '8', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028140/products/utsurimono.jpg', 1, 'Approved', 
    '8', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Goshiki'
    ('28', 'Koi Goshiki - #0001', 1450000, 'Goshiki', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Friendly', '67', '23', 'Healthy', '9', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028160/products/goshiki.jpg', 1, 'Approved', 
    '9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('29', 'Koi Goshiki - #0002', 1380000, 'Goshiki', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Sociable', '65', '22', 'Healthy', '8', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028160/products/goshiki.jpg', 1, 'Approved', 
    '9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Kujaku'
    ('30', 'Koi Koromo - #0001', 1500000, 'Koromo', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Gentle', '66', '23', 'Healthy', '9', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028180/products/kujaku.jpg', 1, 'Approved', 
    '10', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('31', 'Koi Koromo - #0002', 1420000, 'Koromo', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Calm', '64', '22', 'Healthy', '8', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028180/products/kujaku.jpg', 1, 'Approved', 
    '10', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
	
	 -- ProductItems for 'Koi Matsuba'
    ('32', 'Koi Matsuba - #0001', 1500000, 'Matsuba', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Calm', '65', '22', 'Healthy', '8', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027774/products/matsuba.jpg', 1, 'Approved', 
    '11', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('33', 'Koi Matsuba - #0002', 1450000, 'Matsuba', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Active', '63', '21', 'Healthy', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027774/products/matsuba.jpg', 1, 'Approved', 
    '11', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Kikokuryu'
    ('34', 'Koi Kikokuryu - #0001', 1600000, 'Kikokuryu', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Docile', '68', '23', 'Healthy', '8', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027737/products/kikokryu.jpg', 1, 'Approved', 
    '12', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('35', 'Koi Kikokuryu - #0002', 1550000, 'Kikokuryu', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Peaceful', '66', '22', 'Healthy', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027737/products/kikokryu.jpg', 1, 'Approved', 
    '12', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for 'Koi Ochiba'
    ('36', 'Koi Ochiba - #0001', 1550000, 'Ochiba', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Gentle', '66', '23', 'Healthy', '9', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027785/products/ochiba.jpg', 1, 'Approved', 
    '13', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('37', 'Koi Ochiba - #0002', 1500000, 'Ochiba', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Calm', '64', '22', 'Healthy', '8', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027785/products/ochiba.jpg', 1, 'Approved', 
    '13', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);
GO


-- Insert Empty Batches into Batch table
INSERT INTO [dbo].[Batch]
    ([Id], 
    [Name], 
    [Price], 
    [Description], 
    [Quantity], 
    [ImageUrl], 
    [CreatedTime], 
    [LastUpdatedTime], 
    [DeletedTime], 
    [IsDeleted], 
    [DeletedAt])
VALUES
    ('batch-001', 'Lô #001', 5300000, N'Lô cá Koi này gồm các dòng Kohaku và Ogon, nổi bật với vẻ đẹp truyền thống và sự sang trọng. Kohaku mang sắc đỏ rực rỡ trên nền trắng tinh khôi, biểu tượng của sự thuần khiết và may mắn. Ogon lại tỏa sáng với lớp vảy ánh kim vàng óng, thể hiện sự quyền quý và nổi bật. Tất cả đều khỏe mạnh, dáng bơi uyển chuyển, sẵn sàng làm điểm nhấn ấn tượng cho bất kỳ hồ cá cảnh nào.', 4, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('batch-002', 'Lô #002', 5550000, N'Lô cá Koi này bao gồm hai dòng đẳng cấp: Showa và Tancho. Showa thu hút với sự pha trộn mạnh mẽ giữa ba màu đen, trắng, và đỏ, tạo nên những hoa văn độc đáo và ấn tượng. Tancho, biểu tượng của sự tinh tế, nổi bật với một đốm đỏ tròn rực rỡ trên nền trắng tinh khiết, gợi nhớ đến quốc kỳ Nhật Bản. Cả hai dòng đều khỏe mạnh, đầy sức sống, và sẽ là tâm điểm nổi bật trong bất kỳ hồ cá nào.', 4, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('batch-003', 'Lô #003', 1150000, N'Lô cá Koi này gồm toàn bộ là dòng Bekko, nổi bật với vẻ đẹp tối giản nhưng đầy cuốn hút. Với nền trắng, đỏ, hoặc vàng, điểm xuyết những đốm đen tuyền sắc nét, Bekko mang lại cảm giác hài hòa và thanh lịch. Những chú cá trong lô đều khỏe mạnh, dáng bơi nhẹ nhàng, rất thích hợp để tạo điểm nhấn tinh tế cho hồ cảnh.', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('batch-004', 'Lô #004', 1250000, N'Lô cá Koi này thuộc dòng Ginrin, nổi bật với lớp vảy lấp lánh như kim cương dưới ánh sáng. Những chú cá trong lô đều mang vẻ đẹp rực rỡ, tạo hiệu ứng ánh sáng lung linh khi bơi lội. Với dáng vẻ khỏe mạnh và chuyển động uyển chuyển, lô Ginrin này sẽ mang lại sự sang trọng và ấn tượng cho bất kỳ hồ cá cảnh nào.', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);
GO

-- Insert ProductItems for Batch 1
INSERT INTO [dbo].[ProductItem]
    ([Id], 
    [Name], 
    [Price], 
    [Category], 
    [Origin], 
    [Sex], 
    [Age], 
    [Size], 
    [Species], 
    [Personality], 
    [FoodAmount], 
    [WaterTemp], 
    [MineralContent], 
    [PH], 
    [ImageUrl], 
    [Quantity], 
    [Type], 
    [ProductId], 
    [BatchId], 
    [CreatedTime], 
    [LastUpdatedTime], 
    [DeletedTime], 
    [IsDeleted], 
    [DeletedAt])
VALUES
    -- ProductItems for Batch 1 (2 from Koi Kohaku)
    ('pi-batch1-001', 'Koi Kohaku - #0006', 1050000, 'Kohaku', 'Japan', 'Unknown', 2, 'Medium', 'Koi', 'Territorial', '50', '20', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 1, 'Approved', 
    '1', 'batch-001', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('pi-batch1-002', 'Koi Kohaku - #0007', 1100000, 'Kohaku', 'Japan', 'Unknown', 3, 'Medium', 'Koi', 'Friendly', '60', '21', 'Medium', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 1, 'Approved', 
    '2', 'batch-001', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for Batch 1 (2 from Koi Ogon)
    ('pi-batch1-003', 'Koi Ogon - #0006', 1500000, 'Ogon', 'Japan', 'Male', 3, 'Large', 'Koi', 'Territorial', '55', '18', 'High', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 1, 'Approved', 
    '3', 'batch-001', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('pi-batch1-004', 'Koi Ogon - #0008', 1550000, 'Ogon', 'Japan', 'Female', 2, 'Large', 'Koi', 'Shy', '75', '19', 'High', '7', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 1, 'Approved', 
    '4', 'batch-001', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);

GO




-- Insert ProductItems for Batch 2
INSERT INTO [dbo].[ProductItem]
    ([Id], 
    [Name], 
    [Price], 
    [Category], 
    [Origin], 
    [Sex], 
    [Age], 
    [Size], 
    [Species], 
    [Personality], 
    [FoodAmount], 
    [WaterTemp], 
    [MineralContent], 
    [PH], 
    [ImageUrl], 
    [Quantity], 
    [Type], 
    [ProductId], 
    [BatchId], 
    [CreatedTime], 
    [LastUpdatedTime], 
    [DeletedTime], 
    [IsDeleted], 
    [DeletedAt])
VALUES
    -- ProductItems for Batch 2 (2 from Koi Showa)
    ('pi-batch2-001', 'Koi Showa - #0009', 2000000, 'Showa', 'Japan', 'Male', 1, 'Small', 'Koi', 'Shy', 80, 22, 'Low', 7, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 70, 'Approved', 
    '1', 'batch-002', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('pi-batch2-002', 'Koi Showa - #0008', 2100000, 'Showa', 'Japan', 'Female', 3, 'Small', 'Koi', 'Friendly', 90, 21, 'Medium', 7, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 50, 'Approved', 
    '2', 'batch-002', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

    -- ProductItems for Batch 2 (2 from Koi Tancho)
    ('pi-batch2-003', 'Koi Tancho - #0007', 1200000, 'Tancho', 'Japan', 'Female', 2, 'Small', 'Koi', 'Friendly', 60, 20, 'Medium', 7, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 80, 'Approved', 
    '4', 'batch-002', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('pi-batch2-004', 'Koi Tancho - #0008', 1250000, 'Tancho', 'Japan', 'Male', 3, 'Large', 'Koi', 'Shy', 70, 22, 'Medium', 7, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 60, 'Approved', 
    '5', 'batch-002', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);
GO

-- Insert ProductItems for Batch 3
INSERT INTO [dbo].[ProductItem]
    ([Id], 
    [Name], 
    [Price], 
    [Category], 
    [Origin], 
    [Sex], 
    [Age], 
    [Size], 
    [Species], 
    [Personality], 
    [FoodAmount], 
    [WaterTemp], 
    [MineralContent], 
    [PH], 
    [ImageUrl], 
    [Quantity], 
    [Type], 
    [ProductId], 
    [BatchId], 
    [CreatedTime], 
    [LastUpdatedTime], 
    [DeletedTime], 
    [IsDeleted], 
    [DeletedAt])
VALUES
    -- ProductItem for Batch 3
    ('pi-batch3-001', 'Koi Bekko - #0008', 1150000, 'Bekko', 'Japan', 'Unknown', 2, 'Medium', 'Koi', 'Calm', 60, 18, 'Medium', 7, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg', 40, 'Approved', 
    '5', 'batch-003', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);
GO

-- Insert ProductItems for Batch 4
INSERT INTO [dbo].[ProductItem]
    ([Id], 
    [Name], 
    [Price], 
    [Category], 
    [Origin], 
    [Sex], 
    [Age], 
    [Size], 
    [Species], 
    [Personality], 
    [FoodAmount], 
    [WaterTemp], 
    [MineralContent], 
    [PH], 
    [ImageUrl], 
    [Quantity], 
    [Type], 
    [ProductId], 
    [BatchId], 
    [CreatedTime], 
    [LastUpdatedTime], 
    [DeletedTime], 
    [IsDeleted], 
    [DeletedAt])
VALUES
    -- ProductItem for Batch 4
    ('pi-batch4-001', 'Koi Ginrin - #0008', 1250000, 'Ginrin', 'Japan', 'Female', 3, 'Large', 'Koi', 'Friendly', 75, 22, 'Low', 7, 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027702/products/ginrin.jpg', 50, 'Approved', 
    '1', 'batch-004', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);
GO


-- Insert certificates into Certificate table
INSERT INTO [dbo].[Certificate] 
	([Id], 
	[Name], 
	[ImageUrl], 
	[CreatedTime], 
	[LastUpdatedTime], 
	[IsDeleted]) 
VALUES 
	('ab3d6e2d2bc7417a825cf1113fb8f7f5', N'ISO 9001', 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1730188615/certificates/mwhssoyn08g26aaf94d1.jpg', GETDATE(), GETDATE(), 0),
	('32edff23aa5a4aad802d98b356c89688', N'Fair Trade', 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1730188614/certificates/uhqhcdljmi35kcpcpbmr.jpg', GETDATE(), GETDATE(), 0),
	('1bc906b277b84b149b1bb2f01f6cdac4', N'Organic Certification', 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1730188613/certificates/lssx2px3tthkzyszk6jv.jpg', GETDATE(), GETDATE(), 0),
	('28bdb42d6bcf4ad6a836009b40c4367e', N'Quality Assurance', 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1730188613/certificates/dhwqviydt6frio9dazk6.jpg', GETDATE(), GETDATE(), 0);

-- Insert product certificates in to ProductCertificate table
INSERT INTO [dbo].[ProductCertificate]
	([Id], 
	[Provider], 
	[CertificateId], 
	[ProductItemId], 
	[CreatedTime], 
	[LastUpdatedTime], 
	[IsDeleted])
VALUES
	('87a612eef7574f4aac35c4e73cd41781', 'Spring viraemia of carp (SVC)', 'ab3d6e2d2bc7417a825cf1113fb8f7f5', 'pi-batch2-002', GETDATE(), GETDATE(), 0),
	('a7a81c9f2da64422bbd92e86c754f927', 'Koi herpesvirus disease (KHV)', '32edff23aa5a4aad802d98b356c89688', 'pi-batch2-002', GETDATE(), GETDATE(), 0),
	('76a7951ab7d24a93b925ba4eeb1845a2', 'Epizootic ulcerative syndrome (EUS)', '1bc906b277b84b149b1bb2f01f6cdac4', 'pi-batch2-001', GETDATE(), GETDATE(), 0),
	('83d67800d54a48f08ec14436ccd1cdd8', 'Epizootic haematopoietic necrosis (EHN)', '28bdb42d6bcf4ad6a836009b40c4367e', 'pi-batch2-001', GETDATE(), GETDATE(), 0),
	('0ca9eab22a8442709cbd699085f556b6', 'White spot disease (WSD)', 'ab3d6e2d2bc7417a825cf1113fb8f7f5', 'pi-batch2-001', GETDATE(), GETDATE(), 0),
	('30d1284b00e649c28bb988b82979658b', 'Furunculosis (Aeromonas Salmonicida)', '32edff23aa5a4aad802d98b356c89688', '6', GETDATE(), GETDATE(), 0),
	('ebc5e31111dd4f1abcaa2a7ee335613f', 'Spring viraemia of carp (SVC)', '1bc906b277b84b149b1bb2f01f6cdac4', '7', GETDATE(), GETDATE(), 0),
	('41e79bd13d794e54921abbc597601b5d', 'Koi herpesvirus disease (KHV)', '28bdb42d6bcf4ad6a836009b40c4367e', '7', GETDATE(), GETDATE(), 0),
	('72efbad9552147eca704d95fdbaa134a', 'Epizootic ulcerative syndrome (EUS)', 'ab3d6e2d2bc7417a825cf1113fb8f7f5', '8', GETDATE(), GETDATE(), 0),
	('f53e97deb527426f91267bb8b316a350', 'Epizootic haematopoietic necrosis (EHN)', '32edff23aa5a4aad802d98b356c89688', '9', GETDATE(), GETDATE(), 0),
	('7a5e27df7b0c481cb2633889655ca92e', 'White spot disease (WSD)', '1bc906b277b84b149b1bb2f01f6cdac4', '10', GETDATE(), GETDATE(), 0),
	('2126db843f704c929ccc02eea482ea1e', 'Furunculosis (Aeromonas Salmonicida)', '28bdb42d6bcf4ad6a836009b40c4367e', '11', GETDATE(), GETDATE(), 0),
	('2cd8c720057f4020b01b81eba393607b', 'Spring viraemia of carp (SVC)', 'ab3d6e2d2bc7417a825cf1113fb8f7f5', '12', GETDATE(), GETDATE(), 0),
	('64818625770544b79074aec86891df97', 'Koi herpesvirus disease (KHV)', '32edff23aa5a4aad802d98b356c89688', '14', GETDATE(), GETDATE(), 0),
	('f9bc3ee5720541a989070efb8420deb7', 'Epizootic ulcerative syndrome (EUS)', '1bc906b277b84b149b1bb2f01f6cdac4', '1', GETDATE(), GETDATE(), 0),
	('a71e589a61b746398d6269f8de080ef6', 'Epizootic haematopoietic necrosis (EHN)', '28bdb42d6bcf4ad6a836009b40c4367e', '2', GETDATE(), GETDATE(), 0),
	('b76612e8c6154ec99b6c850284870f91', 'White spot disease (WSD)', 'ab3d6e2d2bc7417a825cf1113fb8f7f5', '3', GETDATE(), GETDATE(), 0),
	('c9bee55418c24e019147f8b167f26c28', 'Spring viraemia of carp (SVC)', '32edff23aa5a4aad802d98b356c89688', '4', GETDATE(), GETDATE(), 0),
	('29de0dff5816465b8ce148d791813600', 'Koi herpesvirus disease (KHV)', '1bc906b277b84b149b1bb2f01f6cdac4', '5', GETDATE(), GETDATE(), 0),
	('fbdc2b32134b4990b42ae4dd8316c735', 'Epizootic ulcerative syndrome (EUS)', '28bdb42d6bcf4ad6a836009b40c4367e', '6', GETDATE(), GETDATE(), 0),
	('48e01d8e1d7d4a20ba101c66f8e9ff44', 'Epizootic haematopoietic necrosis (EHN)', 'ab3d6e2d2bc7417a825cf1113fb8f7f5', '7', GETDATE(), GETDATE(), 0);
GO

COMMIT;