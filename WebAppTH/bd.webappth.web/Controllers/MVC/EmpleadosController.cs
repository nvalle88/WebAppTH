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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace bd.webappth.web.Controllers.MVC
{

    public class EmpleadosController : Controller
    {

        public class ObtenerInstancia
        {
            private static EmpleadoViewModel instance;

            private ObtenerInstancia() { }

            public static EmpleadoViewModel Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new EmpleadoViewModel();
                        instance.Persona = new Persona();
                        instance.Empleado = new Empleado();
                        instance.DatosBancarios = new DatosBancarios();
                        instance.EmpleadoContactoEmergencia = new EmpleadoContactoEmergencia();
                        instance.IndiceOcupacionalModalidadPartida = new IndiceOcupacionalModalidadPartida();
                        instance.PersonaEstudio = new List<PersonaEstudio>();
                        instance.TrayectoriaLaboral = new List<TrayectoriaLaboral>();
                        instance.PersonaDiscapacidad = new List<PersonaDiscapacidad>();
                        instance.PersonaEnfermedad = new List<PersonaEnfermedad>();
                        instance.PersonaSustituto = new PersonaSustituto();
                        instance.DiscapacidadSustituto = new List<DiscapacidadSustituto>();
                        instance.EnfermedadSustituto = new List<EnfermedadSustituto>();
                        instance.EmpleadoFamiliar = new List<EmpleadoFamiliar>();

                    }
                    return instance;
                }
                set
                {
                    instance = null;
                }
            }
        }

        private readonly IApiServicio apiServicio;


        public EmpleadosController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public async Task<IActionResult> Index()
        {
            var lista = new List<ListaEmpleadoViewModel>();
            try
            {
                lista = await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/Empleados/ListarEmpleados");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando empleados",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }




        private async Task CargarCombos()
        {
            //Tabla Persona
            ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "/api/Sexos/ListarSexos"), "IdSexo", "Nombre");
            ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "/api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
            ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "/api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
            ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "/api/Generos/ListarGeneros"), "IdGenero", "Nombre");
            ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "/api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
            ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "/api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
            ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "/api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");

            ViewData["IdPaisLugarNacimiento"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdPaisLugarSufragio"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdPaisDireccion"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");

            ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "/api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");
            ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "/api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");

            ViewData["IdRegimenLaboral"] = new SelectList(await apiServicio.Listar<RegimenLaboral>(new Uri(WebApp.BaseAddress), "/api/RegimenesLaborales/ListarRegimenesLaborales"), "IdRegimenLaboral", "Nombre");
            ViewData["IdFondoFinanciamiento"] = new SelectList(await apiServicio.Listar<FondoFinanciamiento>(new Uri(WebApp.BaseAddressRM), "/api/FondoFinanciamiento/ListarFondoFinanciamiento"), "IdFondoFinanciamiento", "Nombre");
            ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "/api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
            ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "/api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
            ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "/api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");

        }

       



        public async Task<string> InsertarEmpleado(EmpleadoViewModel empleadoViewModel)
        {
            string mensaje = string.Empty;

            var ins = ObtenerInstancia.Instance;

            ins.Persona = empleadoViewModel.Persona;
            ins.Empleado = empleadoViewModel.Empleado;
            ins.DatosBancarios = empleadoViewModel.DatosBancarios;
            ins.EmpleadoContactoEmergencia = empleadoViewModel.EmpleadoContactoEmergencia;
            ins.IndiceOcupacionalModalidadPartida = empleadoViewModel.IndiceOcupacionalModalidadPartida;
            ins.PersonaSustituto = empleadoViewModel.PersonaSustituto;

            var response = await apiServicio.InsertarAsync(ins, new Uri(WebApp.BaseAddress), "/api/Empleados/InsertarEmpleado");

            if (response.IsSuccess)
            {
                LogEntryTranfer logEntryTranfer = new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    ExceptionTrace = null,
                    Message = "Se ha creado un empleado",
                    UserName = "Usuario 1",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Create),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                    EntityID = "Empleado",
                    ObjectPrevious = "NULL",
                    ObjectNext = JsonConvert.SerializeObject(response.Resultado),
                };

                var responseLog = await GuardarLogService.SaveLogEntry(logEntryTranfer);

                mensaje = Mensaje.Satisfactorio;

                ObtenerInstancia.Instance = null;
                return mensaje;
            }
            mensaje = response.Message;
            return mensaje;

        }



        public async Task<string> ActualizarEmpleado(EmpleadoViewModel empleadoViewModel)
        {
            string mensaje = string.Empty;



            return mensaje;

        }


        [HttpPost]
        public async Task<JsonResult> NuevoEmpleado(EmpleadoViewModel empleadoViewModel)
        {

            string mensaje = string.Empty;
            mensaje = InsertarEmpleado(empleadoViewModel).Result;

            if (!mensaje.Equals("La acción se ha realizado satisfactoriamente"))
                return Json(mensaje);
            else
                return Json(new { result = "Redireccionar", url = Url.Action("Index", "Empleados") });

        }


        [HttpPost]
        public async Task<JsonResult> EditarEmpleado(EmpleadoViewModel empleadoViewModel)
        {

            string mensaje = string.Empty;
            mensaje = ActualizarEmpleado(empleadoViewModel).Result;

            if (!mensaje.Equals("La acción se ha realizado satisfactoriamente"))
                return Json(mensaje);
            else
                return Json(new { result = "Redireccionar", url = Url.Action("Index", "Empleados") });

        }


       

        public async Task<IActionResult> Create()
        {

            ObtenerInstancia.Instance = null;

            await CargarCombos();

            return View();
        }


        public async Task<JsonResult> ListarNacionalidadIndigena(string etnia)
        {
            var Etnia = new NacionalidadIndigena
            {
                IdEtnia = Convert.ToInt32(etnia),
            };
            var listaNacionalidadIndigena = await apiServicio.Listar<NacionalidadIndigena>(Etnia, new Uri(WebApp.BaseAddress), "api/NacionalidadesIndigenas/ListarNacionalidadesIndigenasPorEtnias");
            return Json(listaNacionalidadIndigena);
        }

        public async Task<JsonResult> ListarCiudadesPorPais(string pais)
        {
            var Pais = new Pais
            {
                IdPais = Convert.ToInt32(pais),
            };
            var listaCiudades = await apiServicio.Listar<Ciudad>(Pais, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorPais");
            return Json(listaCiudades);
        }

        public async Task<JsonResult> ListarProvinciaPorPais(string pais)
        {
            var Pais = new Pais
            {
                IdPais = Convert.ToInt32(pais),
            };
            var listaProvincias = await apiServicio.Listar<Provincia>(Pais, new Uri(WebApp.BaseAddress), "api/Provincia/ListarProvinciaPorPais");
            return Json(listaProvincias);
        }

        public async Task<JsonResult> ListarCiudadPorProvincia(string provincia)
        {
            var Provincia = new Provincia
            {
                IdProvincia = Convert.ToInt32(provincia),
            };
            var listaCiudades = await apiServicio.Listar<Ciudad>(Provincia, new Uri(WebApp.BaseAddress), "api/Ciudad/ListarCiudadPorProvincia");
            return Json(listaCiudades);
        }

        public async Task<JsonResult> ListarParroquiaPorCiudad(int idCiudad)
        {
            var Ciudad = new Ciudad
            {
                IdCiudad = Convert.ToInt32(idCiudad),
            };
            var listaParroquias = await apiServicio.Listar<Parroquia>(Ciudad, new Uri(WebApp.BaseAddress), "api/Parroquia/ListarParroquiaPorCiudad");
            return Json(listaParroquias);
        }

        public async Task<JsonResult> ListarAreasConocimientosporEstudio(int idEstudio)
        {
            var Estudio = new Estudio
            {
                IdEstudio = Convert.ToInt32(idEstudio),
            };
            var listaAreasConocimientos = await apiServicio.Listar<AreaConocimiento>(Estudio, new Uri(WebApp.BaseAddress), "api/AreasConocimientos/ListarAreasConocimientosporEstudio");
            return Json(listaAreasConocimientos);
        }

        public async Task<JsonResult> ListarTitulosporAreaConocimiento(int idAreaConocimiento, int idEstudio)
        {
            var Titulo = new Titulo
            {
                IdAreaConocimiento = idAreaConocimiento,
                IdEstudio = idEstudio
            };
            var listaTitulos = await apiServicio.Listar<Titulo>(Titulo, new Uri(WebApp.BaseAddress), "api/Titulos/ListarTitulosporAreaConocimiento");
            return Json(listaTitulos);
        }

        public async Task<JsonResult> InsertarEmpleadoPersona(int idTipoIdentificacion, string Identificacion, string Nombres, string Apellidos, int idSexo, int idGenero, int idEstadoCivil, int idTipoSangre, int idNacionalidad, int etniaF, int nacionalidadIndigenaF, string CorreoPrivado, string FechaNacimiento, string LugarTrabajo, string CallePrincipal, string CalleSecundaria, string Referencia, string Numero, int parroquiaLugarFamiliar, string TelefonoPrivado, string TelefonoCasa, int Parentesco)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;

                empleadoviewmodel.EmpleadoFamiliar.Add(new EmpleadoFamiliar
                {
                    IdParentesco = Parentesco,
                    Persona = new Persona
                    {
                        IdTipoIdentificacion = idTipoIdentificacion,
                        Identificacion = Identificacion,
                        Nombres = Nombres,
                        Apellidos = Apellidos,
                        IdSexo = idSexo,
                        IdGenero = idGenero,
                        IdEstadoCivil = idEstadoCivil,
                        IdTipoSangre = idTipoSangre,
                        IdNacionalidad = idNacionalidad,
                        IdEtnia = etniaF,
                        IdNacionalidadIndigena = nacionalidadIndigenaF,
                        CorreoPrivado = CorreoPrivado,
                        LugarTrabajo = LugarTrabajo,
                        CallePrincipal = CallePrincipal,
                        CalleSecundaria = CalleSecundaria,
                        Referencia = Referencia,
                        Numero = Numero,
                        IdParroquia = parroquiaLugarFamiliar,
                        TelefonoPrivado = TelefonoPrivado,
                        TelefonoCasa = TelefonoCasa
                    }

                }

                );

                return Json(true);
            }
            catch (Exception ex)
            {

                return Json(false);
            }

        }

        public async Task<JsonResult> InsertarFamiliar(int idTipoIdentificacion, string Identificacion, string Nombres, string Apellidos, int idSexo, int idGenero, int idEstadoCivil, int idTipoSangre, int idNacionalidad, int etniaF, int nacionalidadIndigenaF, string CorreoPrivado, DateTime FechaNacimiento, string LugarTrabajo, string CallePrincipal, string CalleSecundaria, string Referencia, string Numero, int parroquiaLugarFamiliar, string TelefonoPrivado, string TelefonoCasa, int Parentesco, string Ocupacion)
        {
            try
            {

                if (!String.IsNullOrEmpty(idTipoIdentificacion.ToString()) && !String.IsNullOrEmpty(Identificacion) && !String.IsNullOrEmpty(Nombres) && !String.IsNullOrEmpty(Apellidos) && !String.IsNullOrEmpty(idSexo.ToString()) && !String.IsNullOrEmpty(idGenero.ToString()) && !String.IsNullOrEmpty(idEstadoCivil.ToString()) && !String.IsNullOrEmpty(idTipoSangre.ToString()) && !String.IsNullOrEmpty(idNacionalidad.ToString()) && !String.IsNullOrEmpty(etniaF.ToString()) && !String.IsNullOrEmpty(nacionalidadIndigenaF.ToString()) && !String.IsNullOrEmpty(CorreoPrivado) && !String.IsNullOrEmpty(FechaNacimiento.ToString()) && !String.IsNullOrEmpty(CallePrincipal) && !String.IsNullOrEmpty(CalleSecundaria) && !String.IsNullOrEmpty(Referencia) && !String.IsNullOrEmpty(Numero) && !String.IsNullOrEmpty(parroquiaLugarFamiliar.ToString()) && !String.IsNullOrEmpty(TelefonoPrivado) && !String.IsNullOrEmpty(TelefonoCasa) && !String.IsNullOrEmpty(Parentesco.ToString()) && !String.IsNullOrEmpty(Ocupacion))
                {
                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.EmpleadoFamiliar.Exists(x => x.Persona.IdTipoIdentificacion == idTipoIdentificacion && x.Persona.Identificacion == Identificacion && x.Persona.Nombres == Nombres && x.Persona.Apellidos == Apellidos);
                    if (!existe)
                    {
                        empleadoviewmodel.EmpleadoFamiliar.Add
                        (new EmpleadoFamiliar
                        {
                            IdParentesco = Parentesco,
                            Persona = new Persona
                            {
                                IdTipoIdentificacion = idTipoIdentificacion,
                                Identificacion = Identificacion,
                                Nombres = Nombres,
                                Apellidos = Apellidos,
                                IdSexo = idSexo,
                                IdGenero = idGenero,
                                IdEstadoCivil = idEstadoCivil,
                                IdTipoSangre = idTipoSangre,
                                IdNacionalidad = idNacionalidad,
                                IdEtnia = etniaF,
                                IdNacionalidadIndigena = nacionalidadIndigenaF,
                                CorreoPrivado = CorreoPrivado,
                                FechaNacimiento = FechaNacimiento,
                                LugarTrabajo = LugarTrabajo,
                                CallePrincipal = CallePrincipal,
                                CalleSecundaria = CalleSecundaria,
                                Referencia = Referencia,
                                Numero = Numero,
                                IdParroquia = parroquiaLugarFamiliar,
                                TelefonoPrivado = TelefonoPrivado,
                                TelefonoCasa = TelefonoCasa,
                                Ocupacion = Ocupacion
                            }
                        }
                        );

                        return Json(true);
                    }
                    else
                    {
                        ViewData["InsertarFamiliar"] = Mensaje.ExisteRegistro;
                        return Json(false);
                    }
                    return Json(false);
                }

            }
            catch (Exception ex)
            {
                return Json(false);
            }

            return Json(false);
        }

        public async Task<JsonResult> EliminarFamiliar(string id)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.EmpleadoFamiliar.Find(c => c.Persona.Identificacion == id);
                empleadoviewmodel.EmpleadoFamiliar.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> InsertarFormacionAcademica(int IdTitulo, string Observaciones, DateTime FechaGraduado, string NoSenescyt)
        {
            try
            {
                if (!String.IsNullOrEmpty(IdTitulo.ToString()) && !String.IsNullOrEmpty(Observaciones) && !String.IsNullOrEmpty(FechaGraduado.ToString()) && !String.IsNullOrEmpty(NoSenescyt))
                {

                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.PersonaEstudio.Exists(x => x.IdTitulo == IdTitulo && x.NoSenescyt == NoSenescyt && x.FechaGraduado == FechaGraduado);

                    if (!existe)
                    {
                        empleadoviewmodel.PersonaEstudio.Add(new PersonaEstudio
                        {
                            FechaGraduado = FechaGraduado.Date,
                            Observaciones = Observaciones,
                            IdTitulo = IdTitulo,
                            NoSenescyt = NoSenescyt
                        }
                    );
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }

            }
            catch (Exception ex)
            {

                return Json(false);
            }

            return Json(false);
        }


        public async Task<JsonResult> EliminarFormacionAcademica(int idTitulo)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.PersonaEstudio.Find(c => c.IdTitulo == idTitulo);
                empleadoviewmodel.PersonaEstudio.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> InsertarTrayectoriaLaboral(DateTime FechaInicio, DateTime FechaFin, string Empresa, string PuestoTrabajo, string DescripcionFunciones)
        {
            try
            {

                if (!String.IsNullOrEmpty(FechaInicio.ToString()) && !String.IsNullOrEmpty(FechaFin.ToString()))
                {

                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.TrayectoriaLaboral.Exists(x => x.FechaInicio == FechaInicio && x.FechaFin == FechaFin);
                    if (!existe)
                    {
                        empleadoviewmodel.TrayectoriaLaboral.Add(new TrayectoriaLaboral
                        {
                            FechaInicio = FechaInicio.Date,
                            FechaFin = FechaFin.Date,
                            Empresa = Empresa,
                            PuestoTrabajo = PuestoTrabajo,
                            DescripcionFunciones = DescripcionFunciones
                        }
                        );
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {

                return Json(false);
            }

            return Json(false);
        }


        public async Task<JsonResult> EliminarTrayectoriaLaboral(DateTime fechainicio, DateTime fechafin)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.TrayectoriaLaboral.Find(c => c.FechaInicio == fechainicio.Date && c.FechaFin == fechafin.Date);
                empleadoviewmodel.TrayectoriaLaboral.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }


        public async Task<JsonResult> InsertarDiscapacidad(int IdTipoDiscapacidad, string NumeroCarnet, int Porciento)
        {
            try
            {
                if (!String.IsNullOrEmpty(IdTipoDiscapacidad.ToString()) && !String.IsNullOrEmpty(NumeroCarnet) && !String.IsNullOrEmpty(Porciento.ToString()))
                {
                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.PersonaDiscapacidad.Exists(x => x.IdTipoDiscapacidad == IdTipoDiscapacidad && x.NumeroCarnet == NumeroCarnet && x.Porciento == Porciento);

                    if (!existe)
                    {
                        empleadoviewmodel.PersonaDiscapacidad.Add(new PersonaDiscapacidad
                        {
                            IdTipoDiscapacidad = IdTipoDiscapacidad,
                            NumeroCarnet = NumeroCarnet,
                            Porciento = Porciento,
                        }
                         );

                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(false);
        }


        public async Task<JsonResult> EliminarDiscapacidad(int idTipoDiscapacidad)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.PersonaDiscapacidad.Find(c => c.IdTipoDiscapacidad == idTipoDiscapacidad);
                empleadoviewmodel.PersonaDiscapacidad.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }


        public async Task<JsonResult> InsertarEnfermedad(int IdTipoEnfermedad, string InstitucionEmite)
        {
            try
            {
                if (!String.IsNullOrEmpty(IdTipoEnfermedad.ToString()) && !String.IsNullOrEmpty(InstitucionEmite))
                {

                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.PersonaEnfermedad.Exists(x => x.IdTipoEnfermedad == IdTipoEnfermedad && x.InstitucionEmite == InstitucionEmite);
                    if (!existe)
                    {
                        empleadoviewmodel.PersonaEnfermedad.Add(new PersonaEnfermedad
                        {
                            IdTipoEnfermedad = IdTipoEnfermedad,
                            InstitucionEmite = InstitucionEmite,
                        }
                       );
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(false);

        }


        public async Task<JsonResult> EliminarEnfermedad(int idTipoEnfermedad)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.PersonaEnfermedad.Find(c => c.IdTipoEnfermedad == idTipoEnfermedad);
                empleadoviewmodel.PersonaEnfermedad.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> InsertarDiscapacidadSustituto(int IdTipoDiscapacidadSustituto, string NumeroCarnetSustituto, int PorcientoSustituto)
        {
            try
            {
                if (!String.IsNullOrEmpty(IdTipoDiscapacidadSustituto.ToString()) && !String.IsNullOrEmpty(NumeroCarnetSustituto) && !String.IsNullOrEmpty(PorcientoSustituto.ToString()))
                {

                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.DiscapacidadSustituto.Exists(x => x.IdTipoDiscapacidad == IdTipoDiscapacidadSustituto && x.NumeroCarnet == NumeroCarnetSustituto && x.PorcentajeDiscapacidad == PorcientoSustituto);

                    if (!existe)
                    {
                        empleadoviewmodel.DiscapacidadSustituto.Add(new DiscapacidadSustituto
                        {
                            IdTipoDiscapacidad = IdTipoDiscapacidadSustituto,
                            NumeroCarnet = NumeroCarnetSustituto,
                            PorcentajeDiscapacidad = PorcientoSustituto,
                        }
                        );
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }

            return Json(false);
        }


        public async Task<JsonResult> EliminarDiscapacidadSustituto(int idTipoDiscapacidadSustituto)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.DiscapacidadSustituto.Find(c => c.IdTipoDiscapacidad == idTipoDiscapacidadSustituto);
                empleadoviewmodel.DiscapacidadSustituto.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }


        public async Task<JsonResult> InsertarEnfermedadSustituto(int IdTipoEnfermedadSustituto, string InstitucionEmiteSustituto)
        {
            try
            {
                if (!string.IsNullOrEmpty(IdTipoEnfermedadSustituto.ToString()) && !string.IsNullOrEmpty(InstitucionEmiteSustituto))
                {

                    var empleadoviewmodel = ObtenerInstancia.Instance;

                    bool existe = empleadoviewmodel.EnfermedadSustituto.Exists(x => x.IdTipoEnfermedad == IdTipoEnfermedadSustituto && x.InstitucionEmite == InstitucionEmiteSustituto);

                    if (!existe)
                    {
                        empleadoviewmodel.EnfermedadSustituto.Add(new EnfermedadSustituto
                        {
                            IdTipoEnfermedad = IdTipoEnfermedadSustituto,
                            InstitucionEmite = InstitucionEmiteSustituto,
                        }
                        );
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(false);
        }


        public async Task<JsonResult> EliminarEnfermedadSustituto(int idTipoEnfermedadSustituto)
        {
            try
            {
                var empleadoviewmodel = ObtenerInstancia.Instance;
                var elemento = empleadoviewmodel.EnfermedadSustituto.Find(c => c.IdTipoEnfermedad == idTipoEnfermedadSustituto);
                empleadoviewmodel.EnfermedadSustituto.Remove(elemento);
                return Json(true);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }
        

        public async Task<JsonResult> ListarRelacionesLaboralesPorRegimen(int regimen)
        {
            try
            {
                var regimenLaboral = new RegimenLaboral
                {
                    IdRegimenLaboral = regimen,
                };
                var listarelacionesLaborales = await apiServicio.Listar<RelacionLaboral>(regimenLaboral, new Uri(WebApp.BaseAddress), "api/RelacionesLaborales/ListarRelacionesLaboralesPorRegimen");
                return Json(listarelacionesLaborales);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }


        [HttpPost]
        public async Task<JsonResult> llenarTrayectoriaLaboralJSON(EmpleadoViewModel objetoEmpleadoViewModel)
        {

            string mensaje = "Error al añadir en la lista";
            EmpleadoViewModel empleadoViewModel;

            try
            {
                empleadoViewModel = await apiServicio.ObtenerElementoAsync1<EmpleadoViewModel>(objetoEmpleadoViewModel.Empleado.IdEmpleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerTrayectoriaLaboralEmpleado");

                if (empleadoViewModel.TrayectoriaLaboral != null)
                {
                    //Enviando la lista como json
                    return Json(JsonConvert.SerializeObject(empleadoViewModel.TrayectoriaLaboral));
                }
                else
                {
                    return Json(mensaje);
                }

            }
            catch (Exception ex)
            {
                return Json(mensaje);
            }
        }

        [HttpPost]
        public async Task<JsonResult> llenarPersonaEstudioJSON(EmpleadoViewModel objetoEmpleadoViewModel)
        {

            string mensaje = "Error al añadir en la lista";
           List<PersonaEstudioViewModel> personaEstudioViewModel=new List<PersonaEstudioViewModel>();

            try
            {
                personaEstudioViewModel = await apiServicio.Listar<PersonaEstudioViewModel>(objetoEmpleadoViewModel.Empleado.IdEmpleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerPersonaEstudioEmpleado");

                if (personaEstudioViewModel != null)
                {
                    //Enviando la lista como json
                    Debug.WriteLine("JsonConvert.SerializeObject(personaEstudioViewModel)=" + JsonConvert.SerializeObject(personaEstudioViewModel));
                    return Json(JsonConvert.SerializeObject(personaEstudioViewModel));
                }
                else
                {
                    return Json(mensaje);
                }

            }
            catch (Exception ex)
            {
                return Json(mensaje);
            }
        }

        [HttpPost]
        public async Task<JsonResult> llenarEmpleadoFamiliarJSON(EmpleadoViewModel objetoEmpleadoViewModel)
        {

            string mensaje = "Error al añadir en la lista";
            EmpleadoViewModel empleadoViewModel;

            try
            {
                empleadoViewModel = await apiServicio.ObtenerElementoAsync1<EmpleadoViewModel>(objetoEmpleadoViewModel.Empleado.IdEmpleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerEmpleadoFamiliarEmpleado");

                if (empleadoViewModel.EmpleadoFamiliar != null)
                {
                    //Enviando la lista como json
                    return Json(JsonConvert.SerializeObject(empleadoViewModel.EmpleadoFamiliar));
                }
                else
                {
                    return Json(mensaje);
                }

            }
            catch (Exception ex)
            {
                return Json(mensaje);
            }
        }


        [HttpPost]
        public async Task<JsonResult> llenarPersonaDiscapacidadJSON(EmpleadoViewModel objetoEmpleadoViewModel)
        {

            string mensaje = "Error al añadir en la lista";
            EmpleadoViewModel empleadoViewModel;

            try
            {
                empleadoViewModel = await apiServicio.ObtenerElementoAsync1<EmpleadoViewModel>(objetoEmpleadoViewModel.Empleado.IdEmpleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerPersonaDiscapacidadEmpleado");

                if (empleadoViewModel.PersonaDiscapacidad != null)
                {
                    //Enviando la lista como json
                    return Json(JsonConvert.SerializeObject(empleadoViewModel.PersonaDiscapacidad));
                }
                else
                {
                    return Json(mensaje);
                }

            }
            catch (Exception ex)
            {
                return Json(mensaje);
            }
        }

        [HttpPost]
        public async Task<JsonResult> llenarPersonaEnfermedadJSON(EmpleadoViewModel objetoEmpleadoViewModel)
        {

            string mensaje = "Error al añadir en la lista";
            EmpleadoViewModel empleadoViewModel;

            try
            {
                empleadoViewModel = await apiServicio.ObtenerElementoAsync1<EmpleadoViewModel>(objetoEmpleadoViewModel.Empleado.IdEmpleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerPersonaEnfermedadEmpleado");

                if (empleadoViewModel.PersonaEnfermedad != null)
                {
                    //Enviando la lista como json
                    return Json(JsonConvert.SerializeObject(empleadoViewModel.PersonaEnfermedad));
                }
                else
                {
                    return Json(mensaje);
                }

            }
            catch (Exception ex)
            {
                return Json(mensaje);
            }
        }

        [HttpPost]
        public async Task<JsonResult> llenarDiscapacidadSustitutoJSON(EmpleadoViewModel objetoEmpleadoViewModel)
        {

            string mensaje = "Error al añadir en la lista";
            EmpleadoViewModel empleadoViewModel;

            try
            {
                empleadoViewModel = await apiServicio.ObtenerElementoAsync1<EmpleadoViewModel>(objetoEmpleadoViewModel.Empleado.IdEmpleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerDiscapacidadSustitutoEmpleado");

                if (empleadoViewModel.DiscapacidadSustituto != null)
                {
                    //Enviando la lista como json
                    Debug.WriteLine("JsonConvert.SerializeObject(empleadoViewModel.DiscapacidadSustituto)= " + JsonConvert.SerializeObject(empleadoViewModel.DiscapacidadSustituto));
                    return Json(JsonConvert.SerializeObject(empleadoViewModel.DiscapacidadSustituto));
                }
                else
                {
                    return Json(mensaje);
                }

            }
            catch (Exception ex)
            {
                return Json(mensaje);
            }
        }

        [HttpPost]
        public async Task<JsonResult> llenarEnfermedadSustitutoJSON(EmpleadoViewModel objetoEmpleadoViewModel)
        {

            string mensaje = "Error al añadir en la lista";
            EmpleadoViewModel empleadoViewModel;

            try
            {
                empleadoViewModel = await apiServicio.ObtenerElementoAsync1<EmpleadoViewModel>(objetoEmpleadoViewModel.Empleado.IdEmpleado, new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerEnfermedadSustitutoEmpleado");

                if (empleadoViewModel.EnfermedadSustituto != null)
                {
                    //Enviando la lista como json
                    Debug.WriteLine("JsonConvert.SerializeObject(empleadoViewModel.EnfermedadSustituto)= " + JsonConvert.SerializeObject(empleadoViewModel.EnfermedadSustituto));
                    return Json(JsonConvert.SerializeObject(empleadoViewModel.EnfermedadSustituto));
                }
                else
                {
                    return Json(mensaje);
                }

            }
            catch (Exception ex)
            {
                return Json(mensaje);
            }
        }
        public async Task<JsonResult> ListarModalidadPartidaRelacion(int relacion)
        {
            try
            {
                var relacionLaboral = new RelacionLaboral
                {
                    IdRelacionLaboral = relacion,
                };
                var listaremodalidadPartida = await apiServicio.Listar<ModalidadPartida>(relacionLaboral, new Uri(WebApp.BaseAddress), "api/ModalidadesPartida/ListarModalidadesPartidaPorRelacionLaboral");
                return Json(listaremodalidadPartida);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }

        public async Task<JsonResult> ListarTipoNombramientoRelacion(int relacion)
        {
            try
            {
                var relacionLaboral = new RelacionLaboral
                {
                    IdRelacionLaboral = relacion,
                };
                var listarelacionesLaborales = await apiServicio.Listar<TipoNombramiento>(relacionLaboral, new Uri(WebApp.BaseAddress), "api/TiposDeNombramiento/ListarTiposDeNombramientoPorRelacion");
                return Json(listarelacionesLaborales);
            }
            catch (Exception)
            {
                return Json(Mensaje.Error);
            }

        }
        

        public async Task<IActionResult> ListarEmpleados()
        {

            var lista = new List<ListaEmpleadoViewModel>();
            try
            {
                lista = await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "/api/Empleados/ListarEmpleados");
                return View(lista);
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Listando estados civiles",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.NetActivity),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });
                return BadRequest();
            }
        }


        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {

                    EmpleadoViewModel empleadoViewModel = await apiServicio.ObtenerElementoAsync1<EmpleadoViewModel>(Convert.ToInt32(id), new Uri(WebApp.BaseAddress), "api/Empleados/ObtenerDatosEmpleadoActualizar");

                    ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "/api/Sexos/ListarSexos"), "IdSexo", "Nombre");
                    ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "/api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
                    ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "/api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
                    ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "/api/Generos/ListarGeneros"), "IdGenero", "Nombre");
                    ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "/api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
                    ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "/api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
                    ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "/api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");
                    ViewData["IdNacionalidadIndigena"] = new SelectList(await apiServicio.Listar<NacionalidadIndigena>(new Uri(WebApp.BaseAddress), "/api/NacionalidadesIndigenas/ListarNacionalidadesIndigenas"), "IdNacionalidadIndigena", "Nombre");
                    ViewData["IdProvinciaPorPais"] = new SelectList(await apiServicio.Listar<Provincia>(empleadoViewModel.Persona.Parroquia.Ciudad.Provincia.Pais, new Uri(WebApp.BaseAddress), "/api/Provincia/ListarProvinciaPorPais"), "IdProvincia", "Nombre", empleadoViewModel.Persona.Parroquia.Ciudad.Provincia.IdProvincia);
                    ViewData["IdCiudadPorProvincia"] = new SelectList(await apiServicio.Listar<Ciudad>(empleadoViewModel.Persona.Parroquia.Ciudad.Provincia, new Uri(WebApp.BaseAddress), "/api/Ciudad/ListarCiudadPorPais"), "IdCiudad", "Nombre", empleadoViewModel.Persona.Parroquia.Ciudad.IdCiudad);
                    ViewData["IdPaisLugarSufragio"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre", empleadoViewModel.Persona.Parroquia.Ciudad.Provincia.Pais.IdPais);
                    ViewData["IdParroquia"] = new SelectList(await apiServicio.Listar<Parroquia>(new Uri(WebApp.BaseAddress), "/api/Parroquia/ListarParroquia"), "IdParroquia", "Nombre", empleadoViewModel.Persona.Parroquia.IdParroquia);

                    ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "/api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");

                    ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "/api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");

                    ViewData["IdFondoFinanciamiento"] = new SelectList(await apiServicio.Listar<FondoFinanciamiento>(new Uri(WebApp.BaseAddressRM), "/api/FondoFinanciamiento/ListarFondoFinanciamiento"), "IdFondoFinanciamiento", "Nombre");

                    ViewData["IdPaisDireccion"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");
                    ViewData["IdRegimenLaboral"] = new SelectList(await apiServicio.Listar<RegimenLaboral>(new Uri(WebApp.BaseAddress), "/api/RegimenesLaborales/ListarRegimenesLaborales"), "IdRegimenLaboral", "Nombre");
                    ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "/api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");

                    ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "/api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");

                    ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "/api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");

                    ViewData["IdPaisLugarNacimiento"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre",empleadoViewModel.Empleado.CiudadNacimiento.IdPais);
                    ViewData["IdCiudadLugarNacimiento"] = new SelectList(await apiServicio.Listar<Ciudad>(new Uri(WebApp.BaseAddress), "/api/Ciudad/ListarCiudadPorPais"), "IdCiudad", "Nombre", empleadoViewModel.Empleado.IdCiudadLugarNacimiento);
                    
                    ViewData["IdPaisLugarSufragio"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre", empleadoViewModel.Empleado.ProvinciaSufragio.IdPais);
                    ViewData["IdProvinciaLugarSufragio"] = new SelectList(await apiServicio.Listar<Provincia>(new Uri(WebApp.BaseAddress), "/api/Provincia/ListarProvinciaPorPais"), "IdProvincia", "Nombre", empleadoViewModel.Empleado.IdProvinciaLugarSufragio);

                    ViewData["IdTipoNombramiento"] = new SelectList(await apiServicio.Listar<TipoNombramiento>(new Uri(WebApp.BaseAddress), "/api/TiposDeNombramiento/ListarTiposDeNombramientoPorRelacion"), "IdTipoNombramiento", "Nombre", empleadoViewModel.IndiceOcupacionalModalidadPartida.TipoNombramiento.IdRelacionLaboral);
                    ViewData["IdModalidadPartida"] = new SelectList(await apiServicio.Listar<ModalidadPartida>(new Uri(WebApp.BaseAddress), "/api/ModalidadesPartida/ListarModalidadesPartidaPorRelacionLaboral"), "IdModalidadPartida", "Nombre", empleadoViewModel.IndiceOcupacionalModalidadPartida.TipoNombramiento.IdRelacionLaboral);


                    if (empleadoViewModel!=null)
                    {
                        return View(empleadoViewModel);
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
        public async Task<IActionResult> Edit(string id, EmpleadoViewModel empleadoViewModel)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    
                    response = await apiServicio.EditarAsync(id, empleadoViewModel, new Uri(WebApp.BaseAddress),
                                                                 "/api/Empleados");

                    if (response.IsSuccess)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                            EntityID = string.Format("{0} : {1}", "Empleados", id),
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ADV),
                            Message = "Se ha actualizado un empleado",
                            UserName = "Usuario 1"
                        });

                        return RedirectToAction("Index");
                    }

                    ViewData["Error"] = response.Message;


                    ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "/api/Sexos/ListarSexos"), "IdSexo", "Nombre");
                    ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "/api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
                    ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "/api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
                    ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "/api/Generos/ListarGeneros"), "IdGenero", "Nombre");
                    ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "/api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
                    ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "/api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
                    ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "/api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");
                    ViewData["IdNacionalidadIndigena"] = new SelectList(await apiServicio.Listar<NacionalidadIndigena>(new Uri(WebApp.BaseAddress), "/api/NacionalidadesIndigenas/ListarNacionalidadesIndigenas"), "IdNacionalidadIndigena", "Nombre");
                    ViewData["IdProvinciaPorPais"] = new SelectList(await apiServicio.Listar<Provincia>(empleadoViewModel.Persona.Parroquia.Ciudad.Provincia.Pais, new Uri(WebApp.BaseAddress), "/api/Provincia/ListarProvinciaPorPais"), "IdProvincia", "Nombre", empleadoViewModel.Persona.Parroquia.Ciudad.Provincia.IdProvincia);
                    ViewData["IdCiudadPorProvincia"] = new SelectList(await apiServicio.Listar<Ciudad>(empleadoViewModel.Persona.Parroquia.Ciudad.Provincia, new Uri(WebApp.BaseAddress), "/api/Ciudad/ListarCiudadPorPais"), "IdCiudad", "Nombre", empleadoViewModel.Persona.Parroquia.Ciudad.IdCiudad);
                    ViewData["IdPaisLugarSufragio"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre", empleadoViewModel.Persona.Parroquia.Ciudad.Provincia.Pais.IdPais);
                    ViewData["IdParroquia"] = new SelectList(await apiServicio.Listar<Parroquia>(new Uri(WebApp.BaseAddress), "/api/Parroquia/ListarParroquia"), "IdParroquia", "Nombre", empleadoViewModel.Persona.Parroquia.IdParroquia);
                    ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "/api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera", "Nombre");
                    ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "/api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");
                    ViewData["IdFondoFinanciamiento"] = new SelectList(await apiServicio.Listar<FondoFinanciamiento>(new Uri(WebApp.BaseAddressRM), "/api/FondoFinanciamiento/ListarFondoFinanciamiento"), "IdFondoFinanciamiento", "Nombre");
                    ViewData["IdPaisLugarNacimiento"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");
                    ViewData["IdPaisDireccion"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");
                    ViewData["IdRegimenLaboral"] = new SelectList(await apiServicio.Listar<RegimenLaboral>(new Uri(WebApp.BaseAddress), "/api/RegimenesLaborales/ListarRegimenesLaborales"), "IdRegimenLaboral", "Nombre");
                    ViewData["IdEstudio"] = new SelectList(await apiServicio.Listar<Estudio>(new Uri(WebApp.BaseAddress), "/api/Estudios/ListarEstudios"), "IdEstudio", "Nombre");
                    ViewData["IdTipoDiscapacidad"] = new SelectList(await apiServicio.Listar<TipoDiscapacidad>(new Uri(WebApp.BaseAddress), "/api/TiposDiscapacidades/ListarTiposDiscapacidades"), "IdTipoDiscapacidad", "Nombre");
                    ViewData["IdTipoEnfermedad"] = new SelectList(await apiServicio.Listar<TipoEnfermedad>(new Uri(WebApp.BaseAddress), "/api/TiposEnfermedades/ListarTiposEnfermedades"), "IdTipoEnfermedad", "Nombre");

                    return View(empleadoViewModel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = "Editando una empleado",
                    ExceptionTrace = ex,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();
            }
        }



    }
}