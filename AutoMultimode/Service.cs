﻿using System.Collections;
using System.Collections.Generic;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace AutoMultimode;

public class Service
{
    public static void Initialize(IDalamudPluginInterface pluginInterface) => pluginInterface.Create<Service>();

    [PluginService]
    public static IChatGui Chat { get; private set; } = null!;

    [PluginService]
    public static IClientState ClientState { get; private set; } = null!;

    [PluginService]
    public static IFramework Framework { get; private set; } = null!;

    [PluginService]
    public static IPluginLog PluginLog { get; private set; } = null!;

    [PluginService]
    public static IGameConfig GameConfig { get; private set; } = null!;

    public static readonly Queue LogMessages = new();
    private const int MaxLogSize = 50;

    public static void PrintDebug(string message)
    {
        if (LogMessages.Count >= MaxLogSize)
        {
            LogMessages.Dequeue();
        }

        LogMessages.Enqueue(message);
        PluginLog.Debug(message);
    }

    public static void PrintError(string message)
    {
        if (LogMessages.Count >= MaxLogSize)
        {
            LogMessages.Dequeue();
        }

        LogMessages.Enqueue(message);
        PluginLog.Error(message);
    }
}
