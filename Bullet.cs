using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Asteroids
{
	/// <summary>
	/// Summary description for Bullet.
	/// </summary>
	public class Bullet
	{
		public Point pPosition;
		
		private bool bActive = false;
		private int iSpeed;
		private float dDirection;
		private bool bCollisionStop = false;
		private int iMaxReturns = 4;
		private int iCurrentReturn;
		private int iSpeed;


		// Attributes
		protected int iDiameter = 5;


		public Bullet(int _startX, int _startY, int _speed, float _direction)
		{
			pPosition = new Point();
			pPosition.X = _startX;
			pPosition.Y = _startY;

			iSpeed = _speed;
			dDirection = _direction;
			iCurrentReturn = 0;
		}
		public void Draw(Graphics _g)
		{
			if (!bActive)
				return;

			_g.FillEllipse(new SolidBrush(Color.OrangeRed),pPosition.X,pPosition.Y,iDiameter,iDiameter);
		}
		public int Speed
		{
			get 
			{
				return iSpeed;
			}
			set
			{
				iSpeed = value;
			}
		}
		public bool StayInPlay
		{
			get 
			{
				return bCollisionStop;
			}
			set
			{
				bCollisionStop = value;
			}
		}
		public bool Active
		{
			get 
			{
				return bActive;
			}
			set
			{
				bActive = value;
			}
		}
		public void Move()
		{
			int iNewX = (int)(iSpeed * System.Math.Sin((System.Math.PI*dDirection)/180));  // uses radians
			int iNewY = -(int)(iSpeed * System.Math.Cos((System.Math.PI*dDirection)/180));

			pPosition.X = pPosition.X + iNewX;
			pPosition.Y = pPosition.Y + iNewY;
		}
		public bool InRectangle(Rectangle _rectTest)
		{
			if (pPosition.X < _rectTest.X)
			{
				if (iCurrentReturn < iMaxReturns)
				{
					if (bCollisionStop)
						pPosition.X = _rectTest.X; // move to X origin
					else
						pPosition.X = _rectTest.Right - -(pPosition.X); // move to X limit
					iCurrentReturn++;
				}else return true;
			}
			if (pPosition.X > _rectTest.Right)
			{
				if (iCurrentReturn < iMaxReturns) 
				{
					if (bCollisionStop)
						pPosition.X = _rectTest.Right - iDiameter; // move to X limit
					else
						pPosition.X = _rectTest.X + (_rectTest.Right - pPosition.X); 
					iCurrentReturn++;
				}else return true;
			}
				
			if (pPosition.Y < _rectTest.Y) 
			{
				if (iCurrentReturn < iMaxReturns)
				{
					if (bCollisionStop)
						pPosition.Y = _rectTest.Y; // move to Y origin
					else
						pPosition.Y = _rectTest.Bottom - -(pPosition.Y); // to X limit
					iCurrentReturn++;
				}else return true;
			}
				
			if (pPosition.Y > _rectTest.Bottom)
			{
				if (iCurrentReturn < iMaxReturns)
				{
					if (bCollisionStop)
						pPosition.Y = _rectTest.Bottom - iDiameter; // move to Y limit
					else
						pPosition.Y = _rectTest.Y + (_rectTest.Bottom - pPosition.Y);
					iCurrentReturn++;
				}else return true;
			}
			return false;
		}
		public Rectangle GetBounds()
		{
			return new Rectangle(pPosition.X, pPosition.Y, iDiameter, iDiameter);
		}
		public bool IntersectsWith(Rectangle _rect, Bitmap _bitmap) // check collision at pixel level
		{
			Rectangle rIntersect = new Rectangle(_rect.X,_rect.Y,_rect.Width,_rect.Height);

			Rectangle rTest = this.GetBounds();
			
			// check intersection exists
			if (!rIntersect.IntersectsWith(rTest))
				return false;
			// set rectangle to intersection
			rIntersect.Intersect(rTest);

			// GDI+ : the return format is BGR, NOT RGB.
			// only source bitmap is treated since the bullet doesn't have a bitmap
			BitmapData bmData2 = _bitmap.LockBits(new Rectangle(rIntersect.X - _rect.X,rIntersect.Y - _rect.Y, rIntersect.Width, rIntersect.Height),ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

			// bitmap height in bits
			int stride = bmData2.Stride;
			// bitmap start point
			System.IntPtr Scan1 = bmData2.Scan0;

			unsafe
			{
				// pointer to byte
				byte * p1 = (byte *)Scan1;

				int nOffset = stride - rIntersect.Width*3;
	
				for(int y=0;y<rIntersect.Height;++y)
				{
					for(int x=0; x < rIntersect.Width; ++x )
					{
						if (p1[0] != 0 &&
							p1[1] != 0 &&
							p1[2] != 0)
						{
							_bitmap.UnlockBits(bmData2);
							return true;
						}
						// move pointer to next pixel, each pixel has 3 bytes
						p1 += 3;
					}
					// move pointer to next column
					p1 += nOffset;
				}
			}

			_bitmap.UnlockBits(bmData2);

			return false;			
		}
	}
}
