using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using JetBrains.Annotations;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;

namespace NFive.Start.Client
{
	[PublicAPI]
	public class StartService : Service
	{
		public StartService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, INuiManager nui, User user) : base(logger, ticks, events, rpc, nui, user)
		{
			Task.Factory.StartNew(async () =>
			{
				// Position character 
				Game.Player.Character.Position = new Vector3(0f, 0f, 71f);

				// Load character model
				while (!await Game.Player.ChangeModel(new Model(PedHash.FreemodeMale01))) await BaseScript.Delay(100);
				Game.Player.Character.Style.SetDefaultClothes();

				// Fade out
				Screen.Fading.FadeOut(1000);
				while (Screen.Fading.IsFadingOut) await BaseScript.Delay(100);

				// Hide loading screen
				API.ShutdownLoadingScreen();

				// Set menus
				API.SetPauseMenuActive(true);
				API.SetNoLoadingScreen(true);

				// Fade in
				Screen.Fading.FadeIn(1000);
				while (Screen.Fading.IsFadingIn) await BaseScript.Delay(100);
			});
		}
	}
}
