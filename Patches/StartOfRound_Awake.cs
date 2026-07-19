using HarmonyLib;

namespace LethalAutoScan.Patches;

[HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.Awake))]
internal static class StartOfRound_Awake
{
    private static void Postfix()
    {
        LethalAutoScan.Instance.DisableCurrentLobby = false;
        LethalAutoScan.Instance.DisableCurrentDay = false;
    }
}
