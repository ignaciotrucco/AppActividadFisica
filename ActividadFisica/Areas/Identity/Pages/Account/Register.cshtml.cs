// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using ActividadFisica.Data;
using ActividadFisica.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ActividadFisica.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _context;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _rolManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            RoleManager<IdentityRole> rolManager,
            ApplicationDbContext context,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _rolManager = rolManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Vuelve a llenar el ViewData["Genero"] cuando la validación falla
            ViewData["Genero"] = new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = "[SELECCIONE...]" }
                }.Concat(Enum.GetValues(typeof(Genero)).Cast<Genero>().Select(e => new SelectListItem
                {
                    Value = e.GetHashCode().ToString(),
                    Text = e.ToString().ToUpper()
                })).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string NombreCompleto, DateTime FechaNacimiento, Genero Genero, string Peso, string Altura, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {

                    var usuario = _context.Users.Where(u => u.Email == user.Email).SingleOrDefault();

                    await _userManager.AddToRoleAsync(usuario, "USUARIO");

                    //AGREGAR CULTURA ESPAÑOL ARGENTINA AL METODO
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("es-AR");

                    //CONVERTIR PESO A DECIMAL POR SI EL USUARIO SELECCIONA UN .
                    string pesoString = Peso;
                    if (!string.IsNullOrEmpty(pesoString))
                    {
                        pesoString = pesoString.Replace(".", ",");
                    }
                    Decimal pesoDecimal = new Decimal();
                    var validaPeso = Decimal.TryParse(pesoString, out pesoDecimal);

                    //CONVERTIR ALTURA A DECIMAL POR SI EL USUARIO SELECCIONA UN .
                    string alturaString = Altura;
                    if (!string.IsNullOrEmpty(alturaString)) {
                        alturaString = alturaString.Replace(".", ",");
                    }
                    Decimal alturaDecimal = new Decimal();
                    var validaAltura = Decimal.TryParse(alturaString, out alturaDecimal);

                    var nuevaPersona = new Persona
                    {
                        NombreCompleto = NombreCompleto,
                        FechaNacimiento = FechaNacimiento,
                        Genero = Genero,
                        Peso = pesoDecimal,
                        Altura = alturaDecimal,
                        UsuarioID = user.Id,
                    };

                    _context.Personas.Add(nuevaPersona);
                    _context.SaveChanges();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            // Vuelve a llenar el ViewData["Genero"] cuando la validación falla
            ViewData["Genero"] = new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = "[SELECCIONE...]" }
                }.Concat(Enum.GetValues(typeof(Genero)).Cast<Genero>().Select(e => new SelectListItem
                {
                    Value = e.GetHashCode().ToString(),
                    Text = e.ToString().ToUpper()
                })).ToList();


            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
