using AshBot.Data;
using AshBot.Services;
using Discord;
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
        private readonly string[] JUDGEMENT_ROLES = { "Satan", "Archdemon", "Demon", "Unrepented", "Repented", "Angel", "Archangel", "DemiGod" };

        Random rand = new Random();

        [Command("stats")]
        [Summary("Check your stats.")]
        public async Task CheckStats()
        {
            UserData userData = JudgementService._activeUserList.Single(item => item.Id == Context.User.Id);
            await ReplyAsync("> __*User Stats -* ***" + userData.Username + "***__" +
                "\n> __.[Level]__ ***" + userData.Level + "***" +
                "\n> __.[Exp]__ ***" + userData.Xp + "***"+
                "\n> __.[Rank]__ ***" +userData.GetRole()+"***");
        }

        #region AngelCommands
        [Command("slap")]
        public async Task SlapUser(SocketGuildUser user = null)
        {
            var authorUser = Context.User;
            string result = "Not an Angel. Use !help noob.";

            if (user == null)
            {
                await ReplyAsync("Nobody to slap.");
                return;
            }

            UserData userData = JudgementService._activeUserList.Single(item => item.Id == user.Id);
            UserData authorData = JudgementService._activeUserList.Single(item => item.Id == authorUser.Id);

            //make sure author is angle or above
            if (authorData.Level >= 2)
            {
                authorData.LoseXP(100);
                userData.LoseXP((int)Math.Abs(userData.Level * 0.2f) + 25);

                result = "Slappin' " + userData.Username +
                    " around like the noob he is." + ((int)Math.Abs(userData.Level * 0.2f) + 25) +
                    " dmg.\n Their XP is now " + userData.Xp + ".\n" + authorData.Username + " lost 20XP!";
            }
            //Reply in chat
            await ReplyAsync(result);
        }

        [Command("heal")]
        public async Task HealUser(SocketGuildUser user = null)
        {
            var authorUser = Context.User;
            string result = "Not an Angel. Use !help noob.";

            if (user == null)
            {
                await ReplyAsync("Nobody to heal.");
                return;
            }

            UserData userData = JudgementService._activeUserList.Single(item => item.Id == user.Id);
            UserData authorData = JudgementService._activeUserList.Single(item => item.Id == authorUser.Id);

            //make sure author is angle or above
            if (authorData.Level >= 2)
            {
                authorData.LoseXP(100);
                userData.AddXP((int)Math.Abs(userData.Level * 0.2f) + 25);

                result = "Healed " + userData.Username +
                    " for" + ((int)Math.Abs(userData.Level * 0.2f) + 25) +
                    "XP!\n Their XP is now " + userData.Xp + ".\n" + authorData.Username + " sacrificed 100XP!";
            }
            //Reply in chat
            await ReplyAsync(result);
        }
#endregion

        #region ArchAngelCommands
        [Command("judge")]
        public async Task JudgeUser(SocketGuildUser user = null)
        {
            var authorUser = Context.User;
            string result = "Not an ArchAngel. Use !help noob.";

            if (user == null)
            {
                await ReplyAsync("Nobody to Judge.");
                return;
            }

            UserData userData = JudgementService._activeUserList.Single(item => item.Id == user.Id);
            UserData authorData = JudgementService._activeUserList.Single(item => item.Id == authorUser.Id);

            //make sure author is angle or above
            if (authorData.Level >= 1337)
            {
                authorData.LoseXP(200);
                userData.LoseXP((int)Math.Abs(userData.Level * 0.2f) + 125);

                result = "You shall be JUDGED " + userData.Username +
                    "!\nYou owe the gods " + ((int)Math.Abs(userData.Level * 0.2f) + 125) +
                    " XP.\n Your XP is now " + userData.Xp + ".\n" + authorData.Username + " lost 100XP!";
            }
            //Reply in chat
            await ReplyAsync(result);
        }

        [Command("sacrifice")]
        public async Task SacrificeUser(SocketGuildUser user = null)
        {
            var authorUser = Context.User;
            string result = "Not an ArchAngel. Use !help noob.";

            if (user == null)
            {
                await ReplyAsync("Nobody to heal.");
                return;
            }

            UserData userData = JudgementService._activeUserList.Single(item => item.Id == user.Id);
            UserData authorData = JudgementService._activeUserList.Single(item => item.Id == authorUser.Id);

            //make sure author is angle or above
            if (authorData.Level >= 1337)
            {
                authorData.LoseXP(200);
                userData.AddXP((int)Math.Abs(userData.Level * 0.2f) + 125);

                result = "Sacrificed self to heal " + userData.Username +
                    " for" + ((int)Math.Abs(userData.Level * 0.2f) + 125) +
                    "XP!\n Their XP is now " + userData.Xp + ".\n" + authorData.Username + " lost 100XP!";
            }
            //Reply in chat
            await ReplyAsync(result);
        }
        #endregion

        #region RoleControls
        /// <summary>
        /// Update the user roles based on JUDGEMENT_ROLES[index]
        /// </summary>
        /// <param name="user"></param>
        /// <param name="judgementRoleIndex"></param>
        /// <returns></returns>
        private async Task UpdateRoles(SocketGuildUser user, int judgementRoleIndex)
        {
            //Remove previous roles from user
            for (int i = 0; i < JUDGEMENT_ROLES.Length; i++)
            {
                if(CheckUserForRole(user, JUDGEMENT_ROLES[i]))
                {
                    await RemoveUserRole(user, JUDGEMENT_ROLES[i]);
                }
            }
            //Assign new role
            await AddUserRole(user, JUDGEMENT_ROLES[judgementRoleIndex]);
        }

        private bool CheckUserForRole(SocketGuildUser user, string role)
        {
            string[] list = user.Roles.Select(p => p.ToString()).ToArray();
            foreach (string s in list)
            {
                if (s == role)
                {
                    Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] Success! <" + user.Username + "> Found role: " + role);
                    return true;
                }
            }
            Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] Failed: <" + user.Username + "> Found " + role);
            return false;
        }

        private async Task RemoveUserRole(SocketGuildUser user, string role)
        {
            var r = Context.Guild.Roles.FirstOrDefault(x => x.Name == role);
            Console.Out.WriteLine($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Removed] Role: <" + user.Username + "> " + role);
            await (user as IGuildUser).RemoveRoleAsync(r);
        }

        private async Task AddUserRole(SocketGuildUser user, string role)
        {
            var r = Context.Guild.Roles.FirstOrDefault(x => x.Name == role);
            await (user as IGuildUser).AddRoleAsync(r);
        }
        #endregion
    }
}