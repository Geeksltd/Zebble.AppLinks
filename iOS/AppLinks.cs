namespace Zebble
{
    using Foundation;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class AppLinks
    {
        static AppLinks()
        {
            UIRuntime.OnOpenUrl.Handle(ExtractAppLinkData);
            UIRuntime.OnOpenUrlWithOptions.Handle(@params => ExtractAppLinkData(@params.Item2));
        }

        static void ExtractAppLinkData(NSUrl url)
        {
            var result = new List<Data>();
            var rurl = new Rivets.AppLinkUrl(url.ToString());

            if (rurl != null)
                foreach (var param in rurl.InputQueryParameters)
                    result.Add(new Data(param.Key, param.Value));

            if (result.Any()) OnAppLinkReceived.Raise(result);
        }
    }
}