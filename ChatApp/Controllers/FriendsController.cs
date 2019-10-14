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
        public class Friend
        {
            public int ID { get; set; }
            public string Username { get; set; }
            public DateTime LastActive { get; set; }

            public Friend(int ID, string Username, DateTime LastActive)
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

        public async Task<Friend[]> GetFriends()
        {
            int currentUserID = GetCurrentUserID();
            var result = _dbContext.Users.Include(user => user.Friendships).ThenInclude(fs => fs.Friend).Single(x => x.Id == currentUserID);
            return result.Friendships.Select(x => new Friend(x.FriendID, x.Friend.UserName, x.Friend.LastActive)).ToArray();
        }

        public async Task<string> Add(string Username)
        {
            ChatUser owner = await _userManager.GetUserAsync(User);

            ChatUser friend = await _userManager.FindByNameAsync(Username);
            if (friend == null)
                return "User does not exist";

            Friendship ownerFriendship = new Friendship(owner, friend);
            Friendship otherFriendship = new Friendship(friend, owner);

            _dbContext.Friendships.Add(ownerFriendship);
            _dbContext.Friendships.Add(otherFriendship);
            await _dbContext.SaveChangesAsync();


            return "";
        }

        private int GetCurrentUserID()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

    }
}
