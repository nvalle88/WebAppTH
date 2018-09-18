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

            var lista = new List<DistributivoSituacionActual>();
            try
            {
                lista = await apiServicio.Listar<DistributivoSituacionActual>(
                    new Uri(WebApp.BaseAddress)
                    , "api/Distributivos/ObtenerDistributivoReal");

                
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        public async Task<IActionResult> ListaMovimientos(int IdEmpleado,int Empty)
        {
            
            return await ListaMovimientos(IdEmpleado);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListaMovimientos(int IdEmpleado)
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
                            IdEmpleado = IdEmpleado
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

                return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "Index",
                            $"{Mensaje.Error}|{Mensaje.ErrorReingresarIdentificacion}|{"7000"}"
                    );
            }
        }


        #region Métodos para mantenimiento

        public async Task<IActionResult> Create(int id)
        {
            try
            {

                var respuesta = await apiServicio.ObtenerElementoAsync1 <Response>(
                    id,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/ObtenerNuevaAccionPersonalViewModel");

                if (respuesta.IsSuccess)
                {
                    var modelo = JsonConvert.DeserializeObject<AccionPersonalViewModel>(respuesta.Resultado.ToString());
                    
                    await InicializarCombos();
                    

                    return View(modelo);

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
            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    accionPersonalViewModel.NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                }
                else {
                    return RedirectToAction("Login", "Login");
                }
                    
                 

                var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                    accionPersonalViewModel,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/InsertarAccionPersonal");

                if (respuesta.IsSuccess)
                {


                    return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "ListaMovimientos",
                             new { IdEmpleado = respuesta.Resultado },
                            $"{Mensaje.Success}|{respuesta.Message}|{"7000"}"
                    );


                }

                return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "ListaMovimientos",
                             new { IdEmpleado = respuesta.Resultado },
                            $"{Mensaje.Error}|{respuesta.Message}|{"7000"}"
                    );

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var IdAccionPersonal = id;

                var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                    IdAccionPersonal,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/ObtenerAccionPersonalViewModelRegistrada");

                if (respuesta.IsSuccess)
                {
                    var modelo = JsonConvert.DeserializeObject<AccionPersonalViewModel>(respuesta.Resultado.ToString());

                    await InicializarCombos();


                    return View(modelo);

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
        public async Task<IActionResult> Edit(AccionPersonalViewModel accionPersonalViewModel)
        {
            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    accionPersonalViewModel.NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }



                var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                    accionPersonalViewModel,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/EditarAccionPersonal");

                if (respuesta.IsSuccess)
                {


                    return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "ListaMovimientos",
                             new { IdEmpleado = respuesta.Resultado },
                            $"{Mensaje.Success}|{respuesta.Message}|{"7000"}"
                    );


                }

                return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "ListaMovimientos",
                             new { IdEmpleado = respuesta.Resultado },
                            $"{Mensaje.Error}|{respuesta.Message}|{"7000"}"
                    );

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        #endregion






        /*
        


        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                int IdEmpleado = id;

                var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                    IdEmpleado,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/ObtenerAccionPersonalViewModelParaEditar");

                if (respuesta.IsSuccess)
                {
                    var modelo = JsonConvert.DeserializeObject<AccionPersonalViewModel>(respuesta.Resultado.ToString());

                    await InicializarCombos();


                    if (modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta != null) {

                        await CargarRelacionLaboralPorRegimen(modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta.TipoNombramiento.RelacionLaboral.IdRegimenLaboral);

                        await CargarTipoNombramientoPorRelacion(modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta.TipoNombramiento.IdRelacionLaboral);

                        await CargarSucursalesPorCiudad(modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta.IndiceOcupacional.Dependencia.Sucursal.IdCiudad);

                        await CargarPerfilPuestoPorDependencia(modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta
                            .IndiceOcupacional.Dependencia.IdDependencia,
                            modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta
                            .IndiceOcupacional.IdManualPuesto
                              );


                    }


                    return View(modelo);

                }

                return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "ListaMovimientos",
                             new { identificacion = respuesta.Resultado },
                            $"{Mensaje.Error}|{respuesta.Message}|{"7000"}"
                    );

            }
            catch (Exception ex)
            {
                return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "Index",
                            $"{Mensaje.Error}|{Mensaje.ErrorReingresarIdentificacion}|{"7000"}"
                    );
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccionPersonalViewModel accionPersonalViewModel)
        {
            try
            {

                var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                    accionPersonalViewModel,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/EditarAccionPersonal");

                if (respuesta.IsSuccess)
                {


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
                            $"{Mensaje.Error}|{respuesta.Message}|{"7000"}"
                    );

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> Visualizar(int id)
        {
            try
            {

                var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                    id,
                    new Uri(WebApp.BaseAddress),
                    "api/AccionesPersonal/ObtenerAccionPersonalViewModelParaVisualizar");

                if (respuesta.IsSuccess)
                {
                    var modelo = JsonConvert.DeserializeObject<AccionPersonalViewModel>(respuesta.Resultado.ToString());

                    await InicializarCombosVisualizar();


                    if (modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta != null)
                    {

                        await CargarRelacionLaboralPorRegimen(modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta.TipoNombramiento.RelacionLaboral.IdRegimenLaboral);

                        await CargarTipoNombramientoPorRelacion(modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta.TipoNombramiento.IdRelacionLaboral);

                        await CargarSucursalesPorCiudad(modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta.IndiceOcupacional.Dependencia.Sucursal.IdCiudad);

                        await CargarPerfilPuestoPorDependencia(modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta
                            .IndiceOcupacional.Dependencia.IdDependencia,
                            modelo.EmpleadoMovimiento.IndiceOcupacionalModalidadPartidaHasta
                            .IndiceOcupacional.IdManualPuesto
                              );


                    }


                    return View(modelo);

                }

                return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "ListaMovimientos",
                             new { identificacion = respuesta.Resultado },
                            $"{Mensaje.Error}|{respuesta.Message}|{"7000"}"
                    );

            }
            catch (Exception ex)
            {
                return this.RedireccionarMensajeTime(
                            "MovimientosPersonalTTHH",
                            "Index",
                            $"{Mensaje.Error}|{Mensaje.ErrorReingresarIdentificacion}|{"7000"}"
                    );
            }

        }



       

        

        public async Task InicializarCombosVisualizar()
        {
            // Carga de listas para combos

            // ** Tipos de acciones
            var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

            ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");



            //** Estados de aprobación
            var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosAprobacionAprobador");

            ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");




            ViewData["IdRegimenLaboral"] = new SelectList(await apiServicio.Listar<RegimenLaboral>(new Uri(WebApp.BaseAddress), "api/RegimenesLaborales/ListarRegimenesLaborales"), "IdRegimenLaboral", "Nombre");


            ViewData["IdFondoFinanciamiento"] = new SelectList(await apiServicio.Listar<FondoFinanciamiento>(new Uri(WebApp.BaseAddressRM), "/api/FondoFinanciamiento/ListarFondoFinanciamiento"), "IdFondoFinanciamiento", "Nombre");


            ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");


            ViewData["IdModalidadPartida"] = new SelectList(await apiServicio.Listar<ModalidadPartida>(new Uri(WebApp.BaseAddress), "api/ModalidadesPartida/ListarModalidadesPartida"), "IdModalidadPartida", "Nombre");

        }

        public async Task<JsonResult> ListarRelacionesLaboralesPorRegimen(int regimen)
        {
            try
            {
                var regimenLaboral = new RegimenLaboral
                {
                    IdRegimenLaboral = regimen,
                };
                var listarelacionesLaborales = await apiServicio.Listar<RelacionLaboral>(regimenLaboral, new Uri(WebApp.BaseAddress), "api/RelacionesLaborales/ListarRelacionesLaboralesPorRegimen");
                return Json(listarelacionesLaborales);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        

        public async Task<JsonResult> ListarManualPuestoporDependencia(int iddependencia, int idRelacionLaboral)
        {
            try
            {

                var indiceOcupacional = new IndiceOcupacional
                {
                    IdDependencia = iddependencia,
                    IdRelacionLaboral = idRelacionLaboral
                };
                var listarmanualespuestos = await apiServicio.Listar<IndicesOcupacionalesModalidadPartidaViewModel>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/Empleados/ListarManualPuestoporDependenciaYRelacionLaboral");
                return Json(listarmanualespuestos);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> ListarRolPuestoporManualPuesto(int idmanualpuesto, int iddependencia, int idrelacionLaboral)
        {
            try
            {
                var indiceOcupacional = new IndiceOcupacional
                {
                    IdManualPuesto = idmanualpuesto,
                    IdDependencia = iddependencia,
                    IdRelacionLaboral = idrelacionLaboral
                };

                var listarrolespuestos = await apiServicio.ObtenerElementoAsync1<IndicesOcupacionalesModalidadPartidaViewModel>(
                    indiceOcupacional,
                    new Uri(WebApp.BaseAddress),
                    "api/Empleados/ListarRolPuestoporManualPuestoYRelacionLaboral"
                );

                return Json(listarrolespuestos);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }


        public async Task<ActionResult> CargarDependencias(int idsucursal)
        {
            var sucursal = new Sucursal()
            {
                IdSucursal = idsucursal
            };
            var dependenciasporsucursal = await apiServicio.ObtenerElementoAsync1<Dependencia>(sucursal, new Uri(WebApp.BaseAddress)
                                                                  , "api/Dependencias/ListarDependenciaporSucursalPadreHijo");


            //InicializarMensaje(mensaje);
            return PartialView(dependenciasporsucursal);

        }



        // para la edicion carga de combos
        public async Task CargarRelacionLaboralPorRegimen(int IdRegimenLaboral)
        {
            try
            {
                var regimenLaboral = new RegimenLaboral
                {
                    IdRegimenLaboral = IdRegimenLaboral,
                };

                var listarelacionesLaborales = await apiServicio.Listar<RelacionLaboral>(regimenLaboral, new Uri(WebApp.BaseAddress), "api/RelacionesLaborales/ListarRelacionesLaboralesPorRegimen");

                ViewData["IdRelacionLaboral"] = new SelectList(listarelacionesLaborales, "IdRelacionLaboral", "Nombre");

            }
            catch (Exception ex) { }
        }

        public async Task CargarTipoNombramientoPorRelacion(int IdRelacionLaboral)
        {
            try
            {
                var relacionLaboral = new RelacionLaboral
                {
                    IdRelacionLaboral = IdRelacionLaboral,
                };
                var listarTipoNombramientos = await apiServicio.Listar<TipoNombramiento>(relacionLaboral, new Uri(WebApp.BaseAddress), "api/TiposDeNombramiento/ListarTiposDeNombramientoPorRelacion");

                ViewData["IdTipoNombramiento"] = new SelectList(listarTipoNombramientos, "IdTipoNombramiento", "Nombre");

            }
            catch (Exception ex) { }
        }

        public async Task CargarSucursalesPorCiudad(int IdCiudad)
        {
            try
            {
                var ciudad = new Ciudad
                {
                    IdCiudad = IdCiudad,
                };
                var listarsucursales = await apiServicio.Listar<Sucursal>(ciudad, new Uri(WebApp.BaseAddress), "api/Sucursal/ListarSucursalporCiudad");

                ViewData["IdSucursal"] = new SelectList(listarsucursales, "IdSucursal", "Nombre");

            }
            catch (Exception ex) { }
        }

        public async Task CargarPerfilPuestoPorDependencia(int IdDependencia, int IdManualPuesto)
        {
            try
            {
                var indiceOcupacional = new IndiceOcupacional
                {
                    IdDependencia = IdDependencia,
                };
                var listarmanualespuestos = await apiServicio.Listar<IndiceOcupacional>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/Empleados/ListarManualPuestoporDependenciaTodosEstados");

                var mostrarLista = new List<ManualPuesto>();

                foreach (var item in listarmanualespuestos)
                {
                    if (item.IdManualPuesto == IdManualPuesto)
                    {
                        mostrarLista.Add(new ManualPuesto { IdManualPuesto = item.ManualPuesto.IdManualPuesto, Nombre = item.ManualPuesto.Nombre });
                        break;
                    }
                }

                ViewData["IdManualPuesto"] = new SelectList(mostrarLista, "IdManualPuesto", "Nombre");

            }
            catch (Exception ex) { }
        }

        */


        // 

        public async Task InicializarCombos()
        {
            // Carga de listas para combos

            // ** Tipos de acciones
            var listaTipoAccionespersonales = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

            ViewData["TipoAcciones"] = new SelectList(listaTipoAccionespersonales, "IdTipoAccionPersonal", "Nombre");



            //** Estados de aprobación
            var listaEstadosAprobacion = await apiServicio.Listar<AprobacionMovimientoInternoViewModel>(new Uri(WebApp.BaseAddress), "api/AccionesPersonal/ListarEstadosProcesoAccionPersonal");

            ViewData["Estados"] = new SelectList(listaEstadosAprobacion, "ValorEstado", "NombreEstado");


            //** ListaPartidasVacantes
            var listaPartidasVacantes = await apiServicio.Listar<IndiceOcupacionalModalidadPartida>(
                new Uri(WebApp.BaseAddress), "api/IndicesOcupacionalesModalidadPartida/ListaPuestosVacantes");

            foreach (var item in listaPartidasVacantes) {
                item.NumeroPartidaIndividual = item.NumeroPartidaIndividual + item.CodigoContrato;
            }

            ViewData["PartidasVacantes"] = new SelectList(
                listaPartidasVacantes, "IdIndiceOcupacionalModalidadPartida", "NumeroPartidaIndividual");


            ViewData["IdModalidadPartida"] = new SelectList(await apiServicio.Listar<ModalidadPartida>(new Uri(WebApp.BaseAddress), "api/ModalidadesPartida/ListarModalidadesPartida"), "IdModalidadPartida", "Nombre");


            ViewData["IdFondoFinanciamiento"] = new SelectList(await apiServicio.Listar<FondoFinanciamiento>(new Uri(WebApp.BaseAddressRM), "/api/FondoFinanciamiento/ListarFondoFinanciamiento"), "IdFondoFinanciamiento", "Nombre");
            
        }

        
        #region Métodos para ajax combos

        public async Task<JsonResult> VerTipoAccion(int idAccion)
        {
            try
            {
                var modeloEnviar = new TipoAccionPersonal()
                {
                    IdTipoAccionPersonal = idAccion
                };
                var modelo = await apiServicio.ObtenerElementoAsync1<TipoAccionPersonal>(
                    modeloEnviar,
                    new Uri(WebApp.BaseAddress),
                    "/api/TiposAccionesPersonales/ObtenerTipoAccionPersonal"
                    );

                return Json(modelo);

            }
            catch (Exception ex)
            {
                return Json(Mensaje.Error);
            }
        }

        public async Task<JsonResult> ObtenerDatosPartida(int id)
        {
            try
            {
                int Id = id;
                var modelo = await apiServicio.ObtenerElementoAsync1<IndiceOcupacionalModalidadPartida>(
                    Id,
                    new Uri(WebApp.BaseAddress),
                    "api/IndicesOcupacionalesModalidadPartida/ObtenerPartidaPorId");

                return Json(modelo);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }
        
        public async Task<JsonResult> ListarTipoNombramientoRelacion(int relacion)
        {
            try
            {
                var relacionLaboral = new RelacionLaboral
                {
                    IdRelacionLaboral = relacion,
                };
                var listarelacionesLaborales = await apiServicio.Listar<TipoNombramiento>(relacionLaboral, new Uri(WebApp.BaseAddress), "api/TiposDeNombramiento/ListarTiposDeNombramientoPorRelacion");
                return Json(listarelacionesLaborales);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        #endregion







    }
}