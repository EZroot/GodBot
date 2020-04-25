using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public async Task SayHelp()
        {
            string s = "[Info]" +
          "\n!stats - shows your current level and xp" +
          "\n!judge @user - to judge someone <cost 20xp>" +
          "\n-------------------------------------------------" +
          "\n[Music Info]" +
          "\n!play songname.mp3 - plays a song" +
          "\n!stop - stops playing music" +
          "\n-------------------------------------------------" +
          "\n[Songs]\n- ";

            string[] list = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "usermusic"));

            s += String.Join("\n- ", list.Select(file => Path.GetFileName(file)).ToArray());
            await ReplyAsync(s);
        }
    }
}
