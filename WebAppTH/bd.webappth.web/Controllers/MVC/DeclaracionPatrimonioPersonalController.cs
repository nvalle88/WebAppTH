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
using bd.webappth.entidades.ViewModels;
using System.Security.Claims;
using bd.webappth.servicios.Extensores;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class DeclaracionPatrimonioPersonalController : Controller
    {
        private readonly IApiServicio apiServicio;


        public DeclaracionPatrimonioPersonalController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }



        public async Task<IActionResult> Create(int idEmpleado)
        {
            var modelo = new ViewModelDeclaracionPatrimonioPersonal();
            modelo.IdEmpleado = idEmpleado;

            var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                modelo,
                new Uri(WebApp.BaseAddress),
                "api/DeclaracionPatrimonioPersonal/ObtenerDeclaracionPatrimonial");

            if (respuesta.IsSuccess == true) {

                modelo = JsonConvert.DeserializeObject<ViewModelDeclaracionPatrimonioPersonal>(respuesta.Resultado.ToString());
                
            }

            

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewModelDeclaracionPatrimonioPersonal viewModelDeclaracionPatrimonioPersonal)
        {
            

            Response response = new Response();
            try
            {

                response = await apiServicio.InsertarAsync(
                    viewModelDeclaracionPatrimonioPersonal,
                    new Uri(WebApp.BaseAddress),
                    "api/DeclaracionPatrimonioPersonal/InsertarDeclaracionPatrimonioPersonal");



                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "DeclaracionPatrimonioPersonal",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"12000"}";
                return View(viewModelDeclaracionPatrimonioPersonal);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
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



        public async Task<IActionResult> Edit(int idDeclaracionPatrimonio, int idEmpleado)
        {
            try
            {

                var response = await apiServicio.ObtenerElementoAsync1<Response>(
                    idDeclaracionPatrimonio,
                    new Uri(WebApp.BaseAddress),
                    "api/DeclaracionPatrimonioPersonal/ObtenerDeclaracionPatrimonioPersonal");


                if (response.IsSuccess)
                {

                    var modelo = JsonConvert.DeserializeObject<ViewModelDeclaracionPatrimonioPersonal>(response.Resultado.ToString());

                    return View(modelo);

                }

                return this.RedireccionarMensajeTime(
                    "DeclaracionPatrimonioPersonal",
                    "Historial",
                    idEmpleado,
                    $"{Mensaje.Error}|{response.Message}|{"7000"}"
                );

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ViewModelDeclaracionPatrimonioPersonal viewModelDeclaracionPatrimonioPersonal)
        {
            Response response = new Response();
            try
            {
                    response = await apiServicio.EditarAsync<Response>(viewModelDeclaracionPatrimonioPersonal, new Uri(WebApp.BaseAddress), "api/DeclaracionPatrimonioPersonal/EditarDeclaracionPatrimonioPersonal");

                    if (response.IsSuccess)
                    {

                        return this.RedireccionarMensajeTime(
                            "DeclaracionPatrimonioPersonal",
                            "Historial",
                            new { idEmpleado = viewModelDeclaracionPatrimonioPersonal.IdEmpleado },
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                    }
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"12000"}";
                    return View(viewModelDeclaracionPatrimonioPersonal);
                
            }
            catch (Exception ex)
            {
               
                return BadRequest();
            }
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

                    lista = await apiServicio.Listar<ListaEmpleadoViewModel>(
                            new Uri(WebApp.BaseAddress)
                            , "api/Empleados/ListarEmpleadosActivos");
                    
                    return View(lista);
                }

                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        public async Task<IActionResult> Delete(int id, int idEmpleado)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(
                    id+"", 
                    new Uri(WebApp.BaseAddress),
                    "api/DeclaracionPatrimonioPersonal");

                if (response.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                        "DeclaracionPatrimonioPersonal",
                        "Historial",
                        new { idEmpleado },
                        $"{Mensaje.Success}|{response.Message}|{"6000"}"
                    );
                }

                return this.RedireccionarMensajeTime(
                    "DeclaracionPatrimonioPersonal",
                    "Historial",
                    new { idEmpleado },
                    $"{Mensaje.Aviso}|{response.Message}|{"7000"}"
                );
            }
            catch (Exception ex)
            {
               
                return BadRequest();
            }
        }


        public async Task<IActionResult> Historial(int idEmpleado)
        {

            try
            {
                if (idEmpleado<1) {

                    return this.RedireccionarMensajeTime(
                            "DeclaracionPatrimonioPersonal",
                            "Index",
                            $"{Mensaje.Aviso}|{Mensaje.SessionCaducada}|{"7000"}"
                    );
                }


                var modelo = await apiServicio.ObtenerElementoAsync1<DeclaracionPatrimonioPersonalHistoricoViewModel>(
                            idEmpleado,
                            new Uri(WebApp.BaseAddress)
                            , "api/DeclaracionPatrimonioPersonal/ObtenerHistoricoDeclaracionPatrimonioPersonalPorIdEmpleado");
                

                return View(modelo);

            }
            catch (Exception ex)
            {

                return this.RedireccionarMensajeTime(
                            "DeclaracionPatrimonioPersonal",
                            "Index",
                            $"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}|{"7000"}"
                    );

            }

        }


        public async Task<IActionResult> Add(int idEmpleado)
        {

            var fechaActual = DateTime.Now;

            var modelo = new ViewModelDeclaracionPatrimonioPersonal {
                IdEmpleado = idEmpleado,
                

                DeclaracionPatrimonioPersonalPasado = new DeclaracionPatrimonioPersonal
                {
                    IdEmpleado = idEmpleado,
                    FechaDeclaracion = new DateTime(fechaActual.Year-1,fechaActual.Month,1),
                    TotalEfectivo = 0,
                    TotalPasivo = 0,
                    TotalPatrimonio = 0
                },

                DeclaracionPatrimonioPersonalActual = new DeclaracionPatrimonioPersonal {
                    FechaDeclaracion = DateTime.Now,
                    TotalEfectivo = 0,
                    TotalPasivo = 0,
                    TotalPatrimonio = 0,
                    IdEmpleado = idEmpleado,
                },

                OtroIngresoActual = new OtroIngreso {
                    IngresoConyuge = 0,
                    IngresoArriendos = 0,
                    IngresoNegocioParticular = 0,
                    IngresoRentasFinancieras = 0,
                    OtrosIngresos = 0,
                    Total = 0,
                }


            };
           
            
            return View(modelo);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ViewModelDeclaracionPatrimonioPersonal viewModelDeclaracionPatrimonioPersonal)
        {


            Response response = new Response();
            try
            {
                if (!ModelState.IsValid) {

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"12000"}";
                    return View(viewModelDeclaracionPatrimonioPersonal);

                }


                response = await apiServicio.InsertarAsync(
                    viewModelDeclaracionPatrimonioPersonal,
                    new Uri(WebApp.BaseAddress),
                    "api/DeclaracionPatrimonioPersonal/AddDeclaracionPatrimonioPersonal");



                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                            "DeclaracionPatrimonioPersonal",
                            "Historial",
                            new { idEmpleado = viewModelDeclaracionPatrimonioPersonal.IdEmpleado },
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"12000"}";
                return View(viewModelDeclaracionPatrimonioPersonal);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


    }
}