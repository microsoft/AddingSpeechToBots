using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Text;


namespace StudyBot
{
	public sealed partial class MainPage : Page
	{
		DirectLineClient _client;
		Conversation _conversation;
		ObservableCollection<Activity> _messagesFromBot;
		Activity newActivity;

		

        string botSecretKey = "<ADD YOUR DIRECTLINE SECRET KEY HERE>";
        string botHandle = "<ADD YOUR BOT NAME HERE>";

        string query;
		string subject;
		string kbName1 = "biology";
		string kbName2 = "geology";
		string kbName3 = "sociology";

		// Option to create a user ID and name
		string userId = "";
		string userName = "Bot: ";

		// Will handle query to add to Bing Search
		static string[] biologyQuestions = new string[]{"biology", "virus", "bug", "bacteria", "parasite", "asexual",
					   "sexual", "sex", "reproduction", "cancer", "tumor", "blood brain barrier"};
		static string[] geologyQuestions = new string[]{"geology", "magnitude", "magma", "lava", "rock", "metamorphic",
																"era", "period", "epoch", "time" };
		static string[] sociologyQuestions = new string[]{"sociology", "poverty", "minority", "cultural", "pluralism",
									 "sterotype", "affirmative action", "apartheid", "bicultural"};
		HashSet<string> set1 = new HashSet<string>(biologyQuestions);
		HashSet<string> set2 = new HashSet<string>(geologyQuestions);
		HashSet<string> set3 = new HashSet<string>(sociologyQuestions);

		public MainPage()
		{
			this.InitializeComponent();

			// Set binding context to update message list items.
			DataContext = this;

			NewMessageTextBox.PlaceholderText = "Type a study term.";

			// Add an event handler for the ContainerContentChanging event of the ListView
			MessagesList.ContainerContentChanging += OnContainerContentChanging;
		}

		public new Brush BorderBrush { get; set; }

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			// Create collection for bot messages.
			_messagesFromBot = new ObservableCollection<Activity>();
			// Initialize conversation with bot.
			await InitializeBotConversation();
		}

		// Send button
		private async void Button_Send(object sender, RoutedEventArgs e)
		{
			await SendMessageToBot();

			InputQueryToWebsites();
		}

		// Handle button click when user wants to send message to bot.
		async Task SendMessageToBot()
		{
			// Activity object with (optional) name of the user and text.
			newActivity = new Activity { From = new ChannelAccount(userId, userName), Text = NewMessageTextBox.Text, Type = ActivityTypes.Message };

			// Post message to your bot. 
			if (_conversation != null)
			{
				try
				{
					await _client.Conversations.PostActivityAsync(_conversation.ConversationId, newActivity);
				}
				catch (Exception e)
				{
					Debug.WriteLine("Call stack: " + e.GetBaseException());
				}
			}
		}

		async Task InitializeBotConversation()
		{
			// Initialize Direct Client with secret obtained in the Bot Portal.
			_client = new DirectLineClient(botSecretKey);
			// Initialize new converstation.
			_conversation = await _client.Conversations.StartConversationAsync();
			// Wait for the responses from bot.
			await ReadBotMessagesAsync(_client, _conversation.ConversationId);
		}

		// Handles messages from bot.
		async Task ReadBotMessagesAsync(DirectLineClient client, string conversationId)
		{
			// Optionally set watermark - this is last message id seen by bot. It is for paging.
			string watermark = null;

			while (true)
			{
				// Get all messages returned by bot.
				var convActivities = await client.Conversations.GetActivitiesAsync(conversationId, watermark);

				watermark = convActivities?.Watermark;

				// Get messages from your bot - From.Name should match your Bot Handle.
				var messagesFromBotText = from x in convActivities.Activities
										  where x.From.Name == botHandle
										  select x;

				// Iterate through all messages.
				foreach (Activity message in messagesFromBotText)
				{
					message.Text = userName + message.Text;
					await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
					() => {
						// Add message to the list and update ListView source to display response on the UI.
						if (!_messagesFromBot.Contains(message))
						{
							_messagesFromBot.Add(newActivity); // Adds user query to chat window.
							_messagesFromBot.Add(message); // Adds bot reponse to chat window.
						}
						MessagesList.ItemsSource = _messagesFromBot;

						// Auto-scrolls to last item in chat
						MessagesList?.ScrollIntoView(MessagesList.Items[_messagesFromBot.Count - 1], ScrollIntoViewAlignment.Leading);
					});
				}

				await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
			}
		}

		// Removes punctuation from query 
		static string CleanInput(string strIn)
		{
			// Replace invalid characters with empty strings.
			try
			{
				if(strIn != null)
				{
					// Change to lowercase to match sets
					strIn = strIn.ToLower();
				}


				return Regex.Replace(strIn, @"[^\w\s]", "", // won't remove chars in [], otherwise will with empty string
									 RegexOptions.None, TimeSpan.FromSeconds(1.5));
			}
			// If we timeout when replacing invalid characters, return Empty.
			catch (RegexMatchTimeoutException)
			{
				return String.Empty;
			}
		}

		// Handles when 'Enter' pressed after chat entry
		private async void NewMessageTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
			{
				Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
				await SendMessageToBot();

				InputQueryToWebsites();
			}
		}

		// Decides how to search in websites based on query
		private async void InputQueryToWebsites()
		{
			await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				if (query == "" || query == null)
				{
					// Gets query for other uses.
					query = NewMessageTextBox.Text;
				}

				// Strip query of punctuation & make lowercase             
				query = CleanInput(query);

				// Get subject of query (topic of your knowledge base)
				if (set1.Contains(query))
				{
					if (query == kbName1)
					{
						subject = " "; // don't add a subject if query is already subject word
					}
					else
					{
						subject = kbName1;
					}
				}
				else if (set2.Contains(query))
				{
					if (query == kbName2)
					{
						subject = ""; // don't add a subject if query is already subject word
					}
					else
					{
						subject = kbName2;
					}
				}
				else if (set3.Contains(query))
				{
					if (query == kbName3)
					{
						subject = ""; // don't add a subject if query is already subject word
					}
					else
					{
						subject = kbName3;
					}
				}
				else // if no subject, then must be a LUIS default intent (Greeting, Cancel, Help, or None)
				{
					subject = "";
					query = "";

					// Sites need the root URLs to render, rather than empty query/subject in URL
					Encyclopedia.Navigate(new Uri("https://en.wikipedia.org/"));
					MicrosoftAcademic.Navigate(new Uri("https://academic.microsoft.com/"));
					NewsBlogs.Navigate(new Uri("https://www.bing.com/"));
					return;
				}

				// Set query into Encyclopedia, Microsoft Academics, and Bing Search
				Encyclopedia.Navigate(new Uri("https://en.wikipedia.org/w/index.php?search=" + query + "+" + subject + "&title=Special%3ASearch&go=Go"));
				NewsBlogs.Navigate(new Uri("https://www.bing.com/search?q=" + query + "+" + subject));
				MicrosoftAcademic.Navigate(new Uri("https://academic.microsoft.com/#/search?iq=%40" + query + "%20" + subject + "%40&q=" + query + "%20" + subject));
			});

			// Clears text for next query.
			await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				NewMessageTextBox.Text = String.Empty;
			});
			query = "";
		}

		// Back button for Pivot.
		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			if (rootPivot.SelectedIndex > 0)
			{
				// If not at the first item, go back to the previous one.
				rootPivot.SelectedIndex -= 1;
			}
			else
			{
				// The first PivotItem is selected, so loop around to the last item.
				rootPivot.SelectedIndex = rootPivot.Items.Count - 1;
			}
		}

		// Next button for Pivot.
		private void NextButton_Click(object sender, RoutedEventArgs e)
		{
			if (rootPivot.SelectedIndex < rootPivot.Items.Count - 1)
			{
				// If not at the last item, go to the next one.
				rootPivot.SelectedIndex += 1;
			}
			else
			{
				// The last PivotItem is selected, so loop around to the first item.
				rootPivot.SelectedIndex = 0;
			}
		}

		private void Journals_LoadCompleted(object sender, NavigationEventArgs e)
		{

		}

		private void OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
		{
			if (args.InRecycleQueue) return;

			// Currently we are adding messages to the ListView.ItemSource as Activity objects
			// since this handler is called when the content changes (an item is added)
			// intercept the item as an activity and set its horizontal alignment accordingly
			Activity message = args.Item as Activity;
			if (message != null)
				{
					args.ItemContainer.HorizontalAlignment = (message.From.Name == botHandle) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
					args.ItemContainer.BorderBrush = (message.From.Name == botHandle) ? new SolidColorBrush(Windows.UI.Colors.Turquoise) : new SolidColorBrush(Windows.UI.Colors.LightSkyBlue);
					args.ItemContainer.BorderThickness = new Thickness(5);
				}
		}

		
	}
}