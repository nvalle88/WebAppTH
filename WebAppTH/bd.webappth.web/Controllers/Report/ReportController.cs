using bd.webappth.entidades.Utils;
using bd.webappth.entidades.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace bd.webappth.web.Controllers.MVC
{
    public class ReportController : Controller
    {      

        public ActionResult ReporteNomina(int id)
        {
            
            string url = string.Format("{0}{1}{2}", ReportConfig.CompletePath,"RepNomina&IdCalculoNomina=", Convert.ToString(id));
            return Redirect(url);
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
            string url = string.Format("{0}{1}{2}", ReportConfig.CompletePath, "RepCertificadoInduccion&IdEmpleado=", Convert.ToString(IdEmpleado));

            return Redirect(url);

        }


    }
}

