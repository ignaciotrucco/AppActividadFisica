using Microsoft.AspNetCore.Mvc;
using ActividadFisica.Models;
using ActividadFisica.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ActividadFisica.Controllers;

[Authorize]

public class InformeGeneralController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    // CONSTRUCTOR
    public InformeGeneralController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult InformeGeneral()
    {
        return View();
    }

    public JsonResult ListadoInformeGeneral()
    {
        var usuarioLogueado = _userManager.GetUserId(HttpContext.User);
        List<EventoVista> EventoVista = new List<EventoVista>();

        var ejercicios = _context.EjercicioFisico.Where(e => e.UsuarioID == usuarioLogueado).Include(e => e.TipoEjercicio).Include(e => e.Lugar).Include(e => e.EventoDeportivo).ToList();

        foreach (var ejercicio in ejercicios)
        {

            var deportista = _context.Personas.Where(d => d.UsuarioID == usuarioLogueado).Single();

            decimal caloriasQuemadas = 0;
            if (deportista.Genero == Genero.Masculino)
            {
                caloriasQuemadas = ejercicio.TipoEjercicio.NroMET * deportista.Peso * (decimal)ejercicio.IntervaloEjercicio.TotalHours;
            }
            else
            {
                caloriasQuemadas = ejercicio.TipoEjercicio.NroMET * deportista.Peso * (decimal)ejercicio.IntervaloEjercicio.TotalHours * 0.9m;
            }

            var eventoMostrar = EventoVista.FirstOrDefault(e => e.EventoDeportivoID == ejercicio.EventoDeportivoID);

            if (eventoMostrar == null)
            {
                eventoMostrar = new EventoVista
                {
                    EventoDeportivoID = ejercicio.EventoDeportivoID,
                    Nombre = ejercicio.EventoDeportivo.Nombre,
                    VistaLugar = new List<LugarVista>()
                };
                EventoVista.Add(eventoMostrar);
            }

            var lugarMostrar = eventoMostrar.VistaLugar.FirstOrDefault(l => l.LugarID == ejercicio.LugarID);

            if (lugarMostrar == null)
            {
                lugarMostrar = new LugarVista
                {
                    LugarID = ejercicio.LugarID,
                    Nombre = ejercicio.Lugar.Nombre,
                    VistaTipoEjercicio = new List<VistaTipoEjercicio>()
                };
                eventoMostrar.VistaLugar.Add(lugarMostrar);
            }

            var tipoEjercicioMostrar = lugarMostrar.VistaTipoEjercicio.FirstOrDefault(t => t.TipoEjercicioID == ejercicio.TipoEjercicioID);

            if (tipoEjercicioMostrar == null)
            {
                tipoEjercicioMostrar = new VistaTipoEjercicio
                {
                    TipoEjercicioID = ejercicio.TipoEjercicioID,
                    Descripcion = ejercicio.TipoEjercicio.Descripcion,
                    VistaEjercicioFisico = new List<VistaEjercicioFisico>()
                };
                lugarMostrar.VistaTipoEjercicio.Add(tipoEjercicioMostrar);
            }

            var ejerciciosFisicos = new VistaEjercicioFisico
            {
                EjercicioFisicoID = ejercicio.EjercicioFisicoID,
                FechaInicioString = ejercicio.Inicio.ToString("dd/MM/yyyy, HH:mm"),
                FechaFinString = ejercicio.Fin.ToString("dd/MM/yyyy, HH:mm"),
                EstadoEmocionalInicio = Enum.GetName(typeof(EstadoEmocional), ejercicio.EstadoEmocionalInicio),
                EstadoEmocionalFin = Enum.GetName(typeof(EstadoEmocional), ejercicio.EstadoEmocionalFin),
                Observaciones = ejercicio.Observaciones,
                IntervaloEjercicio = ejercicio.IntervaloEjercicio,
                CaloriasQuemadas = caloriasQuemadas
            };
            tipoEjercicioMostrar.VistaEjercicioFisico.Add(ejerciciosFisicos);
        }

        return Json(EventoVista);
    }
}