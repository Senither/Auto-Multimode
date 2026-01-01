using System;
using System.Numerics;
using AutoMultimode.IPC;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;

namespace AutoMultimode.Windows;

public class InformationWindow : Window, IDisposable
{
    private AutoMultimode Plugin;

    public InformationWindow(AutoMultimode plugin) : base(
        name: "Auto Multimode##information-window",
        flags: ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse
    )
    {
        Plugin = plugin;

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 175),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.TextWrapped(
            $"Auto MultiMode is a plugin that integrates with AutoRetainer to automatically enable the MultiMode" +
            " feature whenever your character goes AFK, making it simple and easy to keep retainer ventures going on" +
            " all your characters, even if you're AFK."
        );

        ImGui.Spacing();

        ImGui.TextWrapped("AutoRetainer is:");
        ImGui.SameLine();
        if (AutoRetainerIPC.IsEnabled)
            ImGui.TextColored(ImGuiColors.ParsedGreen, "Enabled");
        else
            ImGui.TextColored(ImGuiColors.DalamudRed, "Disabled");

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        ImGui.PushStyleColor(ImGuiCol.Button, ImGuiColors.DPSRed);
        if (ImGui.Button("Open Settings"))
            Plugin.ToggleConfigUI();

        ImGui.PopStyleColor();

        ImGui.SameLine();

        ImGui.PushStyleColor(ImGuiCol.Button, ImGuiColors.ParsedBlue);
        if (ImGui.Button("Open Repository"))
            Util.OpenLink("https://github.com/Senither/Auto-Multimode");

        ImGui.PopStyleColor();
    }
}
