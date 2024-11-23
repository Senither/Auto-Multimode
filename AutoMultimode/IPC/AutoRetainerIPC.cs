using System;
using System.Collections.Generic;
using ECommons.EzIpcManager;

namespace AutoMultimode.IPC;

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

    internal static void Dispose() => IPCSubscriber.DisposeAll(disposalTokens);
}
