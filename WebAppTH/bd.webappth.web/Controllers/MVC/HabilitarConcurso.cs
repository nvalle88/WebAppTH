using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.Negocio;
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class HabilitarConcursoController : Controller
    {
        private readonly IApiServicio apiServicio;


        public HabilitarConcursoController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Create()
        {
            await Cargarcombos();
            InicializarMensaje(null);
            return View();
        }

        public async Task<IActionResult> Index()
        {

            var lista = new List<ViewModelPartidaFase>();
            try
            {
                lista = await apiServicio.Listar<ViewModelPartidaFase>(new Uri(WebApp.BaseAddress)
                                                                    , "api/HabilitarConcurso/ListarConcursosVacantes");
                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartidasFase partidasFase)
        {
            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                await Cargarcombos();
                
                return View(partidasFase);
            }
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(partidasFase,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/HabilitarConcurso/InsertarHabilitarConsurso");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Create");
                }
                await Cargarcombos();
                ViewData["Error"] = response.Message;
                return View(partidasFase);

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
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/Sexos");


                    respuesta.Resultado = JsonConvert.DeserializeObject<Sexo>(respuesta.Resultado.ToString());
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Sexo Sexo)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, Sexo, new Uri(WebApp.BaseAddress),
                                                                 "api/Sexos");

                    if (response.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    ViewData["Error"] = response.Message;
                    return View(Sexo);

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
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/Sexos");
                if (response.IsSuccess)
                {
                    
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = response.Message });
                //return BadRequest();
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }
        public async Task Cargarcombos ()
        {
            try
            {
                ViewData["IdIndiceOcupacional"] = new SelectList(await apiServicio.Listar<ViewModelPartidaFase>(new Uri(WebApp.BaseAddress), "api/HabilitarConcurso/ListarPuestoVacantes"), "Idindiceocupacional", "PuestoInstitucional");
                ViewData["IdTipoConcurso"] = new SelectList(await apiServicio.Listar<TipoConcurso>(new Uri(WebApp.BaseAddress), "api/TiposConcurso/ListarTiposConcurso"), "IdTipoConcurso", "Nombre");

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}