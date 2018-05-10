using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class MovimientosPersonalTTHHController : Controller
    {
        private readonly IApiServicio apiServicio;


        public MovimientosPersonalTTHHController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        
        


        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> ListaMovimientos(string identificacion,int Empty)
        {
            if ( string.IsNullOrEmpty(identificacion) ) {
                return RedirectToAction("Index");
            }
            return await ListaMovimientos(identificacion + " ");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListaMovimientos(string identificacion)
        {

            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var modeloEnviar = new AccionesPersonalPorEmpleadoViewModel
                    {

                        DatosBasicosEmpleadoViewModel = new DatosBasicosEmpleadoViewModel
                        {
                            Identificacion = identificacion
                        },

                        NombreUsuarioActual = NombreUsuario
                    };


                    var modelo = await apiServicio.ObtenerElementoAsync1<AccionesPersonalPorEmpleadoViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/ListarAccionesPersonalPorEmpleado");

                    if (modelo.DatosBasicosEmpleadoViewModel.IdEmpleado == 0) {
                        
                        return this.Redireccionar(
                            "MovimientosPersonalTTHH",
                            "Index",
                            $"{Mensaje.Error}|{Mensaje.RegistroNoEncontrado}"
                         );
                    }

                    return View("ListaMovimientos", modelo);

                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }



        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var datosBasicosEmpleado = new DatosBasicosEmpleadoSinRequiredViewModel { IdEmpleado = id };

                var respuesta = await apiServicio.ObtenerElementoAsync <DatosBasicosEmpleadoSinRequiredViewModel>(
                    datosBasicosEmpleado,
                    new Uri(WebApp.BaseAddress),
                    "api/Empleados/ObtenerDatosBasicosEmpleado");

                if (respuesta.IsSuccess)
                {
                    datosBasicosEmpleado = JsonConvert.DeserializeObject<DatosBasicosEmpleadoSinRequiredViewModel>(respuesta.Resultado.ToString());


                    var situacionActualViewModel = new SituacionActualEmpleadoViewModel { IdEmpleado = datosBasicosEmpleado.IdEmpleado };

                    var situacionActualEmpleadoViewModelResponse = await apiServicio.ObtenerElementoAsync<SituacionActualEmpleadoViewModel>(situacionActualViewModel, new Uri(WebApp.BaseAddress),
                    "api/Empleados/ObtenerSituacionActualEmpleadoViewModel");

                    if (respuesta.IsSuccess)
                    {
                        situacionActualViewModel = JsonConvert.DeserializeObject<SituacionActualEmpleadoViewModel>(situacionActualEmpleadoViewModelResponse.Resultado.ToString());
                    }

                    
                    var listaIOMP = await apiServicio.Listar<IndicesOcupacionalesModalidadPartidaViewModel>(
                    new Uri(WebApp.BaseAddress),
                    "api/IndicesOcupacionalesModalidadPartida/ListarIndicesOcupacionalesModalidadPartidaViewModel");

                    var model = new AccionPersonalViewModel
                    {
                        DatosBasicosEmpleadoViewModel = datosBasicosEmpleado,
                        Numero = "0",
                        Fecha = DateTime.Now,
                        FechaRige = DateTime.Now,
                        FechaRigeHasta = DateTime.Now,
                        SituacionActualEmpleadoViewModel = situacionActualViewModel,
                        GeneraMovimientoPersonal = false,
                        ListaIndicesOcupacionalesModalidadPartida = listaIOMP
                    };


                    await InicializarCombos();
                    

                    return View(model);

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
        public async Task<IActionResult> Create(AccionPersonalViewModel accionPersonalViewModel)
        {
            try {

                if (!ModelState.IsValid)
                {
                    await InicializarCombos();

                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}";

                    var listaIOMP = await apiServicio.Listar<IndicesOcupacionalesModalidadPartidaViewModel>(
                    new Uri(WebApp.BaseAddress),
                    "api/IndicesOcupacionalesModalidadPartida/ListarIndicesOcupacionalesModalidadPartidaViewModel");

                    accionPersonalViewModel.ListaIndicesOcupacionalesModalidadPartida = listaIOMP;

                    return View(accionPersonalViewModel);
                }

                if (accionPersonalViewModel.GeneraMovimientoPersonal == true) {

                    if (accionPersonalViewModel.IdIndiceOcupacionalModalidadPartidaPropuesta < 1) {

                        this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SeleccioneIndice}|{"10000"}";

                        await InicializarCombos();
                        
                        var listaIOMP = await apiServicio.Listar<IndicesOcupacionalesModalidadPartidaViewModel>(
                    new Uri(WebApp.BaseAddress),
                    "api/IndicesOcupacionalesModalidadPartida/ListarIndicesOcupacionalesModalidadPartidaViewModel");

                        accionPersonalViewModel.ListaIndicesOcupacionalesModalidadPartida = listaIOMP;

                        return View(accionPersonalViewModel);
                    }
                    

                }

                var respuesta = await apiServicio.InsertarAsync<AccionesPersonalPorEmpleadoViewModel>(
                            accionPersonalViewModel,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/InsertarAccionPersonal");
                

                if (respuesta.IsSuccess) {

                    return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "ListaMovimientos",
                             new { identificacion = respuesta.Resultado },
                            $"{Mensaje.Success}|{respuesta.Message}|{"7000"}"
                         );
                }

                return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "ListaMovimientos",
                             new { identificacion = respuesta.Resultado },
                            $"{Mensaje.Error}|{respuesta.Message}|{"10000"}"
                         );

            } catch (Exception){
                return BadRequest();
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            try{

                var modelo = new AccionPersonalViewModel { IdAccionPersonal = id };

                var respuesta = await apiServicio.ObtenerElementoAsync<AccionPersonalViewModel>(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/ObtenerAccionPersonalViewModel");


                if (respuesta.IsSuccess)
                {
                    modelo = JsonConvert.DeserializeObject<AccionPersonalViewModel>(respuesta.Resultado.ToString());

                    await InicializarCombos();


                    var situacionActualViewModel = new SituacionActualEmpleadoViewModel { IdEmpleado = modelo.DatosBasicosEmpleadoViewModel.IdEmpleado };

                    var situacionActualEmpleadoViewModelResponse = await apiServicio.ObtenerElementoAsync<SituacionActualEmpleadoViewModel>(situacionActualViewModel, new Uri(WebApp.BaseAddress),
                    "api/Empleados/ObtenerSituacionActualEmpleadoViewModel");

                    if (respuesta.IsSuccess)
                    {
                        situacionActualViewModel = JsonConvert.DeserializeObject<SituacionActualEmpleadoViewModel>(situacionActualEmpleadoViewModelResponse.Resultado.ToString());
                    }

                    modelo.SituacionActualEmpleadoViewModel = situacionActualViewModel;

                    var listaIOMP = await apiServicio.Listar<IndicesOcupacionalesModalidadPartidaViewModel>(
                    new Uri(WebApp.BaseAddress),
                    "api/IndicesOcupacionalesModalidadPartida/ListarIndicesOcupacionalesModalidadPartidaViewModel");

                    modelo.ListaIndicesOcupacionalesModalidadPartida = listaIOMP;

                    return View(modelo);

                }

                return BadRequest();

            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccionPersonalViewModel accionPersonalViewModel)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    await InicializarCombos();
                    
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}";
                    return View(accionPersonalViewModel);
                }


                if (accionPersonalViewModel.GeneraMovimientoPersonal == true)
                {

                    if (accionPersonalViewModel.IdIndiceOcupacionalModalidadPartidaPropuesta < 1)
                    {

                        this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.SeleccioneIndice}|{"10000"}";

                        await InicializarCombos();

                        var listaIOMP = await apiServicio.Listar<IndicesOcupacionalesModalidadPartidaViewModel>(
                    new Uri(WebApp.BaseAddress),
                    "api/IndicesOcupacionalesModalidadPartida/ListarIndicesOcupacionalesModalidadPartidaViewModel");

                        accionPersonalViewModel.ListaIndicesOcupacionalesModalidadPartida = listaIOMP;

                        return View(accionPersonalViewModel);
                    }


                }

                /*
                var modeloEnviar = new AccionPersonal
                {
                    IdAccionPersonal = accionPersonalViewModel.IdAccionPersonal,
                    IdEmpleado = accionPersonalViewModel.DatosBasicosEmpleadoViewModel.IdEmpleado,
                    Fecha = accionPersonalViewModel.Fecha,
                    FechaRige = accionPersonalViewModel.FechaRige,
                    FechaRigeHasta = accionPersonalViewModel.FechaRigeHasta,
                    Estado = accionPersonalViewModel.Estado,
                    Explicacion = accionPersonalViewModel.Explicacion,
                    NoDias = accionPersonalViewModel.NoDias,
                    Numero = accionPersonalViewModel.Numero,
                    Solicitud = accionPersonalViewModel.Solicitud,
                    IdTipoAccionPersonal = accionPersonalViewModel.TipoAccionPersonalViewModel.IdTipoAccionPersonal

                };
                */

                var respuesta = await apiServicio.EditarAsync<AccionesPersonalPorEmpleadoViewModel>(
                            accionPersonalViewModel,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/EditarAccionPersonalTTHH");

                if (respuesta.IsSuccess)
                {
                    
                    return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "ListaMovimientos",
                             new { identificacion = respuesta.Resultado },
                            $"{Mensaje.Success}|{respuesta.Message}|{"7000"}"
                         );

                }

                return View(accionPersonalViewModel);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> Visualizar(string mensaje, int id)
        {
            try
            {

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{mensaje}";

                var modelo = new AccionPersonalViewModel { IdAccionPersonal = id };

                var respuesta = await apiServicio.ObtenerElementoAsync<AccionPersonalViewModel>(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/ObtenerAccionPersonalViewModel");

                if (respuesta.IsSuccess)
                {
                    modelo = JsonConvert.DeserializeObject<AccionPersonalViewModel>(respuesta.Resultado.ToString());


                    var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

                    ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");



                    var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacion");

                    ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");


                    return View(modelo);

                }

                return BadRequest();

            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }


        public async Task<ActionResult> CargarDependencias(int idSucursal)
        {
            try
            {
                var sucursal = new Sucursal()
                {
                    IdSucursal = idSucursal
                };
                var dependenciasporsucursal = await apiServicio.ObtenerElementoAsync1<List<Dependencia>>(sucursal, new Uri(WebApp.BaseAddress)
                                                                      , "/api/Dependencias/ListarDependenciaporSucursal");

                //InicializarMensaje(mensaje);
                return Json(dependenciasporsucursal);

            } catch (Exception ex) {
                return Json(new List<Dependencia>());
            }
        }


        public async Task<ActionResult> CargarRoles(int idDependencia)
        {
            try
            {
                var dependencia = new Dependencia()
                {
                    IdDependencia = idDependencia
                };
                var listaManualPuesto = await apiServicio.ObtenerElementoAsync1<List<ManualPuesto>>(
                         dependencia, new Uri(WebApp.BaseAddress), "api/ManualPuestos/ListarManualPuestoPorDependencia");
                

                //InicializarMensaje(mensaje);
                return Json(listaManualPuesto);

            }
            catch (Exception ex)
            {
                return Json(new List<Dependencia>());
            }
        }
        
        public async Task<ActionResult> CargarRMU(int idCargo, int idDependencia)
        {
            try
            {
                var filtro = new IdFiltrosViewModel()
                {
                    IdManualPuesto = idCargo,
                    IdDependencia = idDependencia
                };
                var lista = await apiServicio.ObtenerElementoAsync1<List<EscalaGrados>>(filtro, new Uri(WebApp.BaseAddress), "/api/EscalasGrados/ListarEscalaPorManualPuesto");

                var firstRMU = new List<EscalaGrados>();

                if (lista.Count > 0)
                {
                    firstRMU.Add(lista.FirstOrDefault());
                }
                

                //InicializarMensaje(mensaje);
                return Json(firstRMU);

            }
            catch (Exception ex)
            {
                return Json(new List<Dependencia>());
            }
        }


        public async Task InicializarCombos()
        {
            // Carga de listas para combos

            // ** Tipos de acciones
            var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

            ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");


            //** Estados de aprobación
            var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacionTTHH");

            ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");
            
        }


        public async Task<IActionResult> ListarModalidadPartida(string mensaje)
        {
            this.TempData["Mensaje"] = $"{Mensaje.Aviso}|{"Seleccione para generar la propuesta"}";
            try {

                var lista= await apiServicio.Listar<IndicesOcupacionalesModalidadPartidaViewModel>(
                    new Uri(WebApp.BaseAddress),
                    "api/IndicesOcupacionalesModalidadPartida/ListarIndicesOcupacionalesModalidadPartidaViewModel");

                return View(lista);

            } catch (Exception ex)
            {
                return BadRequest();
            }
        }
        


    }
}