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
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class DocumentosIngresoController : Controller
    {
        private readonly IApiServicio apiServicio;


        public DocumentosIngresoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }
        

        #region Métodos para mantenimiento DocumentosIngreso

        public async Task<IActionResult> Index()
        {

            var lista = new List<DocumentosIngreso>();
            try
            {
                lista = await apiServicio.Listar<DocumentosIngreso>(
                    new Uri(WebApp.BaseAddress),
                    "api/DocumentosIngreso/ListarDocumentosIngreso"
                );
                
                return View(lista);
            }
            catch (Exception ex)
            {
               
                return BadRequest();
            }
        }


        public IActionResult Create(string mensaje)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentosIngreso documentosIngreso)
        {
            if (!ModelState.IsValid)
            {
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"7000"}";
                return View(documentosIngreso);

            }
            
            Response response = new Response();

            try
            {

                response = await apiServicio.InsertarAsync(
                    documentosIngreso,
                    new Uri(WebApp.BaseAddress),
                    "api/DocumentosIngreso/InsertarDocumentosIngreso"
                );

                if (response.IsSuccess)
                {
                    
                    return this.RedireccionarMensajeTime(
                        "DocumentosIngreso",
                        "Index",
                        $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );

                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                return View(documentosIngreso);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {

                    var idDocumentosIngreso = Convert.ToInt32(id);

                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(
                        new DocumentosIngreso { IdDocumentosIngreso = idDocumentosIngreso}, 
                        new Uri(WebApp.BaseAddress),
                        "api/DocumentosIngreso/ObtenerDocumentosIngresoPorId"
                    );


                    respuesta.Resultado = JsonConvert.DeserializeObject<DocumentosIngreso>(respuesta.Resultado.ToString());

                    if (respuesta.IsSuccess)
                    {
                        return View(respuesta.Resultado);
                    }

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{ respuesta.Message}|{"10000"}";
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
        public async Task<IActionResult> Edit(DocumentosIngreso documentosIngreso)
        {
            Response response = new Response();
            try
            {
                if (!ModelState.IsValid)
                {
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"7000"}";
                    return View(documentosIngreso);

                }
                

                response = await apiServicio.EditarAsync<Response>(
                    documentosIngreso,
                    new Uri(WebApp.BaseAddress),
                    "api/DocumentosIngreso/EditarDocumentosIngreso"
                );

                if (response.IsSuccess)
                {

                    return this.RedireccionarMensajeTime(
                        "DocumentosIngreso",
                        "Index",
                        $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );

                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";

                return View(documentosIngreso);

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
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest();
                }

                var idDocumentosIngreso = Convert.ToInt32(id);

                var response = await apiServicio.EliminarAsync(
                    new DocumentosIngreso{IdDocumentosIngreso = idDocumentosIngreso }, 
                    new Uri(WebApp.BaseAddress),
                    "api/DocumentosIngreso/EliminarDocumentosIngreso"
                );


                if (response.IsSuccess)
                {
                    
                    return this.RedireccionarMensajeTime(
                        "DocumentosIngreso",
                        "Index",
                        $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
                }

                return this.RedireccionarMensajeTime(
                    "DocumentosIngreso",
                    "Index",
                    $"{Mensaje.Error}|{response.Message}|{"10000"}"
                );
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        #endregion

        
    }
}