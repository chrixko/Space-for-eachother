using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace LoveCommander.Entity {
	public class SimpleGraphic : BaseEntity {
		public Texture2D Graphic { get; set; }

		public SimpleGraphic(Texture2D graphic, int posX, int posY, int width, int height) {
			Graphic = graphic;
			Position = new Vector2(posX, posY);
			
			collidable = false;
			Size = new Vector2(width, height);
		}

		public override void Init() {
			
		}
	   
		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
			spriteBatch.Draw(Graphic, BoundingBox, Color.White);
		}

		public override void Delete() {
		}
	}
}
