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
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class FlujosAprobacionController : Controller
    {
        private readonly IApiServicio apiServicio;


        public FlujosAprobacionController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }
        


        public async Task<IActionResult> Index()
        {

            var lista = new List<FlujoAprobacion>();
            try
            {
                lista = await apiServicio.Listar<FlujoAprobacion>(
                    new Uri(WebApp.BaseAddress), "api/FlujosAprobacion/ListarFlujosAprobacion");
                
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
                                                                  "api/FlujosAprobacion");


                    var modelo = JsonConvert.DeserializeObject<FlujoAprobacion>(respuesta.Resultado.ToString());

                    
                    if (respuesta.IsSuccess)
                    {
                        
                        await InicializarCombos();


                        var filtro = new IdFiltrosViewModel { IdSucursal = modelo.IdSucursal };

                        var lista = await apiServicio.ObtenerElementoAsync1<List<ManualPuesto>>(
                            filtro,
                            new Uri(WebApp.BaseAddress),
                            "api/ManualPuestos/ListarManualPuestoPorSucursal");

                        ViewData["ManualPuesto"] = new SelectList(lista, "IdManualPuesto", "Nombre");



                        return View(modelo);
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
        public async Task<IActionResult> Edit(string id, FlujoAprobacion FlujoAprobacion)
        {
            Response response = new Response();
            try
            {
                if (!ModelState.IsValid)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"10000"}";

                    // Se vuelven a cargar los combos para generar la vista
                    await InicializarCombos();
                    var filtro = new IdFiltrosViewModel { IdSucursal = FlujoAprobacion.IdSucursal };

                    var lista = await apiServicio.ObtenerElementoAsync1<List<ManualPuesto>>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/ManualPuestos/ListarManualPuestoPorSucursal");

                    ViewData["ManualPuesto"] = new SelectList(lista, "IdManualPuesto", "Nombre");


                    return View(FlujoAprobacion);
                }


                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, FlujoAprobacion, new Uri(WebApp.BaseAddress),
                                                                 "api/FlujosAprobacion");

                    if (response.IsSuccess)
                    {
                        return this.Redireccionar(
                            "FlujosAprobacion",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}"
                         );
                        
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                    // Se vuelven a cargar los combos para generar la vista
                    await InicializarCombos();
                    var filtro = new IdFiltrosViewModel { IdSucursal = FlujoAprobacion.IdSucursal };

                    var lista = await apiServicio.ObtenerElementoAsync1<List<ManualPuesto>>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/ManualPuestos/ListarManualPuestoPorSucursal");

                    ViewData["ManualPuesto"] = new SelectList(lista, "IdManualPuesto", "Nombre");


                    return View(FlujoAprobacion);

                }
                
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        

        public async Task<IActionResult> Create()
        {

            await InicializarCombos();

            return View(new FlujoAprobacion());
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlujoAprobacion FlujoAprobacion)
        {
            
            try
            {
                Response response = new Response();

                if (!ModelState.IsValid)
                {

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"10000"}";
                    await InicializarCombos();

                    if (FlujoAprobacion.IdSucursal > 0) {

                        var filtro = new IdFiltrosViewModel { IdSucursal = FlujoAprobacion.IdSucursal };

                        var lista = await apiServicio.ObtenerElementoAsync1<List<ManualPuesto>>(
                            filtro,
                            new Uri(WebApp.BaseAddress),
                            "api/ManualPuestos/ListarManualPuestoPorSucursal");

                        ViewData["ManualPuesto"] = new SelectList(lista, "IdManualPuesto", "Nombre");
                    }
                    

                    return View(FlujoAprobacion);

                }

                response = await apiServicio.InsertarAsync(FlujoAprobacion,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FlujosAprobacion/InsertarFlujoAprobacion");
                if (response.IsSuccess)
                {
                    return this.Redireccionar(
                            "FlujosAprobacion",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}"
                         );
                }


                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                await InicializarCombos();

                if (FlujoAprobacion.IdSucursal > 0)
                {

                    var filtro = new IdFiltrosViewModel { IdSucursal = FlujoAprobacion.IdSucursal };

                    var lista = await apiServicio.ObtenerElementoAsync1<List<ManualPuesto>>(
                        filtro,
                        new Uri(WebApp.BaseAddress),
                        "api/ManualPuestos/ListarManualPuestoPorSucursal");

                    ViewData["ManualPuesto"] = new SelectList(lista, "IdManualPuesto", "Nombre");
                }

                return View(FlujoAprobacion);

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
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/FlujosAprobacion");
                if (response.IsSuccess)
                {

                    return this.Redireccionar(
                            "FlujosAprobacion",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}"
                         );
                }

                return this.Redireccionar(
                            "FlujosAprobacion",
                            "Index",
                            $"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}"
                         );

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
        


        public async Task InicializarCombos()
        {

            try
            {
                // Combo de sucursales
                var listaSucursal = await apiServicio.Listar<Sucursal>(
                    new Uri(WebApp.BaseAddress),
                    "api/Sucursal/ListarSucursal");
                
                ViewData["Sucursal"] = new SelectList(listaSucursal, "IdSucursal", "Nombre");


                // Combo de sucursales
                var listaTipoAccionPersonal = await apiServicio.Listar<TipoAccionPersonal>(
                    new Uri(WebApp.BaseAddress),
                    "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

                ViewData["TipoAccionPersonal"] = new SelectList(listaTipoAccionPersonal, "IdTipoAccionPersonal", "Nombre");
                

            }
            catch (Exception ex)
            {

            }

        }

        public async Task<IActionResult> ObtenerManualPuestosPorSucursal(int IdSucursal)
        {

            var lista = new List<ManualPuesto>();

            try
            {
                var filtro = new IdFiltrosViewModel { IdSucursal = IdSucursal };

                lista = await apiServicio.ObtenerElementoAsync1<List<ManualPuesto>>(
                    filtro,
                    new Uri(WebApp.BaseAddress),
                    "api/ManualPuestos/ListarManualPuestoPorSucursal");

                return Json(lista);
            }
            catch (Exception)
            {

                return Json(lista);
            }

        }


    }
}