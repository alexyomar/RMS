using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS.Models
{

    public partial class Reservation
    {

    }

    public class ReservationModel
    {

        public Reservation Trip { get; set; }
        public List<Ocupacion> Rooms { get; set; }

    }

    public class Ocupacion
    {


        public int Adultos { get; set; }
        public int Infantes { get; set; }
        public int IdRoom { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }

    }
}