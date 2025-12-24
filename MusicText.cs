using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.Localization;

namespace MusicDisplay;

internal readonly struct MusicText
{
    public const int MainSlot = 0;
    public const int AuthorSlot = 1;
    public const int SubtitleSlot = 2;
    public const int TitleSlot = 3;

    public static readonly Color[] DefaultColors = [Color.White, new Color(230, 230, 230), new Color(180, 180, 180), new Color(120, 120, 120)];

    public readonly LocalizedText MainText;
    public readonly LocalizedText Author;
    public readonly LocalizedText Subtitle;
    public readonly bool IsUnknown;
    public readonly Func<bool> ShouldDisplay;
    public readonly Color[] Colors;

    public MusicText(LocalizedText mainText, LocalizedText author, LocalizedText subtitle, bool isUnknown = false, Func<bool> shouldDisplay = null, Color[] colors = null)
    {
        MainText = mainText;
        Author = author;
        Subtitle = subtitle;
        IsUnknown = isUnknown;
        ShouldDisplay = shouldDisplay ?? (() => true);
        Colors = colors ?? DefaultColors;

        if (Colors.Length < DefaultColors.Length)
        {
            List<Color> allColors = [.. Colors];

            for (int i = allColors.Count; i < DefaultColors.Length; ++i)
                allColors.Add(DefaultColors[i]);

            Colors = [.. allColors];
        }
    }
}
