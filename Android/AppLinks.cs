namespace Zebble
{
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

            OnAppLinkReceived?.Raise(result);
        }
    }
}