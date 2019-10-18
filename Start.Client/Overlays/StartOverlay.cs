using System;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Services;

namespace NFive.Start.Client.Overlays
{
	public class StartOverlay : Overlay
	{
		public event EventHandler<OverlayEventArgs> Play;

		public StartOverlay(OverlayManager manager) : base(manager) { }

		public override dynamic Ready()
		{
			On("play", () => this.Play?.Invoke(this, new OverlayEventArgs(this)));

			return new
			{
				Text = Service._("Play") // Translated button text
			};
		}
	}
}
