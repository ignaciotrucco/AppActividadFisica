namespace ActividadFisica.Models;

public class EjercicioPorDia
    {
        public string? Mes { get; set; }
        public int Dia { get; set; }
        public int CantidadMinutos { get; set; }
    }

    public class VistaTipoEjercicioFisico
{
     public int TipoEjercicioID { get; set; }
     public string? Descripcion { get; set; } 

     public decimal CantidadMinutos { get; set; }

}