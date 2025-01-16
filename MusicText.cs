﻿using System;
using Terraria.Localization;

namespace MusicDisplay;

internal readonly struct MusicText(LocalizedText mainText, LocalizedText author, LocalizedText subtitle, bool isUnknown = false, Func<bool> shouldDisplay = null)
{
    public readonly LocalizedText MainText = mainText;
    public readonly LocalizedText Author = author;
    public readonly LocalizedText Subtitle = subtitle;
    public readonly bool IsUnknown = isUnknown;
    public readonly Func<bool> ShouldDisplay = shouldDisplay ?? (() => true);
}
