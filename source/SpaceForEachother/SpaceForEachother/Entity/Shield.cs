using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LoveCommander.Pillo;
using LoveCommander.Screens;

namespace LoveCommander.Entity {
	public class Shield : BaseEntity {

		PlayerHand AttachedHand { get; set; }

		public Shield(PlayerHand attachedHand) : base() {
			AttachedHand = attachedHand;
			Position.X = 250;
			Position.Y = AttachedHand.Position.Y;

			Size = new Vector2(251f / 4f, 609f / 4f);

			CollisionType = "shield";
		}

		public override void Init() {
			
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if ((AttachedHand.Hand == Microsoft.Kinect.JointType.HandLeft && !PilloState.LeftHolding) || (AttachedHand.Hand == Microsoft.Kinect.JointType.HandRight && !PilloState.RightHolding)) {
				Screen.RemoveEntity(this);
				if (AttachedHand.Hand == Microsoft.Kinect.JointType.HandLeft) {
					((GameScreen)Screen).LeftShield = null;
				}

				if (AttachedHand.Hand == Microsoft.Kinect.JointType.HandRight) {
					((GameScreen)Screen).RightShield = null;
				}
			}
			Position.Y = AttachedHand.Position.Y;
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
			spriteBatch.Draw(Assets.Images["shield"], BoundingBox, Color.White);
		}

		public override void Delete() {
			
		}
	}
}
