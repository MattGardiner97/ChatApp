using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp
{
    public interface IUserStatusMonitorService
    {
        public Task<DateTime> GetUserStatus(int UserID);
        public void AddOnlineUser(int UserID);
        public Task RemoveAndStoreOnlineUser(int UserID);

    }
}
