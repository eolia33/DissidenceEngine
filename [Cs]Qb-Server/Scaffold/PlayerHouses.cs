using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class PlayerHouses
    {
        public int Id { get; set; }
        public string House { get; set; }
        public string Identifier { get; set; }
        public string Citizenid { get; set; }
        public string Keyholders { get; set; }
        public string Decorations { get; set; }
        public string Stash { get; set; }
        public string Outfit { get; set; }
        public string Logout { get; set; }
    }
}
