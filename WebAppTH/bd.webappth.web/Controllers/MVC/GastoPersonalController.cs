using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using bd.webappth.servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bd.webappth.web.Controllers.MVC
{
    public class GastoPersonalController : Controller
    {
        private readonly IApiServicio apiServicio;


        public GastoPersonalController(IApiServicio apiServicio)
        {
            this.apiServicio = apiServicio;
        }
        public async Task<IActionResult> Index()
        {


            var lista = new List<ListaEmpleadoViewModel>();
            try
            {
                lista = await apiServicio.Listar<ListaEmpleadoViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleadosActivos");

                return View(lista);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}