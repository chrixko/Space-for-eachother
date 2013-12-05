using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoveCommander.Entity {
	public class Number : BaseEntity {

		public int Value { get; set; }
		public Color Color { get; set; }

		public Number(int posX, int posY, int value, Color color) {
			Position = new Vector2(posX, posY);
			Value = value;
			Color = color;
		}

		public override void Init() {
			
		}

		public static void DrawValue(SpriteBatch spriteBatch, Vector2 position, int value, Color color, string format) {
			int i = 0;
			foreach (var c in value.ToString(format)) {
				spriteBatch.Draw(Assets.Images["number_" + c.ToString()], new Rectangle((int)position.X + (int)(i * 80 * 0.5f), (int)position.Y, (int)(75 * 0.5f), (int)(100 * 0.5f)), color);
				i++;
			}
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
			DrawValue(spriteBatch, Position, Value, Color, "");
		}

		public override void Delete() {
			
		}
	}
}
