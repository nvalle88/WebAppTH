using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;

namespace bd.webappth.web.Controllers.MVC
{
    public class TiposAccionesPersonalesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public TiposAccionesPersonalesController(IApiServicio apiServicio)
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
            InicializarMensaje(mensaje);

            var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
            {

                MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            },
                TipoAccionPersonal = new TipoAccionPersonal {
                    NDiasMaximo = 0,
                    NDiasMinimo = 0,
                    NHorasMaximo = 0,
                    NHorasMinimo = 0
              
                }
            };
            
            ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewmodel.MatrizLista, "Id", "Nombre");


            return View(tipoAccionPersonalViewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoAccionPersonalViewModel tipoAccionPersonalViewModel)
        {

            

            Response response = new Response();
            try
            {
                var MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            };

                // Obtener el valor de empleadoCambio y setear la variable del modelo a la que equivale 
                // 0 = n/a || 1 = Modalidad contratación || 2 = desactivar empleado
                switch (tipoAccionPersonalViewModel.empleadoCambio)
                {
                    case 0:
                        tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = false;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.ModalidadContratacion = false;
                        break;

                    case 1:
                        tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = false;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.ModalidadContratacion = true;
                        break;

                    case 2:
                        tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = true;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.ModalidadContratacion = false;
                        break;

                    default:
                    break;
                }

                // Obtener el valor de grp_tiempo_minimo y setear la variable del modelo a la que equivale
                if (tipoAccionPersonalViewModel.grp_tiempo_minimo == "definitivo")
                {
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = true;
                }
                else {
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = false;
                }


                if (!ModelState.IsValid)
                {

                    ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(MatrizLista, "Id", "Nombre");
                    
                    InicializarMensaje(Mensaje.ModeloInvalido);

                    return View(tipoAccionPersonalViewModel);
                }
           


                response = await apiServicio.InsertarAsync(tipoAccionPersonalViewModel.TipoAccionPersonal,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TiposAccionesPersonales/InsertarTipoAccionPersonal");

                tipoAccionPersonalViewModel.MatrizLista = MatrizLista;
                



                if (response.IsSuccess)
                {
                    return RedirectToAction("Index", new { mensaje = response.Message});
                }
                
               
                ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewModel.MatrizLista, "Id", "Nombre");

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                InicializarMensaje("");

                return View(tipoAccionPersonalViewModel);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> Edit(string id,string mensaje)
        {
            InicializarMensaje(mensaje);

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/TiposAccionesPersonales");


                    respuesta.Resultado = JsonConvert.DeserializeObject<TipoAccionPersonal>(respuesta.Resultado.ToString());

                    var tipoAccionPersonalViewModel = new TipoAccionPersonalViewModel
                    {

                        MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            },


                        TipoAccionPersonal = (TipoAccionPersonal)respuesta.Resultado
                    };

                    ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewModel.MatrizLista, "Id", "Nombre");
                   


                    if (respuesta.IsSuccess)
                    {

                        // Obtener el valor de empleadoCambio a la que equivale 
                        // 0 = n/a || 1 = Modalidad contratación || 2 = desactivar empleado
                        
                        if (    
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado == false
                            && tipoAccionPersonalViewModel.TipoAccionPersonal.ModalidadContratacion == false
                        ) {
                            tipoAccionPersonalViewModel.empleadoCambio = 0;
                        }

                        else if (
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado == false
                            && tipoAccionPersonalViewModel.TipoAccionPersonal.ModalidadContratacion == true
                        )
                        {
                            tipoAccionPersonalViewModel.empleadoCambio = 1;
                        }

                        else if (
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado == true
                            && tipoAccionPersonalViewModel.TipoAccionPersonal.ModalidadContratacion == false
                        )
                        {
                            tipoAccionPersonalViewModel.empleadoCambio = 2;
                        }


                        return View(tipoAccionPersonalViewModel);
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
        public async Task<IActionResult> Edit(string id, TipoAccionPersonalViewModel tipoAccionPersonalViewModel)
        {
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "Matriz", Nombre = "Matriz"},
                                new Matriz {Id ="Regional", Nombre = "Regional"},
                                new Matriz {Id = "Matriz y Regional", Nombre = "Matriz y Regional"}
                            };


                    // Obtener el valor de empleadoCambio y setear la variable del modelo a la que equivale 
                    // 0 = n/a || 1 = Modalidad contratación || 2 = desactivar empleado
                    switch (tipoAccionPersonalViewModel.empleadoCambio)
                    {
                        case 0:
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = false;
                            tipoAccionPersonalViewModel.TipoAccionPersonal.ModalidadContratacion = false;
                            break;

                        case 1:
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = false;
                            tipoAccionPersonalViewModel.TipoAccionPersonal.ModalidadContratacion = true;
                            break;

                        case 2:
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = true;
                            tipoAccionPersonalViewModel.TipoAccionPersonal.ModalidadContratacion = false;
                            break;

                        default:
                            break;
                    }

                    // Obtener el valor de grp_tiempo_minimo y setear la variable del modelo a la que equivale
                    if (tipoAccionPersonalViewModel.grp_tiempo_minimo == "definitivo")
                    {
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = true;
                    }
                    else
                    {
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = false;
                    }



                    response = await apiServicio.EditarAsync(id, tipoAccionPersonalViewModel.TipoAccionPersonal, new Uri(WebApp.BaseAddress),"api/TiposAccionesPersonales");
                    

                    if (response.IsSuccess)
                    {

                        return RedirectToAction("Index", new { mensaje = response.Message});
                    }

                    ViewData["Error"] = response.Message;


                    ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(MatrizLista, "Id", "Nombre");
                    


                    return View(tipoAccionPersonalViewModel);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                

                return BadRequest();
            }
        }

        public async Task<IActionResult> Index(string mensaje)
        {
            InicializarMensaje(mensaje);

            var lista = new List<TipoAccionPersonal>();
            try
            {
                lista = await apiServicio.Listar<TipoAccionPersonal>(new Uri(WebApp.BaseAddress)
                                                                    , "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");
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
                                                               , "api/TiposAccionesPersonales");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index", new { mensaje = response.Message});
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
    }
}