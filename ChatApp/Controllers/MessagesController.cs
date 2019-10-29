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
        public async Task<Message> SendMessage(int FriendID, string Message)
        {
            int senderID = Helpers.GetCurrentUserID(User);
            Message result = await _messageService.SendMessage(senderID, FriendID, Message);
            return result;
        }

        [HttpGet]
        public async Task<Message[]> GetFriendMessagesBeforeTime(int FriendID, DateTime Before = default)
        {
            int userID = Helpers.GetCurrentUserID(User);
            Message[] result = await _messageService.GetMessagesBeforeTime(userID,FriendID,Before);
            return result;
        }

        [HttpGet]
        public async Task<Message[]> GetMessagesAfterTime(DateTimeOffset After)
        {
            int userID = Helpers.GetCurrentUserID(User);
            var result = await _messageService.GetMessagesAfterTime(userID, After.UtcDateTime);
            return result;
        }
    }
}