using DiscordBot.Attributes;
using DiscordBot.Handlers.Dialogue;
using DiscordBot.Handlers.Dialogue.Steps;
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

        [Command("dialogo")]
        public async Task Dialogo (CommandContext ctx)
        {
            var inputStep = new TextStep("Fale algo interessante!", null, minLenght:3);
            var funnyStep = new IntStep("Haha, engraçadão", null, maxValue:100);
            string input = string.Empty;
            int value = 0;
            inputStep.OnValidResult += (result) => 
            {
                input = result;
                if (result == "algo interessante")
                {
                    inputStep.SetNextStep(funnyStep);
                }
            };

            funnyStep.OnValidResult += (result) => value = result;

            var userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);
            var inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, inputStep);
            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);
            if (!succeeded) { return; }
            await ctx.Channel.SendMessageAsync(input.ToUpper()).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync(value.ToString()).ConfigureAwait(false);
        }
        [Command("emoji")]
        public async Task EmojiDialogue(CommandContext ctx)
        {
            var yesStep = new TextStep("Você escolheu: sim", null);
            var noStep = new TextStep("Você escolheu: não", null);

            var emojiStep = new ReactionStep("Sim ou Não?", new Dictionary<DiscordEmoji, ReactionStepData>
            {
                {DiscordEmoji.FromName(ctx.Client, ":thumbsup:"),new ReactionStepData {Content = "Significa sim", NextStep=yesStep} },
                {DiscordEmoji.FromName(ctx.Client, ":thumbsdown:"),new ReactionStepData {Content = "Significa não", NextStep=noStep} }
            });
            var inputDialogueHandler = new DialogueHandler(ctx.Client, ctx.Channel, ctx.User, emojiStep);
            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);
            if (!succeeded) { return; }
        }
    }
}
