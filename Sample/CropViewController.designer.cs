// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace Sample
{
	[Register ("CropViewController")]
	partial class CropViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton AttachButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		CroppableImageView croppableView { get; set; }

		[Action ("AttachButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void AttachButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (AttachButton != null) {
				AttachButton.Dispose ();
				AttachButton = null;
			}
			if (croppableView != null) {
				croppableView.Dispose ();
				croppableView = null;
			}
		}
	}
}
