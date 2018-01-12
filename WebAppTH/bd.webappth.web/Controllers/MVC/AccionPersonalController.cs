using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class AccionPersonalController : Controller
    {

        private readonly IApiServicio apiServicio;


        public AccionPersonalController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Index()
        {

            var lista = new List<AccionPersonal>();
            try
            {
                lista = await apiServicio.Listar<AccionPersonal>(new Uri(WebApp.BaseAddress)
                                                                    , "api/AccionPersonal/ListarAccionPersonal");
                return View(lista);
            }
            catch (Exception ex)
            {
                return View(lista);
            }
        }

        public async Task<IActionResult> Create()
        {

            ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "NombreApellido");
            ViewData["IdTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoAccionPersonal>(new TipoAccionPersonal {EsResponsableTH=true},new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonalesPorEsTalentoHumano"), "IdTipoAccionPersonal", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccionPersonal accionPersonal)
        {
            Response response = new Response();
            try
            {
               
                if (ModelState.IsValid)
                {

                 if (accionPersonal.FechaRigeHasta.Subtract(accionPersonal.FechaRige).TotalDays <= 0)
                 {
                        ModelState.AddModelError("FechaRige", Mensaje.FechaRangoMenor);
                        ModelState.AddModelError("FechaRigeHasta", Mensaje.FechaRangoMayor);
                 }

                    response = await apiServicio.InsertarAsync(accionPersonal,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/AccionPersonal/CrearAccionPersonal");
                if (response.IsSuccess)
                { 
                  return RedirectToAction("Index");
                }

                    ViewData["Error"] = response.Message;
                    ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "NombreApellido");
                    ViewData["IdTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoAccionPersonal>(new TipoAccionPersonal { EsResponsableTH = true }, new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonalesPorEsTalentoHumano"), "IdTipoAccionPersonal", "Nombre");
                    return View(accionPersonal);
                }

                ViewData["Error"] = response.Message;
                ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "NombreApellido");
                ViewData["IdTipoAccionPersonal"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoAccionPersonal>(new TipoAccionPersonal { EsResponsableTH = true }, new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonalesPorEsTalentoHumano"), "IdTipoAccionPersonal", "Nombre");
                return View(accionPersonal);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}