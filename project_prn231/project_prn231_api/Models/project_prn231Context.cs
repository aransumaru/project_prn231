﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace project_prn231_api.Models
{
    public partial class project_prn231Context : DbContext
    {
        public project_prn231Context()
        {
        }

        public project_prn231Context(DbContextOptions<project_prn231Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<ExamResult> ExamResults { get; set; } = null!;
        public virtual DbSet<ExamResultHistory> ExamResultHistorys { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                                           .SetBasePath(Directory.GetCurrentDirectory())
                                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.AnswerImage).HasMaxLength(255);

                entity.Property(e => e.AnswerText).HasMaxLength(200);

                entity.Property(e => e.IsCorrect).HasDefaultValueSql("((0))");

                entity.Property(e => e.PkQuestionId).HasColumnName("PK_QuestionId");

                entity.HasOne(d => d.PkQuestion)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.PkQuestionId)
                    .HasConstraintName("FK__Answers__PK_Ques__656C112C");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.ExamDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PkCategoryId).HasColumnName("PK_CategoryId");

                entity.Property(e => e.PkUserId).HasColumnName("PK_UserId");

                entity.HasOne(d => d.PkCategory)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.PkCategoryId)
                    .HasConstraintName("FK__Exams__PK_Catego__6A30C649");

                entity.HasOne(d => d.PkUser)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.PkUserId)
                    .HasConstraintName("FK__Exams__PK_UserId__693CA210");
            });

            modelBuilder.Entity<ExamResult>(entity =>
            {
                entity.Property(e => e.PkAnswerId).HasColumnName("PK_AnswerId");

                entity.Property(e => e.PkExamId).HasColumnName("PK_ExamId");

                entity.Property(e => e.PkQuestionId).HasColumnName("PK_QuestionId");

                entity.HasOne(d => d.PkAnswer)
                    .WithMany(p => p.ExamResults)
                    .HasForeignKey(d => d.PkAnswerId)
                    .HasConstraintName("FK__ExamResul__PK_An__6FE99F9F");

                entity.HasOne(d => d.PkExam)
                    .WithMany(p => p.ExamResults)
                    .HasForeignKey(d => d.PkExamId)
                    .HasConstraintName("FK__ExamResul__PK_Ex__6E01572D");

                entity.HasOne(d => d.PkQuestion)
                    .WithMany(p => p.ExamResults)
                    .HasForeignKey(d => d.PkQuestionId)
                    .HasConstraintName("FK__ExamResul__PK_Qu__6EF57B66");
            });

            modelBuilder.Entity<ExamResultHistory>(entity =>
            {
                entity.HasKey(e => e.ExamHistoryId)
                    .HasName("PK__ExamResu__DDE33612B21CCB87");

                entity.Property(e => e.PkExamId).HasColumnName("PK_ExamId");

                entity.Property(e => e.Result).HasMaxLength(50);

                entity.HasOne(d => d.PkExam)
                    .WithMany(p => p.ExamResultHistories)
                    .HasForeignKey(d => d.PkExamId)
                    .HasConstraintName("FK__ExamResul__PK_Ex__72C60C4A");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.PkCategoryId).HasColumnName("PK_CategoryId");

                entity.Property(e => e.QuestionImage).HasMaxLength(255);

                entity.Property(e => e.QuestionText).HasMaxLength(500);

                entity.HasOne(d => d.PkCategory)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.PkCategoryId)
                    .HasConstraintName("FK__Questions__PK_Ca__628FA481");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.IsAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
