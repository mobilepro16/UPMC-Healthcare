using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Fingerprint;
using Plugin.CurrentActivity;
using Plugin.Fingerprint.Dialog;
using SVG.Forms.Plugin.Droid;

namespace UPMC_App.Droid
{
    [Activity(Label = "UPMC_App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            CrossCurrentActivity.Current.Init(this, bundle);
            
            CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);
            CrossFingerprint.SetDialogFragmentType<MyFingerprintCustomDialogFragment>();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            SvgImageRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, bundle);

            LoadApplication(new App());
        }

        /// <summary>
        /// For Xamarin.Essentials initialization (get permissions requests in runtime)
        /// </summary>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

}

