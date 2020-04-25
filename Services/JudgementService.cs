using AshBot.Data;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AshBot.Services
{
    public class JudgementService
    {
        private readonly string[] JUDGEMENT_ROLES = { "Satan", "Archdemon", "Demon", "Unrepented", "Repented", "Angel", "Archangel", "DemiGod" };

        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        private string _userDirectory { get; }
        private string _userFile => Path.Combine(_userDirectory, $"UserData.json");

        //Get our users
        public static List<UserData> _activeUserList = new List<UserData>();

        // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
        public JudgementService(DiscordSocketClient discord, CommandService commands)
        {
            _userDirectory = Path.Combine(AppContext.BaseDirectory, "userinfo");

            _discord = discord;
            _commands = commands;

            _discord.MessageReceived += OnJudgeAsync;

            //Load whatever user data available
            _activeUserList = LoadUserData();
        }

        private async Task OnJudgeAsync(SocketMessage msg)
        {
            if (!Directory.Exists(_userDirectory))     // Create the log directory if it doesn't exist
                Directory.CreateDirectory(_userDirectory);
            if (!File.Exists(_userFile))               // Create today's log file if it doesn't exist
                File.Create(_userFile).Dispose();

            SocketGuildUser user = msg.Author as SocketGuildUser;                          //User who sent the message
            SocketUserMessage userMsg = msg as SocketUserMessage;                         //Socket user message
            SocketCommandContext context = new SocketCommandContext(_discord, userMsg);  //Context of the message

            //Create temp user data
            UserData userData = new UserData(user.Id,
                msg.Author.Username,
                String.Join(",", user.Roles.Select(p => p.ToString()).ToArray()),
                0,
                0,
                1000);

            //List element check + tracking active users
            bool containsItem = _activeUserList.Any(item => item.Id == userData.Id); //compare userID to every userID in list to see if we exist
            string result = "";
            if (!containsItem) //Add User
            {
                _activeUserList.Add(userData);
                result = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] <" +userData.Username + "> Added to active user list";
            }
            else              //User Already Exists
            {
                userData = _activeUserList.Single(item => item.Id == user.Id);
                //Handle Score
                await ScoreHandler(user, userData, userMsg);
                result = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] <" + userData.Username + "> updated.";
            }

            //Save our data
            SaveUserData();
            await Console.Out.WriteLineAsync(result);       // Write the log text to the console
        }

        //Save our user data
        private void SaveUserData()
        {
            Console.Out.WriteLine($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Saved] Saved all user data!");
            File.WriteAllText(_userFile, JsonConvert.SerializeObject(_activeUserList, Formatting.Indented));
        }

        //Load User Data
        private List<UserData> LoadUserData()
        {
            if (!File.Exists(_userFile))
            {
                Console.Out.WriteLine($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Failed to Load] User data json file doesnt exist.");
                return new List<UserData>();
            }
            Console.Out.WriteLine($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Loaded] User data json file");
            string json = File.ReadAllText(_userFile);
            return JsonConvert.DeserializeObject<List<UserData>>(json);
        }

        //ToDo: filter based on positive words/negative words add them together and combine the score
        private async Task ScoreHandler(SocketGuildUser user, UserData userdata, SocketUserMessage msg)
        {
            //-1
            if (msg.Content.Contains("warrior") || msg.Content.Contains("fag") || msg.Content.Contains("gay") || msg.Content.Contains("bitch") || msg.Content.Contains("omg") || msg.Content.Contains("omfg") || msg.Content.Contains("fk") || msg.Content.Contains("shit"))
            {
                userdata.LoseXP(12);
            }
            //-5
            if (msg.Content.Contains("gay") || msg.Content.Contains("autism") || msg.Content.Contains("autistic") || msg.Content.Contains("retard") || msg.Content.Contains("retarded") || msg.Content.Contains("dick"))
            {
                userdata.LoseXP(28);
            }
            //-50
            if (msg.Content.Contains("goddammit") || msg.Content.Contains("god damn") || msg.Content.Contains("god dam") || msg.Content.Contains("goddam") || msg.Content.Contains("goddamn"))
            {
                userdata.LoseXP(12);
            }
            if (msg.Content.Contains("Warrior") || msg.Content.Contains("Fag") || msg.Content.Contains("Gay") || msg.Content.Contains("Bitch") || msg.Content.Contains("Omg") || msg.Content.Contains("Fuck") || msg.Content.Contains("fuck") || msg.Content.Contains("shit") || msg.Content.Contains("ass"))
            {
                userdata.LoseXP(12);
            }

            //5
            if (msg.Content.Contains("wifi") || msg.Content.Contains("space") || msg.Content.Contains("elon") || msg.Content.Contains("rocket") || msg.Content.Contains("good") || msg.Content.Contains("games") || msg.Content.Contains("thankfully") || msg.Content.Contains("beautifully") || msg.Content.Contains("beautiful") || msg.Content.Contains("cute") || msg.Content.Contains("trap") || msg.Content.Contains("christian") || msg.Content.Contains("amen") || msg.Content.Contains("holy"))
            {
                userdata.AddXP(24);
            }
            //10
            if (msg.Content.Contains("love") || msg.Content.Contains("thanks") || msg.Content.Contains("thank you") || msg.Content.Contains("thank u"))
            {
                userdata.AddXP(82);
            }
            //20
            if (msg.Content.Contains("i love you") || msg.Content.Contains("praise the sun") || msg.Content.Contains("Praise the sun"))
            {
                userdata.AddXP(113);
            }


            //Level Update
            if(userdata.Xp > -4000 && userdata.Xp <= -3000)
            {
                userdata.Level = -666;
                await UpdateRoles(msg, user, 0);
            }
            else if (userdata.Xp > -3000 && userdata.Xp <= -2000)
            {
                userdata.Level = -66;
                await UpdateRoles(msg, user, 1);
            }
            else if (userdata.Xp > -2000 && userdata.Xp <= -1000)
            {
                userdata.Level = -6;
                await UpdateRoles(msg, user, 2);
            }
            else if (userdata.Xp > -1000 && userdata.Xp <= 0)
            {
                userdata.Level = 0;
                await UpdateRoles(msg, user, 3);
            }
            else if (userdata.Xp > 0 && userdata.Xp <= 1000)
            {
                userdata.Level = 1;
                await UpdateRoles(msg, user, 4);
            }
            else if (userdata.Xp > 1000 && userdata.Xp <= 2000)
            {
                userdata.Level = 2;
                await UpdateRoles(msg, user, 5);
            }
            else if (userdata.Xp > 2000 && userdata.Xp <= 3000)
            {
                userdata.Level = 1337;
                await UpdateRoles(msg, user, 6);
            }
            else if (userdata.Xp > 3000 && userdata.Xp <= 4000)
            {
                userdata.Level = 9000;
                await UpdateRoles(msg, user, 7);
            }
        }

        private async Task UpdateRoles(SocketUserMessage msg, SocketGuildUser user, int judgementRoleIndex)
        {
            //Assign new role
            await AddUserRole(msg, user, JUDGEMENT_ROLES[judgementRoleIndex]);
            //Remove previous roles from user
            for (int i = 0; i < JUDGEMENT_ROLES.Length; i++)
            {
                if (CheckUserForRole(user, JUDGEMENT_ROLES[i]) && JUDGEMENT_ROLES[judgementRoleIndex] != JUDGEMENT_ROLES[i])
                {
                    await RemoveUserRole(msg, user, JUDGEMENT_ROLES[i]);
                }
            }
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

        private async Task RemoveUserRole(SocketUserMessage msg, SocketGuildUser user, string role)
        {
            var context = new SocketCommandContext(_discord, msg);

            var r = context.Guild.Roles.FirstOrDefault(x => x.Name == role);
            Console.Out.WriteLine($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Removed] Role: <" + user.Username + "> " + role);
            await (user as IGuildUser).RemoveRoleAsync(r);
        }

        private async Task AddUserRole(SocketUserMessage msg, SocketGuildUser user, string role)
        {
            var context = new SocketCommandContext(_discord, msg);

            var r = context.Guild.Roles.FirstOrDefault(x => x.Name == role);
            await (user as IGuildUser).AddRoleAsync(r);
        }
    }
}
