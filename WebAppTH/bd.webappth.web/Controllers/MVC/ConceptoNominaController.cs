using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Constantes;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.webappth.servicios.Extensores;
using bd.webappth.servicios.Interfaces;
using bd.webappth.servicios.Nomina;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace bd.webappth.web.Controllers.MVC
{
    public class ConceptoNominaController : Controller
    {
        private readonly IApiServicio apiServicio;
        private readonly IConstantesNomina constantesNomina;

        public ConceptoNominaController(IApiServicio apiServicio, IConstantesNomina constantesNomina)
        {
            this.apiServicio = apiServicio;
            this.constantesNomina = constantesNomina;

        }
        public async Task<bool> ValidarFormulaGuardar(string formula)
        {
            try
            {
                Compilador a = new Compilador(apiServicio, constantesNomina);
                try
                {
                    var b = await a.Evaluar(formula);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<JsonResult> ActualizarEstado(string valorCombo,int idConcepto)
        {
            try
            {
                try
                {
                    var respuesta = await apiServicio.EditarAsync<Response>(new ConceptoNomina { Estatus = valorCombo, IdConcepto = idConcepto }, new Uri(WebApp.BaseAddress)
                                                                      , "api/ConceptoNomina/EditarEstado");

                    if (respuesta.IsSuccess)
                    {
                        return Json(true);
                    }
                    return Json(false);
                }
                catch (Exception)
                {
                    return Json(false);
                }
               
            }
            catch (Exception)
            {
                return Json(false);
            }
        }


        public async Task<JsonResult> ExisteFormula(int idRegimenLaboral, int idConcepto)
        {
            try
            {
                try
                {
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(new FormulaNomina{IdRegimenLaboral = idRegimenLaboral,IdConceptoNomina = idConcepto}, new Uri(WebApp.BaseAddress),"api/ConceptoNomina/ExisteFormula");

                    if (respuesta.IsSuccess)
                    {
                        return Json(true);
                    }
                    return Json(false);
                }
                catch (Exception)
                {
                    return Json(false);
                }

            }
            catch (Exception)
            {
                return Json(false);
            }
        }




        public async Task<IActionResult> CreateConceptoConjunto(string mensaje)
        {
            if (ObtenerConceptoNomina().IdConcepto>0)
            {

           
             
            await CargarComboxConceptoConjunto();
            var vista = new ConceptoConjuntoNomina { Suma = true, Resta = false };
            return View(vista);
            }
            return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
        }

        public async Task CargarComboxConceptoConjunto()
        {
            ViewData["IdConjunto"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ConjuntoNomina>(new Uri(WebApp.BaseAddress), "api/ConjuntoNomina/ListarConjuntoNomina"), "IdConjunto", "Descripcion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConceptoConjunto(ConceptoConjuntoNomina ConceptoConjuntoNomina)
        {
            if (!ModelState.IsValid)
            {
                 
                return View(ConceptoConjuntoNomina);
            }
            Response response = new Response();
            try
            {
                if (ObtenerConceptoNomina().IdConcepto > 0)
                {
                    ConceptoConjuntoNomina.IdConcepto = ObtenerConceptoNomina().IdConcepto;
                    response = await apiServicio.InsertarAsync(ConceptoConjuntoNomina,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/ConceptoConjuntoNomina/InsertarConceptoConjuntoNomina");
                    if (response.IsSuccess)
                    {
                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}", "IndexConceptoConjunto");
                    }

                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    await CargarComboxConceptoConjunto();
                    return View(ConceptoConjuntoNomina);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");

            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> EditConceptoConjunto(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var ConceptoConjuntoNomina = new ConceptoConjuntoNomina { IdConceptoConjunto = Convert.ToInt32(id) };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(ConceptoConjuntoNomina, new Uri(WebApp.BaseAddress),
                                                                  "api/ConceptoConjuntoNomina/ObtenerConceptoConjuntoNomina");
                    if (respuesta.IsSuccess)
                    {
                         
                        var vista = JsonConvert.DeserializeObject<ConceptoConjuntoNomina>(respuesta.Resultado.ToString());
                        await CargarComboxConceptoConjunto();
                        return View(vista);
                    }
                }

                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConceptoConjunto(ConceptoConjuntoNomina ConceptoConjuntoNomina)
        {

            if (!ModelState.IsValid)
            {
                 
                return View(ConceptoConjuntoNomina);
            }
            Response response = new Response();
            try
            {
                if (ObtenerConceptoNomina().IdConcepto > 0)
                {
                    ConceptoConjuntoNomina.IdConcepto = ObtenerConceptoNomina().IdConcepto;
                    response = await apiServicio.EditarAsync<Response>(ConceptoConjuntoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/ConceptoConjuntoNomina/EditarConceptoConjuntoNomina");

                    if (response.IsSuccess)
                    {

                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}","IndexConceptoConjunto");
                    }
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    await CargarComboxConceptoConjunto();
                    return View(ConceptoConjuntoNomina);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> IndexConceptoConjunto(string mensaje)
        {
            try
            {
                if (ObtenerConceptoNomina().IdConcepto > 0)
                {
                     
                    var concepto = new ConceptoNomina { IdConcepto = ObtenerConceptoNomina().IdConcepto };
                    var lista = await apiServicio.Listar<ConceptoConjuntoNomina>(concepto, new Uri(WebApp.BaseAddress)
                                                                         , "api/ConceptoConjuntoNomina/ListarConceptoConjuntoNomina");
                    return View(lista);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> DeleteConceptoConjunto(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.Redireccionar($"{Mensaje.Error}|{Mensaje.RegistroNoExiste}");
                }
                var tipoConjuntoEliminar = new ConceptoConjuntoNomina { IdConceptoConjunto = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/ConceptoConjuntoNomina/EliminarConceptoConjuntoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}","IndexConceptoConjunto");
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

        public async Task<IActionResult> CreateConceptoNomina(string mensaje)
        {
             
            await CargarCombox();
            var vista = new ConceptoNomina { Estatus = "Activo"};
            return View(vista);
        }

        public async Task CargarCombox()
        {
            ViewData["IdProceso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ProcesoNomina>(new Uri(WebApp.BaseAddress), "api/ProcesoNomina/ListarProcesoNomina"), "IdProceso", "Descripcion");
            ViewData["IdTipoConcepto"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoConceptoNomina>(new Uri(WebApp.BaseAddress), "api/ConceptoNomina/ListarTipoConcepto"), "IdTipoConcepto", "Descripcion");
        }


        public async Task<IActionResult> FormulaNomina(int IdConcepto)
        {

            if (HttpContext.Session.GetInt32(Constantes.idConceptoNominaSession) != IdConcepto)
            {
                HttpContext.Session.SetInt32(Constantes.idConceptoNominaSession, IdConcepto);
            }

            var Respuesta = await apiServicio.ObtenerElementoAsync1<Response>(new ConceptoNomina { IdConcepto = ObtenerConceptoNomina().IdConcepto }, new Uri(WebApp.BaseAddress)
                                                                    , "api/ConceptoNomina/ObtenerConceptoNomina");

            if (Respuesta.IsSuccess)
            {
                
                ViewData["IdRegimeLaboral"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<RegimenLaboral>(new Uri(WebApp.BaseAddress), "api/RegimenesLaborales/ListarRegimenesLaborales"), "IdRegimenLaboral", "Nombre");
                return View(JsonConvert.DeserializeObject<ConceptoNomina>(Respuesta.Resultado.ToString()));
            }


            //  var procesoNomina = new ProcesoNomina { Codigo = "123", IdProceso = 1, Descripcion = "Primera Quincena" };
            //  var procesoNomina1 = new ProcesoNomina { Codigo = "456", IdProceso = 2, Descripcion = "Segunda Quincena" };
            //  var procesoNomina2 = new ProcesoNomina { Codigo = "789", IdProceso = 3, Descripcion = "Otros" };

            //  var FormulaNomina = new FormulaNomina { Formula = "A+B*C", IdConceptoNomina = 1, IdRegimenLaboral = 1, RegimenLaboral = new RegimenLaboral { Nombre = "LSEP" } };
            //  var FormulaNomina1 = new FormulaNomina { Formula = "H+G*H", IdConceptoNomina = 1, IdRegimenLaboral = 2, RegimenLaboral = new RegimenLaboral { Nombre = "CT" } };

            //  var conceptoProceso = new ConceptoProcesoNomina { IdProcesoNomina = 1, IdConceptoNomina = 1, ProcesoNomina = procesoNomina };
            //  var conceptoProceso_1 = new ConceptoProcesoNomina { IdProcesoNomina = 2, IdConceptoNomina = 1, ProcesoNomina = procesoNomina1 };

            //  var listaTodosProcesos = new List<ProcesoNomina>();
            //  listaTodosProcesos.Add(procesoNomina);
            //  listaTodosProcesos.Add(procesoNomina1);
            //  listaTodosProcesos.Add(procesoNomina2);

            //  var listaConceptoProceso = new List<ConceptoProcesoNomina>();
            //  var listaConceptoProceso1 = new List<ConceptoProcesoNomina>();
            //  listaConceptoProceso.Add(conceptoProceso);
            //  listaConceptoProceso.Add(conceptoProceso_1);
            //  listaConceptoProceso1.Add(conceptoProceso_1);

            //  var listaFormula = new List<FormulaNomina>();
            //  var listaFormula1 = new List<FormulaNomina>();
            //  listaFormula.Add(FormulaNomina);
            //  listaFormula1.Add(FormulaNomina1);

            //  listaFormula.Add(FormulaNomina1);
            //  var tipoConcepto = new TipoConceptoNomina { IdTipoConcepto = 1, Descripcion = "Tipo 1", Signo = 1 };
            //  var conceptonomina = new ConceptoNomina { IdConcepto = 1, Codigo = "123", Descripcion = "Descripcion1", Estatus = "Activo", FormulaNomina = listaFormula, ConceptoProcesoNomina = listaConceptoProceso, ListaTodosProcesos = listaTodosProcesos ,TipoConceptoNomina=tipoConcepto};
            ////  var conceptonomina1 = new ConceptoNomina { IdConcepto = 2, Codigo = "123", Descripcion = "Descripcion1", Estatus = "Activo", FormulaNomina = listaFormula1, ConceptoProcesoNomina = listaConceptoProceso1};

            return View();

           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConceptoNomina(ConceptoNomina ConceptoNomina)
        {
            try
            {
                for (int i = 0; i < ConceptoNomina.ProcesosSeleccionados.Length ; i++)
                {
                    var conceptoProceso = new ConceptoProcesoNomina { IdProcesoNomina = ConceptoNomina.ProcesosSeleccionados[i] };
                    ConceptoNomina.ConceptoProcesoNomina.Add(conceptoProceso);
                };
              var  response = await apiServicio.InsertarAsync<Response>(ConceptoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ConceptoNomina/InsertarConceptoNomina");

                if (response.IsSuccess)
                {
                    var respuestaConceptoNomina = JsonConvert.DeserializeObject<ConceptoNomina>(response.Resultado.ToString());
                    return this.Redireccionar("ConceptoNomina","FormulaNomina",new { respuestaConceptoNomina.IdConcepto, },$"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
                }
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                await CargarCombox();
                return View(ConceptoNomina);
            }
            catch (Exception)
            {
                return this.Redireccionar("Create", $"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        public async Task<IActionResult> DetalleConceptoNomina(int id)
        {
            try
            {
                var entrada = false;
                var vista = new ConceptoNomina();
                var respuesta = new Response();

                if (HttpContext.Session.GetInt32(Constantes.idConceptoNominaSession) != id)
                {
                    HttpContext.Session.SetInt32(Constantes.idConceptoNominaSession, id);
                    var conceptoNominaSession = new ConceptoNomina { IdConcepto = id };
                    respuesta= await apiServicio.ObtenerElementoAsync1<Response>(conceptoNominaSession, new Uri(WebApp.BaseAddress),
                                                            "api/ConceptoNomina/ObtenerConceptoNomina");
                    if (respuesta.IsSuccess)
                    {
                        vista = JsonConvert.DeserializeObject<ConceptoNomina>(respuesta.Resultado.ToString());
                    }
                    HttpContext.Session.SetString(Constantes.CodigoConceptoNominaSession, vista.Codigo);
                    HttpContext.Session.SetString(Constantes.DescripcionConceptoNominaSession, vista.Descripcion);

                    entrada = true;
                }

                if (!entrada)
                {
                    var ConceptoNomina = new ConceptoNomina { IdConcepto = ObtenerConceptoNomina().IdConcepto };
                     respuesta = await apiServicio.ObtenerElementoAsync1<Response>(ConceptoNomina, new Uri(WebApp.BaseAddress),
                                                                  "api/ConceptoNomina/ObtenerConceptoNomina");
                    if (respuesta.IsSuccess)
                    {
                        vista = JsonConvert.DeserializeObject<ConceptoNomina>(respuesta.Resultado.ToString());
                    }
                }

                await CargarCombox();
                return View(vista);


            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetalleConceptoNomina(ConceptoNomina ConceptoNomina)
        {

            if (!ModelState.IsValid)
            {
                 
                return View(ConceptoNomina);
            }
            Response response = new Response();
            try
            {
               
                    ConceptoNomina.IdConcepto = ObtenerConceptoNomina().IdConcepto;
                    response = await apiServicio.EditarAsync<Response>(ConceptoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/ConceptoNomina/EditarConceptoNomina");

                    if (response.IsSuccess)
                    {
                     var vista = JsonConvert.DeserializeObject<ConceptoNomina>(response.Resultado.ToString());
                    HttpContext.Session.SetString(Constantes.CodigoConceptoNominaSession, vista.Codigo);
                    HttpContext.Session.SetString(Constantes.DescripcionConceptoNominaSession, vista.Descripcion);
                    return this.Redireccionar( $"{Mensaje.Informacion}|{Mensaje.Satisfactorio}", "DetalleConceptoNomina");
                    }
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                await CargarCombox();
                    return View(ConceptoNomina);
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEditar}");
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {
            try
            {

                var lista = await apiServicio.Listar<ConceptoNomina>(new Uri(WebApp.BaseAddress)
                                                                     , "api/ConceptoNomina/ListarConceptoNomina");

                lista.FirstOrDefault().ListaTodosProcesos = new List<ProcesoNomina>();
                lista.FirstOrDefault().ListaTodosProcesos= await apiServicio.Listar<ProcesoNomina>(new Uri(WebApp.BaseAddress), "api/ProcesoNomina/ListarProcesoNomina");

                //var procesoNomina = new ProcesoNomina { Codigo = "123", IdProceso = 1, Descripcion = "Primera Quincena" };
                //var procesoNomina1 = new ProcesoNomina { Codigo = "456", IdProceso = 2, Descripcion = "Segunda Quincena" };
                //var procesoNomina2 = new ProcesoNomina { Codigo = "789", IdProceso = 3, Descripcion = "Otros" };

                //var FormulaNomina = new FormulaNomina { Formula = "A+B*C", IdConceptoNomina = 1, IdRegimenLaboral = 1, RegimenLaboral = new RegimenLaboral { Nombre = "LSEP" } };
                //var FormulaNomina1 = new FormulaNomina { Formula = "H+G*H", IdConceptoNomina = 1, IdRegimenLaboral = 2, RegimenLaboral = new RegimenLaboral { Nombre = "CT" } };

                //var conceptoProceso = new ConceptoProcesoNomina { IdProcesoNomina = 1, IdConceptoNomina = 1, ProcesoNomina=procesoNomina };
                //var conceptoProceso_1 = new ConceptoProcesoNomina { IdProcesoNomina = 2, IdConceptoNomina = 1, ProcesoNomina =procesoNomina1 };

                //var listaTodosProcesos = new List<ProcesoNomina>();
                //listaTodosProcesos.Add(procesoNomina);
                //listaTodosProcesos.Add(procesoNomina1);
                //listaTodosProcesos.Add(procesoNomina2);

                //var listaConceptoProceso = new List<ConceptoProcesoNomina>();
                //var listaConceptoProceso1 = new List<ConceptoProcesoNomina>();
                //listaConceptoProceso.Add(conceptoProceso);
                //listaConceptoProceso.Add(conceptoProceso_1);
                //listaConceptoProceso1.Add(conceptoProceso_1);

                //var listaFormula = new List<FormulaNomina>();
                //var listaFormula1 = new List<FormulaNomina>();
                //listaFormula.Add(FormulaNomina);
                //listaFormula1.Add(FormulaNomina1);

                //listaFormula.Add(FormulaNomina1);

                //var conceptonomina = new ConceptoNomina { IdConcepto = 1, Codigo = "123", Descripcion = "Descripcion1", Estatus = "Activo", FormulaNomina = listaFormula, ConceptoProcesoNomina=listaConceptoProceso,ListaTodosProcesos=listaTodosProcesos};
                //var conceptonomina1 = new ConceptoNomina { IdConcepto = 2, Codigo = "123", Descripcion = "Descripcion1", Estatus = "Activo", FormulaNomina = listaFormula1, ConceptoProcesoNomina = listaConceptoProceso1};
                //var lista = new List<ConceptoNomina>();

                //lista.Add(conceptonomina);
                //lista.Add(conceptonomina1);
                return View(lista);
            }
            catch (Exception ex)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorListado}");
            }
        }

        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Index");
                }
                var tipoConjuntoEliminar = new ConceptoNomina { IdConcepto = Convert.ToInt32(id) };

                var response = await apiServicio.EliminarAsync(tipoConjuntoEliminar, new Uri(WebApp.BaseAddress)
                                                               , "api/ConceptoNomina/EliminarConceptoNomina");
                if (response.IsSuccess)
                {
                    return this.Redireccionar($"{Mensaje.Satisfactorio}|{Mensaje.Satisfactorio}");
                }
                return this.Redireccionar($"{Mensaje.Satisfactorio}|{Mensaje.BorradoNoSatisfactorio}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEliminar}");
            }
        }

        public ConceptoNomina ObtenerConceptoNomina()
        {
            var concepto = new ConceptoNomina
            {
                IdConcepto = Convert.ToInt32(HttpContext.Session.GetInt32(Constantes.idConceptoNominaSession)),
                Descripcion = Convert.ToString(HttpContext.Session.GetInt32(Constantes.DescripcionConceptoNominaSession)),
                Codigo = Convert.ToString(HttpContext.Session.GetInt32(Constantes.CodigoConceptoNominaSession)),
            };
            return concepto;
        }

    }
}