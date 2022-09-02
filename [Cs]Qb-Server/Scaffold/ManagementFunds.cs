using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class ManagementFunds
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
    }
}
