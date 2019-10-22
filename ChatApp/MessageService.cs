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

        public async Task<string> SendMessage(int SenderID, int RecipientID, string Contents)
        {
            Message newMessage = new Message()
            {
                SenderID = SenderID,
                RecipientID = RecipientID,
                Contents = Contents,
                Timestamp = DateTime.UtcNow
            };

            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();
            return "";
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

        //Gets messages which were sent while the user was offline
        public async Task<Message[]> GetAllRecentMessages(int UserID)
        {
            DateTime lastMessageCheckTime = await _dbContext.Users.Where(user => user.Id == UserID).Select(x => x.LastMessageCheckTime).FirstOrDefaultAsync();

            var query = _dbContext.Messages.Where(msg => msg.Timestamp > lastMessageCheckTime && (msg.RecipientID == UserID || msg.SenderID == UserID)).AsNoTracking();
            var result = await query.ToArrayAsync();

            await SetLastMessageCheckTime(UserID);

            return result;
        }

        public async Task<Message[]> GetMessagesBetweenUsers(int UserID, int FriendID, DateTime BeforeTime = default)
        {
            if (BeforeTime == default)
                BeforeTime = DateTime.UtcNow;

            var query = _dbContext.Messages
                .Where(msg => msg.Timestamp < BeforeTime && ((msg.SenderID == UserID && msg.RecipientID == FriendID) || (msg.SenderID == FriendID && msg.RecipientID == UserID)))
                .AsNoTracking();

            Message[] result = await query.ToArrayAsync();
            await SetLastMessageCheckTime(UserID);
            return result;
        }

        public async Task<Message[]> GetNewMessages(int UserID, DateTime AfterTime)
        {
            var query = _dbContext.Messages
                .Where(msg => msg.Timestamp > AfterTime && msg.RecipientID == UserID)
                .AsNoTracking();
            Message[] result = await query.ToArrayAsync();

            await SetLastMessageCheckTime(UserID);

            return result;
        }


    }
}
