﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LoveCommander.Screens;
using LoveCommander.Bang.Actions;

namespace LoveCommander.Entity {
	public class Ufo : BaseEntity {

		public int Lifes = 2;
		bool selected = false;

		private static Random rand = new Random();

		public Ufo(int posX, int posY) : base() {
			Position.X = posX;
			Position.Y = posY;

			Size = new Vector2(403f / 5f, 250f / 5f);
			CollisionType = "ufo";
		}

		public override void Init() {
			
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (GetFirstCollidingEntity("ship") != null) {
				Assets.Sounds["hit"].Play();
				Screen.RemoveEntity(this);
				Screen.AddEntity(new Clouds((int)Position.X + 20, (int)Position.Y));

				var num = new Number((int)Position.X, (int)Position.Y, 50, Color.Red);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score -= 50; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);
			}

			if (GetFirstCollidingEntity("reticle") != null) {
				selected = true;
			} else {
				selected = false;
			}

			Position.X -= 85f * (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (Position.X < -Size.X) {
				Screen.RemoveEntity(this);
				var num = new Number((int)Position.X, (int)Position.Y, 30, Color.Red);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score -= 30; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);
			}

			if (rand.Next(0, 280) < 1) {
				var shootPosition = new Vector2(Position.X, Position.Y + (Size.Y / 2f));
				var velocity = (((GameScreen)Screen).CurrentShip.Position - shootPosition);
				velocity.Normalize();
				velocity *= 500;
				var proj = new Projectile((int)shootPosition.X, (int)shootPosition.Y, velocity);
				Screen.AddEntity(proj);
			}
		}

		public void Hit() {
			Lifes--;
			if (Lifes <= 0) {
				Assets.Sounds["pling"].Play();
				Screen.RemoveEntity(this);
				var num = new Number((int)Position.X, (int)Position.Y, 50, Color.Green);
				num.Actions.AddAction(new TweenPositionTo(num, new Vector2(210, MainGame.Height - 75), 1f, Tweening.Back.EaseIn), true);
				num.Actions.AddAction(new CallFunction(() => { ((GameScreen)num.Screen).CurrentPlayer.Score += 50; num.Screen.RemoveEntity(num); }), true);
				Screen.AddEntity(num);
			}
		}

		public override void Draw(SpriteBatch spriteBatch) {
			if (selected) {
				spriteBatch.Draw(Assets.Images["shadow_alien"], new Rectangle((int)Position.X - 10, (int)Position.Y - 10, (int)Size.X + 20, (int)Size.Y + 20), Color.Green);
			}
			spriteBatch.Draw(Assets.Images["alien"], BoundingBox, Color.White);			
		}

		public override void Delete() {
			
		}
	}
}
