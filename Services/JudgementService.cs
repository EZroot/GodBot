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
            bool containsItem = _userDataList.Any(item => item.Id == userData.Id); //compare userID to every userID in list to see if we exist
            string result = "";
            if (!containsItem) //Add User
            {
                _userDataList.Add(userData);
                result = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] <" +userData.Username + "> Added to active user list";
            }
            else              //User Already Exists
            {
                userData = _userDataList.Single(item => item.Id == sgu.Id);
                //Add XP
                userData.AddXP(400);

                result = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [Judgement] <" + userData.Username + "> updated.";
            }

            //Save our data
            File.WriteAllText(_userFile, SerializedUserDataList(_userDataList));
            return Console.Out.WriteLineAsync(result);       // Write the log text to the console
        }

        //Converts all userdata to json serialized object strings
        private string SerializedUserDataList(List<UserData> userList)
        {
            string serializedJson = "";
            foreach(UserData d in userList)
            {
                serializedJson += JsonConvert.SerializeObject(d,Formatting.Indented);
            }
            return serializedJson;
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
    }
}
