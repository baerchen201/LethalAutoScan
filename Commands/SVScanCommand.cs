using ChatCommandAPI;
using ChatCommandAPI.Utils;
using GameNetcodeStuff;

namespace LethalAutoScan.Commands;

public class SVScanCommand : ServerCommand
{
    public override string Name => CLScanCommand.NAME;
    public override string Description => CLScanCommand.DESCRIPTION;

    public override void Invoke(PlayerControllerB caller, string args)
    {
        if (!LethalAutoScan.Instance.IsValid)
            throw new ShipIsNotLandedException();
        Chat.PrintWarning(caller, LethalAutoScan.Instance.GetMessage());
    }
}
