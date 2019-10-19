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

        [HttpGet]
        public async Task<int> GetConversationID(int UserID)
        {
            int currentID = Helpers.GetCurrentUserID(User);

            int lowID = currentID < UserID ? currentID : UserID;
            int highID = currentID > UserID ? currentID : UserID;

            var result = await _dbContext.Conversations.Where(x => x.User1ID == lowID && x.User2ID == highID).Take(1).AsNoTracking().FirstOrDefaultAsync();
            if (result == null)
            {
                result = new Conversation()
                {
                    User1ID = lowID,
                    User2ID = highID
                };
                _dbContext.Conversations.Add(result);
                await _dbContext.SaveChangesAsync();
            }

            return result.ID;
        }

        [HttpPost]
        public async Task<string> SendMessage(int ConversationID, string Message)
        {
            int senderID = Helpers.GetCurrentUserID(User);
            string result = await _messageService.SendMessage(ConversationID, senderID, Message);
            return result;
        }

        [HttpGet]
        public async Task<Message[]> GetMessages(int ConversationID)
        {
            var result = await _messageService.GetMessages(ConversationID);
            return result;
        }
    }
}