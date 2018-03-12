using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class JefeFAOController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IApiServicio apiServicio;

        public JefeFAOController(IApiServicio apiServicio)
        {

            this.apiServicio = apiServicio;


        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                var lista = new List<DocumentoFAOViewModel>();

                var usuario = new DocumentoFAOViewModel
                {
                    NombreUsuario = NombreUsuario

                };
                lista = await apiServicio.Listar<DocumentoFAOViewModel>(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleadosConFAOJefes");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        //public async Task<IActionResult> Create()
        //{
        //    try
        //    {
        //        var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
        //        var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

        //        var usuario = new DocumentoFAOViewModel
        //        {
        //            NombreUsuario = NombreUsuario

        //        };
        //        var response = await apiServicio.ObtenerElementoAsync(usuario, new Uri(WebApp.BaseAddress)
        //                                                            , "api/Empleados/ObtenerEncabezadoEmpleadosFao");

        //        if (response.IsSuccess)
        //        {
        //            var empleado = JsonConvert.DeserializeObject<DocumentoFAOViewModel>(response.Resultado.ToString());
        //            return View(empleado);
        //        }
        //        ViewData["Error"] = response.Message;
        //        return View();

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //}
        public async Task<IActionResult> Create(int id)
        {

            try
            {
               

                var usuario = new DocumentoFAOViewModel
                {
                    IdEmpleado = id

                };
                var response = await apiServicio.ObtenerElementoAsync(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ObtenerEncabezadoEmpleadosFaoValidar");

                if (response.IsSuccess)
                {
                    var empleado = JsonConvert.DeserializeObject<DocumentoFAOViewModel>(response.Resultado.ToString());
                    return View(empleado);
                }
                ViewData["Error"] = response.Message;
                return View();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}