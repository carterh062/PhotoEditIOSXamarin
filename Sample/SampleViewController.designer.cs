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
	[Register ("SampleViewController")]
	partial class SampleViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		CustomEditorView EditorView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		DrawableImageView MainImageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton PlaceButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton PostButton { get; set; }

		[Action ("PlaceButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void PlaceButton_TouchUpInside (UIButton sender);

		[Action ("PostButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void PostButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (EditorView != null) {
				EditorView.Dispose ();
				EditorView = null;
			}
			if (MainImageView != null) {
				MainImageView.Dispose ();
				MainImageView = null;
			}
			if (PlaceButton != null) {
				PlaceButton.Dispose ();
				PlaceButton = null;
			}
			if (PostButton != null) {
				PostButton.Dispose ();
				PostButton = null;
			}
		}
	}
}
