namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Olive;

    public static partial class AppLinks
    {
        public static AsyncEvent<List<Data>> OnAppLinkReceived = new();

        static List<Data> UriToData(Uri uri)
        {
            var urlQuery = uri.OriginalString.RemoveBeforeAndIncluding("//");
            return urlQuery.Split('/', StringSplitOptions.RemoveEmptyEntries).Select(x => new Data(x, "")).ToList();
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
