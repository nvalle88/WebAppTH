using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Hosting;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using bd.webappth.entidades.ObjectTransfer;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using System.Security.Claims;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class MaterialesInduccionController : Controller
    {
        private readonly IApiServicio apiServicio;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IUploadFileService uploadFileService;



        public MaterialesInduccionController(IApiServicio apiServicio, IHostingEnvironment _hostingEnvironment, IUploadFileService uploadFileService)
        {
            this.apiServicio = apiServicio;
            this._hostingEnvironment = _hostingEnvironment;
            this.uploadFileService = uploadFileService;

        }


        #region Métodos para el mantenimiento del proceso de MaterialesInduccion

        public async Task<IActionResult> Index()
        {

            var lista = new List<MaterialInduccion>();
            var imagenes = new List<MaterialInduccion>();
            var documentos = new List<MaterialInduccion>();
            var videos = new List<MaterialInduccion>();

            try
            {


                lista = await apiServicio.Listar<MaterialInduccion>(
                    new Uri(WebApp.BaseAddress),
                    "api/MaterialesInduccion/ListarMaterialesInduccionTTHH"
                );

                foreach (var item in lista)
                {
                    var ext = Path.GetExtension(item.Url);
                    if (ext == ".jpeg" || ext == ".bmp" || ext == ".jpe" || ext == ".jpg" || ext == ".gif" || ext == ".png")
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
                return BadRequest();
            }
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialInduccion view, List<IFormFile> files)
        {
            if (files.Count > 0)
            {
                byte[] data;
                using (var br = new BinaryReader(files[0].OpenReadStream()))
                    data = br.ReadBytes((int)files[0].OpenReadStream().Length);

                var ext = Path.GetExtension(files[0].FileName);

                var documenttransfer = new DocumentoInstitucionalTransfer
                {
                    Nombre = view.Titulo,
                    Descripcion = view.Descripcion,
                    Extension = ext,
                    Fichero = data,
                };

                var respuesta = await CreateFichero(documenttransfer);

                MaterialInduccion materialInduccion = JsonConvert.DeserializeObject<MaterialInduccion>(respuesta.Resultado.ToString());

                if (respuesta.IsSuccess)
                {
                    await uploadFileService.UploadFile(documenttransfer.Fichero, "MaterialInduccion", Convert.ToString(materialInduccion.IdMaterialInduccion), ext);

                    materialInduccion.Url = string.Format("{0}/{1}{2}", "MaterialInduccion", Convert.ToString(materialInduccion.IdMaterialInduccion), ext);

                    return this.RedireccionarMensajeTime(
                        "MaterialesInduccion",
                        "Index",
                        $"{Mensaje.Success}|{respuesta.Message}|{"7000"}"
                    );
                }
                else
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{respuesta.Message}|{"10000"}";

                    var documento = new MaterialInduccion
                    {
                        Titulo = view.Titulo,
                    };
                    return View(view);
                }

            }
            else
            {

                var documenttransfer = new DocumentoInstitucionalTransfer
                {
                    Nombre = view.Titulo,
                    Descripcion = view.Descripcion,
                    Url = view.Url,
                };

                var respuesta = await CreateFichero(documenttransfer);

                MaterialInduccion materialInduccion = JsonConvert.DeserializeObject<MaterialInduccion>(respuesta.Resultado.ToString());

                if (respuesta.IsSuccess)
                {


                    return this.RedireccionarMensajeTime(
                        "MaterialesInduccion",
                        "Index",
                        $"{Mensaje.Success}|{respuesta.Message}|{"7000"}"
                    );
                }
                else
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{respuesta.Message}|{"10000"}";

                    var documento = new MaterialInduccion
                    {
                        Titulo = view.Titulo,
                    };
                    return View(view);
                }

            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> CreateFichero(DocumentoInstitucionalTransfer file)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(
                    file,
                    new Uri(WebApp.BaseAddress),
                    "api/MaterialesInduccion/UploadFiles"
                );

                if (response.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = true,
                        Message = response.Message,
                        Resultado = response.Resultado
                    };

                }


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



        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(
                        id,
                        new Uri(WebApp.BaseAddress),
                        "api/MaterialesInduccion"
                    );


                    respuesta.Resultado = JsonConvert.DeserializeObject<MaterialInduccion>(respuesta.Resultado.ToString());

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
        public async Task<IActionResult> Edit(string id, MaterialInduccion documentoInformacionInstitucional)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(
                        id, 
                        documentoInformacionInstitucional, 
                        new Uri(WebApp.BaseAddress),
                        "api/MaterialesInduccion"
                     );

                    if (response.IsSuccess)
                    {
                       
                        return this.RedireccionarMensajeTime(
                        "MaterialesInduccion",
                        "Index",
                        $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"7000"}";
                    return View(documentoInformacionInstitucional);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        



        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var respuestaMaterial = await apiServicio.SeleccionarAsync<Response>(
                    id, 
                    new Uri(WebApp.BaseAddress),
                    "api/MaterialesInduccion"
                );

                var materialinduccion = JsonConvert.DeserializeObject<MaterialInduccion>(respuestaMaterial.Resultado.ToString());

                var ext = Path.GetExtension(materialinduccion.Url);

                var response = await apiServicio.EliminarAsync(
                    id, 
                    new Uri(WebApp.BaseAddress)
                    , "api/MaterialesInduccion"
                );

                if (response.IsSuccess)
                {
                    var respuestaFile = uploadFileService.DeleteFile("MaterialInduccion", Convert.ToString(id), ext);
                    
                    return this.RedireccionarMensajeTime(
                        "MaterialesInduccion",
                        "Index",
                        $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
                }

                return this.RedireccionarMensajeTime(
                        "MaterialesInduccion",
                        "Index",
                        $"{Mensaje.Error}|{response.Message}|{"10000"}"
                    );

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        

        public async Task<IActionResult> Download(string id)
        {
            try {
                var response = await apiServicio.SeleccionarAsync<Response>(
                    id, 
                    new Uri(WebApp.BaseAddress),
                    "api/MaterialesInduccion"
                );

                var materialinduccion = JsonConvert.DeserializeObject<MaterialInduccion>(response.Resultado.ToString());
                var d = new MaterialInduccion
                {
                    IdMaterialInduccion = Convert.ToInt32(id),
                    Url = materialinduccion.Url
                };

                var ext = Path.GetExtension(materialinduccion.Url);

                if (ext != "")
                {
                    var responseGetFile = await apiServicio.ObtenerElementoAsync(
                        d,
                        new Uri(WebApp.BaseAddress),
                        "api/MaterialesInduccion/GetFile"
                    );

                    var m = JsonConvert.DeserializeObject<DocumentoInstitucionalTransfer>(responseGetFile.Resultado.ToString());



                    var fileName = string.Format("{0}{1}", $"{ responseGetFile.Message}", ext);
                    string mime = MimeKit.MimeTypes.GetMimeType(fileName);
                    return File(m.Fichero, mime, fileName);
                }
                else
                {
                    return this.RedireccionarMensajeTime(
                        "MaterialesInduccion",
                        "Index",
                        $"{Mensaje.Aviso}|{Mensaje.SinArchivo}|{"7000"}"
                    );
                }
            }
            catch (Exception ex)
            {

                return this.RedireccionarMensajeTime(
                        "MaterialesInduccion",
                        "Index",
                        $"{Mensaje.Aviso}|{Mensaje.SinArchivo}|{"7000"}"
                    );
            }

           
          
        }

        #endregion

    }
}