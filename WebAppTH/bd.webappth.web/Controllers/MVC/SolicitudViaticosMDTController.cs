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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using bd.webappth.servicios.Extensores;

namespace bd.webappth.web.Controllers.MVC
{
    public class SolicitudViaticosMDTController : Controller
    {

        private readonly IApiServicio apiServicio;


        public SolicitudViaticosMDTController(IApiServicio apiServicio)
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


        public async Task<ActionResult> ListadoEmpleadosSolicitudViaticos()
        {
            var listadoEmpleados = await ListarEmpleadosconViaticos();
            return View(listadoEmpleados);
        }


        public async Task<List<EmpleadoSolicitudViewModel>> ListarEmpleadosconViaticos()
        {
            try
            {

              var   lista = await apiServicio.Listar<EmpleadoSolicitudViewModel>(new Uri(WebApp.BaseAddress)
                                                                    , "api/Empleados/ListarEmpleadosdeJefeconSolucitudesViaticosMDT");                
                return lista;
            }
            catch (Exception)
            {
                return new List<EmpleadoSolicitudViewModel>();
            }

        }
    }

}