---
topic: sample
languages: 
  - csharp
services: cognitive-services, qnamaker, chit-chat, luis, language-understanding, bing spell check, speech service
products: 
  - azure
author: wiazur, chrhoder
---
# Study Bot Scenario

This sample walks through how to add speech to a Bot running in a C# windows app. 

In this lab you will create an application for learning study terms in three subjects: Geology, Biology and Sociology. The goal is to enable a more engaging study epxerience, where students can study a subject with a customized chat bot along with multiple web resources.

Each typed or spoken query into the chat bot will be accompanied by relevant search results in an encyclopedia, Microsoft Academic, and a general Bing search as a study aid. Teachers are able to create their own question and answer FAQs to create a study guide as input for the chat bot if they want it to follow a preferred curriculum. 
Users can either ask the bot for definitions in these areas or simple chit-chat.

This sample uses [Azure Bot Service](https://azure.microsoft.com/en-us/services/bot-service/), [QnA Maker](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/index) (with [Chit-chat](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base)), [LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/), [Speech Service](https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/), and [Bing Spell Check](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-spell-check/). 

Demo FAQs used in this tutorial are included in the Qna-Luis-Bot-v4/FAQs folder.

## Features

* **QnA Maker with LUIS**: 4 knowledge bases will be created in [qnamaker.ai](https://www.qnamaker.ai) for Biology, Geology, Sociology. We will also use the QnA Maker feature [Chitchat](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/chit-chat-knowledge-base) as a 4th knowledge base to enable a more natural conversation experience.

* **Bing Spell Check**: Enables the user to make simple spelling mistakes. For instance, from the sociology knowledge base, "Apartheid" can be recognized if the user inputs "apartide", "aparteid", "apartaid", etc.

* **Speech Service**: By integrating speech services, users can speak their query instead of typing them. Users will enable speech services by pressing a microphone button 

* The web resources will take a student query, like "virus", and return relevant information about it in an encyclopedia, Microsoft Academic, as well as a general Bing search that returns mostly news and blogs on the query.

## Prerequisites

See parent [README](../README.md).

## Tutorial Steps

1. Deploy and configure the Bot Service. [Follow these steps](./Qna-Luis-Bot-v4/README.md)

2. Set-up the study bot app & configure the speech input. [Follow these steps](./StudyBot/README.md)

