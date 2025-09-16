using System;
using System.Collections.Generic;

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

    public string Clasificacion { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual Estudiante Estudiante { get; set; } = null!;

    public virtual Materia Materia { get; set; } = null!;
}
