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
                var empleado = await ObtenerEmpleado();

                var trayectoriaLaboral = new TrayectoriaLaboral()
                {
                    FechaInicio = viewModelTrayectoriaLaboral.FechaInicio,
                    FechaFin = viewModelTrayectoriaLaboral.FechaFin,
                    Empresa = viewModelTrayectoriaLaboral.Empresa,
                    PuestoTrabajo = viewModelTrayectoriaLaboral.PuestoTrabajo,
                    DescripcionFunciones = viewModelTrayectoriaLaboral.DescripcionFunciones
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
                            DescripcionFunciones = trayectorialaboral.DescripcionFunciones
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
                var empleado = await ObtenerEmpleado();
                var trayectoriaLaboral = new TrayectoriaLaboral()
                {
                    IdTrayectoriaLaboral = viewModelTrayectoriaLaboral.IdTrayectoriaLaboral,
                    FechaInicio = viewModelTrayectoriaLaboral.FechaInicio,
                    FechaFin = viewModelTrayectoriaLaboral.FechaFin,
                    Empresa = viewModelTrayectoriaLaboral.Empresa,
                    PuestoTrabajo = viewModelTrayectoriaLaboral.PuestoTrabajo,
                    DescripcionFunciones = viewModelTrayectoriaLaboral.DescripcionFunciones
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
            ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
            ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
            ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");

            //Nacionalidades indígenas
            var Etnia = new Etnia { IdEtnia = empleadoFamiliarViewModel.IdEtnia };
            var listaNacionalidadIndigena = await apiServicio.Listar<NacionalidadIndigena>(Etnia, new Uri(WebApp.BaseAddress), "api/NacionalidadesIndigenas/ListarNacionalidadesIndigenasPorEtnias");
            ViewData["IdNacionalidadIndigena"] = new SelectList(listaNacionalidadIndigena, "IdNacionalidadIndigena", "Nombre");

            //Cargar Paises
            var listaPaises = await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "api/Pais/ListarPais");
            ViewData["IdPaisLugarPersona"] = new SelectList(listaPaises, "IdPais", "Nombre", empleadoFamiliarViewModel.IdPaisLugarPersona);

            //Cargar Provincias por país


            var paisLugarPersona = new Pais { IdPais = empleadoFamiliarViewModel.IdPaisLugarPersona };
            var listaProvinciaLugarPersona = await apiServicio.Listar<Provincia>(paisLugarPersona, new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvinciaPorPais");
            ViewData["IdProvinciaLugarPersona"] = new SelectList(listaProvinciaLugarPersona, "IdProvincia", "Nombre");


            //Cargar ciudades por provincia
            var provinciaLugarPersona = new Provincia { IdProvincia = empleadoFamiliarViewModel.IdProvinciaLugarPersona };
            var listaCiudadesLugarPersona = await apiServicio.Listar<Ciudad>(provinciaLugarPersona, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorProvincia");
            ViewData["IdCiudadLugarPersona"] = new SelectList(listaCiudadesLugarPersona, "IdCiudad", "Nombre");

            //Cargar Parroquia por ciudad

            var ciudadLugarPersona = new Ciudad { IdCiudad = empleadoFamiliarViewModel.IdCiudadLugarPersona };
            var listaParroquias = await apiServicio.Listar<Parroquia>(ciudadLugarPersona, new Uri(WebApp.BaseAddress), "api/Parroquia/ListarParroquiaPorCiudad");
            ViewData["IdParroquia"] = new SelectList(listaParroquias, "IdParroquia", "Nombre");

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

                    var respuestaParroquia = await apiServicio.SeleccionarAsync<Response>(empleadoFamiliar.Persona.IdParroquia.ToString(), new Uri(WebApp.BaseAddress),
                                                              "api/Parroquia");

                    var parroquia = JsonConvert.DeserializeObject<Parroquia>(respuestaParroquia.Resultado.ToString());

                    if (respuesta.IsSuccess)
                    {
                        var viewmodelEmpleadoFamiliar = new EmpleadoFamiliarViewModel()
                        {
                            FechaNacimiento = empleadoFamiliar.Persona.FechaNacimiento.Value,
                            IdSexo = Convert.ToInt32(empleadoFamiliar.Persona.IdSexo),
                            IdTipoIdentificacion = Convert.ToInt32(empleadoFamiliar.Persona.IdTipoIdentificacion),
                            IdEstadoCivil = Convert.ToInt32(empleadoFamiliar.Persona.IdEstadoCivil),
                            IdGenero = Convert.ToInt32(empleadoFamiliar.Persona.IdGenero),
                            IdNacionalidad = Convert.ToInt32(empleadoFamiliar.Persona.IdNacionalidad),
                            IdTipoSangre = Convert.ToInt32(empleadoFamiliar.Persona.IdTipoSangre),
                            IdEtnia = Convert.ToInt32(empleadoFamiliar.Persona.IdEtnia),
                            Identificacion = empleadoFamiliar.Persona.Identificacion,
                            Nombres = empleadoFamiliar.Persona.Nombres,
                            Apellidos = empleadoFamiliar.Persona.Apellidos,
                            TelefonoPrivado = empleadoFamiliar.Persona.TelefonoPrivado,
                            TelefonoCasa = empleadoFamiliar.Persona.TelefonoCasa,
                            CorreoPrivado = empleadoFamiliar.Persona.CorreoPrivado,
                            LugarTrabajo = empleadoFamiliar.Persona.LugarTrabajo,
                            IdNacionalidadIndigena = empleadoFamiliar.Persona.IdNacionalidadIndigena,
                            CallePrincipal = empleadoFamiliar.Persona.CallePrincipal,
                            CalleSecundaria = empleadoFamiliar.Persona.CalleSecundaria,
                            Referencia = empleadoFamiliar.Persona.Referencia,
                            Numero = empleadoFamiliar.Persona.Numero,
                            IdPaisLugarPersona = parroquia.Ciudad.Provincia.IdPais,
                            IdProvinciaLugarPersona = parroquia.Ciudad.Provincia.IdProvincia,
                            IdCiudadLugarPersona = parroquia.IdCiudad,
                            IdParroquia = Convert.ToInt32(empleadoFamiliar.Persona.IdParroquia),
                            Ocupacion = empleadoFamiliar.Persona.Ocupacion,
                            IdParentesco = empleadoFamiliar.Parentesco.IdParentesco,
                            IdEmpleado = empleadoFamiliar.IdEmpleado,
                            IdEmpleadoFamiliar = empleadoFamiliar.IdEmpleadoFamiliar,
                            IdPersona = empleadoFamiliar.IdPersona

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
        public async Task<IActionResult> CreateEmpleadoContactoEmergencia(EmpleadoFamiliarViewModel empleadoFamiliarViewModel)
        {

            var empleado = await ObtenerEmpleado();
            if (!ModelState.IsValid)
            {
                ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                return View(empleadoFamiliarViewModel);
            }
            empleadoFamiliarViewModel.IdEmpleado = empleado.IdEmpleado;

            try
            {
                var response = await apiServicio.InsertarAsync(empleadoFamiliarViewModel, new Uri(WebApp.BaseAddress), "api/EmpleadosContactosEmergencias/InsertarEmpleadoContactoEmergencia");

                if (response.IsSuccess)
                {
                    return RedirectToAction("IndexEmpleadoContactoEmergencia");
                }
                ViewData["Error"] = response.Message;
                ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                return View(empleadoFamiliarViewModel);
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


                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/EmpleadosContactosEmergencias");

                    var empleadoContactoEmergencia = JsonConvert.DeserializeObject<EmpleadoContactoEmergencia>(respuesta.Resultado.ToString());


                    if (respuesta.IsSuccess)
                    {
                        var viewmodelEmpleadoFamiliar = new EmpleadoFamiliarViewModel()
                        {

                            Nombres = empleadoContactoEmergencia.Persona.Nombres,
                            Apellidos = empleadoContactoEmergencia.Persona.Apellidos,
                            TelefonoPrivado = empleadoContactoEmergencia.Persona.TelefonoPrivado,
                            TelefonoCasa = empleadoContactoEmergencia.Persona.TelefonoCasa,
                            IdParentesco = empleadoContactoEmergencia.Parentesco.IdParentesco,
                            IdEmpleado = empleadoContactoEmergencia.IdEmpleado,
                            IdEmpleadoFamiliar = empleadoContactoEmergencia.IdEmpleadoContactoEmergencia,
                            IdPersona = empleadoContactoEmergencia.IdPersona

                        };
                        ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                        return View(viewmodelEmpleadoFamiliar);
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
        public async Task<IActionResult> EditEmpleadoContactoEmergencia(string id, EmpleadoFamiliarViewModel empleadoFamiliarViewModel)
        {
            Response response = new Response();
            try
            {


                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, empleadoFamiliarViewModel, new Uri(WebApp.BaseAddress),
                                                                 "api/EmpleadosContactosEmergencias");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un contacto de emergencia del empleado",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexEmpleadoContactoEmergencia");
                    }
                    ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                    ViewData["Error"] = response.Message;
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

        public async Task<IActionResult> IndexEmpleadoContactoEmergencia()
        {

            var empleado = await ObtenerEmpleado();
            var empleadoContactoEmergencia = new EmpleadoContactoEmergencia();
            Response respuesta = new Response();
            try
            {

                respuesta = await apiServicio.ObtenerElementoAsync1<Response>(empleado, new Uri(WebApp.BaseAddress)
                                                                    , "api/EmpleadosContactosEmergencias/EmpleadosContactosEmergenciasPorIdEmpleado");
                if (respuesta.Resultado != null)
                {
                    empleadoContactoEmergencia = JsonConvert.DeserializeObject<EmpleadoContactoEmergencia>(respuesta.Resultado.ToString());
                    return View(empleadoContactoEmergencia);
                }
                else
                {
                    EmpleadoContactoEmergencia nuevoempleadoContactoEmergencia = new EmpleadoContactoEmergencia();
                    return View(nuevoempleadoContactoEmergencia);
                }
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

        public async Task<IActionResult> DeleteEmpleadoContactoEmergencia(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/EmpleadosContactosEmergencias");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de contacto de emergencia de empleado eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
                    return RedirectToAction("IndexEmpleadoContactoEmergencia");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Eliminar Empleado Contacto de Emergencia",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }


        public async Task<IActionResult> CreatePersonaSustituto()
        {
            await CargarCombos();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePersonaSustituto(EmpleadoSustitutoViewModel empleadoSustitutoViewModel)
        {

            var empleado = await ObtenerEmpleado();
            if (!ModelState.IsValid)
            {
                await CargarCombos(empleadoSustitutoViewModel);
                return View(empleadoSustitutoViewModel);
            }
            empleadoSustitutoViewModel.IdEmpleado = empleado.IdEmpleado;

            try
            {
                var response = await apiServicio.InsertarAsync(empleadoSustitutoViewModel, new Uri(WebApp.BaseAddress), "api/PersonaSustituto/InsertarPersonaSustituto");

                if (response.IsSuccess)
                {
                    var empleadoRespuesta = JsonConvert.DeserializeObject<EmpleadoSustitutoViewModel>(response.Resultado.ToString());
                    return RedirectToAction("IndexPersonaSustituto");
                }
                await CargarCombos(empleadoSustitutoViewModel);
                return View(empleadoSustitutoViewModel);
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


                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/PersonaSustituto");

                    var personaSustituto = JsonConvert.DeserializeObject<PersonaSustituto>(respuesta.Resultado.ToString());

                    var respuestaParroquia = await apiServicio.SeleccionarAsync<Response>(personaSustituto.PersonaDiscapacidad.Persona.IdParroquia.ToString(), new Uri(WebApp.BaseAddress),
                                                              "api/Parroquia");

                    var parroquia = JsonConvert.DeserializeObject<Parroquia>(respuestaParroquia.Resultado.ToString());

                    if (respuesta.IsSuccess)
                    {
                        var viewmodelEmpleadoSustituo = new EmpleadoSustitutoViewModel()
                        {
                            FechaNacimiento = personaSustituto.PersonaDiscapacidad.Persona.FechaNacimiento.Value,
                            IdSexo = Convert.ToInt32(personaSustituto.PersonaDiscapacidad.Persona.IdSexo),
                            IdTipoIdentificacion = Convert.ToInt32(personaSustituto.PersonaDiscapacidad.Persona.IdTipoIdentificacion),
                            IdEstadoCivil = Convert.ToInt32(personaSustituto.PersonaDiscapacidad.Persona.IdEstadoCivil),
                            IdGenero = Convert.ToInt32(personaSustituto.PersonaDiscapacidad.Persona.IdGenero),
                            IdNacionalidad = Convert.ToInt32(personaSustituto.PersonaDiscapacidad.Persona.IdNacionalidad),
                            IdTipoSangre = Convert.ToInt32(personaSustituto.PersonaDiscapacidad.Persona.IdTipoSangre),
                            IdEtnia = Convert.ToInt32(personaSustituto.PersonaDiscapacidad.Persona.IdEtnia),
                            Identificacion = personaSustituto.PersonaDiscapacidad.Persona.Identificacion,
                            Nombres = personaSustituto.PersonaDiscapacidad.Persona.Nombres,
                            Apellidos = personaSustituto.PersonaDiscapacidad.Persona.Apellidos,
                            TelefonoPrivado = personaSustituto.PersonaDiscapacidad.Persona.TelefonoPrivado,
                            TelefonoCasa = personaSustituto.PersonaDiscapacidad.Persona.TelefonoCasa,
                            CorreoPrivado = personaSustituto.PersonaDiscapacidad.Persona.CorreoPrivado,
                            LugarTrabajo = personaSustituto.PersonaDiscapacidad.Persona.LugarTrabajo,
                            IdNacionalidadIndigena = personaSustituto.PersonaDiscapacidad.Persona.IdNacionalidadIndigena,
                            CallePrincipal = personaSustituto.PersonaDiscapacidad.Persona.CallePrincipal,
                            CalleSecundaria = personaSustituto.PersonaDiscapacidad.Persona.CalleSecundaria,
                            Referencia = personaSustituto.PersonaDiscapacidad.Persona.Referencia,
                            Numero = personaSustituto.PersonaDiscapacidad.Persona.Numero,
                            IdPaisLugarPersona = parroquia.Ciudad.Provincia.IdPais,
                            IdProvinciaLugarPersona = parroquia.Ciudad.Provincia.IdProvincia,
                            IdCiudadLugarPersona = parroquia.IdCiudad,
                            IdParroquia = Convert.ToInt32(personaSustituto.PersonaDiscapacidad.Persona.IdParroquia),
                            Ocupacion = personaSustituto.PersonaDiscapacidad.Persona.Ocupacion,
                            IdParentesco = personaSustituto.Parentesco.IdParentesco,
                            IdTipoDiscapacidad = personaSustituto.PersonaDiscapacidad.IdTipoDiscapacidad,
                            IdEmpleado = personaSustituto.IdPersonaSustituto,
                            NumeroCarnet= personaSustituto.PersonaDiscapacidad.NumeroCarnet,
                            Porciento = personaSustituto.PersonaDiscapacidad.Porciento,
                            IdPersonaDiscapacidad = personaSustituto.PersonaDiscapacidad.IdPersonaDiscapacidad

                        };
                        await CargarCombosEdit();
                        return View(personaSustituto);
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
        public async Task<IActionResult> EditPersonaSustituto(string id, EmpleadoSustitutoViewModel empleadoSustitutoViewModel)
        {
            Response response = new Response();
            try
            {


                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, empleadoSustitutoViewModel, new Uri(WebApp.BaseAddress),
                                                                 "api/PersonaSustituto");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Sistema", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado una persona sustituto",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("IndexPersonaSustituto");
                    }
                    ViewData["Error"] = response.Message;
                    await CargarCombos(empleadoSustitutoViewModel);
                    return View(empleadoSustitutoViewModel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una persona sustituto",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }

        public async Task<IActionResult> IndexPersonaSustituto()
        {

            var empleado = await ObtenerEmpleado();
            var personaSustituto = new PersonaSustituto();
            var respuesta = new Response();
            try
            {

                respuesta = await apiServicio.ObtenerElementoAsync1<Response>(empleado, new Uri(WebApp.BaseAddress),
                                                              "api/PersonaSustituto/ObtenerPersonaSustitutosPorId");

                if (respuesta.Resultado != null)
                {
                    personaSustituto = JsonConvert.DeserializeObject<PersonaSustituto>(respuesta.Resultado.ToString());
                    return View(personaSustituto);
                }
                else
                {
                    PersonaSustituto personaSustitutoVacio = new PersonaSustituto();
                    return View(personaSustitutoVacio);
                }

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Obteniendo persona sustituto",
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
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
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/PersonaSustituto");
                if (response.IsSuccess)
                {
                    await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                    {
                        ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                        EntityID = string.Format("{0} : {1}", "Sistema", id),
                        Message = "Registro de persona sustituto eliminado",
                        LogCategoryParametre = Convert.ToString(LogCategoryParameter.Delete),
                        LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                        UserName = "Usuario APP webappth"
                    });
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