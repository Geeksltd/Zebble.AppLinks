namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Olive;

    public static partial class AppLinks
    {
        static readonly AsyncEvent<List<Data>> OnAppLinkReceived = new();
        static string[] DataPrefixes;

        public static void Configure(Action<List<Data>> onAppLinkReceived, params string[] dataPrefixes)
        {
            OnAppLinkReceived.Handle(onAppLinkReceived);
            DataPrefixes = dataPrefixes;
        }

        static List<Data> QueryToData(string rawQuery)
        {
            var queries = rawQuery.Remove("?")
                .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            return queries.Except(q => q.Lacks("=")).Select(query =>
            {
                var parameters = query.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                return new Data(parameters.FirstOrDefault(), parameters.LastOrDefault());
            }).ToList();
        }

        static IEnumerable<Data> UriToData(Uri uri)
        {
            var urlQuery = uri.AbsoluteUri;

            DataPrefixes.OrEmpty().OrderByDescending(x => x).Do(x => urlQuery = urlQuery.RemoveBeforeAndIncluding(x));

            return urlQuery.Split('/', StringSplitOptions.RemoveEmptyEntries).Chop(2)
                           .Select(x => new Data(x.FirstOrDefault(), x.LastOrDefault()));
        }

#if ANDROID || IOS
        public static Task<NavigationResult> Navigate(string url)
        {
            return Thread.UI.Run(async () =>
            {
                NavigationResult result;
                var navigatorResult = await Rivets.AppLinks.Navigator.Navigate(url);

                switch (navigatorResult)
                {
                    case Rivets.NavigationResult.Web:
                        result = NavigationResult.Web;
                        break;
                    case Rivets.NavigationResult.App:
                        result = NavigationResult.App;
                        break;
                    case Rivets.NavigationResult.Failed:
                    default:
                        result = NavigationResult.Failed;
                        break;
                }

                return result;
            });
        }
#endif

        public class Data
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public Data(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        public enum NavigationResult
        {
            Failed = 0,
            Web = 1,
            App = 2
        }
    }
}
