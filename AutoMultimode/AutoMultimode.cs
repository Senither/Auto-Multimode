using System;
using System.Reflection;
using AutoMultimode.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ECommons;
using Module = ECommons.Module;

namespace AutoMultimode;

public sealed class AutoMultimode : IDalamudPlugin
{
    [PluginService]
    internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;

    [PluginService]
    internal static ICommandManager CommandManager { get; private set; } = null!;

    public readonly WindowSystem WindowSystem = new(PluginName);

    public readonly Configuration Configuration;
    public readonly CommandHandler CommandHandler;

    private const string CommandNameLong = "/automultimode";
    private const string CommandNameShort = "/amm";
    private const string PluginName = "Auto Multimode";

    public readonly string Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";

    private ConfigWindow ConfigWindow { get; init; }
    private InformationWindow InformationWindow { get; init; }
    private ActivityListener ActivityListener { get; init; }

    public AutoMultimode(IDalamudPluginInterface pluginInterface)
    {
        ECommonsMain.Init(pluginInterface, this, Module.DalamudReflector);

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        CommandHandler = new CommandHandler(this);

        Service.Initialize(pluginInterface);

        ActivityListener = new ActivityListener(this);

        ConfigWindow = new ConfigWindow(this);
        InformationWindow = new InformationWindow(this);

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(InformationWindow);

        CommandManager.AddHandler(CommandNameLong, new CommandInfo(OnCommand)
        {
            HelpMessage = "Displays some information about the plugin.",
            ShowInHelp = true,
        });

        CommandManager.AddHandler(CommandNameShort, new CommandInfo(OnCommand)
        {
            ShowInHelp = false,
        });

        Service.ChatGui.ChatMessage += ActivityListener.OnChatMessage;
        Service.Framework.Update += ActivityListener.OnFrameworkUpdate;

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        if (Configuration.ConfigVersion != Configuration.Version)
        {
            ToggleConfigUI();
            Configuration.Version = Configuration.ConfigVersion;
        }

#if DEBUG
        ConfigWindow.IsOpen = true;
#endif
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();
        InformationWindow.Dispose();

        Service.ChatGui.ChatMessage -= ActivityListener.OnChatMessage;
        Service.Framework.Update -= ActivityListener.OnFrameworkUpdate;

        ECommonsMain.Dispose();

        CommandManager.RemoveHandler(CommandNameLong);
        CommandManager.RemoveHandler(CommandNameShort);
    }

    private void OnCommand(string command, string args)
    {
        CommandHandler.HandleCommand(
            args.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    private void DrawUI() => WindowSystem.Draw();
    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => InformationWindow.Toggle();
}
