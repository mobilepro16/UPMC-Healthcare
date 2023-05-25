﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using UPMC_App.iOS;
using UPMC_App.MyControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HtmlFormattedLabel), typeof(HtmlFormattedLabelRenderer))]
namespace UPMC_App.iOS
{
    public class HtmlFormattedLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (HtmlFormattedLabel)Element;
            if (view == null) return;

            //Original Credits : https://forums.xamarin.com/discussion/23670/how-to-display-html-formatted-text-in-a-uilabel
            var attr = new NSAttributedStringDocumentAttributes();
            var nsError = new NSError();
            attr.DocumentType = NSDocumentType.HTML;

            Control.AttributedText = new NSAttributedString(view.Text, attr, ref nsError);

        }
    }
}