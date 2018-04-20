using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using bd.webappth.entidades.Constantes;
using Microsoft.AspNetCore.Http;

namespace bd.webappth.web.Controllers.MVC
{
    public class SeleccionPersonalTalentoHumanoController : Controller
    {

        private readonly IApiServicio apiServicio;


        public SeleccionPersonalTalentoHumanoController(IApiServicio apiServicio)
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
        public async Task<IActionResult> IndexCandidatosPostulados(int partida)
        {
            try
            {
                InicializarMensaje(null);

                var usuario = new ViewModelSeleccionPersonal
                {
                    IdPartidaFase = partida

                };
                var response = await apiServicio.ObtenerElementoAsync1<ViewModelSeleccionPersonal>(usuario, new Uri(WebApp.BaseAddress)
                                                                         , "api/SeleccionPersonalTalentoHumano/ListaCanditadoPostulados");

                return View(response);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {
            InicializarMensaje(mensaje);
            var lista = new List<ViewModelSeleccionPersonal>();
            try
            {
                lista = await apiServicio.Listar<ViewModelSeleccionPersonal>(new Uri(WebApp.BaseAddress),
                                                                    "api/SeleccionPersonalTalentoHumano/ListarPuestoVacantesSeleccionPersonal");

                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                mensaje = Mensaje.Excepcion;
                return View(lista);
            }
        }
        public async Task<IActionResult> Create(int id, int partida)
        {
            if (HttpContext.Session.GetInt32(Constantes.idDependeciaConcursoSession) != id && HttpContext.Session.GetInt32(Constantes.idParidaFaseConcursoSession) != partida)
            {
                HttpContext.Session.SetInt32(Constantes.idParidaFaseConcursoSession, partida);
                HttpContext.Session.SetInt32(Constantes.idDependeciaConcursoSession, id);
                HttpContext.Session.SetInt32(Constantes.idCandidatoConcursoSession, 0);
            }

            var candidato = ObtenerCandidato();
            try
            {
                var usuario = new ViewModelSeleccionPersonal
                {
                    IdPartidaFase = candidato.IdPartidaFase,
                    iddependecia = candidato.iddependecia

                };
                var postular = await obtenercabecera(candidato);
                if (postular != null)
                {
                    InicializarMensaje(null);
                    return View(postular);
                }

                ViewData["Error"] = candidato.ToString();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> CreateCandidatoConcurso()
        {
            try
            {
                var candidato = ObtenerCandidato();
                InicializarMensaje(null);
                var usuario = new ViewModelSeleccionPersonal
                {
                    IdCandidato = candidato.IdCandidato,
                    iddependecia = candidato.iddependecia,
                    IdPartidaFase = candidato.IdPartidaFase
                };
                var response = await apiServicio.InsertarAsync(usuario, new Uri(WebApp.BaseAddress), "api/SeleccionPersonalTalentoHumano/InsertarCandidatoConcurso");
                if (response.IsSuccess)
                {
                    HttpContext.Session.SetInt32(Constantes.idParidaFaseConcursoSession, 0);
                    HttpContext.Session.SetInt32(Constantes.idCandidatoConcursoSession, 0);
                    HttpContext.Session.SetInt32(Constantes.idDependeciaConcursoSession, 0);
                    return RedirectToAction("Index");
                }
                return View(usuario);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> CreateDatosPersonales(int partida)
        {
            try
            {
                var candidato = ObtenerCandidato();
                InicializarMensaje(null);
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = candidato.iddependecia,
                    IdPartidaFase = candidato.IdPartidaFase
                };
                return View(usuario);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDatosPersonales(ViewModelSeleccionPersonal viewModelSeleccionPersona)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    InicializarMensaje(null);
                    var postular = await obtenercabecera(viewModelSeleccionPersona);
                    return View(postular);
                }
                else
                {
                    var response = await apiServicio.InsertarAsync(viewModelSeleccionPersona, new Uri(WebApp.BaseAddress), "api/SeleccionPersonalTalentoHumano/InsertarCandidato");
                    if (response.IsSuccess)
                    {
                        var candidato = JsonConvert.DeserializeObject<ViewModelSeleccionPersonal>(response.Resultado.ToString());
                        int idCandidato = candidato.IdCandidato;
                        HttpContext.Session.SetInt32(Constantes.idCandidatoConcursoSession, idCandidato);
                        return RedirectToAction("IndexInstruccionFormal", idCandidato);
                    }
                    ViewData["Error"] = Mensaje.ExisteEmpleado;
                    return View(viewModelSeleccionPersona);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> IndexInstruccionFormal(int idCandidato)
        {
            try
            {
                //if (HttpContext.Session.GetInt32(Constantes.idCandidatoConcursoSession) != idCandidato)
                //{
                //    HttpContext.Session.SetInt32(Constantes.idCandidatoConcursoSession, idCandidato);
                //   // HttpContext.Session.SetInt32(Constantes.idParidaFaseConcursoSession, partida);
                //}
                InicializarMensaje(null);
                var candidato = ObtenerCandidato();
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = candidato.iddependecia,
                    IdPartidaFase = candidato.IdPartidaFase,
                    IdCandidato = candidato.IdCandidato

                };
                var lista = new List<CandidatoEstudio>();
                lista = await apiServicio.ObtenerElementoAsync1<List<CandidatoEstudio>>(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Candidatos/ListarEstudiosporCandidato");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> CreateInstrucionFormal(int id)
        {
            try
            {
                InicializarMensaje(null);
                var candidato = ObtenerCandidato();
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = candidato.iddependecia,
                    IdPartidaFase = candidato.IdPartidaFase,
                    IdCandidato = candidato.IdCandidato

                };
                ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                return View(usuario);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInstrucionFormal(ViewModelSeleccionPersonal viewModelSeleccionPersonal)
        {
            try
            {
                var candidato = ObtenerCandidato();
                var candidatoEstudio = new CandidatoEstudio()
                {
                    FechaGraduado = viewModelSeleccionPersonal.FechaGraduado,
                    Observaciones = viewModelSeleccionPersonal.Observaciones,
                    IdTitulo = viewModelSeleccionPersonal.IdTitulo,
                    NoSenescyt = Convert.ToString(viewModelSeleccionPersonal.NoSenescyt),
                    IdCandidato = candidato.IdCandidato
                };

                var response = await apiServicio.InsertarAsync(candidatoEstudio,
                                                         new Uri(WebApp.BaseAddress),
                                                         "api/Candidatos/InsertarCandidatoEstudio");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexInstruccionFormal");
                }
                ViewData["Error"] = Mensaje.ExisteEmpleado;
                return View(candidatoEstudio);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IActionResult> IndexExperiencia(int partida, int idCandidato)
        {
            InicializarMensaje(null);
            var candidato = ObtenerCandidato();
            var usuario = new ViewModelSeleccionPersonal
            {
                iddependecia = candidato.iddependecia,
                IdPartidaFase = candidato.IdPartidaFase,
                IdCandidato = candidato.IdCandidato

            };
            var lista = new List<CandidatoTrayectoriaLaboral>();
            lista = await apiServicio.ObtenerElementoAsync1<List<CandidatoTrayectoriaLaboral>>(usuario, new Uri(WebApp.BaseAddress)
                                                                , "api/Candidatos/ListarTrayectoriasLaborales");
            return View(lista);
        }
        public async Task<IActionResult> CreateExperiencia(int partida)
        {
            try
            {
                InicializarMensaje(null);
                var candidato = ObtenerCandidato();
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = candidato.iddependecia,
                    IdPartidaFase = candidato.IdPartidaFase,
                    IdCandidato = candidato.IdCandidato

                };
                return View(usuario);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExperiencia(ViewModelSeleccionPersonal viewModelSeleccionPersonal)
        {
            try
            {
                var candidato = ObtenerCandidato();
                var candidatoTrayectoria = new CandidatoTrayectoriaLaboral()
                {
                    FechaInicio = viewModelSeleccionPersonal.fechainicio,
                    FechaFin = viewModelSeleccionPersonal.fechahasta,
                    Institucion = viewModelSeleccionPersonal.Instituacion,
                    IdCandidato = candidato.IdCandidato
                };
                candidatoTrayectoria.IdCandidato = candidato.IdCandidato;

                var response = await apiServicio.InsertarAsync(candidatoTrayectoria,
                                                         new Uri(WebApp.BaseAddress),
                                                         "api/Candidatos/InsertarTrayectoriaLaboral");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexExperiencia");
                }
                ViewData["Error"] = Mensaje.ExisteEmpleado;
                return View(candidatoTrayectoria);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> Evaluar(int id)
        {
            try
            {
                InicializarMensaje(null);

                var usuario = new ViewModelEvaluarCandidato
                {
                    IdCandidato = id

                };
                var response = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluarCandidato>(usuario, new Uri(WebApp.BaseAddress)
                                                                         , "api/SeleccionPersonalTalentoHumano/CandidatoEvaluar");

                return View(response);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<ViewModelSeleccionPersonal> obtenercabecera(ViewModelSeleccionPersonal viewModelSeleccionPersonal)
        {
            var response = await apiServicio.ObtenerElementoAsync1<ViewModelSeleccionPersonal>(viewModelSeleccionPersonal, new Uri(WebApp.BaseAddress)
                                                                   , "api/SeleccionPersonalTalentoHumano/ObtenerEncabezadopostulante");
            //if (response!= null)
            //{
            //    var idependecia =response.iddependecia;

            //}
            return response;
            
        }
        public ViewModelSeleccionPersonal ObtenerCandidato()
        {
            var candidato = new ViewModelSeleccionPersonal
            {
                iddependecia = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idDependeciaConcursoSession)),
                IdPartidaFase = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idParidaFaseConcursoSession)),
                IdCandidato = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idCandidatoConcursoSession)),
            };
            return candidato;
        }
    }
}