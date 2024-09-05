using Microsoft.AspNetCore.Mvc;
using ActividadFisica.Models;
using ActividadFisica.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ActividadFisica.Controllers;

[Authorize]

public class EjerciciosFisicosController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public EjerciciosFisicosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult EjerciciosFisicos()
    {

        // Crear una lista de SelectListItem que incluya el elemento adicional
        var selectListItems = new List<SelectListItem>
         {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]"}
         };

        // Obtener todas las opciones del enum
        var enumValues = Enum.GetValues(typeof(EstadoEmocional)).Cast<EstadoEmocional>();

        // Convertir las opciones del enum en SelectListItem
        selectListItems.AddRange(enumValues.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));

        // Pasar la lista de opciones al modelo de la vista
        ViewBag.EstadoEmocionalInicio = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.EstadoEmocionalFin = selectListItems.OrderBy(t => t.Text).ToList();

        var tipoEjercicios = _context.Tipo_Ejercicios.ToList();
        var tipoEjercicioBuscar = _context.Tipo_Ejercicios.ToList();
        tipoEjercicios.Add(new Tipo_Ejercicio { TipoEjercicioID = 0, Descripcion = "[SELECCIONE...]" });
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        tipoEjercicioBuscar.Add(new Tipo_Ejercicio { TipoEjercicioID = 0, Descripcion = "[Tipos de ejercicios]" });
        ViewBag.TipoEjercicioBuscarID = new SelectList(tipoEjercicioBuscar.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        var lugares = _context.Lugares.ToList();
        lugares.Add(new Lugar { LugarID = 0, Nombre = "SELECCIONE EL LUGAR"});
        ViewBag.LugarID = new SelectList(lugares.OrderBy(l => l.LugarID), "LugarID", "Nombre");

        return View();
    }
    public IActionResult InformeEjerciciosFisicos()
    {
        return View();
    }

    public JsonResult ListadoEjerciciosFisicos(int? ejercicioFisicosID, DateTime? FechaDesde, DateTime? FechaHasta, int? TipoEjercicioBuscar)
    {
        List<VistaEjercicioFisico> ejerciciosFisicosMostrar = new List<VistaEjercicioFisico>();

        //VARIABLE PARA GUARDAR LA LISTA DE DATOS DE EJERCICIOS FISICOS
        var ejerciciosFisicos = _context.EjercicioFisico.Include(e => e.Lugar).ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (ejercicioFisicosID != null)
        {
            //FILTRAMOS CADA EJERCICIO POR ID PARA QUE COINCIDAN
            ejerciciosFisicos = ejerciciosFisicos.Where(l => l.EjercicioFisicoID == ejercicioFisicosID).ToList();
        }

        //CONDICION PARA QUE FILTRE POR FECHA
        if (FechaDesde != null && FechaHasta != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.Inicio >= FechaDesde && e.Inicio <= FechaHasta).ToList();
        }

        //CONDICION PARA QUE FILTRE POR TIPO DE EJERCICIO
        if (TipoEjercicioBuscar != 0)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.TipoEjercicioID == TipoEjercicioBuscar).ToList();
        }

        var tiposEjercicios = _context.Tipo_Ejercicios.ToList();

        foreach (var ejercicioFisico in ejerciciosFisicos)
        {
            var tipoEjercicio = tiposEjercicios.Where(t => t.TipoEjercicioID == ejercicioFisico.TipoEjercicioID).Single();

            var ejercicioFisicoMostrar = new VistaEjercicioFisico
            {
                EjercicioFisicoID = ejercicioFisico.EjercicioFisicoID,
                TipoEjercicioID = ejercicioFisico.TipoEjercicioID,
                TipoEjercicioDescripcion = tipoEjercicio.Descripcion,
                FechaInicioString = ejercicioFisico.Inicio.ToString("dd/MM/yyyy, HH:mm"),
                FechaFinString = ejercicioFisico.Fin.ToString("dd/MM/yyyy, HH:mm"),
                EstadoEmocionalInicio = Enum.GetName(typeof(EstadoEmocional), ejercicioFisico.EstadoEmocionalInicio),
                EstadoEmocionalFin = Enum.GetName(typeof(EstadoEmocional), ejercicioFisico.EstadoEmocionalFin),
                Observaciones = ejercicioFisico.Observaciones,
                LugarID = ejercicioFisico.LugarID,
                LugarNombre = ejercicioFisico.Lugar.Nombre
            };
            ejerciciosFisicosMostrar.Add(ejercicioFisicoMostrar);
        }

        return Json(ejerciciosFisicosMostrar);
    }

    public JsonResult LlamarDatosAlModal(int? ejercicioFisicoID)
    {
        var ejercicioFisico = _context.EjercicioFisico.ToList();

        if (ejercicioFisicoID != null)
        {
            //FILTRAMOS CADA EJERCICIO POR ID PARA QUE COINCIDAN
            ejercicioFisico = ejercicioFisico.Where(e => e.EjercicioFisicoID == ejercicioFisicoID).ToList();
        }
        return Json(ejercicioFisico.ToList());
    }


    public JsonResult GuardarEjerciciosFisicos(int ejercicioFisicoID, int tipoEjercicioID, int LugarID, DateTime inicio, DateTime fin, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, string observaciones)
    {

        string resultado = "";
        observaciones = observaciones.ToUpper();

        //VERIFICA SI CREA O EDITA
        //CREA
        if (ejercicioFisicoID == 0)
        {
            var nuevoEjercicio = new EjercicioFisico
            {
                EjercicioFisicoID = ejercicioFisicoID,
                TipoEjercicioID = tipoEjercicioID,
                LugarID = LugarID,
                Inicio = inicio,
                Fin = fin,
                EstadoEmocionalInicio = estadoEmocionalInicio,
                EstadoEmocionalFin = estadoEmocionalFin,
                Observaciones = observaciones
            };

            _context.Add(nuevoEjercicio);
            _context.SaveChanges();
            resultado = "El elemento ha sido creado correctamente";
        }



        //EDITA
        else
        {
            var editarEjercicio = _context.EjercicioFisico.Where(e => e.EjercicioFisicoID == ejercicioFisicoID).SingleOrDefault();
            if (editarEjercicio != null)
            {
                editarEjercicio.TipoEjercicioID = tipoEjercicioID;
                editarEjercicio.LugarID = LugarID;
                editarEjercicio.Inicio = inicio;
                editarEjercicio.Fin = fin;
                editarEjercicio.EstadoEmocionalInicio = estadoEmocionalInicio;
                editarEjercicio.EstadoEmocionalFin = estadoEmocionalFin;
                editarEjercicio.Observaciones = observaciones;
                _context.SaveChanges();
                resultado = "El elemento ha sido modificado correctamente";
            }
            else
            {
                resultado = "No se pudo editar el elemento";
            }
        }


        return Json(resultado);
    }

    public JsonResult EliminarEjercicio(int ejercicioFisicoID)
    {
        var eliminarEjercicio = _context.EjercicioFisico.Find(ejercicioFisicoID);
        _context.Remove(eliminarEjercicio);
        _context.SaveChanges();

        return Json(true);
    }

    public JsonResult ListadoInformeEjerciciosFisicos(DateTime? FechaDesde, DateTime? FechaHasta)
    {
         List<VistaNombreEjercicio> informeEjerciciosFisicosMostrar = new List<VistaNombreEjercicio>();

         var listadoInformeEjerciciosFisicos = _context.EjercicioFisico
         .Include(l => l.TipoEjercicio).Include(l => l.Lugar)
         .OrderBy(l => l.Inicio).OrderBy(l => l.TipoEjercicio.Descripcion)
         .ToList();

         if (FechaDesde != null && FechaHasta != null)
        {
            listadoInformeEjerciciosFisicos = listadoInformeEjerciciosFisicos.Where(e => e.Inicio >= FechaDesde && e.Inicio <= FechaHasta).ToList();
        }

         foreach (var listadoInforme in listadoInformeEjerciciosFisicos)
         {
            var tipoEjercicioMostrar = informeEjerciciosFisicosMostrar.Where(t => t.TipoEjercicioID == listadoInforme.TipoEjercicioID).SingleOrDefault();
            if (tipoEjercicioMostrar == null)
            {
                tipoEjercicioMostrar = new VistaNombreEjercicio 
                {
                    TipoEjercicioID = listadoInforme.TipoEjercicioID,
                    TipoEjercicioDescripcion = listadoInforme.TipoEjercicio.Descripcion,
                    VistaEjercicioFisico = new List<VistaEjercicioFisico>()
                };
                informeEjerciciosFisicosMostrar.Add(tipoEjercicioMostrar);
            }

            var vistaEjerciciosFisicos = new VistaEjercicioFisico
            {
                EjercicioFisicoID = listadoInforme.EjercicioFisicoID,
                TipoEjercicioID = listadoInforme.TipoEjercicioID,
                TipoEjercicioDescripcion = listadoInforme.TipoEjercicio.Descripcion,
                FechaInicioString = listadoInforme.Inicio.ToString("dd/MM/yyyy, HH:mm"),
                FechaFinString = listadoInforme.Fin.ToString("dd/MM/yyyy, HH:mm"),
                EstadoEmocionalInicio = Enum.GetName(typeof(EstadoEmocional), listadoInforme.EstadoEmocionalInicio),
                EstadoEmocionalFin = Enum.GetName(typeof(EstadoEmocional), listadoInforme.EstadoEmocionalFin),
                Observaciones = listadoInforme.Observaciones,
                IntervaloEjercicio = listadoInforme.IntervaloEjercicio,
                LugarID = listadoInforme.LugarID,
                LugarNombre = listadoInforme.Lugar.Nombre
            };
            tipoEjercicioMostrar.VistaEjercicioFisico.Add(vistaEjerciciosFisicos);
         };

        return Json(informeEjerciciosFisicosMostrar);
    }
}

// public JsonResult TraerpersonaEjercicios(int? id, int? TipoEjercicioaBuscarID, string NombreEjercicio)
//          {
//             List<ListaTipoEjercicio> tiposEjercicioMostrar = new List<ListaTipoEjercicio>();

   

//             var personas = _context.Personas.Include(t => t.TipoEjercicio).ToList();
            

//             if (NombreEjercicio != null)
//             {
//                 personas = personas.Where(t => t.TipoEjercicio.Nombre == NombreEjercicio).ToList();
//             }

//             foreach (var persona in personas)
//             {
//                 var tipoEjercicioMostrar = tiposEjercicioMostrar.SingleOrDefault(t => t.TipoEjercicioID == persona.TipoEjercicioID);
//                 if (tipoEjercicioMostrar == null)
//                 {
//                     tipoEjercicioMostrar =  new ListaTipoEjercicio
//                     {
//                         PersonaID = persona.TipoEjercicioID,
//                         TipoEjercicioID = persona.TipoEjercicioID,
//                         NombreTipoEjercicio = persona.TipoEjercicio.Nombre,
//                         ListadoEjercicios = new List<VistaPersonasEjercicios>()
//                     };
//                     tiposEjercicioMostrar.Add(tipoEjercicioMostrar);


//                 }
//                     var VistaPersonasEjercicios = new VistaPersonasEjercicios
//                     {    
//                          = persona.PersonaID,
//                         NombrePersona = persona.Nombre,
//                         ApellidoPersona = persona.Apellido,
                        
//                     };
//                     tipoEjercicioMostrar.ListadoEjercicios.Add(VistaPersonasEjercicios);

//             }

//              return Json(tiposEjercicioMostrar);
// }  