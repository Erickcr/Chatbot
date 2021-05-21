// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EchoBot.Infrastructure;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {

        protected readonly ILuisRecognizerService luisRecognizerService;

        public EchoBot(ILuisRecognizerService _luisRecognizerService)
        {
            _luisRecognizerService = luisRecognizerService;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hola, Bienve!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        public override Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            return base.OnTurnAsync(turnContext, cancellationToken);
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var recognizerResult = await luisRecognizerService._recognizer.RecognizeAsync(turnContext, cancellationToken);
            await ManageIntentions(turnContext, recognizerResult, cancellationToken);
        }

        private async Task ManageIntentions(ITurnContext<IMessageActivity> turnContext, RecognizerResult recognizerResult, CancellationToken cancellationToken)
        {
            var topIntent = recognizerResult.GetTopScoringIntent();
            switch(topIntent.intent)
            {
                case "Saludar":
                    await IntentSaludar(turnContext, recognizerResult, cancellationToken);
                    break;
                case "Agradecer":
                    await IntentAgradecer(turnContext, recognizerResult, cancellationToken);
                    break;
                case "DatosInscripcion":
                    await IntentDatosInscripcion(turnContext, recognizerResult, cancellationToken);
                    break;
                case "None":
                    await IntentNone(turnContext, recognizerResult, cancellationToken);
                    break;
            }
        }

        private async Task IntentSaludar(ITurnContext<IMessageActivity> turnContext, RecognizerResult recognizerResult, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync($"Hola, ¿como te puedo ayudar?", cancellationToken: cancellationToken);
        }

        private async Task IntentAgradecer(ITurnContext<IMessageActivity> turnContext, RecognizerResult recognizerResult, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync($"Me gusta ayuudar", cancellationToken: cancellationToken);
        }

        private async Task IntentDatosInscripcion(ITurnContext<IMessageActivity> turnContext, RecognizerResult recognizerResult, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync($"Certificado y dinero, es lo que ocupas", cancellationToken: cancellationToken);
        }

        private async Task IntentNone(ITurnContext<IMessageActivity> turnContext, RecognizerResult recognizerResult, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync($"No entiendo", cancellationToken: cancellationToken);
        }
    }
}
