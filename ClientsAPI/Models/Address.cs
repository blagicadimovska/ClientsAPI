using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsAPI.Models
{
    public class Address
    {
        public int AddressID { get; set; }

        [ForeignKey("Client")]
        public int ClientID { get; set; } // Foreign key to Client
        public int Type { get; set; }
        public string ClientAddress { get; set; }
    }
}
