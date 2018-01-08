namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Activation;
    using Windows.System;
    using Windows.UI.Xaml.Controls;

    public static partial class AppLinks
    {
        static AppLinks()
        {
            UIRuntime.OnActivated.Handle(args =>
            {
                GetAppLinkDatas(args);
            });
        }

        public static async Task<NavigationResult> Navigate(string url)
        {
            try
            {
                var result = await ResolveUrl(url);

                if (result.NavigationResult != NavigationResult.Failed)
                {
                    var uri = new Uri(result.MetaAttributes.Single(at => at.Name == MetaAttributeName.Url).Value);
                    await Launcher.LaunchUriAsync(uri);
                }

                return result.NavigationResult;
            }
            catch (Exception ex)
            {
                Device.Log.Error(ex.Message);
                return NavigationResult.Failed;
            }
        }

        static async Task<MetaAttributeResult> ResolveUrl(string url)
        {
            var result = new List<MetaAttribute>();
            var methodResult = new MetaAttributeResult(result, NavigationResult.Web);

            var html = await GetHtml(url);
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var header = doc.DocumentNode.SelectSingleNode("//head");
            if (header != null)
            {
                var metaTags = header.Elements("meta").Where(e => e.HasAttributes &&
                (e.GetAttributeValue("property", "").Contains("al:windows_phone") || e.GetAttributeValue("property", "").Contains("al:web")));

                if (!metaTags.Any()) return new MetaAttributeResult(result, NavigationResult.Failed);

                var alAttributes = metaTags.Select(e => e.Attributes);
                foreach (var attrCol in alAttributes)
                {
                    var nameAttr = attrCol.SingleOrDefault(at => at.Name == "property");
                    var valueAttr = attrCol.SingleOrDefault(at => at.Name == "content");
                    MetaAttribute metaAttr;
                    if (nameAttr.Value == "al:windows_phone:url")
                    {
                        if (valueAttr.Value.Contains("http") || valueAttr.Value.Contains("https"))
                        {
                            methodResult.NavigationResult = NavigationResult.Web;
                        }

                        metaAttr = new MetaAttribute(MetaAttributeName.Url, valueAttr.Value);
                    }
                    else if (nameAttr.Value == "al:windows_phone:app_name")
                    {
                        metaAttr = new MetaAttribute(MetaAttributeName.AppName, valueAttr.Value);
                    }
                    else if (nameAttr.Value == "al:windows_phone:app_id")
                    {
                        metaAttr = new MetaAttribute(MetaAttributeName.Package, valueAttr.Value);
                    }
                    else if (nameAttr.Value == "al:web:url")
                    {
                        metaAttr = new MetaAttribute(MetaAttributeName.WebUrl, valueAttr.Value);
                    }
                    else
                    {
                        metaAttr = new MetaAttribute(MetaAttributeName.WebFallBack, valueAttr.Value);
                    }

                    result.Add(metaAttr);
                }
            }

            if (result.Any(r => r.Name == MetaAttributeName.WebUrl)) return methodResult;
            else
            {
                methodResult.NavigationResult = NavigationResult.App;
                return methodResult;
            }
        }

        static async Task<string> GetHtml(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
        }

        static void GetAppLinkDatas(Tuple<IActivatedEventArgs, Windows.UI.Xaml.Window> args)
        {
            var result = new List<Data>();
            var rootFrame = args.Item2.Content as Frame;

            if (rootFrame == null)
                rootFrame = new Frame();

            args.Item2.Content = rootFrame;

            if (args.Item1.Kind == ActivationKind.Protocol)
            {
                var protocolArgs = args.Item1 as ProtocolActivatedEventArgs;

                var queries = protocolArgs.Uri.Query.Replace("?", string.Empty).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var query in queries)
                {
                    if (!query.Contains("=")) continue;

                    var parameter = query.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    result.Add(new Data(parameter[0], parameter[1]));
                }
            }

            OnAppLinkReceived?.Raise(result);
            args.Item2.Activate();
        }
    }
}