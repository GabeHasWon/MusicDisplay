using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace MusicDisplay;

/// <summary>
/// Handles drawing the display.
/// </summary>
internal static class DisplayDrawing
{
	public static void DrawMusicDisplay(float delta, ref float alpha, MusicText text)
	{
		if (!ModContent.GetInstance<DisplayConfig>().AlwaysOn) //Sets alpha only if we need to draw fadeout
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
			alpha = ModContent.GetInstance<DisplayConfig>().AlwaysOnOpacity;

		GetStartPosition(ModContent.GetInstance<DisplayConfig>().Placement, out float x, out float y, out Vector2 originMod);
		string title = Language.GetTextValue("Mods.MusicDisplay.CurrentMusic");
		var font = FontAssets.DeathText.Value;
		float scale = ModContent.GetInstance<DisplayConfig>().TextScale;

		y -= scale * 50;

        var size = FontAssets.DeathText.Value.MeasureString(title);
		ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, title, new Vector2(x, y - 40 * scale), new Color(120, 120, 120) * alpha, 0, size * originMod, new Vector2(0.4f) * scale);

		size = FontAssets.DeathText.Value.MeasureString(text.MainText.Value);
		ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text.MainText.Value, new Vector2(x, y), Color.White * alpha, 0, size * originMod, new Vector2(0.85f) * scale);

		size = FontAssets.DeathText.Value.MeasureString(text.Author.Value);
		ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text.Author.Value, new Vector2(x, y + 46 * scale), new Color(230, 230, 230) * alpha, 0, size * originMod, new Vector2(0.65f) * scale);

		size = FontAssets.DeathText.Value.MeasureString(text.Subtitle.Value);
		var subtitlePos = new Vector2(x, text.Author is null || text.Author.Value == string.Empty ? y + 40 * scale : y + 86 * scale);
		ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text.Subtitle.Value, subtitlePos, new Color(180, 180, 180) * alpha, 0, size * originMod, new Vector2(0.5f) * scale);
	}

	private static void GetStartPosition(DisplayConfig.Placements placement, out float x, out float y, out Vector2 originMod)
	{
		if (placement == DisplayConfig.Placements.Bottom || placement == DisplayConfig.Placements.Top)
		{
			x = Main.screenWidth / 2f;
			y = placement == DisplayConfig.Placements.Top ? 100 : Main.screenHeight - 150;
			originMod = new Vector2(0.5f);
		}
		else
		{
			x = placement == DisplayConfig.Placements.Left ? 10 : Main.screenWidth - 20;
			y = Main.screenHeight / 2f - 80;
			originMod = placement == DisplayConfig.Placements.Left ? new Vector2(0, 0.5f) : new Vector2(1f, 0.5f);
		}
	}
}
