using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.entidades.ViewModels;
using Newtonsoft.Json;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class DependenciasController : Controller
    {
        private readonly IApiServicio apiServicio;

        public DependenciasController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }
        

        public async Task<IActionResult> Index()
        {
            var lista = new List<DependenciaViewModel>();
            try
            {
                lista = await apiServicio.Listar<DependenciaViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Dependencias/ListarDependencias");
                
                return View(lista);
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
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/Dependencias");


                    var dependencia= JsonConvert.DeserializeObject<Dependencia>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        var respuestaCiudad = await apiServicio.SeleccionarAsync<Response>(dependencia.IdSucursal.ToString(), new Uri(WebApp.BaseAddress),
                                                                  "api/Sucursal");
                        var ciudad = JsonConvert.DeserializeObject<Ciudad>(respuestaCiudad.Resultado.ToString());

                        var dependenciaViewModel = new DependenciaViewModel
                        {
                           IdCiudad = ciudad.IdCiudad,
                           IdDependenciaPadre = dependencia.IdDependenciaPadre.Value,
                           NombreDependencia = dependencia.Nombre,
                           IdSucursal = dependencia.IdSucursal,
                           IdDependencia= dependencia.IdDependencia,
                           IdProceso = dependencia.IdProceso,
                           Codigo = dependencia.Codigo
                        };

                        
                        await CargarListaComboEdit(ciudad.IdCiudad, dependencia.IdSucursal);
                        return View(dependenciaViewModel);
                    }

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
        public async Task<IActionResult> Edit(string id, DependenciaViewModel dependenciaViewModel)
        {
            Response response = new Response();
            try
            {
                if (dependenciaViewModel.IdDependenciaPadre == null)
                {
                    dependenciaViewModel.IdDependenciaPadre = 0;
                }

                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, dependenciaViewModel, new Uri(WebApp.BaseAddress),
                                                                 "api/Dependencias");

                    if (response.IsSuccess)
                    {

                        return this.RedireccionarMensajeTime(
                           "Dependencias",
                           "Index",
                           $"{Mensaje.Success}|{response.Message}|{"7000"}"
                        );
                    }
                    
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                    await CargarListaCombox();
                    return View(dependenciaViewModel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una dependencia",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Create(string mensaje)
        {

            await CargarListaCombox();
            InicializarMensaje(mensaje);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DependenciaViewModel dependenciaViewModel)
        {
            if (!ModelState.IsValid)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"7000"}";
                await CargarListaCombox();
                return View(dependenciaViewModel);
            }

            Response response = new Response();

            try
            {
                if (dependenciaViewModel.IdDependenciaPadre == null)
                {
                    dependenciaViewModel.IdDependenciaPadre = 0;
                }

                if (ModelState.IsValid)
                {

                    response = await apiServicio.InsertarAsync(dependenciaViewModel,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/Dependencias/InsertarDependencia");
                    if (response.IsSuccess)
                    {


                        return this.RedireccionarMensajeTime(
                           "Dependencias",
                           "Index",
                           $"{Mensaje.Success}|{response.Message}|{"7000"}"
                        );
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                
                }

                await CargarListaCombox();
                return View(dependenciaViewModel);

            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }

        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }


        private async Task CargarListaCombox()
        {
      
            ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdProceso"] = new SelectList(await apiServicio.Listar<Proceso>(new Uri(WebApp.BaseAddress), "api/Procesos/ListarProcesos"), "IdProceso", "Nombre");
        }

        private async Task CargarListaComboEdit(int idciudad, int idsucursal)
        {

            var ciudad = new Ciudad
            {
                IdCiudad = idciudad
            };

            var sucursal = new Sucursal
            {
                IdSucursal = idsucursal
            };

            ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdProceso"] = new SelectList(await apiServicio.Listar<Proceso>(new Uri(WebApp.BaseAddress), "api/Procesos/ListarProcesos"), "IdProceso", "Nombre");
            //ViewData["IdSucursal"] = new SelectList(await apiServicio.ObtenerElementoAsync1<Sucursal>(ciudad,new Uri(WebApp.BaseAddress), "api/Sucursal/ListarSucursalporCiudad"), "IdSucursal", "Nombre");
            //ViewData["IdDependencia"] = new SelectList(await apiServicio.Listar<DependenciaViewModel>(new Uri(WebApp.BaseAddress), "api/Dependencias/ListarDependencias"), "IdDependencia", "NombreDependencia");

            var listasucursales = await apiServicio.ObtenerElementoAsync1<List<Sucursal>>(ciudad, new Uri(WebApp.BaseAddress)
                                                                , "/api/Sucursal/ListarSucursalporCiudad");
            ViewData["IdSucursal"] = new SelectList(listasucursales, "IdSucursal", "Nombre");

            var listadependencias = await apiServicio.ObtenerElementoAsync1<List<Dependencia>>(sucursal, new Uri(WebApp.BaseAddress)
                                                                   , "/api/Dependencias/ListarDependenciaporSucursal");
            ViewData["IdDependencia"] = new SelectList(listadependencias, "IdDependencia", "Nombre");
        }




        public async Task<JsonResult> ListarPadresPorSucursal(string idSucursal)
        {
            var sucursal = new Dependencia
            {
                IdSucursal = Convert.ToInt32(idSucursal),
            };
            var listaDependencia = await apiServicio.Listar<Dependencia>(sucursal, new Uri(WebApp.BaseAddress), "api/Dependencias/ListarPadresPorSucursal");
            return Json(listaDependencia);
        }

        public async Task<ActionResult> CargarDependencias(int idsucursal)

        {
            var sucursal = new Sucursal()
            {
                IdSucursal = idsucursal
            };
            var dependenciasporsucursal = await apiServicio.ObtenerElementoAsync1<Dependencia>(sucursal, new Uri(WebApp.BaseAddress)
                                                                  , "/api/Dependencias/ListarDependenciaporSucursalPadreHijo");


            //InicializarMensaje(mensaje);
            return PartialView(dependenciasporsucursal);

        }

        public async Task<JsonResult> RedireccionarModal(int idsucursal)
        {

            try
            {

                return Json(new { result = "Redireccionar", url = Url.Action("CargarDependencias", "Empleados", new { idsucursal = idsucursal, @class = "dialog-window" }) });
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/Dependencias");
                if (response.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                           "Dependencias",
                           "Index",
                           $"{Mensaje.Success}|{response.Message}|{"7000"}"
                        );
                }

                return this.RedireccionarMensajeTime(
                           "Dependencias",
                           "Index",
                           $"{Mensaje.Error}|{response.Message}|{"7000"}"
                        );
            }
            catch (Exception ex)
            {
                

                return BadRequest();
            }
        }
    }
}