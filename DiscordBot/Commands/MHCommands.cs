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
        [Description("Informações sobre o monstro escolhido, sendo elas o seu tipo, seus elementos, suas alterações de status e suas fraquezas elementais")]
        public async Task InformationSearch(CommandContext ctx, [Description("Nome do monstro que deseja as informações")][RemainingText] string monsterName)
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

        [Command("random")]
        [Description("Retorna com o nome aleatório de um monstro do jogo escolhido (inclui as expansões)")]
        public async Task RandomMonster(CommandContext ctx, [Description("Nome do jogo, use 'rise' ou 'world'")] string gameName)
        {
            if (string.Equals(gameName, "rise", StringComparison.OrdinalIgnoreCase))
            {
                var path = "MHRSlist.txt";
                List<string> listanomes = File.ReadAllLines(path).ToList();
                var random = new Random();
                int index = random.Next(listanomes.Count);
                await ctx.Channel.SendMessageAsync(listanomes[index]);
            }
            else if (string.Equals(gameName,"world",StringComparison.OrdinalIgnoreCase))
            {
                var path = "MHWIlist.txt";
                List<string> listanomes = File.ReadAllLines(path).ToList();
                var random = new Random();
                int index = random.Next(listanomes.Count);
                await ctx.Channel.SendMessageAsync(listanomes[index]);
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Utilize 'rise' para Monster Hunter Rise Sunbreak ou 'world' para Monster Hunter World Iceborne.");
            }
        }
    }
}