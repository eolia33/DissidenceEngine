using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class Okokbilling
    {
        public int Id { get; set; }
        public string ToId { get; set; }
        public string ToName { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string Society { get; set; }
        public string SocietyName { get; set; }
        public string Item { get; set; }
        public int Value { get; set; }
        public int TaxValue { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }
        public string SentDate { get; set; }
        public string LimitPayDate { get; set; }
        public string PaidDate { get; set; }
    }
}
