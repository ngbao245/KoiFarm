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
    ('41c2fbe4b02549c587837a3c4658e02a', 'Koi Kohaku', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 
	'Koi Kohaku Là dòng Koi được yêu thích nhất. Là dòng Koi được lai tạo đầu tiên tại Nhật. Có lịch sử lâu đời (từ TK 19). Koi nổi bật với nước da trắng hơn tuyết, các điểm đỏ Hi lớn, phân bố đều, hài hòa trên thân. Kohaku nghĩa là đỏ và trắng. Kohaku gồm 7 phiên bản: menkaburi Kohaku; Kuchibeni Kohaku; Inazuma Kohalku; Maruten Kohaku; Straight Hi-kohaku; Tancho Kohaku; Doitsu Kohaku; Nidan Kohaku; Ginrin kohaku', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('d93645edc83a4a0e8cfcbb9f2d819cc9', 'Koi Ogon', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 
	'Cá Koi Ogon Là dòng koi màu ánh bạc kim loại, thuộc dòng đơn sắc, 1 màu. 3 màu phổ biến nhất: vàng, bạch kim, cam. Được nhân giống năm 1946 từ nhà lai tạo Satawa Aoki từ 1 con chép vàng hoang dã ông mua năm 1921. Koi Ogon được lai tạo với 6 siêu phẩm màu sắc khác nhau, cấu trúc body chắc chắn, khỏe mạnh bao gồm: Platinium Ogon, Yamabuki Ogon, Hi Ogon, Orenji Ogon, Mukashi Ogon.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4f89cbd2af1e422fbfbfb1b78eb5d620', 'Koi Showa', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 
	'Cá Koi Showa (Showa sanshoku) Lai tạo thành công năm 1927 từ Koi kohaku. 3 màu chính trên thân: đỏ (Hi), đen (sumi), trắng (Shiroji). Koi Showa có mảng Sumi màu đen lớn hơn, lan rộng ở đầu (trong khi sanke không có màu đen trên đầu – là dấu hiệu phân biệt với sanke). Nhiều người nhầm lẫn Showa với sanke vì màu sắc giống nhau nhưng showa da có nền đen; trắng và đỏ là những mảng màu trên nền đen ấy. Sanke thì nền da trắng; đen và đỏ là những vệt màu trên nền trắng đó. Showa bao gồm dòng: Hi Showa; Kinsai showa; Tancho Showa; Maruten Showa; Doitsu Showa; Ginrin Showa.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('3369c7cbba56453098a3d9487f22e4bc', 'Koi Tancho', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 
	'Cá Koi Tancho: Có mảng đỏ Hi to tròn trên đỉnh đầu. Mình trắng muốt. Biểu tượng “quốc kỳ sống” của Nhật Bản. Gồm 3 loại: Tancho Showa; Tancho Sanke; Tancho Goshiki.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('64eb0df76e7b4c7ab2cdb7e94ddc8d8f', 'Koi Bekko', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg', 
	'Cá Koi Bekko: Cùng họ với Koi Utsuri khác ở điểm Koi bekko có đốm đen trên thân nhỏ hơn so với Utsuri. Hoa văn cũng khác. Nếu so tính thẩm mỹ thì Utsiri chiếm ưu thế hơn.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4ef098cfc4f94649bbb7c48b280cd31a', 'Koi Doitsu', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027691/products/doitsu.jpg', 
	'Cá Koi Doitsu Là dòng Koi da trơn, có hàng vảy rồng to, đều đối xứng nhau 2 bên lưng. Một số em Koi Doitsu còn cả hàng vảy rồng chạy dọc 2 bên hông', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4e55d312c48d4071af5b678b391646d5', 'Koi Ginrin', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027702/products/ginrin.jpg', 
	'Cá Koi Ginrin : Có vảy kim loại (lấp lánh, phát ánh kim). Tên tiếng anh là váy vàng bạc. Cá nổi bật với vảy lấp lánh tương phản với làn da mịn màng. Lớp vảy đồng đều, tạo hiệu ứng ánh sáng rất đẹp mắt.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('bc1b07abc67945cabcfb9dbb4cd26763', 'Koi Goshiki', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027714/products/goshiki.jpg', 
	'Cá Koi Goshiki: Có màu nền trắng với mắt lưới màu đen và xanh, được phủ bởi các đốm màu nâu đỏ như Kohaku. Goshiki khi ở trong nước lạnh màu sắc sẽ tối hơn so với nhiệt độ thường. 1 em Goshiki đẹp phải có đầu sạch, màu trắng và đỏ. Đỉnh đầu không có bất kỳ Sumi đen nào.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('01b364370e6b428581d3d2a344a197b2', 'Koi Benigoi', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027660/products/benigoi.jpg', 
	'Cá koi Benigoi (Tmud pond): có màu đỏ toàn thân (đỏ bình thường) từ chóp mũi đến đuôi (có con màu đỏ ớt). Chỉ có màu đỏ duy nhất, vây ngực không được có màu trắng. Nếu có là Koi Aka Hajio chứ không phải Koi Benigoi.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('5b160e9cee4c40a79e1d8a7cf80f3d0b', 'Koi Asagi', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028069/products/asagi.jpg', 
	'Cá Koi Asagi : Asagi có vây lưới Furukin màu xanh xám hoặc xanh sáng trên lưng. Hai đường biên hông có màu đỏ (1 số em màu vàng hoặc kem). Tại vây, bìa mang, miệng Koi Asagi có thể có các tia đỏ. Koi Asagi đẹp là mảng lưới vẩy trên lưng phải đều, đẹp mắt. Vẩy mỏng, thưa, đứt đoạn kém giá trị.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('e4bfaf2848c2429ab06c85405707a925', 'Koi Platium', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027810/products/platium.jpg',
	'Koi Platinum được ví như “thỏi bạc phát sáng” dưới hồ. Màu trắng bạch kim toàn thân là điểm nhấn độc đáo nhất của koi Platinum. Ai chơi Koi nhất định phải được sở hữu 1 em Bạch kim Platinum của riêng mình. Ở Việt Nam rất ít koi Platinum thuần chủng của Nhật , 80% bố mẹ là Koi F1 hoặc koi Việt. OnKoi Quang Minh là 1 trong số ít đối tác của Dainichi koi Farm Nhật Bản. Chúng tôi hiện đang có 1 vài em Koi Platinum với kết cấu body cực chuẩn, mạng lưới Fukurin đỉnh cao.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
	('e4048e4bbf8d480fadfc8c7d0e3e981d', 'Koi Shusui', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027850/products/shusui.jpg', 
	'Cá Koi Shusui Koi Shusui giống với Asagi nhưng koi shusui lại có da trơn. Shusui đẹp khi màu đỏ Hi kéo dài 2 bên hông từ bụng đến đuôi và vảy đầy đủ, tạo thành hàng dài thẳng hàng.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('ca0884736a6542a98d0206716c2d6846', 'Koi Taisho Sanke', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027825/products/sanke.jpg', 
	'Cá Koi Taisho Sanke Được lai tạo từ Kohaku, có màu đen pha đỏ và trắng trên cơ thể. 1 em Koi taisho Sanke đẹp phải có màu trắng sạch, trắng rực rỡ như tuyết.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('5a84123588704afb83ca029c8eca218b', 'Koi Utsurimono', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027871/products/utsurimono.jpg', 
	'Cá Koi Utsurimono (Hikariutsuri / Hikari Utsurimono) Dòng Koi da đen với các mảng màu trắng, đỏ hoặc vàng tựa mô hình hoa văn ngựa vằn. Hoa văn vàng trên nền đen phổ biến nhất, lâu đời nhất. Gồm 4 phiên bản Hi Utsuri (hai màu đen và đỏ); Ki Utsuri (hai màu đen và vàng); Kin Showa (3 màu đỏ đen, trắng bạc); Shiro Utsuri (hai màu đen và trắng).', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('2f7a5273a19240c58db1f9e0b157b208', 'Koi Chagoi', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027677/products/chagoi.jpg', 
	'Chagoi là dòng Koi đơn sắc, không có ánh kim loại. Màu của nó có thể là màu trà cho Chagoi, màu lục nhạt cho Koi Midorigoi và xanh dương xám cho Soragoi. Koi Chagoi tính tình thân thiện, ngoan ngoãn, kích thước lớn nên tượng trưng cho sự may mắn.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('611ce6b1bca74c909b42609ee18f674d', 'Koi Karashi', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027725/products/karashi.jpg', 
	'Cá Koi Karashi có thể là da trơn (Doitsu), vảy chiếu (Ginrin) và loại vảy thông thường. Độ biến đổi màu sắc của nó có thể bắt đầu từ màu be, đến vàng nhạt, và cuối cùng là vàng đậm', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('ca226aab2bc946ce839057c492574219', 'Koi Kawarimono', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028056/products/kawarimono.jpg', 
	'Cá Koi Kawarimono là 1 nhóm phân loại các loại Koi lai tạo với các nhóm Koi khác gọi chung là Kawarimono. Chúng được chia thành 3 nhóm Koi đơn sắc (single-colored Koi), màu đen tạp (black Koi Breeds) và Koi kawarimono', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('1368ee1fa303461b87ddbb5b26d6d8d0', 'Koi Kujaku', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027762/products/kujaku.jpg', 
	'Cá Koi Kujaku có màu nền bằng kim loại phủ bởi màu đỏ, cam hoặc màu vàng kim loại, tạo thành hiệu ứng đặc biệt,', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('2db74b614ecc44dd9b2c10429ff49cb3', 'Koi Matsuba', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027774/products/matsuba.jpg', 
	'Matsuba là một loại cá Koi có màu kim loại với hoa văn lưới màu đen. Cơ thể của cá koi Matsuba có một màu với vảy hình nón thông. Loại cá này có lớp vảy óng ánh vô cùng đẹp mắt. Để tìm hiểu kỹ hơn về giống cá Koi Matsuba này hãy cùng tham khảo bài viết dưới đây của chúng tôi nhé', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('af42c9de1e8048c7a7d8b254dc71afea', 'Koi KikoKryu', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027737/products/kikokryu.jpg', 
	'Cá Koi KikoKryu& Kikokuryu thực chất là 1 kumonryu kim loại. Có làn da bạch kim sáng bóng, vây với sắc màu sumi đen sâu trong suốt. Được lai tạo từ Kim Ogon và kumonryu. Nhiệt độ, nước, di truyền là yếu tố quyết định mô hình màu sắc, đậm, nhạt của KikoKryu.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('af301c6d526849e4bfde6e2ead5be943', 'Koi Ochiba', 0, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027785/products/ochiba.jpg', 
	'Matsuba là một loại cá Koi có màu kim loại với hoa văn lưới màu đen. Cơ thể của cá koi Matsuba có một màu với vảy hình nón thông. Loại cá này có lớp vảy óng ánh vô cùng đẹp mắt. Để tìm hiểu kỹ hơn về giống cá Koi Matsuba này hãy cùng tham khảo bài viết dưới đây của chúng tôi nhé', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);

COMMIT;
