using HarmonyLib;
using Unity.Netcode;

namespace LethalAutoScan.Patches;

[HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SyncScrapValuesClientRpc))]
internal static class RoundManager_SyncScrapValuesClientRpc
{
    private static void Prefix(ref RoundManager __instance)
    {
#if SV
        if (!__instance.IsServer)
            return;
#endif
        if (__instance.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Execute)
            __instance.StartCoroutine(LethalAutoScan.AutoScan(__instance));
    }
}
