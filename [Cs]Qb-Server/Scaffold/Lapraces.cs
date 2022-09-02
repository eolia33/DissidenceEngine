using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class Lapraces
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Checkpoints { get; set; }
        public string Records { get; set; }
        public string Creator { get; set; }
        public int? Distance { get; set; }
        public string Raceid { get; set; }
    }
}
