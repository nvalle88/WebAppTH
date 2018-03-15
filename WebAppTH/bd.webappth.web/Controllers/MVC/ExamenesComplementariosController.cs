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
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using bd.webappth.entidades.ObjectTransfer;
using Microsoft.AspNetCore.Hosting;

namespace bd.webappth.web.Controllers.MVC
{
    public class ExamenesComplementariosController : Controller
    {
        
        private readonly IApiServicio apiServicio;
        private IHostingEnvironment _hostingEnvironment;


        public ExamenesComplementariosController(IApiServicio apiServicio, IHostingEnvironment _hostingEnvironment)
        {
            this.apiServicio = apiServicio;
            this._hostingEnvironment = _hostingEnvironment;

        }

        private void InicializarMensaje(string mensaje)

        {

            if (mensaje == null)
            {
                 mensaje = "";
            }

            ViewData["Error"] = mensaje;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2(FichaMedica fichaMedica)
        {
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");
            ExamenComplementario antLab = new ExamenComplementario();
            antLab.IdFichaMedica = fichaMedica.IdFichaMedica;
            antLab.Fecha =  DateTime.Today;
            antLab.Resultado = "Esperando resultado";

            InicializarMensaje("");

            return View("Create", antLab);
        }

        

        public async Task<IActionResult> Create(string mensaje, FichaMedica fichaMedica)
        {
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");
            ExamenComplementario antLab = new ExamenComplementario();

            InicializarMensaje(mensaje);

            return View(antLab);
        }


        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamenComplementario ExamenComplementario)
        {

            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");
            ExamenesComplementariosViewModel alvm = new ExamenesComplementariosViewModel();


            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.ModeloInvalido);
            
                return View(ExamenComplementario);
            }


            Response response = new Response();


            try
            {
                response = await apiServicio.InsertarAsync(ExamenComplementario,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ExamenesComplementarios/InsertarExamenesComplementarios");
                if (response.IsSuccess)
                {

                    
                    return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = ExamenComplementario.IdFichaMedica });
                }
                
                //return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = response.Message+"", idFicha = ExamenComplementario.IdFichaMedica });

                InicializarMensaje(Mensaje.ModeloInvalido);

                return View(ExamenComplementario);
            }
            catch (Exception ex)
            {
                InicializarMensaje(Mensaje.Error);
                return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.Excepcion, idFicha = ExamenComplementario.IdFichaMedica });
            }


        }
        */
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamenComplementario examenComplementario, List<IFormFile> files)
        {

            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");
            ExamenesComplementariosViewModel alvm = new ExamenesComplementariosViewModel();


            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.ModeloInvalido);



                return View(examenComplementario);
            }


            Response response = new Response();


            try
            {
                

                if (files.Count > 0)
                {
                    byte[] data;
                    using (var br = new BinaryReader(files[0].OpenReadStream()))
                        data = br.ReadBytes((int)files[0].OpenReadStream().Length);

                    var documenttransfer = new ExamenComplementarioTransfer
                    {
                        IdExamenComplementario = examenComplementario.IdExamenComplementario,
                        Fecha = examenComplementario.Fecha,
                        Resultado = examenComplementario.Resultado,
                        IdTipoExamenComplementario = examenComplementario.IdTipoExamenComplementario,
                        IdFichaMedica = examenComplementario.IdFichaMedica,
                        Url = examenComplementario.Url,

                        Fichero = data,
                    };

                    var respuesta = await CreateFichero(documenttransfer);
                    if (respuesta.IsSuccess)
                    {
                        return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = examenComplementario.IdFichaMedica });
                    }
                    else
                    {
                        ViewData["Error"] = respuesta.Message;

                        var documento = new ExamenComplementarioTransfer
                        {
                            IdExamenComplementario = examenComplementario.IdExamenComplementario,
                            Fecha = examenComplementario.Fecha,
                            Resultado = examenComplementario.Resultado,
                            IdTipoExamenComplementario = examenComplementario.IdTipoExamenComplementario,
                            IdFichaMedica = examenComplementario.IdFichaMedica,
                            Url = examenComplementario.Url,

                            Fichero = data,
                        };

                        
                        return View(documento);
                    }

                }
                else
                {
                    if (!ModelState.IsValid)
                    {
                        InicializarMensaje(Mensaje.ModeloInvalido);

                        return View(examenComplementario);
                    }

                    response = await apiServicio.InsertarAsync(examenComplementario,
                                                         new Uri(WebApp.BaseAddress),
                                                         "api/ExamenesComplementarios/InsertarExamenesComplementarios");
                    if (response.IsSuccess)
                    {
                        return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = examenComplementario.IdFichaMedica });
                    }
                }
                
                return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.ErrorCargaArchivo, idFicha = examenComplementario.IdFichaMedica });
            }
            catch (Exception ex)
            {
                InicializarMensaje(Mensaje.Error);
                return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.Excepcion, idFicha = examenComplementario.IdFichaMedica });
            }


        }
        
    


        public async Task<IActionResult> Edit(string id)
        {
            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/ExamenesComplementarios");


                    respuesta.Resultado = JsonConvert.DeserializeObject<ExamenComplementario>(respuesta.Resultado.ToString());

                    
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
        public async Task<IActionResult> Edit(string id, ExamenComplementario examenComplementario, List<IFormFile> files)
        {

            ViewData["IdTipoExamenComplementario"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<TipoExamenComplementario>(new Uri(WebApp.BaseAddress), "api/TiposExamenesComplementarios/ListarTiposExamenesComplementarios"), "IdTipoExamenComplementario", "Nombre");

            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {

                    if (files.Count > 0)
                    {
                        byte[] data;
                        using (var br = new BinaryReader(files[0].OpenReadStream()))
                            data = br.ReadBytes((int)files[0].OpenReadStream().Length);

                        var documenttransfer = new ExamenComplementarioTransfer
                        {
                            IdExamenComplementario = examenComplementario.IdExamenComplementario,
                            Fecha = examenComplementario.Fecha,
                            Resultado = examenComplementario.Resultado,
                            IdTipoExamenComplementario = examenComplementario.IdTipoExamenComplementario,
                            IdFichaMedica = examenComplementario.IdFichaMedica,
                            Url = examenComplementario.Url,

                            Fichero = data,
                        };

                        var respuesta = await UpdateFichero(documenttransfer);

                        if (respuesta.IsSuccess)
                        {
                            return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = examenComplementario.IdFichaMedica });
                        }
                        else
                        {
                            ViewData["Error"] = respuesta.Message;

                            var documento = new ExamenComplementarioTransfer
                            {
                                IdExamenComplementario = examenComplementario.IdExamenComplementario,
                                Fecha = examenComplementario.Fecha,
                                Resultado = examenComplementario.Resultado,
                                IdTipoExamenComplementario = examenComplementario.IdTipoExamenComplementario,
                                IdFichaMedica = examenComplementario.IdFichaMedica,
                                Url = examenComplementario.Url,

                                Fichero = data,
                            };


                            return View(documento);
                        }

                    }
                    else
                    {
                        response = await apiServicio.EditarAsync(id, examenComplementario, new Uri(WebApp.BaseAddress),
                                                                 "api/ExamenesComplementarios");

                        if (response.IsSuccess)
                        {

                            return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = examenComplementario.IdFichaMedica });
                        }

                        return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = response.Message, idFicha = examenComplementario.IdFichaMedica });
                    }
    
                }

                return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.ErrorCargaArchivo, idFicha = examenComplementario.IdFichaMedica });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.Excepcion, idFicha = examenComplementario.IdFichaMedica });

            }
        }

        

        public async Task<IActionResult> Index(string mensaje, int idFicha, int idPersona)
        {

            var alvm = new ExamenesComplementariosViewModel();

            var lista = new List<ExamenComplementario>();

            var fichaMedica = new FichaMedica();
            fichaMedica.IdPersona = idPersona;
            fichaMedica.IdFichaMedica = idFicha;


            Response response = new Response();

            try
            {

                if (idPersona < 1)
                {
                    response = await apiServicio.InsertarAsync<Response>(fichaMedica.IdFichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/VerIdPersonaPorFicha");

                    if (response.IsSuccess)
                    {
                        fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());

                    }

                }
                else
                {
                    response = await apiServicio.InsertarAsync<Response>(fichaMedica,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/FichasMedicas/VerUltimaFichaMedica");


                    if (response.IsSuccess)
                    {
                        fichaMedica = JsonConvert.DeserializeObject<FichaMedica>(response.Resultado.ToString());
                    }

                }


                response = await apiServicio.InsertarAsync<Response>(fichaMedica.IdFichaMedica,
                                                                 new Uri(WebApp.BaseAddress),
                                                                 "api/ExamenesComplementarios/ListarExamenesComplementariosPorFicha");

                if (response.IsSuccess)
                {
                    lista = JsonConvert.DeserializeObject<List<ExamenComplementario>>(response.Resultado.ToString());
                }



                InicializarMensaje(mensaje);


                alvm.ListaExamenesComplementarios = lista;
                alvm.fichaMedica = fichaMedica;

                return View(alvm);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }



        public async Task<IActionResult> Delete(string id, int idFM)
        {

            FichaMedica fm = new FichaMedica();
            fm.IdFichaMedica = idFM;

            try
            {
                var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                               , "api/ExamenesComplementarios");
                if (response.IsSuccess)
                {

                    return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = Mensaje.BorradoSatisfactorio, idFicha = idFM });

                }

                return RedirectToAction("Index", "ExamenesComplementarios", new { mensaje = response.Message, idFicha = idFM });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> CreateFichero(ExamenComplementarioTransfer file)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(file,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ExamenesComplementarios/UploadFiles");
                if (response.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = true,
                        Message = response.Message,
                    };

                }

                ViewData["Error"] = response.Message;
                return new Response
                {
                    IsSuccess = false,
                    Message = response.Message,
                };

            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = response.Message,
                };
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> UpdateFichero(ExamenComplementarioTransfer file)
        {
            Response response = new Response();
            try
            {
                response = await apiServicio.InsertarAsync(file,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ExamenesComplementarios/UpdateFiles");
                if (response.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = true,
                        Message = response.Message,
                    };

                }

                ViewData["Error"] = response.Message;
                return new Response
                {
                    IsSuccess = false,
                    Message = response.Message,
                };

            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = response.Message,
                };
            }
        }




        public async Task<FileResult> Download(string id)
        {
            var id2 = new ExamenComplementario
            {
                IdExamenComplementario = Convert.ToInt32(id),
            };

            var response = await apiServicio.ObtenerElementoAsync(id2,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/ExamenesComplementarios/GetFile");

            
            var m = JsonConvert.DeserializeObject<ExamenComplementarioTransfer>(response.Resultado.ToString());
            var fileName = $"{ response.Message}.pdf";

            return File(m.Fichero, "application/pdf", fileName);
        }




    }
}