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
    public class ChatbotLUIS : ActivityHandler
    {

        protected readonly ILuisRecognizerService _luisRecognizerService;

        public ChatbotLUIS(ILuisRecognizerService luisRecognizerService)
        {
            _luisRecognizerService = luisRecognizerService;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hola, Bienvenido!";
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
            var recognizeResult = await _luisRecognizerService._recognizer.RecognizeAsync(turnContext, cancellationToken);
            await ManageIntentions(turnContext, recognizeResult, cancellationToken);
        }

        private async Task ManageIntentions(ITurnContext<IMessageActivity> turnContext, RecognizerResult recognizerResult, CancellationToken cancellationToken)
        {
            var topIntent = recognizerResult.GetTopScoringIntent();
            switch (topIntent.intent)
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
            await turnContext.SendActivityAsync($"Estoy para servirte, me gusta ayudar.", cancellationToken: cancellationToken);
        }

        private async Task IntentDatosInscripcion(ITurnContext<IMessageActivity> turnContext, RecognizerResult recognizerResult, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync($"-Copia cotejada del título de licenciatura, cédula profesional, acta de examen profesional o documento oficial equivalente en Derecho o disciplinas afines al Programa a cursar." + "\r\n" +
                $"-Copia de certificado de estudios con promedio mínimo de ocho." + "\r\n" +
                $"Copia del acta de nacimiento." + "\r\n" +
                $"Dos copias del CURP(actualizado)." + "\r\n" +
                $"Curriculum Vitae con soporte documental en copia." + "\r\n" +
                $"Carta de exposición de motivos." + "\r\n" +
                $"Dos fotografías tamaño infantil de frente." + "\r\n" +
                $"Dos cartas de recomendación académica.", cancellationToken: cancellationToken);
        }

        private async Task IntentNone(ITurnContext<IMessageActivity> turnContext, RecognizerResult recognizerResult, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync($"No entiendo, se más claro.", cancellationToken: cancellationToken);
        }
    }
}
