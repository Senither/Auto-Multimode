using System;
using AutoMultimode.IPC;
using Dalamud.Game.Config;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.System.String;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Shell;

namespace AutoMultimode;

public class FrameworkListener
{
    private Configuration Configuration;

    private uint lastPlayerStatus = 0;
    private long enforceUpdateStateAt = 0;

    public FrameworkListener(AutoMultimode plugin)
    {
        Configuration = plugin.Configuration;
    }

    public void OnFrameworkUpdate(IFramework _)
    {
        if (!AutoRetainerIPC.IsEnabled)
        {
            return;
        }

        var player = Service.ClientState.LocalPlayer;
        if (player == null) return;

        var playerStatus = player.OnlineStatus.Value.RowId;
        if (playerStatus != lastPlayerStatus && playerStatus == 17) // 17 represents the player being AFK
        {
            EnableAutoRetainerMultimode();
        }

        lastPlayerStatus = playerStatus;

        Service.GameConfig.TryGet(SystemConfigOption.AutoAfkSwitchingTime, out uint afkTime);
        if (afkTime != 0)
        {
            return;
        }

        if (AutoRetainerIPC.IsBusy.Invoke())
        {
            // Adds the current UTC unix timestamp to the enforcement update state, with an additional five minutes.
            enforceUpdateStateAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (60 * 5);
            return;
        }

        if (enforceUpdateStateAt <= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        {
            // Sets the enforce update state to the current UTC unix timestamp + 15 minutes, so we don't spam enable
            // the option if AutoRetainer is not ready to collection retainers, and multi mode is just gonna be idle.
            enforceUpdateStateAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (60 * 15);
            Service.GameConfig.Set(SystemConfigOption.AutoAfkSwitchingTime, Configuration.EnforcedAfkTimer);
        }
    }

    protected unsafe void EnableAutoRetainerMultimode()
    {
        if (AutoRetainerIPC.IsBusy.Invoke())
        {
            Service.PrintDebug(
                "Attempted to enable AutoRetainer Multimode due to player state being AFK but AutoRetainer is busy"
            );
            return;
        }

        Service.PrintDebug("Enabling AutoRetainer Multimode due to player state being AFK");
        AutoRetainerIPC.EnableMultiMode.Invoke();
    }
}
