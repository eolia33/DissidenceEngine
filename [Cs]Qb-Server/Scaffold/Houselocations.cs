using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class Houselocations
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Coords { get; set; }
        public bool? Owned { get; set; }
        public int? Price { get; set; }
        public sbyte? Tier { get; set; }
        public string Garage { get; set; }
    }
}
