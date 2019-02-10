## Overview
This repo contains the walk through for a 2 hour tutorial on adding speech to bots. 

In this tutorial we will walk you through two scenarios for integrating speech to text (STT) and text to speech (TTS) capabilities.

The first scenario is based on the [JFK Hoover Bot](https://github.com/Azure-Samples/jfkfileshooverbot) and will showcase how you integrate STT and TTS capabilities into a web app. You will also learn about integrating a custom voice.

The Hoover Bot is a single-page Web app that works in any modern browser and provides a conversational interface to The JFK Files. The JFK Files is a Web app that lets you search a corpus of documents related to the assassination of President John F. Kennedy on November 22, 1963, released by the United States government. Microsoft has presented this technology demonstration, which showcases the power of Azure Cognitive Search, on several occasions.

The second scenario is based on the [Study Bot](https://github.com/Azure-Samples/cognitive-services-studybot-csharp
) sample which showcases how to integrate STT capabilities into a C# UWP application. This application, lets users learn study terms in three subjects: Geology, Biology and Sociology through a conversational experience. The goal is to enable a more engaging study epxerience.

## Prerequisites

1. [Azure Account](http://portal.azure.com/)

1. [Visual Studio 2017+](https://www.visualstudio.com/downloads)

1. [Bot Framework Emulator](https://aka.ms/Emulator-wiki-getting-started)

1. Install and configure [ngrok](https://github.com/Microsoft/BotFramework-Emulator/wiki/Tunneling-%28ngrok%29). Ngrok has a free version and you don't need to create an account, just download it. If Ngrok is not configured, you'll see a link in your emulator where you can click to configure (edit) it.

1. [msbot](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/MSBot) and [Dipatch](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/Dispatch) CLI tools

1. Knowledge of [.bot](https://docs.microsoft.com/en-us/azure/bot-service/bot-file-basics?view=azure-bot-service-4.0) files

1. Knowledge of [ASP.Net](https://docs.microsoft.com/aspnet/core/) Core and asynchronous programming in C#

1. For the JFK Hoover bot, you will need access to an instance of the [JFK Files demo](https://github.com/Microsoft/AzureSearch_JFK_Files) and [Custom Voice Model](http://cris.ai/). You can either configure your own or ask your facilitator for the keys.


## Tutorial Steps
Work through the following README files:

1. [JFK Hoover Bot]()

1. [Study Bot](./Study-bot-example/README.md)

## Further reading and Documentation

- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot basics](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [Azure Bot Service Introduction](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- The [14.nlp-with-dispatch](https://github.com/Microsoft/BotBuilder-Samples/tree/master/samples/csharp_dotnetcore/14.nlp-with-dispatch) sample. The Qna-Luis-Botv4 sample is largely based on this sample.
- [LUIS](https://luis.ai)
- [QnA Maker](https://qnamaker.ai)
- [QnA Maker Dcoumentation](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/index)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [Prompt Types](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-prompts?view=azure-bot-service-4.0&)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)
- [QnA Maker API V4.0](https://westus.dev.cognitive.microsoft.com/docs/services/5a93fcf85b4ccd136866eb37/operations/5ac266295b4ccd1554da75ff)
- [Add Chit-chat to a QnA Maker knowledge base](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base)
- [Language Understanding (LUIS) Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
- [LUIS Programmatic APIs v2.0](https://westus.dev.cognitive.microsoft.com/docs/services/5890b47c39e2bb17b84a55ff/operations/5890b47c39e2bb052c5b9c2f)
- [LUIS Endpoint API](https://westus.dev.cognitive.microsoft.com/docs/services/5819c76f40a6350ce09de1ac/operations/5819c77140a63516d81aee78)
- [Bing Spell Check API Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/)
- [Bing Spell Check API v7 reference](https://docs.microsoft.com/en-us/rest/api/cognitiveservices/bing-spell-check-api-v7-reference)
- [Integrating QnA Maker and LUIS bot v4 tutorial, using Dispatch](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-tutorial-dispatch?view=azure-bot-service-4.0&tabs=csharp)
- [Speech Services Documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/)
