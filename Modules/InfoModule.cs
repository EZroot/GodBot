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
            string s = "> ***__MOTD__***\n> Welcome to ***Church Of Sun***.\n> Here, you gain levels by being a good, nice boy." +
          "\n\n***__More Help__***" +
          "\n```yaml\n!motd - message of the day\n!stats - shows your current level and xp\n!commands - shows commands you can use against other people\n!music - shows a list of available music commands\n!songs - shows a list of available songs to stream\n```"
            + "\n ***__Bot Info__***\n" +
           "```diff\n+ [Added] Audio streaming, Role-based commands for Angel/ArchAngel(s)\n```" +
           "```diff\n- [Coming Soon] Role based commands to use against other players and ruin their XP gains.\n```";

            await ReplyAsync(s);
        }

        [Command("motd")]
        public async Task MOTDCommand()
        {
            string s = "> ***__MOTD__***\n> Welcome to __***Church Of Sun***__.\n> Here, you gain levels by being a good, nice boy.";
            await ReplyAsync(s);
        }

        [Command("commands")]
        public async Task Commands()
        {
            string s = "***__Commands__***" +
            "\n```yaml\n!stats - shows your current level and xp\n!slap @user - slap a noob <100xp>\n!heal @user - give healing <100xp>\n!judge @user - to judge someone <200xp>\n!sacrifice @user - sacrifice souls for the greater good <200xp>\n```";
            await ReplyAsync(s);
        }

        [Command("musiccommands")]
        public async Task MusicCommands()
        {
            string s = "***__Music Info__***" +
          "\n```fix\n!play songname.mp3 - plays a song\n!stop - stops playing music\n!minsound - min volume\n!maxsound - max volume\n!normalsound - normal volume\n```";
            await ReplyAsync(s);
        }

        [Command("music")]
        public async Task Musics()
        {
            string s = "***__Music Info__***" +
          "\n```fix\n!play songname.mp3 - plays a song\n!stop - stops playing music\n!minsound - min volume\n!maxsound - max volume\n!normalsound - normal volume\n```"+
          "\n***__Songs__***";

            string[] list = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "usermusic"));

            s += "\n```css\n";
            s += String.Join("\n- ", list.Select(file => Path.GetFileName(file)).ToArray());
            s += "```";
            await ReplyAsync(s);
        }

        [Command("songs")]
        public async Task SongList()
        {
            string s = "***__Songs__***";

            string[] list = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "usermusic"));

            s += "\n```css\n";
            s += String.Join("\n- ", list.Select(file => Path.GetFileName(file)).ToArray());
            s += "```";
            await ReplyAsync(s);
        }
    }
}