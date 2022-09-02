using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class PlayerVehicles
    {
        public int Id { get; set; }
        public string License { get; set; }
        public string Citizenid { get; set; }
        public string Vehicle { get; set; }
        public string Hash { get; set; }
        public string Mods { get; set; }
        public string Plate { get; set; }
        public string Fakeplate { get; set; }
        public string Garage { get; set; }
        public int? Fuel { get; set; }
        public float? Engine { get; set; }
        public float? Body { get; set; }
        public int? State { get; set; }
        public int Depotprice { get; set; }
        public int? Drivingdistance { get; set; }
        public string Status { get; set; }
        public int Balance { get; set; }
        public int Paymentamount { get; set; }
        public int Paymentsleft { get; set; }
        public int Financetime { get; set; }
        public int Impound { get; set; }
        public string Type { get; set; }
        public string Job { get; set; }
        public string ParkCoord { get; set; }
        public int? Isparked { get; set; }
    }
}
