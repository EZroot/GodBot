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
        [Command("stats")]
        [Summary("Check your stats.")]
        public async Task CheckStats()
        {
            UserData userData = JudgementService._activeUserList.Single(item => item.Id == Context.User.Id);
            await ReplyAsync(":exclamation:" + userData.Username + ":exclamation:\n.Level: " + userData.Level + "\n.XP:    " + userData.Xp+"\n.Goal:"+userData.XpTillLevel);
        }
    }
}