namespace Zebble
{
    using Android.App;
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
                if (uri.Query.HasValue())
                    result.AddRange(QueryToData(uri.Query));
                else
                {
                    var urlQuery = uri.AbsoluteUri;

                    var activity = UIRuntime.CurrentActivity;
                    if (activity is not null)
                    {
                        try
                        {
                            var intentFilter = activity.GetType()
                                .GetCustomAttributes(typeof(IntentFilterAttribute), inherit: false)
                                .FirstOrDefault() as IntentFilterAttribute;

                            if (intentFilter is not null)
                            {
                                if (intentFilter.DataScheme.HasValue())
                                    urlQuery = urlQuery.RemoveBeforeAndIncluding(intentFilter.DataScheme);

                                intentFilter.DataHosts.OrEmpty().OrderByDescending(x => x).Do(x => urlQuery = urlQuery.RemoveBeforeAndIncluding(x));

                                if (intentFilter.DataPathPrefix.HasValue())
                                    urlQuery = urlQuery.RemoveBeforeAndIncluding(intentFilter.DataPathPrefix);
                            }
                        }
                        catch (Exception ex)
                        {
                            var xx = ex.Message;
                        }
                    }

                    result.AddRange(
                        urlQuery.Split('/', StringSplitOptions.RemoveEmptyEntries).Chop(2)
                                .Select(x => new Data(x.FirstOrDefault(), x.LastOrDefault()))
                    );
                }
            }

            if (result.Any()) OnAppLinkReceived.Raise(result);
        }
    }
}