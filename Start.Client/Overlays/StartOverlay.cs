using System;
using NFive.SDK.Client.Interface;
using NFive.SDK.Core.Locales;

namespace NFive.Start.Client.Overlays
{
	public class StartOverlay : Overlay
	{
		private readonly ILocaleCatalog catalog;

		public event EventHandler<OverlayEventArgs> Play;

		public StartOverlay(IOverlayManager manager, ILocaleCatalog catalog) : base(manager)
		{
			this.catalog = catalog;
		}

		protected override dynamic Ready()
		{
			On("play", () => this.Play?.Invoke(this, new OverlayEventArgs(this)));

			return new
			{
				Text = this.catalog._("Play") // Translated button text
			};
		}
	}
}
