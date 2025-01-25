namespace Zebble
{
    using Olive;
    using System;
    using System.Collections.Generic;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml.Controls;

    public static partial class AppLinks
    {
        static AppLinks() => UIRuntime.OnActivated.Handle(ExtractAppLinkData);

        static void ExtractAppLinkData(Tuple<IActivatedEventArgs, Windows.UI.Xaml.Window> args)
        {
            var result = new List<Data>();
            var rootFrame = args.Item2.Content as Frame ?? new Frame();

            args.Item2.Content = rootFrame;

            if (args.Item1.Kind == ActivationKind.Protocol)
            {
                var protocolArgs = args.Item1 as ProtocolActivatedEventArgs;

                if (protocolArgs.Uri.Query.HasValue()) result.AddRange(QueryToData(protocolArgs.Uri.Query));
                else result.AddRange(UriToData(protocolArgs.Uri));
            }

            if (result.None()) return;

            OnAppLinkReceived.Raise(result);
            args.Item2.Activate();
        }
    }
}