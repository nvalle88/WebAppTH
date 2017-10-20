using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
                        instance.EmpleadoFamiliar = new List<EmpleadoFamiliar>();
                    }
                    return instance;
                }
            }
        }

        private readonly IApiServicio apiServicio;
        

        public EmpleadosController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }

        public IActionResult Index()
        {
            return View();
        }

        private async Task CargarCombos()
        {
            ViewData["IdTipoIdentificacion"] = new SelectList(await apiServicio.Listar<TipoIdentificacion>(new Uri(WebApp.BaseAddress), "/api/TiposIdentificacion/ListarTiposIdentificacion"), "IdTipoIdentificacion", "Nombre");
            ViewData["IdSexo"] = new SelectList(await apiServicio.Listar<Sexo>(new Uri(WebApp.BaseAddress), "/api/Sexos/ListarSexos"), "IdSexo", "Nombre");
            ViewData["IdGenero"] = new SelectList(await apiServicio.Listar<Genero>(new Uri(WebApp.BaseAddress), "/api/Generos/ListarGeneros"), "IdGenero", "Nombre");
            ViewData["IdEstadoCivil"] = new SelectList(await apiServicio.Listar<EstadoCivil>(new Uri(WebApp.BaseAddress), "/api/EstadosCiviles/ListarEstadosCiviles"), "IdEstadoCivil", "Nombre");
            ViewData["IdTipoSangre"] = new SelectList(await apiServicio.Listar<TipoSangre>(new Uri(WebApp.BaseAddress), "/api/TiposDeSangre/ListarTiposDeSangre"), "IdTipoSangre", "Nombre");
            ViewData["IdNacionalidad"] = new SelectList(await apiServicio.Listar<Nacionalidad>(new Uri(WebApp.BaseAddress), "/api/Nacionalidades/ListarNacionalidades"), "IdNacionalidad", "Nombre");
            ViewData["IdEtnia"] = new SelectList(await apiServicio.Listar<Etnia>(new Uri(WebApp.BaseAddress), "/api/Etnias/ListarEtnias"), "IdEtnia", "Nombre");

            ViewData["IdPaisLugarNacimiento"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdPaisLugarSufragio"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");
            ViewData["IdPaisDireccion"] = new SelectList(await apiServicio.Listar<Pais>(new Uri(WebApp.BaseAddress), "/api/Pais/ListarPais"), "IdPais", "Nombre");

            ViewData["IdInstitucionFinanciera"] = new SelectList(await apiServicio.Listar<InstitucionFinanciera>(new Uri(WebApp.BaseAddress), "/api/InstitucionesFinancieras/ListarInstitucionesFinancieras"), "IdInstitucionFinanciera","Nombre");
            ViewData["IdParentesco"] = new SelectList(await apiServicio.Listar<Parentesco>(new Uri(WebApp.BaseAddress), "/api/Parentescos/ListarParentescos"), "IdParentesco", "Nombre");


        }

        [HttpPost]
        public async Task<IActionResult> Create(EmpleadoViewModel empleadoViewModel)
        {

          var ins=  ObtenerInstancia.Instance;

            ins.Empleado = empleadoViewModel.Empleado;
            ins.Persona = empleadoViewModel.Persona;
            ins.DatosBancarios = empleadoViewModel.DatosBancarios;

           var response= await apiServicio.InsertarAsync(ins,new Uri( WebApp.BaseAddress), "/api/empleados/insertar");

            if (response.IsSuccess)
            {

            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create()
        {

            await  CargarCombos();

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

        public async Task<JsonResult> InsertarEmpleadoPersona(int idTipoIdentificacion, string Identificacion, string Nombres, string Apellidos, int idSexo, int idGenero, int idEstadoCivil, int idTipoSangre, int idNacionalidad, int etniaF, int nacionalidadIndigenaF, string CorreoPrivado, string FechaNacimiento, string LugarTrabajo, string CallePrincipal, string CalleSecundaria, string Referencia, string Numero, int parroquiaLugarFamiliar, string TelefonoPrivado, string TelefonoCasa, int Parentesco)
        {
            try
            {
                //EmpleadoViewModel empleadoviewmodel = new EmpleadoViewModel();
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

        public async Task<JsonResult> InsertarFamiliar(int idTipoIdentificacion, string Identificacion, string Nombres, string Apellidos, int idSexo, int idGenero, int idEstadoCivil, int idTipoSangre, int idNacionalidad, int etniaF, int nacionalidadIndigenaF, string CorreoPrivado, string FechaNacimiento, string LugarTrabajo, string CallePrincipal, string CalleSecundaria, string Referencia, string Numero, int parroquiaLugarFamiliar, string TelefonoPrivado, string TelefonoCasa, int Parentesco)
        {
            try
            {
                //EmpleadoViewModel empleadoviewmodel = new EmpleadoViewModel();
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

                //foreach (var item in empleadoviewmodel.EmpleadoFamiliar)
                //{

                //    var persona = item.Persona;

                //    var personaReturn= DB.save(persona);

                //    item.IdEmpleado = variable;
                //    item.IdPersona = personaReturn.id;

                //    DBNull,save()




                //}
                return Json(true);
            }
            catch (Exception ex)
            {

                return Json(false);
            }

            
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
    }
}