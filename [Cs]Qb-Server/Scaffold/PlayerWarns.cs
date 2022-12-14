using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class PlayerWarns
    {
        public int Id { get; set; }
        public string SenderIdentifier { get; set; }
        public string TargetIdentifier { get; set; }
        public string Reason { get; set; }
        public string WarnId { get; set; }
    }
}
