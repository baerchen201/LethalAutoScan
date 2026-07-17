using ChatCommandAPI;
using ChatCommandAPI.Utils;

namespace LethalAutoScan.Commands;

public class CLScanCommand : Command
{
    internal const string NAME = "Scan";

    internal const string DESCRIPTION =
        "Repeats the last auto-scan message (may print outdated information)";

    public override string Name => NAME;
    public override string Description => DESCRIPTION;

    public override void Invoke(string args)
    {
        if (!LethalAutoScan.Instance.IsValid)
            throw new ShipIsNotLandedException();
        Chat.PrintWarning(LethalAutoScan.Instance.GetMessage());
    }
}
