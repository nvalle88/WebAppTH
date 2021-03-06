﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.ViewModels
{
   public class ConocimientosAdicionalesViewModel
    {

        public int IdConocimientosAdicionales { get; set; }

        public int IdIndiceOcupacional { get; set; }

        [Required(ErrorMessage = "Debe introducir {0}")]
        [Display(Name = "Conocimiento adicional:")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "El {0} no puede tener más de {1} y menos de {2}")]
        public string Descripcion { get; set; }
    }
}
