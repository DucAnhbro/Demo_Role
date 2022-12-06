using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Demo_Role.Models
{
    public partial class EmployessContext : DbContext
    {
        public EmployessContext()
        {
        }

        public EmployessContext(DbContextOptions<EmployessContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employe> Employes { get; set; } = null!;
        public virtual DbSet<EmployeRole> EmployeRoles { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-0LJAOF9;Database=Employess; Trust Server Certificate=true;User Id=sa;Password=123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employe>(entity =>
            {
                entity.ToTable("Employe");

                entity.Property(e => e.Adress).HasMaxLength(250);

                entity.Property(e => e.BirthDay).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.IsSystem).HasColumnName("isSystem");

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.UserName).HasMaxLength(250);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Employes)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employe_Group");
            });

            modelBuilder.Entity<EmployeRole>(entity =>
            {
                entity.ToTable("EmployeRole");

                entity.HasOne(d => d.Employe)
                    .WithMany(p => p.EmployeRoles)
                    .HasForeignKey(d => d.EmployeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeRole_Employe");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.EmployeRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeRole_Role");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.GroupName).HasMaxLength(250);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Action).HasMaxLength(150);

                entity.Property(e => e.Controller).HasMaxLength(150);

                entity.Property(e => e.RoleName).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
