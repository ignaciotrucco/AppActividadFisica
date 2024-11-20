using Microsoft.AspNetCore.Mvc;
using ActividadFisica.Models;
using ActividadFisica.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ActividadFisica.Controllers;

[Authorize(Roles = "ADMINISTRADOR")]

public class DeportistasController : Controller
{
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    //CONSTRUCTOR
    public DeportistasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Deportistas()
    {
        return View();
    }

    public JsonResult ListadoDeportistas()
    {
        List<VistaPersonas> vistaPersonas = new List<VistaPersonas>();

        var listadoDeportistas = _context.Personas.ToList();
        var usuarios = _context.Users.ToList();

        foreach (var deportistas in listadoDeportistas)
        {
            var usuario = usuarios.Where(u => u.Id == deportistas.UsuarioID).Single();

            var vistaPersona = new VistaPersonas {
                PersonaID = deportistas.PersonaID,
                UsuarioID = deportistas.UsuarioID,
                Email = usuario.Email,
                NombreCompleto = deportistas.NombreCompleto,
                Genero = deportistas.Genero,
                Altura = deportistas.Altura,
                FechaNacimiento = deportistas.FechaNacimiento,
                Peso = deportistas.Peso,
                GeneroString = Enum.GetName(typeof(Genero), deportistas.Genero),
                FechaNacimientoString = deportistas.FechaNacimiento.ToString("dd/MM/yyyy")
            };
            vistaPersonas.Add(vistaPersona);
        }


        return Json(vistaPersonas);
    }
}