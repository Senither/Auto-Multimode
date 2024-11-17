using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.System.String;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Shell;

namespace SamplePlugin;

public class FrameworkListener
{
    private uint lastPlayerStatus = 0;

    public void OnFrameworkUpdate(IFramework _)
    {
        var player = Service.ClientState.LocalPlayer;
        if (player == null) return;

        var playerStatus = player.OnlineStatus.Value.RowId;
        if (playerStatus != lastPlayerStatus && playerStatus == 17) // 17 represents the player being AFK
        {
            EnableAutoRetainerMultimode();
        }

        lastPlayerStatus = playerStatus;
    }

    protected unsafe void EnableAutoRetainerMultimode()
    {
        Service.PrintDebug("Enabling AutoRetainer Multimode due to player state being AFK");

        var command = new Utf8String("/ays multi enable");
        RaptureShellModule.Instance()->ExecuteCommandInner(&command, UIModule.Instance());
    }
}
