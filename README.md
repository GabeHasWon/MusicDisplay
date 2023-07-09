# MusicDisplay
## FAQ
Q: Can you add in compatiblity for (some mod)?

A: Mods add in their own compatibility. So if you want their mod to support this, show them this page!

It's a fairly simple process so hopefully they listen.

## Compatibility
Adding compatibility is simple.
Adding a track is as follows: 

```
//Where display == MusicDisplay's Mod instance
display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "path/to/music"), "title", "author", "subtitle");
```

This gives the given track a display of

```
Current Music
Title
Author
Subtitle
```

For a further example, this is the code used in Spirit: 

```
public override void PostSetupContent()
{
  if (!ModLoader.TryGetMod("MusicDisplay", out Mod display))
    return;

  void AddMusic(string path, string name, string author) => display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, path), name, "by " + author, "Spirit Mod");

  AddMusic("Sounds/Music/GraniteBiome", "Resounding Stones (Granite Theme)", "salvati");
  //Repeat for every track in the mod
}
```

This would display the following:

```
Current Music:
Resounding Stones (Granite Theme)
by salvati
Spirit Mod
```

--- 

An overload that omits the author is also available, though I do recommend using the author line for a cleaner display.
Code is as follows:

```
display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "path/to/music"), "title", "subtitle");
```

This would display as

```
Current Music:
Track Name
Subtitle
```

## Issues/Suggestions

Feel free to report issues or ask for API suggestions here on this GitHub, on the Steam page, or wherever I am on Discord (@gabehaswon).
Always more than happy to help!
