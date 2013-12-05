using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveCommander.Entity;
using LoveCommander.Entity.Stars;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LoveCommander.Pillo;
using LoveCommander.Bang.Actions;

namespace LoveCommander.Screens {
	public class GameScreen : BaseScreen {
		public Player CurrentPlayer { get; set; }
		public Mothership CurrentShip { get; set; }
		public Reticle RightReticle { get; set; }
		public Reticle LeftReticle { get; set; }

		public Shield LeftShield { get; set; }
		public Shield RightShield { get; set; }

		private Vector2 bgPosition = Vector2.Zero;
		private Vector2 floorPosition = new Vector2(0, MainGame.Height - 100);

		private float gameTimer = 0;
		bool capturePictures = true;

		private List<Texture2D> screenShots = new List<Texture2D>();

		public GameScreen(KinectContext context) : base(context) {
		}

		public override void Init() {
			base.Init();

			Actions.AddAction(new FadeInSong(Assets.MainSong, true, 0.6f), false);

			CurrentPlayer = new Player(Context, SkeletonPlayerAssignment.RightSkeleton);
			AddEntity(CurrentPlayer);
			CurrentPlayer.DrawHands = true;
			AddEntity(new Shield(CurrentPlayer.RightHand));

			RightReticle = new Reticle(CurrentPlayer.RightHand);
			AddEntity(RightReticle);

			LeftReticle = new Reticle(CurrentPlayer.LeftHand);
			AddEntity(LeftReticle);

			CurrentShip = new Mothership(-100, (MainGame.Height / 2) - 50);
			AddEntity(CurrentShip);


			Coroutines.Start(AddEnemies());
			Coroutines.Start(AddTutorial());
		}

		IEnumerator AddTutorial() {
			yield return BaseScreen.Pause(1f);
			
			var sg = new SimpleGraphic(Assets.Images["tutorial_1"], MainGame.Width, MainGame.Height - 90, 800, 80);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(400f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			sg.Actions.AddAction(new WaitForSeconds(7f), true);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(-800f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			AddEntity(sg);

			yield return BaseScreen.Pause(30f);
			
			sg = new SimpleGraphic(Assets.Images["tutorial_2"], MainGame.Width, MainGame.Height - 90, 800, 80);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(400f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			sg.Actions.AddAction(new WaitForSeconds(3f), true);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(-800f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			AddEntity(sg);
			
			yield return BaseScreen.Pause(5f);
			
			sg = new SimpleGraphic(Assets.Images["tutorial_3"], MainGame.Width, MainGame.Height - 90, 800, 80);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(400f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			sg.Actions.AddAction(new WaitForSeconds(5f), true);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(-800f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			AddEntity(sg);

			yield return BaseScreen.Pause(28f);

			sg = new SimpleGraphic(Assets.Images["tutorial_4"], MainGame.Width, MainGame.Height - 90, 800, 80);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(400f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			sg.Actions.AddAction(new WaitForSeconds(3f), true);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(-800f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			AddEntity(sg);

			yield return BaseScreen.Pause(5f);

			sg = new SimpleGraphic(Assets.Images["tutorial_5"], MainGame.Width, MainGame.Height - 90, 800, 80);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(400f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			sg.Actions.AddAction(new WaitForSeconds(8f), true);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(-800f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			AddEntity(sg);

			yield return BaseScreen.Pause(10f);

			sg = new SimpleGraphic(Assets.Images["tutorial_6"], MainGame.Width, MainGame.Height - 90, 800, 80);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(400f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			sg.Actions.AddAction(new WaitForSeconds(5f), true);
			sg.Actions.AddAction(new TweenPositionTo(sg, new Vector2(-800f, MainGame.Height - 90f), 1f, Tweening.Back.EaseInOut), true);
			AddEntity(sg);
		}

		IEnumerator AddEnemies() {
			yield return BaseScreen.Pause(3f);

			int GameMiddle = (MainGame.Height - 185) / 2;
			//addAlienDiagonal();

			//yield return BaseScreen.Pause(10f);

			//addAsteroidField();

			//yield return BaseScreen.Pause(10f);

			//addAlienCross();

			//First enemy non shooting
			AddEntity(new SimpleUfo(MainGame.Width + (100), GameMiddle));


			yield return BaseScreen.Pause(15f);

			//3 non shooting SIO
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle + 100));
			AddEntity(new SimpleUfo(MainGame.Width + (100), GameMiddle));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle - 100));

			yield return BaseScreen.Pause(15f);

			// shooting SIOs           
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle + 100));
			AddEntity(new Ufo(MainGame.Width + (100), GameMiddle));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle - 100));

			yield return BaseScreen.Pause(15f);

			// shooting SIOs           
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle + 100));
			AddEntity(new Ufo(MainGame.Width + (100), GameMiddle));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle - 100));
			yield return BaseScreen.Pause(5f);
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle + 100));
			AddEntity(new Ufo(MainGame.Width + (100), GameMiddle));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle - 100));

			yield return BaseScreen.Pause(15f);

			// first SIA
			AddEntity(new Asteroid(MainGame.Width + (100), GameMiddle));

			yield return BaseScreen.Pause(13f);

			//playing around
			AddEntity(new SimpleUfo(MainGame.Width + (100), GameMiddle + 225));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle + 150));
			AddEntity(new SimpleUfo(MainGame.Width + (300), GameMiddle + 75));
			AddEntity(new Ufo(MainGame.Width + (300), GameMiddle));
			AddEntity(new Ufo(MainGame.Width + (400), GameMiddle));
			AddEntity(new SimpleUfo(MainGame.Width + (300), GameMiddle - 75));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle - 150));
			AddEntity(new SimpleUfo(MainGame.Width + (100), GameMiddle - 225));

			yield return BaseScreen.Pause(15f);

			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle + 100));
			AddEntity(new Ufo(MainGame.Width + (100), GameMiddle));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle - 100));

			AddEntity(new Asteroid(MainGame.Width + (100), GameMiddle));

			yield return BaseScreen.Pause(20f);


			//#######################################LEVEL 2##########################################			

			AddEntity(new SimpleUfo(MainGame.Width + (400), GameMiddle + 225));
			AddEntity(new SimpleUfo(MainGame.Width + (300), GameMiddle + 150));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle + 75));
			AddEntity(new Ufo(MainGame.Width + (100), GameMiddle));
			AddEntity(new Ufo(MainGame.Width + (100), GameMiddle));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle - 75));
			AddEntity(new SimpleUfo(MainGame.Width + (300), GameMiddle - 150));
			AddEntity(new SimpleUfo(MainGame.Width + (400), GameMiddle - 225));

			yield return BaseScreen.Pause(1f);

			AddEntity(new Ufo(MainGame.Width + (400), GameMiddle + 225));
			AddEntity(new SimpleUfo(MainGame.Width + (300), GameMiddle + 150));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle + 75));
			AddEntity(new SimpleUfo(MainGame.Width + (100), GameMiddle));
			AddEntity(new SimpleUfo(MainGame.Width + (100), GameMiddle));
			AddEntity(new SimpleUfo(MainGame.Width + (200), GameMiddle - 75));
			AddEntity(new SimpleUfo(MainGame.Width + (300), GameMiddle - 150));
			AddEntity(new Ufo(MainGame.Width + (400), GameMiddle - 225));

			//wating
			yield return BaseScreen.Pause(15f);

			//lvl2 2. wave
			AddEntity(new Asteroid(MainGame.Width + (250), GameMiddle +220));
			AddEntity(new Asteroid(MainGame.Width + (200), GameMiddle + 150));
			AddEntity(new Asteroid(MainGame.Width + (100), GameMiddle));
			AddEntity(new Asteroid(MainGame.Width + (250), GameMiddle - 150));
			AddEntity(new Asteroid(MainGame.Width + (300), GameMiddle - 250));


			//wating
			yield return BaseScreen.Pause(16f);

			// lvl2 3. wave

			AddEntity(new Asteroid(MainGame.Width + (300), GameMiddle + 50));
			AddEntity(new Asteroid(MainGame.Width + (300), GameMiddle - 50));

			AddEntity(new Ufo(MainGame.Width + (150), GameMiddle));
			AddEntity(new Ufo(MainGame.Width + (250), GameMiddle + 100));
			AddEntity(new Ufo(MainGame.Width + (250), GameMiddle - 100));
			AddEntity(new Ufo(MainGame.Width + (150), GameMiddle));

			yield return BaseScreen.Pause(2f);


			AddEntity(new SimpleUfo(MainGame.Width + (250), GameMiddle + 225));
			AddEntity(new SimpleUfo(MainGame.Width + (325), GameMiddle + 225));
			AddEntity(new SimpleUfo(MainGame.Width + (400), GameMiddle + 225));

			AddEntity(new SimpleUfo(MainGame.Width + (250), GameMiddle - 225));
			AddEntity(new SimpleUfo(MainGame.Width + (325), GameMiddle - 225));
			AddEntity(new SimpleUfo(MainGame.Width + (400), GameMiddle - 225));

			//waiting
			yield return BaseScreen.Pause(20f);

			//lvl2 3. wave
			 AddEntity(new Ufo(MainGame.Width + (100), GameMiddle  - 150));
			 AddEntity(new SimpleUfo(MainGame.Width + (100), GameMiddle + 150));
			 AddEntity(new SimpleUfo(MainGame.Width + (300), GameMiddle));
			 AddEntity(new SimpleUfo(MainGame.Width + (550), GameMiddle - 250));
			 AddEntity(new Ufo(MainGame.Width + (600), GameMiddle + 155));
			 AddEntity(new SimpleUfo(MainGame.Width + (700), GameMiddle - 250));
			 AddEntity(new Ufo(MainGame.Width + (750), GameMiddle + 250));
			AddEntity(new SimpleUfo(MainGame.Width + (750), GameMiddle + 50 ));

			//waiting
			yield return BaseScreen.Pause(20f);

			//lvl3 4. wave
			AddEntity(new Ufo(MainGame.Width + (100), GameMiddle - 200));
			AddEntity(new SimpleUfo(MainGame.Width + (100), GameMiddle));
			AddEntity(new Ufo(MainGame.Width + (100), GameMiddle + 200));

			AddEntity(new SimpleUfo(MainGame.Width + (300), GameMiddle - 100));
			AddEntity(new SimpleUfo(MainGame.Width + (300), GameMiddle + 100));

			AddEntity(new Asteroid(MainGame.Width + (800), GameMiddle - 200));
			AddEntity(new Asteroid(MainGame.Width + (800), GameMiddle + 200));
			AddEntity(new SimpleUfo(MainGame.Width + (900), GameMiddle));

			AddEntity(new SimpleUfo(MainGame.Width + (1100), GameMiddle - 150));
			AddEntity(new SimpleUfo(MainGame.Width + (1100), GameMiddle + 150));
			AddEntity(new Ufo(MainGame.Width + (1300), GameMiddle + 275));
			AddEntity(new Ufo(MainGame.Width + (1300), GameMiddle - 275));

			yield return BaseScreen.Pause(25f);

			capturePictures = false;
			Coroutines.Start(addKinectPictures());
		}

		void addAlienDiagonal() {
			for (int i = 0; i < 6; i++) {
				AddEntity(new Ufo(MainGame.Width + (i * 100), 50 + (i * 70)));
			}
		}

		void addAlienCross() {
			for (int i = 0; i < 6; i++) {
				AddEntity(new Ufo(MainGame.Width + (i * 100), 50 + (i * 70)));
				AddEntity(new Ufo(MainGame.Width + (i * 100), 400 - (i * 70)));
			}
		}

		void addAsteroidField() {
			var rand = new Random();
			for (int i = 0; i < 5; i++) {
				var posX = MainGame.Width + rand.Next(0, MainGame.Width);
				var posY = rand.Next(100, MainGame.Height - 200);

				AddEntity(new Asteroid(posX, posY));
			}
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (capturePictures) {
				gameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (gameTimer >= 10f) {
					var shot = Manager.Game.GetScreenshot();
					if (shot != null) {
						screenShots.Add(Manager.Game.GetScreenshot());
					}
					
					gameTimer = 0;
				}
			}			

			if (LeftReticle != null && LeftShield == null && PilloState.LeftHolding && LeftReticle.SnappedObj == null) {
				LeftShield = new Shield(CurrentPlayer.LeftHand);
				AddEntity(LeftShield);
			}

			if (RightReticle != null && RightShield == null && PilloState.RightHolding && RightReticle.SnappedObj == null) {
				RightShield = new Shield(CurrentPlayer.RightHand);
				AddEntity(RightShield);
			}

			bgPosition.X -= 300f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (bgPosition.X < -MainGame.Width) {
				bgPosition.X = 0;
			}

			floorPosition.X -= 300f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (floorPosition.X < -68) {
				floorPosition.X = 0;
			}
		}

		IEnumerator addKinectPictures() {
			foreach (var tex in screenShots) {
				var sh = new SimpleGraphic(tex, MainGame.Width, (int)((MainGame.Height / 2f) - 480f / 2f), 640, 480);
				sh.Actions.AddAction(new TweenPositionTo(sh, new Vector2(300, (MainGame.Height / 2f) - 480f / 2f), 1f, Tweening.Back.EaseInOut), true);
				sh.Actions.AddAction(new WaitForSeconds(2f), true);
				sh.Actions.AddAction(new TweenPositionTo(sh, new Vector2(-640f, (MainGame.Height / 2f) - 480f / 2f), 1f, Tweening.Back.EaseInOut), true);
				AddEntity(sh);
				yield return BaseScreen.Pause(3f);
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {			
			spriteBatch.Draw(Assets.Images["bg"], new Rectangle((int)bgPosition.X, (int)bgPosition.Y, (int)Assets.Images["bg"].Width, (int)Assets.Images["bg"].Height), Color.White);
			spriteBatch.Draw(Assets.Images["bg"], new Rectangle((int)bgPosition.X + MainGame.Width, (int)bgPosition.Y, (int)Assets.Images["bg"].Width, (int)Assets.Images["bg"].Height), Color.White);

			for (int i = 0; i < 20; i++) {
				spriteBatch.Draw(Assets.Images["floor"], new Rectangle((int)floorPosition.X + (68 * i), (int)floorPosition.Y, (int)Assets.Images["floor"].Width, (int)Assets.Images["floor"].Height), Color.White);
			}

			spriteBatch.Draw(Assets.Images["score"], new Rectangle(30, MainGame.Height - 75, (int)(Assets.Images["score"].Width / 2f), (int)(Assets.Images["score"].Height / 2f)), Color.White);
			Number.DrawValue(spriteBatch, new Vector2(210, MainGame.Height - 75), CurrentPlayer.Score, Color.White, "0000");

			base.Draw(spriteBatch);			
		}
	}
}
