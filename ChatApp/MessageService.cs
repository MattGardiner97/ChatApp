using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Data;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;


namespace ChatApp
{
    public class MessageService
    {
        private ApplicationDbContext _dbContext;

        public MessageService(ApplicationDbContext DBContext)
        {
            _dbContext = DBContext;
        }

        public async Task<string> SendMessage(int ConversationID, int SenderID, string Contents)
        {
            Message newMessage = new Message()
            {
                ConversationID = ConversationID,
                SenderID = SenderID,
                Contents = Contents,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.Messages.Add(newMessage);
            _dbContext.SaveChanges();
            return "";
        }

        public async Task<Message[]> GetMessages(int ConversationID, DateTime BeforeTime = default)
        {
            if (BeforeTime == default)
                BeforeTime = DateTime.UtcNow;

            var query = _dbContext.Messages.Where(msg => msg.ConversationID == ConversationID && msg.Timestamp < BeforeTime).Take(10).AsNoTracking();
            var result = await query.ToArrayAsync();

            return result;
        }


    }
}
