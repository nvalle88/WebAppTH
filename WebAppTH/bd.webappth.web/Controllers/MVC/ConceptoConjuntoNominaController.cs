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
    public class ConceptoConjuntoNominaController : Controller
    {

        private readonly IApiServicio apiServicio;

        public ConceptoConjuntoNominaController(IApiServicio apiServicio)
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


        public async Task<IActionResult> CreateConceptoConjunto(string mensaje)
        {
            InicializarMensaje(mensaje);
            await CargarComboxConceptoConjunto();
            var vista = new ConceptoConjuntoNomina { Suma = false, Resta = false };
            return View(vista);
        }

        public async Task CargarComboxConceptoConjunto()
        {
            ViewData["IdConjunto"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ConjuntoNomina>(new Uri(WebApp.BaseAddress), "api/ConjuntoNomina/ListarConjuntoNomina"), "IdConjunto", "Descripcion");
            ViewData["IdConcepto"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ConceptoNomina>(new Uri(WebApp.BaseAddress), "api/ConceptoNomina/ListarConceptoNomina"), "IdConcepto", "Descripcion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConceptoConjunto(ConceptoConjuntoNomina ConceptoConjuntoNomina)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ConceptoConjuntoNomina);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(ConceptoConjuntoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ConceptoConjuntoNomina/InsertarConceptoConjuntoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Satisfactorio}|{Mensaje.Satisfactorio}");
                }

                ViewData["Error"] = response.Message;
                await CargarComboxConceptoConjunto();
                return View(ConceptoConjuntoNomina);

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> EditConceptoConjunto(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var ConceptoConjuntoNomina = new ConceptoConjuntoNomina { IdConceptoConjunto = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(ConceptoConjuntoNomina, new Uri(WebApp.BaseAddress),
                                                                  "api/ConceptoConjuntoNomina/ObtenerConceptoConjuntoNomina");
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        var vista = JsonConvert.DeserializeObject<ConceptoConjuntoNomina>(respuesta.Resultado.ToString());
                        await CargarComboxConceptoConjunto();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConceptoConjunto(ConceptoConjuntoNomina ConceptoConjuntoNomina)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ConceptoConjuntoNomina);
            }
            Response response = new Response();
            try
            {
                if (ConceptoConjuntoNomina.IdConjunto > 0)
                {
                    response = await apiServicio.EditarAsync<Response>(ConceptoConjuntoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/ConceptoConjuntoNomina/EditarConceptoConjuntoNomina");

                    if (response.IsSuccess)
                    {

                        return this.Redireccionar($"{Mensaje.Satisfactorio}|{Mensaje.Satisfactorio}");
                    }
                    ViewData["Error"] = response.Message;
                    await CargarComboxConceptoConjunto();
                    return View(ConceptoConjuntoNomina);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> IndexConceptoConjunto(string mensaje)
        {
            try
            {
                InicializarMensaje(mensaje);
                var lista = await apiServicio.Listar<ConceptoConjuntoNomina>(new Uri(WebApp.BaseAddress)
                                                                     , "api/ConceptoConjuntoNomina/ListarConceptoConjuntoNomina");
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
                var tipoConjuntoEliminar = new ConceptoConjuntoNomina { IdConceptoConjunto = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/ConceptoConjuntoNomina/EliminarConceptoConjuntoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Satisfactorio}|{Mensaje.Satisfactorio}");
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