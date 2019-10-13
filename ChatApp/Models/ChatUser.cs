using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ChatApp.Models
{
    public class ChatUser : IdentityUser<int>
    {
        //Specifies whether the friends list should be recreated on the next call to GetFriendsList(). Should be set after adding or delete a friend.
        [NotMapped]
        private bool _regenerateFriendList = true;

        [NotMapped]
        private ChatUser[] _friendsList = Array.Empty<ChatUser>();

        [NotMapped]
        public List<int> FriendIDs { get; set; }

        public byte[] FriendIDBinary
        {
            get
            {
                if (FriendIDs == null || FriendIDs.Count == 0)
                    return new byte[0];

                byte[] buffer = new byte[FriendIDs.Count * 4];

                using (BinaryWriter bw = new BinaryWriter(new MemoryStream(buffer)))
                {
                    foreach (int i in FriendIDs)
                    {
                        bw.Write(i);
                    }
                }

                return buffer;
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    FriendIDs = new List<int>();
                    return;
                }

                int[] result = new int[value.Length / 4];

                int index = 0;
                using (BinaryReader br = new BinaryReader(new MemoryStream(value)))
                {
                    result[index++] = br.ReadInt32();
                }
                FriendIDs = new List<int>(result);
            }
        }
        public DateTime LastActive { get; set; }

        private void UpdateFriendsList(IQueryable<ChatUser> AllUsers)
        {
            _friendsList = AllUsers.Where(x => FriendIDs.Contains(x.Id)).ToArray();
        }

        public ChatUser[] GetFriendsList(IQueryable<ChatUser> UserQuery)
        {
            if (_regenerateFriendList == true)
            {
                UpdateFriendsList(UserQuery);
                _regenerateFriendList = false;
            }

            return _friendsList;
        }

        public bool AddFriend(ChatUser NewUser)
        {
            if (FriendIDs.Contains(NewUser.Id))
                return false;

            FriendIDs.Add(NewUser.Id);
            _regenerateFriendList = true;
            return true;
        }
    }
}
