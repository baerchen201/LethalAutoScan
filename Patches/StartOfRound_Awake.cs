using HarmonyLib;

namespace LethalAutoScan.Patches;

[HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Awake))]
internal static class StartOfRound_Awake
{
    private static void Postfix()
    {
        LethalAutoScan.Instance.DisableCurrentDay = false;
    }
}
