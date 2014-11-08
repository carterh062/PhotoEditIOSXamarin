using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Parse;
using System.Threading.Tasks;
using System.Net.Http;

namespace Sample
{
	public partial class SampleViewController : UIViewController
	{
		UIActionSheet cropOptions;
		UIImagePickerController imagePicker;
		CropViewController cropViewController;
		UIImage croppedArea;
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SampleViewController (IntPtr handle) : base (handle)
		{
			NavigationController.SetNavigationBarHidden (true, true); 
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
				base.ViewDidLoad ();
				ParseClient.Initialize("xxx",
					"xxx");
				GetImage (MainImageView);
				SetOriginalImage ();
				this.View.BringSubviewToFront (MainImageView);
				this.View.BringSubviewToFront (EditorView);
				MainImageView.EditorHandle = EditorView.ReturnSelf();
				cropOptions = new UIActionSheet ("Crop and Paste");
				cropOptions.AddButton ("Pick photo");
				cropOptions.AddButton ("Take photo");
				cropOptions.AddButton ("Cancel");
				cropOptions.Clicked += delegate(object a, UIButtonEventArgs b) {
				int clicked = b.ButtonIndex;
					if(clicked == 0){
						NavigationController.PresentModalViewController(imagePicker, true);
						EditorView.img.Layer.BorderColor = UIColor.White.CGColor;
						EditorView.SetNeedsDisplay();
					}
					else if(clicked == 1){
						EditorView.img.Layer.BorderColor = UIColor.White.CGColor;
						EditorView.SetNeedsDisplay();

					}
					else if(clicked == 2){
						cropOptions.Hidden = true;
						EditorView.img.Layer.BorderColor = UIColor.White.CGColor;
						EditorView.SetNeedsDisplay();
					}
				};
				imagePicker = new UIImagePickerController ();
				imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
				imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes (UIImagePickerControllerSourceType.PhotoLibrary);
				imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
				imagePicker.Canceled += Handle_Canceled;
				cropViewController = this.Storyboard.InstantiateViewController ("CropController") as CropViewController;
				EditorView.CropOptions = cropOptions;
				MainImageView.PlaceButton = PlaceButton;
				CheckIfBeingEdited ();
				//SendFile ();
				// Perform any additional setup after loading the view, typically from a nib.
		}

		public void CheckIfBeingEdited(){

			if (MainImageView.IsCroppedArea()) {

				PlaceButton.Hidden = false;

			} else {

				PlaceButton.Hidden = true;
			}


		}
		protected void Handle_FinishedPickingMedia (object sender, UIImagePickerMediaPickedEventArgs e)
		{
			// determine what was selected, video or image
			bool isImage = false;
			switch(e.Info[UIImagePickerController.MediaType].ToString()) {
			case "public.image":
				Console.WriteLine("Image selected");
				isImage = true;
				break;
			case "public.video":
				Console.WriteLine("Video selected");
				break;
			}

			// get common info (shared between images and video)
			NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
			if (referenceURL != null)
				Console.WriteLine("Url:"+referenceURL.ToString ());

			// if it was an image, get the other image info
			if(isImage) {
				// get the original image
				UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
				if(originalImage != null) {
					// do something with the image
					cropViewController.CroppedImage = originalImage;
					Console.WriteLine ("got the original image");
					//imageView.Image = originalImage; // display
				}
			} else { // if it's a video
				// get video url
				NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
				if(mediaURL != null) {
					Console.WriteLine(mediaURL.ToString());
				}
			}
			// dismiss the picker
			imagePicker.DismissModalViewControllerAnimated (true);
			cropViewController.SampleController = this;
			NavigationController.PushViewController (cropViewController, true);
		}
		void Handle_Canceled (object sender, EventArgs e) {
			imagePicker.DismissModalViewControllerAnimated(true);
		}
		public async void SetOriginalImage(){

			var query = ParseObject.GetQuery ("Original");
			ParseObject results = await query.FirstAsync();
			var bytesString = results.Get<ParseFile>("image");
			byte[] bitmap = await new HttpClient().GetByteArrayAsync(bytesString.Url);
			MainImageView.OriginalImage = ToImage (bitmap);
			MainImageView.SetNeedsDisplay ();



		}
		public async void SendFile(){
			ParseObject obj = new ParseObject ("Sample3");
			var image = MainImageView.GetLayer ();
			//var image = UIImage.FromFile ("sample.jpg");
			byte[] data = null;
			if (image == null) {
				data = null;
			} else {
				NSData nsdata = null;

				try {
					nsdata = image.AsPNG ();
					data = nsdata.ToArray();
				} catch (Exception) {

				} finally {
					if (image != null) {
						image.Dispose ();
						image = null;
					}
					if (nsdata != null) {
						nsdata.Dispose ();
						nsdata = null;
					}
				}
			}
			ParseFile file = new ParseFile("sample.png", data);
			await file.SaveAsync();
			obj ["image"] = file;
			await obj.SaveAsync ();
		}
		public static async Task<UIImage> GetImage(DrawableImageView imView){


			var query = ParseObject.GetQuery ("Sample3").OrderByDescending("createdAt");
			ParseObject results = await query.FirstAsync();
			var bytesString = results.Get<ParseFile>("image");
			byte[] bitmap = await new HttpClient().GetByteArrayAsync(bytesString.Url);
			imView.Image = ToImage (bitmap);
			imView.SetNeedsDisplay ();
			return ToImage(bitmap);
		}

		partial void PostButton_TouchUpInside (UIButton sender)
		{
			SendFile();
		}

		public static UIImage ToImage(byte[] data)
		{
			if (data==null) {
				return null;
			}
			UIImage image = null;
			try {

				image = new UIImage(NSData.FromArray(data));
				data = null;
			} catch (Exception ) {
				return null;
			}
			return image;
		}

		partial void PlaceButton_TouchUpInside (UIButton sender)
		{
			MainImageView.PlaceButtonPressed();
			PlaceButton.Hidden = true;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		public void SetImageViewNeedsDisplay(){

			MainImageView.SetNeedsDisplay ();

		}
		public UIImage CroppedArea {

			get{
				return croppedArea;
			
			}
			set{
				croppedArea = value;
				MainImageView.CroppedArea = croppedArea;
			}

		}
		#endregion
	}
}

