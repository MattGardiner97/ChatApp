using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class Friendship
    {
        public int ID { get; set; }
        public ChatUser Owner { get; set; }
        public ChatUser Friend { get; set; }
        public int OwnerID { get; set; }
        public int FriendID { get; set; }

        public Friendship()
        {

        }

        public Friendship(ChatUser Owner,ChatUser Friend)
        {
            this.Owner = Owner;
            this.Friend = Friend;
        }

        public Friendship(int OwnerID, int FriendID)
        {
            this.OwnerID = OwnerID;
            this.FriendID = FriendID;
        }

    }
}
