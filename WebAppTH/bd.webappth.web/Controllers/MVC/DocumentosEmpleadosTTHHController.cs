using bd.log.guardar.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Extensores;
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
        /*
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
        */

        private readonly IApiServicio apiServicio;


        public DocumentosEmpleadosTTHHController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }


        #region Mantenimiento de los documentos entregados por el empleado

        public async Task<IActionResult> Index()
        {


            var lista = new List<DistributivoHistorico>();
            try
            {
                lista = await apiServicio.Listar<DistributivoHistorico>(
                    new Uri(WebApp.BaseAddress),
                    "api/Distributivos/ObtenerDistributivoFormal"
                );


                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        
        public async Task<IActionResult> CheckListDocumentosEmpleado(string id)
        {

            if (String.IsNullOrEmpty(id))
            {
                return this.RedireccionarMensajeTime(
                    "DocumentosEmpleadosTTHH",
                    "Index",
                    $"{Mensaje.Aviso}|{Mensaje.SessionCaducada}|{"7000"}"
                );
            }

            var listaDocumentos = new List<DocumentosIngreso>();
            var listaDocumentosEntregados = new List<DocumentosIngresoEmpleado>();

            var documentoingresoViewModel = new ViewModelDocumentoIngresoEmpleado();
            documentoingresoViewModel.ListaDocumentosIngreso = new List<DocumentosIngreso>();
            documentoingresoViewModel.ListaDocumentosIngresoEntregado = new List<DocumentosIngresoEmpleado>();

            try
            {
                int idEmpleado = Convert.ToInt32(id);

                var distributivo = new DistributivoHistorico();

                Response responseDistributivo = await apiServicio.ObtenerElementoAsync1<Response>(
                    new Empleado { IdEmpleado = idEmpleado }, 
                    new Uri(WebApp.BaseAddress),
                    "api/Distributivos/ObtenerDistributivoFormalPorIdEmpleado");


                
                if (!responseDistributivo.IsSuccess) {

                    return this.RedireccionarMensajeTime(
                            "DocumentosEmpleadosTTHH",
                            "Index",
                            $"{Mensaje.Error}|{responseDistributivo.Message}|{"7000"}"
                    );
                }


                distributivo = JsonConvert.DeserializeObject<DistributivoHistorico>(
                        responseDistributivo.Resultado.ToString()
                );
                

                listaDocumentos = await apiServicio.Listar<DocumentosIngreso>(
                    new Uri(WebApp.BaseAddress), 
                    "api/DocumentosIngreso/ListarDocumentosIngreso");


                listaDocumentosEntregados = await apiServicio.ObtenerElementoAsync1<List<DocumentosIngresoEmpleado>>(
                    new Empleado { IdEmpleado = idEmpleado}, 
                    new Uri(WebApp.BaseAddress), 
                    "api/DocumentosIngreso/ListarDocumentosIngresoEmpleado");


                documentoingresoViewModel = new ViewModelDocumentoIngresoEmpleado
                {
                    Distributivo = distributivo,
                    ListaDocumentosIngreso = listaDocumentos.OrderBy(o=>o.IdDocumentosIngreso).ToList(),
                    ListaDocumentosIngresoEntregado = listaDocumentosEntregados.OrderBy(o => o.IdDocumentosIngreso).ToList()

                };

                return View(documentoingresoViewModel);
                

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

            try {

                
                var response = await apiServicio.EditarAsync<Response>(
                    viewModelDocumentoIngresoEmpleado, 
                    new Uri(WebApp.BaseAddress),
                    "api/DocumentosIngreso/ActualizarDocumentosEntregados"
                );

                if (response.IsSuccess) {

                    return this.RedireccionarMensajeTime(
                        "DocumentosEmpleadosTTHH",
                        "CheckListDocumentosEmpleado",
                        new { id = viewModelDocumentoIngresoEmpleado.Distributivo.IdEmpleado },
                        $"{Mensaje.Success}|{response.Message}|{"7000"}"
                    );
                }
                

                return this.RedireccionarMensajeTime(
                    "DocumentosEmpleadosTTHH",
                    "CheckListDocumentosEmpleado",
                    new { id = viewModelDocumentoIngresoEmpleado.Distributivo.IdEmpleado },
                    $"{Mensaje.Error}|{response.Message}|{"7000"}"
                );

            }
            catch (Exception ex) {

                return BadRequest();
            }
            
        }

        #endregion



        // aquí hay que agregar un reporte
        public async Task<IActionResult> LavadoActivos(string id)
        {

            var model = new LavadoActivoEmpleadoViewModel();
            model.DatosBasicosEmpleadoViewModel = new DatosBasicosEmpleadoViewModel();


            var dis = new DistributivoHistorico ();

            try
            {
                var idEmpleado = Convert.ToInt32(id);

                dis.IdEmpleado = idEmpleado;
                
                Response response = await apiServicio.ObtenerElementoAsync1<Response>(
                    new DatosBasicosEmpleadoViewModel{ IdEmpleado = idEmpleado},
                    new Uri(WebApp.BaseAddress),
                    "api/LavadoActivoEmpleados/ExisteLavadoActivosPorIdEmpleado"
                );

                

                if (response.IsSuccess) {

                    this.TempData["MensajeTimer"] = $"{Mensaje.Aviso}|{"Aquí va el reporte"}|{"7000"}";

                    return View(dis);
                }

                
                this.TempData["MensajeTimer"] = $"{Mensaje.Aviso}|{"El empleado aún no ha completado este documento"}|{"7000"}";

                return View(dis);

            }
            catch (Exception ex)
            {
                return View(model);
            }

            
        }
        

    }
}