using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ActividadFisica.Models;
using ActividadFisica.Data;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;

namespace ActividadFisica.Controllers;

[Authorize(Roles = "ADMINISTRADOR")]
public class TipoEjerciciosController : Controller
{
    private ApplicationDbContext _context;

    // CONSTRUCTOR
    public TipoEjerciciosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult TipoEjercicios()
    {
        return View();
    }

    public JsonResult ListadoTipoEjercicios(int? tipoEjercicioID)
    {
        //DEFINIMOS UNA VARIABLE DONDE GUARDAMOS EL LISTADO COMPLETO DE TIPOS DE EJERCICIOS
        var tipoDeEjercicios = _context.Tipo_Ejercicios.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (tipoEjercicioID != null)
        {
            //FILTRAMOS CADA TIPO DE EJERCICIO POR ID PARA QUE COINCIDAN
            tipoDeEjercicios = tipoDeEjercicios.Where(t => t.TipoEjercicioID == tipoEjercicioID).ToList();
        }

        return Json(tipoDeEjercicios);

    }

    public JsonResult GuardarTipoEjercicios(int tipoEjercicioID, string descripcion)
    {

        //1- VERIFICAMOS SI REALMENTE INGRESO ALGUN CARACTER Y LA VARIABLE NO SEA NULL
        // if (descripcion != null && descripcion != "")
        // {
        //     //INGRESA SI ESCRIBIO SI O SI
        // }

        // if (String.IsNullOrEmpty(descripcion) == false)
        // {
        //     //INGRESA SI ESCRIBIO SI O SI 
        // }

        string resultado = "";

        if (!String.IsNullOrEmpty(descripcion))
        {
            //INGRESA SI ESCRIBIO SI O SI 
            descripcion = descripcion.ToUpper();

            //2- VERIFICAR SI ESTA EDITANDO O CREANDO NUEVO REGISTRO}
            if (tipoEjercicioID == 0)
            {
                //3- VERIFICAMOS SI EXISTE EN BASE DE DATOS UN REGISTRO CON LA MISMA DESCRIPCION
                //PARA REALIZAR ESA VERIFICACION BUSCAMOS EN EL CONTEXTO, ES DECIR EN BASE DE DATOS 
                //SI EXISTE UN REGISTRO CON ESA DESCRIPCION 
                var siExisteTipoEjercicio = _context.Tipo_Ejercicios.Where(t => t.Descripcion == descripcion).Count();
                if (siExisteTipoEjercicio == 0)
                {

                    //GUARDAMOS NUEVO EJERCICIO
                    var nuevoEjercicio = new Tipo_Ejercicio
                    {
                        Descripcion = descripcion
                    };

                    _context.Add(nuevoEjercicio);
                    _context.SaveChanges();
                    resultado = "El elemento ha sido creado correctamente";
                }

                else
                {
                    resultado = "Ya existe un registro con la misma descripción";
                }
            }
            else
            {
                //ESTO QUIERE DECIR QUE VAMOS A EDITAR EL EJERCICIO
                var editarTipoEjercicio = _context.Tipo_Ejercicios.Where(t => t.TipoEjercicioID == tipoEjercicioID).SingleOrDefault();
                if (editarTipoEjercicio != null)
                {
                    //BUSCAMOS EN LA TABLA SI EXISTE UN REGISTRO CON EL MISMO NOMBRE PERO QUE EL ID SEA DISTINTO AL QUE ESTAMOS EDITANDO
                    var siExisteTipoEjercicio = _context.Tipo_Ejercicios.Where(t => t.Descripcion == descripcion && t.TipoEjercicioID != tipoEjercicioID).Count();
                    if (siExisteTipoEjercicio == 0)
                    {
                        //El elemento es correcto y podemos guardarlo
                        editarTipoEjercicio.Descripcion = descripcion;
                        _context.SaveChanges();
                        resultado = "El elemento ha sido modificado correctamente";
                    }
                    else
                    {
                        resultado = "Ya existe un registro con la misma descripción";
                    }
                }
            }
        }
        else
        {
            resultado = "Por favor debe ingresar una descripción";
        }

        return Json(resultado);
    }

    public JsonResult EliminarTipoEjercicio(int tipoEjercicioID)
    {
        bool eliminado = false;

        var existeEjercicio = _context.EjercicioFisico.Where(e => e.TipoEjercicioID == tipoEjercicioID).Count();

        if(existeEjercicio == 0)
        {
        var tipoEjercicio = _context.Tipo_Ejercicios.Find(tipoEjercicioID);
        _context.Remove(tipoEjercicio);
        _context.SaveChanges();
        eliminado = true;
        }

        return Json(eliminado);

    }
}

