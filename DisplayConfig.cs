using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MusicDisplay;

internal class DisplayConfig : ModConfig
{
	public enum Placements
    {
		Top,
		Bottom,
		Left,
    }

    public override ConfigScope Mode => ConfigScope.ClientSide;

	//Label/Tooltip attributes stay so it supports 1.4.3 without whatever garbage I have to do otherwise.

	//[Label("Hide Unknown Music")]
	//[Tooltip("If true, unknown music tracks will simply not be displayed. Otherwise, \"Unknown Track\" will show in place of the track name.")]
	[DefaultValue(true)]
	public bool HideUnknown { get; set; }

	//[Label("Display Placement")]
	//[Tooltip("Controls where the display is showcased.")]
	[DefaultValue(Placements.Bottom)]
	[Slider]
	public Placements Placement { get; set; }

	//[Label("Always On")]
	//[Tooltip("If true, the music track will be displayed at all times.")]
	[DefaultValue(false)]
	public bool AlwaysOn { get; set; }

	//[Label("Always On Opacity")]
	//[Tooltip("How opaque the music display is when Always On is active. Does nothing if Always On is not on.")]
	[DefaultValue(0.8f)]
	public float AlwaysOnOpacity { get; set; }

    [DefaultValue(1f)]
	[Range(0.75f, 2f)]
    public float TextScale { get; set; }
}
