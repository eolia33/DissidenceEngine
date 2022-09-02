using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class OccasionVehicles
    {
        public int Id { get; set; }
        public string Seller { get; set; }
        public int? Price { get; set; }
        public string Description { get; set; }
        public string Plate { get; set; }
        public string Model { get; set; }
        public string Mods { get; set; }
        public string Occasionid { get; set; }
    }
}
