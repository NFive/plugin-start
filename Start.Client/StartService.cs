using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Extensions;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Rpc;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using NFive.Start.Client.Overlays;
using System.Threading.Tasks;

namespace NFive.Start.Client
{
	[PublicAPI]
	public class StartService : Service
	{
		public StartService(ILogger logger, ITickManager ticks, IEventManager events, IRpcHandler rpc, ICommandManager commands, OverlayManager overlayManager, User user) : base(logger, ticks, events, rpc, commands, overlayManager, user) { }

		public override async Task Started()
		{
			// Hide HUD
			Screen.Hud.IsVisible = false;

			// Disable the loading screen from automatically being dismissed
			API.SetManualShutdownLoadingScreenNui(true);

			// Position character, required for switching
			Game.Player.Character.Position = Vector3.Zero;

			// Freeze
			Game.Player.Freeze();

			// Switch out the player if it isn't already in a switch state
			if (!API.IsPlayerSwitchInProgress()) API.SwitchOutPlayer(API.PlayerPedId(), 0, 1);

			// Remove most clouds
			API.SetCloudHatOpacity(0.01f);

			// Wait for switch
			while (API.GetPlayerSwitchState() != 5) await this.Delay(10);

			// Hide loading screen
			API.ShutdownLoadingScreen();

			// Fade out
			Screen.Fading.FadeOut(0);
			while (Screen.Fading.IsFadingOut) await this.Delay(10);

			// Show overlay
			var overlay = new StartOverlay(this.OverlayManager);
			overlay.Play += OnPlay;

			// Focus overlay
			API.SetNuiFocus(true, true);

			// Shut down the NUI loading screen
			API.ShutdownLoadingScreenNui();

			// Fade in
			Screen.Fading.FadeIn(500);
			while (Screen.Fading.IsFadingIn) await this.Delay(10);
		}

		private async void OnPlay(object sender, OverlayEventArgs e)
		{
			// Destroy overlay
			e.Overlay.Dispose();

			// Un-focus overlay
			API.SetNuiFocus(false, false);

			// Position character 
			Game.Player.Character.Position = new Vector3(0f, 0f, 71f);

			// Load character model
			while (!await Game.Player.ChangeModel(new Model(PedHash.FreemodeMale01))) await this.Delay(10);
			Game.Player.Character.Style.SetDefaultClothes();

			// Unfreeze
			Game.Player.Unfreeze();

			// Show HUD
			Screen.Hud.IsVisible = true;

			// Switch in
			API.SwitchInPlayer(API.PlayerPedId());
		}
	}
}
