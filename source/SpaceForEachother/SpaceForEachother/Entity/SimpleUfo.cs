using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LoveCommander.Screens;
using LoveCommander.Bang.Actions;

namespace LoveCommander.Entity {
	public class SimpleUfo : BaseEntity {

		public int Lifes = 2;
		bool selected = false;

		public SimpleUfo(int posX, int posY)
			: base() {
			Position.X = posX;
			Position.Y = posY;

			Size = new Vector2(506f / 5f, 265f / 5f);
			CollisionType = "simple_ufo";
		}

		public override void Init() {
			
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (GetFirstCollidingEntity("ship") != null) {
				Assets.Sounds["hit"].Play();
				Screen.RemoveEntity(this);
				Screen.AddEntity(new Clouds((int)Position.X + 20, (int)Position.Y));

				var num = new Number((int)Position.X, (int)Position.Y, 50, Color.Red);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score -= 50; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);
			}

			if (GetFirstCollidingEntity("reticle") != null) {
				selected = true;
			} else {
				selected = false;
			}

			Position.X -= 85f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			if (Position.X < -Size.X) {
				Screen.RemoveEntity(this);
				var num = new Number((int)Position.X, (int)Position.Y, 30, Color.Red);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score -= 30; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);
			}
		}

		public void Hit() {
			Lifes--;
			if (Lifes <= 0) {
				Assets.Sounds["pling"].Play();
				Screen.RemoveEntity(this);
				var num = new Number((int)Position.X, (int)Position.Y, 30, Color.Green);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score += 30; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			if (selected) {
				spriteBatch.Draw(Assets.Images["shadow_round"], new Rectangle((int)Position.X - 10, (int)Position.Y - 10, (int)Size.X + 20, (int)Size.Y + 20), Color.Green);
			}
			spriteBatch.Draw(Assets.Images["alien_round"], BoundingBox, Color.White);			
		}

		public override void Delete() {
			
		}
	}
}
