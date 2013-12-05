using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace LoveCommander {

	public static class Assets {

		public static Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();
		public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
		public static Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();

		public static Song MainSong;

		public static void LoadContent(ContentManager content) {
			MainSong = content.Load<Song>("Sounds/Rocket");

			Fonts.Add("debug", content.Load<SpriteFont>("DebugFont"));

			Images.Add("glove", content.Load<Texture2D>("glove"));
			Images.Add("glove_fist", content.Load<Texture2D>("glove_fist"));
			Images.Add("alien_round", content.Load<Texture2D>("alien-round"));
			Images.Add("alien", content.Load<Texture2D>("alien-pillow"));
			Images.Add("spaceship", content.Load<Texture2D>("spaceship"));
			Images.Add("pillow", content.Load<Texture2D>("pillo"));

			Images.Add("reticle_blue", content.Load <Texture2D>("shooter-blue"));
			Images.Add("reticle_red", content.Load<Texture2D>("shooter-red"));

			Images.Add("smallstar1", content.Load<Texture2D>("Stars/smallstar1"));
			Images.Add("smallstar2", content.Load<Texture2D>("Stars/smallstar2"));
			Images.Add("smallstar3", content.Load<Texture2D>("Stars/smallstar3"));
			Images.Add("smallstar4", content.Load<Texture2D>("Stars/smallstar4"));
			Images.Add("smallstar5", content.Load<Texture2D>("Stars/smallstar5"));

			Images.Add("specialstar1", content.Load<Texture2D>("Stars/specialstar1"));
			Images.Add("specialstar2", content.Load<Texture2D>("Stars/specialstar2"));

			Images.Add("star1", content.Load<Texture2D>("Stars/star1"));
			Images.Add("star2", content.Load<Texture2D>("Stars/star2"));
			Images.Add("star3", content.Load<Texture2D>("Stars/star3"));

			Images.Add("bg", content.Load<Texture2D>("bg"));
			Images.Add("floor", content.Load<Texture2D>("repeatfloor"));

			Images.Add("planet1_sheet", content.Load<Texture2D>("Planet1/planet1_sheet"));
			Images.Add("asteroid_bar", content.Load<Texture2D>("asteroid_bar"));

			Images.Add("clouds", content.Load<Texture2D>("clouds"));

			Images.Add("shadow_alien", content.Load<Texture2D>("Shadows/alien-pillo"));
			Images.Add("shadow_round", content.Load<Texture2D>("Shadows/alien-round"));
			Images.Add("shadow_planet", content.Load<Texture2D>("Shadows/planet"));

			Images.Add("score", content.Load<Texture2D>("score"));
			Images.Add("beam", content.Load<Texture2D>("tractor_beam"));
			Images.Add("beam_back", content.Load<Texture2D>("tractor_back"));
			Images.Add("shield", content.Load<Texture2D>("shield"));

			Images.Add("startscreen", content.Load<Texture2D>("startscreen"));
			Images.Add("check", content.Load<Texture2D>("startscreen_okey"));

			for (int i = 0; i < 10; i++) {
				Images.Add("number_" + i, content.Load<Texture2D>("Numbers/" + i));
			}

			Images.Add("tutorial_1", content.Load<Texture2D>("Tutorial/1"));
			Images.Add("tutorial_2", content.Load<Texture2D>("Tutorial/2"));
			Images.Add("tutorial_3", content.Load<Texture2D>("Tutorial/3"));
			Images.Add("tutorial_4", content.Load<Texture2D>("Tutorial/4"));
			Images.Add("tutorial_5", content.Load<Texture2D>("Tutorial/5"));
			Images.Add("tutorial_6", content.Load<Texture2D>("Tutorial/6"));

				//Sounds
			Sounds.Add("shoot", content.Load<SoundEffect>("Sounds/shoot"));
			Sounds.Add("hit", content.Load<SoundEffect>("Sounds/hit"));
			Sounds.Add("tension", content.Load<SoundEffect>("Sounds/tension"));
			Sounds.Add("pling", content.Load<SoundEffect>("Sounds/pling"));
			Sounds.Add("asteroid_gone", content.Load<SoundEffect>("Sounds/asteroid_gone"));
		}
	}
}