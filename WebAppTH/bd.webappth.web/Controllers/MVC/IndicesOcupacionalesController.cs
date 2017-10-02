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

        private async Task<bool> CargarComboActividedesEsenciales(IndiceOcupacional indiceOcupacional)
        {
            var listaActividadesEsenciales = await apiServicio.Listar<ActividadesEsenciales>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/ActividadesEsenciales/ListarActividedesEsencialesNoAsignadasIndiceOcupacional");

            var resultado = false;
            if (listaActividadesEsenciales.Count != 0)
            {
                ViewData["IdActividadesEsenciales"] = new SelectList(listaActividadesEsenciales, "IdActividadesEsenciales", "Descripcion");
                resultado = true;
            }

            return resultado;

        }

        private async Task<bool> CargarComboAreaConocimiento(IndiceOcupacional indiceOcupacional)
        {
            var listaAreasConocimientos = await apiServicio.Listar<AreaConocimiento>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientosNoAsignadasIndiceOcupacional");
            var resultado = false;
            if (listaAreasConocimientos.Count!=0)
            {
                ViewData["IdAreaConocimiento"] = new SelectList(listaAreasConocimientos, "IdAreaConocimiento", "Descripcion");
                resultado = true;
            }

            return resultado;
          

        }

        private async Task<bool> CargarComboCapacitaciones(IndiceOcupacional indiceOcupacional)
        {
            var listaCapacitaciones = await apiServicio.Listar<Capacitacion>(indiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/IndiceOcupacionalCapacitaciones/ListaFiltradaCapacitaciones");
            var resultado = false;
            if (listaCapacitaciones.Count != 0)
            {
                ViewData["IdCapacitacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(listaCapacitaciones, "IdCapacitacion", "Nombre");
                resultado = true;
            }

            return resultado;

        }

        public async Task<ActionResult> AdicionarCapacitaciones(string idIndiceOcupacional, string mensaje)

        {
            var indicecapacitaciones = new IndiceOcupacionalCapacitaciones
            {

                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };


            var resultado = await CargarComboCapacitaciones(indiceOcupacional);

            if (resultado)
            {
                InicializarMensaje(mensaje);
                return PartialView(indicecapacitaciones);
            }

            ViewData["Mensaje"] = Mensaje.NoExistenRegistrosPorAsignar;
            return PartialView("NoExisteElemento");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarCapacitacion(IndiceOcupacionalCapacitaciones indiceOcupacionalCapacitaciones)
        {
            Response response = new Response();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await apiServicio.InsertarAsync(indiceOcupacionalCapacitaciones,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "/api/IndiceOcupacionalCapacitaciones/InsertarIndiceOcupacionalCapacitacion");
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
                            EntityID = string.Format("{0} {1} {2} {3}", "Índice ocupacional:", indiceOcupacionalCapacitaciones.IdIndiceOcupacional, "Capacitación:", indiceOcupacionalCapacitaciones.IdCapacitacion),
                        });

                        return RedirectToAction("Detalles", new { id = indiceOcupacionalCapacitaciones.IdIndiceOcupacional });
                    }
                }

                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = indiceOcupacionalCapacitaciones.IdIndiceOcupacional,
                };

                await CargarComboCapacitaciones(indiceOcupacional);
                InicializarMensaje(response.Message);
                return PartialView("AdicionarCapacitaciones", indiceOcupacionalCapacitaciones);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarActividesEsenciales(IndiceOcupacionalActividadesEsenciales indiceOcupacionalActividadesEsenciales)
        {
            Response response = new Response();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await apiServicio.InsertarAsync(indiceOcupacionalActividadesEsenciales,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "/api/IndicesOcupacionales/InsertarActividadesEsenciales");
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
                            EntityID = string.Format("{0} {1} {2} {3}", "Índice ocupacional:", indiceOcupacionalActividadesEsenciales.IdIndiceOcupacional, "Área de conocimiento:", indiceOcupacionalActividadesEsenciales.IdActividadesEsenciales),
                        });

                        return RedirectToAction("Detalles", new { id = indiceOcupacionalActividadesEsenciales.IdIndiceOcupacional });
                    }
                }

                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = indiceOcupacionalActividadesEsenciales.IdIndiceOcupacional,
                };

                await CargarComboActividedesEsenciales(indiceOcupacional);
                InicializarMensaje(response.Message);
                return PartialView("AdicionarActividesEsenciales", indiceOcupacionalActividadesEsenciales);

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



        public async Task<ActionResult> AdicionarActividesEsenciales(string idIndiceOcupacional, string mensaje)

        {
            var indideactividedesEsenciales = new IndiceOcupacionalActividadesEsenciales
            {

                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };


            var resultado = await CargarComboActividedesEsenciales(indiceOcupacional);

            if (resultado)
            {
                InicializarMensaje(mensaje);
                return PartialView(indideactividedesEsenciales);
            }

            ViewData["Mensaje"] = Mensaje.NoExistenRegistrosPorAsignar;
            return PartialView("NoExisteElemento");

        }


        public async Task<ActionResult> AdicionarAreaConocimiento(string idIndiceOcupacional,string mensaje)

        {
            var indideAreaConocimiento = new IndiceOcupacionalAreaConocimiento
            {

                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };


           var resultado= await CargarComboAreaConocimiento(indiceOcupacional);

            if (resultado)
            {
                InicializarMensaje(mensaje);
                return PartialView(indideAreaConocimiento);
            }
            ViewData["Mensaje"] =Mensaje.NoExistenRegistrosPorAsignar;
            return PartialView("NoExisteElemento");
            
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
        public async Task<IActionResult> AdicionarAreaConocimiento(IndiceOcupacionalAreaConocimiento indiceOcupacionalAreaConocimiento)
        {
            Response response = new Response();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await apiServicio.InsertarAsync(indiceOcupacionalAreaConocimiento,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "/api/IndicesOcupacionales/InsertarAreaConocimiento");
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
                            EntityID = string.Format("{0} {1} {2} {3}", "Índice ocupacional:", indiceOcupacionalAreaConocimiento.IdIndiceOcupacional,"Área de conocimiento:",indiceOcupacionalAreaConocimiento.IdAreaConocimiento),
                        });

                        return RedirectToAction("Detalles",new {id=indiceOcupacionalAreaConocimiento.IdIndiceOcupacional});
                    }
                }

                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional=indiceOcupacionalAreaConocimiento.IdIndiceOcupacional,
                };

                await CargarComboAreaConocimiento(indiceOcupacional);
                InicializarMensaje(response.Message);
                return PartialView("AdicionarAreaConocimiento", indiceOcupacionalAreaConocimiento);

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