using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LoveCommander.Entity {
	public class Clouds : BaseEntity {
		Animation animation;

		public Clouds(int posX, int posY) : base() {
			animation = new Animation("cloud_explosion", Assets.Images["clouds"], 600);
			animation.Play(false);

			Position.X = posX;
			Position.Y = posY;

			Size = new Vector2(600f / 4f, 600f / 4f);
		}

		public override void Init() {
			
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
			base.Update(gameTime);

			if (animation.Finished) {
				Screen.RemoveEntity(this);
			}

			animation.Update(gameTime);
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
			spriteBatch.Draw(Assets.Images["clouds"], BoundingBox, animation.FrameRectangle, Color.White);
		}

		public override void Delete() {
			
		}
	}
}
