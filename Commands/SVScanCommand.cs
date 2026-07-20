using ChatCommandAPI;
using ChatCommandAPI.Utils;
using GameNetcodeStuff;

namespace LethalAutoScan.Commands;

public class SVScanCommand : ServerCommand
{
    internal const string NAME = "Scan";

    internal const string DESCRIPTION =
        "Repeats the last auto-scan message (may print outdated information)";

    public override string Name => NAME;
    public override string Description => DESCRIPTION;

    public override void Invoke(PlayerControllerB caller, string args)
    {
        if (!LethalAutoScan.Instance.IsValid)
            throw new ShipIsNotLandedException();
        Chat.PrintWarning(caller, LethalAutoScan.Instance.GetMessage());
    }
}
