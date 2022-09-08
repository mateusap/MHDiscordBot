using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class TeamCommands : BaseCommandModule
    {
        [Command("join")]
        public async Task Join(CommandContext ctx)
        {
            var joinEmbed = new DiscordEmbedBuilder
            {
                Title = "Gostaria de participar?",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = ctx.Client.CurrentUser.AvatarUrl},
                Color = DiscordColor.Green
            };
            var joinMessage = await ctx.Channel.SendMessageAsync(embed : joinEmbed).ConfigureAwait(false);
            var thumbsUp = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var thumbsDown = DiscordEmoji.FromName(ctx.Client, ":-1:");

            await joinMessage.CreateReactionAsync(thumbsUp).ConfigureAwait(false);
            await joinMessage.CreateReactionAsync(thumbsDown).ConfigureAwait(false);

            var interactivity = ctx.Client.GetInteractivity();
            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x.Message == joinMessage && 
                x.User == ctx.User && 
                (x.Emoji == thumbsUp ||x.Emoji== thumbsDown)).ConfigureAwait(false);

            if (reactionResult.Result.Emoji == thumbsUp)
            {
                var otherRole = ctx.Guild.GetRole(1017446541465747466);
                var role = ctx.Guild.GetRole(1017446484108660786);
                await ctx.Member.RevokeRoleAsync(otherRole).ConfigureAwait(false);
                await ctx.Member.GrantRoleAsync(role).ConfigureAwait(false);
            }
            else if (reactionResult.Result.Emoji == thumbsDown)
            {
                var otheRole = ctx.Guild.GetRole(1017446484108660786);
                var role = ctx.Guild.GetRole(1017446541465747466);
                await ctx.Member.RevokeRoleAsync(otheRole).ConfigureAwait(false);
                await ctx.Member.GrantRoleAsync(role).ConfigureAwait(false);
            }
            await joinMessage.DeleteAsync().ConfigureAwait(false);
        }

        [Command("poll")]
        public async Task Poll (CommandContext ctx, TimeSpan duration, params DiscordEmoji[] emojiOptions)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var options = emojiOptions.Select(x => x.ToString());
            var embed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = string.Join(" ", options)
            };
            var pollMessage =  await ctx.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
            foreach (var option in emojiOptions)
            {
                await pollMessage.CreateReactionAsync(option).ConfigureAwait(false);
            }
            var result = await interactivity.CollectReactionsAsync(pollMessage, duration).ConfigureAwait(false);
            var distinctResult = result.Distinct();
            var results = distinctResult.Select(x => $"{x.Emoji}:{x.Total}");
            await ctx.Channel.SendMessageAsync(string.Join("\n", results)).ConfigureAwait(false);
        }
    }
}
