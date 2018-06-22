using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bd.webappth.servicios.Interfaces;
using bd.webappth.entidades.Negocio;
using bd.webappth.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.webappseguridad.entidades.Enumeradores;
using bd.log.guardar.ObjectTranfer;
using bd.log.guardar.Enumeradores;
using Newtonsoft.Json;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class VacacionRelacionesLaboralesController : Controller
    {
        private readonly IApiServicio apiServicio;


        public VacacionRelacionesLaboralesController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }
        

        public async Task<IActionResult> Index()
        {

            var lista = new List<VacacionRelacionLaboral>();
            try
            {
                lista = await apiServicio.Listar<VacacionRelacionLaboral>(
                    new Uri(WebApp.BaseAddress), 
                    "api/VacacionRelacionesLaborales/ListarVacacionRelacionesLaborales");

                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Create()
        {
            try {

                var model = new VacacionRelacionLaboral();

                await CargarCombos();

                return View(model);

            } catch (Exception) {
                return BadRequest();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacacionRelacionLaboral modelo)
        {
            try {

                if (! ModelState.IsValid) {

                    await CargarCombos();

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"12000"}";
                    return View(modelo);
                }

                var respuesta = await apiServicio.InsertarAsync(
                    modelo, 
                    new Uri(WebApp.BaseAddress),
                    "api/VacacionRelacionesLaborales/InsertarVacacionesrelacionesLaborales");

                if (respuesta.IsSuccess) {

                    return this.Redireccionar(
                            "VacacionRelacionesLaborales",
                            "Index",
                            $"{Mensaje.Success}|{respuesta.Message}"
                         );
                }

                await CargarCombos();

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{respuesta.Message}|{"12000"}";
                return View(modelo);


            } catch (Exception) {
                return BadRequest();
            }
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            try
            {

                var modelo = new VacacionRelacionLaboral { IdVacacionRelacionLaboral = id };

                var resultado = await apiServicio.ObtenerElementoAsync1<VacacionRelacionLaboral>(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/VacacionRelacionesLaborales/ObtenerVacacionRelacionLaboral");

                await CargarCombos();
                
                return View(resultado);


            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VacacionRelacionLaboral modelo)
        {
            try
            {

                if (!ModelState.IsValid)
                {

                    await CargarCombos();

                    this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.ModeloInvalido}|{"12000"}";
                    return View(modelo);
                }

                var respuesta = await apiServicio.InsertarAsync(
                    modelo,
                    new Uri(WebApp.BaseAddress),
                    "api/VacacionRelacionesLaborales/EditarVacacionesrelacionesLaborales");

                if (respuesta.IsSuccess)
                {

                    return this.Redireccionar(
                            "VacacionRelacionesLaborales",
                            "Index",
                            $"{Mensaje.Success}|{respuesta.Message}"
                         );
                }

                await CargarCombos();

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{respuesta.Message}|{"12000"}";
                return View(modelo);


            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        
        public async Task<IActionResult> Delete(int id)
        {

            try
            {

                var response = await apiServicio.EliminarAsync(
                            id,
                            new Uri(WebApp.BaseAddress),
                            "api/VacacionRelacionesLaborales/BorrarVacacionRelacionLaboral");

                if (response.IsSuccess == true)
                {

                    return this.Redireccionar(
                            "VacacionRelacionesLaborales",
                            "Index",
                            $"{Mensaje.Success}|{response.Message}"
                         );
                }

                return this.Redireccionar(
                            "VacacionRelacionesLaborales",
                            "Index",
                            $"{Mensaje.Error}|{response.Message}"
                         );

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        public async Task CargarCombos() {
            try {

                var lista = await apiServicio.Listar<RelacionLaboral>(
                    new Uri(WebApp.BaseAddress),
                    "api/RegimenesLaborales/ListarRegimenesLaborales");


                ViewData["RegimenLaboral"] = new SelectList(lista, "IdRegimenLaboral", "Nombre");

            } catch (Exception ex)
            {
                throw;
            }
        }

    }
}