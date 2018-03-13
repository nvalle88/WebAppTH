using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using Newtonsoft.Json;
using bd.log.guardar.Enumeradores;

namespace bd.webappth.web.Controllers.MVC
{
    public class TiposExamenesComplementariosController : Controller
    {
        
        private readonly IApiServicio apiServicio;


        public TiposExamenesComplementariosController(IApiServicio apiServicio)
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

        public async Task<IActionResult> Create(string mensaje)
        {
            ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Empleado>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "Identificacion");
            
            InicializarMensaje(mensaje);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoExamenComplementario TipoExamenComplementario)
        {
            

            if (!ModelState.IsValid)
            {
                InicializarMensaje(null);

                return View(TipoExamenComplementario);
            }

                Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(TipoExamenComplementario,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TiposExamenesComplementarios/InsertarTiposExamenesComplementarios");
                if (response.IsSuccess)
                {
                    InicializarMensaje(Mensaje.GuardadoSatisfactorio);
                    return RedirectToAction("Index", new { mensaje = Mensaje.GuardadoSatisfactorio });
                }

                ViewData["Error"] = response.Message;

                return View(TipoExamenComplementario);

            }
            catch (Exception ex)
            {

                InicializarMensaje(Mensaje.Error);
                return View(TipoExamenComplementario);


            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/TiposExamenesComplementarios");


                    respuesta.Resultado = JsonConvert.DeserializeObject<TipoExamenComplementario>(respuesta.Resultado.ToString());

                    //ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Empleado>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "Identificacion");


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
        public async Task<IActionResult> Edit(string id, TipoExamenComplementario TipoExamenComplementario)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, TipoExamenComplementario, new Uri(WebApp.BaseAddress),
                                                                 "api/TiposExamenesComplementarios");

                    if (response.IsSuccess)
                    {


                        return RedirectToAction("Index", new { mensaje = Mensaje.GuardadoSatisfactorio });
                    }
                    

                    //ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Empleado>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "Identificacion");

                    ViewData["Error"] = response.Message;
                    return View(TipoExamenComplementario);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.WebAppTh),
                    Message = Mensaje.Excepcion,
                    ExceptionTrace = ex.Message,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "Usuario APP webappth"
                });

                return BadRequest();

            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {

            var lista = new List<TipoExamenComplementario>();
            try
            {
                lista = await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress)
                                                                    , "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios");
                InicializarMensaje(mensaje);

                return View(lista);
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }


        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/TiposExamenesComplementarios");
                if (response.IsSuccess)
                {

                    return RedirectToAction("Index", new { mensaje = Mensaje.BorradoSatisfactorio });
                }
                return RedirectToAction("Index", new { mensaje = Mensaje.BorradoNoSatisfactorio });
            }
            catch (Exception ex)
            {
                

                return BadRequest();
            }
        }
        
    }
}