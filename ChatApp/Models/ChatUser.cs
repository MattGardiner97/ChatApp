using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class ChatUser : IdentityUser<int>
    {
        public ICollection<Friendship> Friends { get; set; }

    }
}
 