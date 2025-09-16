using System;
using System.Collections.Generic;
using AppEstudiantesISO905.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppEstudiantesISO905.Repository.WebAppDbContext;

public partial class EstudiantesIso905Context : DbContext
{
    public EstudiantesIso905Context(DbContextOptions<EstudiantesIso905Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Calificacion> Calificacions { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Materia> Materia { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Califica__3214EC276D7DDD19");

            entity.ToTable("Calificacion");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Calificacion1).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Calificacion2).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Calificacion3).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Calificacion4).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Clasificacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComputedColumnSql("(case when ((0.7)*(((([Calificacion1]+[Calificacion2])+[Calificacion3])+[Calificacion4])/(4.0))+(0.3)*[Examen])>=(90) then 'A' when ((0.7)*(((([Calificacion1]+[Calificacion2])+[Calificacion3])+[Calificacion4])/(4.0))+(0.3)*[Examen])>=(80) then 'B' when ((0.7)*(((([Calificacion1]+[Calificacion2])+[Calificacion3])+[Calificacion4])/(4.0))+(0.3)*[Examen])>=(70) then 'C' else 'F' end)", true);
            entity.Property(e => e.Estado)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasComputedColumnSql("(case when ((0.7)*(((([Calificacion1]+[Calificacion2])+[Calificacion3])+[Calificacion4])/(4.0))+(0.3)*[Examen])>=(70) then 'Aprobado' else 'Reprobado' end)", true);
            entity.Property(e => e.EstudianteId).HasColumnName("EstudianteID");
            entity.Property(e => e.Examen).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MateriaId).HasColumnName("MateriaID");
            entity.Property(e => e.PromedioCalificaciones)
                .HasComputedColumnSql("(((([Calificacion1]+[Calificacion2])+[Calificacion3])+[Calificacion4])/(4.0))", false)
                .HasColumnType("numeric(13, 6)");
            entity.Property(e => e.TotalCalificacion)
                .HasComputedColumnSql("((0.7)*(((([Calificacion1]+[Calificacion2])+[Calificacion3])+[Calificacion4])/(4.0))+(0.3)*[Examen])", true)
                .HasColumnType("numeric(16, 7)");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.EstudianteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calificacion_Estudiante");

            entity.HasOne(d => d.Materia).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.MateriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calificacion_Materia");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.EstudianteId).HasName("PK__Estudian__6F768338E6357F63");

            entity.ToTable("Estudiante");

            entity.HasIndex(e => e.Matricula, "UQ__Estudian__0FB9FB4F8D479814").IsUnique();

            entity.Property(e => e.EstudianteId).HasColumnName("EstudianteID");
            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Matricula).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.MateriaId).HasName("PK__Materia__0D019D818C35CCC3");

            entity.Property(e => e.MateriaId).HasColumnName("MateriaID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798E349B96D");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534AD9925DD").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Estado).HasMaxLength(10);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
