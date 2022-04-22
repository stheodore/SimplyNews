using MvvmCross.IoC;
using MvvmCross.Plugin;
using MvvmCross.ViewModels;
using SimplyNews.Core.ViewModels;

namespace SimplyNews.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<NewsFeedViewModel>();
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.MethodBinding.Plugin>();
        }
    }
}
