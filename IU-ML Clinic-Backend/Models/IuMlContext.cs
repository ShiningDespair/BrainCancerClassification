using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BrainCancerClassification.Models;

public partial class IuMlContext : DbContext
{
    public IuMlContext()
    {
    }

    public IuMlContext(DbContextOptions<IuMlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<Prediction> Predictions { get; set; }

    public virtual DbSet<PredictionModel> PredictionModels { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tool> Tools { get; set; }

    public virtual DbSet<ToolImage> ToolImages { get; set; }

    public virtual DbSet<Upload> Uploads { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-1QUI6II;Database=IU-ML;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diseases__3214EC070F67F4F2");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Prediction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Predicti__3214EC0781A95031");

            entity.HasIndex(e => e.ModelId, "IX_Predictions_ModelId");

            entity.HasIndex(e => e.UploadId, "IX_Predictions_UploadId");

            entity.Property(e => e.Confidence).HasColumnType("decimal(5, 4)");
            entity.Property(e => e.PredictedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.PredictionResult).HasMaxLength(100);

            entity.HasOne(d => d.Disease).WithMany(p => p.Predictions)
                .HasForeignKey(d => d.DiseaseId)
                .HasConstraintName("FK_Predictions_Diseases");

            entity.HasOne(d => d.Model).WithMany(p => p.Predictions)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Predictions_Models");

            entity.HasOne(d => d.Upload).WithMany(p => p.Predictions)
                .HasForeignKey(d => d.UploadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Predictions_Uploads");
        });

        modelBuilder.Entity<PredictionModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Predicti__3214EC07853DE106");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.Framework)
                .HasMaxLength(50)
                .HasDefaultValue("TensorFlow/Keras");
            entity.Property(e => e.InputShape).HasMaxLength(50);
            entity.Property(e => e.Labels).HasMaxLength(200);
            entity.Property(e => e.ModelName).HasMaxLength(100);
            entity.Property(e => e.Version).HasMaxLength(20);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC078929C2C3");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61607A072A9C").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Tool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tools__3214EC075393AC45");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Href).HasMaxLength(200);
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<ToolImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ToolImag__3214EC077E404A29");

            entity.HasIndex(e => e.ToolId, "IX_ToolImages_ToolId");

            entity.Property(e => e.Alt).HasMaxLength(200);
            entity.Property(e => e.Src).HasMaxLength(500);

            entity.HasOne(d => d.Tool).WithMany(p => p.ToolImages)
                .HasForeignKey(d => d.ToolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ToolImages_Tools");
        });

        modelBuilder.Entity<Upload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Uploads__3214EC0787C6BFC9");

            entity.HasIndex(e => e.UserId, "IX_Uploads_UserId");

            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.FileType).HasMaxLength(20);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Uploads)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Uploads_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07BE66D15D");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.IsActive, "IX_Users_IsActive");

            entity.HasIndex(e => e.ClerkId, "UQ__Users__4F0B118663B75963").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105341935C8B6").IsUnique();

            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.ClerkId).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.PreferredLanguage)
                .HasMaxLength(5)
                .HasDefaultValue("tr-TR")
                .IsFixedLength();
            entity.Property(e => e.ProfileImageUrl).HasMaxLength(500);
            entity.Property(e => e.RoleId).HasDefaultValue(1);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Users_CreatedBy");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.InverseDeletedByNavigation)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK_Users_DeletedBy");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_Users_UpdatedBy");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
