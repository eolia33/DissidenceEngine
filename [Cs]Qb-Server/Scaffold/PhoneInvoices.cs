using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class PhoneInvoices
    {
        public int Id { get; set; }
        public string Citizenid { get; set; }
        public int Amount { get; set; }
        public string Society { get; set; }
        public string Sender { get; set; }
        public string Sendercitizenid { get; set; }
    }
}
