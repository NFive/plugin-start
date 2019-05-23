using NFive.SDK.Client.Interface;
using System;

namespace NFive.Start.Client.Overlays
{
	public class StartOverlay : Overlay
	{
		public event EventHandler<OverlayEventArgs> Play;

		public StartOverlay(OverlayManager manager) : base("StartOverlay.html", manager)
		{
			Attach("play", (_, callback) => this.Play?.Invoke(this, new OverlayEventArgs(this)));
		}
	}
}
