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

namespace bd.webappth.web.Controllers.MVC
{
    public class MovimientosPersonalTTHHController : Controller
    {
        private readonly IApiServicio apiServicio;


        public MovimientosPersonalTTHHController(IApiServicio apiServicio)
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

            return View();

        }

        public async Task<IActionResult> ListaMovimientos(string identificacion,string mensaje,int num)
        {
            if ( string.IsNullOrEmpty(identificacion) ) {
                return RedirectToAction("Index");
            }
            return await ListaMovimientos(mensaje, identificacion + " ");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListaMovimientos(string mensaje, string identificacion)
        {
            InicializarMensaje(mensaje);

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
                        return RedirectToAction("Index", new { mensaje = Mensaje.RegistroNoEncontrado});
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



        public async Task<IActionResult> Create(string mensaje, int id)
        {
            try
            {
                InicializarMensaje(mensaje);

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



                    var model = new AccionPersonalViewModel
                    {
                        DatosBasicosEmpleadoViewModel = datosBasicosEmpleado,
                        Numero = "0",
                        Fecha = DateTime.Now,
                        FechaRige = DateTime.Now,
                        FechaRigeHasta = DateTime.Now,
                        SituacionActualEmpleadoViewModel = situacionActualViewModel,
                        SituacionPropuestaEmpleadoViewModel = new SituacionActualEmpleadoViewModel()
                        ,
                        GeneraMovimientoPersonal = false
                    };


                    await InicializarCombos(
                        0,
                        model.SituacionPropuestaEmpleadoViewModel.IdDependencia,
                        model.SituacionPropuestaEmpleadoViewModel.IdCargo
                        );



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
                    await InicializarCombos(
                        accionPersonalViewModel.SituacionPropuestaEmpleadoViewModel.IdSucursal,
                        accionPersonalViewModel.SituacionPropuestaEmpleadoViewModel.IdDependencia,
                        accionPersonalViewModel.SituacionPropuestaEmpleadoViewModel.IdCargo
                   );

                    InicializarMensaje(Mensaje.ModeloInvalido);
                    return View(accionPersonalViewModel);
                }

                if (accionPersonalViewModel.GeneraMovimientoPersonal == true) {

                    if (
                        accionPersonalViewModel.SituacionPropuestaEmpleadoViewModel.IdCargo < 1 ||
                        accionPersonalViewModel.SituacionPropuestaEmpleadoViewModel.Remuneracion < 1
                        ) {

                        await InicializarCombos(
                            accionPersonalViewModel.SituacionPropuestaEmpleadoViewModel.IdSucursal,
                            accionPersonalViewModel.SituacionPropuestaEmpleadoViewModel.IdDependencia,
                            accionPersonalViewModel.SituacionPropuestaEmpleadoViewModel.IdCargo
                        );

                        InicializarMensaje(Mensaje.SeleccioneCargo);
                        return View(accionPersonalViewModel);
                    }
                    

                }

                var respuesta = await apiServicio.InsertarAsync<AccionesPersonalPorEmpleadoViewModel>(
                            accionPersonalViewModel,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/InsertarAccionPersonal");
                
                return RedirectToAction("ListaMovimientos", new { identificacion = respuesta.Resultado , mensaje = respuesta.Message});

            } catch (Exception){
                return BadRequest();
            }
        }


        public async Task<IActionResult> Edit(string mensaje, int id)
        {
            try
            {
                InicializarMensaje(mensaje);

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



                    var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacionTTHH");

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccionPersonalViewModel accionPersonalViewModel)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

                    ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");



                    var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacionTTHH");

                    ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");

                    InicializarMensaje(Mensaje.ModeloInvalido);
                    return View(accionPersonalViewModel);
                }


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

                var respuesta = await apiServicio.InsertarAsync<AccionesPersonalPorEmpleadoViewModel>(
                            modeloEnviar,
                            new Uri(WebApp.BaseAddress),
                            "api/AccionesPersonal/EditarAccionPersonalTTHH");

                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("ListaMovimientos", new { identificacion = respuesta.Resultado, mensaje = respuesta.Message });
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
                
                InicializarMensaje(mensaje);

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


        public async Task InicializarCombos( int idSucursal, int idDependencia, int idManualPuesto)
        {
            // Carga de listas para combos

            // ** Tipos de acciones
            var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

            ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");


            //** Estados de aprobación
            var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacionTTHH");

            ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");

            //** Sucursales
            var listaSucursales = await apiServicio.Listar<Sucursal>(new Uri(WebApp.BaseAddress), "api/Sucursal/ListarSucursal");

            ViewData["Sucursales"] = new SelectList(listaSucursales, "IdSucursal", "Nombre");

            if (idSucursal < 1) {
                idSucursal = listaSucursales.FirstOrDefault().IdSucursal;
            }

            //** Dependencias
            var sucursalModel = new Sucursal { IdSucursal =  idSucursal};

            var listaDependencias = await apiServicio.ObtenerElementoAsync1<List<Dependencia>>(
                sucursalModel, new Uri(WebApp.BaseAddress), "api/Dependencias/ListarDependenciaporSucursal");

            ViewData["Dependencia"] = new SelectList(listaDependencias, "IdDependencia", "Nombre");



            //** Roles
            var dependenciaModel = new Dependencia { IdDependencia = 0 };

            if (idDependencia > 0)
            {
                dependenciaModel.IdDependencia = idDependencia;
            }
            else if (listaDependencias.Count > 0)
            {
                dependenciaModel.IdDependencia = listaDependencias.FirstOrDefault().IdDependencia;
            }
            

            var listaManualPuesto = await apiServicio.ObtenerElementoAsync1<List<ManualPuesto>>(
                dependenciaModel, new Uri(WebApp.BaseAddress), "api/ManualPuestos/ListarManualPuestoPorDependencia");

            ViewData["ManualPuesto"] = new SelectList(listaManualPuesto, "IdManualPuesto", "Nombre");



            //** RMU
            var filtroRMU = new IdFiltrosViewModel { IdManualPuesto = 0 };

            if (idManualPuesto > 0)
            {
                filtroRMU.IdManualPuesto = idManualPuesto;
                filtroRMU.IdDependencia = idDependencia;
            }

            else if (listaManualPuesto.Count > 0)
            {
                filtroRMU.IdManualPuesto = listaManualPuesto.FirstOrDefault().IdManualPuesto;
                filtroRMU.IdDependencia = idDependencia;
            }

            var listaRMU = await apiServicio.ObtenerElementoAsync1<List<EscalaGrados>>(
                filtroRMU, new Uri(WebApp.BaseAddress), "api/EscalasGrados/ListarEscalaPorManualPuesto");

            var firstRMU = new List<EscalaGrados>();

            if (listaRMU.Count >0) {
                firstRMU.Add(listaRMU.FirstOrDefault());
            }

            

            ViewData["RMU"] = new SelectList(firstRMU, "IdEscalaGrados", "Remuneracion");
        }
        

    }
}