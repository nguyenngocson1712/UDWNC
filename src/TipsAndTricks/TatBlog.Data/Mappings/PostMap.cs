﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TatBlog.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(p => p.Id);
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(t => t.ShortDescription)
               .IsRequired()
               .HasMaxLength(5000);
            builder.Property(p => p.Description)
                .IsRequired()
               .HasMaxLength(5000);
            builder.Property(p => p.UrlSlug)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(p => p.Meta)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(p => p.ImageUrl)

                .HasMaxLength(1000);
            builder.Property(p => p.ViewCount)
                .IsRequired()
                .HasDefaultValue(false);
            builder.Property(p => p.Published)
                .IsRequired()
                .HasDefaultValue(false);
            builder.Property(p => p.PostedDate)
                .HasColumnType("datetime");
            builder.Property(p => p.ModifiedDate)
                .HasColumnType("datetime");
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .HasConstraintName("FK_Posts_Categories")
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Author)
               .WithMany(a => a.Posts)
               .HasForeignKey(p => p.AuthorId)
               .HasConstraintName("FK_Posts_Authors")
               .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.Tags)
               .WithMany(t => t.Posts)
               .UsingEntity(pt => pt.ToTable("PostTags"));
        }
    }
}