using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class Message
    {
        public int ID { get; set; }
        public int RecipientID { get; set; }
        public int SenderID { get; set; }
        public string Contents { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
