namespace Zebble
{
    using System;
    using System.Collections.Generic;

    public static partial class AppLinks
    {
        static AppLinks()
        {
            UIRuntime.OnNewIntent.Handle(intent => ExtractAppLinkData(intent));
        }

        static void ExtractAppLinkData(Android.Content.Intent intent)
        {
            var result = new List<Data>();
            if (intent.HasExtra("al_applink_data"))
            {
                var appLinkData = intent.GetStringExtra("al_applink_data");
                var appLinkUrl = new Rivets.AppLinkUrl(intent.Data.ToString(), appLinkData);

                if (appLinkUrl != null)
                    foreach (var param in appLinkUrl.InputQueryParameters)
                        result.Add(new Data(param.Key, param.Value));
            }
            else if (Uri.TryCreate(intent.DataString, UriKind.Absolute, out Uri uri))
            {
                result.AddRange(UriToData(uri));
            }
            OnAppLinkReceived?.Raise(result);
        }
    }
}