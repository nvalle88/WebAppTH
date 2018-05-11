using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Extensores;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class CalculoNominaController: Controller
    {

        private readonly IApiServicio apiServicio;

        public CalculoNominaController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create(string mensaje)
        {
            await CargarComboxProcesoPeriodo();
            var vista = new CalculoNomina { Automatico = false, Reportado = false,EmpleadoActivo=true,EmpleadoPasivo=false };
            return View(vista);
        }

        public async Task CargarComboxProcesoPeriodo()
        {
            ViewData["Procesos"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ProcesoNomina>(new Uri(WebApp.BaseAddress), "api/ProcesoNomina/ListarProcesoNomina"), "IdProceso", "Descripcion");
            ViewData["Periodos"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<PeriodoNomina>(new Uri(WebApp.BaseAddress), "api/PeriodoNomina/ListarPeriodoNomina"), "IdPeriodo", "Descripcion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CalculoNomina CalculoNomina)
        {
            if (!ModelState.IsValid)
            {
                 
                return View(CalculoNomina);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(CalculoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/CalculoNomina/InsertarCalculoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                await CargarComboxProcesoPeriodo();
                return View(CalculoNomina);

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }



        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var CalculoNomina = new CalculoNomina { IdCalculoNomina = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(CalculoNomina, new Uri(WebApp.BaseAddress),
                                                                  "api/CalculoNomina/ObtenerCalculoNomina");
                    if (respuesta.IsSuccess)
                    {
                         
                        var vista = JsonConvert.DeserializeObject<CalculoNomina>(respuesta.Resultado.ToString());
                        await CargarComboxProcesoPeriodo();
                        return View(vista);
                    }
                }

                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }


        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }
                var calculoNomina = new CalculoNomina { IdCalculoNomina = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(calculoNomina, new Uri(WebApp.BaseAddress)
                                                               , "api/CalculoNomina/EliminarCalculoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CalculoNomina CalculoNomina)
        {

            if (!ModelState.IsValid)
            {
                 
                return View(CalculoNomina);
            }
            Response response = new Response();
            try
            {
                
                    response = await apiServicio.EditarAsync<Response>(CalculoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/CalculoNomina/EditarCalculoNomina");

                    if (response.IsSuccess)
                    {

                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                    }
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    await CargarComboxProcesoPeriodo();
                    return View(CalculoNomina);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {
            try
            {
                
                var lista = await apiServicio.Listar<CalculoNomina>(new Uri(WebApp.BaseAddress)
                                                                     , "api/CalculoNomina/ListarCalculoNomina");
                return View(lista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> ConceptoConjunto(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }
                var tipoConjuntoEliminar = new CalculoNomina { IdCalculoNomina = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/CalculoNomina/EliminarCalculoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

    }
}