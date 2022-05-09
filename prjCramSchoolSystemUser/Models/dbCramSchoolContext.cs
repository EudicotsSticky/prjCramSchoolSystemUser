using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace prjCoreCramSchoolDB.Models
{
    public partial class dbCramSchoolContext : DbContext
    {
        public dbCramSchoolContext()
        {
        }

        public dbCramSchoolContext(DbContextOptions<dbCramSchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TCommentPhoto> TCommentPhotos { get; set; }
        public virtual DbSet<TCommentThumbUp> TCommentThumbUps { get; set; }
        public virtual DbSet<TPost> TPosts { get; set; }
        public virtual DbSet<TPostComment> TPostComments { get; set; }
        public virtual DbSet<TPostThumbUp> TPostThumbUps { get; set; }
        public virtual DbSet<TSubComment> TSubComments { get; set; }
        public virtual DbSet<TSubCommentPhoto> TSubCommentPhotos { get; set; }
        public virtual DbSet<TSubCommentThumbUp> TSubCommentThumbUps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=dbCramSchool;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<TCommentPhoto>(entity =>
            {
                entity.HasKey(e => e.FCommentPhotoId);

                entity.ToTable("tCommentPhoto");

                entity.Property(e => e.FCommentPhotoId)
                    .HasMaxLength(50)
                    .HasColumnName("fCommentPhotoID");

                entity.Property(e => e.FCommentId)
                    .HasMaxLength(50)
                    .HasColumnName("fCommentID");

                entity.Property(e => e.FCommentPhotoData).HasColumnName("fCommentPhotoData");

                entity.Property(e => e.FCommentPhotoName)
                    .HasMaxLength(50)
                    .HasColumnName("fCommentPhotoName");

                entity.HasOne(d => d.FCommentPhoto)
                    .WithOne(p => p.TCommentPhoto)
                    .HasForeignKey<TCommentPhoto>(d => d.FCommentPhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tCommentPhoto_tSubComment");
            });

            modelBuilder.Entity<TCommentThumbUp>(entity =>
            {
                entity.HasKey(e => e.FComThumbUpId);

                entity.ToTable("tCommentThumbUp");

                entity.Property(e => e.FComThumbUpId)
                    .HasMaxLength(50)
                    .HasColumnName("fComThumbUpID");

                entity.Property(e => e.FAccount)
                    .HasMaxLength(50)
                    .HasColumnName("fAccount");

                entity.Property(e => e.FCommentId)
                    .HasMaxLength(50)
                    .HasColumnName("fCommentID");

                entity.Property(e => e.FThumbUpCheck).HasColumnName("fThumbUpCheck");

                entity.Property(e => e.FThumbUpTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fThumbUpTime");

                entity.HasOne(d => d.FComThumbUp)
                    .WithOne(p => p.TCommentThumbUp)
                    .HasForeignKey<TCommentThumbUp>(d => d.FComThumbUpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tCommentThumbUp_tCommentThumbUp");
            });

            modelBuilder.Entity<TPost>(entity =>
            {
                entity.HasKey(e => e.FPostId);

                entity.ToTable("tPost");

                entity.Property(e => e.FPostId)
                    .HasMaxLength(50)
                    .HasColumnName("fPostID");

                entity.Property(e => e.FAccount)
                    .HasMaxLength(50)
                    .HasColumnName("fAccount");

                entity.Property(e => e.FPhotoId)
                    .HasMaxLength(50)
                    .HasColumnName("fPhotoID");

                entity.Property(e => e.FPostContent).HasColumnName("fPostContent");

                entity.Property(e => e.FPostSort)
                    .HasMaxLength(50)
                    .HasColumnName("fPostSort");

                entity.Property(e => e.FPostTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fPostTime");

                entity.Property(e => e.FPostTitle)
                    .HasMaxLength(50)
                    .HasColumnName("fPostTitle");

                entity.Property(e => e.FPostUpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fPostUpdateTime");
            });

            modelBuilder.Entity<TPostComment>(entity =>
            {
                entity.HasKey(e => e.TCommentId);

                entity.ToTable("tPostComment");

                entity.Property(e => e.TCommentId)
                    .HasMaxLength(50)
                    .HasColumnName("tCommentID");

                entity.Property(e => e.FAccount)
                    .HasMaxLength(50)
                    .HasColumnName("fAccount");

                entity.Property(e => e.FCommentContent).HasColumnName("fCommentContent");

                entity.Property(e => e.FCommentTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fCommentTime");

                entity.Property(e => e.FPhotoId)
                    .HasMaxLength(50)
                    .HasColumnName("fPhotoID");

                entity.Property(e => e.FPostId)
                    .HasMaxLength(50)
                    .HasColumnName("fPostID");

                entity.HasOne(d => d.TComment)
                    .WithOne(p => p.TPostComment)
                    .HasForeignKey<TPostComment>(d => d.TCommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tPostComment_tPost");
            });

            modelBuilder.Entity<TPostThumbUp>(entity =>
            {
                entity.HasKey(e => e.FThumbUpId);

                entity.ToTable("tPostThumbUp");

                entity.HasIndex(e => e.FThumbUpId, "IX_tPostThumbUp");

                entity.Property(e => e.FThumbUpId)
                    .HasMaxLength(50)
                    .HasColumnName("fThumbUpID");

                entity.Property(e => e.FAccount)
                    .HasMaxLength(50)
                    .HasColumnName("fAccount");

                entity.Property(e => e.FPostId)
                    .HasMaxLength(50)
                    .HasColumnName("fPostID");

                entity.Property(e => e.FThumbUpCheck).HasColumnName("fThumbUpCheck");

                entity.Property(e => e.FThumbUpTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fThumbUpTime");

                entity.HasOne(d => d.FThumbUp)
                    .WithOne(p => p.TPostThumbUp)
                    .HasForeignKey<TPostThumbUp>(d => d.FThumbUpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tPostThumbUp_tPost");
            });

            modelBuilder.Entity<TSubComment>(entity =>
            {
                entity.HasKey(e => e.FSubCommentId)
                    .HasName("PK_tSubComment_1");

                entity.ToTable("tSubComment");

                entity.Property(e => e.FSubCommentId)
                    .HasMaxLength(50)
                    .HasColumnName("fSubCommentID");

                entity.Property(e => e.FAccount)
                    .HasMaxLength(50)
                    .HasColumnName("fAccount");

                entity.Property(e => e.FCommentId)
                    .HasMaxLength(50)
                    .HasColumnName("fCommentID");

                entity.Property(e => e.FPhotoId)
                    .HasMaxLength(50)
                    .HasColumnName("fPhotoID");

                entity.Property(e => e.FPostId)
                    .HasMaxLength(50)
                    .HasColumnName("fPostID");

                entity.Property(e => e.FSubCommentContent).HasColumnName("fSubCommentContent");

                entity.Property(e => e.FSubCommentTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fSubCommentTime");

                entity.HasOne(d => d.FSubComment)
                    .WithOne(p => p.TSubComment)
                    .HasForeignKey<TSubComment>(d => d.FSubCommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tSubComment_tPost1");
            });

            modelBuilder.Entity<TSubCommentPhoto>(entity =>
            {
                entity.HasKey(e => e.FSubCommentPhotoId);

                entity.ToTable("tSubCommentPhoto");

                entity.Property(e => e.FSubCommentPhotoId)
                    .HasMaxLength(50)
                    .HasColumnName("fSubCommentPhotoID");

                entity.Property(e => e.FSubCommentId)
                    .HasMaxLength(50)
                    .HasColumnName("fSubCommentID");

                entity.Property(e => e.FSubCommentPhotoData).HasColumnName("fSubCommentPhotoData");

                entity.Property(e => e.FSubCommentPhotoName)
                    .HasMaxLength(50)
                    .HasColumnName("fSubCommentPhotoName");

                entity.HasOne(d => d.FSubCommentPhoto)
                    .WithOne(p => p.TSubCommentPhoto)
                    .HasForeignKey<TSubCommentPhoto>(d => d.FSubCommentPhotoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tSubCommentPhoto_tSubComment");
            });

            modelBuilder.Entity<TSubCommentThumbUp>(entity =>
            {
                entity.HasKey(e => e.FSubComThumbUpId);

                entity.ToTable("tSubCommentThumbUp");

                entity.Property(e => e.FSubComThumbUpId)
                    .HasMaxLength(50)
                    .HasColumnName("fSubComThumbUpID");

                entity.Property(e => e.FAccount)
                    .HasMaxLength(50)
                    .HasColumnName("fAccount");

                entity.Property(e => e.FSubComThumbUpTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fSubComThumbUpTime");

                entity.Property(e => e.FSubCommentId)
                    .HasMaxLength(50)
                    .HasColumnName("fSubCommentID");

                entity.Property(e => e.FSubSubThumbCheck).HasColumnName("fSubSubThumbCheck");

                entity.HasOne(d => d.FSubComThumbUp)
                    .WithOne(p => p.TSubCommentThumbUp)
                    .HasForeignKey<TSubCommentThumbUp>(d => d.FSubComThumbUpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tSubCommentThumbUp_tSubComment");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
