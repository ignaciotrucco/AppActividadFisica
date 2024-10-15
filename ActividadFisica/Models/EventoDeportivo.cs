using System.ComponentModel.DataAnnotations;
using ActividadFisica.Models;

public class EventoDeportivo
{
    [Key]
    public int EventoDeportivoID { get; set; }
    public string? Nombre { get; set; }
    public bool Eliminado { get; set; }
    public virtual ICollection<EjercicioFisico> EjercicioFisico { get; set; }
}