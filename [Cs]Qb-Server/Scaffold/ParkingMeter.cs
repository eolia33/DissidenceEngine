using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class ParkingMeter
    {
        public string Identifier { get; set; }
        public string Plate { get; set; }
        public string Vehicle { get; set; }
        public string Coord { get; set; }
        public string ParkCoord { get; set; }
    }
}
