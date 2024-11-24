﻿using System.Reflection;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using AutoMultimode.Windows;
using Dalamud.Interface.Windowing;
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
    private FrameworkListener FrameworkListener { get; init; }

    public AutoMultimode(IDalamudPluginInterface pluginInterface)
    {
        ECommonsMain.Init(pluginInterface, this, Module.DalamudReflector);

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        CommandHandler = new CommandHandler(this);

        Service.Initialize(pluginInterface);

        FrameworkListener = new(this);

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

        Service.Framework.Update += FrameworkListener.OnFrameworkUpdate;

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();
        InformationWindow.Dispose();

        ECommonsMain.Dispose();

        CommandManager.RemoveHandler(CommandNameLong);
        CommandManager.RemoveHandler(CommandNameShort);
    }

    private void OnCommand(string command, string args)
    {
        CommandHandler.HandleCommand(args.Split(' '));
    }

    private void DrawUI() => WindowSystem.Draw();
    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => InformationWindow.Toggle();
}
