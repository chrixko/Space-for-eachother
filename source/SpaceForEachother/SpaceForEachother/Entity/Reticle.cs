using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LoveCommander.Entity {
	public class Reticle : BaseEntity {
		public PlayerHand AttachedHand { get; set; }

		public BaseEntity SnappedObj { get; set; }
		public Reticle(PlayerHand attachedHand) {
			AttachedHand = attachedHand;
			ZDepth = 1000;

			Size = new Vector2(50f, 50f);

			collidable = true;
			CollisionType = "reticle";
		}

		public override void Init() {
			
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (AttachedHand.GrabbingObject == null) {
				SnappedObj = AttachedHand.GetFirstCollidingEntity("asteroid");
				if (SnappedObj == null) {
					SnappedObj = AttachedHand.GetFirstCollidingEntity("ufo");
				}
				if (SnappedObj == null) {
					SnappedObj = AttachedHand.GetFirstCollidingEntity("simple_ufo");
				}
			} else {
				SnappedObj = AttachedHand.GrabbingObject;
			}

			if (SnappedObj != null) {
				Position = SnappedObj.Position + new Vector2(SnappedObj.Size.X / 2f, SnappedObj.Size.Y / 2f);
			} else {
				Position = AttachedHand.Position;
			}		
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) {
			var graphic = AttachedHand.Hand == Microsoft.Kinect.JointType.HandLeft ? Assets.Images["reticle_red"] : Assets.Images["reticle_blue"];
			spriteBatch.Draw(graphic, new Rectangle((int)Position.X, (int)Position.Y, 200 / 4, 200 / 4), null, Color.White, 0, new Vector2(graphic.Width / 2, graphic.Height / 2), SpriteEffects.None, 0);
		}

		public override void Delete() {
			
		}
	}
}
