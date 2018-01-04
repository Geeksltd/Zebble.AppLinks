namespace Zebble
{
    using Foundation;
    using System.Collections.Generic;

    public static partial class AppLinks
    {
        public static List<AppLinkData> GetAppLinkDatas(NSUrl url,params string[] parameters)
        {
            var result = new List<AppLinkData>();
            var rurl = new Rivets.AppLinkUrl(url.ToString());

            if (rurl != null)
            {
                foreach (var param in parameters)
                {
                    if (rurl.InputQueryParameters.ContainsKey(param)) result.Add(new AppLinkData(param, rurl.InputQueryParameters[param]));
                }
            }

            return result;
        }
    }
}