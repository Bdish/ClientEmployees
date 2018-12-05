using System;
using System.Collections.Generic;
using System.Text;

namespace AccessToWebApi.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string HomeTelephone { get; set; }
        public string MobileTelephone { get; set; }
    }
}
