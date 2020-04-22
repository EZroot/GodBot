﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AshBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
    [RequireOwner(Group = "Permission")]
    [Group("admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Group("clean")]
        public class CleanModule : ModuleBase<SocketCommandContext>
        {
            // ~admin clean
            [Command]
            public async Task DefaultCleanAsync()
            {
                // ...
            }

            // ~admin clean messages 15
            [Command("messages")]
            public async Task CleanAsync(int count)
            {
                // ...
            }
        }
        // ~admin ban foxbot#0282
        [Command("ban")]
        public Task BanAsync(IGuildUser user) =>
            Context.Guild.AddBanAsync(user);
    }
}
