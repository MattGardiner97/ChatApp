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
        private IUserStatusMonitorService _statusMonitorService;

        public ChatHub(ApplicationDbContext AppDbContext, IUserStatusMonitorService UserStatusMonitorService)
        {
            _dbContext = AppDbContext;
            _statusMonitorService = UserStatusMonitorService;
        }

        public override async Task OnConnectedAsync()
        {
            _statusMonitorService.AddOnlineUser(Helpers.GetCurrentUserID(Context.User));
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _statusMonitorService.RemoveAndStoreOnlineUser(Helpers.GetCurrentUserID(Context.User));
            await base.OnDisconnectedAsync(exception);
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

        public async Task GetUserOnlineStatus(string UserID)
        {

        }

    }
}
