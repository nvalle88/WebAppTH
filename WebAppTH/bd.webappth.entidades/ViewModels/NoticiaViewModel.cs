using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
    public class NoticiaViewModel
    {
        public string Titulo { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }


        [DataType(DataType.Url)]
        public string Archivo { get; set; }

        public Noticia Noticia { get; set; }

    }
}
