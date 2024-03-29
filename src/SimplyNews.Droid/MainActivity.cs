﻿using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using System;
using SimplyNews.Core.ViewModels;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace SimplyNews
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : MvxActivity<NewsFeedViewModel>
    {
        WebView newsView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            try
            {
                AppCenter.Start("f28f8278-0802-4348-b2ad-62222f737047", typeof(Analytics), typeof(Crashes));

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