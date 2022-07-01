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

            Rivets.AppLinkUrl appLinkUrl = null;
            if (intent.HasExtra("al_applink_data"))
            {
                var appLinkData = intent.GetStringExtra("al_applink_data");
                try { appLinkUrl = new(intent.Data.ToString(), appLinkData); }
                catch { }
            }

            if (appLinkUrl != null)
            {
                foreach (var param in appLinkUrl.InputQueryParameters)
                    result.Add(new Data(param.Key, param.Value));
            }
            else if (Uri.TryCreate(intent.DataString, UriKind.Absolute, out Uri uri))
            {
                if (uri.Query.HasValue()) result.AddRange(QueryToData(uri.Query));
                else result.AddRange(UriToData(uri));
            }

            if (result.Any()) OnAppLinkReceived.Raise(result);
        }
    }
}