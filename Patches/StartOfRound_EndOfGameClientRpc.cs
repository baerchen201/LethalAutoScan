using HarmonyLib;

namespace LethalAutoScan.Patches;

[HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGameClientRpc))]
internal static class StartOfRound_EndOfGameClientRpc
{
    private static void Postfix()
    {
        LethalAutoScan.Instance.IsValid = false;
#if !SV
        LethalAutoScan.Instance.DisableCurrentDay = false;
#endif
    }
}
