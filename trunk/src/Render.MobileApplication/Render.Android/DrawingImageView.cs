using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.IO;

public enum DrawingType {
	None,
	Pencil,
	SMark,
	DMark,
	HMark,
}

public class DrawingImageView : ImageView
{
	Paint	mPaint, cPaint;
	Bitmap  mBitmap;
	Canvas  mCanvas;
	Android.Graphics.Path    mPath;
	Paint   mBitmapPaint;

	Fragment _fragment;

	public DrawingImageView(Context context, Fragment fragment): base(context) {
		mPaint = new Paint();
		mPaint.AntiAlias = true;
		mPaint.Dither = true;
		mPaint.Color = Color.Yellow;
		mPaint.SetStyle (Paint.Style.Stroke);
		mPaint.StrokeJoin = Paint.Join.Round;
		mPaint.StrokeCap = Paint.Cap.Round;
		mPaint.StrokeWidth = 10;

		cPaint = new Paint ();
		cPaint.Color = Color.Yellow;
		cPaint.StrokeJoin = Paint.Join.Round;
		cPaint.StrokeCap = Paint.Cap.Round;
		cPaint.SetTypeface(Typeface.Default);
		cPaint.TextSize = 40;

		mPath = new Android.Graphics.Path();
		mBitmapPaint = new Paint();
		mBitmapPaint.Color = Color.Yellow;

		DrawingStatus = DrawingType.None;

		_fragment = fragment;
	}


	public DrawingType DrawingStatus { get; set; }

	public void ResetImage(int w, int h) {
		if (this.Drawable != null) {
			mBitmap = resizeImage (w, h);
		} else {
			mBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
		}
		mCanvas = new Canvas(mBitmap);
	}
	protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
	{
		base.OnSizeChanged (w, h, oldw, oldh);
		ResetImage (w, h);
	}
	public byte[] GetOriginalImage() {
		MemoryStream stream = new MemoryStream();
		((BitmapDrawable)this.Drawable).Bitmap.Compress (Bitmap.CompressFormat.Jpeg, 90, stream);
		var image = stream.ToArray ();

		return image;
	}
	public byte[] GetMarkedUpImage() {
		MemoryStream stream = new MemoryStream();
		mBitmap.Compress (Bitmap.CompressFormat.Jpeg, 90, stream);
		var image = stream.ToArray ();

		return image;
	}
	protected Bitmap resizeImage(int w, int h)
	{
		// load the origial Bitmap
		Android.Graphics.Bitmap BitmapOrg = null;

		BitmapOrg = ((BitmapDrawable)this.Drawable).Bitmap.Copy (Bitmap.Config.Argb8888, true);

		int width = BitmapOrg.Width;
		int height = BitmapOrg.Height;
		int newWidth = w;
		int newHeight = h;
		
		float scaleWidth = ((float) newWidth) / width;
		float scaleHeight = ((float) newHeight) / height;

		Android.Graphics.Matrix matrix = new Android.Graphics.Matrix();
		matrix.PostScale(scaleWidth, scaleHeight);
		Bitmap resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(BitmapOrg, 0, 0, width, height, matrix, true);
		return resizedBitmap;
	}

	public override void Draw (Canvas canvas)
	{
		base.Draw (canvas);

		canvas.DrawBitmap(mBitmap, 0, 0, mBitmapPaint);
		canvas.DrawPath(mPath, mPaint);
	}

	private float mX, mY;
	private static readonly float TOUCH_TOLERANCE = 4;

	private void touch_start(float x, float y) {
		mPath.MoveTo(x, y);
		mX = x;
		mY = y;
	}
	private void touch_move(float x, float y) {
		float dx = Math.Abs(x - mX);
		float dy = Math.Abs(y - mY);
		if (dx >= TOUCH_TOLERANCE || dy >= TOUCH_TOLERANCE) {
			mPath.QuadTo(mX, mY, (x + mX)/2, (y + mY)/2);
			mX = x;
			mY = y;
		}
	}
	private void touch_up() {
		mPath.LineTo(mX, mY);
		mCanvas.DrawPath(mPath, mPaint);

		// kill this so we don't double draw
		mPath.Reset();
	}

	public override bool DispatchTouchEvent (MotionEvent e)
	{
		return base.DispatchTouchEvent (e);
		//return true;
	}

	public static readonly int TAKING_PICTURE = 99;
	public override bool OnTouchEvent (MotionEvent e)
	{
		if (this.Drawable == null && MotionEventActions.Up == e.Action) {
			//_fragment.StartActivityForResult(new Intent ("android.media.action.IMAGE_CAPTURE"), TAKING_PICTURE);
		}

		float x = e.GetX ();
		float y = e.GetY();

		switch (DrawingStatus) {
		case DrawingType.Pencil:
			switch (e.Action) {
			case MotionEventActions.Down:
				touch_start (x, y);
				Invalidate ();
				break;
			case MotionEventActions.Move:
				touch_move (x, y);
				Invalidate ();
				break;
			case MotionEventActions.Up:
				touch_up ();
				Invalidate ();
				break;
			}
			break;
		case DrawingType.DMark:
			if (MotionEventActions.Up == e.Action) {
				mCanvas.DrawText ("D", x, y, cPaint);
				mPath.Reset ();
				Invalidate ();
			}
			break;
		case DrawingType.HMark:
			if (MotionEventActions.Up == e.Action) {
				mCanvas.DrawText ("H", x, y, cPaint);
				mPath.Reset ();
				Invalidate ();
			}
			break;
		case DrawingType.SMark:
			if (MotionEventActions.Up == e.Action) {
				mCanvas.DrawText ("S", x, y, cPaint);
				mPath.Reset ();
				Invalidate ();
			}
			break;
		}

		return true;
	}
}
