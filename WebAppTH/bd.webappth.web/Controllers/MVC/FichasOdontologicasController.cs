using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ViewModels;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using bd.webappth.entidades.ObjectTransfer;
using Microsoft.AspNetCore.Http;

namespace bd.webappth.web.Controllers.MVC
{
    public class FichasOdontologicasController : Controller
    {
        
        private readonly IApiServicio apiServicio;
        private IHostingEnvironment _hostingEnvironment;


        public FichasOdontologicasController(IApiServicio apiServicio, IHostingEnvironment _hostingEnvironment)
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
        
       
        public async Task<IActionResult> Index(string mensaje)
        {
            InicializarMensaje(mensaje);

            var modelo = new FichaOdontologicaViewModel();
            modelo.IdPersona = 0;

            var pdfFile =  WebApp.BaseAddress + "/FichasOdontologicasDocumentos/"+modelo.IdPersona+".pdf";
            modelo.Url = pdfFile;

            return View(modelo);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, List<IFormFile> files)
        {
            var modelo = new FichaOdontologicaViewModel();
            modelo.IdPersona = 0;

            try
            {
                if (files.Count > 0)
                {
                    byte[] data;
                    using (var br = new BinaryReader(files[0].OpenReadStream()))
                        data = br.ReadBytes((int)files[0].OpenReadStream().Length);

                    var documenttransfer = new FichaOdontologicaViewModel
                    {
                        IdPersona = id,
                        Fichero = data
                    };

                    var respuesta = await CrearFicheroOdontologicoPdf(documenttransfer);

                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(respuesta.Message);
                        var pdfFile = WebApp.BaseAddress + "/FichasOdontologicasDocumentos/" + modelo.IdPersona + ".pdf";
                        modelo.Url = pdfFile;

                        return View(modelo);
                    }

                    InicializarMensaje(respuesta.Message);
                    return View(modelo);
                }

                InicializarMensaje(Mensaje.ErrorCargaArchivo);
                return View(modelo);

            }
            catch (Exception)
            {
                InicializarMensaje(Mensaje.Excepcion);
                return View(modelo);
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> CrearFicheroOdontologicoPdf(FichaOdontologicaViewModel file)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(file,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ExamenesComplementarios/CrearFicheroOdontologicoPdf");
                if (response.IsSuccess)
                {
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

                return new Response
                {
                    IsSuccess = false,
                    Message = response.Message,
                };
            }
        }



    }
}