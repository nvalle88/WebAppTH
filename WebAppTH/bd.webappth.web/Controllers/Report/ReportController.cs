using bd.webappth.entidades.Utils;
using Microsoft.AspNetCore.Mvc;

using System;

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
    }
}

