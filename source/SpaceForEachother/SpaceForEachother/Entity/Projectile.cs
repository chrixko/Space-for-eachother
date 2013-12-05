using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LoveCommander.Screens;
using LoveCommander.Bang.Actions;

namespace LoveCommander.Entity {
	public class Projectile : BaseEntity {

		public float Rotation = 0f;
		public Vector2 Velocity;

		public Projectile(int posX, int posY, Vector2 velocity)
			: base() {
			Position.X = posX;
			Position.Y = posY;

			Velocity = velocity;			
			Size = new Vector2(349 / 5f, 297 / 5f);

			CollisionType = "projectile";
			ZDepth = -1;
		}

		public override void Init() {
			
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
			Rotation -= 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

			BaseEntity shield = GetFirstCollidingEntity("shield");
			if (shield != null) {
				Assets.Sounds["hit"].Play();
				Screen.RemoveEntity(this);
				Screen.AddEntity(new Clouds((int)Position.X + 20, (int)Position.Y));	
			}

			BaseEntity ent = GetFirstCollidingEntity("ship");
			if (ent != null) {				
				Assets.Sounds["hit"].Play();
				Screen.RemoveEntity(this);

				var num = new Number((int)Position.X, (int)Position.Y, 50, Color.Red);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score -= 50; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);				
			}

		}

		public override void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(Assets.Images["pillow"], new Rectangle((int)Position.X + ((int)(Size.X / 2f)), (int)Position.Y + ((int)(Size.Y / 2f)), (int)Size.X, (int)Size.Y), null, Color.LightBlue, Rotation, new Vector2(Assets.Images["pillow"].Width / 2f, Assets.Images["pillow"].Height / 2f), SpriteEffects.None, 0);
		}

		public override void Delete() {
			
		}
	}
}
