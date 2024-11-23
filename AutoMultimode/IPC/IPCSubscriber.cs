using System;
using ECommons.EzIpcManager;
using ECommons.Reflection;

namespace AutoMultimode.IPC;

internal class IPCSubscriber
{
    internal static bool IsReady(string pluginName) =>
        DalamudReflector.TryGetDalamudPlugin(pluginName, out _, false, true);

    internal static void DisposeAll(EzIPCDisposalToken[] disposalTokens)
    {
        foreach (var token in disposalTokens)
        {
            try
            {
                token.Dispose();
            }
            catch (Exception ex)
            {
                Service.PrintError($"Error while unregistering IPC: {ex}");
            }
        }
    }
}
