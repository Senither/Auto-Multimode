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
    protected Configuration Configuration;

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

        HandleEnablingAutoRetainerMultiMode();
        HandleEnablingAutoAfkSwitchingTimerSetting();
    }

    protected void HandleEnablingAutoRetainerMultiMode()
    {
        var player = Service.ClientState.LocalPlayer;
        if (player == null) return;

        var playerStatus = player.OnlineStatus.Value.RowId;

        // Player status of 17 represents the player being AFK
        if (playerStatus != 17)
        {
            return;
        }

        if (AutoRetainerIPC.GetMultiModeStatus.Invoke())
        {
            return;
        }

        if (AutoRetainerIPC.IsBusy.Invoke())
        {
            Service.PrintDebug(
                "Attempted to enable AutoRetainer MultiMode due to player state being AFK but AutoRetainer is busy"
            );
            return;
        }

        Service.PrintDebug("Enabling AutoRetainer MultiMode due to player state being AFK");
        Service.GameConfig.Set(
            option: SystemConfigOption.AutoAfkSwitchingTime,
            value: 0
        );
        AutoRetainerIPC.EnableMultiMode.Invoke();
    }

    protected void HandleEnablingAutoAfkSwitchingTimerSetting()
    {
        if (!Service.ClientState.IsLoggedIn)
        {
            return;
        }

        Service.GameConfig.TryGet(SystemConfigOption.AutoAfkSwitchingTime, out uint afkTime);
        if (afkTime == Configuration.EnforcedAfkTimer)
        {
            return;
        }

        if (AutoRetainerIPC.GetMultiModeStatus.Invoke())
        {
            return;
        }

        var unixNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (AutoRetainerIPC.IsBusy.Invoke())
        {
            enforceUpdateStateAt = unixNow + 1;
            return;
        }

        if (enforceUpdateStateAt > unixNow)
        {
            return;
        }

        Service.GameConfig.Set(
            option: SystemConfigOption.AutoAfkSwitchingTime,
            value: Configuration.EnforcedAfkTimer
        );
    }
}
