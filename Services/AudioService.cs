using Discord;
using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AshBot.Services
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();
        private string _musicDirectory = Path.Combine(AppContext.BaseDirectory, "usermusic");

        private string _musicVolume = "volume = 0.5";

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            var audioClient = await target.ConnectAsync();

            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
                // If you add a method to log happenings from this service,
                // you can uncomment these commented lines to make use of that.
                //await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
            }
        }

        public async Task LeaveAudio(IGuild guild)
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(guild.Id, out client))
            {
                await client.StopAsync();
                //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
            }
        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path)
        {
            path = Path.Combine(_musicDirectory, path);
            Console.Out.WriteLine(path);
            Console.Out.WriteLine(path);
            // Your task: Get a full path to the file if the value of 'path' is only a filename.
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("File does not exist.");
                return;
            }
            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                //await Log(LogSeverity.Debug, $"Starting playback of {path} in {guild.Name}");
                using (var ffmpeg = CreateProcess(path, _musicVolume))
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
                    finally { await stream.FlushAsync(); }
                }
            }
        }

        public  Task MaximizeSound(IGuild guild, IMessageChannel channel)
        {
             _musicVolume = "volume = 1";
            return Task.CompletedTask;
        }

        public  Task NormalizeSound(IGuild guild, IMessageChannel channel)
        {
             _musicVolume = "volume = 0.5";
            return Task.CompletedTask;
        }

        public Task MinimizeSound(IGuild guild, IMessageChannel channel)
        {
            _musicVolume = "volume = 0.2";
            return Task.CompletedTask;
        }

        private Process CreateProcess(string path, string volume)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -filter:a \"{volume}\" -f wav -ar 48k pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
