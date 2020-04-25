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
            await ReplyAsync(":exclamation:" + userData.Username +
                ":exclamation:\n.Level: " + userData.Level +
                "\n.XP:    " + userData.Xp+"\n.Goal:"+userData.XpTillLevel);
        }

        [Command("judge")]
        [Summary("Judge em.")]
        public async Task JudgeUser(SocketGuildUser user = null)
        {
            var authorUser = Context.User;
            string result = "Not enough XP. Use !help noob.";

            if(user==null)
            {
                await ReplyAsync("Nobody to judge.");
                return;
            }

            UserData userData   = JudgementService._activeUserList.Single(item => item.Id == user.Id);
            UserData authorData = JudgementService._activeUserList.Single(item => item.Id == authorUser.Id);

            /*if (authorData.Xp > 20)
            {
                authorData.LoseXP(20);
                userData.LoseXP(userData.Level * 9 + rand.Next(userData.Level + 4));

                result = "You shall be JUDGED " + userData.Username +
                    "! By the power of Runescape, I smite you for " + (userData.Level * 9 + rand.Next(userData.Level + 4)) +
                    "\n dmg. Your xp is now " + userData.Xp + ".\n" + authorData.Username + " lost 20XP!";
            }*/

            //Update Roles IF!! a level changes
            await UpdateRoles(user,1);
            //Reply in chat
            await ReplyAsync(result);
        }

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
    }
}