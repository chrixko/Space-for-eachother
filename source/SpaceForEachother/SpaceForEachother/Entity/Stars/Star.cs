using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LoveCommander.Entity.Stars {
	public class Star : SimpleGraphic {
		public Star(Texture2D graphic, int posX, int posY, int width, int height)
			: base(graphic, posX, posY, width, height) {
				ZDepth = -100;
		}

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime) {
			base.Update(gameTime);

			Position.X -= 150 * (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (Position.X <= -Size.X) {
				Screen.RemoveEntity(this);
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(Graphic, BoundingBox, Color.Yellow);
		}
	}
}
