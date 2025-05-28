using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Notes.Models;

public partial class NotesContext : DbContext
{
    public NotesContext()
    {
    }

    public NotesContext(DbContextOptions<NotesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NoteFile> NoteFiles { get; set; }

    public virtual DbSet<NoteFileUser> NoteFileUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=145.223.82.144,3306;database=Notes;user id=root;password=Csillagom77?", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_hu_0900_as_cs")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<NoteFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedById).HasColumnName("CreatedBy_Id");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<NoteFileUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("NoteFile_Users");

            entity.HasIndex(e => e.NoteFileId, "FK_NoteFileUsers_NoteFiles");

            entity.HasIndex(e => e.UserId, "FK_NoteFileUsers_Users");

            entity.Property(e => e.NoteFileId).HasColumnName("NoteFile_Id");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.NoteFile).WithMany(p => p.NoteFileUsers)
                .HasForeignKey(d => d.NoteFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoteFileUsers_NoteFiles");

            entity.HasOne(d => d.User).WithMany(p => p.NoteFileUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoteFileUsers_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Password).HasColumnType("text");
            entity.Property(e => e.PreferredColor).HasMaxLength(10);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
