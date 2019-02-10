using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Rest;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace StudyBot
{
    internal static class Util
    {
        internal static async Task GenericApiCallExceptionHandler(Exception ex, string errorTitle)
        {
            string errorDetails = GetMessageFromException(ex);

            await new MessageDialog(errorDetails, errorTitle).ShowAsync();
        }

        internal static string GetMessageFromException(Exception ex)
        {
            string errorDetails = ex.Message;

            HttpOperationException httpException = ex as HttpOperationException;

            if (httpException?.Response?.ReasonPhrase != null)

            {
                errorDetails = string.Format("{0}. The error message was \"{1}\".", ex.Message, httpException?.Response?.ReasonPhrase);
            }

            return errorDetails;
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }
}
