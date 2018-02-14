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
    public class Singleton
    {
        private static IndiceOcupacionalDetalle instance;

        private Singleton() { }

        public static IndiceOcupacionalDetalle Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IndiceOcupacionalDetalle();
                    instance.ListaAreaConocimientos = new List<AreaConocimiento>();
                }
                return instance;
            }
        }
    }

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
            var listaComportamientosObservables = await apiServicio.Listar<Capacitacion>(indiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Capacitaciones/ListarCapacitacionesNoAsignadasIndiceOcupacional");
            var resultado = false;
            if (listaComportamientosObservables.Count != 0)
            {
                ViewData["IdCapacitacion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(listaComportamientosObservables, "IdCapacitacion", "Nombre");
                resultado = true;
            }

            return resultado;

        }

        private async Task<bool> CargarComboComportamientosObservables(IndiceOcupacional indiceOcupacional)
        {
            var listaComportamientosObservables = await apiServicio.Listar<ComportamientoObservable>(indiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ComportamientosObservables/ListarComportamientosObservablesNoAsignadasIndiceOcupacional");
            var resultado = false;
            if (listaComportamientosObservables.Count != 0)
            {
                ViewData["IdComportamientoObservable"] =new SelectList(listaComportamientosObservables);
                resultado = true;
            }

            return resultado;

        }

        private async Task<bool> CargarComboConocimientosAdicionales(IndiceOcupacional indiceOcupacional)
        {
            var listaConocimientosAdicionales = await apiServicio.Listar<ConocimientosAdicionales>(indiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ConocimientosAdicionales/ListarConocimientosAdicionalesNoAsignadasIndiceOcupacional");
            var resultado = false;
            if (listaConocimientosAdicionales.Count != 0)
            {
                ViewData["IdConocimientosAdicionales"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(listaConocimientosAdicionales, "IdConocimientosAdicionales", "Descripcion");
                resultado = true;
            }

            return resultado;

        }

        public async Task<ActionResult> AdicionarCapacitaciones(string idIndiceOcupacional, string mensaje)

        {
            var indiceconocimientoad = new IndiceOcupacionalCapacitaciones
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
                return PartialView(indiceconocimientoad);
            }

            ViewData["Mensaje"] = Mensaje.NoExistenRegistrosPorAsignar;
            return PartialView("NoExisteElemento");

        }

        [HttpGet]
        public async Task<ActionResult> AdicionarComportamientosObservables(string idIndiceOcupacional, string mensaje)

        {

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };
            var indiceconocimientoad = new IndiceOcupacionalComportamientoObservableView
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
                ComportamientoObservables= await apiServicio.Listar<ComportamientoObservable>(indiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ComportamientosObservables/ListarComportamientosObservablesNoAsignadasIndiceOcupacional"),
            };

            if (indiceconocimientoad.ComportamientoObservables.Count!=0)
            {
                InicializarMensaje(mensaje);
                return PartialView(indiceconocimientoad);
            }

            ViewData["Mensaje"] = Mensaje.NoExistenRegistrosPorAsignar;
            return PartialView("NoExisteElemento");

        }

        public async Task<ActionResult> AdicionarConocimientosAdicionales(string idIndiceOcupacional, string mensaje)

        {
            var indiceconocimientoad = new IndiceOcupacionalConocimientosAdicionales
            {

                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };


            var resultado = await CargarComboConocimientosAdicionales(indiceOcupacional);

            if (resultado)
            {
                InicializarMensaje(mensaje);
                return PartialView(indiceconocimientoad);
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
                                                                 "api/IndicesOcupacionales/InsertarCapacitacion");
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
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarConocimientoAdicional(IndiceOcupacionalConocimientosAdicionales indiceOcupacionalConocimientosAdicionales)
        {
            Response response = new Response();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await apiServicio.InsertarAsync(indiceOcupacionalConocimientosAdicionales,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/IndicesOcupacionales/InsertarConocimientoAdicional");
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
                            EntityID = string.Format("{0} {1} {2} {3}", "Índice ocupacional:", indiceOcupacionalConocimientosAdicionales.IdIndiceOcupacional, "Conocimiento Adicional:", indiceOcupacionalConocimientosAdicionales.IdConocimientosAdicionales),
                        });

                        return RedirectToAction("Detalles", new { id = indiceOcupacionalConocimientosAdicionales.IdIndiceOcupacional });
                    }
                }

                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = indiceOcupacionalConocimientosAdicionales.IdIndiceOcupacional,
                };

                await CargarComboConocimientosAdicionales(indiceOcupacional);
                InicializarMensaje(response.Message);
                return PartialView("AdicionarConocimientosAdicionales", indiceOcupacionalConocimientosAdicionales);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un Indice ocupacional ",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarComportamientosObservables(int idIndiceOcupacional,int idComportamientoObservable)
        {
            Response response = new Response();
            try
            {
                var indiceOcupacionalComportamientoObservable =new  bd.webappth.entidades.Negocio.IndiceOcupacionalComportamientoObservable();

                    indiceOcupacionalComportamientoObservable.IdComportamientoObservable=Convert.ToInt32(idComportamientoObservable);
                    indiceOcupacionalComportamientoObservable.IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional);

                    response = await apiServicio.InsertarAsync(indiceOcupacionalComportamientoObservable,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/IndicesOcupacionales/InsertarComportamientoObservable");
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
                            EntityID = string.Format("{0} {1} {2} {3}", "Índice ocupacional:", indiceOcupacionalComportamientoObservable.IdIndiceOcupacional, "Comportamiento Observable:", indiceOcupacionalComportamientoObservable.IdComportamientoObservable),
                        });

                        return RedirectToAction("Detalles", new { id = indiceOcupacionalComportamientoObservable.IdIndiceOcupacional });
                    }

                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = indiceOcupacionalComportamientoObservable.IdIndiceOcupacional,
                };

                await CargarComboComportamientosObservables(indiceOcupacional);
                InicializarMensaje(response.Message);
                //return PartialView("AdicionarComportamientosObservables", indiceOcupacionalComportamientoObservable);
                return PartialView("AdicionarComportamientosObservables", indiceOcupacionalComportamientoObservable);


            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un Indice ocupacional ",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }


        private async Task<bool> CargarComboEstudio(IndiceOcupacional indiceOcupacional)
        {
            var listaEstudio = await apiServicio.Listar<Estudio>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudiosNoAsignadasIndiceOcupacional");
            var resultado = false;
            if (listaEstudio.Count != 0)
            {
                ViewData["IdEstudio"] = new SelectList(listaEstudio, "IdEstudio", "Nombre");
                resultado = true;
            }

            return resultado;
        }

        private async Task<bool> CargarComboExperienciaLaboralRequerida(IndiceOcupacional indiceOcupacional)
        {
            var listaExperienciaLaboralRequerida = await apiServicio.Listar<ExperienciaLaboralRequerida>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/ExperienciaLaboralRequeridas/ListarExperienciaLaboralRequeridaNoAsignadasIndiceOcupacional");
            var resultado = false;
            if (listaExperienciaLaboralRequerida.Count != 0)
            {
               
                resultado = true;
            }

            return resultado;
        }


        private async Task<bool> CargarComboMision(IndiceOcupacional indiceOcupacional)
        {
            var listaMision = await apiServicio.Listar<Mision>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/Misiones/ListarMisionNoAsignadasIndiceOcupacional");
            var resultado = false;
            if (listaMision.Count != 0)
            {
                ViewData["IdMision"] = new SelectList(listaMision, "IdMision", "Nombre");
                resultado = true;
            }

            return resultado;
            
        }


        
        



        private async Task<bool> CargarComboRelacionesInternasExternas(IndiceOcupacional indiceOcupacional)
        {
            var listaRelacionesInternasExternas = await apiServicio.Listar<RelacionesInternasExternas>(indiceOcupacional, new Uri(WebApp.BaseAddress), "api/RelacionesInternasExternas/ListarRelacionesInternasExternasNoAsignadasIndiceOcupacional");
            var resultado = false;
            if (listaRelacionesInternasExternas.Count != 0)
            {
                ViewData["IdRelacionesInternasExternas"] = new SelectList(listaRelacionesInternasExternas, "IdRelacionesInternasExternas", "Nombre");
                resultado = true;
            }

            return resultado;



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
                                                                 "api/IndicesOcupacionales/InsertarActividadesEsenciales");
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
                    ExceptionTrace = ex.Message,
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




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarEstudio(IndiceOcupacionalEstudio indiceOcupacionalEstudio)
        {
            Response response = new Response();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await apiServicio.InsertarAsync(indiceOcupacionalEstudio,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/IndicesOcupacionales/InsertarEstudio");
                    if (response.IsSuccess)
                    {

                        var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            ExceptionTrace = null,
                            Message = "Se ha creado un indice ocupacional estudio",
                            UserName = "Usuario 1",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            EntityID = string.Format("{0} {1} {2} {3}", "Índice ocupacional estudio:", indiceOcupacionalEstudio.IdIndiceOcupacional, "Estudio:", indiceOcupacionalEstudio.IdEstudio),
                        });
                        
                        return RedirectToAction("Detalles", new { id = indiceOcupacionalEstudio.IdIndiceOcupacional });
                    }
                }

                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = indiceOcupacionalEstudio.IdIndiceOcupacional,
                };

                await CargarComboEstudio(indiceOcupacional);
                InicializarMensaje(response.Message);
                return PartialView("AdicionarEstudio", indiceOcupacionalEstudio);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un Indice ocupacional ",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }



        public async Task<ActionResult> AdicionarEstudio(string idIndiceOcupacional, string mensaje)

        {
            var indiceEstudio = new IndiceOcupacionalEstudio
            {

                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };


            var resultado = await CargarComboEstudio(indiceOcupacional);

            if (resultado)
            {
                InicializarMensaje(mensaje);
                return PartialView(indiceEstudio);
            }

            ViewData["Mensaje"] = Mensaje.NoExistenRegistrosPorAsignar;
            return PartialView("NoExisteElemento");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarExperienciaLaboralRequerida(int idExperienciaLaboralRequerida, int IdIndiceOcupacional)
        {
            Response response = new Response();
            try
            {
                var experienciaLaboralRequeridaIndiceOcupacional = new IndiceOcupacionalExperienciaLaboralRequerida
                {
                    IdExperienciaLaboralRequerida = idExperienciaLaboralRequerida,
                    IdIndiceOcupacional = IdIndiceOcupacional,
                };


                response = await apiServicio.InsertarAsync(experienciaLaboralRequeridaIndiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/IndicesOcupacionales/InsertarExperienciaLaboralRequerida");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un indice ocupacional experiencia laboral requerida",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1} {2} {3}", "Índice ocupacional experiencia laboral requerida:", experienciaLaboralRequeridaIndiceOcupacional.IdIndiceOcupacional, "Experiencia Laboral Requerida:", experienciaLaboralRequeridaIndiceOcupacional.IdExperienciaLaboralRequerida),
                    });

                    return RedirectToAction("Detalles", new { id = experienciaLaboralRequeridaIndiceOcupacional.IdIndiceOcupacional });
                }


                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = experienciaLaboralRequeridaIndiceOcupacional.IdIndiceOcupacional,
                };

               
                InicializarMensaje(response.Message);
                return PartialView("AdicionarExperienciaLaboralRequerida", experienciaLaboralRequeridaIndiceOcupacional);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un Indice ocupacional ",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }



        public async Task<ActionResult> AdicionarExperienciaLaboralRequerida(string idIndiceOcupacional, string mensaje)
        {
            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };

           

                var indiceExperienciaLaboralRequerida = new IndiceOcupacionalExperienciaLaboralRequeridaView
                {
                    IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
                    ListaExperienciaLaboralRequerida = await apiServicio.Listar<ExperienciaLaboralRequerida>(indiceOcupacional, new Uri(WebApp.BaseAddress)
                                                                , "api/ExperienciaLaboralRequeridas/ListarExperienciaLaboralRequeridaNoAsignadasIndiceOcupacional")
                };

                var resultado = await CargarComboExperienciaLaboralRequerida(indiceOcupacional);

                if (resultado)
                {
                    InicializarMensaje(mensaje);
                    return PartialView(indiceExperienciaLaboralRequerida);
                }


            ViewData["Mensaje"] = Mensaje.NoExistenRegistrosPorAsignar;
            return PartialView("NoExisteElemento");

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarMision(int idMision, int IdIndiceOcupacional)
        {
            Response response = new Response();
            try
            {
                var misionIndiceOcupacional = new MisionIndiceOcupacional
                {
                    IdMision=idMision,
                    IdIndiceOcupacional=IdIndiceOcupacional,
                };


                    response = await apiServicio.InsertarAsync(misionIndiceOcupacional,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/IndicesOcupacionales/InsertarMision");
                    if (response.IsSuccess)
                    {

                        var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            ExceptionTrace = null,
                            Message = "Se ha creado un indice ocupacional misión",
                            UserName = "Usuario 1",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            EntityID = string.Format("{0} {1} {2} {3}", "Índice ocupacional misión:", misionIndiceOcupacional.IdIndiceOcupacional, "Misión:", misionIndiceOcupacional.IdMision),
                        });

                        return RedirectToAction("Detalles", new { id = misionIndiceOcupacional.IdIndiceOcupacional });
                    }
                

                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = misionIndiceOcupacional.IdIndiceOcupacional,
                };

                await CargarComboMision(indiceOcupacional);
                InicializarMensaje(response.Message);
                return PartialView("AdicionarMision", misionIndiceOcupacional);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un Indice ocupacional ",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }



        public async Task<ActionResult> AdicionarMision(string idIndiceOcupacional, string mensaje)
        {
            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };

            var listaElementos = await apiServicio.Listar<MisionIndiceOcupacional>(indiceOcupacional, new Uri(WebApp.BaseAddress)
                                                                  , "api/Misiones/ListarElementosMisionesIndiceOcupacional");

         

            if (listaElementos.Count==0)
            {
                
                var indiceMision = new IndiceOcupacionalMisionView
                {
                    IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
                    ListaMisiones = await apiServicio.Listar<Mision>(indiceOcupacional, new Uri(WebApp.BaseAddress)
                                                                , "api/Misiones/ListarMisionNoAsignadasIndiceOcupacional")
                };

                InicializarMensaje(mensaje);
                return PartialView(indiceMision);
            }

            ViewData["Mensaje"] = Mensaje.NoExistenRegistrosPorAsignar;
            return PartialView("NoExisteElemento");

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarRIE(int idRIE, int IdIndiceOcupacional)
        {
            Response response = new Response();
            try
            {
                var RIEIndiceOcupacional = new RelacionesInternasExternasIndiceOcupacional
                {
                    IdRelacionesInternasExternas = idRIE,
                    IdIndiceOcupacional = IdIndiceOcupacional,
                };


                response = await apiServicio.InsertarAsync(RIEIndiceOcupacional,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/IndicesOcupacionales/InsertarRelacionesInternasExternas");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado un indice ocupacional relaciones internas externas",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1} {2} {3}", "Índice ocupacional relaciones internas externas:", RIEIndiceOcupacional.IdIndiceOcupacional, "Relaciones Internas Externas:", RIEIndiceOcupacional.IdRelacionesInternasExternas),
                    });

                    return RedirectToAction("Detalles", new { id = RIEIndiceOcupacional.IdIndiceOcupacional });
                }


                var indiceOcupacional = new IndiceOcupacional
                {
                    IdIndiceOcupacional = RIEIndiceOcupacional.IdIndiceOcupacional,
                };

                await CargarComboMision(indiceOcupacional);
                InicializarMensaje(response.Message);
                return PartialView("AdicionarRIE", RIEIndiceOcupacional);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un Indice ocupacional ",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }




        public async Task<ActionResult> AdicionarRIE(string idIndiceOcupacional, string mensaje)
        {
            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };

            var listaElementos = await apiServicio.Listar<RelacionesInternasExternasIndiceOcupacional>(indiceOcupacional, new Uri(WebApp.BaseAddress)
                                                                  , "api/RelacionesInternasExternas/ListarElementosRIE");



            if (listaElementos.Count == 0)
            {

                var indiceRIE = new IndiceOcupacionalRIEView
                {
                    IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
                    ListaRelacionesInternasExternas = await apiServicio.Listar<RelacionesInternasExternas>(indiceOcupacional, new Uri(WebApp.BaseAddress)
                                                                , "api/RelacionesInternasExternas/ListarRIENoAsignadasIndiceOcupacional")
                };

                InicializarMensaje(mensaje);
                return PartialView(indiceRIE);
            }

            ViewData["Mensaje"] = Mensaje.NoExistenRegistrosPorAsignar;
            return PartialView("NoExisteElemento");

        }

        public async Task<ActionResult> AdicionarAreaConocimientoLocal(string idIndiceOcupacional, string mensaje)

        {
            var indideAreaConocimiento = new IndiceOcupacionalAreaConocimiento
            {

                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };

            var indiceOcupacional = new IndiceOcupacional
            {
                IdIndiceOcupacional = Convert.ToInt32(idIndiceOcupacional),
            };


            var resultado = await CargarComboAreaConocimiento(indiceOcupacional);

            if (resultado)
            {
                InicializarMensaje(mensaje);
                return PartialView(indideAreaConocimiento);
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

            var listaEscalaGrados = await apiServicio.Listar<EscalaGrados>(new Uri(WebApp.BaseAddress), "api/EscalasGrados/ListarEscalasGrados");
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
        public async Task<IActionResult> AdicionarAreaConocimientoLocal(IndiceOcupacionalAreaConocimiento indiceOcupacionalAreaConocimiento)
        {
            Response response = new Response();
            try
            {
                    var areaConocimiento = new AreaConocimiento
                    {
                        IdAreaConocimiento=indiceOcupacionalAreaConocimiento.IdAreaConocimiento,
                    };
                    Singleton.Instance.ListaAreaConocimientos.Add(areaConocimiento);

               return  await Create("");
                

            }
            catch (Exception ex)
            {
              return  BadRequest();
            }
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
                                                                 "api/IndicesOcupacionales/InsertarAreaConocimiento");
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
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP Seguridad"
                });

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndiceOcupacionalDetalle indiceOcupacionalDetalle)
        {
            Response response = new Response();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await apiServicio.InsertarAsync(indiceOcupacionalDetalle,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/IndicesOcupacionales/InsertarIndiceOcupacional");
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
                            EntityID = string.Format("{0} {1}", "Indice Ocupacional:", indiceOcupacionalDetalle.IndiceOcupacional.IdIndiceOcupacional),
                        });

                        return RedirectToAction("Index");
                    }
                }
                await CargarListaCombox();
                InicializarMensaje(response.Message);
                return View(indiceOcupacionalDetalle);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando un Indice ocupacional ",
                    ExceptionTrace = ex.Message,
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
            
            return View("Create",Singleton.Instance);
        }

        public async Task<IActionResult> Index()
        {
            var lista = new List<IndiceOcupacional>();
            try
            {
                lista = await apiServicio.Listar<IndiceOcupacional>(new Uri(WebApp.BaseAddress)
                                                                    , "api/IndicesOcupacionales/ListarIndicesOcupaciones");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando Indices ocupacionales",
                    ExceptionTrace = ex.Message,
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