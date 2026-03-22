using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    BadgeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "🏅"),
                    ConditionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Threshold = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.BadgeId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PageCount = table.Column<int>(type: "int", nullable: true),
                    PublishedYear = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: true, defaultValue: 0m),
                    Language = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Vietnamese"),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Books__3DE0C207AE246AD9", x => x.BookId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "user"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4CCD05769E", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BookContents",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    ChapterNumber = table.Column<int>(type: "int", nullable: false),
                    ChapterTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordCount = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BookCont__2907A81E14F582F5", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK__BookConte__BookI__4222D4EF",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookRatings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Stars = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsVerifiedReader = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    BookId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRatings", x => x.RatingId);
                    table.CheckConstraint("CK_BookRating_Stars", "[Stars] >= 1 AND [Stars] <= 5");
                    table.ForeignKey(
                        name: "FK_BookRatings_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRatings_Books_BookId1",
                        column: x => x.BookId1,
                        principalTable: "Books",
                        principalColumn: "BookId");
                    table.ForeignKey(
                        name: "FK_BookRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadingHistory",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    MinutesRead = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ReadingH__4D7B4ABDCEFE4E28", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK__ReadingHi__BookI__6383C8BA",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ReadingHi__UserI__628FA481",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBadges",
                columns: table => new
                {
                    UserBadgeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BadgeId = table.Column<int>(type: "int", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBadges", x => x.UserBadgeId);
                    table.ForeignKey(
                        name: "FK_UserBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "BadgeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBadges_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLibrary",
                columns: table => new
                {
                    UserLibraryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    AddedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Want to Read"),
                    CompletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsCountedAsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserLibr__5B936BDD585C2560", x => x.UserLibraryId);
                    table.ForeignKey(
                        name: "FK__UserLibra__BookI__47DBAE45",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__UserLibra__UserI__46E78A0C",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStats",
                columns: table => new
                {
                    UserStatsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalBooksRead = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalBooksStarted = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalPagesRead = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalMinutesRead = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalWordsRead = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentStreak = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LongestStreak = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastReadDate = table.Column<DateTime>(type: "date", nullable: true),
                    FavoriteGenre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rank = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Mầm Đọc"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStats", x => x.UserStatsId);
                    table.ForeignKey(
                        name: "FK_UserStats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    ContentId = table.Column<int>(type: "int", nullable: true),
                    ChapterNumber = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bookmark__541A3B7158A17D31", x => x.BookmarkId);
                    table.ForeignKey(
                        name: "FK__Bookmarks__BookI__571DF1D5",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Bookmarks__Conte__5812160E",
                        column: x => x.ContentId,
                        principalTable: "BookContents",
                        principalColumn: "ContentId");
                    table.ForeignKey(
                        name: "FK__Bookmarks__UserI__5629CD9C",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Highlights",
                columns: table => new
                {
                    HighlightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    ContentId = table.Column<int>(type: "int", nullable: true),
                    StartPosition = table.Column<int>(type: "int", nullable: false),
                    EndPosition = table.Column<int>(type: "int", nullable: false),
                    HighlightedText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "yellow"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Highligh__B11CEDF0145010ED", x => x.HighlightId);
                    table.ForeignKey(
                        name: "FK__Highlight__BookI__5CD6CB2B",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Highlight__Conte__5DCAEF64",
                        column: x => x.ContentId,
                        principalTable: "BookContents",
                        principalColumn: "ContentId");
                    table.ForeignKey(
                        name: "FK__Highlight__UserI__5BE2A6F2",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    QuoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: true),
                    QuoteText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StartPosition = table.Column<int>(type: "int", nullable: true),
                    EndPosition = table.Column<int>(type: "int", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.QuoteId);
                    table.ForeignKey(
                        name: "FK_Quotes_BookContents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "BookContents",
                        principalColumn: "ContentId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Quotes_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId");
                    table.ForeignKey(
                        name: "FK_Quotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadingProgress",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    ContentId = table.Column<int>(type: "int", nullable: true),
                    CurrentChapter = table.Column<int>(type: "int", nullable: false),
                    CurrentPosition = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true, defaultValue: 0m),
                    LastReadAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ReadingP__BAE29CA5AFA27C7B", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK__ReadingPr__BookI__4F7CD00D",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ReadingPr__Conte__5070F446",
                        column: x => x.ContentId,
                        principalTable: "BookContents",
                        principalColumn: "ContentId");
                    table.ForeignKey(
                        name: "FK__ReadingPr__UserI__4E88ABD4",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookContents_BookId",
                table: "BookContents",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_BookId",
                table: "Bookmarks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_ContentId",
                table: "Bookmarks",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_UserId_BookId",
                table: "Bookmarks",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.CreateIndex(
                name: "IX_BookRatings_BookId",
                table: "BookRatings",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRatings_BookId1",
                table: "BookRatings",
                column: "BookId1");

            migrationBuilder.CreateIndex(
                name: "UQ_BookRating_UserId_BookId",
                table: "BookRatings",
                columns: new[] { "UserId", "BookId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Highlights_BookId",
                table: "Highlights",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Highlights_ContentId",
                table: "Highlights",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Highlights_UserId_BookId",
                table: "Highlights",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_BookId",
                table: "Quotes",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_ContentId",
                table: "Quotes",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_IsPublic",
                table: "Quotes",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_UserId_BookId",
                table: "Quotes",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingHistory_BookId",
                table: "ReadingHistory",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingHistory_UserId",
                table: "ReadingHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingProgress_BookId",
                table: "ReadingProgress",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingProgress_ContentId",
                table: "ReadingProgress",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingProgress_UserId_BookId",
                table: "ReadingProgress",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.CreateIndex(
                name: "UQ__ReadingP__7456C06DB545D34F",
                table: "ReadingProgress",
                columns: new[] { "UserId", "BookId" },
                unique: true,
                filter: "[UserId] IS NOT NULL AND [BookId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserBadges_BadgeId",
                table: "UserBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "UQ_UserBadge_UserId_BadgeId",
                table: "UserBadges",
                columns: new[] { "UserId", "BadgeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLibrary_BookId",
                table: "UserLibrary",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLibrary_UserId",
                table: "UserLibrary",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__UserLibr__7456C06DA4BA71BB",
                table: "UserLibrary",
                columns: new[] { "UserId", "BookId" },
                unique: true,
                filter: "[UserId] IS NOT NULL AND [BookId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__536C85E44497EC7B",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D10534E2D44D2B",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_UserStats_UserId",
                table: "UserStats",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropTable(
                name: "BookRatings");

            migrationBuilder.DropTable(
                name: "Highlights");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "ReadingHistory");

            migrationBuilder.DropTable(
                name: "ReadingProgress");

            migrationBuilder.DropTable(
                name: "UserBadges");

            migrationBuilder.DropTable(
                name: "UserLibrary");

            migrationBuilder.DropTable(
                name: "UserStats");

            migrationBuilder.DropTable(
                name: "BookContents");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
