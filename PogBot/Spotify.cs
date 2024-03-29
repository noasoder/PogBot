﻿using Discord.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PogBot
{
    public class Spotify
    {
        public static async Task StartPlayback(IAudioClient client)
        {
            await SendAsync(client, "src/AntiHero.mp3");
            return;
        }

        private static async Task SendAsync(IAudioClient client, string path)
        {
            using var ffmpeg = CreateStream(path);
            using var output = ffmpeg.StandardOutput.BaseStream;
            using var discord = client.CreateDirectPCMStream(AudioApplication.Music);
            try { await output.CopyToAsync(discord); }
            finally { await discord.FlushAsync(); }
        }

        private static Process? CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }
    }
}
