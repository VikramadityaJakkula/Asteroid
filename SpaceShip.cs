using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace Asteroids
{
	/// <summary>
	/// Summary description for Ship.
	/// </summary>
	public class SpaceShip
	{
		public Point Pposition;
		private Rectangle rectPath;
		private double dSpeed = 0;
		private float fRotAngle = 0;
		private float fDirectionAngle = 0;
		private bool bActive;
		private bool bAccelerating;
		
		// Space Ship attributes
		protected Bitmap imgShip = null;
		protected Bitmap imgExplosion = null;
		protected int iTopSpeed;
		protected int iThrust;
		protected int iMass;
		protected int iTurnRatio;
		protected string iMessage;

		public Bullet[] BulletArray;


		public SpaceShip(int _startX, int _startY)
		{ 
			Pposition.X = _startX;
			Pposition.Y = _startY;

			iTopSpeed = 50;
			iThrust = 50;
			iMass = 5;
			iTurnRatio = 10; 
			BulletArray = new Bullet[30];
			bActive = true;

			if (imgShip  == null)
			{
				imgShip = (Bitmap)Image.FromFile("Falcon.gif");
			}

			imgShip.MakeTransparent(Color.Black);
		}

		public SpaceShip(int _startX, int _startY, string _imgFileName, int _topSpeed, int _mass, int _thrust, int _turnRatio, int _fireRate)
		{
			Pposition.X = _startX;
			Pposition.Y = _startY;

			iTopSpeed = _topSpeed;
			iMass = _mass;
			iThrust = _thrust;
			iTurnRatio = _turnRatio;
			BulletArray = new Bullet[_fireRate];

			if (imgShip == null)
			{
				imgShip = (Bitmap)Image.FromFile(_imgFileName);
			}
			
		}

		public int Mass
		{
			get
			{
				return iMass;
			}
		}

		public double Speed
		{
			get 
			{
				return dSpeed;
			}
			set
			{
				dSpeed = value;
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
		public bool Accelerating
		{
			get 
			{
				return bAccelerating;
			}
		}
		public float Facing
		{
			get 
			{
				return fRotAngle;
			}
		}

		public float MoveDirection
		{
			get 
			{
				return fDirectionAngle;
			}
		}

		public void DrawShip(Graphics _g)
		{

			// center of rectangle
		    Point Pcenter = new Point(rectPath.X+rectPath.Width/2,rectPath.Y+rectPath.Height/2);

			_g.TranslateTransform(Pcenter.X, Pcenter.Y);
			_g.RotateTransform(fRotAngle);

			// offset rectangle co-ordinates to find its original position
			rectPath.Offset(-Pcenter.X, -Pcenter.Y);

			// draw image in rectangle
			_g.DrawImage(imgShip, rectPath,0,0,imgShip.Width,imgShip.Height, GraphicsUnit.Pixel);
			if (bAccelerating) 
			{ // draw thrust 
				_g.FillEllipse(new SolidBrush(Color.Yellow),rectPath.Left + rectPath.Width/2 - 3,rectPath.Bottom,5,14);
				_g.FillEllipse(new SolidBrush(Color.Red),rectPath.Left + rectPath.Width/2 - 2,rectPath.Bottom,3,6);
				bAccelerating = false;
			}

			
			_g.ResetTransform();

			// rectangle to place image
			
			// _g.DrawRectangle(new Pen(Color.Red),Pposition.X,Pposition.Y,imgShip.Width,imgShip.Height);

		}
		public void RotateLeft()
		{
 			fRotAngle -= iTurnRatio;
			if (fRotAngle < 0)
				fRotAngle = 360 - iTurnRatio;	
		}

		public void RotateRight()
		{
			fRotAngle += iTurnRatio;
			if (fRotAngle >= 360)
				fRotAngle = 0;
		}
		public void Accelerate()
		{	
			if (fDirectionAngle != fRotAngle)
			{
				// this part works out the new direction of movement based on inertia and thrust.
				// all trig methods use radians so need to convert to degrees... 
				// might be easier if everything was in radians but I can't get my head round em.

					// thrust destination currently based on Acceleration but should be seperate
					double dRotX = (iThrust/iMass * System.Math.Sin(System.Math.PI*fRotAngle/180));  
					double dRotY = -(iThrust/iMass * System.Math.Cos(System.Math.PI*fRotAngle/180));

					// ship motion destination
					double dDestX = (dSpeed * System.Math.Sin(System.Math.PI*fDirectionAngle/180));  // uses radians
					double dDestY = -(dSpeed * System.Math.Cos(System.Math.PI*fDirectionAngle/180));

					// combined destination
					double dNewX = dDestX + dRotX;
					double dNewY = dDestY + dRotY;

					// get new speed
					dSpeed = System.Math.Sqrt((dNewX * dNewX) + (dNewY * dNewY));
					if (dSpeed > iTopSpeed)
						dSpeed = iTopSpeed;

					// get new direction of movement
					if (dNewX == 0.0)
						fDirectionAngle = 0;
					else
					{ // establish quadrant
						if (dNewX > 0 && dNewY > 0) 
						fDirectionAngle = (float)(System.Math.Atan(dNewY/dNewX)*(180/System.Math.PI) + 90);
						else if (dNewX > 0)
						fDirectionAngle = (float)(System.Math.Atan(dNewX/-dNewY)*(180/System.Math.PI));
						else if (dNewY > 0)
						fDirectionAngle = (float)(System.Math.Atan(-dNewX/dNewY)*(180/System.Math.PI) + 180);
						else
						fDirectionAngle = (float)(System.Math.Atan(-dNewY/-dNewX)*(180/System.Math.PI) + 270);
					}
					//* Debug
//					TextWriter output = File.AppendText("debug.txt");
//					output.WriteLine("dNewY: "+dNewY + @"|dNewX: "+dNewX + "|fdir: " + fDirectionAngle );
//					output.Close();
					//* End
				
			} 
			else 
			{ 
				dSpeed += iThrust/iMass;
			}
			// set true to draw thrust
			bAccelerating = true;

		}
//		private void Slow() // no brakes
//		{
//			fDirectionAngle = fRotAngle;
//			dSpeed -= iDecceleration;
//			
//			if (dSpeed < -(iTopSpeed/2))
//				dSpeed = -(iTopSpeed/2);
//		}
		public Rectangle GetBounds()
		{
			return rectPath; // new Rectangle(Pposition.X, Pposition.Y, imgShip.Width, imgShip.Height);
		}
		public bool InRectangle(Rectangle _rectTest)
		{
			bool bHit = true;
			if (Pposition.X < _rectTest.X)
			{
				Pposition.X = _rectTest.Right - -(rectPath.X); // move to X limit
				bHit = false;
			}
			if (Pposition.X > _rectTest.Right)
			{
				Pposition.X = _rectTest.X + (_rectTest.Right - rectPath.X); 
				bHit = false;
			}
				
			if (Pposition.Y < _rectTest.Y) 
			{
				Pposition.Y = _rectTest.Bottom - -(rectPath.Y); // move to X limit
				bHit = false;
			}
				
			if (Pposition.Y > _rectTest.Bottom)
			{
				Pposition.Y = _rectTest.Y + (_rectTest.Bottom - rectPath.Y); 
				bHit = false;
			}
			return bHit;
		}
		public void Move()
		{
			// Apply drag
			if (dSpeed > 0) 
			{
				dSpeed -= (dSpeed/iMass)/10;
			}

			// use trig to work out new value for X & Y based on Velocity vector
			int iNewX = (int)(dSpeed * System.Math.Sin(System.Math.PI*fDirectionAngle/180));  // uses radians
			int iNewY = -(int)(dSpeed * System.Math.Cos(System.Math.PI*fDirectionAngle/180)); // invert y

			Pposition.X = Pposition.X + iNewX;
			Pposition.Y = Pposition.Y + iNewY;

			rectPath = new Rectangle(Pposition.X, Pposition.Y, imgShip.Width, imgShip.Height);
		}
		public void Fire()
		{
			for(int j = 0;j<BulletArray.Length;j++)
			{
				if((BulletArray[j] == null) || (!BulletArray[j].Active))
				{
					BulletArray[j]= new Bullet(Front().X,Front().Y,(int)dSpeed + 10,fRotAngle);
					BulletArray[j].Active = true;
					j = BulletArray.Length;
				}
			}
		}
		public Point Front()
		{
			// get radius of circle that describes motion of ship square
			int iRadius = (int)Math.Sqrt(Math.Pow(imgShip.Height/2,2) + Math.Pow(imgShip.Width/2,2));

			// get the first point outside ship radius on the middle front
			Point pFront = new Point();
			pFront.X = Pposition.X + imgShip.Width/2 + (int)((iRadius + 1) * System.Math.Sin((System.Math.PI*fRotAngle)/180));  // uses radians
			pFront.Y = Pposition.Y + imgShip.Height/2 + -(int)((iRadius + 1) * System.Math.Cos((System.Math.PI*fRotAngle)/180));

			return pFront;
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
			// lock intersected section of bmps
			BitmapData bmData = imgShip.LockBits(new Rectangle(rIntersect.X - rTest.X,rIntersect.Y - rTest.Y, rIntersect.Width, rIntersect.Height),ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			BitmapData bmData2 = _bitmap.LockBits(new Rectangle(rIntersect.X - _rect.X,rIntersect.Y - _rect.Y, rIntersect.Width, rIntersect.Height),ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

			// bitmap height in bits
			int stride = bmData.Stride;
			// bitmap start point
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr Scan1 = bmData2.Scan0;

			unsafe
			{
				// pointer to byte
				byte * p = (byte *)Scan0;
				byte * p1 = (byte *)Scan1;

				int nOffset = stride - rIntersect.Width*3;
	
				for(int y=0;y<rIntersect.Height;++y)
				{
					for(int x=0; x < rIntersect.Width; ++x )
					{
						if (p[0] != 0 &&  // check if byte is black
							p[1] != 0 &&
							p[2] != 0 &&
							p1[0] != 0 &&
							p1[1] != 0 &&
							p1[2] != 0)
						{				  // a collision has occured
							imgShip.UnlockBits(bmData);
							_bitmap.UnlockBits(bmData2);
							return true;
						}
					// move pointer to next pixel, each pixel has 3 bytes
						p += 3;
						p1 += 3;;
					}
					// move pointer to next column
					p += nOffset;
					p1 += nOffset;
				}
			}

			imgShip.UnlockBits(bmData);
			_bitmap.UnlockBits(bmData2);

			return false;			
		}
		public Bitmap ShipImage
		{
			get
			{
				return imgShip;
			}
		}
	}
}
