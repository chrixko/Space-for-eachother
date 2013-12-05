using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LoveCommander.Pillo;
using Microsoft.Xna.Framework.Audio;
using LoveCommander.Bang.Actions;
using LoveCommander.Screens;

namespace LoveCommander.Entity {
	public class Asteroid : BaseEntity {

		public PlayerHand LockedHand;
		public bool Locked = false;
		public Vector2 LockOffset = Vector2.Zero;

		public Vector2 Velocity;

		private SoundEffectInstance tensionSound;

		public Animation Animation;

		private Animation beamAnimation;
		bool selected = false;

		float beamRotation;

		public Asteroid(int posX, int posY) : base() {
			Animation = new Animation("planet_turn", Assets.Images["planet1_sheet"], 182);
			Animation.FrameTime = 0.05f;
			Animation.Play(true);

			beamAnimation = new Animation("beam_back", Assets.Images["beam_back"], 450);
			beamAnimation.FrameTime = 0.01f;
			beamAnimation.Play(true);

			tensionSound = Assets.Sounds["tension"].CreateInstance();
			Position.X = posX;
			Position.Y = posY;

			Size = new Vector2(182f / 1.5f, 182f / 1.5f);

			Velocity = new Vector2(-150, 0);

			CollisionType = "asteroid";
		}

		public override void Init() {
			
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (GetFirstCollidingEntity("ship") != null) {
				Assets.Sounds["hit"].Play();
				Screen.RemoveEntity(this);
				Screen.AddEntity(new Clouds((int)Position.X + 20, (int)Position.Y));

				var num = new Number((int)Position.X, (int)Position.Y, 70, Color.Red);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score -= 70; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);		
			}

			if (Position.X < -Size.X) {
				Screen.RemoveEntity(this);
				var num = new Number((int)Position.X, (int)Position.Y, 30, Color.Red);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score -= 30; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);
			}

			if (!Actions.IsComplete())
				return;

			var hand = (PlayerHand)GetFirstCollidingEntity("hand_right");
			if (hand == null) {
				hand = (PlayerHand)GetFirstCollidingEntity("hand_left");
			}
			if (hand != null && !hand.IsGrabbing) {
				selected = true;
			} else {
				selected = false;
			}

			if (LockedHand == null && Position.Y > 50 && Position.Y + Size.Y < MainGame.Height - 50) {
				if (hand != null && hand.GrabbingObject == null && PilloState.Holding(hand)) {
					LockOffset = Position - hand.Position;
					LockedHand = hand;
					LockedHand.GrabbingObject = this;
				} else {
					if (LockedHand != null) {
						LockedHand.GrabbingObject = null;
					}
					
					LockedHand = null;
				}
			} else {
				if (!PilloState.Holding(LockedHand)) {
					if (LockedHand != null) {
						LockedHand.GrabbingObject = null;
					}
					
					LockedHand = null;
					
					Screen.ShakeAmount = 0f;
				}
			}

			if (LockedHand != null) {
				beamAnimation.Update(gameTime);
				var ship = ((GameScreen)Screen).CurrentShip;
				var shipPos = ship.Position + new Vector2(ship.Size.X, ship.Size.Y / 2f);

				var ret = LockedHand.Hand == Microsoft.Kinect.JointType.HandLeft ? ((GameScreen)Screen).LeftReticle : ((GameScreen)Screen).RightReticle;

				var target = new Vector2(ret.BoundingBox.Center.X, ret.BoundingBox.Center.Y);
				var delta = target - shipPos;
				beamRotation = -(float)Math.Atan2(delta.X, delta.Y) + MathHelper.ToRadians(90);				

				if (tensionSound.State != SoundState.Playing) {
					tensionSound.Play();
				}				

				var desiredPosition = (LockedHand.Position + LockOffset) - Position;
				if (desiredPosition != Vector2.Zero) {
					desiredPosition.Normalize();
					Velocity = desiredPosition * 400;

					Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
					if (Position.Y < 50 || Position.Y + Size.Y > MainGame.Height - 50) {
						LockedHand.GrabbingObject = null;
						LockedHand = null;
						if (Position.Y < 50) {
							Actions.AddAction(new TweenPositionTo(this, new Vector2(Position.X, -Size.Y), 0.3f, Tweening.Back.EaseIn), true);
						} else {
							Actions.AddAction(new TweenPositionTo(this, new Vector2(Position.X, MainGame.Height), 0.3f, Tweening.Back.EaseIn), true);
						}

						Actions.AddAction(new CallFunction(() => { addScore(); Screen.RemoveEntity(this); }), true);

						Assets.Sounds["pling"].Play();
					}
					if (Position.Y + Size.Y > MainGame.Height - 50) {
						LockedHand = null;
						Actions.AddAction(new TweenPositionTo(this, new Vector2(Position.X, MainGame.Height), 0.3f, Tweening.Back.EaseIn), true);
						Actions.AddAction(new CallFunction(() => { Screen.RemoveEntity(this); }), true);

						Assets.Sounds["pling"].Play();
					}
				}

				Screen.ShakeAmount = 2f;
			} else {				
				Position.X -= 150f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			Animation.Update(gameTime);
		}

		void addScore() {
			var num = new Number((int)Position.X, (int)Position.Y, 100, Color.Green);
			num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
			num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score += 100; num.Screen.RemoveEntity(num); }), true);
			Screen.AddEntity(num);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			if (LockedHand != null) {
				spriteBatch.Draw(Assets.Images["asteroid_bar"], new Rectangle(0, 0, MainGame.Width, 50), Color.Green);
				spriteBatch.Draw(Assets.Images["asteroid_bar"], new Rectangle(0, MainGame.Height - 50, MainGame.Width, 50), null, Color.Green, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
			}

			if (selected) {
				spriteBatch.Draw(Assets.Images["shadow_planet"], new Rectangle((int)Position.X - 10, (int)Position.Y - 10, (int)Size.X + 20, (int)Size.Y + 20), Color.Green);
			}

			if (LockedHand != null) {
				var ship = ((GameScreen)Screen).CurrentShip;
				var shipPos = ship.Position + new Vector2(ship.Size.X, ship.Size.Y / 2f);
				var ret = LockedHand.Hand == Microsoft.Kinect.JointType.HandLeft ? ((GameScreen)Screen).LeftReticle : ((GameScreen)Screen).RightReticle;

				var vec = (new Vector2(ret.BoundingBox.Center.X, ret.BoundingBox.Center.Y) - shipPos) / 2f;
				var pos = shipPos + vec;
				spriteBatch.Draw(Assets.Images["beam"], new Rectangle((int)pos.X, (int)pos.Y, (int)(vec.Length() * 2), Assets.Images["beam"].Height), null, Color.White, beamRotation, new Vector2(Assets.Images["beam"].Width / 2, Assets.Images["beam"].Height / 2), SpriteEffects.None, 0);

				spriteBatch.Draw(Assets.Images["beam_back"], new Rectangle((int)Position.X - 30, (int)Position.Y - 30, (int)Size.X + 60, (int)Size.Y + 60), beamAnimation.FrameRectangle, Color.White);
			}			

			spriteBatch.Draw(Assets.Images["planet1_sheet"], BoundingBox, Animation.FrameRectangle, Color.White);
			

		}

		public override void Delete() {			
		}
	}
}
