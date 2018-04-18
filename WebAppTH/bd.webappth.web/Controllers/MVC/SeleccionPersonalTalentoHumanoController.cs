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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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
        public async Task<IActionResult> Create(int id, int partida)
        {
            try
            {
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = id,
                    IdPrtidaFase = partida
                };
                var postular = await obtenercabecera(usuario);
                if (postular != null)
                {
                    InicializarMensaje(null);
                    return View(postular);
                }

                ViewData["Error"] = postular.ToString();
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
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                var postular = await obtenercabecera(viewModelSeleccionPersonal);
                return View(postular);
            }
            else
            {
                var response = await apiServicio.InsertarAsync(viewModelSeleccionPersonal, new Uri(WebApp.BaseAddress), "api/SeleccionPersonalTalentoHumano/InsertarPostulante");
                if (response.IsSuccess)
                {
                    var empleado = JsonConvert.DeserializeObject<Empleado>(response.Resultado.ToString());
                    //return RedirectToAction("AgregarDistributivo", new { IdEmpleado = empleado.IdEmpleado });
                    return View(empleado);
                }
                ViewData["Error"] = Mensaje.ExisteEmpleado;
                return View(viewModelSeleccionPersonal);
            }

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
            try
            {
                InicializarMensaje(null);
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = id
                };
                var empleado = await ObtenerEmpleado();

                usuario.ListasPersonaEstudio = await apiServicio.ObtenerElementoAsync1<List<PersonaEstudio>>(empleado, new Uri(WebApp.BaseAddress)
                                                                     , "api/PersonasEstudios/ListarEstudiosporEmpleado");

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
                ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
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
        public Task<Empleado> ObtenerEmpleado()
        {
            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var empleadoJson = ObtenerEmpleadoLogueado(NombreUsuario);
            return empleadoJson;
        }
        public async Task<Empleado> ObtenerEmpleadoLogueado(string nombreUsuario)
        {
            try
            {
                var empleado = new Empleado
                {
                    NombreUsuario = nombreUsuario,
                };
                var usuariologueado = await apiServicio.ObtenerElementoAsync1<Empleado>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerEmpleadoLogueado");
                return usuariologueado;
            }
            catch (Exception)
            {
                return new Empleado();
            }
        }
        public async Task<ViewModelSeleccionPersonal> obtenercabecera(ViewModelSeleccionPersonal viewModelSeleccionPersonal)
        {
            var response = await apiServicio.ObtenerElementoAsync1<ViewModelSeleccionPersonal>(viewModelSeleccionPersonal, new Uri(WebApp.BaseAddress)
                                                                   , "api/SeleccionPersonalTalentoHumano/ObtenerEncabezadopostulante");
            //if (response!= null)
            //{
            //    var idependecia =response.iddependecia;

            //}
            return response;
            
        }
    }
}