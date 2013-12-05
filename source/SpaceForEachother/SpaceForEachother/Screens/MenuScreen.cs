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

namespace LoveCommander.Screens {
	public class MenuScreen : BaseScreen {

		public Player CurrentPlayer { get; set; }

		float timer = 0;
		public MenuScreen(KinectContext context)
			: base(context) {
		}

		public override void Init() {
			base.Init();

			CurrentPlayer = new Player(Context, SkeletonPlayerAssignment.RightSkeleton);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (PilloState.LeftHolding && PilloState.RightHolding && CurrentPlayer.IsReady) {
				timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (timer >= 1f) {
					Manager.SwitchScreen(new GameScreen(Context));
				}
			} else {
				timer = 0;
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(Assets.Images["startscreen"], new Rectangle(0, 0, MainGame.Width, MainGame.Height), Color.White);

			if (PilloState.LeftHolding && PilloState.RightHolding) {
				spriteBatch.Draw(Assets.Images["check"], new Rectangle(1100, 500, Assets.Images["check"].Width, Assets.Images["check"].Height), Color.White);
			}

			if (CurrentPlayer.IsReady) {
				spriteBatch.Draw(Assets.Images["check"], new Rectangle(80, 500, Assets.Images["check"].Width, Assets.Images["check"].Height), Color.White);
			}
		}
	}
}
