namespace Zebble
{
    using System.Threading.Tasks;

    public static partial class AppLinks
    {
        public static Task<NavigationResult> Navigate(string url)
        {
            return Device.UIThread.Run(async () =>
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
    }
}
