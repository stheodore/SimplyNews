using Foundation;
using MvvmCross.Platforms.Ios.Core;
using SimplyNews.Core;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace SimplyNews.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        public override void FinishedLaunching(UIApplication application)
        {
            AppCenter.Start("3e62df6b-48ca-4c43-8ad8-c116a9e19432", typeof(Analytics), typeof(Crashes));

            base.FinishedLaunching(application);
        }
    }
}

