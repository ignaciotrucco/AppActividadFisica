using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ActividadFisica.Models;
using Microsoft.AspNetCore.Authorization;
using ActividadFisica.Data;
using Microsoft.AspNetCore.Identity;

namespace ActividadFisica.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _rolManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _rolManager = rolManager;
    }

    public async Task<IActionResult> Index()
    {
        await CrearRolesyPrimerUsuario();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<JsonResult> CrearRolesyPrimerUsuario()
    {
        //CREAMOS ROL ADMINISTRADOR
        var crearRolAdmin = _context.Roles.Where(c => c.Name == "ADMINISTRADOR").SingleOrDefault();
        if (crearRolAdmin == null)
        {
            var roleResult = await _rolManager.CreateAsync(new IdentityRole("ADMINISTRADOR"));
        }

        //CREAMOS ROL USUARIO
        var crearRolUsuario = _context.Roles.Where(c => c.Name == "USUARIO").SingleOrDefault();
        if (crearRolUsuario == null)
        {
            var roleResult = await _rolManager.CreateAsync(new IdentityRole("USUARIO"));
        }

        //CREAMOS USUARIO PRINCIPAL (ADMIN)
        bool creado = false;

        var usuario = _context.Users.Where(u => u.Email == "adminejercicios@gmail.com").SingleOrDefault();
        if (usuario == null)
        {
            var user = new IdentityUser { UserName = "adminejercicios@gmail.com", Email = "adminejercicios@gmail.com" };
            var result = await _userManager.CreateAsync(user, "truccosebastian");

            await _userManager.AddToRoleAsync(user, "ADMINISTRADOR");
            creado = result.Succeeded;
        }

        //CODIGO PARA BUSCAR EL USUARIO EN CASO DE NECESITARLO
        var superusuario = _context.Users.Where(s => s.Email == "adminejercicios@sistema.com").SingleOrDefault();
        if (superusuario != null)
        {

            //var personaSuperusuario = _contexto.Personas.Where(r => r.UsuarioID == superusuario.Id).Count();

            var usuarioID = superusuario.Id;

        }

        return Json(creado);
    }
}
