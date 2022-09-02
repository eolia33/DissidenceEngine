using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CsCoreServer.Scaffold
{
    public partial class PhoneTweets
    {
        public int Id { get; set; }
        public string Citizenid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Message { get; set; }
        public DateTime? Date { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string TweetId { get; set; }
    }
}
