using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        public class Friend
        {
            public int ID;
        }

        private UserManager<ChatUser> _userManager;

        public FriendsController(UserManager<ChatUser> UserManager)
        {
            _userManager = UserManager;
        }

        public ChatUser[] GetFriends()
        {
            return _userManager.Users.ToArray().;
        }

    }
}
