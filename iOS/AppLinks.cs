namespace Zebble
{
    using Foundation;
    using Olive;
    using System;
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


            Rivets.AppLinkUrl appLinkUrl = null;
            try { appLinkUrl = new(url.ToString()); }
            catch { }

            if (appLinkUrl != null)
            {
                foreach (var param in appLinkUrl.InputQueryParameters)
                    result.Add(new Data(param.Key, param.Value));
            }
            else if (Uri.TryCreate(url.ToString(), UriKind.Absolute, out Uri uri))
            {
                if (uri.Query.HasValue()) result.AddRange(QueryToData(uri.Query));
                else result.AddRange(UriToData(uri));
            }

            if (result.Any()) OnAppLinkReceived.Raise(result);
        }
    }
}