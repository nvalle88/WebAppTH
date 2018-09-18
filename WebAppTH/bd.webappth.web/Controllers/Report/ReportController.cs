using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using bd.webappth.servicios.Extensores;
using System.Security.Claims;
using bd.webappth.entidades.Negocio;
using bd.webappth.servicios.Interfaces;

namespace bd.webappth.web.Controllers.MVC
{
    public class ReportController : Controller
    {

        private readonly IApiServicio apiServicio;
        private readonly IReporteServicio reporteServicio;

        public ReportController(IApiServicio apiServicio,  IReporteServicio reporteServicio)
        {
            this.apiServicio = apiServicio;
            this.reporteServicio = reporteServicio;
        }

        public ActionResult ReporteNomina(int id)
        {
            
            string url = string.Format("{0}{1}{2}", ReportConfig.CompletePath,"RepNomina&IdCalculoNomina=", Convert.ToString(id));
            return Redirect(url);
        }

        public ActionResult RepPagoNominaEmpleado(int id)
        {
            var parametersToAdd = reporteServicio.GetDefaultParameters("/ReporteGTH/RepPagoNominaEmpleado");
            parametersToAdd = reporteServicio.AddParameters("IdCalculoNomina", Convert.ToString(id),parametersToAdd);
            var newUri = reporteServicio.GenerateUri(parametersToAdd);
            return Redirect(newUri);
        }

        public ActionResult ReporteSolicitudPagoReliquidacion(int idReliquidacionViatico)
        {
            string url = string.Format("{0}{1}{2}", ReportConfig.CompletePath, "RepSolicitudPagoReliquidacion&IdReliquidacionViatico=", Convert.ToString(idReliquidacionViatico));
            return Redirect(url);
            
        }

        public ActionResult RepMatr05PlanificacionTH()
        {
            string url = string.Format("{0}{1}", ReportConfig.CompletePath, "RepMatr05PlanificacionTH");
            return Redirect(url);

        }

        public ActionResult ReportePlanCapacitaciones()
        {
            string url = string.Format("{0}{1}", ReportConfig.CompletePath, "RepPlanCapacitaciones");
            return Redirect(url);

        }
        public ActionResult ReporteViaticos()
        {
            string url = string.Format("{0}{1}", ReportConfig.CompletePath, "RepViaticos");
            return Redirect(url);

        }
        //public ActionResult ReporteEvaluacionCapacitacion(int idEvento,string Identificacion)
        //{
        //    string url = string.Format("{0}{1}{2}{3}{4}", ReportConfig.CompletePath, "RepEvaluacionEvento&IdPlanCapacitacion=", Convert.ToString(idEvento), "&Identificacion=", Convert.ToString(Identificacion));
        //    return Redirect(url);

        //}
        public ActionResult ReporteEvaluacionCapacitacion()
        {
            string url = string.Format("{0}{1}", ReportConfig.CompletePath, "RepEvaluacionEvento");
            return Redirect(url);

        }
        public ActionResult ReporteSolicitudViaticosMDT(int IdEmpleado,string IdSolicitud)
        {
            string url = string.Format("{0}{1}{2}{3}{4}", ReportConfig.CompletePath, "RepSolicitudViaticoFormatoMDT&IdEmpleado=", Convert.ToString(IdEmpleado), "&IdSolicitud=", Convert.ToString(IdSolicitud));
            return Redirect(url);

        }
        public ActionResult ReporteInformeViaticosMDT(string IdSolicitud)
        {
            string url = string.Format("{0}{1}{2}", ReportConfig.CompletePath, "RepInformeViaticoFormatoMDT&IdSolicitud=", Convert.ToString(IdSolicitud));
            return Redirect(url);

        }

        public ActionResult ReporteConPiePagina(GenerarFirmasViewModel modelo)
        {
            var UrlReporte = modelo.UrlReporte;

            var maxFirmasReporte = 5;
            var idFirmas = "";



            for (int i = 0;i<modelo.ListaIdEmpleados.Count && i<maxFirmasReporte ;i++)
            {
                idFirmas = idFirmas + "&IdEF"+ (i+1) +"="+ modelo.ListaIdEmpleados.Where(w=>w.Prioridad == (i+1) ).FirstOrDefault().IdEmpleado;
                
            }
            
            string url = string.Format("{0}{1}", ReportConfig.CompletePath, UrlReporte + idFirmas);
            return Redirect(url);
            
        }

        public ActionResult ReporteCertificadoInduccion(int IdEmpleado)
        {
            var parametersToAdd = reporteServicio.GetDefaultParameters("/ReporteGTH/RepCertificadoInduccion");
            parametersToAdd = reporteServicio.AddParameters("IdEmpleado", Convert.ToString(IdEmpleado), parametersToAdd);
            var newUri = reporteServicio.GenerateUri(parametersToAdd);
            return Redirect(newUri);
         
        }

        public ActionResult ReporteParticipacionEventosInduccion()
        {
            try {

                var claim = HttpContext.User.Identities.Where(x => x.NameClaimType == ClaimTypes.Name).FirstOrDefault();

                if (claim.IsAuthenticated == true)
                {
                    var nombreUsuario = claim.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

                    var filtros = new IdFiltrosViewModel{ NombreUsuario = nombreUsuario};

                    var sucursal = apiServicio.ObtenerElementoAsync1<Sucursal>(
                            filtros,
                            new Uri(WebApp.BaseAddress),
                            "api/Sucursal/ObtenerSucursalPorEmpleado").Result;


                    string url = string.Format("{0}{1}{2}", ReportConfig.CompletePath, "RepParticipacionEventoInduccion&IdSucursal=", Convert.ToString(sucursal.IdSucursal));

                    return Redirect(url);


                }

                return RedirectToAction("Login", "Login");
                
            } catch (Exception ex) {

                this.TempData["MensajeTimer"] = $"{Mensaje.Error}|{Mensaje.NoProcesarSolicitud}|{"10000"}";

                return BadRequest();

            }
            
        }


        public ActionResult ReporteEstadisticasIngreso()
        {
            string url = string.Format("{0}{1}", ReportConfig.CompletePath, "RepEstadisticasIngresoPersonalPeriodoCorte");

            return Redirect(url);

        }


    }
}

