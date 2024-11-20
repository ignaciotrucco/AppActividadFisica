using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ActividadFisica.Models;

public class Persona
{
    [Key]
    public int PersonaID { get; set; }
    public string? UsuarioID { get; set; }
    public string? NombreCompleto { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public decimal Peso { get; set; }
    public decimal Altura { get; set; }
    public Genero Genero { get; set; }
}

public enum Genero
{
    Masculino = 1,
    Femenino,
    NoBinario,
    Otro
}

public class VistaPersonas
{
    public int PersonaID { get; set; }
    public string? UsuarioID { get; set; }
    public string? NombreCompleto { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string? FechaNacimientoString { get; set; }
    public decimal Peso { get; set; }
    public decimal Altura { get; set; }
    public Genero Genero { get; set; }
    public string? Email { get; set; }
    public string? GeneroString { get; set; }
}