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
    public class PeriodoNominaController : Controller
    {
        private readonly IApiServicio apiServicio;

        public PeriodoNominaController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> Create(string mensaje)
        {
            await CargarCombox();
            return View();
        }

        public async Task CargarCombox()
        {
            ViewData["IdProceso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ProcesoNomina>(new Uri(WebApp.BaseAddress), "api/ProcesoNomina/ListarProcesoNomina"), "IdProceso", "Descripcion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PeriodoNomina PeriodoNomina)
        {

            if (!ModelState.IsValid)
            {
               
                return View(PeriodoNomina);
            }
            Response response = new Response();
            try
            {
                if (PeriodoNomina.FechaFin<PeriodoNomina.FechaInicio)
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{Mensaje.FechaRangoMayor}";
                    ModelState.AddModelError("FechaFin", Mensaje.FechaRangoMayor);
                    ModelState.AddModelError("FechaInicio", Mensaje.FechaRangoMenor);
                    await CargarCombox();
                    return View(PeriodoNomina);
                }

                response = await apiServicio.InsertarAsync(PeriodoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/PeriodoNomina/InsertarPeriodoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }

                this.TempData["Mensaje"] = response.Message;
                await CargarCombox();
                return View(PeriodoNomina);

            }
            catch (Exception)
            {
                return this.Redireccionar("Create", $"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var vista = new PeriodoNomina();
                var respuesta = new Response();
                    var PeriodoNominaSession = new PeriodoNomina { IdPeriodo = id };
                    respuesta = await apiServicio.ObtenerElementoAsync1<Response>(PeriodoNominaSession, new Uri(WebApp.BaseAddress),
                                                            "api/PeriodoNomina/ObtenerPeriodoNomina");
                    if (respuesta.IsSuccess)
                    {
                        vista = JsonConvert.DeserializeObject<PeriodoNomina>(respuesta.Resultado.ToString());
                    }
                await CargarCombox();
                return View(vista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PeriodoNomina PeriodoNomina)
        {

            if (!ModelState.IsValid)
            {
                return View(PeriodoNomina);
            }
            Response response = new Response();
            try
            {
                if (PeriodoNomina.FechaFin < PeriodoNomina.FechaInicio)
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{Mensaje.FechaRangoMayor}";
                    ModelState.AddModelError("FechaFin", Mensaje.FechaRangoMayor);
                    ModelState.AddModelError("FechaInicio", Mensaje.FechaRangoMenor);
                    await CargarCombox();
                    return View(PeriodoNomina);
                }
                response = await apiServicio.EditarAsync<Response>(PeriodoNomina, new Uri(WebApp.BaseAddress),
                                                             "api/PeriodoNomina/EditarPeriodoNomina");

                if (response.IsSuccess)
                {
                    var vista = JsonConvert.DeserializeObject<PeriodoNomina>(response.Resultado.ToString());
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                ViewData["Error"] = response.Message;
                await CargarCombox();
                return View(PeriodoNomina);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEditar}");
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {
            try
            {
                var lista = await apiServicio.Listar<PeriodoNomina>(new Uri(WebApp.BaseAddress)
                                                                     , "api/PeriodoNomina/ListarPeriodoNomina");
                return View(lista);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorListado}");
            }
        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Index");
                }
                var tipoConjuntoEliminar = new PeriodoNomina { IdPeriodo = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/PeriodoNomina/EliminarPeriodoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Satisfactorio}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Satisfactorio}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

      
    }
}