using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class Friendship
    {
        public int User1ID { get; set; }
        public ChatUser User1 { get; set; }
        public int user2ID { get; set; }
        public ChatUser User2 { get; set; }

    }
}
