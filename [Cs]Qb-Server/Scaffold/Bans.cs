using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class Bans
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string License { get; set; }
        public string Discord { get; set; }
        public string Ip { get; set; }
        public string Reason { get; set; }
        public int? Expire { get; set; }
        public string Bannedby { get; set; }
    }
}
