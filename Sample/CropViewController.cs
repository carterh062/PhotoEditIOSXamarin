using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace Sample
{
	partial class CropViewController : UIViewController
	{
		UIImage croppedImage = new UIImage();
		SampleViewController sampleController;
		public CropViewController (IntPtr handle) : base (handle)
		{

		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			croppableView.CroppedImage = croppedImage;
		}

		partial void AttachButton_TouchUpInside (UIButton sender)
		{
			sampleController.CroppedArea = croppableView.CroppedArea;
			sampleController.CheckIfBeingEdited();
			sampleController.SetImageViewNeedsDisplay();
			NavigationController.PopToViewController(sampleController,true);
		}
		public UIImage CroppedImage{
			get{
				return croppedImage;
			}
			set{
				croppedImage = value;
			}



		}
		public SampleViewController SampleController{

			get{
				return sampleController;

			}
			set{

				sampleController = value;

			}


		}
	}
}
