using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MusicDisplay;

internal class DisplaySystem : ModSystem
{
	float Delta => (float)(Main._drawInterfaceGameTime.TotalGameTime.TotalSeconds - setTime.TotalSeconds);

	float alpha = 0;
	TimeSpan setTime = TimeSpan.MinValue;
	MusicText text = default;

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
            bool hide = ModContent.GetInstance<DisplayConfig>().HideUnknown;
			
			if (musicText.ShouldDisplay() && (!hide || !musicText.IsUnknown))
				SetDisplay(musicText);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
		int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Info Accessories Bar"));

		layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
			"MusicDisplay: Music Display",
			delegate
			{
				if (lastMusicSlot > 0)
					DisplayDrawing.DrawMusicDisplay(Delta, ref alpha, text);
				return true;
			},
			InterfaceScaleType.UI)
		);
	}
}
