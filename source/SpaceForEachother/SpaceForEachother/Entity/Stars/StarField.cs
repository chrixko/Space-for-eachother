using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoveCommander.Entity.Stars {
	public class StarField : BaseEntity {

		private Random rand = new Random();

		private List<String> smallStars = new List<string>();
		private List<String> stars = new List<string>();
		private List<String> specialStars = new List<string>();

		public override void Init() {
			smallStars.Add("smallstar1");
			smallStars.Add("smallstar2");
			smallStars.Add("smallstar3");
			smallStars.Add("smallstar4");
			smallStars.Add("smallstar5");

			stars.Add("star1");
			stars.Add("star2");
			stars.Add("star3");

			specialStars.Add("specialstar1");
			specialStars.Add("specialstar2");
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (rand.Next(0, 50) == 1) {			
				var posY = rand.Next(0, MainGame.Height - 125);				
				Screen.AddEntity(new Star(Assets.Images[smallStars[rand.Next(0, smallStars.Count)]], MainGame.Width, posY, 25, 25));
			}

			if (rand.Next(0, 100) == 1) {
				var posY = rand.Next(0, MainGame.Height - 200);
				var image = Assets.Images[stars[rand.Next(0, stars.Count)]];
				float mod = rand.Next(3, 7);
				mod /= 10;

				Screen.AddEntity(new Star(image, MainGame.Width, posY, (int)(image.Width * mod), (int)(image.Height * mod)));
			}

			if (rand.Next(0, 150) == 1) {
				var posY = rand.Next(0, MainGame.Height - 250);
				var image = Assets.Images[specialStars[rand.Next(0, specialStars.Count)]];
				float mod = rand.Next(3, 7);
				mod /= 10;

				Screen.AddEntity(new Star(image, MainGame.Width, posY, (int)(image.Width * mod), (int)(image.Height * mod)));
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {				
		}

		public override void Delete() {
			
		}
	}
}
