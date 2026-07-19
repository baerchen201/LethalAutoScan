using ChatCommandAPI.Utils;
using HarmonyLib;
using Unity.Netcode;

namespace LethalAutoScan.Patches;

[HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.__initializeRpcs))]
internal static class StartOfRound___initializeRpcs
{
    private const uint RPC_ID = 1784503770U; // time rn idc to generate hashes

    private static void Postfix(ref StartOfRound __instance)
    {
        __instance.__registerRpc(
            RPC_ID,
            ReceiveDisableCurrentLobbyClientRpc,
            nameof(ReceiveDisableCurrentLobbyClientRpc)
        );
    }

    private static void ReceiveDisableCurrentLobbyClientRpc(
        NetworkBehaviour target,
        FastBufferReader reader,
        __RpcParams rpcParams
    )
    {
        var networkManager = target.NetworkManager;
        if (networkManager == null || !networkManager.IsListening)
            return;
        var mod = LethalAutoScan.Instance;
        if (mod.DisableCurrentLobby)
            return;
        mod.DisableCurrentLobby = true;
        Chat.PrintWarning($"[{nameof(LethalAutoScan)}] The host has disabled this mod");
    }

    internal static void SendDisableCurrentLobbyClientRpc(this StartOfRound __instance)
    {
        var networkManager = __instance.NetworkManager;
        if (
            networkManager == null
            || !networkManager.IsListening
            || networkManager is { IsServer: false, IsHost: false }
        )
            return;

        ClientRpcParams clientRpcParams = default;
        var bufferWriter = __instance.__beginSendClientRpc(
            RPC_ID,
            clientRpcParams,
            RpcDelivery.Reliable
        );
        __instance.__endSendClientRpc(
            ref bufferWriter,
            RPC_ID,
            clientRpcParams,
            RpcDelivery.Reliable
        );
    }
}
