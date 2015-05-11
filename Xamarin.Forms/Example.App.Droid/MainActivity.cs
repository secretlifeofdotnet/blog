﻿namespace Example.App.Droid
{
    using Android.App;
    using Android.OS;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    [Activity(MainLauncher = true, Theme = "@style/AppTheme")]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);

            this.LoadApplication(new App());
        }
    }
}