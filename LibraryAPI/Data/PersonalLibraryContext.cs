using System;
using System.Collections.Generic;
using LibraryAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data;

public partial class PersonalLibraryContext : DbContext
{
    public PersonalLibraryContext() { }

    public PersonalLibraryContext(DbContextOptions<PersonalLibraryContext> options)
        : base(options) { }

    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<BookContent> BookContents { get; set; }
    public virtual DbSet<Bookmark> Bookmarks { get; set; }
    public virtual DbSet<Highlight> Highlights { get; set; }
    public virtual DbSet<ReadingHistory> ReadingHistories { get; set; }
    public virtual DbSet<ReadingProgress> ReadingProgresses { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserLibrary> UserLibraries { get; set; }
   
    public virtual DbSet<Badge> Badges { get; set; }
    public virtual DbSet<UserBadge> UserBadges { get; set; }
    public virtual DbSet<UserStats> UserStats { get; set; }
    public virtual DbSet<BookRating> BookRatings { get; set; }
    public virtual DbSet<Quote> Quotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C207AE246AD9");
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(500);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FileUrl).HasMaxLength(500);
            entity.Property(e => e.Genre).HasMaxLength(50);
            entity.Property(e => e.Language)
                .HasMaxLength(20)
                .HasDefaultValue("Vietnamese");
            entity.Property(e => e.Rating)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 2)");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<BookContent>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__BookCont__2907A81E14F582F5");
            entity.Property(e => e.ChapterTitle).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.HasOne(d => d.Book).WithMany(p => p.BookContents)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BookConte__BookI__4222D4EF");
        });

        modelBuilder.Entity<Bookmark>(entity =>
        {
            entity.HasKey(e => e.BookmarkId).HasName("PK__Bookmark__541A3B7158A17D31");
            entity.HasIndex(e => new { e.UserId, e.BookId }, "IX_Bookmarks_UserId_BookId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.HasOne(d => d.Book).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Bookmarks__BookI__571DF1D5");
            entity.HasOne(d => d.Content).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.ContentId)
                .HasConstraintName("FK__Bookmarks__Conte__5812160E");
            entity.HasOne(d => d.User).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Bookmarks__UserI__5629CD9C");
        });

        modelBuilder.Entity<Highlight>(entity =>
        {
            entity.HasKey(e => e.HighlightId).HasName("PK__Highligh__B11CEDF0145010ED");
            entity.HasIndex(e => new { e.UserId, e.BookId }, "IX_Highlights_UserId_BookId");
            entity.Property(e => e.Color).HasMaxLength(20).HasDefaultValue("yellow");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.HasOne(d => d.Book).WithMany(p => p.Highlights)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Highlight__BookI__5CD6CB2B");
            entity.HasOne(d => d.Content).WithMany(p => p.Highlights)
                .HasForeignKey(d => d.ContentId)
                .HasConstraintName("FK__Highlight__Conte__5DCAEF64");
            entity.HasOne(d => d.User).WithMany(p => p.Highlights)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Highlight__UserI__5BE2A6F2");
        });

        modelBuilder.Entity<ReadingHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__ReadingH__4D7B4ABDCEFE4E28");
            entity.ToTable("ReadingHistory");
            entity.Property(e => e.MinutesRead).HasDefaultValue(0);
            entity.Property(e => e.ReadAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.HasOne(d => d.Book).WithMany(p => p.ReadingHistories)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ReadingHi__BookI__6383C8BA");
            entity.HasOne(d => d.User).WithMany(p => p.ReadingHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ReadingHi__UserI__628FA481");
        });

        modelBuilder.Entity<ReadingProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__ReadingP__BAE29CA5AFA27C7B");
            entity.ToTable("ReadingProgress");
            entity.HasIndex(e => new { e.UserId, e.BookId }, "IX_ReadingProgress_UserId_BookId");
            entity.HasIndex(e => new { e.UserId, e.BookId }, "UQ__ReadingP__7456C06DB545D34F").IsUnique();
            entity.Property(e => e.CurrentPosition).HasDefaultValue(0);
            entity.Property(e => e.LastReadAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProgressPercentage)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.HasOne(d => d.Book).WithMany(p => p.ReadingProgresses)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ReadingPr__BookI__4F7CD00D");
            entity.HasOne(d => d.Content).WithMany(p => p.ReadingProgresses)
                .HasForeignKey(d => d.ContentId)
                .HasConstraintName("FK__ReadingPr__Conte__5070F446");
            entity.HasOne(d => d.User).WithMany(p => p.ReadingProgresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ReadingPr__UserI__4E88ABD4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CCD05769E");
            entity.HasIndex(e => e.Username, "UQ__Users__536C85E44497EC7B").IsUnique();
            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534E2D44D2B").IsUnique();
            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(20).HasDefaultValue("user");
        });

        modelBuilder.Entity<UserLibrary>(entity =>
        {
            entity.HasKey(e => e.UserLibraryId).HasName("PK__UserLibr__5B936BDD585C2560");
            entity.ToTable("UserLibrary");
            entity.HasIndex(e => e.UserId, "IX_UserLibrary_UserId");
            entity.HasIndex(e => new { e.UserId, e.BookId }, "UQ__UserLibr__7456C06DA4BA71BB").IsUnique();
            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsFavorite).HasDefaultValue(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Want to Read");

            // MỚI
            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.IsCountedAsRead).HasDefaultValue(false);

            entity.HasOne(d => d.Book).WithMany(p => p.UserLibraries)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__UserLibra__BookI__47DBAE45");
            entity.HasOne(d => d.User).WithMany(p => p.UserLibraries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__UserLibra__UserI__46E78A0C");
        });

     
        modelBuilder.Entity<Badge>(entity =>
        {
            entity.HasKey(e => e.BadgeId);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Icon).HasMaxLength(10).HasDefaultValue("🏅");
            entity.Property(e => e.ConditionType).HasMaxLength(50).IsRequired();
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
        });

       
        modelBuilder.Entity<UserBadge>(entity =>
        {
            entity.HasKey(e => e.UserBadgeId);

          
            entity.HasIndex(e => new { e.UserId, e.BadgeId })
                  .IsUnique()
                  .HasDatabaseName("UQ_UserBadge_UserId_BadgeId");

            entity.Property(e => e.EarnedAt)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.UserBadges)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Badge)
                  .WithMany(p => p.UserBadges)
                  .HasForeignKey(d => d.BadgeId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

     
        modelBuilder.Entity<UserStats>(entity =>
        {
            entity.HasKey(e => e.UserStatsId);

            entity.HasIndex(e => e.UserId)
                  .IsUnique()
                  .HasDatabaseName("UQ_UserStats_UserId");

            entity.Property(e => e.TotalBooksRead).HasDefaultValue(0);
            entity.Property(e => e.TotalBooksStarted).HasDefaultValue(0);
            entity.Property(e => e.TotalPagesRead).HasDefaultValue(0);
            entity.Property(e => e.TotalMinutesRead).HasDefaultValue(0);
            entity.Property(e => e.TotalWordsRead).HasDefaultValue(0);
            entity.Property(e => e.CurrentStreak).HasDefaultValue(0);
            entity.Property(e => e.LongestStreak).HasDefaultValue(0);
            entity.Property(e => e.FavoriteGenre).HasMaxLength(50);
            entity.Property(e => e.Rank).HasMaxLength(50).HasDefaultValue("Mầm Đọc");
            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");
            entity.Property(e => e.LastReadDate).HasColumnType("date");

            entity.HasOne(d => d.User)
                  .WithOne(p => p.Stats)
                  .HasForeignKey<UserStats>(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        
        modelBuilder.Entity<BookRating>(entity =>
        {
            entity.HasKey(e => e.RatingId);

            
            entity.HasIndex(e => new { e.UserId, e.BookId })
                  .IsUnique()
                  .HasDatabaseName("UQ_BookRating_UserId_BookId");

            entity.Property(e => e.Stars)
                  .IsRequired();

        
            entity.ToTable(t => t.HasCheckConstraint("CK_BookRating_Stars", "[Stars] >= 1 AND [Stars] <= 5"));

            entity.Property(e => e.Review).HasMaxLength(2000);
            entity.Property(e => e.IsVerifiedReader).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.BookRatings)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Book)
                  .WithMany()
                  .HasForeignKey(d => d.BookId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

       
        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.QuoteId);

            entity.HasIndex(e => new { e.UserId, e.BookId })
                  .HasDatabaseName("IX_Quotes_UserId_BookId");

         
            entity.HasIndex(e => e.IsPublic)
                  .HasDatabaseName("IX_Quotes_IsPublic");

            entity.Property(e => e.QuoteText).IsRequired(); 
            entity.Property(e => e.PersonalNote).HasMaxLength(1000);
            entity.Property(e => e.IsPublic).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Quotes)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Book)
                  .WithMany()
                  .HasForeignKey(d => d.BookId)
                  .OnDelete(DeleteBehavior.NoAction); 

            entity.HasOne(d => d.Content)
                  .WithMany()
                  .HasForeignKey(d => d.ContentId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}