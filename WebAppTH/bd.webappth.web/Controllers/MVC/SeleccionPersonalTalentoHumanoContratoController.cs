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
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class SeleccionPersonalTalentoHumanoContratoController : Controller
    {

        private readonly IApiServicio apiServicio;


        public SeleccionPersonalTalentoHumanoContratoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }


        public async Task<IActionResult> Index(string mensaje)
        {
            var lista = new List<ViewModelSeleccionPersonal>();
            try
            {
                lista = await apiServicio.Listar<ViewModelSeleccionPersonal>(
                    new Uri(WebApp.BaseAddress),
                    "api/SeleccionPersonalTalentoHumanoContrato/ListarPuestoVacantesSeleccionPersonal"
                );

                SetCabecera(0,0,0,0);

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
            var cabecera = getCabecera();

            if (cabecera.IdPartidaFase == 0 || cabecera.iddependecia == 0) {
                SetCabecera(partida,0,0,id);

                cabecera = getCabecera();
            }
            
            try
            {
                var modelo = new ViewModelSeleccionPersonal
                {
                    IdPartidaFase = cabecera.IdPartidaFase,
                    IdCandidato = cabecera.IdCandidato,
                    IdCandidatoConcurso = cabecera.IdCandidatoConcurso,
                    iddependecia = id

                };

                var modeloRespuesta = await apiServicio.ObtenerElementoAsync1<ViewModelSeleccionPersonal>(
                    modelo, 
                    new Uri(WebApp.BaseAddress),
                    "api/SeleccionPersonalTalentoHumanoContrato/ObtenerViewModelSeleccionPersonal"
                );
                

                return View(modeloRespuesta);
              

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
                var cabecera = getCabecera();

                var usuario = new ViewModelSeleccionPersonal
                {
                    IdPartidaFase = cabecera.IdPartidaFase,
                    IdCandidato = cabecera.IdCandidato,
                    IdCandidatoConcurso = cabecera.IdCandidatoConcurso,
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
                    return View(viewModelSeleccionPersona);
                }


                var cabecera = getCabecera();

                var response = await apiServicio.InsertarAsync(
                        viewModelSeleccionPersona, new Uri(WebApp.BaseAddress),
                        "api/SeleccionPersonalTalentoHumanoContrato/InsertarCandidato");


                if (response.IsSuccess)
                {
                    var modeloRespuesta = JsonConvert.DeserializeObject<ViewModelSeleccionPersonal>(response.Resultado.ToString());
                    

                    SetCabecera(viewModelSeleccionPersona.IdPartidaFase,modeloRespuesta.IdCandidato, modeloRespuesta.IdCandidatoConcurso, (int) cabecera.iddependecia);

                    
                    //return RedirectToAction("IndexInstruccionFormal", idCandidato);

                    return this.RedireccionarMensajeTime(
                           "SeleccionPersonalTalentoHumanoContrato",
                           "IndexInstruccionFormal",
                           new { idCandidato = modeloRespuesta.IdCandidato },
                           $"{Mensaje.Success}|{response.Message}|{"7000"}"
                        );

                }

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ExisteEmpleado}|{"10000"}";
                
                return View(viewModelSeleccionPersona);

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
                var cabecera = getCabecera();

                if (cabecera.IdCandidato == 0) {

                    return this.RedireccionarMensajeTime(
                           "SeleccionPersonalTalentoHumanoContrato",
                           "CreateDatosPersonales",
                           new { partida = cabecera.IdPartidaFase},
                           $"{Mensaje.Aviso}|{Mensaje.ErrorCandidatoNoSeleccionado}|{"7000"}"
                        );
                }

                /*
                var usuario = new ViewModelSeleccionPersonal
                {
                    iddependecia = candidato.iddependecia,
                    IdPartidaFase = candidato.IdPartidaFase,
                    IdCandidato = candidato.IdCandidato

                };
                var lista = new List<CandidatoEstudio>();
                lista = await apiServicio.ObtenerElementoAsync1<List<CandidatoEstudio>>(usuario, new Uri(WebApp.BaseAddress)
                                                                    , "api/Candidatos/ListarEstudiosporCandidato");
                */
                var lista = new List<CandidatoEstudio>();

                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexExperiencia(int partida, int idCandidato)
        {
            /*
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
            */

            try
            {
                var cabecera = getCabecera();

                if (cabecera.IdCandidato == 0) {

                    return this.RedireccionarMensajeTime(
                           "SeleccionPersonalTalentoHumanoContrato",
                           "CreateDatosPersonales",
                           new { partida = cabecera.IdPartidaFase },
                           $"{Mensaje.Aviso}|{Mensaje.ErrorCandidatoNoSeleccionado}|{"7000"}"
                        );

                }


                var lista = new List<CandidatoTrayectoriaLaboral>();
                return View(lista);
            }
            catch (Exception){

                return BadRequest();
            }
        }


        public async Task<IActionResult> IndexCandidatosPostulados(int partida)
        {
            try
            {

                var usuario = new ViewModelSeleccionPersonal
                {
                    IdPartidaFase = partida

                };
                var response = await apiServicio.ObtenerElementoAsync1<ViewModelSeleccionPersonal>(
                    usuario,
                    new Uri(WebApp.BaseAddress)
                    , "api/SeleccionPersonalTalentoHumanoContrato/ListaCanditadoPostulados");

                return View(response);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }




        /*
        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }

        
        [HttpPost]
        public async Task<JsonResult> BusquedaCandidatos(string identificacion)
        {
            var candidato = new Candidato
            {
                Identificacion = Convert.ToString( identificacion)
            };
            var listaAreasConocimientos = await apiServicio.ObtenerElementoAsync1<Candidato>(candidato, new Uri(WebApp.BaseAddress), "api/SeleccionPersonalTalentoHumano/ObtenerCandidato");
            if (listaAreasConocimientos != null)
            {
                int idCandidato = listaAreasConocimientos.IdCandidato;
                HttpContext.Session.SetInt32(Constantes.idCandidatoConcursoSession, idCandidato);
            }
            return Json(listaAreasConocimientos);
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
                var response = await apiServicio.InsertarAsync(usuario, new Uri(WebApp.BaseAddress), "api/SeleccionPersonalTalentoHumanoContrato/InsertarCandidatoConcurso");
                if (response.IsSuccess)
                {
                    HttpContext.Session.SetInt32(Constantes.idPartidaFaseConcursoSession, 0);
                    HttpContext.Session.SetInt32(Constantes.idCandidatoConcursoSession, 0);
                    HttpContext.Session.SetInt32(Constantes.idDependeciaConcursoSession, 0);
                    return RedirectToAction("Index");
                }
                return View();

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
        public async Task<IActionResult> Evaluar(int id,int a)
        {
            try
            {
                InicializarMensaje(null);

                var usuario = new ViewModelEvaluarCandidato
                {
                    IdCandidato = id,
                    IdPartidaFase = a

                };
                var response = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluarCandidato>(usuario, new Uri(WebApp.BaseAddress)
                                                                         , "api/SeleccionPersonalTalentoHumanoContrato/CandidatoEvaluar");

                return View(response);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> DetalleEvaluar(int id)
        {
            try
            {
                InicializarMensaje(null);

                var usuario = new ViewModelEvaluarCandidato
                {
                    IdCandidato = id

                };
                var response = await apiServicio.ObtenerElementoAsync1<ViewModelEvaluarCandidato>(usuario, new Uri(WebApp.BaseAddress)
                                                                         , "api/SeleccionPersonalTalentoHumanoContrato/DetalleCandidatoEvaluar");

                return View(response);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Evaluar(ViewModelEvaluarCandidato viewModelEvaluarCandidato)
        {
            try
            {
                InicializarMensaje(null);

                
                var response = await apiServicio.InsertarAsync<ViewModelEvaluarCandidato>(viewModelEvaluarCandidato, new Uri(WebApp.BaseAddress)
                                                                         , "api/SeleccionPersonalTalentoHumanoContrato/InsertarEvaluacion");

                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                return View(viewModelEvaluarCandidato);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        */

        /*
        public async Task<ViewModelSeleccionPersonal> obtenercabecera(ViewModelSeleccionPersonal viewModelSeleccionPersonal)
        {
            var response = await apiServicio.ObtenerElementoAsync1<ViewModelSeleccionPersonal>(viewModelSeleccionPersonal, new Uri(WebApp.BaseAddress)
                                                                   , "api/SeleccionPersonalTalentoHumanoContrato/ObtenerEncabezadopostulante");
            
            return response;
            
        }
        */

        public ViewModelSeleccionPersonal getCabecera()
        {
            var modelo = new ViewModelSeleccionPersonal
            {
                IdPartidaFase = 0,
                IdCandidato = 0,
                IdCandidatoConcurso = 0,
                iddependecia = 0
            };

            try {
                
                modelo = new ViewModelSeleccionPersonal
                {
                    IdPartidaFase = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idPartidaFaseConcursoSession)),
                    IdCandidato = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idCandidatoSession)),
                    IdCandidatoConcurso = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idCandidatoConcursoSession)),
                    iddependecia = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idDependeciaConcursoSession))
                };

            } catch (Exception ex)
            {

                
            }

            
            return modelo;
        }


        public void SetCabecera(int IdPartidaFase, int IdCandidato, int IdCandidatoConcurso,int IdDependencia)
        {

            HttpContext.Session.SetInt32(Constantes.idPartidaFaseConcursoSession, IdPartidaFase);
            HttpContext.Session.SetInt32(Constantes.idCandidatoSession, IdCandidato);
            HttpContext.Session.SetInt32(Constantes.idCandidatoConcursoSession, IdCandidatoConcurso);
            HttpContext.Session.SetInt32(Constantes.idDependeciaConcursoSession, IdDependencia);

        }


    }
}