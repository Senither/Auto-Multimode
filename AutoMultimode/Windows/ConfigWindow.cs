﻿using System;
using System.Diagnostics;
using System.Numerics;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace AutoMultimode.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    public ConfigWindow(AutoMultimode plugin) : base("AutoMultimode Settings###settings-window")
    {
        Flags = ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(460, 140);

        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.TextWrapped(
            "Set the time that should elapse without player activity before your character is marked as AFK and MultiMode is enabled."
        );

        var afkTimer = (int)Configuration.EnforcedAfkTimer;

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        ImGui.Text("Current value:");
        ImGui.SameLine();
        RenderCurrentAfkTimerValue(afkTimer);

        ImGui.BeginTable("afk-timer", 4);

        ImGui.TableNextColumn();
        if (ImGui.RadioButton("5 Minutes", afkTimer == 1))
        {
            SaveEnforcedAfkTimerToConfiguration(1);
        }

        ImGui.TableNextColumn();
        if (ImGui.RadioButton("10 Minutes", afkTimer == 2))
        {
            SaveEnforcedAfkTimerToConfiguration(2);
        }

        ImGui.TableNextColumn();
        if (ImGui.RadioButton("30 Minutes", afkTimer == 3))
        {
            SaveEnforcedAfkTimerToConfiguration(3);
        }

        ImGui.TableNextColumn();
        if (ImGui.RadioButton("1 Hour", afkTimer == 4))
        {
            SaveEnforcedAfkTimerToConfiguration(4);
        }

        ImGui.EndTable();
    }

    private void RenderCurrentAfkTimerValue(int value)
    {
        Vector4 color;
        string text = "OFF";

        switch (value)
        {
            case 1:
                color = ImGuiColors.ParsedGreen;
                text = "5 Minutes";
                break;

            case 2:
                color = ImGuiColors.HealerGreen;
                text = "10 Minutes";
                break;

            case 3:
                color = ImGuiColors.DalamudYellow;
                text = "30 Minutes";
                break;

            case 4:
                color = ImGuiColors.ParsedOrange;
                text = "1 Hour";
                break;

            default:
                color = ImGuiColors.DalamudOrange;
                break;
        }

        ImGui.TextColored(color, text);
    }

    private void SaveEnforcedAfkTimerToConfiguration(uint value)
    {
        Configuration.EnforcedAfkTimer = value;
        Configuration.Save();
    }
}
