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
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bd.webappth.web.Controllers.MVC
{
    public class ActividadesGestionCambioController : Controller
    {
        private readonly IApiServicio apiServicio;


        public ActividadesGestionCambioController(IApiServicio apiServicio)
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

            List<ActividadesGestionCambioViewModel> lista = new List<ActividadesGestionCambioViewModel>();
            var modelo = new ActividadesGestionCambioViewModel();

            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var token = claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;


                    modelo.NombreUsuario = NombreUsuario;


                    // Obtención de datos para generar pantalla
                    lista = await apiServicio.ObtenerElementoAsync1<List<ActividadesGestionCambioViewModel>>(modelo, new Uri(WebApp.BaseAddress), "api/ActividadesGestionCambio/ListarActividadesGestionCambio");

                    return View(lista);


                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {
                mensaje = Mensaje.Excepcion;

                return View(lista);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActividadesGestionCambioViewModel model)
        {

            string mensaje = "";

            try
            {

                if (ModelState.IsValid)
                {

                    Response response = await apiServicio.InsertarAsync(model, new Uri(WebApp.BaseAddress),
                    "api/ActividadesGestionCambio/InsertarActividadesGestionCambio");

                    if (response.IsSuccess)
                    {
                        var mensajeResultado = response.Message;

                        return RedirectToAction("Index", "ActividadesGestionCambio", new { mensaje = mensajeResultado });

                    }

                    mensaje = response.Message;
                }
                else {
                    mensaje = Mensaje.ModeloInvalido;
                }

                

                var modelo = new ActividadesGestionCambioViewModel();

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var token = claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;


                    modelo.NombreUsuario = NombreUsuario;



                    var objetoListas = await apiServicio.ObtenerElementoAsync1<CrearActividadesGestionCambioViewModel>(modelo, new Uri(WebApp.BaseAddress), "api/ActividadesGestionCambio/CrearActividadesGestionCambio");

                    modelo.FechaInicio = objetoListas.actividadesGestionCambioViewModel.FechaInicio;
                    modelo.FechaFin = objetoListas.actividadesGestionCambioViewModel.FechaFin;
                    

                    ViewData["Estados"] = new SelectList(objetoListas.ListaEstadoActividadGestionCambioViewModel, "Valor", "Nombre");

                    ViewData["Dependencias"] = new SelectList(objetoListas.ListaDependenciasViewModel, "IdDependencia", "NombreDependencia");

                    ViewData["Empleados"] = new SelectList(objetoListas.ListaDatosBasicosEmpleadoViewModel, "IdEmpleado", "Nombres");
                    

                    InicializarMensaje(mensaje);
                    return View(model);


                }

                return RedirectToAction("Login", "Login");
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "ActivacionesPersonalTalentoHumano", new { mensaje = Mensaje.Excepcion });
            }
        }

        
        public async Task<IActionResult> Create(string mensaje)
        {
            InicializarMensaje(mensaje);

            var modelo = new ActividadesGestionCambioViewModel();

            try
            {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var token = claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;


                    modelo.NombreUsuario = NombreUsuario;



                    var objetoListas = await apiServicio.ObtenerElementoAsync1<CrearActividadesGestionCambioViewModel>(modelo, new Uri(WebApp.BaseAddress), "api/ActividadesGestionCambio/CrearActividadesGestionCambio");

                    modelo.FechaInicio = objetoListas.actividadesGestionCambioViewModel.FechaInicio;
                    modelo.FechaFin = objetoListas.actividadesGestionCambioViewModel.FechaFin;


                    ViewData["Estados"] = new SelectList(objetoListas.ListaEstadoActividadGestionCambioViewModel, "Valor", "Nombre");

                    ViewData["Dependencias"] = new SelectList(objetoListas.ListaDependenciasViewModel, "IdDependencia", "NombreDependencia");

                    ViewData["Empleados"] = new SelectList(objetoListas.ListaDatosBasicosEmpleadoViewModel, "IdEmpleado", "Nombres");


                    return View(modelo);


                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {
                mensaje = Mensaje.Excepcion;

                return View(modelo);

            }
        }


        public async Task<IActionResult> Edit(string mensaje, int id)
        {
            InicializarMensaje(mensaje);

            var modelo = new ActividadesGestionCambioViewModel();

            try
            {
                modelo.IdActividadesGestionCambio = id;

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var token = claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;


                    modelo.NombreUsuario = NombreUsuario;



                    var objetoListas = await apiServicio.ObtenerElementoAsync1<CrearActividadesGestionCambioViewModel>(modelo, new Uri(WebApp.BaseAddress), "api/ActividadesGestionCambio/ObtenerActividadesGestionCambioPorId");

                    modelo.FechaInicio = objetoListas.actividadesGestionCambioViewModel.FechaInicio;
                    modelo.FechaFin = objetoListas.actividadesGestionCambioViewModel.FechaFin;


                    ViewData["Estados"] = new SelectList(objetoListas.ListaEstadoActividadGestionCambioViewModel, "Valor", "Nombre");

                    ViewData["Dependencias"] = new SelectList(objetoListas.ListaDependenciasViewModel, "IdDependencia", "NombreDependencia");

                    ViewData["Empleados"] = new SelectList(objetoListas.ListaDatosBasicosEmpleadoViewModel, "IdEmpleado", "Nombres");

                    modelo = objetoListas.actividadesGestionCambioViewModel;

                    return View(modelo);


                }

                return RedirectToAction("Login", "Login");

            }
            catch (Exception ex)
            {
                mensaje = Mensaje.Excepcion;

                return View(modelo);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActividadesGestionCambioViewModel model)
        {

            try
            {
                Response response = await apiServicio.InsertarAsync(model, new Uri(WebApp.BaseAddress),
                    "api/ActividadesGestionCambio/EditarActividadesGestionCambio");

                if (response.IsSuccess)
                {
                    var mensajeResultado = response.Message;

                    return RedirectToAction("Index", "ActividadesGestionCambio", new { mensaje = mensajeResultado });

                }

                return RedirectToAction("Edit", "ActividadesGestionCambio", new { mensaje = response.Message , id = model.IdActividadesGestionCambio});

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "ActivacionesPersonalTalentoHumano", new { mensaje = Mensaje.Excepcion });
            }
        }


        public async Task<JsonResult> ListarEmpleadosPorSucursalYDependencia(int idDependencia)
        {
            var modelo = new ActividadesGestionCambioViewModel();

            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

            if (claim.IsAuthenticated == true)
            {
                var token = claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                modelo.IdDependencia = idDependencia;
                modelo.NombreUsuario = NombreUsuario;

                var empleados = await apiServicio.Listar<DatosBasicosEmpleadoViewModel>(modelo, new Uri(WebApp.BaseAddress)
                               , "api/ActividadesGestionCambio/ObtenerEmpleadosPorSucursalYDependencia");

                return Json(empleados);
            }

            return Json( new List<DatosBasicosEmpleadoViewModel>() );
        }

    }
}