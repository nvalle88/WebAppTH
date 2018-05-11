using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ViewModels;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.entidades.Constantes;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace bd.webappth.web.Controllers.MVC
{
    public class FacturaViaticoController : Controller
    {
        private readonly IApiServicio apiServicio;


        public FacturaViaticoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
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
        public async Task<Response> CreateFichero(ViewModelFacturaViatico file)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(file,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FacturaViatico/InsertarFacturas");
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
        public async Task<IActionResult> Create(int IdSolicitudViatico, int IdItinerarioViatico)
        {
            ViewData["ItemViatico"] = new SelectList(await apiServicio.Listar<ItemViatico>(new Uri(WebApp.BaseAddress), "api/ItemViaticos/ListarItemViaticos"), "IdItemViatico", "Descripcion");
            var facturaViatico = new FacturaViatico
            {
                IdSolicitudViatico = IdSolicitudViatico,
                IdItinerarioViatico = IdItinerarioViatico

            };
            InicializarMensaje(null);
            return View(facturaViatico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewModelFacturaViatico viewModelFacturaViatico, List<IFormFile> files)
        {

            Response response = new Response();

            try
            {
                if (files.Count > 0)
                {
                    byte[] data;
                    using (var br = new BinaryReader(files[0].OpenReadStream()))
                        data = br.ReadBytes((int)files[0].OpenReadStream().Length);


                    var documenttransfer = new ViewModelFacturaViatico
                    {
                        NumeroFactura = viewModelFacturaViatico.NumeroFactura,
                        IdItinerarioViatico = viewModelFacturaViatico.IdItinerarioViatico,
                        FechaFactura = viewModelFacturaViatico.FechaFactura,
                        IdItemViatico = viewModelFacturaViatico.IdItemViatico,
                        ValorTotalFactura = viewModelFacturaViatico.ValorTotalFactura,
                        Observaciones = viewModelFacturaViatico.Observaciones,
                        Url = viewModelFacturaViatico.Url,
                        Fichero = data,
                    };
                    var respuesta = await CreateFichero(documenttransfer);
                    if (respuesta.IsSuccess)
                    {

                        return RedirectToAction("Informe", "ItinerarioViatico", new { IdSolicitudViatico = viewModelFacturaViatico.IdSolicitudViatico, IdItinerarioViatico = viewModelFacturaViatico.IdItinerarioViatico });
                    }

                }
                ViewData["ItemViatico"] = new SelectList(await apiServicio.Listar<ItemViatico>(new Uri(WebApp.BaseAddress), "api/ItemViaticos/ListarItemViaticos"), "IdItemViatico", "Descripcion");
                ViewData["Error"] = response.Message;
                return View(viewModelFacturaViatico);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Edit(string id, int IdSolicitudViatico)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/FacturaViatico");


                    respuesta.Resultado = JsonConvert.DeserializeObject<FacturaViatico>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        ViewData["ItemViatico"] = new SelectList(await apiServicio.Listar<ItemViatico>(new Uri(WebApp.BaseAddress), "api/ItemViaticos/ListarItemViaticos"), "IdItemViatico", "Descripcion");
                        ViewData["IdSolicitudViatico"] = IdSolicitudViatico;
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
        public async Task<IActionResult> Edit(string id, FacturaViatico facturaViatico)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, facturaViatico, new Uri(WebApp.BaseAddress),
                                                                 "api/FacturaViatico");

                    if (response.IsSuccess)
                    {
                        var solicitudViatico = new SolicitudViatico
                        {
                            IdSolicitudViatico = facturaViatico.IdSolicitudViatico
                        };
                        return RedirectToAction("Informe", "ItinerarioViatico", new { IdSolicitudViatico = facturaViatico.IdSolicitudViatico, IdItinerarioViatico = facturaViatico.IdItinerarioViatico });
                    }
                    ViewData["Error"] = response.Message;
                    return View(facturaViatico);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {


                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(string id, int IdSolicitudViatico, int IdItinerarioViatico)
        {
            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/FacturaViatico");
                if (response.IsSuccess)
                {

                    return RedirectToAction("Informe", "ItinerarioViatico", new { IdSolicitudViatico = IdSolicitudViatico, IdItinerarioViatico = IdItinerarioViatico });
                }
                return RedirectToAction("Informe", "ItinerarioViatico", new { IdSolicitudViatico = IdSolicitudViatico, IdItinerarioViatico = IdItinerarioViatico, mensaje = response.Message });
            }
            catch (Exception ex)
            {


                return BadRequest();
            }
        }
        public async Task<FileResult> Download(string id, int IdSolicitudViatico, int IdItinerarioViatico)
        {


            var id2 = new ViewModelFacturaViatico
            {
                IdFacturaViatico = Convert.ToInt32(id),
            };
            var response = await apiServicio.ObtenerElementoAsync(id2,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FacturaViatico/GetFile");

            
                var m = JsonConvert.DeserializeObject<ViewModelFacturaViatico>(response.Resultado.ToString());
                var fileName = $"{ response.Message}.pdf";
                return File(m.Fichero, "application/pdf", fileName);

                          
        }
    }
}