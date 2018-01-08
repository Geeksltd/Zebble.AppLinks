namespace Zebble
{
    using Foundation;
    using System.Collections.Generic;

    public static partial class AppLinks
    {
        static AppLinks()
        {
            UIRuntime.OnOpenUrl.Handle(url => ExtractAppLinkData(url));
        }

        static void ExtractAppLinkData(NSUrl url)
        {
            var result = new List<Data>();
            var rurl = new Rivets.AppLinkUrl(url.ToString());

            if (rurl != null)
                foreach (var param in rurl.InputQueryParameters)
                    result.Add(new Data(param.Key, param.Value));

            OnAppLinkReceived?.Raise(result);
        }
    }
}