using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LoveCommander.Bang.Actions;
using LoveCommander.Screens;

namespace LoveCommander.Entity {
	public class Pillow : BaseEntity {

		public float Rotation = 0f;
		public Vector2 Velocity;		

		public Pillow(int posX, int posY, Vector2 velocity) : base() {
			Position.X = posX;
			Position.Y = posY;

			Velocity = velocity;			
			Size = new Vector2(349 / 4f, 297 / 4f);

			CollisionType = "pillow";
			ZDepth = -1;
		}

		public override void Init() {
			
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
			Rotation += 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

			BaseEntity ent = GetFirstCollidingEntity("ufo");
			if (ent != null) {				
				Assets.Sounds["hit"].Play();
				Screen.RemoveEntity(this);
				Screen.AddEntity(new Clouds((int)Position.X + 20, (int)Position.Y));
				(ent as Ufo).Hit();
			}

			ent = GetFirstCollidingEntity("simple_ufo");
			if (ent != null) {
				Assets.Sounds["hit"].Play();
				Screen.RemoveEntity(this);
				Screen.AddEntity(new Clouds((int)Position.X + 20, (int)Position.Y));
				(ent as SimpleUfo).Hit();
			}

			ent = GetFirstCollidingEntity("projectile");
			if (ent != null) {
				Assets.Sounds["hit"].Play();
				Screen.RemoveEntity(this);
				Screen.RemoveEntity(ent);
				Screen.AddEntity(new Clouds((int)Position.X + 20, (int)Position.Y));

				var num = new Number((int)Position.X, (int)Position.Y, 10, Color.LightGreen);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score += 10; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);				
			}

			if (Position.Y < -100 || Position.Y > MainGame.Height || Position.X > MainGame.Width) {
				Screen.RemoveEntity(this);
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(Assets.Images["pillow"], new Rectangle((int)Position.X + ((int)(Size.X / 2f)), (int)Position.Y + ((int)(Size.Y / 2f)), (int)Size.X, (int)Size.Y), null, Color.White, Rotation, new Vector2(Assets.Images["pillow"].Width / 2f, Assets.Images["pillow"].Height / 2f), SpriteEffects.None, 0);
		}

		public override void Delete() {
			
		}
	}
}
