using System.Collections;
using ChatCommandAPI.Utils;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace LethalAutoScan.Patches;

[HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SyncScrapValuesClientRpc))]
internal static class RoundManager_SyncScrapValuesClientRpc
{
    private static void Prefix(ref RoundManager __instance)
    {
        if (__instance.__rpc_exec_stage == NetworkBehaviour.__RpcExecStage.Execute)
            __instance.StartCoroutine(AutoScan(__instance));
    }

    private static IEnumerator AutoScan(RoundManager __instance)
    {
        var modInstance = LethalAutoScan.Instance;
        modInstance.CacheItemAmounts();

        if (!__instance.IsServer)
        {
            yield return new WaitForSecondsRealtime(
                __instance.playersManager.localPlayerController.playerClientId
            );
            if (modInstance.DisableCurrentDay)
                yield break;
        }

        Chat.PrintGlobal(modInstance.GetMessage());
    }
}
