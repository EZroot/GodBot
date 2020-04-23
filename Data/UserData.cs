using System;
using System.Collections.Generic;
using System.Text;

namespace AshBot.Data
{
    public class UserData
    {
        ulong id;
        string username;
        string role;
        int level;
        int xp;
        int xpTillLevel;

        public string Username { get => username; set => username = value; }
        public string Role { get => role; set => role = value; }
        public int Level { get => level; set => level = value; }
        public int Xp { get => xp; set => xp = value; }
        public int XpTillLevel { get => xpTillLevel; set => xpTillLevel = value; }
        public ulong Id { get => id; set => id = value; }

        public UserData(ulong id, string username, string role, int level, int xp, int xpTillLevel)
        {
            this.id = id;
            this.username = username;
            this.role = role;
            this.level = level;
            this.xp = xp;
            this.xpTillLevel = xpTillLevel;
        }
    }
}
