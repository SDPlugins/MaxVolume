using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MaxVolume
{
	public class MaxVolumeCommand : IRocketCommand
	{
		public List<string> Aliases
		{
			get
			{
				return new List<string> () { "mv" };
			}
		}

		public AllowedCaller AllowedCaller
		{
			get
			{
				return AllowedCaller.Both;
			}
		}

		public string Help
		{
			get
			{
				return "Set max stereo volume";
			}
		}

		public string Name
		{
			get
			{
				return "maxvolume";
			}
		}

		public List<string> Permissions
		{
			get
			{
				return new List<string> () { "maxvolume" };
			}
		}

		public string Syntax
		{
			get
			{
				return "<volume>";
			}
		}

		public void Execute (IRocketPlayer caller, string [] command)
		{
			UnturnedPlayer player = UnturnedPlayer.FromName (caller.DisplayName);
			if (command.Length > 0)
			{
				int vol = 0;
				if (int.TryParse (command [0], out vol))
				{
					if (vol < 0)
						vol = 0;
					if (vol > 100)
						vol = 100;

					MaxVolume.Instance.Configuration.Load ();

					MaxVolume.Instance.Configuration.Instance.MaxVolume = vol;

					MaxVolume.Instance.Configuration.Save ();
					UnturnedChat.Say (player, MaxVolume.Instance.Translate ("set_max", MaxVolume.Instance.Configuration.Instance.MaxVolume), Color.green);
					Rocket.Core.Logging.Logger.Log (MaxVolume.Instance.Translate ("set_max", MaxVolume.Instance.Configuration.Instance.MaxVolume));
				}
				else
				{
					UnturnedChat.Say (player, MaxVolume.Instance.Translate ("not_num", command [0]), Color.red);
					Rocket.Core.Logging.Logger.Log (MaxVolume.Instance.Translate ("not_num", command [0]));
				}
			}
			else
			{
				UnturnedChat.Say (player, MaxVolume.Instance.Translate ("echo_max", MaxVolume.Instance.Configuration.Instance.MaxVolume), Color.green);
				Rocket.Core.Logging.Logger.LogError (MaxVolume.Instance.Translate ("echo_max", MaxVolume.Instance.Configuration.Instance.MaxVolume));
			}
		}
	}
}
