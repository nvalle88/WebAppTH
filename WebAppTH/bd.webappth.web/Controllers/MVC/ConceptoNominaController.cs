using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class ConceptoNominaController : Controller
    {
        private readonly IApiServicio apiServicio;

        public ConceptoNominaController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }
        private void InicializarMensaje(string mensaje)

        {

            if (mensaje == null)

            {

                mensaje = "";

            }

            ViewData["Error"] = mensaje;

        }


        public async Task<IActionResult> Create(string mensaje)
        {
            InicializarMensaje(mensaje);
            await CargarCombox();
           // var vista = new ConceptoNomina { Suma = false, Resta = false };
            return View();
        }

        public async Task CargarCombox()
        {
            ViewData["IdProceso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ProcesoNomina>(new Uri(WebApp.BaseAddress), "api/ProcesoNomina/ListarProcesoNomina"), "IdProceso", "Descripcion");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConceptoNomina ConceptoNomina)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ConceptoNomina);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(ConceptoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ConceptoNomina/InsertarConceptoNomina");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index", new { mensaje = Mensaje.GuardadoSatisfactorio });
                }

                ViewData["Error"] = response.Message;
                await CargarCombox();
                return View(ConceptoNomina);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var ConceptoNomina = new ConceptoNomina { IdConcepto = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(ConceptoNomina, new Uri(WebApp.BaseAddress),
                                                                  "api/ConceptoNomina/ObtenerConceptoNomina");
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        var vista = JsonConvert.DeserializeObject<ConceptoNomina>(respuesta.Resultado.ToString());
                        await CargarCombox();
                        return View(vista);
                    }
                }

                return RedirectToAction("Index", new { mensaje = Mensaje.RegistroNoEncontrado });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ConceptoNomina ConceptoNomina)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ConceptoNomina);
            }
            Response response = new Response();
            try
            {
                if (ConceptoNomina.IdConcepto > 0)
                {
                    response = await apiServicio.EditarAsync<Response>(ConceptoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/ConceptoNomina/EditarConceptoNomina");

                    if (response.IsSuccess)
                    {

                        return RedirectToAction("Index", new { mensaje = Mensaje.RegistroEditado });
                    }
                    ViewData["Error"] = response.Message;
                    await CargarCombox();
                    return View(ConceptoNomina);
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {
            try
            {
                InicializarMensaje(mensaje);
                var lista = await apiServicio.Listar<ConceptoNomina>(new Uri(WebApp.BaseAddress)
                                                                     , "api/ConceptoNomina/ListarConceptoNomina");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
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
                var tipoConjuntoEliminar = new ConceptoNomina { IdConcepto = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/ConceptoNomina/EliminarConceptoNomina");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index", new { mensaje = Mensaje.BorradoSatisfactorio });
                }
                return RedirectToAction("Index", new { mensaje = response.Message });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}