using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ChatApp
{
    public class UserStatusMonitorService : IUserStatusMonitorService
    {
        //DI variables
        private IUserStatusStorageService _storageService;

        //Storage container
        private ConcurrentDictionary<int, DateTime> _onlineUsers;

        public UserStatusMonitorService(IUserStatusStorageService StorageService)
        {
            _storageService = StorageService;

            _onlineUsers = new ConcurrentDictionary<int, DateTime>();
        }

        public void AddOnlineUser(int UserID)
        {
            if (_onlineUsers.ContainsKey(UserID) == false)
                _onlineUsers.TryAdd(UserID, DateTime.UtcNow);
        }

        public async Task<DateTime> GetUserStatus(int UserID)
        {
            if (_onlineUsers.ContainsKey(UserID))
                return new DateTime(0);

            return await _storageService.RetrieveUserStatus(UserID);
        }

        public async Task RemoveAndStoreOnlineUser(int UserID)
        {
            DateTime _ = new DateTime();
            _onlineUsers.TryRemove(UserID, out _);
            _storageService.StoreUserStatus(UserID, DateTime.UtcNow);
        }
    }
}
