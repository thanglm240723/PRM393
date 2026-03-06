    namespace LibraryAPI.Data.Seeder
    {
    using global::LibraryAPI.Data.Models;
    using LibraryAPI.Data;
    using LibraryAPI.Data.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Gọi trong Program.cs:
    
    /// </summary>
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(PersonalLibraryContext context, IConfiguration config)
        {
            // Đảm bảo DB đã được tạo và migration đã chạy
            await context.Database.MigrateAsync();

            await SeedUsersAsync(context, config);
            await SeedBooksAsync(context);
            await SeedBookContentsAsync(context);

            Console.WriteLine("✅ Database seeded successfully.");
        }

        // ─────────────────────────────────────────────────────────────────
        //  USERS
        // ─────────────────────────────────────────────────────────────────
        private static async Task SeedUsersAsync(
            PersonalLibraryContext context, IConfiguration config)
        {
            if (await context.Users.AnyAsync()) return;

            // Admin từ appsettings.json
            var adminCfg = config.GetSection("AdminAccount");

            var users = new List<User>
        {
            new()
            {
                Username     = adminCfg["Username"] ?? "admin",
                Email        = adminCfg["Email"]    ?? "admin@library.com",
                FullName     = adminCfg["FullName"] ?? "Administrator",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                                   adminCfg["Password"] ?? "Admin@123"),
                Role      = "user",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
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

        // ─────────────────────────────────────────────────────────────────
        //  BOOKS
        // ─────────────────────────────────────────────────────────────────
        private static async Task SeedBooksAsync(PersonalLibraryContext context)
        {
            if (await context.Books.AnyAsync()) return;

            var books = new List<Book>
        {
            new()
            {
                Title         = "Đắc Nhân Tâm",
                Author        = "Dale Carnegie",
                Description   = "Cuốn sách kinh điển về nghệ thuật giao tiếp và ứng xử với con người. " +
                                "Được xuất bản lần đầu năm 1936, đây là một trong những cuốn sách bán chạy nhất mọi thời đại " +
                                "với hơn 30 triệu bản được bán ra trên toàn thế giới.",
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
                Description   = "Câu chuyện về hành trình tìm kiếm kho báu của cậu bé chăn cừu Santiago. " +
                                "Đây là một trong những cuốn tiểu thuyết bán chạy nhất thế giới, " +
                                "đã được dịch ra hơn 80 ngôn ngữ.",
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
                Description   = "Khám phá hai hệ thống tư duy của não người: tư duy nhanh (bản năng) và tư duy chậm (lý trí). " +
                                "Cuốn sách giúp bạn hiểu tại sao chúng ta thường đưa ra những quyết định sai lầm.",
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
                Description   = "Một cái nhìn toàn diện về lịch sử nhân loại từ thời tiền sử đến hiện đại. " +
                                "Tác phẩm đặt câu hỏi về nguồn gốc, sự phát triển và tương lai của loài Homo sapiens.",
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
                Description   = "Triết học Adler được trình bày dưới dạng đối thoại giữa triết gia và chàng trai trẻ. " +
                                "Cuốn sách chỉ ra rằng hạnh phúc là lựa chọn, không phải số phận.",
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
                Description   = "Tiểu thuyết về gia đình mafia Corleone hùng mạnh ở nước Mỹ. " +
                                "Một tác phẩm kinh điển về quyền lực, lòng trung thành, sự phản bội và tình gia đình.",
                Genre         = "Tiểu thuyết",
                PageCount     = 448,
                PublishedYear = 1969,
                Rating        = 4.8m,
                Language      = "Vietnamese",
                CoverImageUrl = "https://cdn1.fahasa.com/media/catalog/product/0/0/00_2.jpg",
                CreatedAt     = DateTime.Now,
            },
        };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();
            Console.WriteLine($"  → Seeded {books.Count} books.");
        }

        // ─────────────────────────────────────────────────────────────────
        //  BOOK CONTENTS (chapters)
        // ─────────────────────────────────────────────────────────────────
        private static async Task SeedBookContentsAsync(PersonalLibraryContext context)
        {
            if (await context.BookContents.AnyAsync()) return;

            // Lấy BookId theo thứ tự insert
            var books = await context.Books.OrderBy(b => b.BookId).ToListAsync();
            if (!books.Any()) return;

            var contents = new List<BookContent>();

            // ── Sách 1: Đắc Nhân Tâm ──────────────────────────────────────
            if (books.Count >= 1)
            {
                var b = books[0];
                contents.AddRange(new[]
                {
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 1,
                    ChapterTitle  = "Nếu bạn muốn lấy mật, đừng đá tổ ong",
                    WordCount     = 1850,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Ngày 7 tháng 5 năm 1931, tên tội phạm nguy hiểm nhất New York, Two Gun Crowley, đã cố thủ trong một căn hộ tại Tây 90th Street, bị hàng trăm cảnh sát vây hãm.

Crowley là kẻ sát nhân máu lạnh. Khi một cảnh sát dừng xe hỏi giấy tờ, hắn ta đã bắn viên cảnh sát ấy mà không hề do dự. Chính quyền gọi hắn là "tên giết người nguy hiểm nhất từng tung hoành trên đường phố New York".

Trong lúc chiến sự đang xảy ra, Crowley ngồi viết một lá thư. Máu từ vết thương đang nhỏ xuống tờ giấy, nhưng hắn vẫn tiếp tục viết: "Trong lồng ngực tôi là trái tim mệt mỏi, nhưng tử tế, một trái tim sẽ không làm hại ai."

Đây chính là điều kỳ lạ nhất: Crowley không hề tự trách mình về bất cứ điều gì!

Điều này có bình thường không? Hầu như tất cả tội phạm mà tôi từng gặp đều có cùng một kiểu suy nghĩ. Al Capone, tên trùm tội phạm khét tiếng nhất nước Mỹ, từng nói rằng ông ta không hề là tên côn đồ. Ông ta tự coi mình là người có ích cho xã hội.

Bài học đầu tiên trong nghệ thuật đối nhân xử thế: Đừng bao giờ chỉ trích, lên án hay phàn nàn về người khác.

Khi chỉ trích người khác, bạn làm tổn thương lòng tự trọng của họ và khiến họ trở thành kẻ thù của bạn. Thay vào đó, hãy hiểu tại sao họ làm điều đó. Điều này sẽ tạo ra sự đồng cảm, lòng bao dung và lòng tốt.
"""
                },
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 2,
                    ChapterTitle  = "Bí quyết vĩ đại trong việc đối xử với mọi người",
                    WordCount     = 1620,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Chỉ có một cách duy nhất để khiến bất kỳ ai làm bất cứ điều gì. Bạn có biết điều đó là gì không?

Đó là làm cho người kia muốn làm điều đó.

Không có cách nào khác.

Tất nhiên, bạn có thể chỉ cho người đó làm điều gì đó bằng cách chĩa súng vào họ. Bạn có thể dùng roi hoặc đe dọa để ép buộc. Nhưng những phương pháp thô bạo này chỉ gây ra hậu quả không mong muốn.

Sigmund Freud nói rằng mọi việc bạn làm đều xuất phát từ hai động lực cơ bản: ham muốn tình dục và mong muốn được vĩ đại.

John Dewey, một trong những nhà triết học vĩ đại nhất của Mỹ, đã diễn đạt điều này theo cách khác. Ông nói rằng động lực sâu xa nhất trong bản chất con người là "mong muốn được quan trọng."

Hãy nhớ điều này và bạn sẽ có chìa khóa ma thuật để hiểu con người. Hãy nhớ điều này và những rắc rối khi giao tiếp với mọi người sẽ biến mất.

Bí quyết vĩ đại trong việc đối xử với mọi người: Hãy làm cho người kia cảm thấy họ quan trọng và hãy làm điều đó một cách chân thành.
"""
                },
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 3,
                    ChapterTitle  = "Ai làm được điều này sẽ có cả thế giới về phía mình",
                    WordCount     = 1430,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Mùa hè năm đó tôi câu cá ở Maine. Cá hồi thì thích ăn giun đất, còn tôi thì thích ăn dâu tây với kem. Nhưng khi đi câu, tôi không nghĩ đến sở thích của mình. Tôi mắc giun đất vào lưỡi câu và thả xuống nước vì tôi biết cá thích ăn gì.

Tại sao không áp dụng lẽ thường tình này trong cuộc sống?

Nếu bạn muốn ai đó làm gì đó, trước tiên hãy đặt câu hỏi: Làm sao để người này muốn làm điều đó?

Đây là nguyên tắc duy nhất để ảnh hưởng đến người khác: Nói về những gì người kia muốn và chỉ cho họ cách đạt được điều đó.

Henry Ford từng nói: "Bí mật thành công, nếu có, là khả năng hiểu quan điểm của người khác và nhìn mọi việc từ góc nhìn của họ cũng như từ góc nhìn của chính mình."

Hãy tự hỏi mình mỗi ngày: Tôi có thể làm gì để người này muốn làm điều tôi mong muốn?
"""
                },
            });
            }

            // ── Sách 2: Nhà Giả Kim ───────────────────────────────────────
            if (books.Count >= 2)
            {
                var b = books[1];
                contents.AddRange(new[]
                {
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 1,
                    ChapterTitle  = "Phần Một: Cậu bé chăn cừu",
                    WordCount     = 2100,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Cậu bé tên Santiago quyết định ngủ lại đêm đó trong nhà thờ hoang phế. Cậu lùa những con cừu vào qua cổng nhà thờ đổ nát, rồi trải tấm áo khoác ra nằm xuống nền đất lạnh.

Cậu đã kể cho con cừu về những giấc mơ của mình.

"Chúng ta sống những năm tháng như thế này rồi," cậu nói, nhìn vào đôi mắt của con cừu đầu đàn. "Ta đi tìm thức ăn và nước uống. Khi ta mệt mỏi, ta nằm xuống ngủ. Ngày hôm sau lại như vậy, mãi cho đến khi chúng ta chết đi. Mà giữa ngày ta ăn và ngày ta chết, cũng không có gì thay đổi nhiều."

Ngôi nhà thờ không có mái, nhưng bức tường vẫn còn nguyên. Santiago thường ngủ ở đây. Và đêm nay cậu lại nằm mơ thấy một giấc mơ y hệt như tuần trước.

Trong giấc mơ, một đứa trẻ kéo tay cậu và dẫn cậu đến Kim Tự Tháp Ai Cập. Khi đến nơi, đứa trẻ nói: "Nếu cậu đến đây, cậu sẽ tìm thấy kho báu ẩn giấu."

Đó là lúc cậu thức dậy mỗi lần, ngay trước khi cậu được chỉ chỗ kho báu.

Santiago tự hỏi liệu đây có phải là điềm báo không, hay chỉ là giấc mơ vô nghĩa của một người chăn cừu cô đơn?
"""
                },
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 2,
                    ChapterTitle  = "Phần Hai: Cuộc gặp gỡ với Người già",
                    WordCount     = 1980,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Hôm sau, Santiago đến gặp một bà già được tiếng là có thể giải mộng.

Bà dẫn cậu vào một căn phòng nhỏ đầy tượng thần thánh và những chiếc bàn phủ khăn màu. Bà lắng nghe chăm chú khi cậu kể giấc mơ, rồi im lặng hồi lâu.

"Giấc mơ đó khó giải lắm," bà cuối cùng nói. "Nhưng nếu cậu thực sự muốn biết, tôi sẽ nói cho cậu nghe, với điều kiện cậu phải chia cho tôi một phần mười kho báu cậu tìm được."

Santiago không tin bà, nhưng vẫn đồng ý.

Bà nói rằng cậu phải đến Kim Tự Tháp ở Ai Cập. Đó là tất cả những gì bà nói.

Thất vọng, Santiago rời đi và ngồi xuống một chiếc ghế dài ở quảng trường. Đó là lúc một ông già ngồi xuống bên cạnh cậu.

"Cậu đang buồn vì một điều gì đó," ông nói.

"Tôi gặp một người phụ nữ giải mộng," Santiago trả lời, "và bà ta bảo tôi phải đi đến Ai Cập để tìm kho báu."

"Thì đi đi," ông già nói.

"Cả cuộc đời tôi đã học cách chăn cừu. Tôi biết cách chăm sóc chúng, tôi biết đồng cỏ nào tốt nhất ở vùng Andalusia. Còn Kim Tự Tháp thì ở rất xa."

Ông già nhìn cậu và nói: "Ta là vua Melchizedek. Và ta muốn nói với cậu một điều: Khi cậu thực sự muốn điều gì đó, cả vũ trụ sẽ âm mưu giúp cậu đạt được nó."
"""
                },
            });
            }

            // ── Sách 3: Tư Duy Nhanh Và Chậm ────────────────────────────
            if (books.Count >= 3)
            {
                var b = books[2];
                contents.AddRange(new[]
                {
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 1,
                    ChapterTitle  = "Chương 1: Các nhân vật của câu chuyện",
                    WordCount     = 1750,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Để mô tả tâm lý học của sự phán đoán và lựa chọn trực quan, tôi sẽ dùng đến một kỹ thuật mà tôi không tự phát minh ra: hãy nói về bộ não như thể nó bao gồm hai nhân vật, tôi gọi là Hệ thống 1 và Hệ thống 2.

Hệ thống 1 hoạt động tự động và nhanh chóng, với ít hoặc không có sự cố gắng nào và không có cảm giác kiểm soát tự nguyện.

Hệ thống 2 phân bổ sự chú ý đến các hoạt động tinh thần đòi hỏi nỗ lực, bao gồm các tính toán phức tạp.

Hãy xem xét câu đố sau: Gậy và bóng cộng lại giá 1,10 đô. Gậy đắt hơn bóng 1 đô. Hỏi bóng giá bao nhiêu?

Một con số nhảy vọt vào đầu bạn: 10 xu. Nhưng câu trả lời đúng là 5 xu. Nếu bóng giá 10 xu, tổng cộng sẽ là 1,20 đô, không phải 1,10 đô.

Đây là điển hình của Hệ thống 1 đang hoạt động: nó đưa ra câu trả lời cảm giác đúng mà không cần kiểm tra kỹ. Hệ thống 2 mới là hệ thống sẽ nhận ra lỗi sai nếu bạn dừng lại để tính toán.

Mục tiêu của cuốn sách này là cải thiện khả năng nhận biết các tình huống mà sai lầm có thể xảy ra và giúp bạn cố gắng hơn để tránh những sai lầm đó.
"""
                },
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 2,
                    ChapterTitle  = "Chương 2: Chú ý và nỗ lực",
                    WordCount     = 1620,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Nhân vật của Hệ thống 2 trong câu chuyện của tôi là người nỗ lực, chậm chạp và có khả năng tính toán. Bạn sẽ có thể nhận ra Hệ thống 2 bằng cách nghĩ đến bài tập này:

Hãy bắt đầu đi bộ với tốc độ bình thường của bạn, đồng thời cộng 13 cộng 17 trong đầu.

Hầu hết mọi người sẽ dừng lại hoặc đi chậm lại khi làm phép tính này. Đây là vì cả đi bộ và tính toán đều cần đến sự chú ý, và khi sự chú ý bị chia sẻ, cả hai đều kém hiệu quả hơn.

Học sinh trung học đã làm bài kiểm tra nhân 2 chữ số với nhau. Đây là nhiệm vụ đòi hỏi Hệ thống 2. Trong khi làm bài, đồng tử của họ giãn ra đáng kể, thể hiện sự tăng cường tập trung.

Hệ thống 2 có khả năng thay đổi cách hoạt động của Hệ thống 1, theo một nghĩa nào đó. Ví dụ, nếu bạn được yêu cầu so sánh cẩn thận hai hình dạng, Hệ thống 2 sẽ kiểm soát phản ứng vội vàng của Hệ thống 1 và đảm bảo rằng bạn nhìn cẩn thận hơn.
"""
                },
            });
            }

            // ── Sách 4: Sapiens ───────────────────────────────────────────
            if (books.Count >= 4)
            {
                var b = books[3];
                contents.AddRange(new[]
                {
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 1,
                    ChapterTitle  = "Phần I: Cuộc cách mạng nhận thức",
                    WordCount     = 2200,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Khoảng 13,5 tỷ năm trước, vật chất, năng lượng, thời gian và không gian đã được hình thành từ những gì chúng ta gọi là Big Bang. Câu chuyện về những đặc tính cơ bản này được gọi là vật lý học.

Khoảng 300.000 năm sau Big Bang, vật chất và năng lượng bắt đầu hình thành nên những cấu trúc phức tạp hơn được gọi là nguyên tử. Chúng kết hợp lại tạo thành các phân tử. Câu chuyện về nguyên tử, phân tử và các tương tác giữa chúng được gọi là hóa học.

Khoảng 3,8 tỷ năm trước, trên hành tinh chúng ta, một số phân tử kết hợp lại tạo thành các cấu trúc đặc biệt lớn và phức tạp hơn gọi là sinh vật. Câu chuyện về các sinh vật được gọi là sinh học học.

Khoảng 70.000 năm trước, những sinh vật thuộc loài Homo sapiens bắt đầu hình thành nên những cấu trúc còn tinh tế hơn nữa gọi là văn hóa. Sự phát triển liên tiếp của các nền văn hóa này được gọi là lịch sử.

Điều gì đã biến con người từ một loài vượn không đáng kể thành bá chủ của thế giới? Cuộc cách mạng nhận thức xảy ra khoảng 70.000 năm trước đã làm thay đổi mọi thứ. Đó là khi Homo sapiens phát triển khả năng sáng tạo ra những điều không tồn tại, không có thật.
"""
                },
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 2,
                    ChapterTitle  = "Phần II: Cây tri thức",
                    WordCount     = 1950,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Homo sapiens đã cai trị thế giới nhờ một khả năng độc đáo: khả năng hợp tác linh hoạt với số lượng lớn người lạ.

Kiến và ong cũng có thể hợp tác với số lượng lớn, nhưng chúng chỉ làm theo những cách rất cứng nhắc và chỉ với những người thân của mình. Sói và tinh tinh hợp tác linh hoạt hơn nhiều, nhưng chỉ với một số lượng nhỏ các cá thể mà chúng biết mặt.

Con người có thể hợp tác cực kỳ linh hoạt với vô số người lạ. Đây là điều đã dẫn đến sự thành công phi thường của chúng ta.

Làm thế nào Homo sapiens đã đạt được khả năng này? Câu trả lời nằm ở ngôn ngữ.

Nhưng ngôn ngữ không phải là điều duy nhất làm nên điều kỳ diệu. Nhiều loài động vật cũng có ngôn ngữ. Điều làm cho ngôn ngữ của chúng ta thực sự độc đáo là khả năng truyền đạt thông tin về những thứ không tồn tại thực sự, như thần linh, quốc gia, tiền bạc và công ty.

Những huyền thoại chung, những câu chuyện chung, những niềm tin chung - đây là thứ liên kết hàng triệu người lạ với nhau thành những xã hội hiệu quả.
"""
                },
            });
            }

            // ── Sách 5: Dám Bị Ghét ──────────────────────────────────────
            if (books.Count >= 5)
            {
                var b = books[4];
                contents.AddRange(new[]
                {
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 1,
                    ChapterTitle  = "Đêm thứ nhất: Phủ nhận chấn thương tâm lý",
                    WordCount     = 1680,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Chàng trai: Thưa tiên sinh, tôi muốn hỏi về triết học của Adler.

Triết gia: Ồ, thú vị đấy. Cậu đã đọc gì về Adler rồi?

Chàng trai: Tôi đã đọc một vài cuốn sách. Nhưng tôi không đồng ý với ông ta.

Triết gia: Về điều gì?

Chàng trai: Adler nói rằng con người có thể thay đổi. Nhưng tôi không tin điều đó. Con người sinh ra thế nào thì sẽ mãi là như vậy. Tính cách không thể thay đổi được.

Triết gia: Tại sao cậu lại tin như vậy?

Chàng trai: Vì tôi đã thấy điều đó với chính bản thân mình. Tôi luôn muốn thay đổi, nhưng tôi không thể. Tôi đã thất bại nhiều lần rồi.

Triết gia: Adler nói rằng chúng ta không bị quá khứ quyết định. Chúng ta quyết định tương lai của mình từ hiện tại.

Chàng trai: Nhưng chấn thương tâm lý thì sao? Những gì đã xảy ra với chúng ta trong quá khứ ảnh hưởng đến chúng ta như thế nào?

Triết gia: Adler phủ nhận chấn thương tâm lý. Ông nói rằng chúng ta không bị kiểm soát bởi những trải nghiệm quá khứ. Thay vào đó, chúng ta sử dụng những trải nghiệm đó như một cái cớ để đạt được mục tiêu của mình trong hiện tại.
"""
                },
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 2,
                    ChapterTitle  = "Đêm thứ hai: Tất cả phiền não đều từ các mối quan hệ mà ra",
                    WordCount     = 1540,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Chàng trai: Hôm qua tiên sinh nói rằng mọi phiền não đều xuất phát từ các mối quan hệ giữa người với người. Nhưng tôi không hiểu điều đó có nghĩa là gì.

Triết gia: Hãy nghĩ về điều này: Nếu cậu sống một mình trên một hòn đảo hoang, cậu có lo lắng về ngoại hình của mình không?

Chàng trai: Không, chắc là không.

Triết gia: Và nếu không có ai khác trong thế giới này, cậu có cảm thấy tự ti về bản thân không?

Chàng trai: Không.

Triết gia: Đó là điểm mấu chốt. Mặc cảm, tự ti, lo lắng về bản thân - tất cả những điều này chỉ xuất hiện khi có mặt người khác. Những phiền não này phát sinh từ các mối quan hệ giữa người với người.

Chàng trai: Nhưng làm sao chúng ta có thể tránh được các mối quan hệ đó?

Triết gia: Adler không nói chúng ta nên tránh né. Ông nói chúng ta cần học cách xây dựng những mối quan hệ lành mạnh, nơi mỗi người đều có không gian của riêng mình.

Bài học: Hạnh phúc thực sự không đến từ việc được người khác chấp thuận. Nó đến từ việc chấp nhận chính mình và đóng góp cho cộng đồng.
"""
                },
            });
            }

            // ── Sách 6: Bố Già ───────────────────────────────────────────
            if (books.Count >= 6)
            {
                var b = books[5];
                contents.AddRange(new[]
                {
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 1,
                    ChapterTitle  = "Chương I",
                    WordCount     = 2050,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Amerigo Bonasera ngồi trong phòng xử án số 3 của tòa án New York và chờ đợi công lý.

Ông ngồi đó từ sáng sớm, mặc bộ đồ đen trang trọng, thẳng thớm như trong ngày lễ. Ông là một con người nghiêm túc, không bao giờ cảm thấy cần ai giúp đỡ trong suốt cuộc đời. Ông tin vào nước Mỹ.

Nhưng hôm nay ông cần công lý.

Hai thanh niên ngồi ở bàn bị cáo, trẻ, đẹp trai, mặc bộ vest đắt tiền. Họ có vẻ rất bình thản. Và tại sao không? Cha mẹ của chúng là những người Mỹ giàu có, quan trọng. Chúng có những luật sư giỏi nhất.

Chúng đã làm những điều khủng khiếp với con gái ông. Chúng đã phá hủy cuộc đời cô bé. Nhưng khi ra tòa, chúng chỉ bị phạt tù treo.

Sau phiên tòa, Amerigo Bonasera đứng bên ngoài, tim đau như bóp nghẹt. Ông biết mình phải làm gì. Ông phải đến gặp Don Corleone.

Don Vito Corleone ngồi sau chiếc bàn lớn trong văn phòng của ông, lắng nghe thỉnh cầu của Bonasera. Ông là một người đàn ông tầm thước, mái tóc đen đã điểm bạc. Ông ngồi đó như một vị vua tiếp kiến dân chúng.

"Bạn bè của tôi," ông nói chậm rãi, "tôi không quen làm những việc như thế này."
"""
                },
                new BookContent
                {
                    BookId        = b.BookId,
                    ChapterNumber = 2,
                    ChapterTitle  = "Chương II",
                    WordCount     = 1890,
                    CreatedAt     = DateTime.Now,
                    Content       = """
Đám cưới của Connie Corleone diễn ra vào một ngày cuối tháng Tám, nắng vàng rực rỡ.

Theo phong tục của người Sicily, không ai được phép từ chối lời mời của Don Corleone vào ngày cưới của con gái ông. Và cũng theo phong tục đó, Don Corleone sẽ không từ chối bất kỳ thỉnh cầu nào được đưa ra vào ngày hôm đó.

Trong khi tiệc cưới đang diễn ra ngoài sân, Don Corleone ngồi trong văn phòng tối của ông và giải quyết công việc. Hagen, cố vấn người Đức-Ireland của ông, ngồi bên cạnh.

Người đến gặp ông đầu tiên là Amerigo Bonasera.

"Bao nhiêu năm qua, tôi không bao giờ đến nhờ vả Don Corleone," Bonasera nói, giọng run rẩy. "Vì tôi không muốn nợ ai ân huệ. Nhưng bây giờ tôi cần sự giúp đỡ."

Don Corleone lắng nghe câu chuyện về con gái của Bonasera. Khuôn mặt ông không lộ vẻ cảm xúc gì.

"Cậu muốn tôi giết những kẻ đó?" ông hỏi.

"Không! Tôi chỉ muốn công lý. Tôi muốn chúng phải trả giá."

Don Corleone gật đầu chậm rãi. "Được rồi. Vì cậu là người bạn cũ của tôi, tôi sẽ giúp cậu. Nhưng nhớ rằng, một ngày nào đó - mà có thể sẽ không bao giờ đến - tôi sẽ cần cậu làm việc gì đó cho tôi."
"""
                },
            });
            }

            context.BookContents.AddRange(contents);
            await context.SaveChangesAsync();
            Console.WriteLine($"  → Seeded {contents.Count} book contents.");
        }
    }
}
