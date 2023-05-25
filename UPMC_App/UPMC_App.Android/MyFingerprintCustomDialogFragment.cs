using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.Fingerprint.Dialog;

namespace UPMC_App.Droid
{
    public class MyFingerprintCustomDialogFragment : FingerprintDialogFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            DefaultColor = Color.Argb(255, 160, 118, 177);
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            //view.Background = new ColorDrawable(Color.Magenta); // make it fancyyyy :D
            return view;
        }
    }
}