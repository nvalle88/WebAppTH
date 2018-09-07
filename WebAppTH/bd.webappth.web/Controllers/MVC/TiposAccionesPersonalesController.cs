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
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class TiposAccionesPersonalesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public TiposAccionesPersonalesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;

        }

        
        


        public async Task<IActionResult> Create()
        {

            var tipoAccionPersonalViewmodel = new TipoAccionPersonalViewModel
            {

                MatrizLista = new List<Matriz>
                            {
                                new Matriz {Id = "MATRIZ", Nombre = "MATRIZ"},
                                new Matriz {Id ="REGIONAL", Nombre = "REGIONAL"},
                                new Matriz {Id = "MATRIZ Y REGIONAL", Nombre = "MATRIZ Y REGIONAL"}
                            },
                TipoAccionPersonal = new TipoAccionPersonal {
                    NdiasMaximo = 0,
                    NdiasMinimo = 0,
                    NhorasMaximo = 0,
                    NhorasMinimo = 0
              
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
                                new Matriz {Id = "MATRIZ", Nombre = "MATRIZ"},
                                new Matriz {Id ="REGIONAL", Nombre = "REGIONAL"},
                                new Matriz {Id = "MATRIZ Y REGIONAL", Nombre = "MATRIZ Y REGIONAL"}
                            };

                // Obtener el valor de empleadoCambio y setear la variable del modelo a la que equivale 
                // 0 = n/a || 1 = Modalidad contratación || 2 = desactivar empleado
                switch (tipoAccionPersonalViewModel.empleadoCambio)
                {
                    case 0:
                        tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = false;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.ModificarDistributivo = false;
                        break;

                    case 1:
                        tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = false;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.ModificarDistributivo = true;
                        break;

                    case 2:
                        tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = true;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.ModificarDistributivo = true;
                        break;

                    default:
                    break;
                }

                // Obtener el valor de grp_tiempo_minimo y setear la variable del modelo a la que equivale
                if (tipoAccionPersonalViewModel.grp_tiempo_minimo == "definitivo")
                {
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = true;
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Dias = false;
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Horas = false;
                }

                else if (tipoAccionPersonalViewModel.grp_tiempo_minimo == "horas")
                {
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = false;
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Dias = false;
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Horas = true;
                }

                else if (tipoAccionPersonalViewModel.grp_tiempo_minimo == "dias")
                {
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = false;
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Dias = true;
                    tipoAccionPersonalViewModel.TipoAccionPersonal.Horas = false;
                }


                if (!ModelState.IsValid)
                {

                    ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(MatrizLista, "Id", "Nombre");
                    
                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"10000"}";

                    return View(tipoAccionPersonalViewModel);
                }
           


                response = await apiServicio.InsertarAsync(tipoAccionPersonalViewModel.TipoAccionPersonal,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/TiposAccionesPersonales/InsertarTipoAccionPersonal");

                tipoAccionPersonalViewModel.MatrizLista = MatrizLista;
                



                if (response.IsSuccess)
                {
                    return this.Redireccionar(
                            "TiposAccionesPersonales",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}"
                         );
                }
                
               
                ViewData["IdMatriz"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tipoAccionPersonalViewModel.MatrizLista, "Id", "Nombre");

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";
                

                return View(tipoAccionPersonalViewModel);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        public async Task<IActionResult> Edit(string id)
        {

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
                                new Matriz {Id = "MATRIZ", Nombre = "MATRIZ"},
                                new Matriz {Id ="REGIONAL", Nombre = "REGIONAL"},
                                new Matriz {Id = "MATRIZ Y REGIONAL", Nombre = "MATRIZ Y REGIONAL"}
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
                            && tipoAccionPersonalViewModel.TipoAccionPersonal.ModificarDistributivo == false
                        ) {
                            tipoAccionPersonalViewModel.empleadoCambio = 0;
                        }

                        else if (
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado == false
                            && tipoAccionPersonalViewModel.TipoAccionPersonal.ModificarDistributivo == true
                        )
                        {
                            tipoAccionPersonalViewModel.empleadoCambio = 1;
                        }

                        else if (
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado == true
                            && tipoAccionPersonalViewModel.TipoAccionPersonal.ModificarDistributivo == true
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
                                new Matriz {Id = "MATRIZ", Nombre = "MATRIZ"},
                                new Matriz {Id ="REGIONAL", Nombre = "REGIONAL"},
                                new Matriz {Id = "MATRIZ Y REGIONAL", Nombre = "MATRIZ Y REGIONAL"}
                            };


                    // Obtener el valor de empleadoCambio y setear la variable del modelo a la que equivale 
                    // 0 = n/a || 1 = Modalidad contratación || 2 = desactivar empleado
                    switch (tipoAccionPersonalViewModel.empleadoCambio)
                    {
                        case 0:
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = false;
                            tipoAccionPersonalViewModel.TipoAccionPersonal.ModificarDistributivo = false;
                            break;

                        case 1:
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = false;
                            tipoAccionPersonalViewModel.TipoAccionPersonal.ModificarDistributivo = true;
                            break;

                        case 2:
                            tipoAccionPersonalViewModel.TipoAccionPersonal.DesactivarEmpleado = true;
                            tipoAccionPersonalViewModel.TipoAccionPersonal.ModificarDistributivo = true;
                            break;

                        default:
                            break;
                    }

                    // Obtener el valor de grp_tiempo_minimo y setear la variable del modelo a la que equivale
                    if (tipoAccionPersonalViewModel.grp_tiempo_minimo == "definitivo")
                    {
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = true;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Dias = false;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Horas = false;
                    }

                    else if (tipoAccionPersonalViewModel.grp_tiempo_minimo == "horas")
                    {
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = false;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Dias = false;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Horas = true;
                    }

                    else if (tipoAccionPersonalViewModel.grp_tiempo_minimo == "dias")
                    {
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Definitivo = false;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Dias = true;
                        tipoAccionPersonalViewModel.TipoAccionPersonal.Horas = false;
                    }



                    response = await apiServicio.EditarAsync(id, tipoAccionPersonalViewModel.TipoAccionPersonal, new Uri(WebApp.BaseAddress),"api/TiposAccionesPersonales");
                    

                    if (response.IsSuccess)
                    {
                        
                        return this.Redireccionar(
                            "TiposAccionesPersonales",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}"
                         );
                    }


                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{response.Message}|{"10000"}";


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



        public async Task<IActionResult> Index()
        {

            var lista = new List<TipoAccionPersonal>();
            try
            {
                lista = await apiServicio.Listar<TipoAccionPersonal>(
                    new Uri(WebApp.BaseAddress), "api/TiposAccionesPersonales/ListarTiposAccionesPersonales");

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
                    return this.RedireccionarMensajeTime(
                            "TiposAccionesPersonales",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}|{"7000"}"
                         );
                }

                return this.RedireccionarMensajeTime(
                           "TiposAccionesPersonales",
                           "Index",
                           $"{Mensaje.Error}|{response.Message}|{"10000"}"
                        );
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

    

    }
}