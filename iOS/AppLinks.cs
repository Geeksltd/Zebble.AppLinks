namespace Zebble
{
    using Foundation;
    using System.Collections.Generic;

    public static partial class AppLinks
    {
        static AppLinks()
        {
            UIRuntime.OnOpenUrl.Handle(url => GetAppLinkDatas(url));
        }

        static void GetAppLinkDatas(NSUrl url)
        {
            var result = new List<AppLinkData>();
            var rurl = new Rivets.AppLinkUrl(url.ToString());

            if (rurl != null)
            {
                foreach (var param in rurl.InputQueryParameters)
                {
                    result.Add(new AppLinkData(param.Key, param.Value));
                }
            }

            OnAppLinkReceived?.Raise(result);
        }
    }
}