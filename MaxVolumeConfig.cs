using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxVolume
{
	public class MaxVolumeConfig : IRocketPluginConfiguration
	{

		public int MaxVolume;

		public void LoadDefaults ()
		{
			MaxVolume = 10;
		}
	}
}
