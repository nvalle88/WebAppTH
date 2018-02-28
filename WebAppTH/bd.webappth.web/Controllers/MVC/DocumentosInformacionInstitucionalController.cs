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

//using iTextSharp.text;
//using iTextSharp.text.pdf;


namespace bd.webappth.web.Controllers.MVC
{
    public class DocumentosInformacionInstitucionalController : Controller
    {
        private readonly IApiServicio apiServicio;
        private IHostingEnvironment _hostingEnvironment;


        public DocumentosInformacionInstitucionalController(IApiServicio apiServicio, IHostingEnvironment _hostingEnvironment)
        {
            this.apiServicio = apiServicio;
            this._hostingEnvironment = _hostingEnvironment;

        }
        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }
        public IActionResult Create(string mensaje)
        {
            InicializarMensaje(mensaje);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ViewModelDocumentoInstitucional view, List<IFormFile> files)
        {



            if (files.Count > 0)
            {
                byte[] data;
                using (var br = new BinaryReader(files[0].OpenReadStream()))
                    data = br.ReadBytes((int)files[0].OpenReadStream().Length);

                var documenttransfer = new DocumentoInstitucionalTransfer
                {
                    Nombre = view.Nombre,
                    Fichero = data,
                };

                var respuesta = await CreateFichero(documenttransfer);
                if (respuesta.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = respuesta.Message;

                    var documento = new DocumentoInformacionInstitucional
                    {
                        Nombre=view.Nombre,
                    };
                    return View(documento);
                }

            }

            return BadRequest();


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> CreateFichero(DocumentoInstitucionalTransfer file)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(file,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/DocumentosInformacionInstitucional/UploadFiles");
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
                        EntityID = string.Format("{0} {1}", "Documento Información Institucional:", file.Nombre),
                    });

                    return new Response
                    {
                        IsSuccess = true,
                        Message = response.Message,
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
                                                                  "api/DocumentosInformacionInstitucional");


                    respuesta.Resultado = JsonConvert.DeserializeObject<DocumentoInformacionInstitucional>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
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

        public async Task<IActionResult> GetFile(string id)
        {

            return BadRequest();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DocumentoInformacionInstitucional documentoInformacionInstitucional)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, documentoInformacionInstitucional, new Uri(WebApp.BaseAddress),
                                                                 "api/DocumentosInformacionInstitucional");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "documento de información institucional", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un documento de información institucional",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    return View(documentoInformacionInstitucional);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un documento de información institucional",
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

            var lista = new List<DocumentoInformacionInstitucional>();
            try
            {
                lista = await apiServicio.Listar<DocumentoInformacionInstitucional>(new Uri(WebApp.BaseAddress)
                                                                    , "api/DocumentosInformacionInstitucional/ListarDocumentosInformacionInstitucional");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando documentos información institucional",
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
                                                               , "api/DocumentosInformacionInstitucional");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "documento de información institucional", id),
                        Message = "Registro de documento de información institucional eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("Index");
                }
                // return BadRequest();
                return RedirectToAction("Index", new { mensaje = response.Message });
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Documento de Información Institucional",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }



      public async Task<FileResult> Download(string id)
        {
            var d = new DocumentoInformacionInstitucional
            {
                IdDocumentoInformacionInstitucional = Convert.ToInt32(id),
            };

           var response = await apiServicio.ObtenerElementoAsync(d,
                                                            new Uri(WebApp.BaseAddress),
                                                            "api/DocumentosInformacionInstitucional/GetFile");
            var m = JsonConvert.DeserializeObject<DocumentoInstitucionalTransfer>(response.Resultado.ToString());
            var fileName = $"{ response.Message}.pdf";

            return File(m.Fichero, "application/pdf",fileName);
        }

    }
}