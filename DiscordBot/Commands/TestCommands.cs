using DiscordBot.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Retorna Pong")]
        [RequireCategories(ChannelCheckMode.Any, "Canais de Texto")]
        public async Task Ping (CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }
        [Command("mais")]
        [Description("Soma dois números")]
        public async Task Somar (CommandContext ctx,
            [Description("Primeiro número, podendo ser decimal(separado por ponto), para a soma")] double primeiroNumero,
            [Description("Segundo número, podendo ser decimal(separado por ponto), para a soma")] double segundoNumero)
        {
            await ctx.Channel.SendMessageAsync((primeiroNumero+segundoNumero).ToString()).ConfigureAwait(false);

        }
        [Command("menos")]
        [Description("Subtrai dois números")]
        public async Task Subtrair(CommandContext ctx,
            [Description("Primeiro número, podendo ser decimal(separado por ponto), para a subtração")] double primeiroNumero,
            [Description("Segundo número, podendo ser decimal(separado por ponto), para a subtração")] double segundoNumero)
        {
            await ctx.Channel.SendMessageAsync((primeiroNumero - segundoNumero).ToString()).ConfigureAwait(false);

        }
        [Command("multiplica")]
        [Description("Multiplica dois números")]
        public async Task Multiplicar(CommandContext ctx,
            [Description("Primeiro número, podendo ser decimal(separado por ponto), para a multiplicação")] double primeiroNumero,
            [Description("Segundo número, podendo ser decimal(separado por ponto), para a multiplicação")] double segundoNumero)
        {
            await ctx.Channel.SendMessageAsync((primeiroNumero * segundoNumero).ToString()).ConfigureAwait(false);

        }
        [Command("divide")]
        [Description("Divide dois números")]
        public async Task Dividir(CommandContext ctx,
            [Description("Primeiro número, podendo ser decimal(separado por ponto), para a divisão")] double primeiroNumero,
            [Description("Segundo número, podendo ser decimal(separado por ponto), para a divisão")] double segundoNumero)
        {
            await ctx.Channel.SendMessageAsync((primeiroNumero / segundoNumero).ToString()).ConfigureAwait(false);

        }

        [Command("response")]
        public async Task Response (CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(message.Result.Content.ToUpper());
        }

        [Command("reaction")]
        public async Task Reaction(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var message = await interactivity.WaitForReactionAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }
    }
}
