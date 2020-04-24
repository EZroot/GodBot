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

        private Task OnJudgeAsync(SocketMessage msg)
        {
            if (!Directory.Exists(_userDirectory))     // Create the log directory if it doesn't exist
                Directory.CreateDirectory(_userDirectory);
            if (!File.Exists(_userFile))               // Create today's log file if it doesn't exist
                File.Create(_userFile).Dispose();

            SocketGuildUser sgu = msg.Author as SocketGuildUser;                          //User who sent the message
            SocketUserMessage suMsg = msg as SocketUserMessage;                         //Socket user message
            SocketCommandContext context = new SocketCommandContext(_discord, suMsg);  //Context of the message

            //Create temp user data
            UserData userData = new UserData(sgu.Id,
                msg.Author.Username,
                String.Join(",", sgu.Roles.Select(p => p.ToString()).ToArray()),
                1,
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
                userData = _activeUserList.Single(item => item.Id == sgu.Id);
                //Handle Score
                ScoreHandler(userData, suMsg);
                result = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] <" + userData.Username + "> updated.";
            }

            //Save our data
            SaveUserData();
            return Console.Out.WriteLineAsync(result);       // Write the log text to the console
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
            Console.Out.WriteLine($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Loaded] User data json file");
            string json = File.ReadAllText(_userFile);
            return JsonConvert.DeserializeObject<List<UserData>>(json);
        }

        private bool CheckUserForRole(SocketGuildUser sgu, string role)
        {
            string[] list = sgu.Roles.Select(p => p.ToString()).ToArray();
            foreach(string s in list)
            {
                if(s == role)
                {
                    Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] Success! <" + sgu.Username + "> Found role: " + role);
                    return true;
                }
            }
            Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] Failed: <" + sgu.Username + "> Found " + role);
            return false;
        }

        //ToDo: filter based on positive words/negative words add them together and combine the score
        private void ScoreHandler(UserData userdata, SocketUserMessage msg)
        {
            //-1
            if (msg.Content.Contains("warrior") || msg.Content.Contains("fag") || msg.Content.Contains("gay") || msg.Content.Contains("bitch") || msg.Content.Contains("omg") || msg.Content.Contains("omfg") || msg.Content.Contains("fk") || msg.Content.Contains("shit"))
            {
                userdata.LoseXP(5);
            }
            //-5
            if (msg.Content.Contains("gay") || msg.Content.Contains("autism") || msg.Content.Contains("autistic") || msg.Content.Contains("retard") || msg.Content.Contains("retarded") || msg.Content.Contains("dick"))
            {
                userdata.LoseXP(8);
            }
            //-50
            if (msg.Content.Contains("goddammit") || msg.Content.Contains("god damn") || msg.Content.Contains("god dam") || msg.Content.Contains("goddam") || msg.Content.Contains("goddamn"))
            {
                userdata.LoseXP(52);
            }
            if (msg.Content.Contains("Warrior") || msg.Content.Contains("Fag") || msg.Content.Contains("Gay") || msg.Content.Contains("Bitch") || msg.Content.Contains("Omg") || msg.Content.Contains("Fuck") || msg.Content.Contains("fuck") || msg.Content.Contains("shit") || msg.Content.Contains("ass"))
            {
                userdata.LoseXP(4);
            }

            //5
            if (msg.Content.Contains("beautiful") || msg.Content.Contains("cute") || msg.Content.Contains("trap") || msg.Content.Contains("christian") || msg.Content.Contains("amen") || msg.Content.Contains("holy"))
            {
                userdata.AddXP(86);
            }
            //10
            if (msg.Content.Contains("love") || msg.Content.Contains("thanks") || msg.Content.Contains("thank you") || msg.Content.Contains("thank u"))
            {
                userdata.AddXP(122);
            }
            //20
            if (msg.Content.Contains("i love you"))
            {
                userdata.AddXP(213);
            }
        }
    }
}
