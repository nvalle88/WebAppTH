using System;
using System.Collections.Generic;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class FichaOdontologicaViewModel
    {
        public int IdPersona { get; set; }
        public string Url { get; set; }

        /* Campos para el fichero*/

        public string Descripcion { get; set; }
        public string Extension { get; set; }
        public byte[] Fichero { get; set; }
    }
}
