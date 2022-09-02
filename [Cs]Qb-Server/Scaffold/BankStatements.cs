using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class BankStatements
    {
        public long RecordId { get; set; }
        public string Citizenid { get; set; }
        public string Account { get; set; }
        public string Business { get; set; }
        public int? Businessid { get; set; }
        public string Gangid { get; set; }
        public int? Deposited { get; set; }
        public int? Withdraw { get; set; }
        public int? Balance { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
    }
}
