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
    (1, N'Kinh nghiệm nuôi cá Koi trong hồ ngoài trời', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSz2vIkk5DKxeUrIZYz9CxUTVdqkVmSdwDInQ&s', 
	N'Nuôi cá Koi trong hồ ngoài trời không chỉ là một sở thích mà còn là nghệ thuật đòi hỏi sự chăm sóc cẩn thận. Để có một hồ cá Koi đẹp và cá khỏe mạnh, bạn cần chú ý đến chất lượng nước, ánh sáng, và thức ăn. Nước trong hồ cần được giữ sạch, không chứa hóa chất độc hại và có mức pH ổn định từ 7.0 đến 8.5. Ánh sáng tự nhiên là yếu tố quan trọng để cá Koi phát triển màu sắc rực rỡ, tuy nhiên không nên để ánh sáng mặt trời chiếu trực tiếp vào hồ quá lâu, vì nó có thể gây tảo phát triển mạnh. Thức ăn cho cá Koi nên chứa đầy đủ dưỡng chất để hỗ trợ sự phát triển của chúng. Bạn có thể cho cá ăn từ 1-2 lần mỗi ngày, tránh cho ăn quá nhiều để không làm ô nhiễm nước. Ngoài ra, việc tạo không gian bơi rộng rãi, thoáng mát giúp cá Koi có điều kiện sinh trưởng tốt nhất.', 
	'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    (2, N'Bí quyết chọn cá Koi đẹp và khỏe', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSD3aVI1ftuyN1T6vqv90G5vCEZyK-ACDGvxQ&s', 
	N'Khi chọn cá Koi, bạn cần lưu ý đến màu sắc, hình dáng và sự linh hoạt của chúng. Một con cá Koi đẹp thường có màu sắc rực rỡ, phân bố đều trên toàn thân, không bị đốm hay phai màu. Màu trắng nên sáng, không vàng, và màu đỏ nên đậm, đều. Hình dáng của cá cũng rất quan trọng, cơ thể nên thon dài, cân đối và không có dấu hiệu dị tật. Khi mua cá, bạn nên chọn những con bơi lội linh hoạt, không có dấu hiệu lờ đờ hoặc bệnh tật. Quan sát kỹ vây, mắt và miệng của cá để đảm bảo chúng không có vết thương hoặc dấu hiệu của bệnh. Cá Koi khỏe mạnh thường có vây trong suốt, không bị rách, và da không có vết nứt hay nhiễm trùng.', 
	'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    (3, N'Các loại thức ăn tốt nhất cho cá Koi', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTiRrJ2YYAcDB2skplKiWW3k8VjRTiFR9eYhA&s', 
	N'Thức ăn cho cá Koi đóng vai trò quan trọng trong việc phát triển màu sắc và sức khỏe của chúng. Thức ăn chứa hàm lượng protein cao giúp cá tăng trưởng nhanh chóng, đồng thời cung cấp đầy đủ vitamin và khoáng chất để giữ cho cá luôn khỏe mạnh. Một số loại thức ăn tốt cho cá Koi bao gồm thức ăn dạng viên nổi, chứa nhiều tảo Spirulina, giúp tăng cường màu sắc của cá. Bạn cũng có thể bổ sung thức ăn tự nhiên như rau xanh, trái cây và tôm nhỏ để tạo sự đa dạng trong chế độ dinh dưỡng của cá. Lưu ý, không nên cho cá ăn quá nhiều vì điều này có thể dẫn đến ô nhiễm nước, làm cá dễ mắc bệnh.', 
	'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    (4, N'Cách chăm sóc cá Koi trong mùa đông', 'https://i.ytimg.com/vi/0ezGC5uIwLg/maxresdefault.jpg', 
	N'Nhiệt độ nước có ảnh hưởng lớn đến sức khỏe của cá Koi. Nhiệt độ lý tưởng cho cá Koi dao động từ 15°C đến 25°C. Khi nhiệt độ quá lạnh hoặc quá nóng, cá Koi có thể dễ bị căng thẳng và mắc các bệnh như nấm, ký sinh trùng. Trong mùa đông, nếu nhiệt độ giảm xuống dưới 10°C, cá Koi sẽ giảm hoạt động và dễ mắc bệnh hơn. Để giữ nhiệt độ ổn định, bạn có thể sử dụng hệ thống sưởi hồ vào mùa đông và che chắn hồ vào mùa hè để giảm bớt tác động của ánh sáng mặt trời. Đồng thời, việc kiểm tra nhiệt độ thường xuyên là rất cần thiết để đảm bảo môi trường sống tốt nhất cho cá Koi.', 
	'aa5fd505e603484ba3abd30223d0c29f', GETDATE(), GETDATE(), NULL, 0, NULL),
    (5, N'Làm thế nào để xây dựng hồ cá Koi hoàn hảo', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQC-00p63FEvh32z-kQ4HENh5OL6T6jreA_vA&s', 
	N'Xây dựng một hồ cá Koi hoàn hảo cần sự chuẩn bị kỹ lưỡng từ khâu thiết kế đến chọn vật liệu. Đầu tiên, bạn cần xác định kích thước và hình dáng hồ phù hợp với không gian sân vườn. Hồ cá Koi nên có độ sâu từ 1,2m trở lên để cá có không gian bơi thoải mái và tránh hiện tượng nước quá nóng vào mùa hè. Vật liệu xây dựng hồ nên được lựa chọn kỹ càng, ưu tiên sử dụng đá tự nhiên hoặc xi măng chịu nhiệt. Hệ thống lọc nước là yếu tố không thể thiếu, giúp giữ cho nước luôn sạch sẽ, loại bỏ các tạp chất và vi khuẩn gây hại cho cá. Ngoài ra, việc trang trí xung quanh hồ bằng cây cỏ và đèn chiếu sáng cũng giúp hồ cá Koi trở nên sinh động, hài hòa với thiên nhiên.', 
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
    ('41c2fbe4b02549c587837a3c4658e02a', 'Koi Kohaku', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 
	N'Koi Kohaku Là dòng Koi được yêu thích nhất. Là dòng Koi được lai tạo đầu tiên tại Nhật. Có lịch sử lâu đời (từ TK 19). Koi nổi bật với nước da trắng hơn tuyết, các điểm đỏ Hi lớn, phân bố đều, hài hòa trên thân. Kohaku nghĩa là đỏ và trắng. Kohaku gồm 7 phiên bản: menkaburi Kohaku; Kuchibeni Kohaku; Inazuma Kohalku; Maruten Kohaku; Straight Hi-kohaku; Tancho Kohaku; Doitsu Kohaku; Nidan Kohaku; Ginrin kohaku', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('d93645edc83a4a0e8cfcbb9f2d819cc9', 'Koi Ogon', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 
	N'Cá Koi Ogon Là dòng koi màu ánh bạc kim loại, thuộc dòng đơn sắc, 1 màu. 3 màu phổ biến nhất: vàng, bạch kim, cam. Được nhân giống năm 1946 từ nhà lai tạo Satawa Aoki từ 1 con chép vàng hoang dã ông mua năm 1921. Koi Ogon được lai tạo với 6 siêu phẩm màu sắc khác nhau, cấu trúc body chắc chắn, khỏe mạnh bao gồm: Platinium Ogon, Yamabuki Ogon, Hi Ogon, Orenji Ogon, Mukashi Ogon.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4f89cbd2af1e422fbfbfb1b78eb5d620', 'Koi Showa', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 
	N'Cá Koi Showa (Showa sanshoku) Lai tạo thành công năm 1927 từ Koi kohaku. 3 màu chính trên thân: đỏ (Hi), đen (sumi), trắng (Shiroji). Koi Showa có mảng Sumi màu đen lớn hơn, lan rộng ở đầu (trong khi sanke không có màu đen trên đầu – là dấu hiệu phân biệt với sanke). Nhiều người nhầm lẫn Showa với sanke vì màu sắc giống nhau nhưng showa da có nền đen; trắng và đỏ là những mảng màu trên nền đen ấy. Sanke thì nền da trắng; đen và đỏ là những vệt màu trên nền trắng đó. Showa bao gồm dòng: Hi Showa; Kinsai showa; Tancho Showa; Maruten Showa; Doitsu Showa; Ginrin Showa.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('3369c7cbba56453098a3d9487f22e4bc', 'Koi Tancho', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 
	N'Cá Koi Tancho: Có mảng đỏ Hi to tròn trên đỉnh đầu. Mình trắng muốt. Biểu tượng “quốc kỳ sống” của Nhật Bản. Gồm 3 loại: Tancho Showa; Tancho Sanke; Tancho Goshiki.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('64eb0df76e7b4c7ab2cdb7e94ddc8d8f', 'Koi Bekko', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg', 
	N'Cá Koi Bekko: Cùng họ với Koi Utsuri khác ở điểm Koi bekko có đốm đen trên thân nhỏ hơn so với Utsuri. Hoa văn cũng khác. Nếu so tính thẩm mỹ thì Utsiri chiếm ưu thế hơn.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4ef098cfc4f94649bbb7c48b280cd31a', 'Koi Doitsu', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027691/products/doitsu.jpg', 
	N'Cá Koi Doitsu Là dòng Koi da trơn, có hàng vảy rồng to, đều đối xứng nhau 2 bên lưng. Một số em Koi Doitsu còn cả hàng vảy rồng chạy dọc 2 bên hông', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('4e55d312c48d4071af5b678b391646d5', 'Koi Ginrin', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027702/products/ginrin.jpg', 
	N'Cá Koi Ginrin : Có vảy kim loại (lấp lánh, phát ánh kim). Tên tiếng anh là váy vàng bạc. Cá nổi bật với vảy lấp lánh tương phản với làn da mịn màng. Lớp vảy đồng đều, tạo hiệu ứng ánh sáng rất đẹp mắt.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('bc1b07abc67945cabcfb9dbb4cd26763', 'Koi Goshiki', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027714/products/goshiki.jpg', 
	N'Cá Koi Goshiki: Có màu nền trắng với mắt lưới màu đen và xanh, được phủ bởi các đốm màu nâu đỏ như Kohaku. Goshiki khi ở trong nước lạnh màu sắc sẽ tối hơn so với nhiệt độ thường. 1 em Goshiki đẹp phải có đầu sạch, màu trắng và đỏ. Đỉnh đầu không có bất kỳ Sumi đen nào.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('01b364370e6b428581d3d2a344a197b2', 'Koi Benigoi', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027660/products/benigoi.jpg', 
	N'Cá koi Benigoi (Tmud pond): có màu đỏ toàn thân (đỏ bình thường) từ chóp mũi đến đuôi (có con màu đỏ ớt). Chỉ có màu đỏ duy nhất, vây ngực không được có màu trắng. Nếu có là Koi Aka Hajio chứ không phải Koi Benigoi.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('5b160e9cee4c40a79e1d8a7cf80f3d0b', 'Koi Asagi', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028069/products/asagi.jpg', 
	N'Cá Koi Asagi : Asagi có vây lưới Furukin màu xanh xám hoặc xanh sáng trên lưng. Hai đường biên hông có màu đỏ (1 số em màu vàng hoặc kem). Tại vây, bìa mang, miệng Koi Asagi có thể có các tia đỏ. Koi Asagi đẹp là mảng lưới vẩy trên lưng phải đều, đẹp mắt. Vẩy mỏng, thưa, đứt đoạn kém giá trị.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('e4bfaf2848c2429ab06c85405707a925', 'Koi Platium', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027810/products/platium.jpg',
	N'Koi Platinum được ví như “thỏi bạc phát sáng” dưới hồ. Màu trắng bạch kim toàn thân là điểm nhấn độc đáo nhất của koi Platinum. Ai chơi Koi nhất định phải được sở hữu 1 em Bạch kim Platinum của riêng mình. Ở Việt Nam rất ít koi Platinum thuần chủng của Nhật , 80% bố mẹ là Koi F1 hoặc koi Việt. OnKoi Quang Minh là 1 trong số ít đối tác của Dainichi koi Farm Nhật Bản. Chúng tôi hiện đang có 1 vài em Koi Platinum với kết cấu body cực chuẩn, mạng lưới Fukurin đỉnh cao.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
	('e4048e4bbf8d480fadfc8c7d0e3e981d', 'Koi Shusui', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027850/products/shusui.jpg', 
	N'Cá Koi Shusui Koi Shusui giống với Asagi nhưng koi shusui lại có da trơn. Shusui đẹp khi màu đỏ Hi kéo dài 2 bên hông từ bụng đến đuôi và vảy đầy đủ, tạo thành hàng dài thẳng hàng.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('ca0884736a6542a98d0206716c2d6846', 'Koi Taisho Sanke', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027825/products/sanke.jpg', 
	N'Cá Koi Taisho Sanke Được lai tạo từ Kohaku, có màu đen pha đỏ và trắng trên cơ thể. 1 em Koi taisho Sanke đẹp phải có màu trắng sạch, trắng rực rỡ như tuyết.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('5a84123588704afb83ca029c8eca218b', 'Koi Utsurimono', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027871/products/utsurimono.jpg', 
	N'Cá Koi Utsurimono (Hikariutsuri / Hikari Utsurimono) Dòng Koi da đen với các mảng màu trắng, đỏ hoặc vàng tựa mô hình hoa văn ngựa vằn. Hoa văn vàng trên nền đen phổ biến nhất, lâu đời nhất. Gồm 4 phiên bản Hi Utsuri (hai màu đen và đỏ); Ki Utsuri (hai màu đen và vàng); Kin Showa (3 màu đỏ đen, trắng bạc); Shiro Utsuri (hai màu đen và trắng).', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('2f7a5273a19240c58db1f9e0b157b208', 'Koi Chagoi', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027677/products/chagoi.jpg', 
	N'Chagoi là dòng Koi đơn sắc, không có ánh kim loại. Màu của nó có thể là màu trà cho Chagoi, màu lục nhạt cho Koi Midorigoi và xanh dương xám cho Soragoi. Koi Chagoi tính tình thân thiện, ngoan ngoãn, kích thước lớn nên tượng trưng cho sự may mắn.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('611ce6b1bca74c909b42609ee18f674d', 'Koi Karashi', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027725/products/karashi.jpg', 
	N'Cá Koi Karashi có thể là da trơn (Doitsu), vảy chiếu (Ginrin) và loại vảy thông thường. Độ biến đổi màu sắc của nó có thể bắt đầu từ màu be, đến vàng nhạt, và cuối cùng là vàng đậm', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('ca226aab2bc946ce839057c492574219', 'Koi Kawarimono', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028056/products/kawarimono.jpg', 
	N'Cá Koi Kawarimono là 1 nhóm phân loại các loại Koi lai tạo với các nhóm Koi khác gọi chung là Kawarimono. Chúng được chia thành 3 nhóm Koi đơn sắc (single-colored Koi), màu đen tạp (black Koi Breeds) và Koi kawarimono', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('1368ee1fa303461b87ddbb5b26d6d8d0', 'Koi Kujaku', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027762/products/kujaku.jpg', 
	N'Cá Koi Kujaku có màu nền bằng kim loại phủ bởi màu đỏ, cam hoặc màu vàng kim loại, tạo thành hiệu ứng đặc biệt,', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('2db74b614ecc44dd9b2c10429ff49cb3', 'Koi Matsuba', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027774/products/matsuba.jpg', 
	N'Matsuba là một loại cá Koi có màu kim loại với hoa văn lưới màu đen. Cơ thể của cá koi Matsuba có một màu với vảy hình nón thông. Loại cá này có lớp vảy óng ánh vô cùng đẹp mắt. Để tìm hiểu kỹ hơn về giống cá Koi Matsuba này hãy cùng tham khảo bài viết dưới đây của chúng tôi nhé', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('af42c9de1e8048c7a7d8b254dc71afea', 'Koi KikoKryu', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027737/products/kikokryu.jpg', 
	N'Cá Koi KikoKryu Kikokuryu thực chất là 1 kumonryu kim loại. Có làn da bạch kim sáng bóng, vây với sắc màu sumi đen sâu trong suốt. Được lai tạo từ Kim Ogon và kumonryu. Nhiệt độ, nước, di truyền là yếu tố quyết định mô hình màu sắc, đậm, nhạt của KikoKryu.', 
	SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),
    ('af301c6d526849e4bfde6e2ead5be943', 'Koi Ochiba', 1, 'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027785/products/ochiba.jpg', 
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
	('1', 'Koi Kohaku', 1000, 'Kohaku', 'Japan', 'Unknown', 2, 'Medium', 'Koi', 'Calm', 50, 20, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027750/products/kohaku.jpg', 1, 'Fish', 
	'41c2fbe4b02549c587837a3c4658e02a', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Ogon'
	('2', 'Koi Ogon', 1500, 'Ogon', 'Japan', 'Unknown', 3, 'Large', 'Koi', 'Active', 70, 18, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027798/products/ogon.jpg', 1, 'Fish', 
	'd93645edc83a4a0e8cfcbb9f2d819cc9', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Showa'
	('3', 'Koi Showa', 2000, 'Showa', 'Japan', 'Male', 1, 'Large', 'Koi', 'Aggressive', 80, 22, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027838/products/showa.jpg', 1, 'Fish', 
	'4f89cbd2af1e422fbfbfb1b78eb5d620', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Tancho'
	('4', 'Koi Tancho', 1200, 'Tancho', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Friendly', 60, 20, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027860/products/tancho.jpg', 1, 'Fish', 
	'3369c7cbba56453098a3d9487f22e4bc', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Bekko'
	('5', 'Koi Bekko', 1100, 'Bekko', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Aggressive', 60, 18, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028081/products/bekko.jpg', 1, 'Fish', 
	'64eb0df76e7b4c7ab2cdb7e94ddc8d8f', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Doitsu'
	('6', 'Koi Doitsu', 1300, 'Doitsu', 'Japan', 'Unknown', 1, 'Small', 'Koi', 'Calm', 50, 19, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027691/products/doitsu.jpg', 1, 'Fish', 
	'4ef098cfc4f94649bbb7c48b280cd31a', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Ginrin'
	('7', 'Koi Ginrin', 2500, 'Ginrin', 'Japan', 'Female', 4, 'Large', 'Koi', 'Friendly', 90, 22, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027702/products/ginrin.jpg', 1, 'Fish', 
	'4e55d312c48d4071af5b678b391646d5', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Goshiki'
	('8', 'Koi Goshiki', 1800, 'Goshiki', 'Japan', 'Male', 2, 'Medium', 'Koi', 'Calm', 65, 19, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027714/products/goshiki.jpg', 1, 'Fish', 
	'bc1b07abc67945cabcfb9dbb4cd26763', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	('9', 'Koi Benigoi', 1400, 'Benigoi', 'Japan', 'Unknown', 2, 'Medium', 'Koi', 'Friendly', 55, 19, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027660/products/benigoi.jpg', 1, 'Fish', 
	'01b364370e6b428581d3d2a344a197b2', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Asagi'
	('10', 'Koi Asagi', 1900, 'Asagi', 'Japan', 'Male', 3, 'Large', 'Koi', 'Calm', 65, 18, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028069/products/asagi.jpg', 1, 'Fish', 
	'5b160e9cee4c40a79e1d8a7cf80f3d0b', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Platium'
	('11', 'Koi Platium', 2200, 'Platinum', 'Japan', 'Female', 4, 'Large', 'Koi', 'Calm', 75, 20, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027810/products/platium.jpg', 1, 'Fish', 
	'e4bfaf2848c2429ab06c85405707a925', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Shusui'
	('12', 'Koi Shusui', 1600, 'Shusui', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Friendly', 60, 19, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027850/products/shusui.jpg', 1, 'Fish', 
	'e4048e4bbf8d480fadfc8c7d0e3e981d', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Taisho Sanke'
	('13', 'Koi Taisho Sanke', 2400, 'Sanke', 'Japan', 'Female', 2, 'Medium', 'Koi', 'Aggressive', 75, 21, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027825/products/sanke.jpg', 1, 'Fish', 
	'ca0884736a6542a98d0206716c2d6846', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Utsurimono'
	('14', 'Koi Utsurimono', 2500, 'Utsurimono', 'Japan', 'Male', 3, 'Large', 'Koi', 'Aggressive', 80, 22, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027871/products/utsurimono.jpg', 1, 'Fish', 
	'5a84123588704afb83ca029c8eca218b', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Chagoi'
	('15', 'Koi Chagoi', 1800, 'Chagoi', 'Japan', 'Female', 2, 'Large', 'Koi', 'Friendly', 65, 19, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027677/products/chagoi.jpg', 1, 'Fish', 
	'2f7a5273a19240c58db1f9e0b157b208', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Karashi'
	('16', 'Koi Karashi', 1700, 'Karashi', 'Japan', 'Male', 3, 'Medium', 'Koi', 'Calm', 60, 20, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027725/products/karashi.jpg', 1, 'Fish', 
	'611ce6b1bca74c909b42609ee18f674d', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Kawarimono'
	('17', 'Koi Kawarimono', 2100, 'Kawarimono', 'Japan', 'Female', 4, 'Large', 'Koi', 'Aggressive', 70, 21, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728028056/products/kawarimono.jpg', 1, 'Fish', 
	'ca226aab2bc946ce839057c492574219', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Kujaku'
	('18', 'Koi Kujaku', 1900, 'Kujaku', 'Japan', 'Unknown', 2, 'Medium', 'Koi', 'Calm', 65, 20, 'Medium', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027762/products/kujaku.jpg', 1, 'Fish', 
	'1368ee1fa303461b87ddbb5b26d6d8d0', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Matsuba'
	('19', 'Koi Matsuba', 1700, 'Matsuba', 'Japan', 'Female', 3, 'Large', 'Koi', 'Friendly', 70, 19, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027774/products/matsuba.jpg', 1, 'Fish', 
	'2db74b614ecc44dd9b2c10429ff49cb3', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi KikoKryu'
	('20', 'Koi KikoKryu', 2000, 'KikoKryu', 'Japan', 'Unknown', 3, 'Large', 'Koi', 'Aggressive', 80, 22, 'High', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027737/products/kikokryu.jpg', 1, 'Fish', 
	'af42c9de1e8048c7a7d8b254dc71afea', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL),

	-- ProductItem for 'Koi Ochiba'
	('21', 'Koi Ochiba', 1500, 'Ochiba', 'Japan', 'Male', 2, 'Medium', 'Koi', 'Friendly', 55, 19, 'Low', 7, 
	'https://res.cloudinary.com/dv7tuoxwb/image/upload/v1728027785/products/ochiba.jpg', 1, 'Fish', 
	'af301c6d526849e4bfde6e2ead5be943', SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), NULL, 0, NULL);

COMMIT;
