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


        public async Task<JsonResult> ValidarFormula(string formula)
        {
            try
            {
                Compilador a = new Compilador(apiServicio,constantesNomina);
                try
                {
                    await a.Evaluar(formula);
                    return Json(true);
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
            var vista = new ConceptoNomina { TipoConcepto = "percepcion", Estatus = "Activo", TipoCalculo = "automatico", NivelAcumulacion = "periodo", RegistroEn = "dias" };
            return View(vista);
        }

        public async Task CargarCombox()
        {
            ViewData["IdProceso"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<ProcesoNomina>(new Uri(WebApp.BaseAddress), "api/ProcesoNomina/ListarProcesoNomina"), "IdProceso", "Descripcion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConceptoNomina(ConceptoNomina ConceptoNomina)
        {



            if (!ModelState.IsValid)
            {
                 
                return View(ConceptoNomina);
            }
            Response response = new Response();
            try
            {

                response = await apiServicio.InsertarAsync(ConceptoNomina,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ConceptoNomina/InsertarConceptoNomina");
                if (response.IsSuccess)
                {

                    return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Formula(ConceptoNomina ConceptoNomina)
        {

            if (string.IsNullOrEmpty(ConceptoNomina.FormulaCalculo))
            {
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{Mensaje.FaltaIngresoDatos}" ;
                ModelState.AddModelError("FormulaCalculo", "Debe ingresar la fórmula de cálculo");
                return View(ConceptoNomina);
            }
            Response response = new Response();
            try
            {
                if (ObtenerConceptoNomina().IdConcepto > 0)
                {
                    ConceptoNomina.IdConcepto = ObtenerConceptoNomina().IdConcepto;
                    response = await apiServicio.EditarAsync<Response>(ConceptoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/ConceptoNomina/EditarFormula");

                    if (response.IsSuccess)
                    {

                        return this.Redireccionar($"{Mensaje.Informacion}|{Mensaje.Satisfactorio}", "Formula");
                    }
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{response.Message}";
                    await CargarCombox();
                    return View(ConceptoNomina);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorEditar}");
            }
        }

        public async Task<IActionResult> Formula(int id)
        {
            try
            {
                if (ObtenerConceptoNomina().IdConcepto > 0)
                {
                    var vista = new ConceptoNomina();
                    var ConceptoNomina = new ConceptoNomina { IdConcepto = ObtenerConceptoNomina().IdConcepto };
                    var respuesta = await apiServicio.ObtenerElementoAsync1<Response>(ConceptoNomina, new Uri(WebApp.BaseAddress),
                                                                 "api/ConceptoNomina/ObtenerConceptoNomina");
                    if (respuesta.IsSuccess)
                    {
                        vista = JsonConvert.DeserializeObject<ConceptoNomina>(respuesta.Resultado.ToString());
                    }

                    return View(vista);
                }
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
            }
            catch (Exception)
            {
                return this.Redireccionar($"{Mensaje.Error}|{Mensaje.ErrorCargarDatos}");
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