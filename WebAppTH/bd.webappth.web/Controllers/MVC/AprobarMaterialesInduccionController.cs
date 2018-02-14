using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Hosting;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using System.IO;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.ObjectTransfer;
using System.Security.Claims;

namespace bd.webappth.web.Controllers.MVC
{
    public class AprobarMaterialesInduccionController : Controller
    {

        private readonly IApiServicio apiServicio;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IUploadFileService uploadFileService;

        public AprobarMaterialesInduccionController(IApiServicio apiServicio, IHostingEnvironment _hostingEnvironment, IUploadFileService uploadFileService)
        {
            this.apiServicio = apiServicio;
            this._hostingEnvironment = _hostingEnvironment;
            this.uploadFileService = uploadFileService;

        }

        public async Task<IActionResult> IngresarInduccion()
        {
            Response response = new Response();
            try
            {
                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
                var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
                var empleadoJson = ObtenerEmpleadoLogueado(NombreUsuario);

                var induccionEmpleado = new Induccion
                {
                    IdEmpleado = empleadoJson.Result.IdEmpleado,
                    Fecha = DateTime.Now.Date
                };

                response = await apiServicio.InsertarAsync(induccionEmpleado,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/MaterialesInduccion/IngresarInduccionEmpleado");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha realido inducción de empleado",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Inducción:", empleadoJson.Result.IdEmpleado),
                    });



                    return RedirectToAction("Create");
                }

                ViewData["Error"] = response.Message;
                return View();

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Declaración Personal",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
        }

        private async Task<Empleado> ObtenerEmpleadoLogueado(string nombreUsuario)
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

        public async Task<IActionResult> ListarMaterialesInduccion()
        {

            var lista = new List<MaterialInduccion>();
            var imagenes = new List<MaterialInduccion>();
            var documentos = new List<MaterialInduccion>();
            var videos = new List<MaterialInduccion>();
            try
            {
                lista = await apiServicio.Listar<MaterialInduccion>(new Uri(WebApp.BaseAddress)
                                                                    , "api/MaterialesInduccion/ListarMaterialesInduccion");

                foreach (var item in lista)
                {
                    var ext = Path.GetExtension(item.Url);
                    if (ext == ".jpeg" || ext == ".bmp" || ext == ".jpe" || ext == ".jpg" || ext == ".gif")
                    {
                        imagenes.Add(item);
                    }
                    else if (ext == ".pdf" || ext == ".xlsx" || ext == ".xls" || ext == ".docx" || ext == ".doc" || ext == ".pptx" || ext == ".ppt" || ext == "ppsx" || ext == "pps")
                    {
                        documentos.Add(item);
                    }
                    else
                    {
                        item.Url = item.Url.Replace("watch?v=", "embed/");
                        videos.Add(item);
                    }
                }
                var ViewModelInduccion = new ViewModelInduccion
                {
                    Imagenes = imagenes,
                    Archivos = documentos,
                    Videos = videos
                };
                return View(ViewModelInduccion);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = WebApp.NombreAplicacion,
                    Message = "Listando materiales de inducción",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<FileResult> Download(string id)
        {


            var response = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                              "api/MaterialesInduccion");

            var materialinduccion = JsonConvert.DeserializeObject<MaterialInduccion>(response.Resultado.ToString());
            var d = new MaterialInduccion
            {
                IdMaterialInduccion = Convert.ToInt32(id),
                Url = materialinduccion.Url
            };

            var ext = Path.GetExtension(materialinduccion.Url);

            if (ext != "")
            {
                var responseGetFile = await apiServicio.ObtenerElementoAsync(d,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/MaterialesInduccion/GetFile");
                var m = JsonConvert.DeserializeObject<DocumentoInstitucionalTransfer>(responseGetFile.Resultado.ToString());

                //var fileName = $"{ responseGetFile.Message}.pdf";


                var fileName = string.Format("{0}{1}", $"{ responseGetFile.Message}", ext);
                string mime = MimeKit.MimeTypes.GetMimeType(fileName);
                return File(m.Fichero, mime, fileName);
            }
            else
            {
                return File("", "");
            }

        }
    }
}