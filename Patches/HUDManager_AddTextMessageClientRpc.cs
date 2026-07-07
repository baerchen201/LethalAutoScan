using HarmonyLib;

namespace LethalAutoScan.Patches;

[HarmonyPatch(typeof(HUDManager), nameof(HUDManager.AddTextMessageClientRpc))]
internal static class HUDManager_AddTextMessageClientRpc
{
    private static void Postfix(ref string chatMessage)
    {
        if (chatMessage.EndsWith(LethalAutoScan.MOD_ID))
            LethalAutoScan.Instance.DisableCurrentDay = true;
    }
}
