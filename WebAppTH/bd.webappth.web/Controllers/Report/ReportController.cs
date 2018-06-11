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
            
            string url = string.Format("{0}{1}{2}", ReportConfig.CompletePath,"RepNomina&IdCalculoNomina=", Convert.ToString(id));
            return Redirect(url);
        }
        //public ActionResult ReporteSolicitudPagoReliquidacion(int idReliquidacionViatico)
        //{
        //    //var model = this.GetReportViewerModel(Request);
        //    //model.ReportPath = ReportPath("RepSolicitudPagoReliquidacion");
        //    //model.AddParameter("IdReliquidacionViatico", Convert.ToString(idReliquidacionViatico));
        //    ////model.AddParameter("Parameter2", namedParameter2);

        //    //return View("ReportViewer", model);
        //}
    }
}

