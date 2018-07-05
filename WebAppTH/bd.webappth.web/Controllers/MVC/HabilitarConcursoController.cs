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
        public async Task<IActionResult> Create(int id, int vacante)
        {
            var usario = new ViewModelPartidaFase
            {
                Idindiceocupacional = id,
                Vacantes = vacante

            };
            await Cargarcombos();
            InicializarMensaje(null);
            return View(usario);
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
        public async Task<IActionResult> Create(ViewModelPartidaFase partidasFase)
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
                //if (partidasFase.VacantesCredo <= partidasFase.Vacantes)
                //{
                response = await apiServicio.InsertarAsync(partidasFase,
                                                                     new Uri(WebApp.BaseAddress),
                                                                     "api/HabilitarConcurso/InsertarHabilitarConsurso");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                await Cargarcombos();
                //}
                await Cargarcombos();
                //ViewData["Error"] = "Numero de Vancante superior";
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
                    var usario = new ViewModelPartidaFase
                    {
                        IdPartidaFase = Convert.ToInt32(id),

                    };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(usario, new Uri(WebApp.BaseAddress),
                                                                  "api/HabilitarConcurso/Edit");
                    respuesta.Resultado = JsonConvert.DeserializeObject<ViewModelPartidaFase>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        await Cargarcombos();
                        InicializarMensaje(null);
                        return View(respuesta.Resultado);
                    }

                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ViewModelPartidaFase viewModelPartidaFase)
        {
            try
            {
                var respuesta = await apiServicio.EditarAsync<Response>(viewModelPartidaFase, new Uri(WebApp.BaseAddress),
                                                              "api/HabilitarConcurso/Editar");
                if (respuesta.IsSuccess)
                {
                    InicializarMensaje(null);
                    return RedirectToAction("Index");
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task Cargarcombos()
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