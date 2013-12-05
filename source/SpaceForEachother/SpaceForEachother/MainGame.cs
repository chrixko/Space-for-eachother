using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LoveCommander.Screens;
using LoveCommander.Physics;
using LoveCommander.Bang.Actions;
using System.IO;
using System;
using LoveCommander.Pillo;

namespace LoveCommander {

	public class MainGame : Microsoft.Xna.Framework.Game {

		Texture2D lastPicture;

		Vector2 LastPercentage = Vector2.Zero;
		Vector2 Percentage = Vector2.Zero;

		public static GraphicsDeviceManager graphics;
		ExtendedSpriteBatch spriteBatch;
		public KinectContext kinectContext;
		public PilloReader PilloReader;

		DebugComponent debugComponent;

		private readonly Rectangle viewPortRectangle;

		public static int Width = 1280;
		public static int Height = 720;

		public static float KinectScaleX = 640f / Width;
		public static float KinectScaleY = 480f / Height;

		public ScreenManager ScreenManager;

		public bool DebugView = false;

		Matrix spriteScale;

		public MainGame() {
			graphics = new GraphicsDeviceManager(this); 
			Content.RootDirectory = "Content";
					   
			graphics.PreferredBackBufferWidth = Width;
			graphics.PreferredBackBufferHeight = Height;
			graphics.SynchronizeWithVerticalRetrace = false;
			IsFixedTimeStep = true;
			ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);
			this.viewPortRectangle = new Rectangle(0, 0, Width, Height);

			graphics.IsFullScreen = true;
			graphics.SynchronizeWithVerticalRetrace = true;
			IsFixedTimeStep = true;
		}

		protected override void Initialize() {
			Assets.LoadContent(Content);
			kinectContext = new KinectContext(graphics.GraphicsDevice);
			kinectContext.Initialize();

			ScreenManager = new ScreenManager(this);
			ScreenManager.AddScreen(new MenuScreen((kinectContext)));
						
			debugComponent = new DebugComponent(this);

			PilloReader = new PilloReader();
			PilloReader.ConnectToPillo();			

			base.Initialize();
		}

		public Texture2D GetScreenshot() {
			try {
				var tex = new Texture2D(GraphicsDevice, 640, 480);
				Color[] con = new Color[640 * 480];
				if (lastPicture != null) {
					lastPicture.GetData<Color>(con);
				}
				tex.SetData<Color>(con);

				return tex;
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			return null;
		}

		protected override void LoadContent() {           
			spriteBatch = new ExtendedSpriteBatch(GraphicsDevice);

			float scaleX = graphics.GraphicsDevice.Viewport.Width / (float)Width;
			float scaleY = graphics.GraphicsDevice.Viewport.Height / (float)Height;

			spriteScale = Matrix.CreateScale(scaleX, scaleY, 1);
		}

		protected override void UnloadContent() {
			kinectContext.StopSensor();
			PilloReader.DisconnectFromPillo();
		}

		protected override void Update(GameTime gameTime) {
			LastPercentage = Percentage;
			Percentage = PilloReader.Percentage;			

			PilloState.LeftJustPressed = LastPercentage.X == 0 && Percentage.X != 0;
			PilloState.LeftHolding = LastPercentage.X != 0 && Percentage.X != 0;
			PilloState.LeftReleased = LastPercentage.X != 0 && Percentage.X == 0;

			PilloState.RightJustPressed = LastPercentage.Y == 0 && Percentage.Y != 0;
			PilloState.RightHolding = LastPercentage.Y != 0 && Percentage.Y != 0;
			PilloState.RightReleased = LastPercentage.Y != 0 && Percentage.Y == 0;

			debugComponent.Update(gameTime);
			
			kinectContext.Update();					

			ScreenManager.Update(gameTime);

			if (Keyboard.GetState().IsKeyDown(Keys.F12)) {
				DebugView = !DebugView;
			}

			base.Update(gameTime);
		}

		Random random = new Random();

		private Vector3 Shake() {
			return new Vector3(				
				(float)((random.NextDouble() * 2) - 1),
				(float)((random.NextDouble() * 2) - 1), 0);
		}  

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);
			
			var sc = spriteScale;
			if (ScreenManager.TopScreen.ShakeAmount > 0) {
				sc = spriteScale * Matrix.CreateTranslation(Shake() * ScreenManager.TopScreen.ShakeAmount);
			}

			if (kinectContext.CurrentBitmap != null) {
				lastPicture = kinectContext.CurrentBitmap;
			}

			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, sc);			

			ScreenManager.Draw(spriteBatch);			

			if (DebugView) {
				debugComponent.Draw(spriteBatch, gameTime);
			}

			if (kinectContext.Sensor == null) {
				spriteBatch.DrawString(Assets.Fonts["debug"], "NO KINECT SENSOR FOUND! PLEASE CONNECT A WINDOWS KINECT AND RESTART THE GAME!", new Vector2(50, 50), Color.LimeGreen);
			}
			
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
