using AutoMultimode.IPC;
using ECommons;
using ECommons.Logging;

namespace AutoMultimode;

public class CommandHandler(AutoMultimode plugin)
{
    public void HandleCommand(string[] args)
    {
        if (args.Length == 0)
        {
            plugin.ToggleMainUI();
            return;
        }

        if (args[0].EqualsIgnoreCaseAny(new List<string> { "config", "cfg", "c" }))
        {
            plugin.ToggleConfigUI();
            return;
        }

        if (args[0].EqualsIgnoreCaseAny(new List<string> { "status", "sta", "s" }))
        {
            var autoRetainerStatus = AutoRetainerIPC.IsEnabled ? "Enabled" : "Disabled";
            var pluginStatus = plugin.Configuration.Enabled ? "Enabled" : "Disabled";

            DuoLog.Information($"AutoRetainer status:                 {autoRetainerStatus}");
            DuoLog.Information($"AutoMultiMode AFK status:   {pluginStatus}");
            DuoLog.Information($"AutoMultiMode AFK time:     {plugin.Configuration.EnforcedAfkTime}m");
            return;
        }

        DuoLog.Warning("Invalid argument, example usage /automultimode <option>");
        DuoLog.Warning("Options:");
        DuoLog.Warning(" - config / cfg / c");
        DuoLog.Warning(" - status / sta / s");
    }
}
