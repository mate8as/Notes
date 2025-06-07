using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Notes.Models;

public partial class NotesContext : IdentityDbContext<User>
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


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (optionsBuilder.IsConfigured)
    //    {
    //        // Use the connection string from configuration
    //        var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();

    //        var connectionString = configuration.GetConnectionString("NotesContext");
    //        optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("8.0.42-mysql"));
    //    }
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseCollation("utf8mb4_hu_0900_as_cs").HasCharSet("utf8mb4");

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


            entity.Property(e => e.NoteFileId).HasColumnName("NoteFile_Id");
            entity.Property(e => e.UserId).HasColumnName("UserId");

            entity.HasOne(d => d.NoteFile).WithMany(p => p.NoteFileUsers)
                .HasForeignKey(d => d.NoteFileId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.NoteFileUsers)
               .HasForeignKey(d => d.UserId)
               .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.PreferredColor).HasMaxLength(10);
            entity.Property(e => e.Id).HasMaxLength(100);

            entity.ToTable("Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
