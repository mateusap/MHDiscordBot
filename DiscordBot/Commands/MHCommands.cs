using DiscordBot.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class MHCommands : BaseCommandModule
    {
        [Command("info")]
        public async Task InformationSearch(CommandContext ctx, string monsterName)
        {
            var path = "mhinfo.txt";
            StreamReader sr = null;
            sr = File.OpenText(path);
            while (!sr.EndOfStream)
            {
                string[] infos = sr.ReadLine().Split(',');
                string nome = infos[0];
                string descricao = infos[1];
                if (string.Equals(monsterName, nome, StringComparison.OrdinalIgnoreCase))
                {
                    await ctx.Channel.SendMessageAsync(descricao);
                }
            }
        }
    }
}