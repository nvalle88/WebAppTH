using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.Utils
{
   public static class ReportConfig
    {
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string ReportServerUrl { get; set; }
        public static string ReportFolderPath { get; set; }
        public static bool DefaultNetworkCredentials { get; set; }
        public static string CustomDomain { get; set; }
    }
}
