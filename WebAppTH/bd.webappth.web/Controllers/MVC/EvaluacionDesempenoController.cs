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
    public class EvaluacionDesempenoController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IApiServicio apiServicio;
        public EvaluacionDesempenoController(IApiServicio apiServicio)
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
        public async Task<IActionResult> Index()
        {
            var lista = new List<ViewModelEvaluacionDesempeno>();
            try
            {
                lista = await apiServicio.Listar<ViewModelEvaluacionDesempeno>(new Uri(WebApp.BaseAddress)
                                                                    , "api/EvaluacionDesempeno/ListarEmpleados");

                InicializarMensaje(null);
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}