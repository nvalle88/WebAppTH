using bd.webappth.entidades.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace bd.webappth.web.Controllers.MVC
{
    public class ReportController : AlanJuden.MvcReportViewer.ReportController
    {
        protected override ICredentials NetworkCredentials
        {
            get
            {
               
                if (ReportConfig.DefaultNetworkCredentials)
                {
                    //Default domain credentials(windows authentication)
                    return System.Net.CredentialCache.DefaultNetworkCredentials;
                }

                //Custom Domain authentication (be sure to pull the info from a config file)
                return new System.Net.NetworkCredential(ReportConfig.UserName, ReportConfig.Password, ReportConfig.CustomDomain);

                
               
            }
        }

        protected override string ReportServerUrl
        {
            get
            {
                //You don't want to put the full API path here, just the path to the report server's ReportServer directory that it creates (you should be able to access this path from your browser: https://YourReportServerUrl.com/ReportServer/ReportExecution2005.asmx )
                return ReportConfig.ReportServerUrl;
            }
        }

        public string ReportPath(string ReportName)
        {
          return  string.Format("/{0}/{1}", ReportConfig.ReportFolderPath, ReportName);
        }

        public ActionResult ReporteNomina(int idNomina)
        {
            var model = this.GetReportViewerModel(Request);
            model.ReportPath = ReportPath("RepNomina");
            model.AddParameter("IdCalculoNomina", Convert.ToString(idNomina));
            //model.AddParameter("Parameter2", namedParameter2);

            return View("ReportViewer", model);
        }
        public ActionResult ReporteSolicitudPagoReliquidacion(int idReliquidacionViatico)
        {
            var model = this.GetReportViewerModel(Request);
            model.ReportPath = ReportPath("RepSolicitudPagoReliquidacion");
            model.AddParameter("IdReliquidacionViatico", Convert.ToString(idReliquidacionViatico));
            //model.AddParameter("Parameter2", namedParameter2);

            return View("ReportViewer", model);
        }
    }
}

