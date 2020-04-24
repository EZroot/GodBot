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
        float xpTillLevel;

        public string Username { get => username; set => username = value; }
        public string Role { get => role; set => role = value; }
        public int Level { get => level; set => level = value; }
        public int Xp { get => xp; set => xp = value; }
        public float XpTillLevel { get => xpTillLevel; set => xpTillLevel = value; }
        public ulong Id { get => id; set => id = value; }

        public UserData(ulong id, string username, string role, int level, int xp, float xpTillLevel)
        {
            this.id = id;
            this.username = username;
            this.role = role;
            this.level = level;
            this.xp = xp;
            this.xpTillLevel = xpTillLevel;
        }

        public void AddXP(int xp)
        {
            this.xp += xp;
            if(this.xp > this.xpTillLevel)
            {
                Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("hh:mm:ss")} [UserData] <"+this.username+"> is now level "+this.level+"!");
                this.level++;
                this.xpTillLevel *= 1.6f;
            }
            Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("hh:mm:ss")} [UserData] <" + this.username + "> gained " + xp+"XP! Now at "+this.xp);
        }
    }
}
