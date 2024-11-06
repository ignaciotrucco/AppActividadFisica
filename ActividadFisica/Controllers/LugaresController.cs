using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ActividadFisica.Models;
using ActividadFisica.Data;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ActividadFisica.Controllers;

[Authorize]
public class LugaresController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    // CONSTRUCTOR
    public LugaresController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    
    public IActionResult Lugares()
    {
        return View();
    }

    public JsonResult ListadoLugares(int? LugarID)
    {
        var usuarioLogueado = _userManager.GetUserId(HttpContext.User);

        var listadoLugares = _context.Lugares.Where(l => l.UsuarioID == usuarioLogueado).ToList();

        if (LugarID != null) {
            listadoLugares = _context.Lugares.Where(l => l.LugarID == LugarID).ToList();
        }

        return Json(listadoLugares);
    }

    public JsonResult GuardarLugar(int LugarID, string Nombre)
    {
        var usuarioLogueado = _userManager.GetUserId(HttpContext.User);
        string resultado = "";
        if (!String.IsNullOrEmpty(Nombre))
        {
            Nombre = Nombre.ToUpper();

            if (LugarID == 0)
            {
                var existeLugar = _context.Lugares.Where(e => e.Nombre == Nombre).Count();
                if (existeLugar == 0)
                {
                    var nuevoLugar = new Lugar
                    {
                        Nombre = Nombre,
                        UsuarioID = usuarioLogueado
                    };
                    _context.Add(nuevoLugar);
                    _context.SaveChanges();
                    resultado = "Lugar agregado";
                }
                else
                {
                    resultado = "Lugar existente";
                }
            }
            else
            {
                var editarLugar = _context.Lugares.Where(e => e.LugarID == LugarID).SingleOrDefault();
                if (editarLugar != null)
                {
                    var existeLugar = _context.Lugares.Where(e => e.LugarID != LugarID && e.Nombre == Nombre).Count();
                    if (existeLugar == 0)
                    {
                        editarLugar.Nombre = Nombre;
                        _context.SaveChanges();
                        resultado = "Lugar editado";
                    }
                    else
                    {
                        resultado = "Lugar existente";
                    }
                }
            }
        }
        else
        {
            resultado = "Debe ingresar un nombre";
        }

        return Json(resultado);
    }

    public JsonResult EliminarLugar(int LugarID) {
        bool eliminado = false;
        var existeEjercicio = _context.EjercicioFisico.Where(e => e.LugarID == LugarID).Count();

        if (existeEjercicio == 0) {
            var eliminarLugar = _context.Lugares.Find(LugarID);
            _context.Remove(eliminarLugar);
            _context.SaveChanges();
            eliminado = true;
        }
        return Json(eliminado);
    }

}

