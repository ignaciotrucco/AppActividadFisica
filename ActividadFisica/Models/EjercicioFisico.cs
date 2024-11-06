using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadFisica.Models
{
    public class EjercicioFisico
    {
        [Key]
        public int EjercicioFisicoID { get; set; }
        public int TipoEjercicioID { get; set; }
        public int LugarID { get; set; }
        public int EventoDeportivoID { get; set; }
        public string? UsuarioID {get; set;}
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }

        [NotMapped]
        public TimeSpan IntervaloEjercicio { get { return Fin - Inicio; } }
        public EstadoEmocional EstadoEmocionalInicio { get; set; }
        public EstadoEmocional EstadoEmocionalFin { get; set; }
        public string? Observaciones { get; set; }
        public virtual Tipo_Ejercicio TipoEjercicio { get; set; }
        public virtual Lugar Lugar { get; set; }
        public virtual EventoDeportivo EventoDeportivo { get; set; }
    }

    public enum EstadoEmocional
    {
        Feliz = 1,
        Triste,
        Enojado,
        Ansioso,
        Estresado,
        Relajado,
        Aburrido,
        Emocionado,
        Agobiado,
        Confundido,
        Optimista,
        Pesimista,
        Motivado,
        Cansado,
        Eufórico,
        Agitado,
        Satisfecho,
        Desanimado
    }

    public class VistaNombreEjercicio
    {
        public int TipoEjercicioID { get; set; }
        public string? TipoEjercicioDescripcion { get; set; }
        public int EjercicioFisicoID { get; set; }
        public List<VistaEjercicioFisico>? VistaEjercicioFisico { get; set; }
    }

    public class VistaEjercicioFisico
    {
        public int EjercicioFisicoID { get; set; }
        public int TipoEjercicioID { get; set; }
        public int LugarID { get; set; }
        public int EventoDeportivoID { get; set; }
        public string? LugarNombre { get; set; }
        public string? EventoDeportivoNombre { get; set; }
        public string? TipoEjercicioDescripcion { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string FechaInicioString { get; set; }
        public string FechaFinString { get; set; }
        public string? EstadoEmocionalInicio { get; set; }
        public string? EstadoEmocionalFin { get; set; }
        public string? Observaciones { get; set; }
        public TimeSpan IntervaloEjercicio { get; set; }
    }

    public class VistaSumaEjercicioFisico
    {
        public string? TipoEjercicioNombre { get; set; }
        public int TotalidadMinutos { get; set; }
        public int TotalidadDiasConEjercicio { get; set; }
        public int TotalidadDiasSinEjercicio { get; set; }

        public List<VistaEjercicioFisico>? DiasEjercicios { get; set; }
    }

    // public class VistaEjercicioFisico
    // {
    //     public int Anio { get; set; }
    //     public string? Mes { get; set; }
    //     public int? Dia { get; set; }
    //     public int CantidadMinutos { get; set; }
    // }
}

