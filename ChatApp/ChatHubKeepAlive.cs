using ChatApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatApp
{
    public class ChatHubKeepAlive : BackgroundService
    {
        private IHubContext<ChatHub> _hubContext;
        public ChatHubKeepAlive(IHubContext<ChatHub> HubContext)
        {
            _hubContext = HubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await _hubContext.Clients.All.SendAsync("KeepAlive", stoppingToken);
                await Task.Delay(30000);
            }
        }
    }
}
