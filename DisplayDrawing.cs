using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace MusicDisplay;

#nullable enable

/// <summary>
/// Handles drawing the display.
/// </summary>
internal static class DisplayDrawing
{
	public static void DrawMusicDisplay(float delta, ref float alpha, MusicText text, float? forceDrawAlpha = null, DisplayConfig configInstance = null!)
	{
		configInstance ??= ModContent.GetInstance<DisplayConfig>();

        if (!configInstance.AlwaysOn && forceDrawAlpha is null) //Sets alpha only if we need to draw fadeout
		{
			float adjDelta = delta - 3f;

			if (adjDelta < 0f)
				return;

			if (adjDelta < 3f)
				alpha = adjDelta / 3f;
			else if (adjDelta > 5f && adjDelta <= 8f)
				alpha = 1 - (adjDelta - 5f) / 3f;
			else if (adjDelta > 8)
				alpha = 0;
		}
		else //Otherwise user-selected transparency always
			alpha = forceDrawAlpha ?? configInstance.AlwaysOnOpacity;

		GetStartPosition(configInstance.Placement, out float x, out float y, out Vector2 originMod, configInstance);
		string now = Language.GetTextValue("Mods.MusicDisplay.CurrentMusic");
		var font = FontAssets.DeathText.Value;
		float scale = configInstance.TextScale;
		Color[] colors = text.Colors;
		string main = text.MainText.Value;
		string author = text.Author.Value;
		string subTitle = text.Subtitle.Value;

		if (!MusicDatabase.PreDrawById.TryGetValue((short)Main.curMusic, out MusicDatabase.PreDisplay? display) || display(ref now, ref main, ref author, ref subTitle, ref scale, colors, ref delta, 8,
			ref x, ref y, ref originMod, ref alpha, !configInstance.AlwaysOn ? null : configInstance.AlwaysOnOpacity))
		{
            y -= scale * 50;

            var size = FontAssets.DeathText.Value.MeasureString(now);
            DrawString(now, new Vector2(x, y - 20 * scale), colors[MusicText.TitleSlot] * alpha, 0, size * originMod, new Vector2(0.4f) * scale);

			size = FontAssets.DeathText.Value.MeasureString(main);
            DrawString(main, new Vector2(x, y), colors[MusicText.MainSlot] * alpha, 0, size * originMod, new Vector2(0.85f) * scale);

			size = FontAssets.DeathText.Value.MeasureString(author);
            DrawString(author, new Vector2(x, y + 46 * scale), colors[MusicText.AuthorSlot] * alpha, 0, size * originMod, new Vector2(0.65f) * scale);

			size = FontAssets.DeathText.Value.MeasureString(subTitle);
			var subtitlePos = new Vector2(x, string.IsNullOrEmpty(author) ? y + 40 * scale : y + 86 * scale);
			DrawString(subTitle, subtitlePos, colors[MusicText.SubtitleSlot] * alpha, 0, size * originMod, new Vector2(0.5f) * scale);
		}

        void DrawString(string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f) 
			=> ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text, position, baseColor, rotation, origin, baseScale, maxWidth, spread);
    }

	private static void GetStartPosition(DisplayConfig.Placements placement, out float x, out float y, out Vector2 originMod, DisplayConfig config)
	{
		int screenWidth = Main.screenWidth;
		int screenHeight = Main.screenHeight;

		if (placement == DisplayConfig.Placements.Custom)
		{
			x = screenWidth * config.TextPlacement.X;
			y = screenHeight * config.TextPlacement.Y;
			originMod = config.TextPlacement * new Vector2(1, 0);
			return;
		}

        if (placement is DisplayConfig.Placements.Bottom or DisplayConfig.Placements.Top)
		{
			x = screenWidth / 2f;
			y = placement == DisplayConfig.Placements.Top ? 140 : screenHeight - 150;
			originMod = new Vector2(0.5f, 0);
		}
		else
		{
			x = placement == DisplayConfig.Placements.Left ? 10 : screenWidth - 20;
			y = screenHeight / 2f - 10;
			originMod = placement == DisplayConfig.Placements.Left ? new Vector2(0, 0) : new Vector2(1f, 0);
		}
	}
}
