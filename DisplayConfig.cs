using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using static System.Net.Mime.MediaTypeNames;

namespace MusicDisplay;

internal class DisplayConfig : ModConfig
{
	public enum Placements
    {
		Top,
		Bottom,
		Left,
		Custom,
    }

    public override ConfigScope Mode => ConfigScope.ClientSide;

	[DefaultValue(true)]
	public bool HideUnknown { get; set; }

	[DefaultValue(Placements.Bottom)]
	[Slider]
	public Placements Placement { get; set; }

	[DefaultValue(false)]
	public bool AlwaysOn { get; set; }

	[DefaultValue(0.8f)]
	public float AlwaysOnOpacity { get; set; }

    [DefaultValue(1f)]
	[Range(0.75f, 2f)]
    public float TextScale { get; set; }

	[DefaultValue("0, 0")]
	public Vector2 TextPlacement { get; set; }
}

/// <summary>
/// Used to preview the display in the config.
/// </summary>
public class DisplayPreview : ILoadable
{
    const string Text = "Mods.MusicDisplay.TrackNames.";

    private static readonly MusicText SampleText = new(Language.GetText(Text + "SampleTrack"), Language.GetText(Text + "SampleMusician"), Language.GetText(Text + "MusicDisplay"));

    public void Load(Mod mod) => MonoModHooks.Add(typeof(UIModConfig).GetMethod("Draw"), HookDraw);

	public static void HookDraw(Action<UIModConfig, SpriteBatch> orig, UIModConfig self, SpriteBatch batch)
	{
		orig(self, batch);

		if (self.pendingConfig is DisplayConfig config)
		{
			float alpha = 0;
            DisplayDrawing.DrawMusicDisplay(3, ref alpha, SampleText, config.AlwaysOn ? config.AlwaysOnOpacity : 1, config);
		}
	}

	public void Unload() { }
}