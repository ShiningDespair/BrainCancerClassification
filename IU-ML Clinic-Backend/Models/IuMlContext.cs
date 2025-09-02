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

    public virtual DbSet<AuthGroup> AuthGroups { get; set; }

    public virtual DbSet<AuthGroupPermission> AuthGroupPermissions { get; set; }

    public virtual DbSet<AuthPermission> AuthPermissions { get; set; }

    public virtual DbSet<AuthUser> AuthUsers { get; set; }

    public virtual DbSet<AuthUserGroup> AuthUserGroups { get; set; }

    public virtual DbSet<AuthUserUserPermission> AuthUserUserPermissions { get; set; }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<DjangoAdminLog> DjangoAdminLogs { get; set; }

    public virtual DbSet<DjangoContentType> DjangoContentTypes { get; set; }

    public virtual DbSet<DjangoMigration> DjangoMigrations { get; set; }

    public virtual DbSet<DjangoSession> DjangoSessions { get; set; }

    public virtual DbSet<Prediction> Predictions { get; set; }

    public virtual DbSet<PredictionModel> PredictionModels { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tool> Tools { get; set; }

    public virtual DbSet<ToolImage> ToolImages { get; set; }

    public virtual DbSet<Upload> Uploads { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-1QUI6II;Database=IU-ML;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__auth_gro__3213E83F58105432");

            entity.ToTable("auth_group");

            entity.HasIndex(e => e.Name, "auth_group_name_a6ea08ec_uniq").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<AuthGroupPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__auth_gro__3213E83FA963FF51");

            entity.ToTable("auth_group_permissions");

            entity.HasIndex(e => e.GroupId, "auth_group_permissions_group_id_b120cbf9");

            entity.HasIndex(e => new { e.GroupId, e.PermissionId }, "auth_group_permissions_group_id_permission_id_0cd325b0_uniq")
                .IsUnique()
                .HasFilter("([group_id] IS NOT NULL AND [permission_id] IS NOT NULL)");

            entity.HasIndex(e => e.PermissionId, "auth_group_permissions_permission_id_84c5c92e");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");

            entity.HasOne(d => d.Group).WithMany(p => p.AuthGroupPermissions)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_group_permissions_group_id_b120cbf9_fk_auth_group_id");

            entity.HasOne(d => d.Permission).WithMany(p => p.AuthGroupPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_group_permissions_permission_id_84c5c92e_fk_auth_permission_id");
        });

        modelBuilder.Entity<AuthPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__auth_per__3213E83FC4413AC2");

            entity.ToTable("auth_permission");

            entity.HasIndex(e => e.ContentTypeId, "auth_permission_content_type_id_2f476e4b");

            entity.HasIndex(e => new { e.ContentTypeId, e.Codename }, "auth_permission_content_type_id_codename_01ab375a_uniq")
                .IsUnique()
                .HasFilter("([content_type_id] IS NOT NULL AND [codename] IS NOT NULL)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codename)
                .HasMaxLength(100)
                .HasColumnName("codename");
            entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.ContentType).WithMany(p => p.AuthPermissions)
                .HasForeignKey(d => d.ContentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_permission_content_type_id_2f476e4b_fk_django_content_type_id");
        });

        modelBuilder.Entity<AuthUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__auth_use__3213E83FB818D54D");

            entity.ToTable("auth_user");

            entity.HasIndex(e => e.Username, "auth_user_username_6821ab7c_uniq").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateJoined).HasColumnName("date_joined");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .HasColumnName("first_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsStaff).HasColumnName("is_staff");
            entity.Property(e => e.IsSuperuser).HasColumnName("is_superuser");
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(150)
                .HasColumnName("username");
        });

        modelBuilder.Entity<AuthUserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__auth_use__3213E83FAE9DFA22");

            entity.ToTable("auth_user_groups");

            entity.HasIndex(e => e.GroupId, "auth_user_groups_group_id_97559544");

            entity.HasIndex(e => e.UserId, "auth_user_groups_user_id_6a12ed8b");

            entity.HasIndex(e => new { e.UserId, e.GroupId }, "auth_user_groups_user_id_group_id_94350c0c_uniq")
                .IsUnique()
                .HasFilter("([user_id] IS NOT NULL AND [group_id] IS NOT NULL)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Group).WithMany(p => p.AuthUserGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_user_groups_group_id_97559544_fk_auth_group_id");

            entity.HasOne(d => d.User).WithMany(p => p.AuthUserGroups)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_user_groups_user_id_6a12ed8b_fk_auth_user_id");
        });

        modelBuilder.Entity<AuthUserUserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__auth_use__3213E83FD2970D76");

            entity.ToTable("auth_user_user_permissions");

            entity.HasIndex(e => e.PermissionId, "auth_user_user_permissions_permission_id_1fbb5f2c");

            entity.HasIndex(e => e.UserId, "auth_user_user_permissions_user_id_a95ead1b");

            entity.HasIndex(e => new { e.UserId, e.PermissionId }, "auth_user_user_permissions_user_id_permission_id_14a6b632_uniq")
                .IsUnique()
                .HasFilter("([user_id] IS NOT NULL AND [permission_id] IS NOT NULL)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Permission).WithMany(p => p.AuthUserUserPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_user_user_permissions_permission_id_1fbb5f2c_fk_auth_permission_id");

            entity.HasOne(d => d.User).WithMany(p => p.AuthUserUserPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auth_user_user_permissions_user_id_a95ead1b_fk_auth_user_id");
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diseases__3214EC070F67F4F2");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<DjangoAdminLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__django_a__3213E83F0612990E");

            entity.ToTable("django_admin_log");

            entity.HasIndex(e => e.ContentTypeId, "django_admin_log_content_type_id_c4bce8eb");

            entity.HasIndex(e => e.UserId, "django_admin_log_user_id_c564eba6");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionFlag).HasColumnName("action_flag");
            entity.Property(e => e.ActionTime).HasColumnName("action_time");
            entity.Property(e => e.ChangeMessage).HasColumnName("change_message");
            entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");
            entity.Property(e => e.ObjectId).HasColumnName("object_id");
            entity.Property(e => e.ObjectRepr)
                .HasMaxLength(200)
                .HasColumnName("object_repr");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ContentType).WithMany(p => p.DjangoAdminLogs)
                .HasForeignKey(d => d.ContentTypeId)
                .HasConstraintName("django_admin_log_content_type_id_c4bce8eb_fk_django_content_type_id");

            entity.HasOne(d => d.User).WithMany(p => p.DjangoAdminLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("django_admin_log_user_id_c564eba6_fk_auth_user_id");
        });

        modelBuilder.Entity<DjangoContentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__django_c__3213E83FEA575031");

            entity.ToTable("django_content_type");

            entity.HasIndex(e => new { e.AppLabel, e.Model }, "django_content_type_app_label_model_76bd3d3b_uniq")
                .IsUnique()
                .HasFilter("([app_label] IS NOT NULL AND [model] IS NOT NULL)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppLabel)
                .HasMaxLength(100)
                .HasColumnName("app_label");
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .HasColumnName("model");
        });

        modelBuilder.Entity<DjangoMigration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__django_m__3213E83FB10AECAE");

            entity.ToTable("django_migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.App)
                .HasMaxLength(255)
                .HasColumnName("app");
            entity.Property(e => e.Applied).HasColumnName("applied");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DjangoSession>(entity =>
        {
            entity.HasKey(e => e.SessionKey).HasName("PK__django_s__B3BA0F1FA1ED002D");

            entity.ToTable("django_session");

            entity.HasIndex(e => e.ExpireDate, "django_session_expire_date_a5c62663");

            entity.Property(e => e.SessionKey)
                .HasMaxLength(40)
                .HasColumnName("session_key");
            entity.Property(e => e.ExpireDate).HasColumnName("expire_date");
            entity.Property(e => e.SessionData).HasColumnName("session_data");
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

            entity.Property(e => e.Accuracy).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.Framework)
                .HasMaxLength(50)
                .HasDefaultValue("TensorFlow/Keras");
            entity.Property(e => e.InputShape).HasMaxLength(50);
            entity.Property(e => e.Labels).HasMaxLength(200);
            entity.Property(e => e.ModelName).HasMaxLength(100);
            entity.Property(e => e.Version).HasMaxLength(20);

            entity.HasOne(d => d.Tool).WithMany(p => p.PredictionModels)
                .HasForeignKey(d => d.ToolId)
                .HasConstraintName("FK_PredictionModels_Tools");
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
