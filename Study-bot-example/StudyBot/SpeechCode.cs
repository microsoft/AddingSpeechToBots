	private async void Button_Mic(object sender, RoutedEventArgs e)
		{
			// Change color of button when clicked
			Button micButton = FindName("MicButton") as Button;
			micButton.Background = new SolidColorBrush(Windows.UI.Colors.Red);

			// Speech subscription key and region
			var config = SpeechConfig.FromSubscription(speechSubscription, speechRegion);
			try
			{
				// Creates a speech recognizer using microphone as audio input.
				using (SpeechRecognizer recognizer = new SpeechRecognizer(config))
				{
					// Starts recognition. It returns when the first utterance has been recognized.
					var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

					// Checks result.
					StringBuilder sb = new StringBuilder();
					if (result.Reason == ResultReason.RecognizedSpeech)
					{

						// Activity object with (optional) name of the user and text. "newActivity.Text" holds the spoken user query
						newActivity = new Activity { From = new ChannelAccount(userId, userName), Text = result.Text, Type = ActivityTypes.Message };
 
						// Grabs query from speech to use in websites
						query = newActivity.Text;

						// Post message to your bot. 
						if (_conversation != null)
						{
							await _client.Conversations.PostActivityAsync(_conversation.ConversationId, newActivity);
						}

						InputQueryToWebsites();
					}
					else if (result.Reason == ResultReason.NoMatch)
					{
						sb.AppendLine($"NOMATCH: Speech could not be recognized.");
					}
					else if (result.Reason == ResultReason.Canceled)
					{
						var cancellation = CancellationDetails.FromResult(result);
						sb.AppendLine($"CANCELED: Reason={cancellation.Reason}");

						if (cancellation.Reason == CancellationReason.Error)
						{
							sb.AppendLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
							sb.AppendLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
							sb.AppendLine($"CANCELED: Did you update the subscription info?");
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				micButton.Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
			});
		}