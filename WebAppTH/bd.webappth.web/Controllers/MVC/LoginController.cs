using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.log.guardar.Inicializar;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using bd.webappth.entidades.Negocio;
using Microsoft.AspNetCore.Authorization;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Enumeradores;

namespace bd.webappth.web.Controllers.MVC
{
    public class LoginController : Controller
    {

        private readonly IApiServicio apiServicio;


        public LoginController(IApiServicio apiServicio)
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

        public IActionResult Index(string mensaje, string returnUrl = null)
        {
            InicializarMensaje(mensaje);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }




        public async Task<IActionResult> Login()
        {

           

            if (Request.Query.Count !=2)
            {
                return Redirect(WebApp.BaseAddressWebAppLogin);
            }

            Adscpassw adscpassw = new Adscpassw();
            var queryStrings = Request.Query;
            var qsList = new List<string>();
            foreach (var key in queryStrings.Keys)
            {
                qsList.Add(queryStrings[key]);
            }
            var adscpasswSend = new Adscpassw
            {
                AdpsLoginAdm = qsList[0],
                AdpsTokenTemp = qsList[1]
            };
            adscpassw = await GetAdscPassws(adscpasswSend);

           var a= HttpContext.Items.Count;

            if (adscpassw !=null)
            {
                var claims = new[]
                    {
                    new Claim(ClaimTypes.Name,adscpassw.AdpsLoginAdm),
                    new Claim(ClaimTypes.SerialNumber,adscpassw.AdpsToken)

                };



                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

                await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme , principal,new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties {IsPersistent=true });

                var response = await EliminarTokenTemp(adscpassw);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(HomesController.Index), "Homes");
                }
                else
                {
                    return Redirect(WebApp.BaseAddressWebAppLogin);
                }
            }
        
            return Redirect(WebApp.BaseAddressWebAppLogin);

        }

        public async Task<IActionResult> Salir()
        {

            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(LoginController.Index), "Login");


        }

        public async Task<Adscpassw> GetAdscPassws (Adscpassw adscpassw)
        {
            try
            {
                if (!adscpassw.Equals(null))
                {
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(adscpassw, new Uri("http://localhost:85/"),
                                                                  "api/Adscpassws/SeleccionarMiembroLogueado");




                    if (respuesta.IsSuccess)
                    {
                        var obje = JsonConvert.DeserializeObject<Adscpassw>(respuesta.Resultado.ToString());
                        return obje;
                    }

                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> EliminarTokenTemp(Adscpassw adscpassw)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(adscpassw.AdpsLogin))
                {
                    response = await apiServicio.EditarAsync<Response>(adscpassw, new Uri("http://localhost:85/"),
                                                                 "api/Adscpassws/EliminarTokenTemp");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", adscpassw.AdpsLogin),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un estado civil",
                            UserName = "Usuario 1"
                        });

                        return response;
                    }                

                }
                return null;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un estado civil",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return null;
            }
        }


    }
}