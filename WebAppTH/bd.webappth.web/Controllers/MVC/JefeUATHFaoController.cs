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
    public class JefeUATHFaoController : Controller
    {
        // public IActionResult Index()
        // {
        //    return View();
        //}
        private readonly IApiServicio apiServicio;

        public JefeUATHFaoController(IApiServicio apiServicio)
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
                                                                    , "api/Empleados/ListarEmpleadosConFAOTH");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Create(int id, int IdActividad)
        {
            try
            {

                var usuario = new DocumentoFAOViewModel
                {
                    IdEmpleado = id,
                    IdFormularioAnalisisOcupacional = IdActividad

                };
                var response = await apiServicio.ObtenerElementoAsync(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ObtenerEncabezadoEmpleadosFaoValidarConValidacionJefeTH");
                if (response.IsSuccess)
                {
                    var empleado = JsonConvert.DeserializeObject<DocumentoFAOViewModel>(response.Resultado.ToString());
                    ViewData["IdManualPuesto"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList((empleado.ListasManualPuesto), "IdManualPuesto", "Nombre");
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
        [HttpPost]
        public async Task<IActionResult> ValidarJefeTalentoHumano(DocumentoFAOViewModel documentoFAOViewModel)
        {

            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;


                var usuario = new DocumentoFAOViewModel
                {
                    
                    NombreUsuario = NombreUsuario,
                    Revisar = documentoFAOViewModel.Revisar,
                    IdManualPuestoActual = documentoFAOViewModel.IdManualPuestoActual,
                    IdManualPuesto = documentoFAOViewModel.IdManualPuesto,
                    IdAdministracionTalentoHumano = documentoFAOViewModel.IdAdministracionTalentoHumano,
                    IdEmpleado = documentoFAOViewModel.IdEmpleado
                };
                //Debug.Write(documentoFAOViewModel.Count);
                var response = await apiServicio.InsertarAsync(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/InformeUATH/InsertarInforeUATH");

                if (response.IsSuccess)
                {

                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return View(documentoFAOViewModel);
        }
        public async Task<IActionResult> Detalle(int id, int IdActividad)
        {
            try
            {
                var usuario = new DocumentoFAOViewModel
                {
                    IdEmpleado = id,
                    IdFormularioAnalisisOcupacional = IdActividad

                };
                var response = await apiServicio.ObtenerElementoAsync(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/InformeFinalFAO");
                if (response.IsSuccess)
                {
                    var empleado = JsonConvert.DeserializeObject<DocumentoFAOViewModel>(response.Resultado.ToString());
                    ViewData["IdManualPuesto"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList((empleado.ListasManualPuesto), "IdManualPuesto", "Nombre");
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