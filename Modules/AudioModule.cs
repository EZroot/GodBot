using AshBot.Services;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AshBot.Modules
{
    public class AudioModule : ModuleBase<ICommandContext>
    {
        // Scroll down further for the AudioService.
        // Like, way down
        private readonly AudioService _service;

        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public AudioModule(AudioService service)
        {
            _service = service;
        }

        // You *MUST* mark these commands with 'RunMode.Async'
        // otherwise the bot will not respond until the Task times out.
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        // Remember to add preconditions to your commands,
        // this is merely the minimal amount necessary.
        // Adding more commands of your own is also encouraged.
        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        // Remember to add preconditions to your commands,
        // this is merely the minimal amount necessary.
        // Adding more commands of your own is also encouraged.
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd([Remainder] string song)
        {
            await StopCmd();
            await JoinCmd();
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }

        [Command("maxsound", RunMode = RunMode.Async)]
        public async Task MaxSound()
        {
            await _service.SetVolume("volume = 0.5");
        }

        [Command("minsound", RunMode = RunMode.Async)]
        public async Task MinSound()
        {
            await _service.SetVolume("volume = 0.01");
        }

        [Command("normalsound", RunMode = RunMode.Async)]
        public async Task NormalCmd()
        {
            await _service.SetVolume("volume = 0.1");
        }
    }
}
