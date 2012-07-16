using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RMS.Models
{
    [MetadataType(typeof(CustomerModel))]
    public partial class Customer
    {
    }

    public class CustomerModel
    {

        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("Apellido")]
        public string LastName { get; set; }

        [DisplayName("Cédula ó R.I.F")]
        public string PersonalId { get; set; }

        [DisplayName("Teléfono")]
        public string Phone { get; set; }

        [DisplayName("Dirección")]
        public string Address { get; set; }

    }
}