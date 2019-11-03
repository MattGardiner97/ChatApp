using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp
{
    public interface IUserStatusStorageService
    {
        public Task StoreUserStatus(int UserID, DateTime LastActive);
        public Task<DateTime> RetrieveUserStatus(int UserID);

    }
}
