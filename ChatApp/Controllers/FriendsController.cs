using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        public class FriendViewModel
        {
            public int ID { get; set; }
            public string Username { get; set; }
            public DateTime LastActive { get; set; }

            public FriendViewModel(int ID, string Username, DateTime LastActive)
            {
                this.ID = ID;
                this.Username = Username;
                this.LastActive = LastActive;
            }
        }

        private UserManager<ChatUser> _userManager;
        private ApplicationDbContext _dbContext;

        public FriendsController(UserManager<ChatUser> UserManager,ApplicationDbContext DbContext)
        {
            _userManager = UserManager;
            _dbContext = DbContext;
        }

        [HttpGet]
        public async Task<FriendViewModel[]> GetFriends()
        {
            int currentUserID = Helpers.GetCurrentUserID(User);
            var query = _dbContext.Friendships
                .Where(x => x.OwnerID == currentUserID)
                .Select(x => new FriendViewModel(x.FriendID,x.Friend.UserName,x.Friend.LastActive))
                .AsNoTracking();
            return await query.ToArrayAsync();
        }

        [HttpPost]
        public async Task<string> Add(string Username)
        {
            int currentID = Helpers.GetCurrentUserID(User);
            int friendID = await _dbContext.Users.Where(x => x.UserName == Username).Select(x => x.Id).FirstOrDefaultAsync();

            if (friendID == 0)
                return "User does not exist";

            int existingCount = await _dbContext.Friendships.CountAsync(x => x.OwnerID == currentID && x.FriendID == friendID);
            if (existingCount > 0)
                return "User is already on your friend list";

            Friendship ownerFriendship = new Friendship(currentID,friendID);
            Friendship otherFriendship = new Friendship(friendID,currentID);

            _dbContext.Friendships.Add(ownerFriendship);
            _dbContext.Friendships.Add(otherFriendship);
            await _dbContext.SaveChangesAsync();


            return "";
        }

    }
}
