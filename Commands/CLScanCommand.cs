#if !SV
using ChatCommandAPI;
using ChatCommandAPI.Utils;

namespace LethalAutoScan.Commands;

public class CLScanCommand : Command
{
    public override string Name => SVScanCommand.NAME;
    public override string Description => SVScanCommand.DESCRIPTION;

    public override void Invoke(string args)
    {
        if (!LethalAutoScan.Instance.IsValid)
            throw new ShipIsNotLandedException();
        Chat.PrintWarning(LethalAutoScan.Instance.GetMessage());
    }
}
#endif
