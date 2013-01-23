using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RMS.Areas.Web.Models
{
    public class CotizacionModel
    {
        public enum __Booleano { Si, No }

        [Display(Name = "Nombre / Empresa")]
        public string Nombre { get; set; }
        [Display(Name = "Cédula / R.I.F. / Pasaporte")]
        public string Cedula { get; set; }
        public string Email { get; set; }
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        [Display(Name = "Teléfono Habitación")]
        public string TelefonoHab { get; set; }
        [Display(Name = "Teléfono Celular")]
        public string TelefonoCel { get; set; }

        [Display(Name = "¿Qué desea reservar?")]
        [DataType(DataType.MultilineText)]
        public string QueReservar { get; set; }
        [Display(Name = "N° de Adultos")]
        public int Adultos { get; set; }
        [Display(Name = "N° de Niños")]
        public int Ninos { get; set; }
        public string Edades { get; set; }
        [Display(Name = "Fecha de Ida")]
        public string Ida { get; set; }
        [Display(Name = "Fecha de Regreso")]
        public string Regreso { get; set; }
        [Display(Name = "Solicitud de Traslado")]
        public __Booleano Traslado { get; set; }
        [Display(Name = "Solicitud de Boleto Áereo / Marítimo")]
        public __Booleano Boleto { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comentarios { get; set; }

        public int A { get; set; }
        public int AA { get; set; }
        public int AAA { get; set; }
        public int AAAA { get; set; }
        public int AN { get; set; }
        public int ANN { get; set; }
        public int AAN { get; set; }
        public int AANN { get; set; }
        public int AAAN { get; set; }



    }
}