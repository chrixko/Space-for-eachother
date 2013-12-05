using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Linq;
using LoveCommander.Entity;

namespace LoveCommander {

	public class PlayerHand : BaseEntity {

		public Player Player { get; private set; }
		public JointType Hand { get; private set; }        

		public bool IsGrabbing { get; set; }
		public Asteroid GrabbingObject { get; set; }

		public KinectContext Context {
			get {
				return Player.Context;
			}
		}

		public PlayerHand(Player player, JointType hand) {            
			this.Offset = new Vector2(-30f, -30f);
			this.Size = new Vector2(50f, 50f);

			Player = player;
			Hand = hand;

			CollisionType = hand == JointType.HandLeft ? "hand_left" : "hand_right";

			ZDepth = 1000;
		}

		public override void Init() {
		}

		private InteractionHandPointer getHandPointer() {   
			UserInfo userInfo;

			if (Context.UserInfos.TryGetValue(Player.Skeleton.TrackingId, out userInfo)) {
				return (from InteractionHandPointer hp in userInfo.HandPointers where hp.HandType == (Hand == JointType.HandLeft ? InteractionHandType.Left : InteractionHandType.Right) select hp).FirstOrDefault();
			}

			return null;         
		}

		public override void Update(GameTime gameTime) {
			if (!Player.IsReady)
				return;

				#region dragging
				if (Configuration.GRABBING_ENABLED) {
					var handPointer = getHandPointer();

					if (handPointer != null) {
						if (handPointer.HandEventType == InteractionHandEventType.Grip) {
							IsGrabbing = true;
						} else if (handPointer.HandEventType == InteractionHandEventType.GripRelease) {
							IsGrabbing = false;
						}
					}
				}
				#endregion
				
				var pos = Context.SkeletonPointToScreen(Player.Skeleton.Joints[Hand].Position);
				this.Position = new Vector2(pos.X, pos.Y);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			if (!Player.IsReady)
				return;

			if (Player.DrawHands) {
				var glove = Assets.Images["glove"];
				spriteBatch.Draw(glove, new Rectangle((int)Position.X, (int)Position.Y, 56, 64), null, Color.White * 0.1f, 0, new Vector2(glove.Width / 2, glove.Height / 2), Hand == JointType.HandLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			}
		}

		public override void Delete() {
		}
	}
}