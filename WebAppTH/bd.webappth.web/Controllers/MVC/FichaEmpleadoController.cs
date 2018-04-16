using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bd.log.guardar.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class FichaEmpleadoController : Controller
    {
        private readonly IApiServicio apiServicio;


        public FichaEmpleadoController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> CreatePersonaEstudio()
        {
            ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
            return View();
        }

        private async Task CargarListaComboEdit(Titulo titulo)
        {

            var listaAreasConocimientos = await apiServicio.Listar<AreaConocimiento>(titulo, new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientosporEstudio");

            var listaTitulos = await apiServicio.Listar<Titulo>(titulo, new Uri(WebApp.BaseAddress), "api/Titulos/ListarTitulosporAreaConocimiento");


            ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

            ViewData["IdAreaConocimiento"] = new SelectList(listaAreasConocimientos, "IdAreaConocimiento", "Descripcion");

            ViewData["IdTitulo"] = new SelectList(listaTitulos, "IdTitulo", "Nombre");
        }

        public async Task<Empleado> ObtenerEmpleadoLogueado(string nombreUsuario)
        {
            try
            {
                var empleado = new Empleado
                {
                    NombreUsuario = nombreUsuario,
                };
                var usuariologueado = await apiServicio.ObtenerElementoAsync1<Empleado>(empleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerEmpleadoLogueado");
                return usuariologueado;
            }
            catch (Exception)
            {
                return new Empleado();
            }

        }


      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePersonaEstudio(ViewModelPersonaEstudio viewModelPersonaEstudio)
        {
            var empleado = await ObtenerEmpleado();
            var personaEstudio = new PersonaEstudio()
            {
                FechaGraduado = viewModelPersonaEstudio.FechaGraduado,
                Observaciones = viewModelPersonaEstudio.Observaciones,
                IdTitulo = viewModelPersonaEstudio.IdTitulo,
                NoSenescyt = viewModelPersonaEstudio.NoSenescyt
            };

            personaEstudio.IdPersona = empleado.IdPersona;

            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(personaEstudio,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/PersonasEstudios/InsertarPersonaEstudio");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una persona estudio",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Persona Estudio:", personaEstudio.IdPersonaEstudio),
                    });

                    return RedirectToAction("IndexPersonaEstudio");
                }

                ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                ViewData["Error"] = response.Message;
                return View(viewModelPersonaEstudio);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando una Persona Estudio",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

                return BadRequest();
            }
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
                var empleado = await ObtenerEmpleado();
                var personaEstudio = new PersonaEstudio()
                {
                    IdPersonaEstudio = viewModelPersonaEstudio.IdPersonaEstudio,
                    FechaGraduado = viewModelPersonaEstudio.FechaGraduado,
                    Observaciones = viewModelPersonaEstudio.Observaciones,
                    IdTitulo = viewModelPersonaEstudio.IdTitulo,
                    NoSenescyt = viewModelPersonaEstudio.NoSenescyt
                };

                personaEstudio.IdPersona = empleado.IdPersona;

                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, personaEstudio, new Uri(WebApp.BaseAddress),
                                                                 "api/PersonasEstudios");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una persona estudio",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexPersonaEstudio");
                    }
                    ViewData["Error"] = response.Message;
                    return View(viewModelPersonaEstudio);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una persona estudio",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexPersonaEstudio()
        {
            var empleado = await ObtenerEmpleado();
            var lista = new List<PersonaEstudio>();
            try
            {
                lista = await apiServicio.ObtenerElementoAsync1<List<PersonaEstudio>>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/PersonasEstudios/ListarEstudiosporEmpleado");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando personas estudios",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
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
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de persona estudio eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("IndexPersonaEstudio");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Persona Estudio",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
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

                var empleado = await ObtenerEmpleado();

                var trayectoriaLaboral = new TrayectoriaLaboral
                {
                    FechaInicio = viewModelTrayectoriaLaboral.FechaInicio,
                    FechaFin = viewModelTrayectoriaLaboral.FechaFin,
                    Empresa = viewModelTrayectoriaLaboral.Empresa,
                    PuestoTrabajo = viewModelTrayectoriaLaboral.PuestoTrabajo,
                    DescripcionFunciones = viewModelTrayectoriaLaboral.DescripcionFunciones,
                    AreaAsignada=viewModelTrayectoriaLaboral.AreaAsignada,
                    FormaIngreso=viewModelTrayectoriaLaboral.FormaIngreso,
                    MotivoSalida=viewModelTrayectoriaLaboral.MotivoSalida,
                    TipoInstitucion=viewModelTrayectoriaLaboral.TipoInstitucion,
                };

                trayectoriaLaboral.IdPersona = empleado.IdPersona;

                response = await apiServicio.InsertarAsync(trayectoriaLaboral,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TrayectoriasLaborales/InsertarTrayectoriaLaboral");
                if (response.IsSuccess)
                {

                    var responseLog = await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        ExceptionTrace = null,
                        Message = "Se ha creado una trayectoria laboral",
                        UserName = "Usuario 1",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        EntityID = string.Format("{0} {1}", "Trayectoria Laboral:", trayectoriaLaboral.IdTrayectoriaLaboral),
                    });

                    return RedirectToAction("IndexTrayectoriaLaboral");
                }

                ViewData["Error"] = response.Message;
                return View(viewModelTrayectoriaLaboral);

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Creando Trayectoria Laboral",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP WebAppTh"
                });

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

                var empleado = await ObtenerEmpleado();
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

                trayectoriaLaboral.IdPersona = empleado.IdPersona;

                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, trayectoriaLaboral, new Uri(WebApp.BaseAddress),
                                                                 "api/TrayectoriasLaborales");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una trayectoria laboral",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexTrayectoriaLaboral");
                    }
                    ViewData["Error"] = response.Message;
                    return View(viewModelTrayectoriaLaboral);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una trayectoria laboral",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexTrayectoriaLaboral()
        {

            var empleado = await ObtenerEmpleado();
            var lista = new List<TrayectoriaLaboral>();
            try
            {

                lista = await apiServicio.ObtenerElementoAsync1<List<TrayectoriaLaboral>>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/TrayectoriasLaborales/ListarTrayectoriasLaboralesporEmpleado");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando trayectorias laborales",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
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
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de trayectoria laboral eliminada",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("IndexTrayectoriaLaboral");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Trayectoria Laboral",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        private async Task CargarCombos()
        {
            //Tabla Persona
            ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "api/Sexos/ListarSexos"), "IdSexo", "Nombre");
            ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
            ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
            ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "api/Generos/ListarGeneros"), "IdGenero", "Nombre");
            ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
            ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
            ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");
            ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");


            //Cargar Paises
            var listaPaises = await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais");
            ViewData["IdPaisLugarNacimiento"] = new SelectList(listaPaises, "IdPais", "Nombre", listaPaises.FirstOrDefault());
            ViewData["IdPaisLugarSufragio"] = new SelectList(listaPaises, "IdPais", "Nombre", listaPaises.FirstOrDefault());
            ViewData["IdPaisDireccion"] = new SelectList(listaPaises, "IdPais", "Nombre", listaPaises.FirstOrDefault());

        }

        private async Task CargarCombosPersonaSustituto()
        {
            ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
        }

        private async Task CargarCombosEdit()
        {
            //Tabla Persona
            ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "api/Sexos/ListarSexos"), "IdSexo", "Nombre");
            ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
            ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
            ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "api/Generos/ListarGeneros"), "IdGenero", "Nombre");
            ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
            ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
            ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");
            ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");


            ViewData["IdPaisLugarPersona"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdProvinciaLugarPersona"] = new SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvincia"), "IdProvincia", "Nombre");
            ViewData["IdCiudadLugarPersona"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudad"), "IdCiudad", "Nombre");
            ViewData["IdParroquia"] = new SelectList(await apiServicio.Listar<Parroquia>(new Uri(WebApp.BaseAddress), "api/Parroquia/ListarParroquia"), "IdParroquia", "Nombre");


        }


        private async Task CargarCombos(EmpleadoFamiliarViewModel empleadoFamiliarViewModel)
        {
            //Tabla Persona
            ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "api/Sexos/ListarSexos"), "IdSexo", "Nombre");
            ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
            ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
            ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "api/Generos/ListarGeneros"), "IdGenero", "Nombre");
            ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");

        }


        private async Task CargarCombos(EmpleadoSustitutoViewModel empleadoSustitutoViewModel)
        {
            //Tabla Persona
            ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "api/Sexos/ListarSexos"), "IdSexo", "Nombre");
            ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
            ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
            ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "api/Generos/ListarGeneros"), "IdGenero", "Nombre");
            ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
            ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
            ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");

            //Nacionalidades indígenas
            var Etnia = new Etnia { IdEtnia = empleadoSustitutoViewModel.IdEtnia };
            var listaNacionalidadIndigena = await apiServicio.Listar<NacionalidadIndigena>(Etnia, new Uri(WebApp.BaseAddress), "api/NacionalidadesIndigenas/ListarNacionalidadesIndigenasPorEtnias");
            ViewData["IdNacionalidadIndigena"] = new SelectList(listaNacionalidadIndigena, "IdNacionalidadIndigena", "Nombre");

            //Cargar Paises
            var listaPaises = await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais");
            ViewData["IdPaisLugarPersona"] = new SelectList(listaPaises, "IdPais", "Nombre", empleadoSustitutoViewModel.IdPaisLugarPersona);

            //Cargar Provincias por país


            var paisLugarPersona = new Pais { IdPais = empleadoSustitutoViewModel.IdPaisLugarPersona };
            var listaProvinciaLugarPersona = await apiServicio.Listar<Provincia>(paisLugarPersona, new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvinciaPorPais");
            ViewData["IdProvinciaLugarPersona"] = new SelectList(listaProvinciaLugarPersona, "IdProvincia", "Nombre");


            //Cargar ciudades por provincia
            var provinciaLugarPersona = new Provincia { IdProvincia = empleadoSustitutoViewModel.IdProvinciaLugarPersona };
            var listaCiudadesLugarPersona = await apiServicio.Listar<Ciudad>(provinciaLugarPersona, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorProvincia");
            ViewData["IdCiudadLugarPersona"] = new SelectList(listaCiudadesLugarPersona, "IdCiudad", "Nombre");

            //Cargar Parroquia por ciudad

            var ciudadLugarPersona = new Ciudad { IdCiudad = empleadoSustitutoViewModel.IdCiudadLugarPersona };
            var listaParroquias = await apiServicio.Listar<Parroquia>(ciudadLugarPersona, new Uri(WebApp.BaseAddress), "api/Parroquia/ListarParroquiaPorCiudad");
            ViewData["IdParroquia"] = new SelectList(listaParroquias, "IdParroquia", "Nombre");

        }

        public async Task<IActionResult> CreateEmpleadoFamiliar()
        {
            await CargarCombos();
            return View();
        } 


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmpleadoFamiliar(EmpleadoFamiliarViewModel empleadoFamiliarViewModel)
        {

            var empleado = await ObtenerEmpleado();
            if (!ModelState.IsValid)
            {
                await CargarCombos(empleadoFamiliarViewModel);
                return View(empleadoFamiliarViewModel);
            }
            empleadoFamiliarViewModel.IdEmpleado = empleado.IdEmpleado;

            try
            {
                var response = await apiServicio.InsertarAsync(empleadoFamiliarViewModel, new Uri(WebApp.BaseAddress), "api/EmpleadoFamiliares/InsertarEmpleadoFamiliar");

                if (response.IsSuccess)
                {
                    var empleadoRespuesta = JsonConvert.DeserializeObject<EmpleadoFamiliarViewModel>(response.Resultado.ToString());
                    return RedirectToAction("IndexEmpleadoFamiliar");
                }
                await CargarCombos(empleadoFamiliarViewModel);
                return View(empleadoFamiliarViewModel);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> EditEmpleadoFamiliar(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                  

                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/EmpleadoFamiliares");

                    var empleadoFamiliar = JsonConvert.DeserializeObject<EmpleadoFamiliar>(respuesta.Resultado.ToString());

                    if (respuesta.IsSuccess)
                    {
                        var viewmodelEmpleadoFamiliar = new EmpleadoFamiliarViewModel()
                        {
                            FechaNacimiento = empleadoFamiliar.Persona.FechaNacimiento.Value,
                            IdSexo = Convert.ToInt32(empleadoFamiliar.Persona.IdSexo),
                            IdTipoIdentificacion = Convert.ToInt32(empleadoFamiliar.Persona.IdTipoIdentificacion),
                            IdEstadoCivil = Convert.ToInt32(empleadoFamiliar.Persona.IdEstadoCivil),
                            IdGenero = Convert.ToInt32(empleadoFamiliar.Persona.IdGenero),
                            Identificacion = empleadoFamiliar.Persona.Identificacion,
                            Nombres = empleadoFamiliar.Persona.Nombres,
                            Apellidos = empleadoFamiliar.Persona.Apellidos,
                            TelefonoPrivado = empleadoFamiliar.Persona.TelefonoPrivado,
                            LugarTrabajo = empleadoFamiliar.Persona.LugarTrabajo,
                            Ocupacion = empleadoFamiliar.Persona.Ocupacion,
                            IdParentesco = empleadoFamiliar.Parentesco.IdParentesco,
                            IdEmpleado = empleadoFamiliar.IdEmpleado,
                            IdEmpleadoFamiliar = empleadoFamiliar.IdEmpleadoFamiliar,
                            IdPersona = empleadoFamiliar.IdPersona,
                            TelefonoCasa=empleadoFamiliar.Persona.TelefonoCasa,

                        };
                        await CargarCombosEdit();
                        return View(viewmodelEmpleadoFamiliar);
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
        public async Task<IActionResult> EditEmpleadoFamiliar(string id, EmpleadoFamiliarViewModel empleadoFamiliarViewModel)
        {
            Response response = new Response();
            try
            {
               

                if (!string.IsNullOrEmpty(id))
                {

                    if (!ModelState.IsValid)
                    {
                        await CargarCombos();
                        return View(empleadoFamiliarViewModel);
                    }

                    response = await apiServicio.EditarAsync(id, empleadoFamiliarViewModel, new Uri(WebApp.BaseAddress),
                                                                 "api/EmpleadoFamiliares");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un familiar de empleado",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexEmpleadoFamiliar");
                    }
                    ViewData["Error"] = response.Message;
                    await CargarCombos(empleadoFamiliarViewModel);
                    return View(empleadoFamiliarViewModel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una trayectoria laboral",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexEmpleadoFamiliar()
        {

            var empleado = await ObtenerEmpleado();
            var lista = new List<EmpleadoFamiliar>();
            try
            {

                lista = await apiServicio.ObtenerElementoAsync1<List<EmpleadoFamiliar>>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/EmpleadoFamiliares/ListarEmpleadosFamiliaresPorId");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando empleados familiares",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeleteEmpleadoFamiliar(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/EmpleadoFamiliares");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de empleado familiar eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("IndexEmpleadoFamiliar");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Empleado Familiar",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> CreatePersonaDiscapacidad()
        {
            ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePersonaDiscapacidad(PersonaDiscapacidad personaDiscapacidad)
        {

            var empleado = await ObtenerEmpleado();
            personaDiscapacidad.IdPersona = empleado.IdPersona;
            if (!ModelState.IsValid)
            {
                ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                return View(personaDiscapacidad);
            }
            

            try
            {
                var response = await apiServicio.InsertarAsync(personaDiscapacidad, new Uri(WebApp.BaseAddress), "api/PersonasDiscapacidades/InsertarPersonaDiscapacidad");

                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexPersonaDiscapacidad");
                }
                ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                return View(personaDiscapacidad);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> EditPersonaDiscapacidad(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {


                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/PersonasDiscapacidades");

                    var personaDiscapacidad = JsonConvert.DeserializeObject<PersonaDiscapacidad>(respuesta.Resultado.ToString());


                    if (respuesta.IsSuccess)
                    {
                        ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                        return View(personaDiscapacidad);
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
        public async Task<IActionResult> EditPersonaDiscapacidad(string id, PersonaDiscapacidad personaDiscapacidad)
        {
            Response response = new Response();
            try
            {


                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, personaDiscapacidad, new Uri(WebApp.BaseAddress),
                                                                 "api/PersonasDiscapacidades");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una discapacidad de empleado",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexPersonaDiscapacidad");
                    }
                    ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                    return View(personaDiscapacidad);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una trayectoria laboral",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexPersonaDiscapacidad()
        {

            var empleado = await ObtenerEmpleado();
            var lista = new List<PersonaDiscapacidad>();
            try
            {

                lista = await apiServicio.ObtenerElementoAsync1<List<PersonaDiscapacidad>>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/PersonasDiscapacidades/ListarDiscapacidadesEmpleadoPorId");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando discapacidades de empleado",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeletePersonaDiscapacidad(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/PersonasDiscapacidades");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de discapacidad de empleado eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("IndexPersonaDiscapacidad");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Discapacidad de Empleado",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }




        public async Task<IActionResult> CreatePersonaEnfermedad()
        {
            ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
            return View();
        }




        public async Task<IActionResult> CreateDiscapacidadSustituto(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("IndexDiscapacidadSustituto");
            }
            ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
            var discapacidadSustituto = new DiscapacidadSustitutoRequest { IdPersonaSustituto = Convert.ToInt32(id) };
            return View(discapacidadSustituto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiscapacidadSustituto(DiscapacidadSustitutoRequest discapacidadSustitutoRequest)
        {

            if (!ModelState.IsValid)
            {
                ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                return View(discapacidadSustitutoRequest);
            }
            try
            {
                var response = await apiServicio.InsertarAsync(discapacidadSustitutoRequest, new Uri(WebApp.BaseAddress), "api/PersonaSustituto/InsertarDiscapacidadSustituto");

                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexDiscapacidadSustituto", new { id = discapacidadSustitutoRequest.IdPersonaSustituto });
                }
                ViewData["Error"] = response.Message;
                ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                return View(discapacidadSustitutoRequest);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<IActionResult> CreateEnfermedadSustituto(int? id)
        {
            if (id==null)
            {
                return RedirectToAction("IndexEnfermedadSustituto");
            }
            ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
            var enfermedadSustituto = new EnfermedadSustitutoRequest { IdPersonaSustituto =Convert.ToInt32(id)};
            return View(enfermedadSustituto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEnfermedadSustituto(EnfermedadSustitutoRequest enfermedadSustitutoRequest)
        {

            if (!ModelState.IsValid)
            {
                ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
                return View(enfermedadSustitutoRequest);
            }
            try
            {
                var response = await apiServicio.InsertarAsync(enfermedadSustitutoRequest, new Uri(WebApp.BaseAddress), "api/PersonaSustituto/InsertarEnfermedadeSustituto");

                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexEnfermedadSustituto",new {id=enfermedadSustitutoRequest.IdPersonaSustituto });
                }
                ViewData["Error"] = response.Message;
                ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
                return View(enfermedadSustitutoRequest);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePersonaEnfermedad(PersonaEnfermedad personaEnfermedad)
        {

            var empleado = await ObtenerEmpleado();
            if (!ModelState.IsValid)
            {
                ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
                return View(personaEnfermedad);
            }
            personaEnfermedad.IdPersona = empleado.IdPersona;

            try
            {
                var response = await apiServicio.InsertarAsync(personaEnfermedad, new Uri(WebApp.BaseAddress), "api/PersonaEnfermedades/InsertarPersonaEnfermedad");

                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexPersonaEnfermedad");
                }
                ViewData["Error"] = response.Message;
                ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
                return View(personaEnfermedad);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> EditPersonaEnfermedad(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {


                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/PersonaEnfermedades");

                    var personaEnfermedad = JsonConvert.DeserializeObject<PersonaEnfermedad>(respuesta.Resultado.ToString());


                    if (respuesta.IsSuccess)
                    {
                        ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
                        return View(personaEnfermedad);
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
        public async Task<IActionResult> EditPersonaEnfermedad(string id, PersonaEnfermedad personaEnfermedad)
        {
            Response response = new Response();
            try
            {


                if (!string.IsNullOrEmpty(id))
                {
                    if (!ModelState.IsValid)
                    {
                        ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
                        return View(personaEnfermedad);
                    }

                    response = await apiServicio.EditarAsync(id, personaEnfermedad, new Uri(WebApp.BaseAddress),
                                                                 "api/PersonaEnfermedades");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una enfermedad del empleado",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexPersonaEnfermedad");
                    }
                    ViewData["Error"] = response.Message;
                    ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
                    return View(personaEnfermedad);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una enfermedad de la persona",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexPersonaEnfermedad()
        {

            var empleado = await ObtenerEmpleado();
            var lista = new List<PersonaEnfermedad>();
            try
            {

                lista = await apiServicio.ObtenerElementoAsync1<List<PersonaEnfermedad>>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/PersonaEnfermedades/ListarEnfermedadesEmpleadoPorId");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando enfermedades de las personas",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeletePersonaEnfermedad(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/PersonaEnfermedades");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de enfermedad de empleado eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("IndexPersonaEnfermedad");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Enfermedad de Empleado",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> CreateDatoBancario()
        {
            ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDatoBancario(DatosBancarios datosBancarios)
        {

            var empleado = await ObtenerEmpleado();
            if (!ModelState.IsValid)
            {
                ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");
                return View(datosBancarios);
            }
            datosBancarios.IdEmpleado = empleado.IdEmpleado;

            try
            {
                var response = await apiServicio.InsertarAsync(datosBancarios, new Uri(WebApp.BaseAddress), "api/DatosBancarios/InsertarDatosBancarios");

                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexDatoBancario");
                }
                ViewData["Error"] = response.Message;
                ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");
                return View(datosBancarios);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> EditDatoBancario(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {


                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/DatosBancarios");

                    var datobancario = JsonConvert.DeserializeObject<DatosBancarios>(respuesta.Resultado.ToString());


                    if (respuesta.IsSuccess)
                    {
                        ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");
                        return View(datobancario);
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
        public async Task<IActionResult> EditDatoBancario(string id, DatosBancarios datosBancarios)
        {
            Response response = new Response();
            try
            {


                if (!string.IsNullOrEmpty(id))
                {
                    if (!ModelState.IsValid)
                    {
                      ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");
                        return View(datosBancarios);  
                    }

                    response = await apiServicio.EditarAsync(id, datosBancarios, new Uri(WebApp.BaseAddress),
                                                                 "api/DatosBancarios");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un dato bancario del empleado",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexDatoBancario");
                    }
                    ViewData["Error"] = response.Message;
                    ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");
                    return View(datosBancarios);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando un dato bancario del empleado",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexDatoBancario()
        {

            var empleado = await ObtenerEmpleado();
            var datobancario = new DatosBancarios();
            Response respuesta = new Response();
            try
            {

                //lista = await apiServicio.ObtenerElementoAsync1<List<DatosBancarios>>(empleado, new Uri(WebApp.BaseAddress)
                //                                                    , "api/DatosBancarios/DatosBancariosPorIdEmpleado");
                respuesta = await apiServicio.ObtenerElementoAsync1<Response>(empleado, new Uri(WebApp.BaseAddress),
                                                                 "api/DatosBancarios/DatosBancariosPorIdEmpleado");

                if (respuesta.Resultado != null)
                {
                    datobancario = JsonConvert.DeserializeObject<DatosBancarios>(respuesta.Resultado.ToString());
                    return View(datobancario);
                }
                else
                {
                    DatosBancarios datoBancarioVacio = new DatosBancarios();
                    return View(datoBancarioVacio);
                }
              
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando datos bancarios de empleado",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeleteDatoBancario(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/DatosBancarios");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de dato bancario del empleado eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("IndexDatoBancario");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Dato Bancario de Empleado",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> CreateEmpleadoContactoEmergencia()
        {
            ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmpleadoContactoEmergencia(ContactoEmergenciaViewModel contactoEmergenciaViewModel)
        {

            
            if (!ModelState.IsValid)
            {
                ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                return View(contactoEmergenciaViewModel);
            }
            var empleado = await ObtenerEmpleado();
            contactoEmergenciaViewModel.IdEmpleado = empleado.IdEmpleado;

            try
            {
                var response = await apiServicio.InsertarAsync(contactoEmergenciaViewModel, new Uri(WebApp.BaseAddress), "api/EmpleadosContactosEmergencias/InsertarEmpleadoContactoEmergencia");

                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexEmpleadoContactoEmergencia");
                }
                ViewData["Error"] = response.Message;
                ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                return View(contactoEmergenciaViewModel);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> EditEmpleadoContactoEmergencia(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {

                    var contactoEmergencia = new ContactoEmergenciaViewModel { IdEmpleadoContactoEmergencia = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(contactoEmergencia, new Uri(WebApp.BaseAddress),
                                                                  "api/EmpleadosContactosEmergencias/ObtenerContactoEmergenciaPorEmpleado");



                    if (respuesta.IsSuccess)
                    {

                        var empleadoContactoEmergencia = JsonConvert.DeserializeObject<ContactoEmergenciaViewModel>(respuesta.Resultado.ToString());
                       
                        ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                        return View(empleadoContactoEmergencia);
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
        public async Task<IActionResult> EditEmpleadoContactoEmergencia(ContactoEmergenciaViewModel contactoEmergenciaViewModel)
        {
            Response response = new Response();
            try
            {


                if (!string.IsNullOrEmpty(Convert.ToString(contactoEmergenciaViewModel.IdEmpleadoContactoEmergencia)))
                {
                    if (!ModelState.IsValid)
                    {
                        ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                        return View(contactoEmergenciaViewModel);
                    }

                    response = await apiServicio.EditarAsync<Response>(contactoEmergenciaViewModel, new Uri(WebApp.BaseAddress),
                                                                 "api/EmpleadosContactosEmergencias/EditarContactoEmergenciaEmpleado");

                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexEmpleadoContactoEmergencia");
                    }
                    ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                    ViewData["Error"] = response.Message;
                    return View(contactoEmergenciaViewModel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una trayectoria laboral",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexEmpleadoContactoEmergencia()
        {

            var empleado = await ObtenerEmpleado();
            var empleadoContactoEmergencia = new EmpleadoContactoEmergencia();
            try
            {

              var  Lista = await apiServicio.Listar<ContactoEmergenciaViewModel>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/EmpleadosContactosEmergencias/EmpleadosContactosEmergenciasPorIdEmpleado");
                if (Lista!=null)
                {
                   
                    return View(Lista);
                }
                else
                {
                    var lista = new List<ContactoEmergenciaViewModel>();
                    return View(lista);
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task CargarCombosEmpleado(DatosBasicosEmpleadoViewModel datosBasicosEmpleado)
        {
            //Tabla Persona
            ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "api/Sexos/ListarSexos"), "IdSexo", "Nombre");
            ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
            ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
            ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "api/Generos/ListarGeneros"), "IdGenero", "Nombre");
            ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
            ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
            ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");

            //Nacionalidades indígenas
            var Etnia = new Etnia { IdEtnia = datosBasicosEmpleado.IdEtnia };
            var listaNacionalidadIndigena = await apiServicio.Listar<NacionalidadIndigena>(Etnia, new Uri(WebApp.BaseAddress), "api/NacionalidadesIndigenas/ListarNacionalidadesIndigenasPorEtnias");
            ViewData["IdNacionalidadIndigena"] = new SelectList(listaNacionalidadIndigena, "IdNacionalidadIndigena", "Nombre");

            //Cargar Paises
            var listaPaises = await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais");
            ViewData["IdPaisLugarNacimiento"] = new SelectList(listaPaises, "IdPais", "Nombre", datosBasicosEmpleado.IdPaisLugarNacimiento);
            ViewData["IdPaisLugarSufragio"] = new SelectList(listaPaises, "IdPais", "Nombre", datosBasicosEmpleado.IdPaisLugarSufragio);
            ViewData["IdPaisDireccion"] = new SelectList(listaPaises, "IdPais", "Nombre", datosBasicosEmpleado.IdPaisLugarPersona);

            //Cargar Provincias por país

            var paisLugarSufragio = new Pais { IdPais = datosBasicosEmpleado.IdPaisLugarSufragio };
            var listaProvinciasLugarSufragio = await apiServicio.Listar<Provincia>(paisLugarSufragio, new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvinciaPorPais");
            ViewData["IdProvinciaLugarSufragio"] = new SelectList(listaProvinciasLugarSufragio, "IdProvincia", "Nombre");

            var paisLugarPersona = new Pais { IdPais = datosBasicosEmpleado.IdPaisLugarPersona };
            var listaProvinciaLugarPersona = await apiServicio.Listar<Provincia>(paisLugarPersona, new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvinciaPorPais");
            ViewData["IdProvinciaLugarPersona"] = new SelectList(listaProvinciaLugarPersona, "IdProvincia", "Nombre");

            //Cargar Ciudades por Paises
            var paisCiudadLugarNacimiento = new Pais { IdPais = datosBasicosEmpleado.IdPaisLugarNacimiento };
            var listaCiudadLugarNacimiento = await apiServicio.Listar<Ciudad>(paisCiudadLugarNacimiento, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorPais");
            ViewData["IdCiudadLugarNacimiento"] = new SelectList(listaCiudadLugarNacimiento, "IdCiudad", "Nombre");


            //Cargar ciudades por provincia
            var provinciaLugarPersona = new Provincia { IdProvincia = datosBasicosEmpleado.IdProvinciaLugarPersona };
            var listaCiudadesLugarPersona = await apiServicio.Listar<Ciudad>(provinciaLugarPersona, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorProvincia");
            ViewData["IdCiudadLugarPersona"] = new SelectList(listaCiudadesLugarPersona, "IdCiudad", "Nombre");

            //Cargar Parroquia por ciudad

            var ciudadLugarPersona = new Ciudad { IdCiudad = datosBasicosEmpleado.IdCiudadLugarPersona };
            var listaParroquias = await apiServicio.Listar<Parroquia>(ciudadLugarPersona, new Uri(WebApp.BaseAddress), "api/Parroquia/ListarParroquiaPorCiudad");
            ViewData["IdParroquia"] = new SelectList(listaParroquias, "IdParroquia", "Nombre");




        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detalle(DatosBasicosEmpleadoViewModel datosBasicosEmpleado)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);
                await CargarCombosEmpleado(datosBasicosEmpleado);
                return View(datosBasicosEmpleado);
            }

            try
            {
                var response = await apiServicio.EditarAsync<Response>(datosBasicosEmpleado, new Uri(WebApp.BaseAddress), "api/Empleados/EditarEmpleado");

                if (response.IsSuccess)
                {
                    var empleado = JsonConvert.DeserializeObject<Empleado>(response.Resultado.ToString());
                    return RedirectToAction("Detalle");
                }
                await CargarCombosEmpleado(datosBasicosEmpleado);
                ViewData["Error"] = Mensaje.ExisteEmpleado;
                return View(datosBasicosEmpleado);
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<IActionResult> Detalle(string mensaje)
        {

            var empleado = await ObtenerEmpleado();

            Response response = new Response();
           
                var empleadoActual = new DatosBasicosEmpleadoViewModel { IdEmpleado = empleado.IdEmpleado };
                response = await apiServicio.ObtenerElementoAsync1<Response>(empleadoActual,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/Empleados/ObtenerDatosBasicosEmpleado");
            if (response.IsSuccess)
            {
                InicializarMensaje(mensaje);
                var empleadoRespuesta = JsonConvert.DeserializeObject<DatosBasicosEmpleadoViewModel>(response.Resultado.ToString());
                await CargarCombosEmpleado(empleadoRespuesta);
                return View(empleadoRespuesta);
            }
            return BadRequest();
        }

        public async Task<IActionResult> DeleteEmpleadoContactoEmergencia(string id)
        {

            try
            {

                if (!string.IsNullOrEmpty(id))
                {

                    var contactoEmergencia = new ContactoEmergenciaViewModel { IdEmpleadoContactoEmergencia = Convert.ToInt32(id) };
                    var response = await apiServicio.EliminarAsync(contactoEmergencia, new Uri(WebApp.BaseAddress)
                                                               , "api/EmpleadosContactosEmergencias/EliminarContactoEmergencia");
                    if (response.IsSuccess)
                    {   
                        return RedirectToAction("IndexEmpleadoContactoEmergencia");
                    }
                }
                return BadRequest();
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

        public async Task<IActionResult> CreatePersonaSustituto()
        {
            await CargarCombos();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePersonaSustituto(ViewModelEmpleadoSustituto viewModelEmpleadoSustituto)
        {

            var empleado = await ObtenerEmpleado();
            if (!ModelState.IsValid)
            {
                await CargarCombosPersonaSustituto();
                return View(viewModelEmpleadoSustituto);
            }
            viewModelEmpleadoSustituto.IdEmpleado = empleado.IdEmpleado;

            try
            {
                var response = await apiServicio.InsertarAsync(viewModelEmpleadoSustituto, new Uri(WebApp.BaseAddress), "api/PersonaSustituto/InsertarPersonaSustituto");

                if (response.IsSuccess)
                {
                    //var empleadoRespuesta = JsonConvert.DeserializeObject<EmpleadoSustitutoViewModel>(response.Resultado.ToString());
                    return RedirectToAction("IndexPersonaSustituto");
                }
                await CargarCombosPersonaSustituto();
                return View(viewModelEmpleadoSustituto);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> EditPersonaSustituto(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {

                    var personaSustituto = new ViewModelEmpleadoSustituto { IdPersonaSustituto = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(personaSustituto, new Uri(WebApp.BaseAddress),
                                                                  "api/PersonaSustituto/ObtenerPersonasSustitutos");

                    if (respuesta.IsSuccess)
                    {
                        await CargarCombosPersonaSustituto();
                        var PersonaSustituto = JsonConvert.DeserializeObject<ViewModelEmpleadoSustituto>(respuesta.Resultado.ToString());
                        return View(PersonaSustituto);
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
        public async Task<IActionResult> EditPersonaSustituto(ViewModelEmpleadoSustituto viewModel)
        {
            Response response = new Response();
            try
            {


                if (!string.IsNullOrEmpty(Convert.ToString(viewModel.IdPersonaSustituto)))
                {
                    if (!ModelState.IsValid)
                    {
                        await CargarCombosPersonaSustituto();
                        return View(viewModel);
                    }

                    response = await apiServicio.EditarAsync<Response>(viewModel, new Uri(WebApp.BaseAddress),
                                                                 "api/PersonaSustituto/EditarPersonaSustituto");

                    if (response.IsSuccess)
                    {
                        return RedirectToAction("IndexPersonaSustituto");
                    }
                    ViewData["Error"] = response.Message;
                    await CargarCombosPersonaSustituto();
                    return View(viewModel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexEnfermedadSustituto(int? id)
        {

            try
            {
                if (id==null)
                {
                    return RedirectToAction("IndexPersonaSustituto");
                }
                var personaSustitutoActual = new EnfermedadSustitutoRequest { IdPersonaSustituto =Convert.ToInt32(id) };
                var response = await apiServicio.ObtenerElementoAsync1<Response>(personaSustitutoActual, new Uri(WebApp.BaseAddress),
                                                              "api/PersonaSustituto/ListarEnfermedadesSustitutos");

                if (response.IsSuccess)
                {
                    var enfermedadRequest = JsonConvert.DeserializeObject<EnfermedadSustitutoRequest>(response.Resultado.ToString());
                    return View(enfermedadRequest);
                }
                return RedirectToAction("IndexPersonaSustituto");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexDiscapacidadSustituto(int? id)
        {

            try
            {
                if (id == null)
                {
                    return RedirectToAction("IndexPersonaSustituto");
                }
                var personaSustitutoActual = new DiscapacidadSustitutoRequest { IdPersonaSustituto = Convert.ToInt32(id) };
                var response = await apiServicio.ObtenerElementoAsync1<Response>(personaSustitutoActual, new Uri(WebApp.BaseAddress),
                                                              "api/PersonaSustituto/ListarDiscapacidadSustitutos");

                if (response.IsSuccess)
                {
                    var discapacidadRequest = JsonConvert.DeserializeObject<DiscapacidadSustitutoRequest>(response.Resultado.ToString());
                    return View(discapacidadRequest);
                }
                return RedirectToAction("IndexPersonaSustituto");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexPersonaSustituto()
        {

            var empleado = await ObtenerEmpleado();
            try
            {
                var empleadoActual = new ViewModelEmpleadoSustituto { IdEmpleado = empleado.IdEmpleado };
                var listaRespuesta = await apiServicio.Listar<ViewModelEmpleadoSustituto>(empleadoActual, new Uri(WebApp.BaseAddress),
                                                              "api/PersonaSustituto/ListarPersonasSustitutos");

                return View(listaRespuesta);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> DeleteDiscapacidadSustituto(string id, string idDiscapacidad)
        {

            try
            {

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(idDiscapacidad))
                {
                    return RedirectToAction("IndexPersonaSustituto");
                }

                var enfermedadSustitutoEliminar = new DiscapacidadSustitutoRequest { IdDiscapacidadSustituto = Convert.ToInt32(idDiscapacidad) };
                var response = await apiServicio.EliminarAsync(enfermedadSustitutoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/PersonaSustituto/EliminarDiscapacidadSustituto");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexDiscapacidadSustituto", new { id });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Persona Sustituto",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }


        public async Task<IActionResult> DeleteEnfermedadSustituto(string id,string idEnfermedad)
        {

            try
            {

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(idEnfermedad))
                {
                    return RedirectToAction("IndexPersonaSustituto");
                }

                var enfermedadSustitutoEliminar = new EnfermedadSustitutoRequest { IdEnfermedadSustituto = Convert.ToInt32(idEnfermedad) };
                var response = await apiServicio.EliminarAsync(enfermedadSustitutoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/PersonaSustituto/EliminarEnfermedadeSustituto");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexEnfermedadSustituto",new {id });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Persona Sustituto",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> DeletePersonaSustituto(string id)
        {

            try
            {

                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("IndexPersonaSustituto");
                }

                var personaSustitutoEliminar = new ViewModelEmpleadoSustituto { IdPersonaSustituto = Convert.ToInt32(id) };
                var response = await apiServicio.EliminarAsync(personaSustitutoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/PersonaSustituto/EliminarPersonasSustitutos");
                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexPersonaSustituto");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Persona Sustituto",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> CreatePersonaSustitutoDiscapacidad()
        {
            ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePersonaSustitutoDiscapacidad(PersonaDiscapacidad personaDiscapacidad)
        {

            var empleado = await ObtenerEmpleado();
            if (!ModelState.IsValid)
            {
                ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                return View(personaDiscapacidad);
            }
            personaDiscapacidad.IdPersona = empleado.IdPersona;

            try
            {
                var response = await apiServicio.InsertarAsync(personaDiscapacidad, new Uri(WebApp.BaseAddress), "api/PersonasDiscapacidades/InsertarPersonaDiscapacidad");

                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexPersonaDiscapacidad");
                }
                ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                return View(personaDiscapacidad);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<IActionResult> EditPersonaSustitutoDiscapacidad(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {


                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/PersonasDiscapacidades");

                    var personaDiscapacidad = JsonConvert.DeserializeObject<PersonaDiscapacidad>(respuesta.Resultado.ToString());


                    if (respuesta.IsSuccess)
                    {
                        ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                        return View(personaDiscapacidad);
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
        public async Task<IActionResult> EditPersonaSustitutoDiscapacidad(string id, PersonaDiscapacidad personaDiscapacidad)
        {
            Response response = new Response();
            try
            {


                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, personaDiscapacidad, new Uri(WebApp.BaseAddress),
                                                                 "api/PersonasDiscapacidades");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una discapacidad de empleado",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexPersonaDiscapacidad");
                    }
                    ViewData["Error"] = response.Message;
                    ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                    return View(personaDiscapacidad);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una trayectoria laboral",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexPersonaSustitutoDiscapacidad()
        {

            var empleado = await ObtenerEmpleado();
            var lista = new List<PersonaDiscapacidad>();
            try
            {

                lista = await apiServicio.ObtenerElementoAsync1<List<PersonaDiscapacidad>>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/PersonasDiscapacidades/ListarDiscapacidadesEmpleadoPorId");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando discapacidades de empleado",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeletePersonaSustitutoDiscapacidad(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/PersonasDiscapacidades");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de discapacidad de empleado eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("IndexPersonaDiscapacidad");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Discapacidad de Empleado",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        //public async Task<IActionResult> CreatePersonaEnfermedad()
        //{
        //    ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
        //    return View();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreatePersonaEnfermedad(PersonaEnfermedad personaEnfermedad)
        //{

        //    var empleado = await ObtenerEmpleado();
        //    if (!ModelState.IsValid)
        //    {
        //        ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
        //        return View(personaEnfermedad);
        //    }
        //    personaEnfermedad.IdPersona = empleado.IdPersona;

        //    try
        //    {
        //        var response = await apiServicio.InsertarAsync(personaEnfermedad, new Uri(WebApp.BaseAddress), "api/PersonaEnfermedades/InsertarPersonaEnfermedad");

        //        if (response.IsSuccess)
        //        {
        //            return RedirectToAction("IndexPersonaEnfermedad");
        //        }
        //        ViewData["Error"] = response.Message;
        //        ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
        //        return View(personaEnfermedad);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        //public async Task<IActionResult> EditPersonaEnfermedad(string id)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(id))
        //        {


        //            var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
        //                                                          "api/PersonaEnfermedades");

        //            var personaEnfermedad = JsonConvert.DeserializeObject<PersonaEnfermedad>(respuesta.Resultado.ToString());


        //            if (respuesta.IsSuccess)
        //            {
        //                ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
        //                return View(personaEnfermedad);
        //            }

        //        }

        //        return BadRequest();
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditPersonaEnfermedad(string id, PersonaEnfermedad personaEnfermedad)
        //{
        //    Response response = new Response();
        //    try
        //    {


        //        if (!string.IsNullOrEmpty(id))
        //        {
        //            response = await apiServicio.EditarAsync(id, personaEnfermedad, new Uri(WebApp.BaseAddress),
        //                                                         "api/PersonaEnfermedades");

        //            if (response.IsSuccess)
        //            {
        //                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
        //                {
        //                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
        //                    EntityID = string.Format("{0} : {1}", "Sistema", id),
        //                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
        //                    LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
        //                    Message = "Se ha actualizado una enfermedad del empleado",
        //                    UserName = "Usuario 1"
        //                });

        //                return RedirectToAction("IndexPersonaEnfermedad");
        //            }
        //            ViewData["Error"] = response.Message;
        //            ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");
        //            return View(personaEnfermedad);

        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
        //        {
        //            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
        //            Message = "Editando una enfermedad de la persona",
        //            ExceptionTrace = ex.Message,
        //            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
        //            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
        //            UserName = "Usuario APP webappth"
        //        });

        //        return BadRequest();
        //    }
        //}

        //public async Task<IActionResult> IndexPersonaEnfermedad()
        //{

        //    var empleado = await ObtenerEmpleado();
        //    var lista = new List<PersonaEnfermedad>();
        //    try
        //    {

        //        lista = await apiServicio.ObtenerElementoAsync1<List<PersonaEnfermedad>>(empleado, new Uri(WebApp.BaseAddress)
        //                                                            , "api/PersonaEnfermedades/ListarEnfermedadesEmpleadoPorId");
        //        return View(lista);
        //    }
        //    catch (Exception ex)
        //    {
        //        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
        //        {
        //            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
        //            Message = "Listando enfermedades de las personas",
        //            ExceptionTrace = ex.Message,
        //            LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
        //            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
        //            UserName = "Usuario APP webappth"
        //        });
        //        return BadRequest();
        //    }
        //}

        //public async Task<IActionResult> DeletePersonaEnfermedad(string id)
        //{

        //    try
        //    {
        //        var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
        //                                                       , "api/PersonaEnfermedades");
        //        if (response.IsSuccess)
        //        {
        //            await GuardarLogService.SaveLogEntry(new LogEntryTranfer
        //            {
        //                ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
        //                EntityID = string.Format("{0} : {1}", "Sistema", id),
        //                Message = "Registro de enfermedad de empleado eliminado",
        //                LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
        //                LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
        //                UserName = "Usuario APP webappth"
        //            });
        //            return RedirectToAction("IndexPersonaEnfermedad");
        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
        //        {
        //            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
        //            Message = "Eliminar Enfermedad de Empleado",
        //            ExceptionTrace = ex.Message,
        //            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
        //            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
        //            UserName = "Usuario APP webappth"
        //        });

        //        return BadRequest();
        //    }
        //}

        public Task<Empleado> ObtenerEmpleado()
        {
            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var empleadoJson = ObtenerEmpleadoLogueado(NombreUsuario);
            return empleadoJson;
        }
    }
}