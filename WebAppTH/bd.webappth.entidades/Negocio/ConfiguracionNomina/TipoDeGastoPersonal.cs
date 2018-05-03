using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Negocio
{
   public class TipoDeGastoPersonal
    {
        [Key]
        public int IdTipoGastoPersonal { get; set; }

        public string Descripcion { get; set; }

        public string NombreConstante { get; set; }

        public virtual ICollection<GastoPersonal> GastoPersonal { get; set; }

    }
}
