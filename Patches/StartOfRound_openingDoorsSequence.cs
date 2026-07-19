using HarmonyLib;

namespace LethalAutoScan.Patches;

[HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.openingDoorsSequence))]
internal static class StartOfRound_openingDoorsSequence
{
    private static void Postfix(ref StartOfRound __instance)
    {
        __instance.SendDisableCurrentLobbyClientRpc();
    }
}
