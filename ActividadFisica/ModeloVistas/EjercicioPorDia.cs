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

public class EventoVista
{
    public int EventoDeportivoID { get; set; }
    public string? Nombre { get; set; }
    public List<LugarVista> VistaLugar {get; set;}
}

public class LugarVista
{
    public int LugarID { get; set; }
    public string? Nombre { get; set; }
    public List<VistaTipoEjercicio> VistaTipoEjercicio {get; set;}
}

public class VistaTipoEjercicio
{
    public int TipoEjercicioID {get; set;}
    public string? Descripcion {get; set;}
    public List<VistaEjercicioFisico> VistaEjercicioFisico {get; set;}
}