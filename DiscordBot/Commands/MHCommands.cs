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
        public async Task<List<InformationMH>> GetList(string nameMH)
        {
            List<InformationMH> dict = new List<InformationMH>();
            using (var sr = new StreamReader("mhinfo.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string[] information = sr.ReadLine().Split(',');
                    string name = information[0];
                    string description = information[1];
                    dict.Add(new InformationMH(name, description));
                }
            }
            return 
            //return dict.FirstOrDefault(x=> x.Name == nameMH);

        }


        [Command("info")]
        public async Task InformationSearch(CommandContext ctx, string monsterName)
        {
            /* using (StreamReader srinfo = File.OpenText("mhinfo.txt"))
             {
                 Dictionary<string, string> dict = new Dictionary<string, string>();
                 while (!srinfo.EndOfStream)
                 {
                     string[] information = srinfo.ReadLine().Split(',');
                     string name = information[0];
                     string description = information[1];

                     if (dict.TryGetValue(monsterName, out description))
                     {
                         await ctx.Channel.SendMessageAsync(description);
                     }*/

            var item = await GetList(monsterName);
            var result = item.Find(x => x.Name == monsterName);
            await ctx.Channel.SendMessageAsync(GetList(monsterName).Result.ToString());
        }
    }
}