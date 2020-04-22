using System;
using System.Collections.Generic;
using System.Text;

namespace AshBot.Data
{
    public class UserData
    {
        string username;
        string role;
        int level;
        int xp;
        int xpTillLevel;

        public UserData(string username, string role, int level, int xp, int xpTillLevel)
        {
            this.username = username;
            this.role = role;
            this.level = level;
            this.xp = xp;
            this.xpTillLevel = xpTillLevel;
        }
    }
}
