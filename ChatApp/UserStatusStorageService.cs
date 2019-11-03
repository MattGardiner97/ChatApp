using ChatApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp
{
    public class UserStatusStorageService : IUserStatusStorageService
    {
        private IServiceProvider _serviceProvider;
        public UserStatusStorageService(IServiceProvider ServiceProvider)
        {
            _serviceProvider = ServiceProvider;
        }

        async Task<DateTime> IUserStatusStorageService.RetrieveUserStatus(int UserID)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    var query = context.Users.Where(user => user.Id == UserID).AsNoTracking().Select(user => user.LastActive);
                    return await query.FirstOrDefaultAsync();
                }
            }
        }

        async Task IUserStatusStorageService.StoreUserStatus(int UserID, DateTime LastActive)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    var user = await context.Users.Where(user => user.Id == UserID).FirstOrDefaultAsync();
                    user.LastActive = LastActive;
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
