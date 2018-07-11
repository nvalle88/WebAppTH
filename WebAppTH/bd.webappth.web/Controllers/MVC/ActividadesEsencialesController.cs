using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class ActividadesEsencialesController : Controller
    {

        private readonly IApiServicio apiServicio;


        public ActividadesEsencialesController(IApiServicio apiServicio)
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

        public IActionResult Create( string mensaje )
        {
            InicializarMensaje(mensaje);
            return View();
        }


        public async Task<IActionResult> EliminarIncideOcupacionalActividadesEsenciales(int IdActividadesEsenciales, int idIndiceOcupacional)
        {

            try
            {

                var IndiceOcupacionalAreaConocimiento = new IndiceOcupacionalActividadesEsenciales
                {
                    IdActividadesEsenciales = IdActividadesEsenciales,
                    IdIndiceOcupacional = idIndiceOcupacional
                };
                var response = await apiServicio.EliminarAsync(IndiceOcupacionalAreaConocimiento, new Uri(WebApp.BaseAddress)
                                                               , "api/ActividadesEsenciales/EliminarIndiceOcupacionalActividadesEsenciales");
                if (response.IsSuccess)
                {
                    /*
                    return RedirectToAction("ActividadesEsenciales", "IndicesOcupacionales", new { id = IndiceOcupacionalAreaConocimiento.IdIndiceOcupacional });
                    */
                    return this.Redireccionar(
                            "ActividadesEsenciales",
                            "IndicesOcupacionales",
                             new { id = IndiceOcupacionalAreaConocimiento.IdIndiceOcupacional},
                            $"{Mensaje.Success}|{response.Message}"
                         );
            }
                return BadRequest();
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActividadesEsenciales actividadesEsenciales)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                return View(actividadesEsenciales);
            }
            Response response = new Response();
            ActividadesEsenciales actividades=new ActividadesEsenciales();

            try
            {
                response = await apiServicio.InsertarAsync(actividadesEsenciales,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ActividadesEsenciales/InsertarActividadesEsenciales");


                if (response.IsSuccess)
                {


                    return this.Redireccionar(
                            "ActividadesEsenciales",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}"
                         );
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                return View(actividadesEsenciales);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Actividad Esencial",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/ActividadesEsenciales");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ActividadesEsenciales>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        return View(respuesta.Resultado);
                    }

                }

                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ActividadesEsenciales actividadesEsenciales)
        {
            Response response = new Response();
            try
            {
                
                if (!string.IsNullOrEmpty(id))
                {

                    var objetoAnterior = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/ActividadesEsenciales");

                    response = await apiServicio.EditarAsync(id, actividadesEsenciales, new Uri(WebApp.BaseAddress),
                                                                 "api/ActividadesEsenciales");

                    if (response.IsSuccess)
                    {

                        return this.Redireccionar(
                                "ActividadesEsenciales",
                                "Index",
                                $"{Mensaje.Success}|{response.Message}"
                             );
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";



                    return View(actividadesEsenciales);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una actividad esencial",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index()
        {

            var lista = new List<ActividadesEsenciales>();
            try
            {
                lista = await apiServicio.Listar<ActividadesEsenciales>(new Uri(WebApp.BaseAddress)
                                                                    , "api/ActividadesEsenciales/ListarActividadesEsenciales");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando actividades esenciales",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/ActividadesEsenciales");
                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "ActividadesEsenciales",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
    }
}