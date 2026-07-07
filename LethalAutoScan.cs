using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LethalAutoScan.Commands;
using UnityEngine;

namespace LethalAutoScan;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(ChatCommandAPI.MyPluginInfo.PLUGIN_GUID)]
public class LethalAutoScan : BaseUnityPlugin
{
    internal const string MOD_ID =
        $"<size=0>{MyPluginInfo.PLUGIN_GUID}-{MyPluginInfo.PLUGIN_VERSION}</size>";

    internal bool DisableCurrentDay;

    internal string CachedInteriorName = null!;
    internal uint CachedScrapAmount;
    internal uint CachedBeehiveAmount;
    internal uint CachedEggAmount;

    private ConfigEntry<string> interiorFormat = null!;
    private ConfigEntry<string> interiorUnknown = null!;
    private ConfigEntry<string> scrapAmountFormat = null!;
    private ConfigEntry<string> lowScrapAmount = null!;
    private ConfigEntry<string> beehiveAmountFormat = null!;
    private ConfigEntry<string> lowBeehiveAmount = null!;
    private ConfigEntry<string> eggAmountFormat = null!;
    private ConfigEntry<string> lowEggAmount = null!;

    public static LethalAutoScan Instance { get; private set; } = null!;
    internal static new ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    private void Awake()
    {
        const string SECTION_GENERAL = "General";

        Logger = base.Logger;
        Instance = this;

        interiorFormat = Config.Bind(
            SECTION_GENERAL,
            nameof(interiorFormat),
            "{0}",
            "The format to use for the interior name (0: string InteriorName)"
        );
        interiorUnknown = Config.Bind(
            SECTION_GENERAL,
            nameof(interiorUnknown),
            "<color=#ffff00>[UNKNOWN INTERIOR]</color>",
            $"The fallback interior name (ignores {nameof(interiorFormat)})"
        );
        scrapAmountFormat = Config.Bind(
            SECTION_GENERAL,
            nameof(scrapAmountFormat),
            " {0}",
            "The format to use for the scrap amount (0: uint ScrapAmount)"
        );
        lowScrapAmount = Config.Bind(
            SECTION_GENERAL,
            nameof(lowScrapAmount),
            " <color=#ff0000>0</color>",
            $"The string to use for the scrap amount when it's 0 (ignores {nameof(scrapAmountFormat)})"
        );
        beehiveAmountFormat = Config.Bind(
            SECTION_GENERAL,
            nameof(beehiveAmountFormat),
            " {0}",
            "The format to use for the beehive amount (0: uint BeehiveAmount)"
        );
        lowBeehiveAmount = Config.Bind(
            SECTION_GENERAL,
            nameof(lowBeehiveAmount),
            " <color=#000000aa>0</color>",
            $"The string to use for the beehive amount when it's 0 (ignores {nameof(beehiveAmountFormat)})"
        );
        eggAmountFormat = Config.Bind(
            SECTION_GENERAL,
            nameof(eggAmountFormat),
            "+{0}",
            "The format to use for the egg amount (0: uint EggAmount)"
        );
        lowEggAmount = Config.Bind(
            SECTION_GENERAL,
            nameof(lowEggAmount),
            "",
            $"The string to use for the egg amount when it's 0 (ignores {nameof(eggAmountFormat)})"
        );

        _ = new CLScanCommand();
        _ = new SVScanCommand();

        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);
        Logger.LogDebug("Patching...");
        Harmony.PatchAll();
        Logger.LogDebug("Finished patching!");

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    internal void CacheItemAmounts()
    {
        try
        {
            CachedInteriorName = RoundManager.Instance.currentDungeonType switch
            {
                0
                or 2 // Large version (unused)
                or 3 // March version (3 exits)
                => "Factory",
                1 => "Mansion",
                4 => "Mineshaft",
                _ => null!,
            };
        }
        catch (Exception e)
        {
            Logger.LogError(e);
            CachedInteriorName = null!;
        }

        CachedScrapAmount = CachedBeehiveAmount = CachedEggAmount = 0;

        foreach (var item in FindObjectsByType<GrabbableObject>(FindObjectsSortMode.None))
        {
            if (item.scrapPersistedThroughRounds)
                continue;

            switch (item.name)
            {
                // yes this is a lazy way of doing this but it avoids timing problems that i dont wanna deal with
                case "RedLocustHive(Clone)":
                    ++CachedBeehiveAmount;
                    break;
                case "KiwiBabyItem(Clone)":
                    // untested cause bird is so fucking rare now
                    ++CachedEggAmount;
                    break;
                default:
                    ++CachedScrapAmount;
                    break;
            }
        }
    }

    public string GetMessage()
    {
        return (
                string.IsNullOrWhiteSpace(CachedInteriorName)
                    ? interiorUnknown.Value
                    : string.Format(interiorFormat.Value, CachedInteriorName)
            )
            + (
                CachedScrapAmount > 0
                    ? string.Format(scrapAmountFormat.Value, CachedScrapAmount)
                    : lowScrapAmount.Value
            )
            + (
                CachedBeehiveAmount > 0
                    ? string.Format(beehiveAmountFormat.Value, CachedBeehiveAmount)
                    : lowBeehiveAmount.Value
            )
            + (
                CachedEggAmount > 0
                    ? string.Format(eggAmountFormat.Value, CachedEggAmount)
                    : lowEggAmount.Value
            )
            + MOD_ID;
    }
}
