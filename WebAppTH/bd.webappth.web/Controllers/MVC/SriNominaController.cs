using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Constantes;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Extensores;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class SriNominaController : Controller
    {
        private readonly IApiServicio apiServicio;

        public SriNominaController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        public async Task<IActionResult> CreateSriDetalle(string mensaje)
        {
            if (ObtenerSriNomina().IdSri > 0)
            {
                await CargarComboxConceptoConjunto();
                return View();
            }
            return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
        }

        public async Task CargarComboxConceptoConjunto()
        {
            ViewData["IdConjunto"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ConjuntoNomina>(new Uri(WebApp.BaseAddress), "api/ConjuntoNomina/ListarConjuntoNomina"), "IdConjunto", "Descripcion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSriDetalle(SriDetalle sriDetalle)
        {
            Response response = new Response();
            try
            {
                if (sriDetalle.FraccionBasica >=sriDetalle.ExcesoHasta)
                {
                    ModelState.AddModelError("FraccionBasica","La Fracción básica no puede ser mayor que Exceso hasta");
                    ModelState.AddModelError("ExcesoHasta", "El Exceso hasta no puede se menor que la Fracción básica");
                    return View(sriDetalle);

                }
                if (ObtenerSriNomina().IdSri > 0)
                {
                    sriDetalle.IdSri = ObtenerSriNomina().IdSri;
                    response = await apiServicio.InsertarAsync(sriDetalle,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/SriNomina/InsertarSriDetalle");
                    if (response.IsSuccess)
                    {
                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}", "IndexSriDetalle");
                    }

                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    await CargarComboxConceptoConjunto();
                    return View(sriDetalle);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> EditSriDetalle(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    
                    var sriDetalle = new SriDetalle { IdSriDetalle = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(sriDetalle, new Uri(WebApp.BaseAddress),
                                                                  "api/SriNomina/ObtenerSriDetalle");
                    if (respuesta.IsSuccess)
                    {
                        var vista = JsonConvert.DeserializeObject<SriDetalle>(respuesta.Resultado.ToString());
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
        public async Task<IActionResult> EditSriDetalle(SriDetalle sriDetalle)
        {
            Response response = new Response();
            try
            {
                if (sriDetalle.FraccionBasica >= sriDetalle.ExcesoHasta)
                {
                    ModelState.AddModelError("FraccionBasica", "La Fracción básica no puede ser mayor que Exceso hasta");
                    ModelState.AddModelError("ExcesoHasta", "El Exceso hasta no puede se menor que la Fracción básica");
                    return View(sriDetalle);

                }
                if (ObtenerSriNomina().IdSri > 0)
                {
                    sriDetalle.IdSri = ObtenerSriNomina().IdSri;
                    response = await apiServicio.EditarAsync<Response>(sriDetalle, new Uri(WebApp.BaseAddress),
                                                                 "api/SriNomina/EditarSriDetalle");

                    if (response.IsSuccess)
                    {

                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}", "IndexSriDetalle");
                    }
                    this.TempData["Mensaje"] = $"{Mensaje.Informacion}|{response.Message}";
                    await CargarComboxConceptoConjunto();
                    return View(sriDetalle);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> IndexSriDetalle(string mensaje)
        {
            try
            {
                if (ObtenerSriNomina().IdSri > 0)
                {
                     
                    var sriDetalle = new SriNomina { IdSri = ObtenerSriNomina().IdSri };
                    var lista = await apiServicio.Listar<SriDetalle>(sriDetalle, new Uri(WebApp.BaseAddress)
                                                                         , "api/SriNomina/ListarSriDetalle");
                    return View(lista);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> DeleteSriDetalle(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}", "IndexSriDetalle");
                }
                var tipoConjuntoEliminar = new SriDetalle { IdSriDetalle = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/SriNomina/EliminarSriDetalle");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}", "IndexSriDetalle");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

        public IActionResult CreateSriNomina(string mensaje)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSriNomina(SriNomina SriNomina)
        {

            if (!ModelState.IsValid)
            {
                 
                return View(SriNomina);
            }
            Response response = new Response();
            try
            {

                response = await apiServicio.InsertarAsync(SriNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/SriNomina/InsertarSriNomina");
                if (response.IsSuccess)
                {

                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }

                this.TempData["Mensaje"] = response.Message;
                return View(SriNomina);

            }
            catch (Exception)
            {
                return this.Redireccionar("Create", $"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }
    
        public async Task<IActionResult> DatosBasicosSri(int id)
        {
            try
            {
                var entrada = false;
                var vista = new SriNomina();
                var respuesta = new Response();

                if (HttpContext.Session.GetInt32(Constantes.IdSriSession) != id)
                {
                    HttpContext.Session.SetInt32(Constantes.IdSriSession, id);
                    var SriNominaSession = new SriNomina { IdSri = id };
                    respuesta = await apiServicio.ObtenerElementoAsync1<Response>(SriNominaSession, new Uri(WebApp.BaseAddress),
                                                            "api/SriNomina/ObtenerSriNomina");
                    if (respuesta.IsSuccess)
                    {
                        vista = JsonConvert.DeserializeObject<SriNomina>(respuesta.Resultado.ToString());
                    }
                    HttpContext.Session.SetString(Constantes.DescripcionSriSession, vista.Descripcion);

                    entrada = true;
                }

                if (!entrada)
                {
                    var SriNomina = new SriNomina { IdSri = ObtenerSriNomina().IdSri };
                    respuesta = await apiServicio.ObtenerElementoAsync1<Response>(SriNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/SriNomina/ObtenerSriNomina");
                    if (respuesta.IsSuccess)
                    {
                        vista = JsonConvert.DeserializeObject<SriNomina>(respuesta.Resultado.ToString());
                    }
                }
                return View(vista);


            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatosBasicosSri(SriNomina SriNomina)
        {

            if (!ModelState.IsValid)
            {
                 
                return View(SriNomina);
            }
            Response response = new Response();
            try
            {

                SriNomina.IdSri = ObtenerSriNomina().IdSri;
                response = await apiServicio.EditarAsync<Response>(SriNomina, new Uri(WebApp.BaseAddress),
                                                             "api/SriNomina/EditarSriNomina");

                if (response.IsSuccess)
                {
                    var vista = JsonConvert.DeserializeObject<SriNomina>(response.Resultado.ToString());
                    HttpContext.Session.SetString(Constantes.DescripcionSriSession, vista.Descripcion);
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}", "DatosBasicosSri");
                }
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                return View(SriNomina);
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
                 
                var lista = await apiServicio.Listar<SriNomina>(new Uri(WebApp.BaseAddress)
                                                                     , "api/SriNomina/ListarSriNomina");
                return View(lista);
            }
            catch (Exception ex)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorListado}");
            }
        }

        public async Task<IActionResult> DesactivarSriNomina(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorActivar}");
                }
                var tipoConjuntoEliminar = new SriNomina { IdSri = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/SriNomina/DesactivarSriNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorDesactivar}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorDesactivar}");
            }
        }

        public async Task<IActionResult> ActivarSriNomina(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorActivar}");
                }
                var tipoConjuntoEliminar = new SriNomina { IdSri = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/SriNomina/ActivarSriNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorActivar}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorActivar}");
            }
        }

        public SriNomina ObtenerSriNomina()
        {
            var concepto = new SriNomina
            {
                IdSri = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.IdSriSession)),
                Descripcion = Convert.ToString(HttpContext.Session.GetInt32(Constantes.DescripcionSriSession)),
            };
            return concepto;
        }
    }
}