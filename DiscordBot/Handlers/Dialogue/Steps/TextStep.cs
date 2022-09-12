using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.Dialogue.Steps
{
    public class TextStep : DialogueStepBase
    {
        private IDialogueStep _nextStep;
        private readonly int? _minLenght;
        private readonly int? _maxLenght;

        public TextStep(string content, IDialogueStep nextStep, int? minLenght = null, int? maxLenght = null) : base(content)
        {
            _nextStep = nextStep;
            _minLenght = minLenght;
            _maxLenght = maxLenght;
        }
        public Action <string> OnValidResult { get; set; } = delegate { };
        public override IDialogueStep NextStep => _nextStep;
        public void SetNextStep (IDialogueStep nextStep)
        {
            _nextStep = nextStep;
        }
        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Responda abaixo",
                Description = $"{user.Mention}, {_content}",
            };
            embedBuilder.AddField("Para encerrar o diálogo", "Use o comando !cancel");
            if (_minLenght.HasValue)
            {
                embedBuilder.AddField("Tamanho mínimo:", $"{_minLenght.Value} caracteres");
            }
            if (_maxLenght.HasValue)
            {
                embedBuilder.AddField("Tamanho máximo:", $"{_maxLenght.Value} caracteres");
            }
            var interactivity = client.GetInteractivity();
            while (true)
            {
                var embed = await channel.SendMessageAsync(embed : embedBuilder).ConfigureAwait(false);
                OnMessageAdded(embed);
                var messageResult = await interactivity.WaitForMessageAsync(
                    x=>x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(messageResult.Result);
                if (messageResult.Result.Content.Equals("!cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (_minLenght.HasValue)
                {
                    if (messageResult.Result.Content.Length < _minLenght.Value)
                    {
                        await TryAgain(channel, $"Sua mensagem é {_minLenght.Value - messageResult.Result.Content.Length} menor do que o esperado").ConfigureAwait(false);
                        continue;
                    }
                }
                if (_maxLenght.HasValue)
                {
                    if (messageResult.Result.Content.Length < _maxLenght.Value)
                    {
                        await TryAgain(channel, $"Sua mensagem é {messageResult.Result.Content.Length - _maxLenght.Value} maior do que o esperado").ConfigureAwait(false);
                        continue;
                    }
                }
                OnValidResult(messageResult.Result.Content);
                return false;
            }
        }
    }
}
