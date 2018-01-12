using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.webappth.entidades.Utils
{
    public class  ValidacionPersonalizada
    {
        public static ValidationResult ValidarRangoFecha(DateTime fechaInicio ,DateTime fechaFin)
        {
            return Convert.ToInt32(fechaFin.Subtract(fechaInicio)) <=0
                ? new ValidationResult("La fecha de inicio no puede ser mayor ni igual que la fecha final")
                : ValidationResult.Success;
        }

    }
}
