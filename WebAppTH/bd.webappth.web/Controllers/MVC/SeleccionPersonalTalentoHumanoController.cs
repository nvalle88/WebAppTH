using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class SeleccionPersonalTalentoHumanoController : Controller
    {

        private readonly IApiServicio apiServicio;


        public SeleccionPersonalTalentoHumanoController(IApiServicio apiServicio)
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

        public async Task<IActionResult> Index(string mensaje)
        {
            InicializarMensaje(mensaje);
            var lista = new List<ViewModelSeleccionPersonal>();
            try
            {
                lista = await apiServicio.Listar<ViewModelSeleccionPersonal>(new Uri(WebApp.BaseAddress),
                                                                    "api/SeleccionPersonalTalentoHumano/ListarPuestoVacantesSeleccionPersonal");

                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                mensaje = Mensaje.Excepcion;
                return View(lista);
            }
        }
        public async Task<IActionResult> Create(int id)
        {
            try
            {

                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = id
                };
                var response = await apiServicio.ObtenerElementoAsync(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/SeleccionPersonalTalentoHumano/ObtenerEncabezadopostulante");

                if (response.IsSuccess)
                {
                    InicializarMensaje(null);
                    var encabezado = JsonConvert.DeserializeObject<ViewModelSeleccionPersonal>(response.Resultado.ToString());
                    return View(encabezado);
                    
                }
               
                ViewData["Error"] = response.Message;
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewModelSeleccionPersonal viewModelSeleccionPersonal)
        {

            //    if (!ModelState.IsValid)
            //    {
            //        InicializarMensaje(null);
            //        ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
            //        ViewData["IdProvincia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
            //        return View(ciudad);
            //    }
            //    Response response = new Response();
            //    try
            //    {
            //        response = await apiServicio.InsertarAsync(ciudad,
            //                                                     new Uri(WebApp.BaseAddress),
            //                                                     "api/Ciudad/InsertarCiudad");
            //        if (response.IsSuccess)
            //        {

            //            var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
            //            {
            //                ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
            //                ExceptionTrace = null,
            //                Message = "Se ha creado una Ciudad",
            //                UserName = "Usuario 1",
            //                LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
            //                LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
            //                EntityID = string.Format("{0} {1}", "Ciudad:", ciudad.IdCiudad),
            //            });

            //            return RedirectToAction("Index");
            //        }

            //        ViewData["Error"] = response.Message;
            //        ViewData["IdPais"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
            //        ViewData["IdProvincia"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
            //        return View(ciudad);

            //    }
            //    catch (Exception ex)
            //    {
            //        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
            //        {
            //            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
            //            Message = "Creando Ciudad",
            //            ExceptionTrace = ex.Message,
            //            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
            //            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
            //            UserName = "Usuario APP WebAppTh"
            //        });

            //        return BadRequest();
            //    }
            return View();
        }
        
        public async Task<IActionResult> CreateDatosPersonales(int id)
        {
            try
            {
                InicializarMensaje(null);
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = id
                };
                return View(usuario);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexInstruccionFormal(int id)
        {
            ///InicializarMensaje(mensaje);
            //var lista = new List<ViewModelSeleccionPersonal>();
            try
            {
                InicializarMensaje(null);
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = id
                };
                return View(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> CreateInstrucionFormal(int id)
        {
            try
            {
                InicializarMensaje(null);
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = id
                };
                return View(usuario);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexExperiencia(int id)
        {
            ///InicializarMensaje(mensaje);
            //var lista = new List<ViewModelSeleccionPersonal>();
            try
            {
                InicializarMensaje(null);
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = id
                };
                return View(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> CreateExperiencia(int id)
        {
            try
            {
                InicializarMensaje(null);
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = id
                };
                return View(usuario);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}