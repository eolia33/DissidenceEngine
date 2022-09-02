using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class CryptoTransactions
    {
        public int Id { get; set; }
        public string Citizenid { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime? Date { get; set; }
    }
}
