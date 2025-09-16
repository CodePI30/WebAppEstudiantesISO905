using System;
using System.Collections.Generic;

namespace AppEstudiantesISO905.Domain.Models;

public partial class Materia
{
    public int MateriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public int Creditos { get; set; }

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();
}
