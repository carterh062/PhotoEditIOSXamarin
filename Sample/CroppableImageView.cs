using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace Sample
{
	public partial class CroppableImageView : UIView
	{
		UIImage croppedImage;
		UIImage croppedArea;
		RectangleF cropClick;
		RectangleF crop;
		UIPanGestureRecognizer pan;
		UIPinchGestureRecognizer pinch;
		UITapGestureRecognizer doubleTap;
		float cropWidth = 100;
		float cropHeight = 100;
		PointF lastPoint;
		bool firstDraw = true;
		public CroppableImageView (IntPtr handle) : base (handle)
		{
			float dx = 0;
			float dy = 0;
			pan = new UIPanGestureRecognizer ((gesture) => {
				if ((gesture.State == UIGestureRecognizerState.Began || gesture.State == UIGestureRecognizerState.Changed) && (gesture.NumberOfTouches == 1)) {

					var p0 = gesture.LocationInView (this);

					if (dx == 0)
						dx = p0.X - crop.X;

					if (dy == 0)
						dy = p0.Y - crop.Y;

					var p1 = new PointF (p0.X - dx, p0.Y - dy);

					Crop(p1);
				} else if (gesture.State == UIGestureRecognizerState.Ended) {
					dx = 0;
					dy = 0;
				}
			});
			float s0 = 1;

			pinch = new UIPinchGestureRecognizer ((gesture) => {
				float s = gesture.Scale;
				float ds = Math.Abs (s - s0);
				float sf = 0;
				const float rate = 0.5f;

				if (s >= s0) {
					sf = 1 + ds * rate;
				} else if (s < s0) {
					sf = 1 - ds * rate;
				}
				s0 = s;
				cropWidth = cropWidth *sf;
				cropHeight = cropHeight *sf;
				Crop();
				if (gesture.State == UIGestureRecognizerState.Ended) {
					s0 = 1;
				}
			});
			this.AddGestureRecognizer (pan);
			this.AddGestureRecognizer (pinch);
		}
		public override void Draw(RectangleF rect){
			base.Draw (rect);
			using (CGContext g = UIGraphics.GetCurrentContext ()) {
				g.ScaleCTM (1, -1);
				g.TranslateCTM (0, -Bounds.Height);
				if (firstDraw) {
					Crop (new PointF (Bounds.Width / 2, Bounds.Height / 2));
					firstDraw = false;
				}
				if (croppedImage != null) {
					g.DrawImage (rect, croppedImage.CGImage);
				}
				UIBezierPath transparentOverlay = UIBezierPath.FromRect (new RectangleF (0, 0, Bounds.Width, Bounds.Height));
				// following method will fill red color with alpha 0.4 last one in parameter list
				// first 3 are Red, Green and Blue Respectively.
				g.SetRGBFillColor (1, 1, 1, .4f);
				transparentOverlay.Fill ();
				transparentOverlay.Stroke ();
				if (croppedArea != null) {
					g.DrawImage (cropClick, croppedArea.CGImage);
				}
			}

		}

		public override void TouchesBegan (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			UITouch touch = touches.AnyObject as UITouch;
			PointF p = touch.LocationInView (this);
			//Crop (p);
		}
		public void Crop(PointF touch){
		
			lastPoint = touch;
			PointF scaledPoint = new PointF(lastPoint.X,Bounds.Height-lastPoint.Y);
			crop = new RectangleF (lastPoint.X, lastPoint.Y, cropWidth, -cropHeight);
			croppedArea = UIImage.FromImage(croppedImage.Scale(new SizeF(Bounds.Width,Bounds.Height)).CGImage.WithImageInRect (crop));
			cropClick = new RectangleF (scaledPoint, new SizeF (cropWidth, cropHeight));
			this.SetNeedsDisplay ();

		}
		public void Crop(){
			PointF scaledPoint = new PointF(lastPoint.X,Bounds.Height-lastPoint.Y);
			crop = new RectangleF (lastPoint.X, lastPoint.Y, cropWidth, -cropHeight);
			croppedArea = UIImage.FromImage(croppedImage.Scale(new SizeF(Bounds.Width,Bounds.Height)).CGImage.WithImageInRect (crop));
			cropClick = new RectangleF (scaledPoint, new SizeF (cropWidth, cropHeight));
			this.SetNeedsDisplay ();

		}
		public UIImage CroppedImage{
			get{
				return croppedImage;

			}
			set{
				croppedImage = value;

			}

		}
		public UIImage CroppedArea{
			get{

				return croppedArea;

			}
			set{

				croppedArea = value;


			}



		}

	}
}


