using ChatApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Models;

namespace ChatApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private ApplicationDbContext _dbContext;

        public ChatHub(ApplicationDbContext AppDbContext)
        {
            _dbContext = AppDbContext;
        }

        public async Task SendMessage(string recipientID, string contents)
        {
            var senderID = Helpers.GetCurrentUserID(Context.User);
            Message newMessage = new Message()
            {
                SenderID = senderID,
                RecipientID = int.Parse(recipientID),
                Contents = contents,
                Timestamp = DateTime.UtcNow
            };
            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();


            await Clients.Users(senderID.ToString(), newMessage.RecipientID.ToString()).SendAsync("ReceiveMessage", newMessage);
        }

    }
}
