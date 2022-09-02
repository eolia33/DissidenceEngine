using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class HousePlants
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public string Stage { get; set; }
        public string Sort { get; set; }
        public string Gender { get; set; }
        public int? Food { get; set; }
        public int? Health { get; set; }
        public int? Progress { get; set; }
        public string Coords { get; set; }
        public string Plantid { get; set; }
    }
}
