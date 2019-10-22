using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ChatApp.Models
{
    public class ChatUser : IdentityUser<int>
    {
        public DateTime LastActive { get; set; }
        public DateTime LastMessageCheckTime { get; set; }
    }
}
