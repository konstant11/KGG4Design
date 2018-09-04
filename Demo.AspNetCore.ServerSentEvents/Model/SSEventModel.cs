using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.AspNetCore.ServerSentEvents.Model
{
    public class SSEventModel
    {
        public int sequance_id { get; set; }
        public string cti_event { get; set; }
        public string CallingDN { get; set; }
        public string data { get; set; }
        public string callid { get; set; }
        public string selectedDN { get; set; }
        public List<Contact> Contacts { get; set; }
    }
    public class Contact
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
