using System.Collections.Generic;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using System.Timers;

namespace MaxVolume
{
	public class MaxVolume : RocketPlugin<MaxVolumeConfig>
	{
		public static MaxVolume Instance;

		public Timer timer;

		protected override void Load ()
		{
			Instance = this;

			Configuration.Load ();
			if (Configuration.Instance.MaxVolume < 0)
				Configuration.Instance.MaxVolume = 0;
			if (Configuration.Instance.MaxVolume > 100)
				Configuration.Instance.MaxVolume = 100;
			Configuration.Save ();

			timer = new Timer ();
			timer.Interval = 1000;
			timer.Elapsed += Timer_Elapsed;
			timer.Start ();
		}

		private void Timer_Elapsed (object sender, ElapsedEventArgs e)
		{
			if (Configuration.Instance.MaxVolume != 100)
			{
				var stereos = Object.FindObjectsOfType<InteractableStereo> ();
				foreach (var stereo in stereos)
				{
					if (stereo.compressedVolume > Configuration.Instance.MaxVolume)
					{
						byte newVolume = (byte)Configuration.Instance.MaxVolume;
						byte x;
						byte y;
						ushort plant;
						ushort index;
						BarricadeRegion region;
						if (!BarricadeManager.tryGetInfo (stereo.transform, out x, out y, out plant, out index, out region))
							return;
						if ((int)plant == (int)ushort.MaxValue)
							BarricadeManager.instance.channel.send ("tellUpdateStereoVolume", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object)x, (object)y, (object)plant, (object)index, (object)newVolume);
						else
							BarricadeManager.instance.channel.send ("tellUpdateStereoVolume", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object [5]
							{
								(object) x,
								(object) y,
								(object) plant,
								(object) index,
								(object) newVolume
							});
						region.barricades [(int)index].barricade.state [16] = newVolume;
					}
				}
			}
		}

		public override TranslationList DefaultTranslations
		{
			get
			{
				return new TranslationList ()
				{
					{ "set_max", "Set max volume for stereo to {0}%" },
					{ "echo_max", "Max Volume is {0}%" },
					{ "not_num", "{0} is not a number"}
				};
			}
		}
	}
}
