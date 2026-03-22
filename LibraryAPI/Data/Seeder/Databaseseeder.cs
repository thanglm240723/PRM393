namespace LibraryAPI.Data.Seeder
{
    using LibraryAPI.Data;
    using LibraryAPI.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(PersonalLibraryContext context, IConfiguration config)
        {
            await context.Database.MigrateAsync();

            await SeedUsersAsync(context, config);
            await SeedBooksAsync(context);
            await SeedBookContentsAsync(context);
            await SeedBadgesAsync(context);        // MỚI
            await SeedUserStatsAsync(context);     // MỚI
            await SeedSampleRatingsAsync(context); // MỚI

            Console.WriteLine("✅ Database seeded successfully.");
        }

      
        private static async Task SeedUsersAsync(
            PersonalLibraryContext context, IConfiguration config)
        {
            if (await context.Users.AnyAsync()) return;

            var adminCfg = config.GetSection("AdminAccount");

            var users = new List<User>
            {
                new()
                {
                    Username     = adminCfg["Username"] ?? "admin",
                    Email        = adminCfg["Email"]    ?? "admin@library.com",
                    FullName     = adminCfg["FullName"] ?? "Administrator",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminCfg["Password"] ?? "Admin@123"),
                    Role         = "admin",
                    CreatedAt    = DateTime.Now,
                    UpdatedAt    = DateTime.Now,
                },
                new()
                {
                    Username     = "user1",
                    Email        = "user1@example.com",
                    FullName     = "Nguyễn Văn An",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    Role         = "user",
                    CreatedAt    = DateTime.Now,
                    UpdatedAt    = DateTime.Now,
                },
                new()
                {
                    Username     = "user2",
                    Email        = "user2@example.com",
                    FullName     = "Trần Thị Bình",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    Role         = "user",
                    CreatedAt    = DateTime.Now,
                    UpdatedAt    = DateTime.Now,
                },
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();
            Console.WriteLine($"  → Seeded {users.Count} users.");
        }

        // ─────────────────────────────────────────────────────────────
        //  BOOKS (giữ nguyên)
        // ─────────────────────────────────────────────────────────────
        private static async Task SeedBooksAsync(PersonalLibraryContext context)
        {
            if (await context.Books.AnyAsync()) return;

            var books = new List<Book>
            {
                new()
                {
                    Title         = "Đắc Nhân Tâm",
                    Author        = "Dale Carnegie",
                    Description   = "Cuốn sách kinh điển về nghệ thuật giao tiếp và ứng xử với con người.",
                    Genre         = "Kỹ năng",
                    PageCount     = 320,
                    PublishedYear = 1936,
                    Rating        = 4.8m,
                    Language      = "Vietnamese",
                    CoverImageUrl = "https://cdn1.fahasa.com/media/catalog/product/9/7/9786043949247.jpg",
                    CreatedAt     = DateTime.Now,
                },
                new()
                {
                    Title         = "Nhà Giả Kim",
                    Author        = "Paulo Coelho",
                    Description   = "Câu chuyện về hành trình tìm kiếm kho báu của cậu bé chăn cừu Santiago.",
                    Genre         = "Tiểu thuyết",
                    PageCount     = 228,
                    PublishedYear = 1988,
                    Rating        = 4.7m,
                    Language      = "Vietnamese",
                    CoverImageUrl = "https://cdn1.fahasa.com/media/catalog/product/i/m/image_195509_1_36793_1.jpg",
                    CreatedAt     = DateTime.Now,
                },
                new()
                {
                    Title         = "Tư Duy Nhanh Và Chậm",
                    Author        = "Daniel Kahneman",
                    Description   = "Khám phá hai hệ thống tư duy của não người.",
                    Genre         = "Kỹ năng",
                    PageCount     = 512,
                    PublishedYear = 2011,
                    Rating        = 4.6m,
                    Language      = "Vietnamese",
                    CoverImageUrl = "https://cdn1.fahasa.com/media/flashmagazine/images/page_images/tu_duy_nhanh_va_cham_tai_ban_2021/2021_06_23_08_21_26_1-390x510.jpg",
                    CreatedAt     = DateTime.Now,
                },
                new()
                {
                    Title         = "Sapiens: Lược Sử Loài Người",
                    Author        = "Yuval Noah Harari",
                    Description   = "Một cái nhìn toàn diện về lịch sử nhân loại từ thời tiền sử đến hiện đại.",
                    Genre         = "Lịch sử",
                    PageCount     = 443,
                    PublishedYear = 2011,
                    Rating        = 4.9m,
                    Language      = "Vietnamese",
                    CoverImageUrl = "https://cdn1.fahasa.com/media/flashmagazine/images/page_images/sapiens_luoc_su_loai_nguoi/2023_03_21_16_35_44_1-390x510.jpg",
                    CreatedAt     = DateTime.Now,
                },
                new()
                {
                    Title         = "Dám Bị Ghét",
                    Author        = "Ichiro Kishimi & Fumitake Koga",
                    Description   = "Triết học Adler: hạnh phúc là lựa chọn, không phải số phận.",
                    Genre         = "Kỹ năng",
                    PageCount     = 288,
                    PublishedYear = 2013,
                    Rating        = 4.5m,
                    Language      = "Vietnamese",
                    CoverImageUrl = "https://cdn1.fahasa.com/media/catalog/product/8/9/8935235215283_1.jpg",
                    CreatedAt     = DateTime.Now,
                },
                new()
                {
                    Title         = "Bố Già",
                    Author        = "Mario Puzo",
                    Description   = "Tiểu thuyết về gia đình mafia Corleone hùng mạnh ở nước Mỹ.",
                    Genre         = "Tiểu thuyết",
                    PageCount     = 480,
                    PublishedYear = 1969,
                    Rating        = 4.8m,
                    Language      = "Vietnamese",
                    CoverImageUrl = "https://cdn1.fahasa.com/media/catalog/product/b/o/bo-gia.jpg",
                    CreatedAt     = DateTime.Now,
                },
            };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();
            Console.WriteLine($"  → Seeded {books.Count} books.");
        }

       
        private static async Task SeedBookContentsAsync(PersonalLibraryContext context)
        {
            if (await context.BookContents.AnyAsync()) return;

            var books = await context.Books.OrderBy(b => b.BookId).ToListAsync();
            if (!books.Any()) return;

            var contents = new List<BookContent>();

            // Sách 1: Đắc Nhân Tâm
            if (books.Count >= 1)
            {
                var b = books[0];
                contents.AddRange(new[]
                {
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 1,
                        ChapterTitle = "Nếu bạn muốn lấy mật đừng đá tổ ong",
                        WordCount = 1850, CreatedAt = DateTime.Now,
                        Content = """
Vào ngày 7 tháng 5 năm 1931, tên tội phạm khét tiếng nhất New York bị bắt. Đó là "Hai Súng" Crowley - kẻ không uống rượu, không hút thuốc. Crowley đã bắn một cảnh sát đến chết chỉ vì anh ta yêu cầu xem giấy phép lái xe của hắn.

Khi cảnh sát bao vây căn hộ của Crowley ở đại lộ West End, hắn viết một bức thư bắt đầu bằng: "Dưới chiếc áo mệt mỏi này là trái tim kiệt sức nhưng nhân hậu - trái tim không muốn hại ai."

Đây là điều quan trọng nhất trong toàn bộ cuốn sách này: Crowley không tự trách mình dù một chút nào. Đây có phải là thái độ bất thường của những tên tội phạm không? Nếu bạn nghĩ vậy thì hãy nghe chuyện này.

Al Capone từng nói: "Tôi đã dành những năm tháng tốt nhất của cuộc đời mình để mang lại niềm vui, sự giải trí cho mọi người, và tất cả những gì tôi nhận được là những lời lăng mạ và danh tiếng của một tên tội phạm."

Đây là bí quyết lớn trong việc đối xử với con người: Đừng chỉ trích, đừng lên án, đừng phàn nàn.
"""
                    },
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 2,
                        ChapterTitle = "Bí quyết lớn trong việc đối đãi với người khác",
                        WordCount = 1720, CreatedAt = DateTime.Now,
                        Content = """
Chỉ có một cách duy nhất để khiến bất kỳ ai làm bất cứ điều gì. Bạn có bao giờ dừng lại để suy nghĩ về điều đó không? Vâng, chỉ có một cách duy nhất. Đó là khiến người kia muốn làm điều đó.

Hãy nhớ rằng không có cách nào khác.

Tất nhiên, bạn có thể dùng súng ép người ta đưa đồng hồ cho bạn. Bạn có thể đe dọa nhân viên để họ hợp tác - ít nhất là khi bạn có mặt. Bạn có thể dùng roi hay đe dọa để khiến trẻ em làm theo ý mình. Nhưng những phương pháp thô bạo này chỉ gây ra những phản ứng không mong muốn.

Điều duy nhất thực sự tạo động lực cho hành động là mong muốn. Sigmund Freud nói rằng tất cả mọi hành động của bạn đều bắt nguồn từ hai động cơ: ham muốn tình dục và mong muốn trở nên vĩ đại.

Hãy cho người khác điều họ muốn và họ sẽ cho bạn điều bạn muốn.
"""
                    },
                });
            }

            // Sách 2: Nhà Giả Kim
            if (books.Count >= 2)
            {
                var b = books[1];
                contents.AddRange(new[]
                {
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 1,
                        ChapterTitle = "Phần một: Cậu bé chăn cừu",
                        WordCount = 1930, CreatedAt = DateTime.Now,
                        Content = """
Cậu bé tên là Santiago đến nơi trú ngụ qua đêm trong một tu viện hoang đã bị bỏ. Tu viện này không còn mái che từ lâu, và một cây sung lớn đã mọc lên ngay tại nơi đặt phòng thánh.

Cậu quyết định ở lại đây tối nay. Cậu đã gặp những con cừu đi vào qua cánh cửa hỏng, và cậu trải tấm chăn trên sàn và đặt đầu gối lên một cuốn sách mà cậu đã mang theo.

Cậu bé nhìn những ngôi sao qua mái tu viện hỏng và nghĩ rằng chuyến hành trình này sẽ kéo dài bao lâu. Cậu đã di chuyển qua cánh đồng Andalusia đã hai năm rồi và biết rõ tất cả những thị trấn nhỏ trong vùng.

Cậu đọc đến đoạn mà Narcissus qua đời khi cúi nhìn vẻ đẹp của mình trong hồ. Và khi cậu đọc câu chuyện đó, cậu cảm thấy nó đang thật sự xảy ra.

Có lẽ đó là lý do tại sao cậu thích sách. Chúng kể những chuyện kỳ diệu về con người đang sống cuộc đời của họ.
"""
                    },
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 2,
                        ChapterTitle = "Phần hai: Lời tiên tri",
                        WordCount = 1880, CreatedAt = DateTime.Now,
                        Content = """
Cậu bé đến gặp một bà lão đọc bói. Bà nhìn vào lòng bàn tay cậu và im lặng một lúc lâu.

"Tôi sẽ không lấy tiền ngay bây giờ," bà nói. "Tôi sẽ chỉ giải thích giấc mơ của cậu nếu cậu hứa sẽ chia cho tôi một phần kho báu khi cậu tìm thấy."

Cậu bé cười. Trái tim cậu đã bắt đầu tin vào những điều mà cậu chưa từng tin trước đây.

"Hãy đến Ai Cập," bà nói. "Hãy đến Kim Tự Tháp. Tôi không thể giải thích nhiều hơn. Nhưng nếu cậu đến đó, cậu sẽ tìm thấy kho báu khiến cậu trở nên giàu có."

Đây là thông điệp cốt lõi của cuốn sách: Khi bạn thực sự muốn điều gì đó, toàn bộ vũ trụ sẽ hợp sức giúp bạn đạt được điều đó.
"""
                    },
                });
            }

            // Sách 3: Tư duy nhanh và chậm
            if (books.Count >= 3)
            {
                var b = books[2];
                contents.AddRange(new[]
                {
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 1,
                        ChapterTitle = "Hai hệ thống tư duy",
                        WordCount = 1760, CreatedAt = DateTime.Now,
                        Content = """
Hãy nhìn vào hình ảnh dưới đây.

Bạn đã thấy khuôn mặt tức giận. Và bạn đã biết điều gì đó về tâm trạng của người phụ nữ này - cô ấy đang tức giận, và cô ấy có thể sẽ nói những điều không hay. Bạn cũng có thể đoán được giọng nói của cô ấy sẽ to và gay gắt.

Tất cả những điều này đến với bạn một cách tự nhiên và dễ dàng - đó là Hệ thống 1 đang hoạt động.

Bây giờ hãy thử bài toán này: 17 × 24.

Bạn biết đây là bài toán nhân và bạn có thể học để giải nó. Nhưng hầu hết mọi người không biết ngay đáp án. Để giải bài toán này, bạn phải tập trung và cố tình thực hiện một chuỗi các bước. Đó là Hệ thống 2.

Hệ thống 1 nhanh, tự động, cảm xúc. Hệ thống 2 chậm, có chủ đích, logic. Và sự tương tác giữa hai hệ thống này là chìa khóa để hiểu tại sao chúng ta đưa ra những quyết định như vậy.
"""
                    },
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 2,
                        ChapterTitle = "Sự chú ý và nỗ lực",
                        WordCount = 1640, CreatedAt = DateTime.Now,
                        Content = """
Nếu bạn muốn trải nghiệm sự mâu thuẫn trong não mình, hãy thử đi nhanh và tính nhẩm cùng một lúc.

Hầu hết mọi người nhận thấy rằng nhịp đi của họ chậm lại khi họ cố gắng giải một bài toán khó. Đây không phải là ngẫu nhiên. Suy nghĩ tốn năng lượng, và não bộ phân bổ nguồn lực nhận thức một cách cẩn thận.

Nghiên cứu của chúng tôi với Amos Tversky chỉ ra rằng con người thường xuyên phụ thuộc vào những lối tắt nhận thức - các heuristics - để đưa ra quyết định nhanh chóng.

Điều đáng lo ngại là những lối tắt này thường dẫn đến sai lầm có hệ thống, được gọi là thiên kiến nhận thức.

Ví dụ: khi được hỏi "Linda là ai?", hầu hết mọi người chọn đáp án có vẻ đại diện nhất cho mô tả của Linda, mặc dù theo xác suất, đáp án đó ít khả năng đúng hơn.
"""
                    },
                });
            }

            // Sách 4: Sapiens
            if (books.Count >= 4)
            {
                var b = books[3];
                contents.AddRange(new[]
                {
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 1,
                        ChapterTitle = "Phần I: Cuộc cách mạng nhận thức",
                        WordCount = 2100, CreatedAt = DateTime.Now,
                        Content = """
Khoảng 13,5 tỷ năm trước, vật chất, năng lượng, thời gian và không gian xuất hiện trong sự kiện được gọi là Big Bang. Câu chuyện của những đặc tính cơ bản này được gọi là vật lý.

Khoảng 300.000 năm sau Big Bang, vật chất và năng lượng bắt đầu kết hợp thành các cấu trúc phức tạp gọi là nguyên tử, sau đó tập hợp thành phân tử. Câu chuyện của các phân tử được gọi là hóa học.

Khoảng 3,8 tỷ năm trước trên hành tinh Trái đất, một số phân tử kết hợp thành những cấu trúc đặc biệt lớn hơn và phức tạp hơn gọi là sinh vật sống. Câu chuyện của các sinh vật sống được gọi là sinh học.

Khoảng 70.000 năm trước, những sinh vật thuộc loài Homo sapiens bắt đầu hình thành nên những cấu trúc còn tinh tế hơn nữa gọi là văn hóa. Sự phát triển liên tiếp của các nền văn hóa này được gọi là lịch sử.
"""
                    },
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 2,
                        ChapterTitle = "Phần II: Cây tri thức",
                        WordCount = 1950, CreatedAt = DateTime.Now,
                        Content = """
Homo sapiens đã cai trị thế giới nhờ một khả năng độc đáo: khả năng hợp tác linh hoạt với số lượng lớn người lạ.

Kiến và ong cũng có thể hợp tác với số lượng lớn, nhưng chúng chỉ làm theo những cách rất cứng nhắc và chỉ với những người thân của mình.

Con người có thể hợp tác cực kỳ linh hoạt với vô số người lạ. Đây là điều đã dẫn đến sự thành công phi thường của chúng ta.

Làm thế nào Homo sapiens đã đạt được khả năng này? Câu trả lời nằm ở ngôn ngữ. Nhưng không phải ngôn ngữ bình thường - mà là khả năng tạo ra và chia sẻ những câu chuyện hư cấu.

Những huyền thoại chung, những câu chuyện chung, những niềm tin chung - đây là thứ liên kết hàng triệu người lạ với nhau thành những xã hội hiệu quả.
"""
                    },
                });
            }

            // Sách 5: Dám Bị Ghét
            if (books.Count >= 5)
            {
                var b = books[4];
                contents.AddRange(new[]
                {
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 1,
                        ChapterTitle = "Đêm thứ nhất: Phủ nhận chấn thương tâm lý",
                        WordCount = 1680, CreatedAt = DateTime.Now,
                        Content = """
Chàng trai: Thưa tiên sinh, tôi muốn hỏi về triết học của Adler.

Triết gia: Ồ, thú vị đấy. Cậu đã đọc gì về Adler rồi?

Chàng trai: Tôi đã đọc một vài cuốn sách. Nhưng tôi không đồng ý với ông ta. Adler nói rằng con người có thể thay đổi. Nhưng tôi không tin điều đó.

Triết gia: Tại sao cậu lại tin như vậy?

Chàng trai: Vì tôi đã thấy điều đó với chính bản thân mình. Tôi luôn muốn thay đổi, nhưng tôi không thể.

Triết gia: Adler nói rằng chúng ta không bị quá khứ quyết định. Chúng ta quyết định tương lai của mình từ hiện tại.

Chàng trai: Nhưng chấn thương tâm lý thì sao?

Triết gia: Adler phủ nhận chấn thương tâm lý. Ông nói rằng chúng ta không bị kiểm soát bởi những trải nghiệm quá khứ. Thay vào đó, chúng ta sử dụng những trải nghiệm đó như một cái cớ để đạt được mục tiêu của mình trong hiện tại.
"""
                    },
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 2,
                        ChapterTitle = "Đêm thứ hai: Tất cả phiền não đều từ các mối quan hệ mà ra",
                        WordCount = 1540, CreatedAt = DateTime.Now,
                        Content = """
Chàng trai: Hôm qua tiên sinh nói rằng mọi phiền não đều xuất phát từ các mối quan hệ giữa người với người. Nhưng tôi không hiểu điều đó.

Triết gia: Hãy nghĩ về điều này: Nếu cậu sống một mình trên một hòn đảo hoang, cậu có lo lắng về ngoại hình của mình không?

Chàng trai: Không, chắc là không.

Triết gia: Đó là điểm mấu chốt. Mặc cảm, tự ti, lo lắng về bản thân - tất cả những điều này chỉ xuất hiện khi có mặt người khác.

Chàng trai: Nhưng làm sao chúng ta có thể tránh được các mối quan hệ đó?

Triết gia: Adler không nói chúng ta nên tránh né. Hạnh phúc thực sự không đến từ việc được người khác chấp thuận. Nó đến từ việc chấp nhận chính mình và đóng góp cho cộng đồng.
"""
                    },
                });
            }

            // Sách 6: Bố Già
            if (books.Count >= 6)
            {
                var b = books[5];
                contents.AddRange(new[]
                {
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 1,
                        ChapterTitle = "Chương I",
                        WordCount = 2050, CreatedAt = DateTime.Now,
                        Content = """
Amerigo Bonasera ngồi trong phòng xử án số 3 của tòa án New York và chờ đợi công lý.

Ông ngồi đó từ sáng sớm, mặc bộ đồ đen trang trọng. Ông là một con người nghiêm túc, không bao giờ cảm thấy cần ai giúp đỡ trong suốt cuộc đời. Ông tin vào nước Mỹ.

Nhưng hôm nay ông cần công lý.

Hai thanh niên ngồi ở bàn bị cáo, trẻ, đẹp trai, mặc bộ vest đắt tiền. Chúng đã làm những điều khủng khiếp với con gái ông. Nhưng khi ra tòa, chúng chỉ bị phạt tù treo.

Sau phiên tòa, Amerigo Bonasera đứng bên ngoài, tim đau như bóp nghẹt. Ông biết mình phải làm gì. Ông phải đến gặp Don Corleone.

Don Vito Corleone ngồi sau chiếc bàn lớn trong văn phòng của ông, lắng nghe thỉnh cầu của Bonasera.

"Bạn bè của tôi," ông nói chậm rãi, "tôi không quen làm những việc như thế này."
"""
                    },
                    new BookContent
                    {
                        BookId = b.BookId, ChapterNumber = 2,
                        ChapterTitle = "Chương II",
                        WordCount = 1890, CreatedAt = DateTime.Now,
                        Content = """
Đám cưới của Connie Corleone diễn ra vào một ngày cuối tháng Tám, nắng vàng rực rỡ.

Theo phong tục của người Sicily, không ai được phép từ chối lời mời của Don Corleone vào ngày cưới của con gái ông.

Trong khi tiệc cưới đang diễn ra ngoài sân, Don Corleone ngồi trong văn phòng tối của ông và giải quyết công việc.

"Bao nhiêu năm qua, tôi không bao giờ đến nhờ vả Don Corleone," Bonasera nói, giọng run rẩy. "Nhưng bây giờ tôi cần sự giúp đỡ."

Don Corleone lắng nghe câu chuyện về con gái của Bonasera. Khuôn mặt ông không lộ vẻ cảm xúc gì.

"Được rồi. Vì cậu là người bạn cũ của tôi, tôi sẽ giúp cậu. Nhưng nhớ rằng, một ngày nào đó - tôi sẽ cần cậu làm việc gì đó cho tôi."
"""
                    },
                });
            }

            context.BookContents.AddRange(contents);
            await context.SaveChangesAsync();
            Console.WriteLine($"  → Seeded {contents.Count} book contents.");
        }

        // ─────────────────────────────────────────────────────────────
        //  BADGES — Danh mục huy hiệu
        // ─────────────────────────────────────────────────────────────
        private static async Task SeedBadgesAsync(PersonalLibraryContext context)
        {
            if (await context.Badges.AnyAsync()) return;

            var badges = new List<Badge>
            {
                // ── Cấp bậc theo số sách đã đọc ──────────────────────
                new() { Name = "Mầm Đọc",       Icon = "🌱", ConditionType = "books_read", Threshold = 1,    DisplayOrder = 1,
                        Description = "Đọc hoàn thành cuốn sách đầu tiên (≥70%)" },
                new() { Name = "Độc Giả Mới",   Icon = "📖", ConditionType = "books_read", Threshold = 5,    DisplayOrder = 2,
                        Description = "Đọc hoàn thành 5 cuốn sách" },
                new() { Name = "Mọt Sách",      Icon = "📚", ConditionType = "books_read", Threshold = 10,   DisplayOrder = 3,
                        Description = "Đọc hoàn thành 10 cuốn sách" },
                new() { Name = "Học Giả",       Icon = "🎓", ConditionType = "books_read", Threshold = 50,   DisplayOrder = 4,
                        Description = "Đọc hoàn thành 50 cuốn sách" },
                new() { Name = "Bậc Thầy",      Icon = "👑", ConditionType = "books_read", Threshold = 100,  DisplayOrder = 5,
                        Description = "Đọc hoàn thành 100 cuốn sách" },
                new() { Name = "Huyền Thoại",   Icon = "🏆", ConditionType = "books_read", Threshold = 500,  DisplayOrder = 6,
                        Description = "Đọc hoàn thành 500 cuốn sách" },

                // ── Streak ────────────────────────────────────────────
                new() { Name = "Khởi Đầu",      Icon = "🔥", ConditionType = "streak", Threshold = 3,  DisplayOrder = 10,
                        Description = "Đọc 3 ngày liên tiếp" },
                new() { Name = "Không Ngừng",   Icon = "💪", ConditionType = "streak", Threshold = 7,  DisplayOrder = 11,
                        Description = "Đọc 7 ngày liên tiếp" },
                new() { Name = "Kiên Định",     Icon = "⚡", ConditionType = "streak", Threshold = 30, DisplayOrder = 12,
                        Description = "Đọc 30 ngày liên tiếp" },
                new() { Name = "Bất Khuất",     Icon = "🌟", ConditionType = "streak", Threshold = 100,DisplayOrder = 13,
                        Description = "Đọc 100 ngày liên tiếp" },

                // ── Số trang đã đọc ───────────────────────────────────
                new() { Name = "Bước Đầu",      Icon = "👣", ConditionType = "pages_read", Threshold = 100,  DisplayOrder = 20,
                        Description = "Đọc tổng cộng 100 trang" },
                new() { Name = "Hành Trình",    Icon = "🗺️", ConditionType = "pages_read", Threshold = 1000, DisplayOrder = 21,
                        Description = "Đọc tổng cộng 1.000 trang" },
                new() { Name = "Vạn Trang",     Icon = "📜", ConditionType = "pages_read", Threshold = 10000,DisplayOrder = 22,
                        Description = "Đọc tổng cộng 10.000 trang" },

                // ── Số giờ đọc ────────────────────────────────────────
                new() { Name = "Cú Đêm",        Icon = "🌙", ConditionType = "night_read", Threshold = 5,  DisplayOrder = 30,
                        Description = "Đọc sau 11 giờ đêm 5 lần" },
                new() { Name = "Chim Sớm",      Icon = "🌅", ConditionType = "morning_read",Threshold = 5,  DisplayOrder = 31,
                        Description = "Đọc trước 7 giờ sáng 5 lần" },
                new() { Name = "Tốc Độc",       Icon = "💨", ConditionType = "speed_read",  Threshold = 1,  DisplayOrder = 32,
                        Description = "Đọc xong 1 cuốn sách trong 1 ngày" },

                // ── Thể loại ──────────────────────────────────────────
                new() { Name = "Chuyên Gia KN", Icon = "💡", ConditionType = "genre_master", Threshold = 5, DisplayOrder = 40,
                        Description = "Đọc 5 cuốn sách Kỹ năng" },
                new() { Name = "Nhà Văn Học",   Icon = "✍️", ConditionType = "genre_master", Threshold = 5, DisplayOrder = 41,
                        Description = "Đọc 5 cuốn Tiểu thuyết" },
                new() { Name = "Sử Gia",        Icon = "🏛️", ConditionType = "genre_master", Threshold = 5, DisplayOrder = 42,
                        Description = "Đọc 5 cuốn Lịch sử" },
            };

            context.Badges.AddRange(badges);
            await context.SaveChangesAsync();
            Console.WriteLine($"  → Seeded {badges.Count} badges.");
        }

        // ─────────────────────────────────────────────────────────────
        //  USER STATS — Khởi tạo stats mặc định cho mỗi user
        // ─────────────────────────────────────────────────────────────
        private static async Task SeedUserStatsAsync(PersonalLibraryContext context)
        {
            if (await context.UserStats.AnyAsync()) return;

            var users = await context.Users.ToListAsync();
            if (!users.Any()) return;

            var stats = users.Select(u => new UserStats
            {
                UserId = u.UserId,
                TotalBooksRead = 0,
                TotalBooksStarted = 0,
                TotalPagesRead = 0,
                TotalMinutesRead = 0,
                TotalWordsRead = 0,
                CurrentStreak = 0,
                LongestStreak = 0,
                LastReadDate = null,
                FavoriteGenre = null,
                Rank = "Mầm Đọc",
                UpdatedAt = DateTime.Now,
            }).ToList();

            context.UserStats.AddRange(stats);
            await context.SaveChangesAsync();
            Console.WriteLine($"  → Seeded {stats.Count} user stats.");
        }

        // ─────────────────────────────────────────────────────────────
        //  SAMPLE RATINGS — Một vài đánh giá mẫu
        // ─────────────────────────────────────────────────────────────
        private static async Task SeedSampleRatingsAsync(PersonalLibraryContext context)
        {
            if (await context.BookRatings.AnyAsync()) return;

            var users = await context.Users.Where(u => u.Role == "user").ToListAsync();
            var books = await context.Books.ToListAsync();

            if (!users.Any() || !books.Any()) return;

            var ratings = new List<BookRating>();

            // user1 rate 3 cuốn
            if (users.Count >= 1 && books.Count >= 1)
            {
                ratings.Add(new BookRating
                {
                    UserId = users[0].UserId,
                    BookId = books[0].BookId,
                    Stars = 5,
                    IsVerifiedReader = true,
                    CreatedAt = DateTime.Now,
                    Review = "Cuốn sách thay đổi cách tôi giao tiếp với mọi người. Cực kỳ hữu ích!"
                });
            }
            if (users.Count >= 1 && books.Count >= 2)
            {
                ratings.Add(new BookRating
                {
                    UserId = users[0].UserId,
                    BookId = books[1].BookId,
                    Stars = 4,
                    IsVerifiedReader = true,
                    CreatedAt = DateTime.Now,
                    Review = "Câu chuyện đẹp, đầy cảm hứng. Đọc xong muốn đi theo đuổi ước mơ ngay."
                });
            }

            // user2 rate 2 cuốn
            if (users.Count >= 2 && books.Count >= 1)
            {
                ratings.Add(new BookRating
                {
                    UserId = users[1].UserId,
                    BookId = books[0].BookId,
                    Stars = 5,
                    IsVerifiedReader = false,
                    CreatedAt = DateTime.Now,
                    Review = "Kinh điển muôn thuở, ai cũng nên đọc ít nhất một lần."
                });
            }
            if (users.Count >= 2 && books.Count >= 3)
            {
                ratings.Add(new BookRating
                {
                    UserId = users[1].UserId,
                    BookId = books[3].BookId,
                    Stars = 5,
                    IsVerifiedReader = true,
                    CreatedAt = DateTime.Now,
                    Review = "Harari viết quá hay! Đọc xong nhìn lịch sử nhân loại theo cách hoàn toàn khác."
                });
            }

            if (!ratings.Any()) return;

            context.BookRatings.AddRange(ratings);
            await context.SaveChangesAsync();

            // Cập nhật Rating trung bình cho từng sách
            await UpdateBookAverageRatings(context);

            Console.WriteLine($"  → Seeded {ratings.Count} book ratings.");
        }

        // ─────────────────────────────────────────────────────────────
        //  HELPER: Cập nhật Rating trung bình của sách
        // ─────────────────────────────────────────────────────────────
        private static async Task UpdateBookAverageRatings(PersonalLibraryContext context)
        {
            var books = await context.Books
                .Include(b => b.BookRatings)  // Cần add navigation property vào Book
                .ToListAsync();

            // Bỏ qua nếu chưa có navigation property BookRatings trong Book
            // (thêm sau khi update Book.cs)
        }
    }
}