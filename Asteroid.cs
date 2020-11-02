using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Asteroids
{
	/// <summary>
	/// Summary description for Asteroid.
	/// </summary>
	public class Asteroid
	{
		public Point pPosition = new Point();

		private int iSize;
		private int iDiameter;
		private Bitmap imgAsteroid = null;
		private Bitmap imgExplosion = null;
		private float fDirection;
		private int iSpeed;
		private bool bCollisionStop = false;
		private bool bActive;
		private int iExplodeSeq = 0;
		private int iLastExplosion = 17;
		private string sAsteroidExFileSuffix = "379041g";
		private string iMessage = "Collision";

		public Asteroid()
		{
			// TODO: fully implement
			pPosition.X = 50;
			pPosition.Y = 50;
			iSize = 1;
			iSpeed = 0;
			fDirection = 0;
			bActive = true;

		    Random rand = new Random();

			if (imgAsteroid == null)
			{
				imgAsteroid = (Bitmap)Image.FromFile("Asteroid" + rand.Next(3) + ".gif");
			}
			if (imgExplosion == null)
			{
				imgExplosion = (Bitmap)Image.FromFile(".gif");
			}
			iDiameter = imgAsteroid.Width;
		}

		public Asteroid(int _startX, int _startY, int _size, float _direction)
		{
			pPosition.X = _startX;
			pPosition.Y = _startY;
			iSize = _size;
			iSpeed = _size * 2;
			fDirection = _direction;
			bActive = true;

			Random rand = new Random();

			if (imgAsteroid == null)
			{
				imgAsteroid = (Bitmap)Image.FromFile("Asteroid" + rand.Next(3) + ".gif");
			}

			iDiameter = imgAsteroid.Width/_size;
			imgAsteroid.MakeTransparent(Color.Black);
		}
		public void Draw(Graphics _g)
		{
			if (iExplodeSeq == 0) 
			{
				Rectangle rectPath = new Rectangle(pPosition.X, pPosition.Y, iDiameter, iDiameter);
				// _g.FillEllipse(new SolidBrush(Color.Beige),rectPath);
				_g.DrawImage(imgAsteroid, rectPath,0,0,imgAsteroid.Width,imgAsteroid.Height, GraphicsUnit.Pixel);
			}
			else
			{
				DrawExplosion(_g);
				iExplodeSeq++;
				if (iExplodeSeq > iLastExplosion)
					bActive=false;
			}
		}
		private void DrawExplosion(Graphics _g)
		{
			Rectangle rectPath = new Rectangle(pPosition.X, pPosition.Y, iDiameter, iDiameter);
			
			imgExplosion = (Bitmap)Image.FromFile(sAsteroidExFileSuffix + iExplodeSeq + ".gif");
			imgExplosion.MakeTransparent(Color.Black);
			
			_g.DrawImage(imgExplosion, rectPath,0,0,imgExplosion.Width,imgExplosion.Height, GraphicsUnit.Pixel);
		}
		public Rectangle GetBounds()
		{
			return new Rectangle(pPosition.X, pPosition.Y, iDiameter, iDiameter);
		}
		public void Move()
		{
			// use trig to work out new value for X & Y based on Speed and direction vector
			int iNewX = (int)(iSpeed * System.Math.Sin((System.Math.PI*fDirection)/180));  // uses radians
			int iNewY = -(int)(iSpeed * System.Math.Cos((System.Math.PI*fDirection)/180));

			pPosition.X = pPosition.X + iNewX;
			pPosition.Y = pPosition.Y + iNewY;
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
		public int Size
		{
			get 
			{
				return iSize;
			}
			set
			{
				iSize = value;
				iDiameter = imgAsteroid.Width/iSize;
			}
		}
		public float Direction
		{
			get
			{
				return fDirection;
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
		public bool Exploding
		{
			get 
			{
				if (iExplodeSeq == 0)
				    return false;
				else
					return true;
			}
			set
			{
				iExplodeSeq = 1;
			}
		}
		public bool InRectangle(Rectangle _rectTest)
		{
			bool bHit = true;
			if (pPosition.X < _rectTest.X)
			{
				if (bCollisionStop)
					pPosition.X = _rectTest.X; // move to X origin
				else
					pPosition.X = _rectTest.Right - -(pPosition.X); // move to X limit
				bHit = false;
			}
			if (pPosition.X > _rectTest.Right)
			{
				if (bCollisionStop)
					pPosition.X = _rectTest.Right - iDiameter; // move to X limit
				else
					pPosition.X = _rectTest.X + (_rectTest.Right - pPosition.X); 
				bHit = false;
			}
				
			if (pPosition.Y < _rectTest.Y) 
			{
				if (bCollisionStop)
					pPosition.Y = _rectTest.Y; // move to Y origin
				else
					pPosition.Y = _rectTest.Bottom - -(pPosition.Y); // to X limit
				bHit = false;
			}
				
			if (pPosition.Y > _rectTest.Bottom)
			{
				if (bCollisionStop)
					pPosition.Y = _rectTest.Bottom - iDiameter; // move to Y limit
				else
					pPosition.Y = _rectTest.Y + (_rectTest.Bottom - pPosition.Y); 
				bHit = false;
			}
			return bHit;
		}
		public Point Front()
		{

			// get the first point outside asteroid radius on the middle front
			Point pFront = new Point();
			pFront.X = pPosition.X + iDiameter/2 + (int)((iDiameter + 10) * System.Math.Sin((System.Math.PI*fDirection)/180));  // uses radians
			pFront.Y = pPosition.Y + iDiameter/2 + -(int)((iDiameter + 10) * System.Math.Cos((System.Math.PI*fDirection)/180));

			return pFront;
		}
		public Point back()
		{
			float fNewDirection;
			if ((fDirection + 180)> 359)
                      //nISHITA 0 AT 18....nAVY pIER...nISHITA PREDATOR...ANYTIME HAHAHA ANYTIME....
				fNewDirection = fDirection + 180 - 359;
			else
				fNewDirection = fDirection;

			// get the first point outside asteroid radius on the middle front
			Point pBack = new Point();
			pBack.X = pPosition.X + iDiameter/2 + (int)((iDiameter + 10) * System.Math.Sin((System.Math.PI*fNewDirection)/180));  // uses radians
			pBack.Y = pPosition.Y + iDiameter/2 + -(int)((iDiameter + 10) * System.Math.Cos((System.Math.PI*fNewDirection)/180));

			return pBack;
		}
		public Point Right()
		{
			float fNewDirection;
			if ((fDirection + 90)> 359)
				fNewDirection = fDirection + 90 - 359;
			else
				fNewDirection = fDirection;

			// get the first point outside asteroid radius on the middle front
			Point pRight = new Point();
			pRight.X = pPosition.X + iDiameter/2 + (int)((iDiameter + 10) * System.Math.Sin((System.Math.PI*fNewDirection)/180));  // uses radians
			pRight.Y = pPosition.Y + iDiameter/2 + -(int)((iDiameter + 10) * System.Math.Cos((System.Math.PI*fNewDirection)/180));

			return pRight;
		}
		public Point Left()
		{
			float fNewDirection;
			if ((fDirection + 270)> 359)
                      ////WSU 0 AT 27
				fNewDirection = fDirection + 270 - 359;
			else
				fNewDirection = fDirection;

			// get the first point outside asteroid radius on the middle front
			Point pLeft = new Point();
			pLeft.X = pPosition.X + iDiameter/2 + (int)((iDiameter + 10) * System.Math.Sin((System.Math.PI*fNewDirection)/180));  // uses radians
			pLeft.Y = pPosition.Y + iDiameter/2 + -(int)((iDiameter + 10) * System.Math.Cos((System.Math.PI*fNewDirection)/180));

			return pLeft;
		}
		public Point MiddlePointOnFace(float _direction)
		{
			// given an angle work out middle point of corresponding facing
			float fNewDirection;
			if (_direction > 359)
				fNewDirection = _direction - 359;
			else
				fNewDirection = _direction;

			// get the first point outside asteroid radius on the middle of facing
			Point pMiddle = new Point();
			pMiddle.X = pPosition.X + iDiameter/2 + (int)((iDiameter + 10) * System.Math.Sin((System.Math.PI*fNewDirection)/180));  // uses radians
			pMiddle.Y = pPosition.Y + iDiameter/2 + -(int)((iDiameter + 10) * System.Math.Cos((System.Math.PI*fNewDirection)/180));

			return pMiddle;
		}

		public Bitmap AsteroidImage
		{
			get
			{
				return new Bitmap(imgAsteroid,iDiameter,iDiameter);
			}
		}
	}
}
