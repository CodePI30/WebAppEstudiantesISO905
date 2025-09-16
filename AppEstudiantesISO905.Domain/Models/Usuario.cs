using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppEstudiantesISO905.Domain.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;

    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public string Estado { get; set; } = null!;
}
