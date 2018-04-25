using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace bd.webappth.web.Controllers.MVC
{
    public class ProcesoNominaController : Controller
    {
        private readonly IApiServicio apiServicio;

        public ProcesoNominaController(IApiServicio apiServicio)
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


        public IActionResult Create(string mensaje)
        {
            InicializarMensaje(mensaje);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProcesoNomina ProcesoNomina)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ProcesoNomina);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(ProcesoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ProcesoNomina/InsertarProcesoNomina");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index", new { mensaje = Mensaje.GuardadoSatisfactorio });
                }

                ViewData["Error"] = response.Message;
                return View(ProcesoNomina);

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
                    var ProcesoNomina = new ProcesoNomina { IdProceso=Convert.ToInt32(id)};
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(ProcesoNomina, new Uri(WebApp.BaseAddress),
                                                                  "api/ProcesoNomina/ObtenerProcesoNomina");
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        var vista = JsonConvert.DeserializeObject<ProcesoNomina>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> Edit(ProcesoNomina ProcesoNomina)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(ProcesoNomina);
            }
            Response response = new Response();
            try
            {
                if (ProcesoNomina.IdProceso > 0)
                {
                    response = await apiServicio.EditarAsync<Response>(ProcesoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/ProcesoNomina/EditarProcesoNomina");

                    if (response.IsSuccess)
                    {

                        return RedirectToAction("Index", new { mensaje = Mensaje.RegistroEditado });
                    }
                    ViewData["Error"] = response.Message;
                    return View(ProcesoNomina);
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
                var lista = await apiServicio.Listar<ProcesoNomina>(new Uri(WebApp.BaseAddress)
                                                                     , "api/ProcesoNomina/ListarProcesoNomina");
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
                var tipoConjuntoEliminar = new ProcesoNomina { IdProceso = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/ProcesoNomina/EliminarProcesoNomina");
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
