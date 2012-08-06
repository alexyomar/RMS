using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;

namespace RMS.Models
{
    [MetadataType(typeof(RoomModel))]
    public partial class Room
    {
    }

    public class RoomModel
    {
        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("Capacidad")]
        public string Capacity { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Valor inválido."), Required(ErrorMessage = "Debe indicar este valor."), DisplayName("Precio Temporada Baja")]
        public decimal LowSeasonPrice { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "Valor inválido."), Required(ErrorMessage = "Debe indicar este valor."), DisplayName("Precio Temporada Alta")]
        public decimal HighSeasonPrice { get; set; }

        [DisplayName("% Descuento 3 Noches")]
        [Range(0.0, 100, ErrorMessage = "El valor debe ser de 0 a 100.")]
        public decimal Discount1 { get; set; }

        [DisplayName("% Descuento 5 Noches")]
        [Range(0.0, 100, ErrorMessage = "El valor debe ser de 0 a 100.")]
        public decimal Discount2 { get; set; }

        [DisplayName("% Descuento 7 Noches")]
        [Range(0.0, 100, ErrorMessage = "El valor debe ser de 0 a 100.")]
        public decimal Discount3 { get; set; }

        [Range(0.0, 100, ErrorMessage = "El valor debe ser de 0 a 100.")]
        public decimal Discount4 { get; set; }


    }

}