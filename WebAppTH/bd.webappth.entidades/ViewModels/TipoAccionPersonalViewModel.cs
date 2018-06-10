
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

        // 0 = n/a || 1 = Modalidad contratación || 2 = desactivar empleado
        public int empleadoCambio { get; set; }

        // Obtiene el valor del radio buton para validar si es definitivo o no
        public string grp_tiempo_minimo { get; set; }

    }
}
