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
		string now = Language.GetTextValue("Mods.MusicDisplay.CurrentMusic");
		var font = FontAssets.DeathText.Value;
		float scale = ModContent.GetInstance<DisplayConfig>().TextScale;
		Color[] colors = text.Colors;
		string main = text.MainText.Value;
		string author = text.Author.Value;
		string subTitle = text.Subtitle.Value;

		if (!MusicDatabase.PreDrawById.TryGetValue((short)Main.curMusic, out MusicDatabase.PreDisplay display) || display(ref now, ref main, ref author, ref subTitle, ref scale, colors, ref delta, 8,
			ref x, ref y, ref originMod, ref alpha, !ModContent.GetInstance<DisplayConfig>().AlwaysOn ? null : ModContent.GetInstance<DisplayConfig>().AlwaysOnOpacity))
		{
            y -= scale * 50;

            var size = FontAssets.DeathText.Value.MeasureString(now);
            DrawString(now, new Vector2(x, y - 40 * scale), colors[MusicText.TitleSlot] * alpha, 0, size * originMod, new Vector2(0.4f) * scale);

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
