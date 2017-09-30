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
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class IndicesOcupacionalesController : Controller
    {
        private readonly IApiServicio apiServicio;

        public IndicesOcupacionalesController(IApiServicio apiServicio)
        {

            this.apiServicio = apiServicio;

        }

        private async Task CargarListaCombox()
        {

            var ListaDependencia = await apiServicio.Listar<Dependencia>(new Uri(WebApp.BaseAddress), "api/Dependencias/ListarDependencias");
            ViewData["IdDependencia"] = new SelectList(ListaDependencia, "IdDependencia", "Nombre");

            var listaManualPuesto = await apiServicio.Listar<ManualPuesto>(new Uri(WebApp.BaseAddress), "api/ManualPuestos/ListarManualPuestos");
            ViewData["IdManualPuesto"] = new SelectList(listaManualPuesto, "IdManualPuesto", "Nombre");

            var listaRoles = await apiServicio.Listar<RolPuesto>(new Uri(WebApp.BaseAddress), "api/RolesPuesto/ListarRolesPuesto");
            ViewData["IdRolPuesto"] = new SelectList(listaRoles, "IdRolPuesto", "Nombre");

            var listaEscalaGrados = await apiServicio.Listar<EscalaGrados>(new Uri(WebApp.BaseAddress), "/api/EscalasGrados/ListarEscalasGrados");
            ViewData["IdEscalaGrados"] = new SelectList(listaEscalaGrados, "IdEscalaGrados", "Remuneracion");

          
        }

        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndiceOcupacional indiceOcupacional)
        {
            Response response = new Response();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await apiServicio.InsertarAsync(indiceOcupacional,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "/api/IndicesOcupacionales/InsertarIndiceOcupacional");
                    if (response.IsSuccess)
                    {

                        var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            ExceptionTrace = null,
                            Message = "Se ha creado un indice ocupacional",
                            UserName = "Usuario 1",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            EntityID = string.Format("{0} {1}", "Indice Ocupacional:", indiceOcupacional.IdIndiceOcupacional),
                        });

                        return RedirectToAction("Index");
                    }
                }
                await CargarListaCombox();
                InicializarMensaje(response.Message);
                return View(indiceOcupacional);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un Indice ocupacional ",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }


        public async Task<IActionResult> Create(string mensaje)
        {

           await CargarListaCombox();
            InicializarMensaje(mensaje);
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var lista = new List<IndiceOcupacional>();
            try
            {
                lista = await apiServicio.Listar<IndiceOcupacional>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/IndicesOcupacionales/ListarIndicesOcupaciones");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Indices ocupacionales",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> Detalles(int id)
        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional=id,
            };

            var IndiOcupacionalDetalle = new IndiceOcupacionalDetalle
            {
                IndiceOcupacional=indiceOcupacional,
            };


            var indiceOcupacionalDetalle = await apiServicio.ObtenerElementoAsync1<IndiceOcupacionalDetalle>(IndiOcupacionalDetalle, new Uri(WebApp.BaseAddress), "api/IndicesOcupacionales/DetalleIndiceOcupacional");

            return View(indiceOcupacionalDetalle);
        }
    }
}