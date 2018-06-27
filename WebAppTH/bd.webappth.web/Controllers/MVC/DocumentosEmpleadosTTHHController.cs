using bd.log.guardar.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using EnviarCorreo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SendMails.methods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace bd.webappth.web.Controllers.MVC
{

    public class DocumentosEmpleadosTTHHController : Controller
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


        public DocumentosEmpleadosTTHHController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Index()
        {
            

            var lista = new List<ListaEmpleadoViewModel>();
            try
            {
                lista = await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleados");

                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task<IActionResult> CheckListDocumentosEmpleado(string identificacion, string mensaje)
        {
            InicializarMensaje(mensaje);

            var listaDocumentos = new List<DocumentosIngreso>();
            var listaDocumentosEntregados = new List<DocumentosIngresoEmpleado>();

            var documentoingresoViewModel = new ViewModelDocumentoIngresoEmpleado();
            documentoingresoViewModel.listadocumentosingreso = new List<DocumentosIngreso>();
            documentoingresoViewModel.listadocumentosingresoentregado = new List<DocumentosIngresoEmpleado>();

            try
            {
                var empleado = new Empleado
                {
                    Persona = new Persona
                    {
                        Identificacion = identificacion
                    }
                };
                var emp = await apiServicio.ObtenerElementoAsync1<ListaEmpleadoViewModel>(empleado, new Uri(WebApp.BaseAddress),
                                                              "api/Empleados/ObtenerDatosCompletosEmpleado");

                var empleadoConsulta = new Empleado
                {
                    IdEmpleado = emp.IdEmpleado
                };
                //var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(empleadoConsulta, new Uri(WebApp.BaseAddress), "api/DocumentosIngreso/GetDocumentoIngresoEmpleado");
                //if (!respuesta.IsSuccess)
                //{
                listaDocumentos = await apiServicio.Listar<DocumentosIngreso>(new Uri(WebApp.BaseAddress)
                                     , "api/DocumentosIngreso/ListarDocumentosIngreso");

                listaDocumentosEntregados = await apiServicio.ObtenerElementoAsync1<List<DocumentosIngresoEmpleado>>(empleadoConsulta, new Uri(WebApp.BaseAddress), "api/DocumentosIngreso/ListarDocumentosIngresoEmpleado");

                documentoingresoViewModel = new ViewModelDocumentoIngresoEmpleado
                {
                    empleadoViewModel = emp,
                    listadocumentosingreso = listaDocumentos.OrderBy(o=>o.IdDocumentosIngreso).ToList(),
                    listadocumentosingresoentregado = listaDocumentosEntregados.OrderBy(o => o.IdDocumentosIngreso).ToList()

                };



                return View(documentoingresoViewModel);
                //}

            }
            catch (Exception)
            {
                return View(documentoingresoViewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DocumentoEntregado(ViewModelDocumentoIngresoEmpleado viewModelDocumentoIngresoEmpleado)
        {
            Response response = new Response();
            var listaDocumentosEntregados = new List<DocumentosIngresoEmpleado>();
            try
            {
                var empleado = new Empleado
                {
                    IdEmpleado = viewModelDocumentoIngresoEmpleado.empleadoViewModel.IdEmpleado
                };

                listaDocumentosEntregados = await apiServicio.ObtenerElementoAsync1<List<DocumentosIngresoEmpleado>>(empleado, new Uri(WebApp.BaseAddress)
                                                                  , "api/DocumentosIngreso/ListarDocumentosIngresoEmpleado");

                if (listaDocumentosEntregados.Count == 0)
                {
                    response = await apiServicio.InsertarAsync(viewModelDocumentoIngresoEmpleado,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/DocumentosIngreso/InsertarDocumentosIngresoEmpleado");
                }
                else
                {
                    response = await apiServicio.ObtenerElementoAsync1<Response>(viewModelDocumentoIngresoEmpleado, new Uri(WebApp.BaseAddress),
                                                                                     "api/DocumentosIngreso/EditarCheckListDocumentos");
                }

                if (response.IsSuccess)
                {

                    return RedirectToAction("CheckListDocumentosEmpleado", new { identificacion = viewModelDocumentoIngresoEmpleado.empleadoViewModel.Identificacion, mensaje = response.Message });
                }

                ViewData["Error"] = response.Message;
                return RedirectToAction("CheckListDocumentosEmpleado", new { identificacion = viewModelDocumentoIngresoEmpleado.empleadoViewModel.Identificacion });

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        public async Task<IActionResult> LavadoActivos(string identificacion, string mensaje)
        {
            InicializarMensaje("");

            var model = new LavadoActivoEmpleadoViewModel();
            model.DatosBasicosEmpleadoViewModel = new DatosBasicosEmpleadoViewModel();

            var emp = new ListaEmpleadoViewModel();
            

            try
            {

                var empleado = new Empleado
                {
                    Persona = new Persona
                    {
                        Identificacion = identificacion
                    }
                };

                emp = await apiServicio.ObtenerElementoAsync1<ListaEmpleadoViewModel>(empleado, new Uri(WebApp.BaseAddress),
                                                              "api/Empleados/ObtenerDatosCompletosEmpleado");



                model.DatosBasicosEmpleadoViewModel.IdEmpleado = emp.IdEmpleado;
                
                Response response = await apiServicio.InsertarAsync(model,
                                                         new Uri(WebApp.BaseAddress),
                                                         "api/LavadoActivoEmpleados/ExisteLavadoActivosPorIdEmpleado");

                if (response.IsSuccess) {

                    InicializarMensaje("Aqui va el reporte");
                    return View(emp);
                }


                
                InicializarMensaje("El empleado aún no ha completado este documento");
                return View(emp);

            }
            catch (Exception ex)
            {
                return View(model);
            }

            
        }

    }
}