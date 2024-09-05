using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ActividadFisica.Models;

public class Lugar 
{
    [Key]
    public int LugarID {get; set;}
    public string? Nombre {get; set;}
    public virtual ICollection<EjercicioFisico> EjercicioFisico {get; set;}
}

public class VistaLugar {
    public int LugarID {get; set;}
    public string? Nombre {get; set;}
    public List<VistaEjercicioFisico> VistaEjercicios {get; set;}
}