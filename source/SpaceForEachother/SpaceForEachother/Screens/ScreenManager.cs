using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LoveCommander.Bang;
using Microsoft.Xna.Framework.Media;
using LoveCommander.Bang.Actions;
using Microsoft.Xna.Framework.Input;

namespace LoveCommander.Screens {
	public class ScreenManager {
		private List<BaseScreen> Screens = new List<BaseScreen>();

		public ActionList Actions = new ActionList();

		public BaseScreen TopScreen {
			get {
				return Screens.Last();
			}
		}

		public MainGame Game { get; private set; }

		KeyboardState previousState;
		KeyboardState state;

		bool paused = false;

		public ScreenManager(MainGame game) {
			Game = game;
		}

		public void AddScreen(BaseScreen screen) {
			Screens.Add(screen);
			screen.Manager = this;
			
			if (!screen.Inited)
				screen.Init();            
		}

		public void FadeInSong(Song song, bool repeat, float maxVolume) {
			Actions.AddAction(new FadeInSong(song, repeat, maxVolume), true);
		}

		public void RemoveScreen(BaseScreen screen) {
			Screens.Remove(screen);
		}

		public void SwitchScreen(BaseScreen screen) {
			Screens.Clear();
			AddScreen(screen);
		}

		public void Update(GameTime gameTime) {
			previousState = state;
			state = Keyboard.GetState();
			if (!previousState.IsKeyDown(Keys.P) && state.IsKeyDown(Keys.P)) {
				paused = !paused;
				if (MediaPlayer.State == MediaState.Playing) {
					MediaPlayer.Pause();
				} else {
					MediaPlayer.Resume();
				}
			}

			if (!paused) {
				Actions.Update(gameTime);
				Screens.Last().Update(gameTime);                             
			}
		}

		public void Draw(ExtendedSpriteBatch spriteBatch) {
			foreach (var screen in Screens) {
				screen.Draw(spriteBatch);
			}
		}        
	}
}
