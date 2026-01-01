using System.Numerics;
using AutoMultimode.IPC;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace AutoMultimode;

public class FrameworkListener(AutoMultimode plugin)
{
    private DateTime? lastActiveAt;
    private Vector3 lastPlayerPosition;
    private float lastPlayerRotation;

    private const float PositionThreshold = 0.1f;
    private const float RotationThreshold = 0.01f;

    public void OnFrameworkUpdate(IFramework _)
    {
        if (!plugin.Configuration.Enabled || !AutoRetainerIPC.IsEnabled)
            return;

        var player = Service.ObjectTable.LocalPlayer;
        if (player == null || AutoRetainerIPC.GetMultiModeStatus())
        {
            lastActiveAt = null;
            return;
        }

        lastActiveAt ??= DateTime.UtcNow;

        if (IsPlayerActive(player))
            lastActiveAt = DateTime.UtcNow;

        if ((DateTime.UtcNow - lastActiveAt).Value.Minutes >= plugin.Configuration.EnforcedAfkTime)
            EnableAutoRetainerMultiMode();
    }

    protected bool IsPlayerActive(IPlayerCharacter player)
    {
        var pos = player.Position;
        var rot = player.Rotation;

        // Player movement/rotations
        var dist = Vector3.Distance(pos, lastPlayerPosition);
        if (dist > PositionThreshold || Math.Abs(rot - lastPlayerRotation) > RotationThreshold)
        {
            lastPlayerPosition = pos;
            lastPlayerRotation = rot;

            return true;
        }

        // Casting/actions
        if (player.IsCasting)
            return true;

        // Other statuses (crafting, gathering, combat, etc.)
        if (player.StatusFlags is not (StatusFlags.None or StatusFlags.WeaponOut))
            return true;

        // Check if player is crafting
        try
        {
            unsafe
            {
                var atkStage = AtkStage.Instance();
                if (atkStage != null)
                {
                    var synthesisAddon = atkStage->RaptureAtkUnitManager->GetAddonByName("Synthesis", 1);
                    if (synthesisAddon != null && synthesisAddon->IsVisible)
                        return true;
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return false;
    }

    protected static void EnableAutoRetainerMultiMode()
    {
        if (AutoRetainerIPC.GetMultiModeStatus())
            return;

        if (AutoRetainerIPC.IsBusy())
        {
            Service.PrintDebug(
                "Attempted to enable AutoRetainer MultiMode due to player state being AFK but AutoRetainer is busy"
            );
            return;
        }

        Service.PrintDebug("Enabling AutoRetainer MultiMode");
        AutoRetainerIPC.EnableMultiMode();
    }
}
