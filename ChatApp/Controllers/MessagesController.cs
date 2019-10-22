using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private MessageService _messageService;
        private ApplicationDbContext _dbContext;

        public MessagesController(MessageService MessageService, ApplicationDbContext DBContext)
        {
            _messageService = MessageService;
            _dbContext = DBContext;
        }

        [HttpPost]
        public async Task<string> SendMessage(int FriendID, string Message)
        {
            int senderID = Helpers.GetCurrentUserID(User);
            string result = await _messageService.SendMessage(senderID, FriendID, Message);
            return "";
        }

        [HttpGet]
        public async Task<Message[]> GetMessages(int FriendID, DateTime Before)
        {
            return null;
        }

        [HttpGet]
        public async Task<Message[]> GetRecentMessages()
        {
            int userID = Helpers.GetCurrentUserID(User);
            Message[] result = await _messageService.GetAllRecentMessages(userID);
            return result;
        }

        [HttpGet]
        public async Task<Message[]> GetNewMessages(DateTime LastCheckTime = default)
        {
            int userID = Helpers.GetCurrentUserID(User);

            if (LastCheckTime == default)
                LastCheckTime = await _dbContext.Users.Where(user => user.Id == userID).AsNoTracking().Select(u => u.LastMessageCheckTime).FirstOrDefaultAsync();


            var result = await _messageService.GetNewMessages(userID, LastCheckTime);
            return result;
        }
    }
}