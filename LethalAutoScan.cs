using System;
using System.Collections;
using BepInEx;
using BepInEx.Logging;
using ChatCommandAPI.Utils;
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

    internal bool IsValid;
#if !SV
    internal bool DisableCurrentDay;
#endif

    internal string CachedInteriorName = null!;
    internal uint CachedScrapAmount;
    internal uint CachedBeehiveAmount;
    internal uint CachedEggAmount;

    public static LethalAutoScan Instance { get; private set; } = null!;
    internal static new ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

#if !SV
        _ = new CLScanCommand();
#else
        Logger.LogWarning(
            "You are using the host-only version of this mod. It will not activate in any games you join"
        );
#endif
        _ = new SVScanCommand();

        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);
        Logger.LogDebug("Patching...");
        Harmony.PatchAll();
        Logger.LogDebug("Finished patching!");

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    internal void CacheItemAmounts()
    {
        IsValid = true;
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
            if (
                item.scrapPersistedThroughRounds
                || item.itemProperties?.isScrap is not true
                || item.isInShipRoom
            )
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
        return $"{CachedInteriorName} {(CachedScrapAmount > 0 ? CachedScrapAmount : $"<color=#ff0000>{CachedScrapAmount}</color>")} {(CachedBeehiveAmount > 0 ? CachedBeehiveAmount : $"<color=#000000aa>{CachedBeehiveAmount}</color>")}{(CachedEggAmount > 0 ? $"+{CachedEggAmount}" : null)}{MOD_ID}";
    }

    internal static IEnumerator AutoScan(RoundManager __instance)
    {
        yield return null;
        var modInstance = Instance;
        modInstance.CacheItemAmounts();

#if !SV
        if (!__instance.IsServer)
        {
            yield return new WaitForSecondsRealtime(
                __instance.playersManager.localPlayerController.playerClientId
            );
            if (modInstance.DisableCurrentDay)
                yield break;
        }
#endif

        Chat.PrintGlobal(modInstance.GetMessage());
    }
}
