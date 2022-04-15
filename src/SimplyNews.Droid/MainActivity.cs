using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using System;
using SimplyNews.Core.ViewModels;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace SimplyNews
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : MvxActivity<NewsFeedViewModel>
    {
        WebView newsView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            try
            {
                var toolbar = (Toolbar)FindViewById(Resource.Id.topActionBar);
                SetSupportActionBar(toolbar);
                
                newsView = (WebView)FindViewById(Resource.Id.newscontent);
                newsView.Settings.BuiltInZoomControls = true;
                newsView.Settings.DisplayZoomControls = false;
                newsView.SetWebViewClient(new CustomWebViewClient());
            }
            catch(Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Short).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if(keyCode == Keycode.Back && newsView.CanGoBack())
            {
                newsView.GoBack();
                return true;
            }

            return base.OnKeyDown(keyCode, e);
        }
    }
}