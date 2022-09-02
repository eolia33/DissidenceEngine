using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class Dealers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Coords { get; set; }
        public string Time { get; set; }
        public string Createdby { get; set; }
    }
}
