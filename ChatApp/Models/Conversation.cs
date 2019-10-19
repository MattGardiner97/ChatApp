using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class Conversation
    {
        public int ID { get; set; }
        public int User1ID { get; set; }
        public int User2ID { get; set; }
        public int User1LastReadMessage { get; set; }
        public int User2LastReadMessage { get; set; }


    }
}
