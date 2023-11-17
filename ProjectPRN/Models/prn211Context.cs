using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectPRN.Models
{
    public partial class prn211Context : DbContext
    {
        public prn211Context()
        {
        }

        public prn211Context(DbContextOptions<prn211Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CourseContent> CourseContents { get; set; } = null!;
        public virtual DbSet<CourseDetail> CourseDetails { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<Lecture> Lectures { get; set; } = null!;
        public virtual DbSet<Progress> Progresses { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server = GL-M; database = prn211;uid=sa;pwd=123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("course");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.LectureId).HasColumnName("lecture_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Weeks).HasColumnName("weeks");

                entity.HasOne(d => d.Lecture)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.LectureId)
                    .HasConstraintName("FK_course_lecture");
            });

            modelBuilder.Entity<CourseContent>(entity =>
            {
                entity.HasKey(e => e.CId);

                entity.ToTable("course_content");

                entity.Property(e => e.CId)
                    .ValueGeneratedNever()
                    .HasColumnName("c_id");

                entity.Property(e => e.W1)
                    .HasMaxLength(255)
                    .HasColumnName("w1");

                entity.Property(e => e.W2)
                    .HasMaxLength(255)
                    .HasColumnName("w2");

                entity.Property(e => e.W3)
                    .HasMaxLength(255)
                    .HasColumnName("w3");

                entity.Property(e => e.W4)
                    .HasMaxLength(255)
                    .HasColumnName("w4");

                entity.Property(e => e.W5)
                    .HasMaxLength(255)
                    .HasColumnName("w5");

                entity.Property(e => e.W6)
                    .HasMaxLength(255)
                    .HasColumnName("w6");

                entity.HasOne(d => d.CIdNavigation)
                    .WithOne(p => p.CourseContent)
                    .HasForeignKey<CourseContent>(d => d.CId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_course_content_course");
            });

            modelBuilder.Entity<CourseDetail>(entity =>
            {
                entity.ToTable("course_detail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EId).HasColumnName("e_id");

                entity.Property(e => e.W1).HasColumnType("datetime");

                entity.Property(e => e.W2).HasColumnType("datetime");

                entity.Property(e => e.W3).HasColumnType("datetime");

                entity.Property(e => e.W4).HasColumnType("datetime");

                entity.Property(e => e.W5).HasColumnType("datetime");

                entity.Property(e => e.W6).HasColumnType("datetime");

                entity.HasOne(d => d.EIdNavigation)
                    .WithMany(p => p.CourseDetails)
                    .HasForeignKey(d => d.EId)
                    .HasConstraintName("FK__course_det__e_id__29221CFB");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("enrollment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CId).HasColumnName("c_id");

                entity.Property(e => e.EDate)
                    .HasColumnType("datetime")
                    .HasColumnName("e_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ovd)
                    .HasColumnName("ovd")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SId).HasColumnName("s_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CId)
                    .HasConstraintName("FK__enrollment__c_id__3F466844");

                entity.HasOne(d => d.SIdNavigation)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.SId)
                    .HasConstraintName("FK__enrollment__s_id__3E52440B");
            });

            modelBuilder.Entity<Lecture>(entity =>
            {
                entity.ToTable("lecture");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(1000)
                    .HasColumnName("avatar");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Progress>(entity =>
            {
                entity.ToTable("progress");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.As1)
                    .HasMaxLength(255)
                    .HasColumnName("AS1");

                entity.Property(e => e.As2)
                    .HasMaxLength(255)
                    .HasColumnName("AS2");

                entity.Property(e => e.As3)
                    .HasMaxLength(255)
                    .HasColumnName("AS3");

                entity.Property(e => e.EId).HasColumnName("e_id");

                entity.Property(e => e.Ed1)
                    .HasColumnName("ed1")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Ed2)
                    .HasColumnName("ed2")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Ed3).HasColumnName("ed3");

                entity.HasOne(d => d.EIdNavigation)
                    .WithMany(p => p.Progresses)
                    .HasForeignKey(d => d.EId)
                    .HasConstraintName("FK_progress_enrollment");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("student");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(1000)
                    .HasColumnName("avatar");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(255)
                    .HasColumnName("display_name");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
