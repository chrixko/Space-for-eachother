using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LoveCommander.Screens;
using LoveCommander.Bang;
using LoveCommander.Bang.Coroutine;

namespace LoveCommander {

	public abstract class BaseEntity {

		public Vector2 Position = Vector2.Zero;

		public ActionList Actions = new ActionList();
		public Coroutines Coroutines = new Coroutines();

		public Vector2 Size { get; set; }
		public Vector2 Offset { get; set; }

		public BaseScreen Screen { get; set; }

		public bool collidable = true;
		public string CollisionType { get; set; }

		public int ZDepth = 1;

		public IEnumerable<BaseEntity> GetCollidingEntities(string type) {
			return from ent in Screen.Entities where ent.collidable && ent.CollisionType == type && ent.BoundingBox.Intersects(BoundingBox) select ent;
		}

		public BaseEntity GetFirstCollidingEntity(string type) {
			return GetCollidingEntities(type).FirstOrDefault();
		}

		public Rectangle BoundingBox {
			get {
				return new Rectangle((int)Position.X + (int)Offset.X, (int)Position.Y + (int)Offset.Y, (int)Size.X, (int)Size.Y);
			}
		}

		public abstract void Init();
		public virtual void Update(GameTime gameTime) {
			Actions.Update(gameTime);
			Coroutines.Update();
		}
		public abstract void Draw(SpriteBatch spriteBatch);
		public abstract void Delete();
	}
}
