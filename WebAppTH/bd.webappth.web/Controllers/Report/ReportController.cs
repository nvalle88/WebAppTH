using bd.webappth.entidades.Utils;
using Microsoft.AspNetCore.Mvc;
using ReportServiceWCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace bd.webappth.web.Controllers.MVC
{
    public class ReportController : Controller
    {
      

        public ActionResult ReporteNomina(int id)
        {
            var de = new class2();
            
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

