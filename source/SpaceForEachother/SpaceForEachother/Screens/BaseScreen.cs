using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LoveCommander.Bang;
using LoveCommander.Bang.Coroutine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LoveCommander.Bang.Actions;
using System.Diagnostics;

namespace LoveCommander.Screens {

	public abstract class BaseScreen {

		public KinectContext Context { get; private set; }
		public ScreenManager Manager { get; set; }

		public bool Inited { get; private set; }

		public List<BaseEntity> Entities = new List<BaseEntity>();
		public ActionList Actions = new ActionList();
		public Coroutines Coroutines = new Coroutines();

		public float ShakeAmount = 0f;

		public BaseScreen(KinectContext context) {
			Context = context;
			Inited = false;
		}

		public void AddEntity(BaseEntity entity) {
			Entities.Add(entity);
			entity.Screen = this;

			entity.Init();
		}

		public IEnumerator ShakeForXSeconds(float seconds, float amount) {
			ShakeAmount = amount;
			yield return Pause(seconds);
			
			ShakeAmount = 0;
			yield return null;
		}

		public static IEnumerator Pause(float time) {
			var watch = Stopwatch.StartNew();
			while (watch.Elapsed.TotalSeconds < time)
				yield return 0;
		}

		public void RemoveEntity(BaseEntity entity) {
			if (!Entities.Contains(entity))
				return;

			entity.Delete();
			Entities.Remove(entity);
		}

		public virtual void Init() {            
			Inited = true;
		}

		public virtual void Update(GameTime gameTime) {
			//Dirty? Calling ToArray to make a copy of the entity collection preventing crashing when entities create other entities through an update call			
			foreach (var ent in Entities.ToArray()) {
				ent.Update(gameTime);
			}
			

			Actions.Update(gameTime);
			Coroutines.Update();
		}
		public virtual void Draw(SpriteBatch spriteBatch) {
			foreach (var ent in Entities.ToArray().OrderBy(e => e.ZDepth)) {
				ent.Draw(spriteBatch);
			}
		}
	}
}
