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
            ViewData["IdPaisDireccion"] = new SelectList(listaPaises, "IdPais", "Nombre", empleadoFamiliarViewModel.IdPaisLugarPersona);

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
        public async Task<IActionResult> EditEmpleadoFamiliar(string id, ViewModelTrayectoriaLaboral viewModelTrayectoriaLaboral)
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

        public async Task<IActionResult> IndexEmpleadoFamiliar()
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

        public async Task<IActionResult> DeleteEmpleadoFamiliar(string id)
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
        public Task<Empleado> ObtenerEmpleado()
        {
            var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();
            var NombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var empleadoJson = ObtenerEmpleadoLogueado(NombreUsuario);
            return empleadoJson;
        }
    }
}