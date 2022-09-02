using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class Players
    {
        public int Id { get; set; }
        public string Citizenid { get; set; }
        public int? Cid { get; set; }
        public string License { get; set; }
        public string Name { get; set; }
        public string Money { get; set; }
        public string Charinfo { get; set; }
        public string Job { get; set; }
        public string Gang { get; set; }
        public string Position { get; set; }
        public string Metadata { get; set; }
        public string Inventory { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
