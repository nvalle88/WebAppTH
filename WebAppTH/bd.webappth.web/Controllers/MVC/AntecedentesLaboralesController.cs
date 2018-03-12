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

namespace bd.webappth.web.Controllers.MVC
{
    public class AntecedentesLaboralesController : Controller
    {
        
        private readonly IApiServicio apiServicio;


        public AntecedentesLaboralesController(IApiServicio apiServicio)
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

        public async Task<IActionResult> Create(string mensaje, FichaMedica fichaMedica)
        {
            //ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");

            AntecedentesLaborales antLab = new AntecedentesLaborales();
            //antLab.IdFichaMedica = this.idFichaCache;

            InicializarMensaje(mensaje);

            return View(antLab);
        }


        public async Task<IActionResult> Create2(string mensaje, int idFicha, int idPersona)
        {

            AntecedentesLaborales antLab = new AntecedentesLaborales();
            antLab.IdFichaMedica = idFicha;

            InicializarMensaje("");

            return View("Create", antLab);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2(FichaMedica fichaMedica)
        {

            AntecedentesLaborales antLab = new AntecedentesLaborales();
            antLab.IdFichaMedica = fichaMedica.IdFichaMedica;

            InicializarMensaje("");

            return View("Create", antLab);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AntecedentesLaborales AntecedentesLaborales)
        {


            AntecedentesLaboralesViewModel alvm = new AntecedentesLaboralesViewModel();


            if (!ModelState.IsValid)
            {
                InicializarMensaje(Mensaje.ModeloInvalido);

                //ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                return View(AntecedentesLaborales);
            }


            Response response = new Response();


            try
            {
                response = await apiServicio.InsertarAsync(AntecedentesLaborales,
                                                             new Uri(WebApp.BaseAddress),
                                                             "api/AntecedentesLaborales/InsertarAntecedentesLaborales");
                if (response.IsSuccess)
                {

                    //ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
                    
                    return RedirectToAction("Index","AntecedentesLaborales", new { mensaje = Mensaje.GuardadoSatisfactorio ,idFicha = AntecedentesLaborales.IdFichaMedica});
                }

                //ViewData["Error"] = response.Message + ", verifique que haya seleccionado un: código ficha médica" ;
                //ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");

                return RedirectToAction("Index", "AntecedentesLaborales", new { mensaje = response.Message ,idFicha = AntecedentesLaborales.IdFichaMedica });

            }
            catch (Exception ex)
            {
                InicializarMensaje(Mensaje.Error);
                return RedirectToAction("Index", "AntecedentesLaborales", new { mensaje = Mensaje.Excepcion,idFicha = AntecedentesLaborales.IdFichaMedica });
            }


        }

        public async Task<IActionResult> Edit(string id)
        {
            //ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var respuesta = await apiServicio.SeleccionarAsync<Response>(id, new Uri(WebApp.BaseAddress),
                                                                  "api/AntecedentesLaborales");


                    respuesta.Resultado = JsonConvert.DeserializeObject<AntecedentesLaborales>(respuesta.Resultado.ToString());

                    //ViewData["IdEmpleado"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<Empleado>(new Uri(WebApp.BaseAddress), "api/Empleados/ListarEmpleados"), "IdEmpleado", "Identificacion");


                    if (respuesta.IsSuccess)
                    {
                        InicializarMensaje(null);
                        return View(respuesta.Resultado);
                    }
                    //ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");

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
        public async Task<IActionResult> Edit(string id, AntecedentesLaborales AntecedentesLaborales)
        {
            ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");
            Response response = new Response();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    response = await apiServicio.EditarAsync(id, AntecedentesLaborales, new Uri(WebApp.BaseAddress),
                                                                 "api/AntecedentesLaborales");

                    if (response.IsSuccess)
                    {
                        
                        return RedirectToAction("Index", "AntecedentesLaborales", new { mensaje = Mensaje.GuardadoSatisfactorio, idFicha = AntecedentesLaborales.IdFichaMedica });
                    }
                    
                    return RedirectToAction("Index", "AntecedentesLaborales", new { mensaje = response.Message, idFicha = AntecedentesLaborales.IdFichaMedica });
                    

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                

                return BadRequest();

            }
        }

        public async Task<IActionResult> Index(string mensaje, int idFicha, int idPersona)
        {
            //ViewData["IdFichaMedica"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await apiServicio.Listar<FichaMedica>(new Uri(WebApp.BaseAddress), "api/FichasMedicas/ListarFichasMedicas"), "IdFichaMedica", "IdFichaMedica");

            var alvm = new AntecedentesLaboralesViewModel();

            var lista = new List<AntecedentesLaborales>();

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
                                                                 "api/AntecedentesLaborales/ListarAntecedentesLaboralesPorFicha");

                if (response.IsSuccess)
                {
                    lista = JsonConvert.DeserializeObject<List<AntecedentesLaborales>>(response.Resultado.ToString());    
                }
                

                
                InicializarMensaje(mensaje);


                alvm.ListaAntecedentesLaborales = lista;
                alvm.fichaMedica = fichaMedica;

                return View(alvm);

            }
            catch (Exception ex)
            {
               
                return BadRequest();
            }
        }


        

            public async Task<IActionResult> Delete(string id, int idFM )
            {

                FichaMedica fm = new FichaMedica();
                fm.IdFichaMedica = idFM;

                try
                {
                    var response = await apiServicio.EliminarAsync(id, new Uri(WebApp.BaseAddress)
                                                                   , "api/AntecedentesLaborales");
                    if (response.IsSuccess)
                    {
                    
                        return RedirectToAction("Index", "AntecedentesLaborales", new { mensaje = Mensaje.BorradoSatisfactorio,idFicha = idFM });

                    }
                    
                    return RedirectToAction("Index", "AntecedentesLaborales", new { mensaje = response.Message , idFicha = idFM });
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }
        
    }
}