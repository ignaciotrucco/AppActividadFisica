using ActividadFisica.Data;
using ActividadFisica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ActividadFisica.Controllers;

[Authorize]
public class PanelEjerciciosController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    // CONSTRUCTOR
    public PanelEjerciciosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public IActionResult PanelEjercicios()
    {
        var tipoEjercicios = _context.Tipo_Ejercicios.ToList();
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        return View();
    }

    public JsonResult GraficoBarraEjercicios(int TipoEjercicioId, int Mes, int Anio)
    {
        var usuarioLogueado = _userManager.GetUserId(HttpContext.User);

        List<EjercicioPorDia> ejerciciosPorDia = new List<EjercicioPorDia>();

        //VARIABLE QUE AGREGA AL LISTADO TODOS LOS DIAS DEL MES
        var diasDelMes = DateTime.DaysInMonth(Anio, Mes);

        //INICIALIZO UNA VARIABLE DE TIPO FECHA
        DateTime fechaDelMes = new DateTime();

        //RESTAMOS UN MES SOBRE ESA FECHA
        fechaDelMes = fechaDelMes.AddMonths(Mes - 1);

        for (int i = 1; i <= diasDelMes; i++)
        {
            var mostrarDiaMes = new EjercicioPorDia
            {
                Dia = i,
                Mes = fechaDelMes.ToString("MMM").ToUpper(),
                CantidadMinutos = 0
            };
            ejerciciosPorDia.Add(mostrarDiaMes);
        }

        //BUSCAR EN BASE DE DATOS EJERCICIOS CON LOS PARAMETROS PROPUESTOS
        var ejercicios = _context.EjercicioFisico.Where(e => e.TipoEjercicioID == TipoEjercicioId
        && e.Inicio.Month == Mes && e.Inicio.Year == Anio && e.UsuarioID == usuarioLogueado).ToList();

        foreach (var ejercicio in ejercicios.OrderBy(e => e.Inicio))
        {
            //POR CADA EJERCICIO DEBEMOS AGREGAR EN EL LISTADO SI EL DIA DE ESE EJERCICIO NO EXISTE, SINO SUMARLO
            var diaEjercicioMostrar = ejerciciosPorDia.Where(e => e.Dia == ejercicio.Inicio.Day).SingleOrDefault();
            if (diaEjercicioMostrar != null)
            {
                diaEjercicioMostrar.CantidadMinutos += Convert.ToInt32(ejercicio.IntervaloEjercicio.TotalMinutes);
            }
        }
        return Json(ejerciciosPorDia);
    }

    public JsonResult GraficoCircular(int Mes, int Anio) 
    {
        var usuarioLogueado = _userManager.GetUserId(HttpContext.User);
        //Nuevo listado de tipo de ejercicios
        var vistaTipoEjercicios = new List<VistaTipoEjercicioFisico>();

        //Buscamos tipos de ejercicios activos
        var tipoEjerciciosActivos = _context.Tipo_Ejercicios.Where(t => t.Eliminado == false).ToList();

        //Los recorremos
        foreach (var tipoEjercicioActivo in tipoEjerciciosActivos)
        {
            //POR CADA TIPO DE EJERCICIO BUSQUEMOS EN LA TABLA DE EJERCICIOS FISICOS POR ESE TIPO, EN EL MES Y AÑO SOLICITADO
            var ejercicios = _context.EjercicioFisico.Where(e => e.TipoEjercicioID == tipoEjercicioActivo.TipoEjercicioID 
            && e.Inicio.Month == Mes && e.Inicio.Year == Anio && e.UsuarioID == usuarioLogueado).ToList();

            foreach (var ejercicio in ejercicios)
            {
                var mostrarTipoEjercicio = vistaTipoEjercicios.Where(m => m.TipoEjercicioID == tipoEjercicioActivo.TipoEjercicioID).SingleOrDefault();
                if(mostrarTipoEjercicio == null)
                {
                    mostrarTipoEjercicio = new VistaTipoEjercicioFisico
                    {
                      TipoEjercicioID = tipoEjercicioActivo.TipoEjercicioID,
                      Descripcion = tipoEjercicioActivo.Descripcion,
                      CantidadMinutos = Convert.ToDecimal(ejercicio.IntervaloEjercicio.TotalMinutes)  
                    };
                    vistaTipoEjercicios.Add(mostrarTipoEjercicio);
                }
                else
                {
                    mostrarTipoEjercicio.CantidadMinutos += Convert.ToDecimal(ejercicio.IntervaloEjercicio.TotalMinutes);
                };
            };
        };


        return Json(vistaTipoEjercicios);
    }

}