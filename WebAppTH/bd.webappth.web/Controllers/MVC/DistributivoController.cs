using bd.log.guardar.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Extensores;
using bd.webappth.servicios.Interfaces;
using EnviarCorreo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SendMails.methods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bd.webappth.web.Controllers.MVC
{

    public class DistributivoController : Controller
    {

        public class ObtenerInstancia
        {
            private static EmpleadoViewModel instance;

            private ObtenerInstancia() { }

            public static EmpleadoViewModel Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new EmpleadoViewModel();
                        instance.Persona = new Persona();
                        instance.Empleado = new Empleado();
                        instance.DatosBancarios = new DatosBancarios();
                        instance.EmpleadoContactoEmergencia = new EmpleadoContactoEmergencia();
                        instance.IndiceOcupacionalModalidadPartida = new IndiceOcupacionalModalidadPartida();
                        instance.PersonaEstudio = new List<PersonaEstudio>();
                        instance.TrayectoriaLaboral = new List<TrayectoriaLaboral>();
                        instance.PersonaDiscapacidad = new List<PersonaDiscapacidad>();
                        instance.PersonaEnfermedad = new List<PersonaEnfermedad>();
                        instance.PersonaSustituto = new PersonaSustituto();
                        instance.DiscapacidadSustituto = new List<DiscapacidadSustituto>();
                        instance.EnfermedadSustituto = new List<EnfermedadSustituto>();
                        instance.EmpleadoFamiliar = new List<EmpleadoFamiliar>();

                    }
                    return instance;
                }
                set
                {
                    instance = null;
                }
            }
        }

        private readonly IApiServicio apiServicio;


        public DistributivoController(IApiServicio apiServicio)
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


        public async Task<IActionResult> Index()
        {

            var lista = new List<ListaEmpleadoViewModel>();
            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    lista = await apiServicio.ObtenerElementoAsync1<List<ListaEmpleadoViewModel>>(
                            NombreUsuario
                            , new Uri(WebApp.BaseAddress)
                            , "api/Empleados/ListarDistributivo");

                    InicializarMensaje(null);
                    return View(lista);
                }

                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> AgregarDistributivo(int IdEmpleado)
        {

            try
            {
                await CargarCombosDistributivo();



                var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                    IdEmpleado,
                    new Uri(WebApp.BaseAddress),
                    "/api/Empleados/ObtenerEmpleadoDistributivo"
                );



                var empleado = new Empleado
                {
                    IdEmpleado = IdEmpleado
                };


                var empleadoViewModel = new EmpleadoViewModel()
                {
                    Empleado = empleado
                };


                if (respuesta.IsSuccess == true)
                {

                    empleadoViewModel = JsonConvert.DeserializeObject<EmpleadoViewModel>(respuesta.Resultado.ToString());

                    if (
                        empleadoViewModel.IndiceOcupacionalModalidadPartida != null
                        && empleadoViewModel.IndiceOcupacionalModalidadPartida.IdIndiceOcupacionalModalidadPartida > 0
                        )
                    {

                        await CargarRelacionLaboralPorRegimen(empleadoViewModel.IndiceOcupacionalModalidadPartida.TipoNombramiento.RelacionLaboral.IdRegimenLaboral);

                        await CargarTipoNombramientoPorRelacion
                            (empleadoViewModel.IndiceOcupacionalModalidadPartida.TipoNombramiento.IdRelacionLaboral);

                        await CargarSucursalesPorCiudad(empleadoViewModel.IndiceOcupacionalModalidadPartida.IndiceOcupacional.Dependencia.Sucursal.IdCiudad);

                        await CargarPerfilPuestoPorDependencia(
                                empleadoViewModel.IndiceOcupacionalModalidadPartida.IndiceOcupacional.IdDependencia,
                                empleadoViewModel.IndiceOcupacional.IdManualPuesto
                              );
                        
                        await CargarFondoFinanciamento(
                                (int)(empleadoViewModel.IndiceOcupacionalModalidadPartida.IdFondoFinanciamiento)
                            );

                    }
                }


                return View(empleadoViewModel);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        private async Task CargarCombosDistributivo()
        {

            ViewData["IdRegimenLaboral"] = new SelectList(await apiServicio.Listar<RegimenLaboral>(new Uri(WebApp.BaseAddress), "api/RegimenesLaborales/ListarRegimenesLaborales"), "IdRegimenLaboral", "Nombre");
            ViewData["IdFondoFinanciamiento"] = new SelectList(await apiServicio.Listar<FondoFinanciamiento>(new Uri(WebApp.BaseAddressRM), "/api/FondoFinanciamiento/ListarFondoFinanciamiento"), "IdFondoFinanciamiento", "Nombre");
            ViewData["IdDependencia"] = new SelectList(await apiServicio.Listar<Dependencia>(new Uri(WebApp.BaseAddress), "api/Dependencias/ListarDependencias"), "IdDependencia", "Nombre");
            ViewData["IdCiudad"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdModalidadPartida"] = new SelectList(await apiServicio.Listar<ModalidadPartida>(new Uri(WebApp.BaseAddress), "api/ModalidadesPartida/ListarModalidadesPartida"), "IdModalidadPartida", "Nombre");

        }

        public async Task CargarFondoFinanciamento(int IdFondoFinanciamiento)
        {
            try
            {

                var lista = await apiServicio.Listar<FondoFinanciamiento>(
                    new Uri(WebApp.BaseAddressRM), "api/FondoFinanciamiento/ListarFondoFinanciamiento");

                ViewData["IdFondoFinanciamiento"] = new SelectList(lista, "IdFondoFinanciamiento", "Nombre");

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
    }
}