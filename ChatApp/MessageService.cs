using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Data;
using ChatApp.Models;
using Microsoft.Data.SqlClient;
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

        public async Task<Message> SendMessage(int SenderID, int RecipientID, string Contents)
        {
            if (string.IsNullOrEmpty(Contents))
                return null;

            int friendshipCount = await _dbContext.Friendships.Where(fs => fs.OwnerID == SenderID && fs.FriendID == RecipientID).CountAsync();
            if (friendshipCount == 0)
                return null;

            Message newMessage = new Message()
            {
                SenderID = SenderID,
                RecipientID = RecipientID,
                Contents = Contents,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();
            return newMessage;
        }

        private async Task SetLastMessageCheckTime(int UserID)
        {
            //EF Core doesn't provide a way to update a record without first retrieving it, so we use raw SQL to update
            await _dbContext.Database.ExecuteSqlCommandAsync("UPDATE AspNetUsers SET LastMessageCheckTime = @time WHERE Id = @id",
                new SqlParameter[]{
                    new SqlParameter("time", DateTime.UtcNow),
                    new SqlParameter("id", UserID)
                    });
        }

        public async Task<Message[]> GetMessagesBeforeTime(int UserID, int FriendID, DateTime Before = default)
        {
            if (Before == default)
                Before = DateTime.UtcNow;

            var query = _dbContext.Messages
                .Where(msg => msg.Timestamp < Before && ((msg.SenderID == UserID && msg.RecipientID == FriendID) || (msg.SenderID == FriendID && msg.RecipientID == UserID)))
                .OrderByDescending(msg => msg.Timestamp)
                .Take(20)
                .AsNoTracking();

            await SetLastMessageCheckTime(UserID);

            Message[] result = await query.ToArrayAsync();
            return result;
        }

        public async Task<Message[]> GetMessagesAfterTime(int UserID, DateTime After)
        {
            if (After == default)
                After = await _dbContext.Users
                    .Where(user => user.Id == UserID)
                    .AsNoTracking()
                    .Select(u => u.LastMessageCheckTime)
                    .FirstOrDefaultAsync();

            await SetLastMessageCheckTime(UserID);

            var query = _dbContext.Messages
                .Where(msg => msg.Timestamp > After && msg.RecipientID == UserID)
                .AsNoTracking();
            Message[] result = await query.ToArrayAsync();


            return result;
        }


    }
}
