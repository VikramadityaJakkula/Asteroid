using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Runtime.InteropServices;


namespace Asteroids
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Timers.Timer timer1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private SpaceShip TheShip;
		private ArrayList AsteroidList;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem File;
		private System.Windows.Forms.MenuItem Start;
		private System.Windows.Forms.MenuItem Exit;
		private string sKeyPress = "";
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private bool bStarted;
		private int iAsteroidNo = 1;
		private int iTextHolder;
//		private int iExplodeSeq = 0;
//		private int iLastExplosion = 16;
//		private string sExplosionFileSuffix = "389044p";
		private bool bGameWin = true;
		public enum PlayFlags
		{ 
			Synchronously = 0x0000, 
			Asynchronously = 0x00010, 
			NoDefault = 0x0002, 
			Memory = 0x0004, 
			Loop = 0x0001, 
			NoStop = 0x00108, 
			NoWait = 0x00002000, 
			Alias = 0x00010000, 
			AliasId = 0x00110000, 
			Filename = 0x00020000, 
			Resource = 0x00060006, 
			Purge = 0x0037, 
			Application = 0x0080 
		}

		const int NoOfBabyAsteroids = 7;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;

		private Thread oThread = null;

		[DllImport("winmm.dll")]
		public static extern bool PlaySound(string lpszName, int hModule, PlayFlags dwFlags);

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// reduce flicker by enabling double buffering

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.timer1 = new System.Timers.Timer();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.File = new System.Windows.Forms.MenuItem();
			this.Start = new System.Windows.Forms.MenuItem();
			this.Exit = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 75;
			this.timer1.SynchronizingObject = this;
			this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.File,
																					  this.menuItem1,
																					  this.menuItem6});
			// 
			// File
			// 
			this.File.Index = 0;
			this.File.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				 this.Start,
																				 this.Exit});
			this.File.Text = "&File";
			// 
			// Start
			// 
			this.Start.Index = 0;
			this.Start.Text = "&Start";
			this.Start.Click += new System.EventHandler(this.Start_Click);
			// 
			// Exit
			// 
			this.Exit.Index = 1;
			this.Exit.Text = "E&xit";
			this.Exit.Click += new System.EventHandler(this.Exit_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem3,
																					  this.menuItem4,
																					  this.menuItem5});
			this.menuItem1.Text = "Asteroids";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "1  -";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "2";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "3";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.Text = "4";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 2;
			this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem7,
																					  this.menuItem8,
																					  this.menuItem9});
			this.menuItem6.Text = "Speed";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 0;
			this.menuItem7.Text = "Slowest";
			this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "Optimized";
			this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 2;
			this.menuItem9.Text = "Fast";
			this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(704, 430);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{

			Graphics g = e.Graphics;
			
			g.Clear(Color.Black);
			
			if (!bStarted)
			{
				String drawString = "Asteroids";
				// Create font and brush.
				Font drawFont = new Font("Verdana", 48);
				SolidBrush drawBrush = new SolidBrush(Color.Red);
				
				PointF drawPoint = new PointF(this.ClientRectangle.Width/4,this.ClientRectangle.Height - iTextHolder);
				// Draw string to screen.
				e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);

				drawString = "AURORA";
				// Create font and brush.
				drawFont = new Font("TIMES NEW ROMAN", 14);
				drawBrush = new SolidBrush(Color.White);
				
				drawPoint = new PointF(this.ClientRectangle.Width/4,this.ClientRectangle.Height - (iTextHolder - 90));
				// Draw string to screen.
				e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);

				if (iTextHolder > this.ClientRectangle.Height + 25)
					iTextHolder = 25;
				else
					iTextHolder = iTextHolder + 5;

				return;
			}
			
			Bullet[] BulletArray = TheShip.BulletArray;
			// Move Ship
			if(TheShip.Active)
			{
				TheShip.Move();
				// check ship is not out of bounds
				TheShip.InRectangle(this.ClientRectangle);
			}
			// Move Bullets
			for(int i=0;i<BulletArray.Length;i++)
			{
				// check if bullet is still in scope
				if (BulletArray[i] != null )
				{
					if (BulletArray[i].Active)
					{
						BulletArray[i].Move();
						// trim or reposition bullets
						if (BulletArray[i].InRectangle(this.ClientRectangle))
							BulletArray[i].Active = false;

						// test ship & bullet collision
						if (TheShip.Active)
						{
							if (BulletArray[i].IntersectsWith(TheShip.GetBounds(),TheShip.ShipImage))
							{
								BulletArray[i].Active = false;
								TheShip.Active = false;
							}
						}
					}
				}
			}
			// Move Asteroids
			foreach(Asteroid A in AsteroidList)
			{
				A.Move();
				A.InRectangle(this.ClientRectangle);
			}
			
			// check for collisions
			for(int j = 0;j<AsteroidList.Count;j++)
			{
				Asteroid A = (Asteroid)AsteroidList[j];
				// if exploding nothing to do
				if (A.Exploding==false)
				{
					// with bullets
					for(int i = 0;i<BulletArray.Length;i++)
					{
						if (BulletArray[i] != null)
						{
							if (BulletArray[i].Active)
							{
								if(BulletArray[i].IntersectsWith(A.GetBounds(),A.AsteroidImage))
								{
									A.Exploding=true;
									spawnMoreAsteroids(A);
									BulletArray[i].Active = false;
								}
							}
						}
					}

					// with ship
					if(TheShip.Active)
					{
						if (TheShip.IntersectsWith(A.GetBounds(),A.AsteroidImage))
						{
							A.Exploding=true;
							spawnMoreAsteroids(A);
							TheShip.Active=false;
						}
					}
				}
			}

			// trim any inactive asteroids
			for(int i = 0;i<AsteroidList.Count;i++)
			{
				Asteroid A = (Asteroid)AsteroidList[i];
				if (A.Active)
					A.Draw(g);
				else
				{
					AsteroidList.RemoveAt(i);
				}
			}

			for(int i = 0;i<BulletArray.Length;i++)
				if (BulletArray[i] != null)
					if (BulletArray[i].Active)
						BulletArray[i].Draw(g);

			if(TheShip.Active)
				TheShip.DrawShip(g);




		}
		private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			sKeyPress = sKeyPress + "," + e.KeyData.ToString();
		}

		private void spawnMoreAsteroids(Asteroid A)
		{
			if (A.Size < NoOfBabyAsteroids) 
			{
				Point pTest = A.MiddlePointOnFace(A.Direction + 45);
				AsteroidList.Add(new Asteroid(pTest.X,pTest.Y,A.Size+1,A.Direction + 45));
				pTest = A.MiddlePointOnFace(A.Direction + 135);
				AsteroidList.Add(new Asteroid(pTest.X,pTest.Y,A.Size+1,A.Direction + 135));
				pTest = A.MiddlePointOnFace(A.Direction + 225);
				AsteroidList.Add(new Asteroid(pTest.X,pTest.Y,A.Size+1,A.Direction + 225));
				pTest = A.MiddlePointOnFace(A.Direction + 315);
				AsteroidList.Add(new Asteroid(pTest.X,pTest.Y,A.Size+1,A.Direction + 315));
			}
		}
		private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (TheShip != null && !TheShip.Active)
				YouLoose();
			else if (AsteroidList != null && AsteroidList.Count == 0)	
							else
			{
				HandleKeyPresses();
				sKeyPress = "";
			}
			this.Refresh();
		}

		private void YouLoose()
		{
			foreach(Asteroid A in AsteroidList)
			{
				if (!A.Exploding)
				  A.Exploding = true;
			}
//			timer1.Stop();
//			MessageBox.Show("You Loose");
		}
		private void YouWin()
		{
			timer1.Stop();
			MessageBox.Show("You Win");
		}

		private void HandleKeyPresses()
		{
			string[] sKeys = sKeyPress.Split(new Char[]{','});
			for(int i=0;i<sKeys.Length;i++)
			{
				switch(sKeys[i])
				{
					case("Left"):
						if ((TheShip != null) && (TheShip.Active))
						  TheShip.RotateLeft();
						break;
					case("Right"):
						if ((TheShip != null) && (TheShip.Active))
						  TheShip.RotateRight();
						break;
					case("Up"):
						if ((TheShip != null) && (TheShip.Active))
							TheShip.Accelerate();
						break;
//					case("Down"):
//						if ((TheShip != null) && (TheShip.Active))
//							TheShip.Slow();
//						break;
					case("Space"):
						if ((TheShip != null) && (TheShip.Active))
						{
							TheShip.Fire();
													}
						break;
					case("Enter"):
						
						break;
					default:
						break;
				}
			}
		}

		private void Start_Click(object sender, System.EventArgs e)
		{
			InitializeAll();		
		}
		private void InitializeAll()
		{
			bStarted = true;
			Random r = new Random();
			int x;
			int y;
			sKeyPress = "";

			TheShip = new SpaceShip(this.ClientRectangle.Width/2,this.ClientRectangle.Height/2);
			AsteroidList = new ArrayList();

			for (int i = 0;i<iAsteroidNo;i++)
			{
				// position asteroids out of center space
				x = r.Next(this.ClientRectangle.Width/2);
				if (x > this.ClientRectangle.Width/4)
					x = x + (this.ClientRectangle.Width/2);
				
				y = r.Next(this.ClientRectangle.Height/2);
				if (y > this.ClientRectangle.Height/4)
					y = y + (this.ClientRectangle.Height/2);

				AsteroidList.Add(new Asteroid(x,y,1,r.Next(359)));
			}
			if (!timer1.Enabled)
				timer1.Start();


		}
		private int m_nCurrentPriority = 3;
		
		public void PlayASound()
		{
			if (m_strCurrentSoundFile.Length > 0)
			{
				PlaySound(Application.StartupPath + "\\" + m_strCurrentSoundFile,0,PlayFlags.Asynchronously);
			}
			m_strCurrentSoundFile = "";
			m_nCurrentPriority = 3;
			oThread.Abort();
		}

		
		public void PlaySoundInThread(string wavefile, int priority)
		{
			if (priority <= m_nCurrentPriority)
			{
				m_nCurrentPriority = priority;
				if (oThread != null)
					oThread.Abort();

				m_strCurrentSoundFile = wavefile;
				oThread = new Thread(new ThreadStart(PlayASound));
				oThread.Start();

			}
			
		}

		private void Exit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			iAsteroidNo = 1;
			menuItem2.Checked = true;
			menuItem3.Checked = true;
			menuItem4.Checked = true;
			menuItem5.Checked = true;
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			iAsteroidNo = 2;
			menuItem3.Checked = true;
			menuItem2.Checked = true;
			menuItem4.Checked = true;
			menuItem5.Checked = true;
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			iAsteroidNo = 3;
			menuItem4.Checked = true;
			menuItem3.Checked = true;
			menuItem2.Checked = true;
			menuItem5.Checked = true;
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			iAsteroidNo = 4;
			menuItem5.Checked = true;
			menuItem3.Checked = true;
			menuItem4.Checked = true;
			menuItem2.Checked = true;
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			iTextHolder = this.ClientRectangle.Height/3;
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			timer1.Interval = 100;
			menuItem7.Checked = true;
			menuItem8.Checked = true;
			menuItem9.Checked = true;
		}

		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			timer1.Interval = 75;
			menuItem7.Checked = true;
			menuItem8.Checked = true;
			menuItem9.Checked = true;
		
		}

		private void menuItem9_Click(object sender, System.EventArgs e)
		{
			timer1.Interval = 50;
			menuItem7.Checked = true;
			menuItem8.Checked = true;
			menuItem9.Checked = true;		
		}


	}
}
