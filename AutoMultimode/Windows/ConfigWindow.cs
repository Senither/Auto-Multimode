using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ECommons.ImGuiMethods;

namespace AutoMultimode.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly AutoMultimode plugin;

    public ConfigWindow(AutoMultimode plugin) : base("AutoMultimode Settings###settings-window")
    {
        Flags = ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(460, 190);

        this.plugin = plugin;
    }

    public void Dispose() { }

    public override void OnClose()
    {
        plugin.Configuration.Save();
    }

    public override void Draw()
    {
        if (ImGui.BeginTabBar("##ConfigTabBar"))
        {
            General();

            About();
        }

        ImGui.EndTabBar();
    }

    private void General()
    {
        if (!ImGui.BeginTabItem("General"))
            return;

        ImGui.TextWrapped(
            "Set the time that should elapse without player activity before your character is marked as AFK and MultiMode is enabled."
        );

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        var configurationEnabled = plugin.Configuration.Enabled;
        if (ImGui.Checkbox("Enabled###stuff", ref configurationEnabled))
        {
            plugin.Configuration.Enabled = configurationEnabled;
        }

        if (!configurationEnabled)
            ImGui.BeginDisabled();

        ImGui.Text("AFK Timer in minutes");

        var time = plugin.Configuration.EnforcedAfkTime;
        if (ImGuiEx.SliderInt("###afk-time-int-slider", ref time, 1, 60))
        {
            plugin.Configuration.EnforcedAfkTime = time;
        }

        if (!configurationEnabled)
            ImGui.EndDisabled();

        ImGui.EndTabItem();
    }

    private void About()
    {
        if (!ImGui.BeginTabItem("About"))
            return;

        ImGui.TextUnformatted("Author:");
        ImGui.SameLine();
        ImGui.TextColored(ImGuiColors.ParsedGold, "Senither");

        ImGui.TextUnformatted("Discord:");
        ImGui.SameLine();
        ImGui.TextColored(ImGuiColors.ParsedGold, "@senither");

        ImGui.TextUnformatted("Version:");
        ImGui.SameLine();
        ImGui.TextColored(ImGuiColors.ParsedOrange, plugin.Version);

        ImGui.EndTabItem();
    }
}
