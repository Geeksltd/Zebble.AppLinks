namespace Zebble
{
    using System.Collections.Generic;

    public static partial class AppLinks
    {
        static AppLinks()
        {
            UIRuntime.OnNewIntent.Handle(intent => GetAppLinkDatas(intent));
        }

        static void GetAppLinkDatas(Android.Content.Intent intent)
        {
            var result = new List<AppLinkData>();
            if (intent.HasExtra("al_applink_data"))
            {
                var appLinkData = intent.GetStringExtra("al_applink_data");

                var alUrl = new Rivets.AppLinkUrl(intent.Data.ToString(), appLinkData);

                if (alUrl != null)
                {
                    foreach (var param in alUrl.InputQueryParameters)
                    {
                        result.Add(new AppLinkData(param.Key, param.Value));
                    }
                }
            }

            OnAppLinkReceived?.Raise(result);
        }
    }
}