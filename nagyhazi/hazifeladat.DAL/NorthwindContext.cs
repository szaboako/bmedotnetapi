using hazifeladat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.DAL
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tag>()
                .Property(t => t.Name)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Tag>().HasData(
                    new Tag { Id = 1, Name = "Gazdaság" },
                    new Tag { Id = 2, Name = "Bulvár"}
             );

            modelBuilder.Entity<Picture>().HasData(
                    new Picture { Id = 1, Name = "tom_and_jerry", Description= "Tom kergeti Jerry-t", Path = Path.Combine(Environment.CurrentDirectory, @"Pictures\", "tom_and_jerry") }
            );

            modelBuilder.Entity<Article>().HasData(
                new Article { Id = 1, Label = "Tom még mindig kergeti Jerry-t", Description= "Macska egér harc", Content="Tom évek óta megszálottan kergeti Jerry-t, sosem áll le, sose lassít.", PictureId = 1 }
            );

            modelBuilder.Entity<ArticleTag>().HasData(
                new ArticleTag { Id = 1, ArticleId = 1, TagId = 2 }
            );

            modelBuilder.Entity<Article>()
            .HasMany(a => a.Tags)
            .WithMany(t => t.Articles)
            .UsingEntity<ArticleTag>(
                j => j
                    .HasOne(at => at.Tag)
                    .WithMany(t => t.TaggedArticles)
                    .HasForeignKey(at => at.TagId)
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne(at => at.Article)
                    .WithMany(a => a.ArticleTags)
                    .HasForeignKey(at => at.ArticleId)
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey(at => at.Id);
                });
        }


        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Picture> Pictures { get; set; }

    }
}
