using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace Sample
{
	public partial class DrawableImageView : UIView
	{
		UIBezierPath path;
		PointF cropPlacement;
		UIColor currentStrokeColor = UIColor.Black;
		float currentStrokeWidth = 2.0f;
		UIImage incrementalImg = new UIImage ();
		UIImage originalImg;
		CustomEditorView editorHandle;
		UIImage croppedArea;
		bool croppedAreaPlaced = false;
		PointF[] pts = new PointF[5];
		int ctr;
		float dx = 0;
		float dy = 0;
		UIButton placeButton = new UIButton();
		private UIImage img = null;

		public DrawableImageView (IntPtr handle) : base (handle)
		{
			path = new UIBezierPath ();
			this.MultipleTouchEnabled = false;
			path.LineWidth = currentStrokeWidth;
			cropPlacement = new PointF (Bounds.Width / 2, Bounds.Height / 2);
			//AddGestureRecognizer (pan);
		}
		public bool IsCroppedArea(){


			return (croppedArea != null);
		}
		public void PlaceButtonPressed(){

			croppedAreaPlaced = true;

		}
		public override void Draw(RectangleF rect){
			base.Draw (rect);
			SetCurrentStrokeColor ();
			using (CGContext g = UIGraphics.GetCurrentContext ()) {
				g.ScaleCTM (1, -1);
				g.TranslateCTM (0, -Bounds.Height);
				if (originalImg != null) {
					g.DrawImage (rect, originalImg.CGImage);
				}
				if (img != null) {
					g.ScaleCTM (1, -1);
					g.TranslateCTM (0, -Bounds.Height);
					g.DrawImage (rect, img.CGImage);
				}
				if (croppedAreaPlaced) {
					if (croppedArea != null) {
						g.DrawImage (new RectangleF (cropPlacement.X, cropPlacement.Y, croppedArea.CGImage.Width, croppedArea.CGImage.Height), croppedArea.CGImage);
					}
					if (incrementalImg != null) {
						g.ScaleCTM (1, -1);
						g.TranslateCTM (0, -Bounds.Height);
						incrementalImg.Draw (rect);
					}
				} else {
					if (incrementalImg != null) {
						g.ScaleCTM (1, -1);
						g.TranslateCTM (0, -Bounds.Height);
						incrementalImg.Draw (rect);
					}
					if (croppedArea != null) {
						g.DrawImage (new RectangleF (cropPlacement.X, cropPlacement.Y, croppedArea.CGImage.Width, croppedArea.CGImage.Height), croppedArea.CGImage);
					}
				}
				currentStrokeColor.SetStroke ();
				path.Stroke ();
			}

		}
		public override void TouchesBegan (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			ctr = 0;
			UITouch touch = touches.AnyObject as UITouch;
			PointF p = touch.LocationInView (this);
			pts[0] = new PointF(p.X,Bounds.Height-p.Y);
			dx = 0;
			dy = 0;
		}
		public override void TouchesMoved (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
			UITouch touch = touches.AnyObject as UITouch;
			PointF p = touch.LocationInView (this);
			if (!croppedAreaPlaced) {
					

				if (dx == 0)
					dx = p.X - cropPlacement.X;

				if (dy == 0)
					dy = Bounds.Height - p.Y - cropPlacement.Y;

				var p1 = new PointF (p.X - dx,Bounds.Height-p.Y - dy);

				cropPlacement = p1;
				SetNeedsDisplay ();
			} if(croppedArea == null || croppedAreaPlaced) {
				ctr++;
				pts [ctr] = new PointF (p.X, Bounds.Height - p.Y);
				if (ctr == 4) {
					pts [3] = new PointF (((float)pts [2].X + (float)pts [4].X) / (float)2.0, ((float)pts [2].Y + (float)pts [4].Y) / (float)2.0); // move the endpoint to the middle of the line joining the second control point of the first Bezier segment and the first control point of the second Bezier segment 

					path.MoveTo (pts [0]);
					path.AddCurveToPoint (pts [3], pts [1], pts [2]); // add a cubic Bezier from pt[0] to pt[3], with control points pt[1] and pt[2]

					this.SetNeedsDisplay ();
					// replace points and get ready to handle the next segment
					pts [0] = pts [3]; 
					pts [1] = pts [4]; 
					ctr = 1;
				}
			}
		}
		public void SetCurrentStrokeColor(){

			String selected = editorHandle.Selected;
			if (selected.Equals ("Orange")) {
				currentStrokeColor = UIColor.Orange;

			} else if (selected.Equals ("Blue")) {
				currentStrokeColor = UIColor.Blue;

			} else if (selected.Equals ("Green")) {
				currentStrokeColor = UIColor.Green;

			} else if (selected.Equals ("Red")) {
				currentStrokeColor = UIColor.Red;

			}
		}
		public UIImage GetLayer(){
			UIGraphics.BeginImageContextWithOptions (new SizeF (Bounds.Width, Bounds.Height), false, (float)0.0);
			currentStrokeColor.SetStroke ();
			if (incrementalImg == null) {
				UIBezierPath bp = UIBezierPath.FromRect (new RectangleF (0, 0, Bounds.Width, Bounds.Height));
				UIColor.Clear.SetFill ();
				bp.Fill ();
			} else {
				incrementalImg.Draw (new PointF (0, 0));
				if (croppedArea!=null) {
					UIGraphics.GetCurrentContext().ScaleCTM (1, -1);
					UIGraphics.GetCurrentContext().TranslateCTM (0, -Bounds.Height);
					croppedArea.Draw (new PointF(cropPlacement.X,Bounds.Height-cropPlacement.Y));

				}
			}
			path.Stroke ();
			var returnval =  UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return returnval;
		}
		public override void TouchesEnded (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			this.DrawBitmap ();
			this.SetNeedsDisplay ();
			path.RemoveAllPoints ();
			ctr = 0;
		}
		public override void TouchesCancelled (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			this.TouchesEnded (touches, evt);
		}
		public void DrawBitmap(){
			UIGraphics.BeginImageContextWithOptions (new SizeF (Bounds.Width, Bounds.Height), false, (float)0.0);
			currentStrokeColor.SetStroke ();
			if (incrementalImg == null) {
				UIBezierPath bp = UIBezierPath.FromRect (new RectangleF (0, 0, Bounds.Width, Bounds.Height));
				UIColor.Clear.SetFill ();
				bp.Fill ();
			} else {
				incrementalImg.Draw (new PointF (0, 0));
				if (croppedArea!=null) {

					//croppedArea.Draw (cropPlacement);

				}
			}
			path.Stroke ();
			incrementalImg = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			


		}
		public UIImage Image{
			get{
				return img;
			}
			set{
				img = value;
			}
		}
		public float CurrentStrokeWidth{
			get{

				return currentStrokeWidth;
			}
			set{
				currentStrokeWidth = value;
				path.LineWidth = currentStrokeWidth;
			}
		}
		public UIImage OriginalImage{

			get{

				return originalImg;

			}
			set{
				originalImg = value;

			}

		}
		public CustomEditorView EditorHandle{
			get{

				return editorHandle;
			}
			set{
				editorHandle = value;
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
		public UIButton PlaceButton{

			get{ 
				return placeButton;
			}
			set{
				placeButton = value;
			}


		}
	}
}


