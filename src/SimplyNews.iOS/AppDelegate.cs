using Foundation;
using MvvmCross.Platforms.Ios.Core;
using SimplyNews.Core;

namespace SimplyNews.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
    }
}

