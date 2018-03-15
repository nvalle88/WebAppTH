using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.ViewModels;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;

namespace bd.webappth.web.Controllers.MVC
{
    public class FichasMedicasController : Controller
    {
        
        private readonly IApiServicio apiServicio;


        public FichasMedicasController(IApiServicio apiServicio)
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


        public async Task<IActionResult> Menu(int idPersona, int idFichaMedica, int idMenu)
        {




            FichaMedica fichaMedica = new FichaMedica();

            fichaMedica.IdPersona = idPersona;
            fichaMedica.IdFichaMedica = idFichaMedica;


            Response response = new Response();

            response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/VerUltimaFichaMedica");


            if (response.IsSuccess)
            {
                fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());


                if (idMenu == 0)
                {
                    return RedirectToAction("Index", "FichasMedicas", fichaMedica);
                }

                else if (idMenu == 1)
                {

                    return RedirectToAction("AntecedentesProfesionales", "FichasMedicas", new { mensaje = "", idFicha = fichaMedica.IdFichaMedica , idPersona = fichaMedica.IdPersona } );
                }
                 
                else if (idMenu == 2)
                {
                    return RedirectToAction("Index", "AntecedentesLaborales", fichaMedica);
                }

                else if (idMenu == 3)
                {
                    return RedirectToAction("Index", "AntecedentesFamiliares", fichaMedica);
                }

                else if (idMenu == 4)
                {
                    return RedirectToAction("Habitos", "FichasMedicas", new { mensaje = "", idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });
                }

                else if (idMenu == 5)
                {
                    return RedirectToAction("DatosMedicos", "FichasMedicas", new { mensaje = "", idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });

                }

                else if (idMenu == 6)
                {
                    return RedirectToAction("Index", "ExamenesComplementarios", fichaMedica);
                }

                else if (idMenu == 7)
                {
                    return RedirectToAction("Index", "FichasMedicas", new { mensaje = "", idFicha = 0, idPersona = 0 });
                }

            }

            if ( idMenu == 0 )
            {
                return RedirectToAction("Index", "FichasMedicas", new { mensaje = "", idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });
            }

            else if (idMenu == 7)
            {
                return RedirectToAction("Index", "FichasMedicas", new { mensaje = "", idFicha = 0, idPersona = 0 });
            }

            else if(idPersona < 1){
                return RedirectToAction("Index", "FichasMedicas", new { mensaje = "Debe seleccionar un paciente, o ingresar una cédula", idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });
            }

            
            return RedirectToAction("Index", "FichasMedicas", new { mensaje = response.Message, idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });
        }







        public async Task<IActionResult> Index(string mensaje, FichaMedica fichaMedica, int idPersona)
        {
            
            InicializarMensaje(mensaje);
            
            

            try
            {

                var fmvm = new FichaMedicaViewModel();

                fmvm.FichasMedicas = new List<FichaMedica>();
                fmvm.DatosBasicosPersonaViewModel = new DatosBasicosPersonaViewModel();
                fmvm.DatosBasicosPersonaViewModel.IdPersona = idPersona;

                var lista = new List<Persona>();

                lista = await apiServicio.Listar<Persona>(new Uri(WebApp.BaseAddress)
                                                                    , "api/FichasMedicas/ListarPersonasFichaMedica");


                fmvm.ListaPersonas = lista;



                fmvm.DatosBasicosPersonaViewModel.IdPersona = fichaMedica.IdPersona;



                if (fichaMedica.IdPersona > 0)
                {

                    Response response = new Response();

                    response = await apiServicio.ObtenerElementoAsync1<Response>(fmvm,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/ListarFichaMedicaViewModel");



                    if (response.IsSuccess)
                    {
                        var fms = JsonConvert.DeserializeObject<FichaMedicaViewModel>(response.Resultado.ToString());


                        return View(fms);
                    }
                }

                //fmvm.ListaPersonas = new List<Persona>();
                


                return View(fmvm);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(FichaMedicaViewModel fichaMedicaViewModel)
        {
            InicializarMensaje("");

            Response response = new Response();
            try
            {


                response = await apiServicio.ObtenerElementoAsync1<Response>(fichaMedicaViewModel,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/ListarFichaMedicaViewModel");



                if (response.IsSuccess)
                    {
                        var fms = JsonConvert.DeserializeObject<FichaMedicaViewModel>(response.Resultado.ToString());
                    

                        return View("Index",fms);

                }


                ViewData["Error"] = response.Message;


                DatosBasicosPersonaViewModel dbvm = new DatosBasicosPersonaViewModel();

                dbvm.IdPersona = 0;

                FichaMedicaViewModel fmvm2 = new FichaMedicaViewModel();
                List<FichaMedica> listafm = new List<FichaMedica>();
                List<Persona> lista = new List<Persona>();

                lista = await apiServicio.Listar<Persona>(new Uri(WebApp.BaseAddress)
                                                                    , "api/FichasMedicas/ListarPersonasFichaMedica");


                fmvm2.ListaPersonas = lista;

                fmvm2.DatosBasicosPersonaViewModel = dbvm;
                fmvm2.FichasMedicas = listafm;
                
                return View("Index", fmvm2);


            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "FichasMedicas", new { mensaje = response.Message, idFicha = 0, idPersona = 0 });
                //return BadRequest();ertretre

            }
        }


        public async Task<IActionResult> SeleccionPaciente(string id)
        {

            FichaMedicaViewModel fmvm = new FichaMedicaViewModel();
            DatosBasicosPersonaViewModel dbp= new DatosBasicosPersonaViewModel();
            List<FichaMedica> listaF = new List<FichaMedica>();
            List<Persona> listaP = new List<Persona>();

            fmvm.DatosBasicosPersonaViewModel = dbp;
            fmvm.ListaPersonas = listaP;
            fmvm.FichasMedicas = listaF;

            fmvm.DatosBasicosPersonaViewModel.Identificacion = id;
            fmvm.DatosBasicosPersonaViewModel.IdPersona = 0;

            return await Index(fmvm);
        }




        public async Task<IActionResult> Create(string mensaje, string id)
        {

            Response response = new Response();

            InicializarMensaje(mensaje);

            var fichaMedica = new FichaMedica();
            int id2 = Convert.ToInt32(id);


            fichaMedica.IdPersona = id2;



            try
            {

                response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/FichasMedicas/VerUltimaFichaMedica");


                if (response.IsSuccess)
                {
                    fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());
                    

                    // aqui!!!!
                    
                    return RedirectToAction("Index", "FichasMedicas", new { mensaje = "Ya existe una ficha en edición, no se ha creado una nueva ficha", idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });
                }
                else
                {

                    DateTime hoy = DateTime.Today;


                    fichaMedica.FechaFichaMedica = hoy;
                    fichaMedica.Estado = 0;

                    fichaMedica.Diagnostico = "Sin diagnóstico";



                    if (!ModelState.IsValid)
                    {
                        InicializarMensaje(null);

                        return View(fichaMedica);
                    }

                    response = await apiServicio.InsertarAsync(fichaMedica,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/FichasMedicas/InsertarFichasMedicas");


                    if (response.IsSuccess)
                    {


                        response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/FichasMedicas/VerUltimaFichaMedica");


                        if (response.IsSuccess)
                        {
                            fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());

                            return RedirectToAction("AntecedentesProfesionales", "FichasMedicas", fichaMedica);

                        }


                    }


                }




            }
            catch (Exception ex1) {


            }

            return View();
            
            
        }
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FichaMedica fichaMedica)
        {

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);

                return View(fichaMedica);
            }

                Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(fichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/InsertarFichasMedicas");
                if (response.IsSuccess)
                {

                    return RedirectToAction("Index");
                }

                ViewData["Error"] = response.Message;

                return View(fichaMedica);

            }
            catch (Exception ex)
            {
                
                InicializarMensaje(Mensaje.Error);

                return View(fichaMedica);


            }
        }


        public async Task<IActionResult> Habitos(string mensaje, int idFicha ,int idPersona)
        {


            FichaMedica fichaMedica = new FichaMedica();
            fichaMedica.IdPersona = idPersona;


            InicializarMensaje(mensaje);

            
            Response response = new Response();
                
            response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/VerUltimaFichaMedica");


            if (response.IsSuccess)
            {
                fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());

            }else{

                mensaje = "Habitos Index mnes,ficha nodata";
            }

            
            return View(fichaMedica);
            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Habitos(FichaMedica fichaMedica)
        {

            Response response = new Response();

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);

                return View(fichaMedica);
            }


            response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                            new Uri(WebApp.BaseAddress),
                                                            "api/FichasMedicas/VerUltimaFichaMedica");
            if (response.IsSuccess)
            {
                var fichaMedica2 = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());

                fichaMedica2.AntecedenteMedico = fichaMedica.AntecedenteMedico;
                fichaMedica2.AntecedenteQuirurgico = fichaMedica.AntecedenteQuirurgico;
                fichaMedica2.Alergias = fichaMedica.Alergias;
                fichaMedica2.Vacunas = fichaMedica.Vacunas;
                fichaMedica2.UsoMedicinaDiaria = fichaMedica.UsoMedicinaDiaria;
                fichaMedica2.FechaUltimaDosis = fichaMedica.FechaUltimaDosis;

                fichaMedica2.PrimeraMenstruacion = fichaMedica.PrimeraMenstruacion;
                fichaMedica2.UltimaMenstruacion = fichaMedica.UltimaMenstruacion;
                fichaMedica2.CicloMenstrual = fichaMedica.CicloMenstrual;
                fichaMedica2.Gestas = fichaMedica.Gestas;
                fichaMedica2.Partos = fichaMedica.Partos;
                fichaMedica2.Cesarias = fichaMedica.Cesarias;
                fichaMedica2.Abortos = fichaMedica.Abortos;
                fichaMedica2.HijosVivos = fichaMedica.HijosVivos;
                fichaMedica2.UltimoPapTest = fichaMedica.UltimoPapTest;
                fichaMedica2.UltimaMamografia = fichaMedica.UltimaMamografia;
                fichaMedica2.Anticoncepcion = fichaMedica.Anticoncepcion;
                
                fichaMedica2.Cigarrillo = fichaMedica.Cigarrillo;
                fichaMedica2.FrecuenciaCigarrillo = fichaMedica.FrecuenciaCigarrillo;
                fichaMedica2.CigarrilloDesde = fichaMedica.CigarrilloDesde;
                fichaMedica2.CigarrilloHasta = fichaMedica.CigarrilloHasta;

                fichaMedica2.Licor = fichaMedica.Licor;
                fichaMedica2.LicorFrecuencia = fichaMedica.LicorFrecuencia;
                fichaMedica2.LicorDesde = fichaMedica.LicorDesde;
                fichaMedica2.LicorHasta = fichaMedica.LicorHasta;

                fichaMedica2.Drogas = fichaMedica.Drogas;
                fichaMedica2.FrecuenciaDrogas = fichaMedica.FrecuenciaDrogas;
                fichaMedica2.DrogasDesde = fichaMedica.DrogasDesde;
                fichaMedica2.DrogasHasta = fichaMedica.DrogasHasta;

                fichaMedica2.Ejercicios = fichaMedica.Ejercicios;
                fichaMedica2.EjerciciosFrecuencia = fichaMedica.EjerciciosFrecuencia;
                fichaMedica2.EjerciciosTipo = fichaMedica.EjerciciosTipo;

                fichaMedica2.HabitosObservaciones = fichaMedica.HabitosObservaciones;


                response = await apiServicio.EditarAsync(fichaMedica.IdFichaMedica + "", fichaMedica2, new Uri(WebApp.BaseAddress),
                                                                    "api/FichasMedicas");

                if (response.IsSuccess)
                {
                    fichaMedica = fichaMedica2;
                    
                    return RedirectToAction("Habitos", "FichasMedicas", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });
                }

            }


            return View();

        }


        public async Task<IActionResult> AntecedentesProfesionales(string mensaje, int idFicha ,int idPersona)
        {


            FichaMedica fichaMedica = new FichaMedica();
            fichaMedica.IdPersona = idPersona;


            InicializarMensaje(mensaje);

            
            Response response = new Response();
                
            response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/VerUltimaFichaMedica");


            if (response.IsSuccess)
            {
                fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());

            }else{

                mensaje = "AntecedentesProfesionales Index mnes,ficha nodata";
            }

            
            return View(fichaMedica);
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AntecedentesProfesionales(FichaMedica fichaMedica)
        {

            Response response = new Response();

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);

                return View(fichaMedica);
            }

            
             response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/VerUltimaFichaMedica");
             if (response.IsSuccess)
             {
                var fichaMedica2 = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());

                fichaMedica2.AccidenteTrabajo = fichaMedica.AccidenteTrabajo;
                fichaMedica2.FechaAccidente = fichaMedica.FechaAccidente;
                fichaMedica2.EmpresaAccidente = fichaMedica.EmpresaAccidente;

                fichaMedica2.EnfermedadProfesional = fichaMedica.EnfermedadProfesional;
                fichaMedica2.FechaDiagnostico = fichaMedica.FechaDiagnostico;
                fichaMedica2.EmpresaEnfermedad = fichaMedica.EmpresaEnfermedad;
                fichaMedica2.DetalleAccidenteEnfermedadOcupacional = fichaMedica.DetalleAccidenteEnfermedadOcupacional;


                response = await apiServicio.EditarAsync(fichaMedica.IdFichaMedica + "", fichaMedica2, new Uri(WebApp.BaseAddress),
                                                                    "api/FichasMedicas");

                if (response.IsSuccess)
                {
                    fichaMedica = fichaMedica2;

                    return RedirectToAction("AntecedentesProfesionales", "FichasMedicas", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });
                }

             }
             
             
            return View();   
            
        }




        public async Task<IActionResult> DatosMedicos(string mensaje, int idFicha, int idPersona)
        {


            FichaMedica fichaMedica = new FichaMedica();
            fichaMedica.IdPersona = idPersona;


            InicializarMensaje(mensaje);


            Response response = new Response();

            response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/VerUltimaFichaMedica");


            if (response.IsSuccess)
            {
                fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());

            }
            else
            {

                mensaje = "AntecedentesProfesionales Index mnes,ficha nodata";
            }


            return View(fichaMedica);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatosMedicos(FichaMedica fichaMedica)
        {

            Response response = new Response();

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);

                return View(fichaMedica);
            }


            response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                            new Uri(WebApp.BaseAddress),
                                                            "api/FichasMedicas/VerUltimaFichaMedica");
            if (response.IsSuccess)
            {
                var fichaMedica2 = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());

                fichaMedica2.TensionArterial = fichaMedica.TensionArterial;
                fichaMedica2.FrecuenciaCardiaca = fichaMedica.FrecuenciaCardiaca;
                fichaMedica2.FrecuenciaRespiratoria = fichaMedica.FrecuenciaRespiratoria;
                fichaMedica2.Talla = fichaMedica.Talla;
                fichaMedica2.Peso = fichaMedica.Peso;
                fichaMedica2.LateralidadDominante = fichaMedica.LateralidadDominante;
                fichaMedica2.Interpretacion = fichaMedica.Interpretacion;

                fichaMedica2.Cabeza = fichaMedica.Cabeza;
                fichaMedica2.CabezaHallazgos = fichaMedica.CabezaHallazgos;

                fichaMedica2.Ojos = fichaMedica.Ojos;
                fichaMedica2.OjosHallazgos = fichaMedica.OjosHallazgos;

                fichaMedica2.Oidos = fichaMedica.Oidos;
                fichaMedica2.OidosHallazgos = fichaMedica.OidosHallazgos;

                fichaMedica2.Nariz = fichaMedica.Nariz;
                fichaMedica2.NarizHallazgos = fichaMedica.NarizHallazgos;

                fichaMedica2.Boca = fichaMedica.Boca;
                fichaMedica2.BocaHallazgos = fichaMedica.BocaHallazgos;

                fichaMedica2.FaringeAmigdalas = fichaMedica.FaringeAmigdalas;
                fichaMedica2.FaringeAmigdalasHallazgos = fichaMedica.FaringeAmigdalasHallazgos;

                fichaMedica2.Cuello = fichaMedica.Cuello;
                fichaMedica2.CuelloHallazgos = fichaMedica.CuelloHallazgos;

                fichaMedica2.Corazon = fichaMedica.Corazon;
                fichaMedica2.CorazonHallazgos = fichaMedica.CorazonHallazgos;

                fichaMedica2.Pulmones = fichaMedica.Pulmones;
                fichaMedica2.PulmonesHallazgos = fichaMedica.PulmonesHallazgos;

                fichaMedica2.Abdomen = fichaMedica.Abdomen;
                fichaMedica2.AbdomenHallazgos = fichaMedica.AbdomenHallazgos;

                fichaMedica2.Hernias = fichaMedica.Hernias;
                fichaMedica2.HerniasHallazgos = fichaMedica.HerniasHallazgos;

                fichaMedica2.Genitales = fichaMedica.Genitales;
                fichaMedica2.GenitalesHallazgos = fichaMedica.GenitalesHallazgos;

                fichaMedica2.ExtremidadesSuperiores = fichaMedica.ExtremidadesSuperiores;
                fichaMedica2.ExtremidadesSuperioresHallazgos = fichaMedica.ExtremidadesSuperioresHallazgos;

                fichaMedica2.ExtremidadesInferiores = fichaMedica.ExtremidadesInferiores;
                fichaMedica2.ExtremidadesInferioresHallazgos = fichaMedica.ExtremidadesInferioresHallazgos;

                fichaMedica2.Varices = fichaMedica.Varices;
                fichaMedica2.VaricesHallazgos = fichaMedica.VaricesHallazgos;

                fichaMedica2.SistemaNerviosoCentral = fichaMedica.SistemaNerviosoCentral;
                fichaMedica2.SistemaNerviosoHallazgos = fichaMedica.SistemaNerviosoHallazgos;

                fichaMedica2.Piel = fichaMedica.Piel;
                fichaMedica2.PielHallazgos = fichaMedica.PielHallazgos;
                
                fichaMedica2.SospechaEnfermedadLaboral = fichaMedica.SospechaEnfermedadLaboral;
                fichaMedica2.DetalleEnfermedad = fichaMedica.DetalleEnfermedad;

                fichaMedica2.AptoCargo = fichaMedica.AptoCargo;

                fichaMedica2.Diagnostico = fichaMedica.Diagnostico;

                fichaMedica2.Recomendaciones = fichaMedica.Recomendaciones;

                fichaMedica2.Estado = fichaMedica.Estado;


                response = await apiServicio.EditarAsync(fichaMedica.IdFichaMedica + "", fichaMedica2, new Uri(WebApp.BaseAddress),
                                                                    "api/FichasMedicas");

                if (response.IsSuccess)
                {
                    fichaMedica = fichaMedica2;

                    return RedirectToAction("Index", "FichasMedicas", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });
                }

            }


            return RedirectToAction("Index", "FichasMedicas", new { mensaje = response.Message, idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });

        }














        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/FichasMedicas");


                    respuesta.Resultado = JsonConvert.DeserializeObject<FichaMedica>(respuesta.Resultado.ToString());
                    

                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        return View(respuesta.Resultado);
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
        public async Task<IActionResult> Edit(string id, FichaMedica FichaMedica)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, FichaMedica, new Uri(WebApp.BaseAddress),
                                                                 "api/FichasMedicas");

                    if (response.IsSuccess)
                    {
                        
                        return RedirectToAction("Index");
                    }
                    
                    ViewData["Error"] = response.Message;
                    return View(FichaMedica);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest();

            }
        }


        public async Task<IActionResult> CambiarEstado(string id, int idFM, int idPer)
        {
            
            Response response = new Response();

            FichaMedica fichaMedica2 = new FichaMedica();

            fichaMedica2.IdFichaMedica = idFM;
            fichaMedica2.IdPersona = idPer;

            response = await apiServicio.InsertarAsync<Response>(fichaMedica2,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/FichasMedicas/VerUltimaFichaMedica");


            if (response.IsSuccess)
            {
                fichaMedica2 = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());
               
                return RedirectToAction("Index", "FichasMedicas", new { mensaje = Mensaje.ErrorFichaEdicion, idFicha = fichaMedica2.IdFichaMedica, idPersona = fichaMedica2.IdPersona });
            }
            else
            {
                response = await apiServicio.InsertarAsync<Response>(idFM,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/ObtenerFichaPorId");

                if (response.IsSuccess)
                {
                    FichaMedica fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());


                    fichaMedica.Estado = 0;


                    response = await apiServicio.EditarAsync(fichaMedica.IdFichaMedica + "", fichaMedica, new Uri(WebApp.BaseAddress),
                                                                        "api/FichasMedicas");

                    if (response.IsSuccess)
                    {
                        
                        return RedirectToAction("DatosMedicos", "FichasMedicas", new { mensaje = Mensaje.Satisfactorio, idFicha = fichaMedica.IdFichaMedica, idPersona = fichaMedica.IdPersona });

                    }


                    return RedirectToAction("Index", "FichasMedicas", new { mensaje = "No se ha podido cambiar el estado", idFicha = idFM, idPersona = idPer });
                }
            }
            
            return RedirectToAction("Index", "FichasMedicas", new { mensaje = "Error al cambiar estado de la ficha", idFicha = idFM, idPersona = idPer });

        }


        public async Task<IActionResult> Delete(string id, int idFM ,int idPer)
        {
            

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/FichasMedicas");
                if (response.IsSuccess)
                {

                    return RedirectToAction("Index", "FichasMedicas", new { mensaje = Mensaje.BorradoSatisfactorio, idFicha = idFM, idPersona = idPer }); ;

                }

                return RedirectToAction("Index", "FichasMedicas", new { mensaje = response.Message + ", (borre los antecedentes laborales, familiares y exámenes complementarios asignados a esta ficha) ", idFicha = idFM, idPersona = idPer });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        
        
    }
}