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
            string s = "> ***__MOTD__***\n> Welcome to ***Church Of Sun***.\n> Here, you gain levels by being active in church. You get !commands based on your role that you may use against other players." +
          "\n\n***__More Help__***" +
          "\n```bash\n!motd - message of the day\n!stats - shows your current level and xp\n!commands - shows commands you can use against other people\n!music - shows a list of available music commands\n!songs - shows a list of available songs to stream\n```"
            + "\n ***__Bot Info__***\n" +
           "```diff\n+ [Added] Commands for Saved, Pastor, Priest, Angel, Archangle Added!\n```" +
           "```diff\n+ [Added] More Roles!\n```" +
           "```diff\n- [Coming Soon] Better level system, more commands with stronger results.\n```";

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
            "\n```yaml\n!stats - shows your current level and xp\n```";
            await ReplyAsync(s);
            await SavedCmds();
            await PastorCMD();
            await PriestCMD();
            await AngelCMD();
            await ArchAngelCMD();
        }

        [Command("savedhelp")]
        public async Task SavedCmds()
        {
            string s = "***__Saved Commands__***" +
            "\n```yaml\n!belittle @user\n!spread lies about @user\n```";
            await ReplyAsync(s);
        }

        [Command("pastorhelp")]
        public async Task PastorCMD()
        {
            string s = "***__Pastor Commands__***" +
            "\n```yaml\n!chastize @user\n!preach @user\n```";
            await ReplyAsync(s);
        }

        [Command("priesthelp")]
        public async Task PriestCMD()
        {
            string s = "***__Priest Commands__***" +
            "\n```yaml\n!condemn @user\n!forgive @user\n```";
            await ReplyAsync(s);
        }

        [Command("anglehelp")]
        public async Task AngelCMD()
        {
            string s = "***__Angel Commands__***" +
            "\n```yaml\n!slap @user\n!heal @user\n```";
            await ReplyAsync(s);
        }

        [Command("archanglehelp")]
        public async Task ArchAngelCMD()
        {
            string s = "***__ArchAngel Commands__***" +
            "\n```yaml\n!judge @user\n!sacrifice @user\n```";
            await ReplyAsync(s);
        }

        [Command("music")]
        public async Task Musics()
        {
            string s = "***__Music Info__***" +
          "\n```fix\n!songs - list of songs available \n!play songname.mp3 - plays a song\n!stop - stops playing music\n!minsound - min volume\n!maxsound - max volume\n!normalsound - normal volume\n```";

            await ReplyAsync(s);
        }

        [Command("songs")]
        public async Task SongList()
        {
            string s = "***__Songs__***";

            string[] list = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "usermusic"));

            s += "\n```css\n- ";
            s += String.Join("\n- ", list.Select(file => Path.GetFileName(file)).ToArray());
            s += "```";
            await ReplyAsync(s);
        }
    }
}