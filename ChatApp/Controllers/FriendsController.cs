using ChatApp.Data;
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
            public int ID { get; set; }
            public string Username { get; set; }
            public DateTime LastActive { get; set; }
        }

        private UserManager<ChatUser> _userManager;
        private ApplicationDbContext _dbContext;

        public FriendsController(UserManager<ChatUser> UserManager,ApplicationDbContext DbContext)
        {
            _userManager = UserManager;
            _dbContext = DbContext;
        }

        public async Task<Friend[]> GetFriends()
        {
            ChatUser currentUser = await _userManager.GetUserAsync(User);
            IEnumerable<ChatUser> friends = _userManager.Users.Where(x => currentUser.FriendIDs.Contains(x.Id));
            Friend[] result = friends.Select(x => new Friend { ID = x.Id, Username = x.UserName, LastActive = x.LastActive }).ToArray();

            
            return result;
        }

        public async Task<string> Add(string Username)
        {
            ChatUser user = await _userManager.FindByNameAsync(Username);
            if (user == null)
                return "User does not exist";

            ChatUser currentUser = await _userManager.GetUserAsync(User);
            currentUser.FriendIDs.Add(user.Id);
            await _dbContext.SaveChangesAsync();


            return "";
        }

    }
}
