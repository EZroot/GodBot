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
        const string ROLE_DICTATOR = "DICTATOR";

        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        private string _userDirectory { get; }
        private string _userFile => Path.Combine(_userDirectory, $"UserData.json");

        //Get our users
        List<UserData> _userDataList = new List<UserData>();

        // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
        public JudgementService(DiscordSocketClient discord, CommandService commands)
        {
            _userDirectory = Path.Combine(AppContext.BaseDirectory, "userinfo");

            _discord = discord;
            _commands = commands;

            _discord.MessageReceived += OnJudgeAsync;
        }

        private Task OnJudgeAsync(SocketMessage msg)
        {
            if (!Directory.Exists(_userDirectory))     // Create the log directory if it doesn't exist
                Directory.CreateDirectory(_userDirectory);
            if (!File.Exists(_userFile))               // Create today's log file if it doesn't exist
                File.Create(_userFile).Dispose();

            SocketGuildUser su = msg.Author as SocketGuildUser;                          //User who sent the message
            SocketUserMessage suMsg = msg as SocketUserMessage;                         //Socket user message
            SocketCommandContext context = new SocketCommandContext(_discord, suMsg);  //Context of the message

            //Create temp user data
            UserData userData = new UserData(su.Id, msg.Author.Username, String.Join(",", su.Roles.Select(p => p.ToString()).ToArray()), 99, 1337, 2500);

            //Add user to our active user list
            bool containsItem = _userDataList.Any(item => item.Id == userData.Id); //compare userID to every userID in list to see if we exist
            string result = "";
            if (!containsItem)
            {
                _userDataList.Add(userData);
                result = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] " +userData.Username + "-> Added to active user list";
            }
            else
            {
                result = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] User already exists in our active list!";
            }

            //Serialize our user
            string jsonString = JsonConvert.SerializeObject(userData, Formatting.Indented);
            //Find out if we already have him stored as a "player" before we append
            File.WriteAllText(_userFile, jsonString);     // Write the user text to a file
            //if not, create him as a default player
            //check his message
            //update his XP/Level
            return Console.Out.WriteLineAsync(result);       // Write the log text to the console
        }
    }
}
