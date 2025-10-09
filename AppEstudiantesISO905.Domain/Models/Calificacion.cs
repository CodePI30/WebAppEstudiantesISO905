using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppEstudiantesISO905.Domain.Models;

public partial class Calificacion
{
    public int Id { get; set; }

    public int EstudianteId { get; set; }

    public int MateriaId { get; set; }

    public decimal Calificacion1 { get; set; }

    public decimal Calificacion2 { get; set; }

    public decimal Calificacion3 { get; set; }

    public decimal Calificacion4 { get; set; }

    public decimal Examen { get; set; }

    public decimal? PromedioCalificaciones { get; set; }

    public decimal? TotalCalificacion { get; set; }

    public string? Clasificacion { get; set; } = null!;

    public string? Estado { get; set; } = null!;

    public virtual Estudiante Estudiante { get; set; } = null!;

    public virtual Materia Materia { get; set; } = null!;
}

public class CalificacionCreateVM
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "El estudiante es obligatorio.")]
    public int EstudianteId { get; set; }

    [Required(ErrorMessage = "La materia es obligatoria.")]
    public int MateriaId { get; set; }

    [Required]
    [Range(60, 100, ErrorMessage = "La calificación debe estar entre 60 y 100.")]
    public decimal Calificacion1 { get; set; }

    [Required]
    [Range(60, 100, ErrorMessage = "La calificación debe estar entre 60 y 100.")]
    public decimal Calificacion2 { get; set; }

    [Required]
    [Range(60, 100, ErrorMessage = "La calificación debe estar entre 60 y 100.")]
    public decimal Calificacion3 { get; set; }

    [Required]
    [Range(60, 100, ErrorMessage = "La calificación debe estar entre 60 y 100.")]
    public decimal Calificacion4 { get; set; }

    [Required]
    [Range(60, 100, ErrorMessage = "El examen debe estar entre 60 y 100.")]
    public decimal Examen { get; set; }
}
