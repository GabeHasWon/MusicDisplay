using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MusicDisplay;

internal class DisplayConfig : ModConfig
{
	public enum Placements
    {
		Top,
		Bottom
    }

    public override ConfigScope Mode => ConfigScope.ClientSide;

	[Label("Hide Unknown Music")]
	[Tooltip("If true, unknown music tracks will simply not be displayed. Otherwise, \"Unknown Track\" will show in place of the track name.")]
	[DefaultValue(true)]
	public bool HideUnknown { get; set; }

	[Label("Display Placement")]
	[Tooltip("Controls where the display is showcased.")]
	[DefaultValue(Placements.Top)]
	[Slider]
	public Placements Placement { get; set; }
}
