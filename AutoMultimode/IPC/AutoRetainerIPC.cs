using System;
using ECommons.EzIpcManager;

namespace AutoMultimode.IPC;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value null

internal static class AutoRetainerIPC
{
    private static EzIPCDisposalToken[] disposalTokens =
        EzIPC.Init(typeof(AutoRetainerIPC), "AutoRetainer.PluginState", SafeWrapper.IPCException);

    internal static bool IsEnabled => IPCSubscriber.IsReady("AutoRetainer");

    [EzIPC]
    internal static readonly Func<bool> IsBusy;

    [EzIPC]
    internal static readonly Func<bool> AreAnyRetainersAvailableForCurrentChara;

    [EzIPC]
    internal static readonly Action EnableMultiMode;

    [EzIPC]
    internal static readonly Func<bool> GetMultiModeStatus;

    internal static void Dispose() => IPCSubscriber.DisposeAll(disposalTokens);
}

#pragma warning restore CS8618
#pragma warning restore CS0649
