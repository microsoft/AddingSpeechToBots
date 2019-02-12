# Build the Study Bot C#, UWP app from scratch

To understand the process more, you may want to build this app from scratch, instead of downloading and running the StudyBot app from this sample. The following instruct how to do this.

1. Open Visual Studio 2017+, go to File -> New -> Project -> Visual C# -> Windows Universal -> Blank App (Universal Windows). Name your project and press OK. In the New Universal Windows Platform Project popup, choose “Windows 10 Fall Creators Update” for Minimum version. Press OK.
1. In the Nuget Package Manager, install these additional packages:
   * Microsoft.Bot.Connector.DirectLine
   * Microsoft.Rest.ClientRuntime
   * Microsoft.CognitiveServices.Speech
1. First, we’ll create the UWP interface. Open your MainPage.xaml and copy/paste all code from [this sample](https://raw.githubusercontent.com/Azure-Samples/cognitive-services-studybot-csharp/master/StudyBot/StudyBot/MainPage.xaml) in Github. 
   * Make sure your app name is in the Page attribute:
     `<Page> x:Class=”<YOUR-APP-NAME>.MainPage” … </Page>`
   * Also, add your app’s name to the other Page attribute: 
     `<Page> xmlns:local=”using:<YOUR-APP-NAME>” … </Page>`
1. Notice: the bot is embedded as a `<StackPanel>` with components: a list, a text box, and the send and speech buttons. This is your custom WebChat. The tabs (`PivotItems`) below that are websites that will run the user’s query after the query is sent by the user. So, if the user types or speaks “virus” into the WebChat, the websites will search for “virus” and provide results.
1. Next, we’ll add code to our MainPage.xaml.cs file to provide functionality for our interface. Open your MainPage.xaml.cs file and copy/paste code from [this sample](https://raw.githubusercontent.com/Azure-Samples/cognitive-services-studybot-csharp/master/StudyBot/StudyBot/MainPage.xaml.cs).
1. In your MainPage.xaml.cs file, change your namespace name at the top to match your app’s name (gets rid of a lot of errors).
1. Provide your unique values for Cognitive Services at the top: <br>
   `string botSecretKey = "<ADD YOUR DIRECTLINE SECRET KEY HERE>";`<br>
   *Where to find it*: In the Azure portal, open your web app bot service -> go to Channels - > click the globe icon (this is for Direct Line) -> in that channel (click edit) grab one of the keys (either one works).<br>
   `string botHandle = "<ADD YOUR BOT NAME HERE>";`<br>
   `string speechSubscription = "<YOUR AZURE SPEECH SERVICE SUBSCRIPTION KEY>";`<br>
   *Where to find it*: In the Azure portal, open your Speech Service subscription, look under “Keys”.<br>
   `string speechRegion = "<YOUR REGION>"; // ex: westus`<br>
1. Build/run the application.
 You will see the interface below. Type or press the microphone button to speak your queries to get started.
 
 <img src="study-bot-interface.PNG">

