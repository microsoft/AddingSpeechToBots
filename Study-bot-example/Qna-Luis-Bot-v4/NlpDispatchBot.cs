// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// See https://github.com/microsoft/botbuilder-samples for a more comprehensive list of samples.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace Microsoft.BotBuilderSamples
{
    /// <summary>
    /// Main entry point and orchestration for bot.
    /// </summary>
    public class NlpDispatchBot : IBot
    {
        /// <summary>
        /// Key in the Bot config (.bot file) for the Dispatch.
        /// If you have entities in your LUIS app, you will want to create a separate LUIS app for those
        /// that will act as a model for the Dispatch app. Here the Dispatch app is used directly.
        /// </summary>
        public static readonly string DispatchKey = "Qna-Luis-Botv4-Dispatch";

        /// <summary>
        /// Key in the Bot config (.bot file) for the QnaMaker instance(s).
        /// In the .bot file, multiple instances of QnaMaker can be configured.
        /// </summary>
        public static readonly string QnAMakerChitchat = "Chitchat";
        public static readonly string QnAMakerBiology = "StudyBiology";
        public static readonly string QnAMakerSociology = "StudySociology";
        public static readonly string QnAMakerGeology = "StudyGeology";

        // Optional
        private const string WelcomeText = "Welcome to Study Bot!";

        /// <summary>
        /// Services configured from the ".bot" file.
        /// </summary>
        private readonly BotServices _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="NlpDispatchBot"/> class.
        /// </summary>
        /// <param name="services">Services configured from the ".bot" file.</param>
        public NlpDispatchBot(BotServices services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));

            // Verify Qna Maker Chitchat-Shell knowledge base configuration
            if (!_services.QnAServices.ContainsKey(QnAMakerChitchat))
            {
                throw new InvalidOperationException($"Invalid configuration.  Please check your '.bot' file for a QnA service named `{QnAMakerChitchat}`.");
            }

            // Verify Qna Maker StudyBiology knowledge base configuration
            if (!_services.QnAServices.ContainsKey(QnAMakerBiology))
            {
                throw new InvalidOperationException($"Invalid configuration.  Please check your '.bot' file for a QnA service named `{QnAMakerBiology}`.");
            }

            // Verify Qna Maker StudySociology knowledge base configuration
            if (!_services.QnAServices.ContainsKey(QnAMakerSociology))
            {
                throw new InvalidOperationException($"Invalid configuration.  Please check your '.bot' file for a QnA service named `{QnAMakerSociology}`.");
            }

            // Verify Qna Maker StudyGeology knowledge base configuration
            if (!_services.QnAServices.ContainsKey(QnAMakerGeology))
            {
                throw new InvalidOperationException($"Invalid configuration.  Please check your '.bot' file for a QnA service named `{QnAMakerGeology}`.");
            }
        }

        /// <summary>
        /// Run every turn of the conversation. Handles orchestration of messages.
        /// </summary>
        /// <param name="turnContext">Bot Turn Context.</param>
        /// <param name="cancellationToken">Task CancellationToken.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            var activity = turnContext.Activity;

            if (activity.Type == ActivityTypes.Message && !turnContext.Responded)
            {
                // Perform a call to LUIS to retrieve results for the current activity message.
                var luisResults = await _services.LuisServices[DispatchKey].RecognizeAsync(turnContext, cancellationToken);

                var topIntent = luisResults?.GetTopScoringIntent();

                if (topIntent == null)
                {
                    await turnContext.SendActivityAsync("Unable to get the top intent.");
                }
                else
                {
                    await DispatchToTopIntentAsync(turnContext, topIntent, cancellationToken);
                }
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                // Send a welcome message to the user and tell them what actions they may perform to use this bot
                await SendWelcomeMessageAsync(turnContext, cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
            }
        }

        /// <summary>
        /// Depending on the intent from Dispatch, routes to the right LUIS model or QnA service.
        /// </summary>
        private async Task DispatchToTopIntentAsync(ITurnContext context, (string intent, double score)? topIntent, CancellationToken cancellationToken = default(CancellationToken))
        {
            const string chitchatDispatchKey = "q_Chitchat";
            const string qnaBiologyDispatchKey = "q_StudyBiology";
            const string qnaSociologyDispatchKey = "q_StudySociology";
            const string qnaGeologyDispatchKey = "q_StudyGeology";

            switch (topIntent.Value.intent)
            {
                case chitchatDispatchKey:
                    await DispatchToQnAMakerAsync(context, QnAMakerChitchat);
                    break;
                case qnaBiologyDispatchKey:
                    await DispatchToQnAMakerAsync(context, QnAMakerBiology);
                    break;
                case qnaSociologyDispatchKey:
                    await DispatchToQnAMakerAsync(context, QnAMakerSociology);
                    break;
                case qnaGeologyDispatchKey:
                    await DispatchToQnAMakerAsync(context, QnAMakerGeology);
                    break;
                default:
                    // The intent didn't match any case, so just display the recognition results.
                    await context.SendActivityAsync($"Dispatch intent: {topIntent.Value.intent} ({topIntent.Value.score}).");
                    break;
            }
        }

        /// <summary>
        /// Dispatches the turn to the request QnAMaker app.
        /// </summary>
        private async Task DispatchToQnAMakerAsync(ITurnContext context, string appName, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!string.IsNullOrEmpty(context.Activity.Text))
            {
                Console.WriteLine("The context from ITurnContext: " + context + "\n");

                var results = await _services.QnAServices[appName].GetAnswersAsync(context);

                if (results.Any())
                {
                    foreach (var answer in results)
                    {
                        await context.SendActivityAsync(answer.Answer, cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    await context.SendActivityAsync($"Couldn't find an answer in the {appName}.");
                }
            }
        }

        /// <summary>
        /// Dispatches the turn to the requested LUIS model.
        /// </summary>
        private async Task DispatchToLuisModelAsync(ITurnContext context, string appName, CancellationToken cancellationToken = default(CancellationToken))
        {
            await context.SendActivityAsync($"Sending your request to the {appName} system ...");
            var result = await _services.LuisServices[appName].RecognizeAsync(context, cancellationToken);

            await context.SendActivityAsync($"Intents detected by the {appName} app:\n\n{string.Join("\n\n", result.Intents)}");

            if (result.Entities.Count > 0)
            {
                await context.SendActivityAsync($"The following entities were found in the message:\n\n{string.Join("\n\n", result.Entities)}");
            }
        }

        /// <summary>
        /// On a conversation update activity sent to the bot, the bot will
        /// send a message to the any new user(s) that were added.
        /// </summary>
        /// <param name="turnContext">Provides the <see cref="ITurnContext"/> for the turn of the bot.</param>
        /// <param name="cancellationToken" >(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>>A <see cref="Task"/> representing the operation result of the Turn operation.</returns>
        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(WelcomeText, cancellationToken: cancellationToken);
                }
            }
        }
    }
}