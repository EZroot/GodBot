using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AshBot.Services
{
    public class JudgementService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        private string _userDirectory { get; }
        private string _userFile => Path.Combine(_userDirectory, $"userData.txt");

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

            //Check the user who sent the message,
            //Find out if we already have him stored as a "player"
            //if not, create him as a default player
            //check his message
            //update his XP/Level
            string userText = $"User "+msg.Author.Username+" Sent a message. We need to judge him.";
            File.AppendAllText(_userFile, userText + "\n");     // Write the user text to a file

            return Console.Out.WriteLineAsync(userText);       // Write the log text to the console
        }
    }
}
