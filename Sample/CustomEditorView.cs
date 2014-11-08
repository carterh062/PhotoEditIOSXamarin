using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.CoreAnimation;

namespace Sample
{
	public partial class CustomEditorView : UIView
	{
		RectangleF orangeCircle;
		RectangleF blueCircle;
		RectangleF greenCircle;
		RectangleF redCircle;
		RectangleF cropCircle;
		public UIImageView img;
		UIActionSheet cropOptions;
		String selected = "Orange";
		public CustomEditorView (IntPtr handle) : base (handle)
		{
			orangeCircle = new System.Drawing.RectangleF (2, (Bounds.Height / 2) - 15f, 15, 15);
			blueCircle = new System.Drawing.RectangleF (19, (Bounds.Height / 2) - 15f, 15, 15);
			greenCircle = new System.Drawing.RectangleF (36, (Bounds.Height / 2) - 15f, 15, 15);
			redCircle = new System.Drawing.RectangleF (53, (Bounds.Height / 2) - 15f, 15, 15);
			cropCircle = new System.Drawing.RectangleF (20, (Bounds.Height / 2) - 15f, 16, 16);
			img = new UIImageView (cropCircle);
			img.Image = UIImage.FromFile ("sample.jpg");
			img.Layer.MasksToBounds = true;
			img.Layer.CornerRadius = 7.5f;
			img.Layer.BorderWidth = 1.0f;
			img.Layer.BorderColor = UIColor.White.CGColor;
			Add (img);
			BackgroundColor = UIColor.Clear;
		}
		public override void Draw (System.Drawing.RectangleF rect)
		{
			base.Draw (rect);
			using (CGContext g = UIGraphics.GetCurrentContext ()) {
				g.ScaleCTM (-1, 1);
				g.TranslateCTM (-Bounds.Width, 0);
				UIColor.Black.SetStroke ();
				if(selected.Equals("Orange"))
					UIColor.White.SetStroke ();
				UIColor.Orange.SetFill ();
				g.AddEllipseInRect (orangeCircle);
				g.FillPath ();
				g.AddEllipseInRect (orangeCircle);
				g.StrokePath ();
				UIColor.Black.SetStroke ();
				if(selected.Equals("Blue"))
					UIColor.White.SetStroke ();
				UIColor.Blue.SetFill ();
				g.AddEllipseInRect (blueCircle);
				g.FillPath ();
				g.AddEllipseInRect (blueCircle);
				g.StrokePath ();
				UIColor.Black.SetStroke ();
				if(selected.Equals("Green"))
					UIColor.White.SetStroke ();
				UIColor.Green.SetFill ();
				g.AddEllipseInRect (greenCircle);
				g.FillPath ();
				g.AddEllipseInRect (greenCircle);
				g.StrokePath ();
				UIColor.Black.SetStroke ();
				if(selected.Equals("Red"))
					UIColor.White.SetStroke ();
				UIColor.Red.SetFill ();
				g.AddEllipseInRect (redCircle);
				g.FillPath ();
				g.AddEllipseInRect (redCircle);
				g.StrokePath ();
			}
		}
		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			var touch = touches.AnyObject as UITouch;
			PointF loc = touch.LocationInView(this);
			PointF scaledLoc = new PointF (Bounds.Width-loc.X, loc.Y);
			if (orangeCircle.Contains (scaledLoc)) {
				selected = "Orange";
				this.SetNeedsDisplay ();

			} else if (blueCircle.Contains (scaledLoc)) {
				selected = "Blue";
				this.SetNeedsDisplay ();

			} else if (greenCircle.Contains (scaledLoc)) {
				selected = "Green";
				this.SetNeedsDisplay ();

			} else if (redCircle.Contains (scaledLoc)) {
				selected = "Red";
				this.SetNeedsDisplay ();

			} else if (cropCircle.Contains (loc)) {
				img.Layer.BorderColor = UIColor.Black.CGColor;
				cropOptions.ShowInView (this);
			}

		}

		public CustomEditorView ReturnSelf(){
			return this;
		}
		public UIActionSheet CropOptions{
			get{
				return cropOptions;

			}
			set{

				cropOptions = value;
			}

		}
		public String Selected{

			get{

				return selected;
			}

		}
	}
}
