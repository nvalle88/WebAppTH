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
    }
}

