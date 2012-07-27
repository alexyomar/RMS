using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RMS.Models
{
    [MetadataType(typeof(PromoModel))]
    public partial class Promotion
    {
    }
    public class PromoModel
    {
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Valor inválido."), Required(ErrorMessage = "Debe indicar este valor."), DisplayName("Precio Temporada Baja")]
        public decimal LowSeasonPrice { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Valor inválido."), Required(ErrorMessage = "Debe indicar este valor."), DisplayName("Precio Temporada Alta")]
        public decimal HighSeasonPrice { get; set; }

        [Required(ErrorMessage = "Debe indicar este valor."), DisplayName("Válido desde")]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = "Debe indicar este valor."), DisplayName("Válido hasta")]
        public DateTime DateEnd { get; set; }

        [DisplayName("Activo")]
        public Boolean Active { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Valor inválido."),Required(ErrorMessage = "Debe indicar este valor."), DisplayName("Minimo de Adultos")]
        public int MinAdults { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Valor inválido."), Required(ErrorMessage = "Debe indicar este valor."), DisplayName("Minimo de Noches")]
        public int MinDays { get; set; }

    }
}
