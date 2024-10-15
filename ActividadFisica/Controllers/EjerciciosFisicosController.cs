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
        lugares.Add(new Lugar { LugarID = 0, Nombre = "SELECCIONE EL LUGAR" });
        ViewBag.LugarID = new SelectList(lugares.OrderBy(l => l.LugarID), "LugarID", "Nombre");

        var eventos = _context.EventosDeportivos.Where(e => e.Eliminado == false).ToList();
        eventos.Add(new EventoDeportivo { EventoDeportivoID = 0, Nombre = "SELECCIONE EL EVENTO" });
        ViewBag.EventoID = new SelectList(eventos.OrderBy(l => l.EventoDeportivoID), "EventoDeportivoID", "Nombre");

        return View();
    }
    public IActionResult InformeEjerciciosFisicos()
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
        ViewBag.EstadoEmocionalInicioBuscar = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.EstadoEmocionalFinBuscar = selectListItems.OrderBy(t => t.Text).ToList();


        return View();
    }

    public IActionResult InformePorLugar()
    {
        return View();
    }

    public JsonResult ListadoEjerciciosPorLugar(DateTime? FechaDesde, DateTime? FechaHasta)
    {
        List<VistaLugar> vistaLugar = new List<VistaLugar>();

        var listadoEjercicios = _context.EjercicioFisico.Include(l => l.Lugar).Include(l => l.TipoEjercicio).OrderBy(l => l.Inicio).ToList();

        if (FechaDesde != null && FechaHasta != null)
        {
            listadoEjercicios = listadoEjercicios.Where(l => l.Inicio >= FechaDesde && l.Inicio <= FechaHasta).ToList();
        }

        foreach (var listadoEjercicio in listadoEjercicios.OrderBy(l => l.Lugar.Nombre))
        {
            var mostrarLugar = vistaLugar.Where(m => m.LugarID == listadoEjercicio.LugarID).SingleOrDefault();

            if (mostrarLugar == null)
            {
                mostrarLugar = new VistaLugar
                {
                    LugarID = listadoEjercicio.LugarID,
                    Nombre = listadoEjercicio.Lugar.Nombre,
                    VistaEjercicios = new List<VistaEjercicioFisico>()
                };
                vistaLugar.Add(mostrarLugar);
            }

            var mostrarEjerciciosPorLugar = new VistaEjercicioFisico
            {
                EjercicioFisicoID = listadoEjercicio.EjercicioFisicoID,
                TipoEjercicioID = listadoEjercicio.TipoEjercicioID,
                TipoEjercicioDescripcion = listadoEjercicio.TipoEjercicio.Descripcion,
                FechaInicioString = listadoEjercicio.Inicio.ToString("dd/MM/yyyy, HH:mm"),
                FechaFinString = listadoEjercicio.Fin.ToString("dd/MM/yyyy, HH:mm"),
                IntervaloEjercicio = listadoEjercicio.IntervaloEjercicio,
                Observaciones = listadoEjercicio.Observaciones
            };
            mostrarLugar.VistaEjercicios.Add(mostrarEjerciciosPorLugar);
        }

        return Json(vistaLugar);
    }

    public JsonResult ListadoEjerciciosFisicos(int? ejercicioFisicosID, DateTime? FechaDesde, DateTime? FechaHasta, int? TipoEjercicioBuscar)
    {
        List<VistaEjercicioFisico> ejerciciosFisicosMostrar = new List<VistaEjercicioFisico>();

        //VARIABLE PARA GUARDAR LA LISTA DE DATOS DE EJERCICIOS FISICOS
        var ejerciciosFisicos = _context.EjercicioFisico.Include(e => e.Lugar).Include(e => e.EventoDeportivo).ToList();

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
                LugarNombre = ejercicioFisico.Lugar.Nombre,
                EventoDeportivoID = ejercicioFisico.EventoDeportivoID,
                EventoDeportivoNombre = ejercicioFisico.EventoDeportivo.Nombre
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


    public JsonResult GuardarEjerciciosFisicos(int ejercicioFisicoID, int tipoEjercicioID, int LugarID, int EventoID, DateTime inicio, DateTime fin, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, string observaciones)
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
                EventoDeportivoID = EventoID,
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
                editarEjercicio.EventoDeportivoID = EventoID;
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