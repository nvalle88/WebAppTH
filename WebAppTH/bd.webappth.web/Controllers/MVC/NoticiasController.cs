using bd.log.guardar.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ObjectTransfer;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace bd.webappth.web.Controllers.MVC
{
    public class NoticiasController : Controller
    {
        private readonly IApiServicio apiServicio;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IUploadFileService uploadFileService;

        public NoticiasController(IApiServicio apiServicio, IHostingEnvironment _hostingEnvironment, IUploadFileService uploadFileService)
        {
            this.apiServicio = apiServicio;
            this._hostingEnvironment = _hostingEnvironment;
            this.uploadFileService = uploadFileService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NoticiaViewModel view, List<IFormFile> files)
        {
            
            if (files.Count > 0)
            {
                byte[] data;
                using (var br = new BinaryReader(files[0].OpenReadStream()))
                    data = br.ReadBytes((int)files[0].OpenReadStream().Length);

                var noticiatransfer = new NoticiaTransfer
                {
                    Titulo = view.Titulo,
                    Fecha= view.Fecha,
                    Descripcion=view.Descripcion,
                    Fichero = data,
                };

                var respuesta = await CrearFichero(noticiatransfer);

                Noticia onoticia=JsonConvert.DeserializeObject<Noticia>(respuesta.Resultado.ToString());
                
                if (respuesta.IsSuccess)
                {

                    await uploadFileService.UploadFile(noticiatransfer.Fichero, "Noticias", Convert.ToString(onoticia.IdNoticia), "jpg");

                    onoticia.Foto = string.Format("{0}/{1}.{2}", "Noticias", Convert.ToString(onoticia.IdNoticia), "jpg");

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;

                    var noticia = new Noticia
                    {
                        Titulo = view.Titulo,
                    };
                    return View(noticia);
                }

            }

            return BadRequest();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> CrearFichero(NoticiaTransfer file)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(file,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/Noticias/UploadFiles");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha subido un archivo",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Noticia:", file.Titulo),
                    });

                    return new Response
                    {
                        IsSuccess = true,
                        Message = response.Message,
                        Resultado = response.Resultado
                    };
                }

                ViewData["Error"] = response.Message;
                return new Response
                {
                    IsSuccess = false,
                    Message = response.Message,
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Subiendo archivo",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return new Response
                {
                    IsSuccess = false,
                    Message = response.Message,
                };
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> ActualizarFichero(NoticiaTransfer file)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(file,
                                                             new Uri(WebApp.BaseAddress),
                                                             "/api/Noticias/UploadFilesActualizar");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha subido un archivo",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Noticia:", file.Titulo),
                    });

                    return new Response
                    {
                        IsSuccess = true,
                        Message = response.Message,
                        Resultado=response.Resultado
                    };
                }

                ViewData["Error"] = response.Message;
                return new Response
                {
                    IsSuccess = false,
                    Message = response.Message,
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Subiendo archivo",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return new Response
                {
                    IsSuccess = false,
                    Message = response.Message,
                };
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "/api/Noticias");
                    
                    respuesta.Resultado = JsonConvert.DeserializeObject<Noticia>(respuesta.Resultado.ToString());

                    var item = new NoticiaViewModel
                    {
                        Noticia = (Noticia)respuesta.Resultado,
                    };

                    ViewData["Foto"] = item.Noticia.Foto;

                    if (respuesta.IsSuccess)
                    {
                        return View(respuesta.Resultado);
                    }

                }

                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Noticia noticia, List<IFormFile> files)
        {
            if (files.Count > 0)
            {
                byte[] data;
                using (var br = new BinaryReader(files[0].OpenReadStream()))
                    data = br.ReadBytes((int)files[0].OpenReadStream().Length);

                var noticiatransfer = new NoticiaTransfer
                {
                    Id=noticia.IdNoticia,
                    Titulo = noticia.Titulo,
                    Fecha = noticia.Fecha,
                    Descripcion = noticia.Descripcion,
                    Fichero = data,
                };
                
                var respuesta = await ActualizarFichero(noticiatransfer);

                Noticia objnoticia = JsonConvert.DeserializeObject<Noticia>(respuesta.Resultado.ToString());

                if (respuesta.IsSuccess)
                {
                    await uploadFileService.UploadFile(noticiatransfer.Fichero, "Noticias", Convert.ToString(objnoticia.IdNoticia), "jpg");

                    objnoticia.Foto = string.Format("{0}/{1}.{2}", "Noticias", Convert.ToString(objnoticia.IdNoticia), "jpg");

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;

                    var onoticia = new Noticia
                    {
                        Titulo = noticia.Titulo,
                    };
                    return View(onoticia);
                }

            }

            return BadRequest();
            
        }
        
        public async Task<IActionResult> GetFile(string id)
        {
                return BadRequest();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(string id, Noticia noticia)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    
                    response = await apiServicio.EditarAsync(id, noticia, new Uri(WebApp.BaseAddress),
                                                                 "/api/Noticias");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "noticia", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una noticia",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    return View(noticia);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una noticia",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index()
        {

            var lista = new List<Noticia>();
            try
            {
                lista = await apiServicio.Listar<Noticia>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/Noticias/ListarNoticias");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando noticia",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "/api/Noticias");
                if (response.IsSuccess)
                {

                    var respuestaFile = uploadFileService.DeleteFile("Noticias", Convert.ToString(id), "jpg");

                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "noticia", id),
                        Message = "Registro de noticia eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar noticia",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }
        
    }
}