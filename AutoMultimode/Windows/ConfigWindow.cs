using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;

namespace AutoMultimode.Windows;

public class ConfigWindow : Window, IDisposable
{
    private AutoMultimode Plugin;

    public ConfigWindow(AutoMultimode plugin) : base("AutoMultimode Settings###settings-window")
    {
        Flags = ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(460, 165);

        Plugin = plugin;
    }

    public void Dispose() { }

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

        var afkTimer = (int)Plugin.Configuration.EnforcedAfkTimer;

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        ImGui.Text("Current value:");
        ImGui.SameLine();
        RenderCurrentAfkTimerValue(afkTimer);

        ImGui.BeginTable("afk-timer", 4);

        ImGui.TableNextColumn();
        if (ImGui.RadioButton("5 Minutes", afkTimer == 1))
            SaveEnforcedAfkTimerToConfiguration(1);

        ImGui.TableNextColumn();
        if (ImGui.RadioButton("10 Minutes", afkTimer == 2))
            SaveEnforcedAfkTimerToConfiguration(2);

        ImGui.TableNextColumn();
        if (ImGui.RadioButton("30 Minutes", afkTimer == 3))
            SaveEnforcedAfkTimerToConfiguration(3);

        ImGui.TableNextColumn();
        if (ImGui.RadioButton("1 Hour", afkTimer == 4))
            SaveEnforcedAfkTimerToConfiguration(4);

        ImGui.EndTable();
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
        ImGui.TextColored(ImGuiColors.ParsedOrange, Plugin.Version);

        ImGui.EndTabItem();
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
        Plugin.Configuration.EnforcedAfkTimer = value;
        Plugin.Configuration.Save();
    }
}
