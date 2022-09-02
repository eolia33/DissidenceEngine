using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class PlayerOutfits
    {
        public int Id { get; set; }
        public string Citizenid { get; set; }
        public string Outfitname { get; set; }
        public string Model { get; set; }
        public string Skin { get; set; }
        public string OutfitId { get; set; }
    }
}
