using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Constantes;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class FichaCandidatosController : Controller
    {
        private readonly IApiServicio apiServicio;


        public FichaCandidatosController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detalle(FichaCandidatoViewModel fichaCandidato)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.CorregirFormulario);
                return View(fichaCandidato);
            }

            try
            { 
                var response = await apiServicio.EditarAsync<Response>(fichaCandidato, new Uri(WebApp.BaseAddress), "api/Candidatos/EditarCandidato");

                if (response.IsSuccess)
                {
                    return RedirectToAction("Detalle", new { mensaje = Mensaje.GuardadoSatisfactorio, idCandidato = HttpContext.Session.GetInt32(Constantes.idCandidatoSession) });
                }
               // await CargarCombosEmpleado(datosBasicosEmpleado);
                ViewData["Error"] = Mensaje.ExisteEmpleado;
                return View(fichaCandidato);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> Detalle(string mensaje, int idCandidato,int idPersona)
        {

            if (HttpContext.Session.GetInt32(Constantes.idCandidatoSession) != idCandidato)
            {
                HttpContext.Session.SetInt32(Constantes.idCandidatoSession, idCandidato);
                HttpContext.Session.SetInt32(Constantes.idCandidatoPersonaSession, idPersona);
            }

            var candidato = ObtenerCandidato();

            var empleadoActual = new FichaCandidatoViewModel { IdCandidato = candidato.IdCandidato };
            var response = await apiServicio.ObtenerElementoAsync1<Response>(empleadoActual,
                                                         new Uri(WebApp.BaseAddress),
                                                         "api/Candidatos/ObtenerFichaCandidato");
            if (response.IsSuccess)
            {
                InicializarMensaje(mensaje);
                var empleadoRespuesta = JsonConvert.DeserializeObject<FichaCandidatoViewModel>(response.Resultado.ToString());
                return View(empleadoRespuesta);
            }
            return BadRequest();
        }

        public async Task<IActionResult> Index()
            {
              
                try
                {
                  var  lista = await apiServicio.Listar<FichaCandidatoViewModel>(new Uri(WebApp.BaseAddress)
                                                                        , "api/Candidatos/ListarCandidatos");

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
        public async Task<IActionResult> Create(FichaCandidatoViewModel fichaCandidato)
        {


            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.CorregirFormulario);
                return View(fichaCandidato);
            }

            try
            {
                var response = await apiServicio.InsertarAsync(fichaCandidato, new Uri(WebApp.BaseAddress), "api/Empleados/InsertarEmpleado");

                if (response.IsSuccess)
                {
                    var empleado = JsonConvert.DeserializeObject<Empleado>(response.Resultado.ToString());
                    return RedirectToAction("AgregarDistributivo", new { IdEmpleado = empleado.IdEmpleado });
                }
                ViewData["Error"] = Mensaje.ExisteEmpleado;
                return View(fichaCandidato);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> CreatePersonaEstudio()
        {
            ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
            return View();
        }

        public IActionResult CreateTrayectoriaLaboral()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrayectoriaLaboral(ViewModelTrayectoriaLaboral viewModelTrayectoriaLaboral)
        {
            Response response = new Response();
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(viewModelTrayectoriaLaboral);
                }

                var candidato = ObtenerCandidato();

                var trayectoriaLaboral = new TrayectoriaLaboral
                {
                    FechaInicio = viewModelTrayectoriaLaboral.FechaInicio,
                    FechaFin = viewModelTrayectoriaLaboral.FechaFin,
                    Empresa = viewModelTrayectoriaLaboral.Empresa,
                    PuestoTrabajo = viewModelTrayectoriaLaboral.PuestoTrabajo,
                    DescripcionFunciones = viewModelTrayectoriaLaboral.DescripcionFunciones,
                    AreaAsignada = viewModelTrayectoriaLaboral.AreaAsignada,
                    FormaIngreso = viewModelTrayectoriaLaboral.FormaIngreso,
                    MotivoSalida = viewModelTrayectoriaLaboral.MotivoSalida,
                    TipoInstitucion = viewModelTrayectoriaLaboral.TipoInstitucion,
                };

                trayectoriaLaboral.IdPersona = candidato.IdPersona;

                response = await apiServicio.InsertarAsync(trayectoriaLaboral,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TrayectoriasLaborales/InsertarTrayectoriaLaboral");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexTrayectoriaLaboral");
                }

                ViewData["Error"] = response.Message;
                return View(viewModelTrayectoriaLaboral);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> EditTrayectoriaLaboral(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/TrayectoriasLaborales");


                    var trayectorialaboral = JsonConvert.DeserializeObject<TrayectoriaLaboral>(respuesta.Resultado.ToString());

                    if (respuesta.IsSuccess)
                    {
                        var viewmodelTrayectoriaLaboral = new ViewModelTrayectoriaLaboral()
                        {
                            IdTrayectoriaLaboral = trayectorialaboral.IdTrayectoriaLaboral,
                            FechaInicio = trayectorialaboral.FechaInicio,
                            FechaFin = trayectorialaboral.FechaFin,
                            Empresa = trayectorialaboral.Empresa,
                            PuestoTrabajo = trayectorialaboral.PuestoTrabajo,
                            DescripcionFunciones = trayectorialaboral.DescripcionFunciones,

                            AreaAsignada = trayectorialaboral.AreaAsignada,
                            FormaIngreso = trayectorialaboral.FormaIngreso,
                            MotivoSalida = trayectorialaboral.MotivoSalida,
                            TipoInstitucion = trayectorialaboral.TipoInstitucion,



                        };
                        return View(viewmodelTrayectoriaLaboral);
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
        public async Task<IActionResult> EditTrayectoriaLaboral(string id, ViewModelTrayectoriaLaboral viewModelTrayectoriaLaboral)
        {
            Response response = new Response();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModelTrayectoriaLaboral);
                }

                var candidato = ObtenerCandidato();
                var trayectoriaLaboral = new TrayectoriaLaboral()
                {
                    IdTrayectoriaLaboral = viewModelTrayectoriaLaboral.IdTrayectoriaLaboral,
                    FechaInicio = viewModelTrayectoriaLaboral.FechaInicio,
                    FechaFin = viewModelTrayectoriaLaboral.FechaFin,
                    Empresa = viewModelTrayectoriaLaboral.Empresa,
                    PuestoTrabajo = viewModelTrayectoriaLaboral.PuestoTrabajo,
                    DescripcionFunciones = viewModelTrayectoriaLaboral.DescripcionFunciones,
                    AreaAsignada = viewModelTrayectoriaLaboral.AreaAsignada,
                    FormaIngreso = viewModelTrayectoriaLaboral.FormaIngreso,
                    MotivoSalida = viewModelTrayectoriaLaboral.MotivoSalida,
                    TipoInstitucion = viewModelTrayectoriaLaboral.TipoInstitucion,
                };

                trayectoriaLaboral.IdPersona = candidato.IdPersona;

                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, trayectoriaLaboral, new Uri(WebApp.BaseAddress),
                                                                 "api/TrayectoriasLaborales");

                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexTrayectoriaLaboral");
                    }
                    ViewData["Error"] = response.Message;
                    return View(viewModelTrayectoriaLaboral);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexTrayectoriaLaboral()
        {

            var candidato = ObtenerCandidato();
            var lista = new List<TrayectoriaLaboral>();
            try
            {
                lista = await apiServicio.ObtenerElementoAsync1<List<TrayectoriaLaboral>>(candidato, new Uri(WebApp.BaseAddress)
                                                                    , "api/TrayectoriasLaborales/ListarTrayectoriasLaboralesporEmpleado");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeleteTrayectoriaLaboral(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/TrayectoriasLaborales");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexTrayectoriaLaboral");
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
        public async Task<IActionResult> CreatePersonaEstudio(ViewModelPersonaEstudio viewModelPersonaEstudio)
        {
            var candidato = ObtenerCandidato();
            var personaEstudio = new PersonaEstudio()
            {
                FechaGraduado = viewModelPersonaEstudio.FechaGraduado,
                Observaciones = viewModelPersonaEstudio.Observaciones,
                IdTitulo = viewModelPersonaEstudio.IdTitulo,
                NoSenescyt = viewModelPersonaEstudio.NoSenescyt
            };

            personaEstudio.IdPersona = candidato.IdPersona;

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(personaEstudio,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/PersonasEstudios/InsertarPersonaEstudio");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexPersonaEstudio");
                }

                ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                ViewData["Error"] = response.Message;
                return View(viewModelPersonaEstudio);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        private async Task CargarListaComboEdit(Titulo titulo)
        {

            var listaAreasConocimientos = await apiServicio.Listar<AreaConocimiento>(titulo, new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientosporEstudio");

            var listaTitulos = await apiServicio.Listar<Titulo>(titulo, new Uri(WebApp.BaseAddress), "api/Titulos/ListarTitulosporAreaConocimiento");


            ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

            ViewData["IdAreaConocimiento"] = new SelectList(listaAreasConocimientos, "IdAreaConocimiento", "Descripcion");

            ViewData["IdTitulo"] = new SelectList(listaTitulos, "IdTitulo", "Nombre");
        }

        public async Task<IActionResult> EditPersonaEstudio(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/PersonasEstudios");


                    var personaestudio = JsonConvert.DeserializeObject<PersonaEstudio>(respuesta.Resultado.ToString());
                    if (respuesta.IsSuccess)
                    {
                        var respuestaTitulo = await apiServicio.SeleccionarAsync<Response>(personaestudio.IdTitulo.ToString(), new Uri(WebApp.BaseAddress),
                                                                  "api/Titulos");

                        var titulo = JsonConvert.DeserializeObject<Titulo>(respuestaTitulo.Resultado.ToString());
                        await CargarListaComboEdit(titulo);
                        var viewmodelPersonaEstudio = new ViewModelPersonaEstudio()
                        {
                            IdPersonaEstudio = personaestudio.IdPersonaEstudio,
                            IdEstudio = titulo.IdEstudio,
                            IdAreaConocimiento = titulo.IdAreaConocimiento,
                            IdTitulo = titulo.IdTitulo,
                            FechaGraduado = personaestudio.FechaGraduado,
                            Observaciones = personaestudio.Observaciones,
                            NoSenescyt = personaestudio.NoSenescyt,
                            IdPersona = personaestudio.IdPersona

                        };
                        return View(viewmodelPersonaEstudio);
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
        public async Task<IActionResult> EditPersonaEstudio(string id, ViewModelPersonaEstudio viewModelPersonaEstudio)
        {
            Response response = new Response();
            try
            {
                var candidato = ObtenerCandidato();
                var personaEstudio = new PersonaEstudio()
                {
                    IdPersonaEstudio = viewModelPersonaEstudio.IdPersonaEstudio,
                    FechaGraduado = viewModelPersonaEstudio.FechaGraduado,
                    Observaciones = viewModelPersonaEstudio.Observaciones,
                    IdTitulo = viewModelPersonaEstudio.IdTitulo,
                    NoSenescyt = viewModelPersonaEstudio.NoSenescyt
                };

                personaEstudio.IdPersona = candidato.IdPersona;

                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, personaEstudio, new Uri(WebApp.BaseAddress),
                                                                 "api/PersonasEstudios");

                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexPersonaEstudio");
                    }
                    ViewData["Error"] = response.Message;
                    return View(viewModelPersonaEstudio);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeletePersonaEstudio(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/PersonasEstudios");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexPersonaEstudio");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexPersonaEstudio()
        {
            var candidato = ObtenerCandidato();
            var lista = new List<PersonaEstudio>();
            try
            {
                lista = await apiServicio.ObtenerElementoAsync1<List<PersonaEstudio>>(candidato, new Uri(WebApp.BaseAddress)
                                                                    , "api/Candidatos/ListarEstudiosporCandidato");
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }


        public FichaCandidatoViewModel ObtenerCandidato()
        {
            var candidato = new FichaCandidatoViewModel
            {
                IdCandidato = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idCandidatoSession)),
                IdPersona= Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idCandidatoPersonaSession)),
            };
            return candidato;
        }
    }
}