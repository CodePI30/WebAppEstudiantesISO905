using System;
using System.Collections.Generic;

namespace AppEstudiantesISO905.Domain.Models;

public partial class Estudiante
{
    public int EstudianteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Matricula { get; set; } = null!;

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();
}

public partial class EstudianteCreateModel
{
    public int EstudianteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string FechaNacimiento { get; set; } = null!;

    public string Matricula { get; set; } = null!;
}


