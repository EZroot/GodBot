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
            string s = "__***Information***__" +
          "\n```diff\n+ Praise the sun! Welcome to church. Here, you gain levels by being a good boy.\n```" +
          "\n```diff\n- [Coming Soon] Role based commands to use against other players and ruin their XP gains.\n```" +
          "\n***__Commands__***" +
          "\n```yaml\n!stats - shows your current level and xp\n!judge @user - to judge someone<cost 20xp >\n```" +
          "\n***__Music Info__***" +
          "\n```fix\n!play songname.mp3 - plays a song\n!stop - stops playing music\n!minsound - min volume\n!maxsound - max volume\n!normalsound - normal volume\n```" +
          "\n***__Songs__***\n";

            string[] list = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "usermusic"));

            s += "```css";
            s += String.Join("\n- ", list.Select(file => Path.GetFileName(file)).ToArray());
            s += "\n```";
            await ReplyAsync(s);
        }
    }
}