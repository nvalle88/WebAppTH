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
using System.Security.Claims;

namespace bd.webappth.web.Controllers.MVC
{
    public class LavadoActivosEmpleadosController : Controller
    {

        private readonly IApiServicio apiServicio;


        public LavadoActivosEmpleadosController(IApiServicio apiServicio)
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

            var modelo = new LavadoActivoEmpleadoViewModel();
            modelo.ListaLavadoActivoItemViewModel = new List<LavadoActivoItemViewModel>();

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {

                    var token = claim.Claims.Where(c => c.Type == ClaimTypes.SerialNumber).FirstOrDefault().Value;
                    var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    modelo.NombreUsuario = NombreUsuario;

                    // Obtención de datos para generar pantalla
                    var respuesta = await apiServicio.ObtenerElementoAsync<LavadoActivoEmpleadoViewModel>(modelo, new Uri(WebApp.BaseAddress), "api/LavadoActivoEmpleados/CrearLavadoActivoEmpleado");

                    if (respuesta.IsSuccess)
                    {

                        modelo = JsonConvert.DeserializeObject<LavadoActivoEmpleadoViewModel>(respuesta.Resultado.ToString());
                        

                        return View(modelo);
                    }
                    else {

                        InicializarMensaje(respuesta.Message);
                        return View(modelo);
                    }

                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }


            }
            catch (Exception ex)
            {
                return View();
            }



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Volver()
        {
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LavadoActivoEmpleadoViewModel model)
        {
            
            try
            {

                Response response = await apiServicio.InsertarAsync(model,
                                                         new Uri(WebApp.BaseAddress),
                                                         "api/LavadoActivoEmpleados/InsertarLavadoActivosEmpleados");
                
                
                if (response.IsSuccess)
                {
                    
                    return RedirectToAction("Index", "LavadoActivosEmpleados", new { mensaje = response.Message });

                }
                
                return RedirectToAction("Index", "LavadoActivosEmpleados", new { mensaje = response.Message });
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "LavadoActivosEmpleados", new { mensaje = Mensaje.Excepcion });
            }
        }


        

    }
}