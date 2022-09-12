using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.Dialogue.Steps
{
    public abstract class DialogueStepBase : IDialogueStep
    {
        protected readonly string _content;
        public DialogueStepBase(string content)
        {
            _content = content;
        }
        public Action<DiscordMessage> OnMessageAdded { get; set; } = delegate { };
        public abstract IDialogueStep NextStep { get; }
        public abstract Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

        protected async Task TryAgain(DiscordChannel channel, string problem)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Tente novamente.",
                Color = DiscordColor.Red,
            };
            embedBuilder.AddField("Houve um problema com seu pedido anterior", problem);
            var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
            OnMessageAdded(embed);
        }
    }
}
