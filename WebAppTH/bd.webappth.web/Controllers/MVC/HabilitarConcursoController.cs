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
using bd.webappth.servicios.Extensores;

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
        public async Task<IActionResult> Create(ViewModelPartidaFase partidasFaseViewModel)
        {
            if (!ModelState.IsValid)
            {
                await Cargarcombos();

                return View(partidasFaseViewModel);
            }

            Response response = new Response();

            try
            {
                if (partidasFaseViewModel.VacantesCreadas > partidasFaseViewModel.Vacantes) {

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ErrorIngresoVacantes}|{"7000"}";
                    return View(partidasFaseViewModel);
                }


                var partidasFase = new PartidasFase
                {
                    IdTipoConcurso = partidasFaseViewModel.IdTipoConcurso,
                    Contrato = partidasFaseViewModel.Contrato,
                    IdIndiceOcupacional = partidasFaseViewModel.Idindiceocupacional,
                    Vacantes = partidasFaseViewModel.VacantesCreadas
                };

                response = await apiServicio.InsertarAsync(
                    partidasFase,
                    new Uri(WebApp.BaseAddress),
                    "api/HabilitarConcurso/InsertarHabilitarConcurso"
                );


                if (response.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                       "HabilitarConcurso",
                       "Index",
                       $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
                }

                await Cargarcombos();
                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Resultado}|{"7000"}";

                return View(partidasFase);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> Edit(string id, int vacantesDisponibles)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var modelo = new ViewModelPartidaFase
                    {
                        IdPartidaFase = Convert.ToInt32(id),
                        Vacantes = vacantesDisponibles

                    };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(modelo, new Uri(WebApp.BaseAddress),
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
        public async Task<IActionResult> Edit(ViewModelPartidaFase partidasFaseViewModel)
        {
            try
            {
                if (partidasFaseViewModel.VacantesCreadas > partidasFaseViewModel.Vacantes)
                {

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ErrorIngresoVacantes}|{"7000"}";
                    return View(partidasFaseViewModel);
                }


                var partidasFase = new PartidasFase
                {
                    IdPartidasFase = partidasFaseViewModel.IdPartidaFase,
                    Vacantes = partidasFaseViewModel.VacantesCreadas
                };

                var response = await apiServicio.EditarAsync<Response>(
                    partidasFase,
                    new Uri(WebApp.BaseAddress),
                    "api/HabilitarConcurso/Editar"
                );

                if (response.IsSuccess)
                {
                    return this.RedireccionarMensajeTime(
                       "HabilitarConcurso",
                       "Index",
                       $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Resultado}|{"7000"}";

                return View(partidasFaseViewModel);
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