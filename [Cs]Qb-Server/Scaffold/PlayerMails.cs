using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class PlayerMails
    {
        public int Id { get; set; }
        public string Citizenid { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public sbyte? Read { get; set; }
        public int? Mailid { get; set; }
        public DateTime? Date { get; set; }
        public string Button { get; set; }
    }
}
