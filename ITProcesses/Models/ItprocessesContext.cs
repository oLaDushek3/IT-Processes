﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ITProcesses.Models;

public partial class ItprocessesContext : DbContext
{
    public ItprocessesContext()
    {
    }

    public ItprocessesContext(DbContextOptions<ItprocessesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Archive> Archives { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskDocument> TaskDocuments { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<TaskTag> TaskTags { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersTask> UsersTasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ITProcesses;Username=postgres;Password=qazqaz");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Archive>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("archive_pkey");

            entity.ToTable("archive");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateArchivedTimestamp)
                .HasDefaultValueSql("now()")
                .HasColumnName("date_archived_timestamp");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Task).WithMany(p => p.Archives)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("archive_task_id_fkey");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("documents_pkey");

            entity.ToTable("documents");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Path).HasColumnName("path");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("project_pkey");

            entity.ToTable("project");

            entity.HasIndex(e => e.Name, "project_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "role_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tag_pkey");

            entity.ToTable("tag");

            entity.HasIndex(e => e.Name, "tag_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.ToTable("task");

            entity.HasIndex(e => e.Name, "task_name_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Archived).HasColumnName("archived");
            entity.Property(e => e.DateCreateTimestamp)
                .HasDefaultValueSql("now()")
                .HasColumnName("date_create_timestamp");
            entity.Property(e => e.DateEndTimestamp).HasColumnName("date_end_timestamp");
            entity.Property(e => e.DateStartTimestamp).HasColumnName("date_start_timestamp");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_project_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("task_status_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("task_type_id_fkey");
        });

        modelBuilder.Entity<TaskDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_documents_pkey");

            entity.ToTable("task_documents");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Documents).HasColumnName("documents");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.DocumentsNavigation).WithMany(p => p.TaskDocuments)
                .HasForeignKey(d => d.Documents)
                .HasConstraintName("task_documents_documents_fkey");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskDocuments)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("task_documents_task_id_fkey");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_status_pkey");

            entity.ToTable("task_status");

            entity.HasIndex(e => e.Name, "task_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TaskTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_tag_pkey");

            entity.ToTable("task_tag");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tag).HasColumnName("tag");
            entity.Property(e => e.Task).HasColumnName("task");

            entity.HasOne(d => d.TagNavigation).WithMany(p => p.TaskTags)
                .HasForeignKey(d => d.Tag)
                .HasConstraintName("task_tag_tag_fkey");

            entity.HasOne(d => d.TaskNavigation).WithMany(p => p.TaskTags)
                .HasForeignKey(d => d.Task)
                .HasConstraintName("task_tag_task_fkey");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("type_pkey");

            entity.ToTable("type");

            entity.HasIndex(e => e.Name, "type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<UsersTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_task_pkey");

            entity.ToTable("users_task");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UserComments).HasColumnName("user_comments");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Task).WithMany(p => p.UsersTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("users_task_task_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UsersTasks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("users_task_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
