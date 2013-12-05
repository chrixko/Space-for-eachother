using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LoveCommander.Screens;
using LoveCommander.Pillo;
using LoveCommander.Bang.Actions;

namespace LoveCommander.Entity {
	public class Mothership : BaseEntity {

		public Mothership(int posX, int posY)
			: base() {
			Position.X = posX;
			Position.Y = posY;

			Size = new Vector2(493 / 3, 272 / 3);

			CollisionType = "ship";
		}

		public override void Init() {
			Actions.AddAction(new TweenPositionTo(this, new Vector2(50, Position.Y), 1f, Tweening.Linear.EaseIn), true);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (Actions.IsComplete()) {
				var rP = ((GameScreen)Screen).RightReticle.Position - Position;
				rP.X = 0;
				if (rP != Vector2.Zero) {
					rP.Normalize();

					Position += rP * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
				}

				if (PilloState.LeftJustPressed && ((GameScreen)Screen).LeftReticle != null && ((GameScreen)Screen).LeftReticle.SnappedObj != null && ((GameScreen)Screen).LeftReticle.SnappedObj.CollisionType.EndsWith("ufo")) {
					Assets.Sounds["shoot"].Play();

					var shootPosition = new Vector2(Position.X + (Size.X / 2f), Position.Y);

					var velocity = (((GameScreen)Screen).CurrentPlayer.LeftHand.Position - shootPosition);
					velocity.Normalize();
					velocity *= 600;
					Screen.AddEntity(new Pillow((int)shootPosition.X, (int)shootPosition.Y, velocity));

					Screen.Coroutines.Start(Screen.ShakeForXSeconds(0.5f, 5f));
				}


				if (PilloState.RightJustPressed && ((GameScreen)Screen).RightReticle != null && ((GameScreen)Screen).RightReticle.SnappedObj != null && ((GameScreen)Screen).RightReticle.SnappedObj.CollisionType.EndsWith("ufo")) {
					Assets.Sounds["shoot"].Play();

					var shootPosition = new Vector2(Position.X + (Size.X / 2f), Position.Y + (Size.Y / 2f));

					var velocity = (((GameScreen)Screen).CurrentPlayer.RightHand.Position - new Vector2(349 / 8f, 297 / 8f) - shootPosition);
					velocity.Normalize();
					velocity *= 600;
					Screen.AddEntity(new Pillow((int)shootPosition.X, (int)shootPosition.Y, velocity));

					Screen.Coroutines.Start(Screen.ShakeForXSeconds(0.5f, 5f));
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(Assets.Images["spaceship"], BoundingBox, Color.White);
		}

		public override void Delete() {
			
		}
	}
}