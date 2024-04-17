namespace Zebble
{
    using Olive;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class AppLinks
    {
        static AppLinks() => UIRuntime.OnNewIntent.Handle(ExtractAppLinkData);

        static void ExtractAppLinkData(Android.Content.Intent intent)
        {
            var result = new List<Data>();

            if (Uri.TryCreate(intent.DataString, UriKind.Absolute, out Uri uri))
            {
                if (uri.Query.HasValue()) result.AddRange(QueryToData(uri.Query));
                else result.AddRange(UriToData(uri));
            }

            if (result.Any()) OnAppLinkReceived.Raise(result);
        }
    }
}