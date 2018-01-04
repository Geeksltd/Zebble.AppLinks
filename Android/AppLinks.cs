namespace Zebble
{
    using System.Collections.Generic;

    public static partial class AppLinks
    {
        public static List<AppLinkData> GetAppLinkDatas(params string[] parameters)
        {
            var result = new List<AppLinkData>();
            var mainActivity = UIRuntime.CurrentActivity;
            if (mainActivity.Intent.HasExtra("al_applink_data"))
            {
                var appLinkData = mainActivity.Intent.GetStringExtra("al_applink_data");

                var alUrl = new Rivets.AppLinkUrl(mainActivity.Intent.Data.ToString(), appLinkData);

                if (alUrl != null)
                {
                    foreach (var param in parameters)
                    {
                        if (alUrl.InputQueryParameters.ContainsKey(param)) result.Add(new AppLinkData(param, alUrl.InputQueryParameters[param]));
                    }
                }
            }

            return result;
        }
    }
}