using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ActividadFisica.Models;
using ActividadFisica.Data;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace ActividadFisica.Controllers;

[Authorize(Roles = "ADMINISTRADOR")]
public class EventosDeportivosController : Controller
{
    private ApplicationDbContext _context;

    // CONSTRUCTOR
    public EventosDeportivosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult EventosDeportivos()
    {
        return View();
    }

    public JsonResult ListadoEventos(int? EventoID)
    {

        var listadoEventos = _context.EventosDeportivos.ToList();

        if (EventoID != null)
        {
            listadoEventos = listadoEventos.Where(l => l.EventoDeportivoID == EventoID).ToList();
        }

        return Json(listadoEventos);
    }

    public JsonResult GuardarEvento(int EventoID, string EventoNombre)
    {
        EventoNombre = EventoNombre.ToUpper();
        string resultado = "";

        if (EventoID == 0)
        {
            var nuevoEvento = new EventoDeportivo
            {
                Nombre = EventoNombre,
                Eliminado = false
            };
            _context.EventosDeportivos.Add(nuevoEvento);
            _context.SaveChanges();
            resultado = "Evento guardado correctamente";
        }
        else
        {
            var editarEvento = _context.EventosDeportivos.Where(e => e.EventoDeportivoID == EventoID).SingleOrDefault();

            if (editarEvento != null)
            {
                var existeEvento = _context.EventosDeportivos.Where(t => t.EventoDeportivoID == EventoID && t.Nombre == EventoNombre).Count();
                if (existeEvento == 0)
                {
                    editarEvento.Nombre = EventoNombre;
                    _context.SaveChanges();
                    resultado = "Evento editado correctamente";
                }
                else
                {
                    resultado = "Evento existente";
                }
            }
        }


        return Json(resultado);
    }

    public JsonResult DeshabilitarEvento(int EventoID)
    {
        bool Eliminado = false;
        
        var existeEjercicio = _context.EjercicioFisico.Where(e => e.EventoDeportivoID == EventoID).Count();

        if (existeEjercicio == 0) {
            var deshabilitarEvento = _context.EventosDeportivos.Find(EventoID);
            deshabilitarEvento.Eliminado = true;
            _context.SaveChanges();
            Eliminado = true;
        }

        return Json(Eliminado);
    }

    public JsonResult HabilitarEvento(int EventoID)
    {
        var habilitarEvento = _context.EventosDeportivos.Find(EventoID);
        habilitarEvento.Eliminado = false;
        _context.SaveChanges();

        return Json(habilitarEvento);
    }

}

