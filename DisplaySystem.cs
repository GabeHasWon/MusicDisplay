using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace MusicDisplay;

internal class DisplaySystem : ModSystem
{
	float Delta => (float)(Main._drawInterfaceGameTime.TotalGameTime.TotalSeconds - setTime.TotalSeconds);

	float alpha = 0;
	TimeSpan setTime = TimeSpan.MinValue;
	MusicText text = new MusicText(string.Empty, string.Empty, string.Empty, false);

	short lastMusicSlot = -1;

	public static void SetDisplay(MusicText text)
    {
		if (Main.dedServ || Main.netMode == NetmodeID.Server)
			return;

		var system = ModContent.GetInstance<DisplaySystem>();

		system.alpha = 0;
		system.setTime = Main.gameTimeCache.TotalGameTime;
		system.text = text;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (Main.curMusic != lastMusicSlot && Main.musicVolume > 0)
        {
			lastMusicSlot = (short)Main.curMusic;

			var musicText = MusicDatabase.GetMusicText(lastMusicSlot);
			var hide = ModContent.GetInstance<DisplayConfig>().HideUnknown;
			
			if (!hide || !musicText.IsUnknown)
				SetDisplay(musicText);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
		int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));

		layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
			"MusicDisplay: Music Display",
			delegate
			{
				if (lastMusicSlot > 0)
					DrawMusicDisplay();
				return true;
			},
			InterfaceScaleType.UI)
		);
	}

    private void DrawMusicDisplay()
    {
		float adjDelta = Delta - 3f;

		if (adjDelta < 0f)
			return;

		if (adjDelta < 3f)
			alpha = adjDelta / 3f;
		else if (adjDelta > 5f && adjDelta <= 8f)
			alpha = 1 - ((adjDelta - 5f) / 3f);
		else if (adjDelta > 8)
			alpha = 0;

		float x = Main.screenWidth / 2f;
		float y = ModContent.GetInstance<DisplayConfig>().Placement == DisplayConfig.Placements.Top ? 100 : Main.screenHeight - 150;
		string title = "Current Music:";
		var font = FontAssets.DeathText.Value;

		var size = FontAssets.DeathText.Value.MeasureString(title);
		ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, title, new Vector2(x, y - 36), new Color(120, 120, 120) * alpha, 0, size / 2f, new(0.4f));

		size = FontAssets.DeathText.Value.MeasureString(text.MainText);
		ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text.MainText, new Vector2(x, y), Color.White * alpha, 0, size / 2f, new(0.85f));

		size = FontAssets.DeathText.Value.MeasureString(text.Author);
		ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text.Author, new Vector2(x, y + 46), new Color(230, 230, 230) * alpha, 0, size / 2f, new(0.65f));

		size = FontAssets.DeathText.Value.MeasureString(text.Subtitle);
		var subtitlePos = new Vector2(x, text.Author is null || text.Author == string.Empty ? y + 40 : y + 86);
		ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, text.Subtitle, subtitlePos, new Color(180, 180, 180) * alpha, 0, size / 2f, new(0.5f));
	}
}
