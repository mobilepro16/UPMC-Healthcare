using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using UPMC_App.Droid;
using UPMC_App.MyControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlFormattedLabel), typeof(HtmlFormattedLabelRenderer))]
namespace UPMC_App.Droid
{
    public class HtmlFormattedLabelRenderer:LabelRenderer
    {
        public HtmlFormattedLabelRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (HtmlFormattedLabel)Element;
            if (view == null) return;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                Control.SetText(Html.FromHtml(view.Text.ToString(), FromHtmlOptions.ModeLegacy), TextView.BufferType.Spannable);
            }
            else
            {
#pragma warning disable CS0618 // Type or member is obsolete
                Control.SetText(Html.FromHtml(view.Text.ToString()), TextView.BufferType.Spannable);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }
}