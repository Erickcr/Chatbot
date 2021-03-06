using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Infrastructure
{
    public class LuisRecognizerService : ILuisRecognizerService
    {
        public LuisRecognizer _recognizer { get; private set; }

        public LuisRecognizerService(IConfiguration configuration)
        {
            var luisApplication = new LuisApplication(
                configuration["LuisAppId"],
                configuration["LuisApiKey"],
                configuration["LuisHostNme"]
                );
            var recognizerOptions = new LuisRecognizerOptionsV3(luisApplication)
            {
                PredictionOptions = new Microsoft.Bot.Builder.AI.LuisV3.LuisPredictionOptions()
                {
                    IncludeInstanceData = true
                }
            };
            _recognizer = new LuisRecognizer(recognizerOptions);
        }
    }
}
