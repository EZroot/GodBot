using AshBot.Data;
using AshBot.Services;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshBot.Modules
{
    public class JudgementModule : ModuleBase<SocketCommandContext>
    {
        Random rand = new Random();

        [Command("stats")]
        [Summary("Check your stats.")]
        public async Task CheckStats()
        {
            UserData userData = JudgementService._activeUserList.Single(item => item.Id == Context.User.Id);
            await ReplyAsync(":exclamation:" + userData.Username + ":exclamation:\n.Level: " + userData.Level + "\n.XP:    " + userData.Xp+"\n.Goal:"+userData.XpTillLevel);
        }

        [Command("judge")]
        [Summary("Judge em.")]
        public async Task JudgeUser(SocketUser user = null)
        {
            string result = "";
            if (user == null)
                user = Context.User;

            UserData userData = JudgementService._activeUserList.Single(item => item.Id == user.Id);
            result = "You shall be JUDGED " + userData.Username + "! By the power of Runescape, I smite you for " + (userData.Level * 9 + rand.Next(userData.Level + 4)) + "\n dmg. Your xp is now " + userData.Xp + ".";
            await ReplyAsync(result);
        }
    }
}