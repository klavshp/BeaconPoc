using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TestApp2
{
    [Activity(Label = "TestApp2", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _view = FindViewById<RelativeLayout>(Resource.Id.TestView);

            _view.SetBackgroundColor(Color.Green);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button.Text = "Hello App2!";
        }
    }
}

