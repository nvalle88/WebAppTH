
using bd.webappth.entidades.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bd.webappth.entidades.ViewModels
{

    public partial class TipoAccionPersonalViewModel
    {

        public TipoAccionPersonal TipoAccionPersonal { get; set; }

        //Propiedades Virtuales Referencias a otras clases

        public virtual ICollection<AccionPersonal> AccionPersonal { get; set; }

        public List<Matriz> MatrizLista { get; set; }

    }
}
