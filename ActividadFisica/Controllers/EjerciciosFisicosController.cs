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
        tipoEjercicios.Add(new Tipo_Ejercicio { TipoEjercicioID = 0, Descripcion = "[SELECCIONE...]" });
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        return View();
    }

    public JsonResult ListadoEjerciciosFisicos(int? ejercicioFisicosID)
    {
        List<VistaEjercicioFisico> ejerciciosFisicosMostrar = new List<VistaEjercicioFisico>();

        //VARIABLE PARA GUARDAR LA LISTA DE DATOS DE EJERCICIOS FISICOS
        var ejerciciosFisicos = _context.EjercicioFisico.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (ejercicioFisicosID != null)
        {
            //FILTRAMOS CADA EJERCICIO POR ID PARA QUE COINCIDAN
            ejerciciosFisicos = ejerciciosFisicos.Where(l => l.EjercicioFisicoID == ejercicioFisicosID).ToList();
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
                Observaciones = ejercicioFisico.Observaciones
            };
            ejerciciosFisicosMostrar.Add(ejercicioFisicoMostrar);
        }

        return Json(ejerciciosFisicosMostrar);
    }

    public JsonResult LlamarDatosAlModal(int? ejercicioFisicoID) {
        var ejercicioFisico = _context.EjercicioFisico.ToList();

        if (ejercicioFisicoID != null)
        {
            //FILTRAMOS CADA EJERCICIO POR ID PARA QUE COINCIDAN
            ejercicioFisico = ejercicioFisico.Where(e => e.EjercicioFisicoID == ejercicioFisicoID).ToList();
        }
        return Json(ejercicioFisico.ToList());
    }


    public JsonResult GuardarEjerciciosFisicos(int ejercicioFisicoID, int tipoEjercicioID, DateTime inicio, DateTime fin, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, string observaciones)
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
}