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

-- Check and Delete if any data exists in the Role table
IF EXISTS (SELECT 1 FROM [dbo].[ProductItem])
BEGIN
    DELETE FROM [dbo].[ProductItem];
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
    ('41c2fbe4b02549c587837a3c4658e02a', 'Koi Kohaku', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 
	N'Koi Kohaku Là dòng Koi được yêu thích nhất. Là dòng Koi được lai tạo đầu tiên tại Nhật. Có lịch sử lâu đời (từ TK 19). Koi nổi bật với nước da trắng hơn tuyết, các điểm đỏ Hi lớn, phân bố đều, hài hòa trên thân. Kohaku nghĩa là đỏ và trắng. Kohaku gồm 7 phiên bản: menkaburi Kohaku; Kuchibeni Kohaku; Inazuma Kohalku; Maruten Kohaku; Straight Hi-kohaku; Tancho Kohaku; Doitsu Kohaku; Nidan Kohaku; Ginrin kohaku', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('d93645edc83a4a0e8cfcbb9f2d819cc9', 'Koi Ogon', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 
	N'Cá Koi Ogon Là dòng koi màu ánh bạc kim loại, thuộc dòng đơn sắc, 1 màu. 3 màu phổ biến nhất: vàng, bạch kim, cam. Được nhân giống năm 1946 từ nhà lai tạo Satawa Aoki từ 1 con chép vàng hoang dã ông mua năm 1921. Koi Ogon được lai tạo với 6 siêu phẩm màu sắc khác nhau, cấu trúc body chắc chắn, khỏe mạnh bao gồm: Platinium Ogon, Yamabuki Ogon, Hi Ogon, Orenji Ogon, Mukashi Ogon.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4f89cbd2af1e422fbfbfb1b78eb5d620', 'Koi Showa', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 
	N'Cá Koi Showa (Showa sanshoku) Lai tạo thành công năm 1927 từ Koi kohaku. 3 màu chính trên thân: đỏ (Hi), đen (sumi), trắng (Shiroji). Koi Showa có mảng Sumi màu đen lớn hơn, lan rộng ở đầu (trong khi sanke không có màu đen trên đầu – là dấu hiệu phân biệt với sanke). Nhiều người nhầm lẫn Showa với sanke vì màu sắc giống nhau nhưng showa da có nền đen; trắng và đỏ là những mảng màu trên nền đen ấy. Sanke thì nền da trắng; đen và đỏ là những vệt màu trên nền trắng đó. Showa bao gồm dòng: Hi Showa; Kinsai showa; Tancho Showa; Maruten Showa; Doitsu Showa; Ginrin Showa.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('3369c7cbba56453098a3d9487f22e4bc', 'Koi Tancho', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 
	N'Cá Koi Tancho: Có mảng đỏ Hi to tròn trên đỉnh đầu. Mình trắng muốt. Biểu tượng “quốc kỳ sống” của Nhật Bản. Gồm 3 loại: Tancho Showa; Tancho Sanke; Tancho Goshiki.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('64eb0df76e7b4c7ab2cdb7e94ddc8d8f', 'Koi Bekko', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg', 
	N'Cá Koi Bekko: Cùng họ với Koi Utsuri khác ở điểm Koi bekko có đốm đen trên thân nhỏ hơn so với Utsuri. Hoa văn cũng khác. Nếu so tính thẩm mỹ thì Utsiri chiếm ưu thế hơn.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4ef098cfc4f94649bbb7c48b280cd31a', 'Koi Doitsu', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027691/products/doitsu.jpg', 
	N'Cá Koi Doitsu Là dòng Koi da trơn, có hàng vảy rồng to, đều đối xứng nhau 2 bên lưng. Một số em Koi Doitsu còn cả hàng vảy rồng chạy dọc 2 bên hông', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4e55d312c48d4071af5b678b391646d5', 'Koi Ginrin', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027702/products/ginrin.jpg', 
	N'Cá Koi Ginrin : Có vảy kim loại (lấp lánh, phát ánh kim). Tên tiếng anh là váy vàng bạc. Cá nổi bật với vảy lấp lánh tương phản với làn da mịn màng. Lớp vảy đồng đều, tạo hiệu ứng ánh sáng rất đẹp mắt.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('bc1b07abc67945cabcfb9dbb4cd26763', 'Koi Goshiki', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027714/products/goshiki.jpg', 
	N'Cá Koi Goshiki: Có màu nền trắng với mắt lưới màu đen và xanh, được phủ bởi các đốm màu nâu đỏ như Kohaku. Goshiki khi ở trong nước lạnh màu sắc sẽ tối hơn so với nhiệt độ thường. 1 em Goshiki đẹp phải có đầu sạch, màu trắng và đỏ. Đỉnh đầu không có bất kỳ Sumi đen nào.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('01b364370e6b428581d3d2a344a197b2', 'Koi Benigoi', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027660/products/benigoi.jpg', 
	N'Cá koi Benigoi (Tmud pond): có màu đỏ toàn thân (đỏ bình thường) từ chóp mũi đến đuôi (có con màu đỏ ớt). Chỉ có màu đỏ duy nhất, vây ngực không được có màu trắng. Nếu có là Koi Aka Hajio chứ không phải Koi Benigoi.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('5b160e9cee4c40a79e1d8a7cf80f3d0b', 'Koi Asagi', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028069/products/asagi.jpg', 
	N'Cá Koi Asagi : Asagi có vây lưới Furukin màu xanh xám hoặc xanh sáng trên lưng. Hai đường biên hông có màu đỏ (1 số em màu vàng hoặc kem). Tại vây, bìa mang, miệng Koi Asagi có thể có các tia đỏ. Koi Asagi đẹp là mảng lưới vẩy trên lưng phải đều, đẹp mắt. Vẩy mỏng, thưa, đứt đoạn kém giá trị.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('e4bfaf2848c2429ab06c85405707a925', 'Koi Platium', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027810/products/platium.jpg',
	N'Koi Platinum được ví như “thỏi bạc phát sáng” dưới hồ. Màu trắng bạch kim toàn thân là điểm nhấn độc đáo nhất của koi Platinum. Ai chơi Koi nhất định phải được sở hữu 1 em Bạch kim Platinum của riêng mình. Ở Việt Nam rất ít koi Platinum thuần chủng của Nhật , 80% bố mẹ là Koi F1 hoặc koi Việt. OnKoi Quang Minh là 1 trong số ít đối tác của Dainichi koi Farm Nhật Bản. Chúng tôi hiện đang có 1 vài em Koi Platinum với kết cấu body cực chuẩn, mạng lưới Fukurin đỉnh cao.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
	('e4048e4bbf8d480fadfc8c7d0e3e981d', 'Koi Shusui', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027850/products/shusui.jpg', 
	N'Cá Koi Shusui Koi Shusui giống với Asagi nhưng koi shusui lại có da trơn. Shusui đẹp khi màu đỏ Hi kéo dài 2 bên hông từ bụng đến đuôi và vảy đầy đủ, tạo thành hàng dài thẳng hàng.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('ca0884736a6542a98d0206716c2d6846', 'Koi Taisho Sanke', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027825/products/sanke.jpg', 
	N'Cá Koi Taisho Sanke Được lai tạo từ Kohaku, có màu đen pha đỏ và trắng trên cơ thể. 1 em Koi taisho Sanke đẹp phải có màu trắng sạch, trắng rực rỡ như tuyết.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('5a84123588704afb83ca029c8eca218b', 'Koi Utsurimono', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027871/products/utsurimono.jpg', 
	N'Cá Koi Utsurimono (Hikariutsuri / Hikari Utsurimono) Dòng Koi da đen với các mảng màu trắng, đỏ hoặc vàng tựa mô hình hoa văn ngựa vằn. Hoa văn vàng trên nền đen phổ biến nhất, lâu đời nhất. Gồm 4 phiên bản Hi Utsuri (hai màu đen và đỏ); Ki Utsuri (hai màu đen và vàng); Kin Showa (3 màu đỏ đen, trắng bạc); Shiro Utsuri (hai màu đen và trắng).', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('2f7a5273a19240c58db1f9e0b157b208', 'Koi Chagoi', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027677/products/chagoi.jpg', 
	N'Chagoi là dòng Koi đơn sắc, không có ánh kim loại. Màu của nó có thể là màu trà cho Chagoi, màu lục nhạt cho Koi Midorigoi và xanh dương xám cho Soragoi. Koi Chagoi tính tình thân thiện, ngoan ngoãn, kích thước lớn nên tượng trưng cho sự may mắn.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('611ce6b1bca74c909b42609ee18f674d', 'Koi Karashi', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027725/products/karashi.jpg', 
	N'Cá Koi Karashi có thể là da trơn (Doitsu), vảy chiếu (Ginrin) và loại vảy thông thường. Độ biến đổi màu sắc của nó có thể bắt đầu từ màu be, đến vàng nhạt, và cuối cùng là vàng đậm', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('ca226aab2bc946ce839057c492574219', 'Koi Kawarimono', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028056/products/kawarimono.jpg', 
	N'Cá Koi Kawarimono là 1 nhóm phân loại các loại Koi lai tạo với các nhóm Koi khác gọi chung là Kawarimono. Chúng được chia thành 3 nhóm Koi đơn sắc (single-colored Koi), màu đen tạp (black Koi Breeds) và Koi kawarimono', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('1368ee1fa303461b87ddbb5b26d6d8d0', 'Koi Kujaku', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027762/products/kujaku.jpg', 
	N'Cá Koi Kujaku có màu nền bằng kim loại phủ bởi màu đỏ, cam hoặc màu vàng kim loại, tạo thành hiệu ứng đặc biệt,', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('2db74b614ecc44dd9b2c10429ff49cb3', 'Koi Matsuba', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027774/products/matsuba.jpg', 
	N'Matsuba là một loại cá Koi có màu kim loại với hoa văn lưới màu đen. Cơ thể của cá koi Matsuba có một màu với vảy hình nón thông. Loại cá này có lớp vảy óng ánh vô cùng đẹp mắt. Để tìm hiểu kỹ hơn về giống cá Koi Matsuba này hãy cùng tham khảo bài viết dưới đây của chúng tôi nhé', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('af42c9de1e8048c7a7d8b254dc71afea', 'Koi KikoKryu', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027737/products/kikokryu.jpg', 
	N'Cá Koi KikoKryu Kikokuryu thực chất là 1 kumonryu kim loại. Có làn da bạch kim sáng bóng, vây với sắc màu sumi đen sâu trong suốt. Được lai tạo từ Kim Ogon và kumonryu. Nhiệt độ, nước, di truyền là yếu tố quyết định mô hình màu sắc, đậm, nhạt của KikoKryu.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('af301c6d526849e4bfde6e2ead5be943', 'Koi Ochiba', 100, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027785/products/ochiba.jpg', 
	N'Matsuba là một loại cá Koi có màu kim loại với hoa văn lưới màu đen. Cơ thể của cá koi Matsuba có một màu với vảy hình nón thông. Loại cá này có lớp vảy óng ánh vô cùng đẹp mắt. Để tìm hiểu kỹ hơn về giống cá Koi Matsuba này hãy cùng tham khảo bài viết dưới đây của chúng tôi nhé', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);

-- Insert ProductItems for each product
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
	-- ProductItem for 'Koi Kohaku'
	('1', 'Koi Kohaku', 1000000, 'Kohaku', 'Japan', 'Unknown', 2, 'Medium', 'Koi', 'Calm', 50, 20, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 100, 'Fish', 
	'41c2fbe4b02549c587837a3c4658e02a', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Ogon'
	('2', 'Koi Ogon', 1500000, 'Ogon', 'Japan', 'Unknown', 3, 'Large', 'Koi', 'Active', 70, 18, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 100, 'Fish', 
	'd93645edc83a4a0e8cfcbb9f2d819cc9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Showa'
	('3', 'Koi Showa', 2000000, 'Showa', 'Japan', 'Male', 1, 'Large', 'Koi', 'Aggressive', 80, 22, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 100, 'Fish', 
	'4f89cbd2af1e422fbfbfb1b78eb5d620', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Tancho'
	('4', 'Koi Tancho', 1200000, 'Tancho', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Friendly', 60, 20, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 100, 'Fish', 
	'3369c7cbba56453098a3d9487f22e4bc', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Bekko'
	('5', 'Koi Bekko', 1100000, 'Bekko', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Aggressive', 60, 18, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg', 100, 'Fish', 
	'64eb0df76e7b4c7ab2cdb7e94ddc8d8f', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Doitsu'
	('6', 'Koi Doitsu', 1300000, 'Doitsu', 'Japan', 'Unknown', 1, 'Small', 'Koi', 'Calm', 50, 19, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027691/products/doitsu.jpg', 100, 'Fish', 
	'4ef098cfc4f94649bbb7c48b280cd31a', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Ginrin'
	('7', 'Koi Ginrin', 2500000, 'Ginrin', 'Japan', 'Female', 4, 'Large', 'Koi', 'Friendly', 90, 22, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027702/products/ginrin.jpg', 100, 'Fish', 
	'4e55d312c48d4071af5b678b391646d5', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Goshiki'
	('8', 'Koi Goshiki', 1800000, 'Goshiki', 'Japan', 'Male', 2, 'Medium', 'Koi', 'Calm', 65, 19, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027714/products/goshiki.jpg', 100, 'Fish', 
	'bc1b07abc67945cabcfb9dbb4cd26763', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('9', 'Koi Benigoi', 1400000, 'Benigoi', 'Japan', 'Unknown', 2, 'Medium', 'Koi', 'Friendly', 55, 19, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027660/products/benigoi.jpg', 100, 'Fish', 
	'01b364370e6b428581d3d2a344a197b2', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Asagi'
	('10', 'Koi Asagi', 1900000, 'Asagi', 'Japan', 'Male', 3, 'Large', 'Koi', 'Calm', 65, 18, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028069/products/asagi.jpg', 100, 'Fish', 
	'5b160e9cee4c40a79e1d8a7cf80f3d0b', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Platium'
	('11', 'Koi Platium', 2200000, 'Platinum', 'Japan', 'Female', 4, 'Large', 'Koi', 'Calm', 75, 20, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027810/products/platium.jpg', 100, 'Fish', 
	'e4bfaf2848c2429ab06c85405707a925', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Shusui'
	('12', 'Koi Shusui', 1600000, 'Shusui', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Friendly', 60, 19, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027850/products/shusui.jpg', 100, 'Fish', 
	'e4048e4bbf8d480fadfc8c7d0e3e981d', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Taisho Sanke'
	('13', 'Koi Taisho Sanke', 2400000, 'Sanke', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Aggressive', 75, 21, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027825/products/sanke.jpg', 100, 'Fish', 
	'ca0884736a6542a98d0206716c2d6846', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Utsurimono'
	('14', 'Koi Utsurimono', 2500000, 'Utsurimono', 'Japan', 'Male', 3, 'Large', 'Koi', 'Aggressive', 80, 22, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027871/products/utsurimono.jpg', 100, 'Fish', 
	'5a84123588704afb83ca029c8eca218b', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Chagoi'
	('15', 'Koi Chagoi', 1800000, 'Chagoi', 'Japan', 'Female', 2, 'Large', 'Koi', 'Friendly', 65, 19, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027677/products/chagoi.jpg', 100, 'Fish', 
	'2f7a5273a19240c58db1f9e0b157b208', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Karashi'
	('16', 'Koi Karashi', 1700000, 'Karashi', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Calm', 60, 20, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027725/products/karashi.jpg', 100, 'Fish', 
	'611ce6b1bca74c909b42609ee18f674d', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Kawarimono'
	('17', 'Koi Kawarimono', 2100000, 'Kawarimono', 'Japan', 'Female', 4, 'Large', 'Koi', 'Aggressive', 70, 21, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028056/products/kawarimono.jpg', 100, 'Fish', 
	'ca226aab2bc946ce839057c492574219', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Kujaku'
	('18', 'Koi Kujaku', 1900000, 'Kujaku', 'Japan', 'Unknown', 2, 'Medium', 'Koi', 'Calm', 65, 20, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027762/products/kujaku.jpg', 100, 'Fish', 
	'1368ee1fa303461b87ddbb5b26d6d8d0', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Matsuba'
	('19', 'Koi Matsuba', 1700000, 'Matsuba', 'Japan', 'Female', 3, 'Large', 'Koi', 'Friendly', 70, 19, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027774/products/matsuba.jpg', 100, 'Fish', 
	'2db74b614ecc44dd9b2c10429ff49cb3', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi KikoKryu'
	('20', 'Koi KikoKryu', 2000000, 'KikoKryu', 'Japan', 'Unknown', 3, 'Large', 'Koi', 'Aggressive', 80, 22, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027737/products/kikokryu.jpg', 100, 'Fish', 
	'af42c9de1e8048c7a7d8b254dc71afea', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Ochiba'
	('21', 'Koi Ochiba', 1500000, 'Ochiba', 'Japan', 'Male', 2, 'Medium', 'Koi', 'Friendly', 55, 19, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027785/products/ochiba.jpg', 100, 'Fish', 
	'af301c6d526849e4bfde6e2ead5be943', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);



INSERT INTO [KoiFarm].[dbo].[Consignment]
    ([Id],
    [UserId],
    [CreatedTime],
    [LastUpdatedTime],
    [DeletedTime],
    [IsDeleted],
    [DeletedAt])
VALUES
    ('c7d35ea3c567426ea7f71e9a9c2c147e', 'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    ('f8b24c9ae45847d9b3091c1d6bf72d89', 'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    ('a9c45d8eb12340f68ed7c2a5f4b3e901', 'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL);
GO

INSERT INTO [KoiFarm].[dbo].[ConsignmentItem]
    ([Id],
    [Name],
    [Category],
    [Origin],
    [Sex],
    [Age],
    [Size],
    [Species],
    [Checkedout],
    [Status],
    [ImageUrl],
    [ConsignmentId],
    [OrderItemId],
    [CreatedTime],
    [LastUpdatedTime],
    [DeletedTime],
    [IsDeleted],
    [DeletedAt])
VALUES
    -- First consignment items
    ('d1e45f2acd894728b567a901bcdef123', N'Kohaku Premium', 'Premium', 'Japan', 'Female', 2, '45cm', 'Kohaku', 0, 'Pending', 
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 
    'c7d35ea3c567426ea7f71e9a9c2c147e', NULL, GETDATE(), GETDATE(), NULL, 0, NULL),
    
    ('e2f56g3bde905839c678b012cdefg234', N'Tancho Elite', 'Elite', 'Japan', 'Male', 3, '50cm', 'Tancho', 0, 'Approved',
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg',
    'c7d35ea3c567426ea7f71e9a9c2c147e', NULL, GETDATE(), GETDATE(), NULL, 0, NULL),

    -- Second consignment items
    ('f3g67h4cef016940d789c123defgh345', N'Bekko Special', 'Special', 'Japan', 'Female', 1, '35cm', 'Bekko', 0, 'Pending',
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg',
    'f8b24c9ae45847d9b3091c1d6bf72d89', NULL, GETDATE(), GETDATE(), NULL, 0, NULL),
    
    ('g4h78i5dfg127051e890d234efghi456', N'Doitsu Classic', 'Classic', 'Japan', 'Male', 2, '40cm', 'Doitsu', 0, 'Approved',
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027691/products/doitsu.jpg',
    'f8b24c9ae45847d9b3091c1d6bf72d89', NULL, GETDATE(), GETDATE(), NULL, 0, NULL),

    -- Third consignment items
    ('h5i89j6efg238162f901e345fghij567', N'Ginrin Select', 'Select', 'Japan', 'Female', 4, '55cm', 'Ginrin', 0, 'Pending',
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027702/products/ginrin.jpg',
    'a9c45d8eb12340f68ed7c2a5f4b3e901', NULL, GETDATE(), GETDATE(), NULL, 0, NULL),
    
    ('i6j90k7fgh349273g012f456ghijk678', N'Goshiki Deluxe', 'Deluxe', 'Japan', 'Male', 2, '48cm', 'Goshiki', 0, 'Approved',
    'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027714/products/goshiki.jpg',
    'a9c45d8eb12340f68ed7c2a5f4b3e901', NULL, GETDATE(), GETDATE(), NULL, 0, NULL);
GO



COMMIT;
