using ActividadFisica.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActividadFisica.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tipo_Ejercicio> Tipo_Ejercicios {get; set;} 
    public DbSet<EjercicioFisico> EjercicioFisico {get; set;}
    public DbSet<Lugar> Lugares {get; set;}
    public DbSet<EventoDeportivo> EventosDeportivos {get; set;}
    public DbSet<Persona> Personas {get; set;}
}
