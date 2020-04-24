using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AshBot.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
    => ReplyAsync(echo);

        [Command("help")]
        [Summary("Help message.")]
        public Task SayHelp()
    => ReplyAsync("!stats - shows your current level and xp\n!judge @user to judge someone\n[Info]\nBe nice n youll get ex pees!");
    }
}
