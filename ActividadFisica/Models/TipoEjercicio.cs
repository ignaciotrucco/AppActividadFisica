using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ActividadFisica.Models;

public class Tipo_Ejercicio 
{
    [Key]
    public int TipoEjercicioID {get; set;}
    public string? Descripcion {get; set;}
    public bool Eliminado {get; set;}
    public virtual ICollection<EjercicioFisico> EjercicioFisico {get; set;}
}